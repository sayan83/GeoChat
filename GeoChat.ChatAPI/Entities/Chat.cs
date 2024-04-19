using System.ComponentModel.DataAnnotations.Schema;

namespace GeoChat.ChatAPI.Entities;

// TODO : set up indexes
public class Chat
{
    public Guid ChatId { get; set; }
    
    // Foreign key rooms
    public Guid RoomId { get; set; }

    // Foreignkey users table
    public string UserId { get; set; }

    public long SendTime { get; set; }
    public string Message { get; set; }
}
