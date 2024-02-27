using System.Collections.Concurrent;
using GeoChat.ChatAPI.Models;

namespace GeoChat.ChatAPI;

public class NotificationQueueStore
{
    private readonly ConcurrentQueue<NotificationDto> _notificationQueue;
    public static NotificationQueueStore notificationQueue = new NotificationQueueStore();
    public NotificationQueueStore() {
        _notificationQueue = new ConcurrentQueue<NotificationDto>();
    }

    public void EnqueueNotification(NotificationDto notification) {
        // TODO : Handle cases of a full queue || Or replace with channels
        _notificationQueue.Enqueue(notification);
    }

    public NotificationDto DequeueNotification() {
        NotificationDto? pushNotification;
        if(!_notificationQueue.TryDequeue(out pushNotification)) {
            // No element in queue
            return null;
        }
        return pushNotification;
    }
}
