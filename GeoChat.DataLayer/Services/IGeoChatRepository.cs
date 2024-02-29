using GeoChat.DataLayer.Entities;
using GeoChat.DataLayer.Models;

namespace GeoChat.DataLayer.Services;

public interface IGeoChatRepository
{
    Task<bool> VerifyCredentialsAsync(string userId, string password);
    Task<UserInfoDto> GetUserAsync(string userId);
    void AddNewUser(string userId, string name, string password);
    Task<bool> VerifyUserIdExistsAsync(string userId);
    Task<Room> GetRoomDetailsAsync(Guid roomId);
    Guid CreateNewroom(RoomDto roomInfo);
    void DeleteRoom(Room roomToDelete);
    void JoinRoom(Guid roomId, string userId);
    Task<bool> CheckParticipantValid(Guid roomId, string userId);
    void LeaveRoom(Guid roomId, string userId);
    Task<IEnumerable<Room>> ShowRoomsAsync();
    Task<List<string>> GetRoomMembersAsync(Guid RoomId);
    void AddNewMessage(string UserId, Guid RoomId, string Message);
    Task<IEnumerable<ChatDto>> FetchMessagesAsync(Guid RoomId, int StartRange, int noOfMessages);
    Task<bool> SaveChangesAsync();
}
