using GeoChat.DataLayer.Models;

namespace GeoChat.DataLayer.Services;

public interface IGeoChatRepository
{
    Task<bool> VerifyCredentialsAsync(string userId, string password);
    Task<UserInfoDto> GetUserAsync(string userId);
    void AddNewUser(string userId, string name, string password);
    Task<bool> VerifyUserIdExists(string userId);
    void CreateNewroom(RoomDto roomInfo);
    Task<bool> SaveChangesAsync();
}
