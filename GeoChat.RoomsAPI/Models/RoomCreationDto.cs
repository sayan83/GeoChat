namespace GeoChat.RoomsAPI.Models;

public class RoomCreationDto
{
    public string RoomName { get; set; } = String.Empty;
    public string? Description { get; set; }
    public int Range { get; set; }     // Define validation rules for this property
    public long Latitude { get; set; }   // TODO : Try combining both lat long with LocInfoDto
    public long Longitude { get; set; }
    public string CreatedBy { get; set; } = String.Empty;   // Replace later with claims from token
}
