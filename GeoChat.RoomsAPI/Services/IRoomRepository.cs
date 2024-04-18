using GeoChat.RoomsAPI.Models;

namespace GeoChat.RoomsAPI.Services;

public interface IRoomRepository
{
    Task<RoomDto> GetRoomDetailsAsync(Guid roomId);
    Guid CreateNewroom(RoomCreationDto roomInfo);
    void DeleteRoom(RoomDto roomToDelete);
    void JoinRoom(Guid roomId, string userId);
    Task<bool> CheckParticipantValid(Guid roomId, string userId);
    void LeaveRoom(Guid roomId, string userId);
    Task<IEnumerable<RoomDto>> ShowRoomsAsync();
    Task<List<string>> GetRoomMembersAsync(Guid roomId);
    Task<bool> SaveChangesAsync();
}
