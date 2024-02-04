using System.ComponentModel.DataAnnotations.Schema;

namespace GeoChat.DataLayer.Entities;

// TODO : set up indexes
public class Chat
{
    public Guid ChatId { get; set; }
    
    [ForeignKey("RoomId")]
    public Room RoomRef { get; set; }
    public Guid RoomId { get; set; }

    [ForeignKey("UserId")]
    public User SendBy { get; set; }
    public string UserId { get; set; }

    public long SendTime { get; set; }
    public string Message { get; set; }
}
