
using GeoChat.DataLayer.DbContexts;
using GeoChat.DataLayer.Entities;
using GeoChat.DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace GeoChat.DataLayer.Services;

public class GeoChatRepository : IGeoChatRepository
{
    private readonly GeoChatDBContext _context;
    // public GeoChatRepository(GeoChatDBContext context) {
    //     _context = context;
    // }
    public async Task<UserInfoDto> GetUserAsync(string userId)
    {
        User? user = await _context.Users.Where(u => u.UserId == userId).FirstOrDefaultAsync();
        UserInfoDto? userInfo = null;
        if(user == null) {
            return null;
        }
        userInfo = new UserInfoDto {
            UserId = user.UserId,
            Name = user.Name
        };

        return userInfo;
    }
    public GeoChatRepository() {
        _context = new GeoChatDBContext();
    }
    public async Task<bool> VerifyUserIdExists(string userId)
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

    public void CreateNewroom(RoomDto roomInfo)
    {
        Room newRoom = new Room(roomInfo.RoomName) {
            RoomId = Guid.NewGuid(),
            Description = roomInfo.Description,
            UserId = roomInfo.CreatedBy
        };
        // _context.Rooms.Add();
    }
}
