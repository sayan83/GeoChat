using System.ComponentModel.DataAnnotations;

namespace GeoChat.RoomsAPI.Entities;

public class Room
{
    [Key]
    public Guid RoomId { get; set; }
    public string RoomName { get; set; }
    public string? Description { get; set; }
    // Foreign key in users table
    public string UserId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int Range { get; set; }

    public Room(string roomName) {
        RoomName = roomName;
    }
}
