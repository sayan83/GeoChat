using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoChat.DataLayer.Entities;

public class Room
{
    [Key]
    public Guid RoomId { get; set; }
    public string RoomName { get; set; }
    public string? Description { get; set; }
    [ForeignKey("UserId")]
    public User CreatedBy { get; set; }
    public string UserId { get; set; }

    public Room(string roomName) {
        RoomName = roomName;
    }
}
