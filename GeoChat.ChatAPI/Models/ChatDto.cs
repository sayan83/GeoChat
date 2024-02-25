using System.ComponentModel.DataAnnotations;

namespace GeoChat.ChatAPI.Models;

public class ChatDto
{
    public string UserId { get; set; } = String.Empty;
    public long Timestamp { get; set; }
    public string Message { get; set; } = String.Empty;
}
