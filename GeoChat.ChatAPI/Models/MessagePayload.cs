namespace GeoChat.ChatAPI.Models;

public class MessagePayload
{
    public string Type { get; set; } = String.Empty;
    public Guid RoomId { get; set; }
    public int MsgStartRange { get; set; }
    public string? Message { get; set; }
}
