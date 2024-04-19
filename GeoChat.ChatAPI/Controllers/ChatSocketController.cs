using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using GeoChat.ChatAPI.Filters;
using GeoChat.ChatAPI.Models;
using GeoChat.ChatAPI.Services;
using GeoChat.RoomsAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using static System.Net.Mime.MediaTypeNames;

namespace GeoChat.ChatAPI.Controllers;
[ApiController]
[Route("/api/chat")]
[TypeFilter<ChatSocketExceptionFilterAttribute>]
public class ChatSocketController : ControllerBase
{
    private readonly ILogger<ChatSocketController> _logger;
    private readonly IChatRepository _chatRepository; 
    private readonly IHttpClientFactory _httpClientFactory;
    // private readonly INotificationService _notificationService;
    public ChatSocketController(ILogger<ChatSocketController> logger, IChatRepository chatRepository, IHttpClientFactory httpClientFactory) {
        _logger = logger;
        _chatRepository = chatRepository; 
        _httpClientFactory = httpClientFactory;
        // _notificationService = notificationService;
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("initws/{userId}")]
    public async Task InitiateSocketConnection(string userId) {
        if (HttpContext.WebSockets.IsWebSocketRequest) {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            // TODO : Later use proper adapter like redis for scaling
            // TODO : Add check if username already exists in dictionary, then replace it. 
            ConnectionListStore.activeConnections.ActiveConnections.TryAdd(userId,webSocket);
            _logger.LogInformation("InitiateSocketConnection : Socket connection active. Opening receive channel");
            await ReceiveMsg(webSocket,userId);
        }
        else {
            _logger.LogInformation("InitiateSocketConnection : Invalid Websocketrequest");
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    private async Task<bool> ValidateParticipation(Guid roomId, string userId) {
        // var roomServiceRequestMsg = new HttpRequestMessage(
        //     HttpMethod.Post,
        //     "http://localhost")
        //     {
        //         Headers = {
        //             { HeaderNames.ContentType, "application/json" }
        //         }
        //     };
        
        ParticipantDto participant = new ParticipantDto() {
            RoomId = roomId,
            UserId = userId
        };
        
        var roomServiceClient = _httpClientFactory.CreateClient("RoomsService");
        var validParticipationRespMsg = await roomServiceClient.PostAsync("checkparticipation",new StringContent(
            JsonSerializer.Serialize(participant),
            Encoding.UTF8,
            Application.Json
        ));

        if(validParticipationRespMsg.IsSuccessStatusCode) {
            return true;
        }
        return false;
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
            try {
                string payloadString = System.Text.Encoding.UTF8.GetString(buffer,0,receiveResult.Count);
                MessagePayload? receivedPayloadJSON = JsonSerializer.Deserialize<MessagePayload>(payloadString);
                ResponseMessagePayload respMsg = new ResponseMessagePayload();
                if(receivedPayloadJSON == null || receivedPayloadJSON.RoomId == Guid.Empty) {
                    _logger.LogError("Invalid Message type in websocket from {0}",userId);
                    respMsg.Type = "INVALID_MSG_TYPE";
                }
                else if(await ValidateParticipation(receivedPayloadJSON.RoomId,userId)){
                    switch (receivedPayloadJSON.Type) {
                        case "newmessage" : 
                            _chatRepository.AddNewMessage(userId,receivedPayloadJSON.RoomId,receivedPayloadJSON.Message);
                            // bool saved = await _chatRepository.SaveChangesAsync();
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
                            IEnumerable<ChatDto> chats = await _chatRepository.FetchMessagesAsync(receivedPayloadJSON.RoomId,receivedPayloadJSON.MsgStartRange,10);
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

        ConnectionListStore.activeConnections.ActiveConnections.TryRemove(userId,out WebSocket? value);
        await webSocket.CloseAsync(
            receiveResult.CloseStatus.Value,
            receiveResult.CloseStatusDescription,
            CancellationToken.None);
        
    }
}
