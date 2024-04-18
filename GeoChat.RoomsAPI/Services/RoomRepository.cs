using GeoChat.RoomsAPI.DbContexts;
using GeoChat.RoomsAPI.Entities;
using GeoChat.RoomsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GeoChat.RoomsAPI.Services;

public class RoomRepository : IRoomRepository
{
    private readonly RoomDBContext _context;
    public RoomRepository(RoomDBContext context) {
        _context = context;
    }

    public Guid CreateNewroom(RoomCreationDto roomInfo)
    {
        Room newRoom = new Room(roomInfo.RoomName) {
            RoomId = Guid.NewGuid(),
            Description = roomInfo.Description,
            UserId = roomInfo.CreatedBy,
            Latitude = roomInfo.LocInfo.Latitude,
            Longitude = roomInfo.LocInfo.Longitude,
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

    public async Task<RoomDto> GetRoomDetailsAsync(Guid roomId)
    {
        Room? roomInfo = await _context.Rooms.Where(r => r.RoomId == roomId).FirstOrDefaultAsync();
        if(roomInfo == null) {
            return null;
        }
        RoomDto roomDetails = new RoomDto() {
            RoomId = roomInfo.RoomId,
            Description = roomInfo.Description,
            RoomName = roomInfo.RoomName,
            CreatedBy = roomInfo.UserId,
            Range = roomInfo.Range,
            LocInfo = new LocationInfoDto() {
                Latitude = roomInfo.Latitude, 
                Longitude = roomInfo.Longitude
            }
        };

       return roomDetails;
    }

    public async void DeleteRoom(RoomDto roomToDelete)
    {
        Room roomToDeleteEntity = new Room(roomToDelete.RoomName) {
            RoomId = roomToDelete.RoomId,
            Description = roomToDelete.Description,
            UserId = roomToDelete.CreatedBy,
            Range = roomToDelete.Range,
            Latitude = roomToDelete.LocInfo.Latitude,
            Longitude = roomToDelete.LocInfo.Longitude
        };
        _context.Rooms.Remove(roomToDeleteEntity);
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
    
    public async Task<bool> CheckParticipantValid(Guid roomId, string userId)
    {
        RoomParticipant? participant = await _context.Participants.Where(p => p.RoomId == roomId && p.UserId == userId).FirstOrDefaultAsync();
        if(participant == null) {
            return false;
        }
        return true;
    }
    public void LeaveRoom(Guid roomId, string userId)
    {
        RoomParticipant participant = new RoomParticipant {
            RoomId = roomId,
            UserId = userId
        };
        _context.Participants.Remove(participant);
    }

    public async Task<IEnumerable<RoomDto>> ShowRoomsAsync()
    {
        List<Room> rooms = await _context.Rooms.ToListAsync();
        if(rooms == null) {
            return null;
        }
        List<RoomDto> roomsInfo = new List<RoomDto>(rooms.Count);
        foreach(Room room in rooms) {
            roomsInfo.Add(new RoomDto() {
                RoomId = room.RoomId,
                Description = room.Description,
                RoomName = room.RoomName,
                CreatedBy = room.UserId,
                Range = room.Range,
                LocInfo = new LocationInfoDto() {
                    Latitude = room.Latitude,
                    Longitude = room.Longitude
                }
            });
        }
        return roomsInfo;
    }
    public async Task<List<string>> GetRoomMembersAsync(Guid RoomId)
    {
        List<string> userIds = await _context.Participants.Where(r => r.RoomId == RoomId).Select(r => r.UserId).ToListAsync();
        return userIds;
    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync() > 0); 
    }

}
