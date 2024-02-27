namespace GeoChat.ChatAPI.Models;

public class NotificationDto
{
    public Guid RoomId {get; set;}
    public string From {get; set;} = String.Empty;
    public string Message {get; set;} = String.Empty;
}
