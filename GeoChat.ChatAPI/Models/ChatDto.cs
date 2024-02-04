using System.ComponentModel.DataAnnotations;

namespace GeoChat.ChatAPI.Models;

public class ChatDto
{
    public Guid RoomId { get; set; }
    public string UserId { get; set; }
    public string Timestamp { get; set; }
    public string Message { get; set; }
}
