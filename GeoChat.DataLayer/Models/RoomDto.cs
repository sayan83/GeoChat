namespace GeoChat.DataLayer.Models;

public class RoomDto
{
    public Guid RoomId { get; set; }
    public string RoomName { get; set; } = String.Empty;
    public string? Description { get; set; }
    public int Range { get; set; }  // Range of room in meters
    public string CreatedBy { get; set; } = String.Empty;
}
