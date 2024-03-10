using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using GeoChat.ChatAPI.Filters;
using GeoChat.ChatAPI.Models;
using GeoChat.ChatAPI.Services;
using GeoChat.DataLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeoChat.ChatAPI.Controllers;
[ApiController]
[Route("/api/chat")]
[TypeFilter<ChatSocketExceptionFilterAttribute>]
public class ChatSocketController : ControllerBase
{
    private readonly ILogger<ChatSocketController> _logger;
    private readonly IGeoChatRepository _geoChatRepository;
    // private readonly INotificationService _notificationService;
    public ChatSocketController(ILogger<ChatSocketController> logger, IGeoChatRepository geoChatRepository) {
        _logger = logger;
        _geoChatRepository = geoChatRepository;
        // _notificationService = notificationService;
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("initws/{userId}")]
    public async Task InitiateSocketConnection(string userId) {
        if (HttpContext.WebSockets.IsWebSocketRequest) {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            // TODO : Later use proper adapter like redis for scaling
            // TODO : Add check if username already exists in dictionary, then replace it. 
            ConnectionListStore.activeConnections.ActiveConnections.Add(userId,webSocket);
            _logger.LogInformation("InitiateSocketConnection : Socket connection active. Opening receive channel");
            await ReceiveMsg(webSocket,userId);
        }
        else {
            _logger.LogInformation("InitiateSocketConnection : Invalid Websocketrequest");
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    private async Task ReceiveMsg(WebSocket webSocket, string userId)
    {
        // TODO : Maybe reconsider using this logic as a service layer instead of a controller
        var buffer = new byte[1024 * 4];
        byte[] respBuffer;
        string responseJSONstring = String.Empty;
        var receiveResult = await webSocket.ReceiveAsync(
            new ArraySegment<byte>(buffer), CancellationToken.None);
        

        while (!receiveResult.CloseStatus.HasValue)
        {
            // TODO : Add a global check if roomid is valid for this user
            try {
                string payloadString = System.Text.Encoding.UTF8.GetString(buffer,0,receiveResult.Count);
                MessagePayload? receivedPayloadJSON = JsonSerializer.Deserialize<MessagePayload>(payloadString);
                ResponseMessagePayload respMsg = new ResponseMessagePayload();
                if(receivedPayloadJSON == null || receivedPayloadJSON.RoomId == Guid.Empty) {
                    _logger.LogError("Invalid Message type in websocket from {0}",userId);
                    respMsg.Type = "INVALID_MSG_TYPE";
                }
                else if(await _geoChatRepository.CheckParticipantValid(receivedPayloadJSON.RoomId,userId)){
                    switch (receivedPayloadJSON.Type) {
                        case "newmessage" : 
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
                        break;
                        case "loadmessage" : 
                            // TODO : Try to perform join b/w users and chat
                            IEnumerable<GeoChat.DataLayer.Models.ChatDto> chats = await _geoChatRepository.FetchMessagesAsync(receivedPayloadJSON.RoomId,receivedPayloadJSON.MsgStartRange,10);
                            respMsg.Type = "CHATS";
                            respMsg.Chats = chats;
                        break;    
                        default: 
                            _logger.LogInformation("Invalid Message type received {0}",payloadString);
                            respMsg.Type = "INVALID_REQUEST";
                        break;
                    }
                }
                else {
                    _logger.LogWarning("Invalid user for this room - {0}, {1}",userId, receivedPayloadJSON.RoomId);
                    respMsg.Type = "INVALID_USER";
                }
                responseJSONstring = JsonSerializer.Serialize<ResponseMessagePayload>(respMsg); 
            } catch (Exception e) {
                _logger.LogError("Exception encountered while decoding message payload - {0}", e.ToString());
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
