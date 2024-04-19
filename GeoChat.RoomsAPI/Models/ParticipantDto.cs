namespace GeoChat.RoomsAPI;

public class ParticipantDto
{
    public string UserId { get; set; } = String.Empty;
    public Guid RoomId { get; set; }
}
