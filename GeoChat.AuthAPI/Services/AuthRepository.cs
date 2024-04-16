using GeoChat.AuthAPI.DbContexts;
using GeoChat.AuthAPI.Entities;
using GeoChat.AuthAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GeoChat.AuthAPI.Services;

public class AuthRepository : IAuthRepository
{
    private readonly AuthDBContext _context;
    public AuthRepository(AuthDBContext context) {
        _context = context;
    }

    public async Task<UserDto> GetUserAsync(string userId)
    {
        User? user = await _context.Users.Where(u => u.UserId == userId).FirstOrDefaultAsync();
        UserDto? userInfo = null;
        if(user == null) {
            return null;
        }
        userInfo = new UserDto {
            UserId = user.UserId,
            Name = user.Name
        };

        return userInfo;
    }
    public async Task<bool> VerifyUserIdExistsAsync(string userId)
    {
        User? exists = await _context.Users.Where(u => u.UserId == userId).FirstOrDefaultAsync();
        if(exists == null) 
            return false;
        
        return true;
    }

    public void AddNewUser(string userId, string name, string password)
    {
        User newUser = new User(userId,name,password); 
        _context.Users.Add(newUser);
    }

    public async Task<bool> VerifyCredentialsAsync(string userId, string password)
    {
        User? resp = await _context.Users.Where(u => u.UserId == userId && u.Password == password).FirstOrDefaultAsync();
        if(resp == null)
            return false;
        
        return true;
    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync() > 0); 
    }
}
