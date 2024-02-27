using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using GeoChat.ChatAPI.Models;
using GeoChat.ChatAPI.Services;
using GeoChat.DataLayer.Models;
using GeoChat.DataLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeoChat.ChatAPI.Controllers;

[Route("/api/chat")]
public class ChatSocketController : ControllerBase
{
    private readonly IGeoChatRepository _geoChatRepository;
    private readonly INotificationService _notificationService;
    public ChatSocketController(IGeoChatRepository geoChatRepository,INotificationService notificationService) {
        _geoChatRepository = geoChatRepository;
        _notificationService = notificationService;
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("initws/{userId}")]
    public async Task InitiateSocketConnection(string userId) {
        if (HttpContext.WebSockets.IsWebSocketRequest) {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            // Add connection to ConnectionList
            // TODO : Later use proper adapter like redis for scaling
            // TODO : Add check if username already exists in dictionary, then replace it. 
            ConnectionListStore.activeConnections.ActiveConnections.Add(userId,webSocket);
            await ReceiveMsg(webSocket,userId);
        }
        else {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    private async Task ReceiveMsg(WebSocket webSocket, string userId)
    {
        var buffer = new byte[1024 * 4];
        byte[] respBuffer;
        string responseJSONstring = String.Empty;
        var receiveResult = await webSocket.ReceiveAsync(
            new ArraySegment<byte>(buffer), CancellationToken.None);
        

        while (!receiveResult.CloseStatus.HasValue)
        {
            try {
                string payloadString = System.Text.Encoding.UTF8.GetString(buffer,0,receiveResult.Count);
                //Console.WriteLine(jsonString);
                MessagePayload? receivedPayloadJSON = JsonSerializer.Deserialize<MessagePayload>(payloadString);
                ResponseMessagePayload respMsg = new ResponseMessagePayload();
                if(receivedPayloadJSON.Type == "newmessage") {
                    _geoChatRepository.AddNewMessage(userId,receivedPayloadJSON.RoomId,receivedPayloadJSON.Message);
                    // bool saved = await _geoChatRepository.SaveChangesAsync();
                    // if(!saved) {
                        // TODO : Check how to handle status codes with WebSockets
                    // }
                    // TODO : Trigger the notification service asynchronously
                    NotificationDto sendNotification = new NotificationDto {
                        From = userId,
                        Message = receivedPayloadJSON.Message,
                        RoomId = receivedPayloadJSON.RoomId
                    };
                    NotificationQueueStore.notificationQueue.EnqueueNotification(sendNotification);
                    respMsg.Type = "SEND_ACK";  
                }
                else if(receivedPayloadJSON.Type == "loadmessage") {
                    // TODO : Try to perform join b/w users and chat
                    IEnumerable<GeoChat.DataLayer.Models.ChatDto> chats = await _geoChatRepository.FetchMessagesAsync(receivedPayloadJSON.RoomId,receivedPayloadJSON.MsgStartRange,10);
                    respMsg.Type = "CHATS";
                    respMsg.Chats = chats;
                }
                responseJSONstring = JsonSerializer.Serialize<ResponseMessagePayload>(respMsg); 
            } catch (Exception e) {
                Console.WriteLine(e);
            }
            
            respBuffer = Encoding.UTF8.GetBytes(responseJSONstring);
            await webSocket.SendAsync(
                new ArraySegment<byte>(respBuffer),
                receiveResult.MessageType,
                receiveResult.EndOfMessage,
                CancellationToken.None);

            receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);
        }

        ConnectionListStore.activeConnections.ActiveConnections.Remove(userId);
        await webSocket.CloseAsync(
            receiveResult.CloseStatus.Value,
            receiveResult.CloseStatusDescription,
            CancellationToken.None);
        
    }
}
