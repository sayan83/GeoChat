
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using GeoChat.ChatAPI.Models;
using GeoChat.DataLayer.Services;

namespace GeoChat.ChatAPI.Services;

public class NotificationWorker : BackgroundService
{
    private readonly ILogger<NotificationWorker> _logger;
    private readonly IGeoChatRepository _geoChatRepository;
    public NotificationWorker(ILogger<NotificationWorker> logger) {
        _logger = logger;
        _geoChatRepository = new GeoChatRepository();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker Thread : Starting listening to queue ...");
        await BackgroundNotificationProcessing(stoppingToken);
    }

    private async Task BackgroundNotificationProcessing(CancellationToken stoppingToken) {
        while(!stoppingToken.IsCancellationRequested) {
            NotificationDto? notificationMsg = NotificationQueueStore.notificationQueue.DequeueNotification();
            if(notificationMsg == null) {
                await Task.Delay(1000);
                continue;
            }
            // TODO : Ideally should hand over the below work to another worker thread
            //        and continue monitoring the queue again.
            string notificationMsgString = JsonSerializer.Serialize<NotificationDto>(notificationMsg);
            byte[] notificationByte = Encoding.UTF8.GetBytes(notificationMsgString);
            Guid roomId = notificationMsg.RoomId;
            List<string> userIds = await _geoChatRepository.GetRoomMembersAsync(roomId);
            List<Task> pushNotificationTasks = new List<Task>();
            foreach (string userId in userIds) {
                if(userId != notificationMsg.From &&
                    ConnectionListStore.activeConnections.ActiveConnections.ContainsKey(userId)) {
                    pushNotificationTasks.Add(ConnectionListStore.activeConnections
                            .ActiveConnections[userId].SendAsync(
                                new ArraySegment<byte>(notificationByte),
                                WebSocketMessageType.Text,
                                true,
                                CancellationToken.None
                            ));
                }
            }
            await Task.WhenAll(pushNotificationTasks);
        }
    }
}