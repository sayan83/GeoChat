
using System.Security.Cryptography.X509Certificates;
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
    public GeoChatRepository() {
        _context = new GeoChatDBContext();
    }
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

    public Guid CreateNewroom(RoomDto roomInfo)
    {
        Room newRoom = new Room(roomInfo.RoomName) {
            RoomId = Guid.NewGuid(),
            Description = roomInfo.Description,
            UserId = roomInfo.CreatedBy,
            Latitude = roomInfo.BaseLocation.Latitude,
            Longitude = roomInfo.BaseLocation.Longitude,
            Range = roomInfo.Range
        };
        _context.Rooms.Add(newRoom);
        _context.Participants.Add(new RoomParticipant {
            RoomId = newRoom.RoomId,
            UserId = newRoom.UserId
        });
        //TODO Make sure both are saved while calling savechangesasync()
        return newRoom.RoomId;
    }

    public async Task<Room> GetRoomDetailsAsync(Guid roomId)
    {
        Room? roomInfo = await _context.Rooms.Where(r => r.RoomId == roomId).FirstOrDefaultAsync();
        if(roomInfo == null) {
            return null;
        }

       return roomInfo;
    }

    public async void DeleteRoom(Room roomToDelete)
    {
        _context.Rooms.Remove(roomToDelete);
        // TODO: Remove all participants from RoomParticipants table
        _context.Participants.RemoveRange(await _context.Participants.Where(p => p.RoomId == roomToDelete.RoomId).ToListAsync());
    }

    public void JoinRoom(Guid roomId, string userId)
    {
        RoomParticipant participant = new RoomParticipant {
            RoomId = roomId,
            UserId = userId
        };
        _context.Participants.Add(participant);
    }

    public async Task<IEnumerable<Room>> ShowRoomsAsync()
    {
        List<Room> rooms = await _context.Rooms.ToListAsync();
        if(rooms == null) {
            return null;
        }
        return rooms;
    }

    public void AddNewMessage(string UserId, Guid RoomId, string Message)
    {
        Chat newChat = new Chat() {
            ChatId = Guid.NewGuid(),
            UserId = UserId,
            RoomId = RoomId,
            Message = Message,
            SendTime = DateTimeOffset.Now.ToUnixTimeSeconds()
        };
        _context.Chats.Add(newChat);
    }

    public async Task<IEnumerable<ChatDto>> FetchMessagesAsync(Guid roomId, int StartRange, int noOfMessages)
    {
        List<Chat> messages = await _context.Chats.Where(c => c.RoomId == roomId)
                                        .OrderByDescending(c => c.SendTime)
                                        .Skip(StartRange)
                                        .Take(noOfMessages).
                                        ToListAsync();

        List<ChatDto> chats = new List<ChatDto>();
        foreach(Chat message in messages) {
            chats.Add(new ChatDto{
                Username = message.UserId,
                Message = message.Message,
                Timestamp = message.SendTime
            });
        }
        return chats;
    }
}
