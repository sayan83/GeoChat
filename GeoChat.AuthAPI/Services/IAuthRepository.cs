using GeoChat.AuthAPI.Models;

namespace GeoChat.AuthAPI.Services;

public interface IAuthRepository
{
    Task<bool> VerifyCredentialsAsync(string userId, string password);
    Task<UserDto> GetUserAsync(string userId);
    void AddNewUser(string userId, string name, string password);
    Task<bool> VerifyUserIdExistsAsync(string userId);
    Task<bool> SaveChangesAsync();
}
