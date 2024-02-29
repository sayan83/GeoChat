using GeoChat.DataLayer.Entities;
using GeoChat.DataLayer.Models;
using GeoChat.DataLayer.Services;
using GeoChat.RoomsAPI.Filters;
using GeoChat.RoomsAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace GeoChat.RoomsAPI.Controllers;

// TODO : Add versioning info 
[ApiController]
[Route("/api/rooms")]
[TypeFilter<RoomExceptionFilterAttribute>]
public class RoomsController : ControllerBase
{
    private readonly ILogger<RoomsController> _logger;
    private readonly IGeoChatRepository _geoChatRepository;
    public RoomsController(ILogger<RoomsController> logger, IGeoChatRepository geoChatRepository) {
        _logger = logger;
        _geoChatRepository = geoChatRepository;
    }

    [HttpGet("{roomId}")]
    public async Task<ActionResult<Models.RoomDto>> GetRoomInfo(Guid roomId) {
        DataLayer.Entities.Room roomInfo = await _geoChatRepository.GetRoomDetailsAsync(roomId);
        if(roomInfo == null) {
            return NotFound();
        }

        return Ok(new Models.RoomDto{
            RoomId = roomInfo.RoomId,
            RoomName = roomInfo.RoomName,
            Description = roomInfo.Description,
            LocInfo = new Models.LocationInfoDto {
                Latitude = roomInfo.Latitude,
                Longitude = roomInfo.Longitude
            },
            Range = roomInfo.Range,
            CreatedBy = roomInfo.UserId
        });
    }
    
    [HttpPost("search")]
    public async Task<ActionResult<IEnumerable<DataLayer.Models.RoomDto>>> SearchRooms(DataLayer.Models.LocationInfoDto locInfo) {
        // TODO : Calculate lat/long distances. For now just showing all rooms
        // TODO : WARNING do not use datalayer entity object(Room) here
        IEnumerable<Room> roomsAvailable = await _geoChatRepository.ShowRoomsAsync();
        if(roomsAvailable == null) {
            return NotFound();
        }
        return Ok(roomsAvailable);
    }


    [HttpPost("create")]
    public async Task<ActionResult<Models.RoomDto>> CreateRoom(RoomCreationDto roomInfo) {
        Guid roomId = _geoChatRepository.CreateNewroom(new DataLayer.Models.RoomDto {
            RoomName = roomInfo.RoomName,
            Description = roomInfo.Description,
            CreatedBy = roomInfo.CreatedBy,
            Range = roomInfo.Range,
            BaseLocation = new DataLayer.Models.LocationInfoDto {
                Latitude = roomInfo.LocInfo.Latitude,
                Longitude = roomInfo.LocInfo.Longitude
            }
        });
        bool saved = await _geoChatRepository.SaveChangesAsync();
        if(!saved) {
            _logger.LogError("CreateRoom : Room adding to DB failed");
            return StatusCode(500);
        }

        _logger.LogInformation($"CreateRoom : New room created {roomInfo.RoomName}");
        return CreatedAtAction("GetRoomInfo", 
                                new { roomId },
                                new Models.RoomDto {
                                    RoomId = roomId,
                                    RoomName = roomInfo.RoomName,
                                    Description = roomInfo.Description,
                                    CreatedBy = roomInfo.CreatedBy,
                                    Range = roomInfo.Range,
                                    LocInfo = new Models.LocationInfoDto {
                                        Latitude = roomInfo.LocInfo.Latitude,
                                        Longitude = roomInfo.LocInfo.Longitude
                                }});
        
    }


    [HttpPost("join/{roomId}/{userId}")]
    public async Task<ActionResult> JoinRoom(Guid roomId,string userId, Models.LocationInfoDto locInfo) {
        // TODO : Check if room and user exists
        // TODO : Check if location info matches within the range of room
        _geoChatRepository.JoinRoom(roomId,userId);
        bool saved = await _geoChatRepository.SaveChangesAsync();
        if(!saved) {
            _logger.LogError("JoinRoom : Adding participant to DB failed");
            return StatusCode(500);
        }
        return NoContent(); 
    }

    [HttpPost("leave/{roomId}/user/{userId}")]
    public async Task<ActionResult> LeaveRoom(Guid roomId, string userId) {
        bool participantValid = await _geoChatRepository.CheckParticipantValid(roomId,userId); 
        if(!participantValid) {
            _logger.LogInformation("LeaveRoom : User not part of room");
            return BadRequest();
        }

        _geoChatRepository.LeaveRoom(roomId,userId);
        bool saved = await _geoChatRepository.SaveChangesAsync();
        if(!saved) {
            _logger.LogError("LeaveRoom : Error saving changes to DB");
            return StatusCode(500);
        }
        return NoContent();
    }

    [HttpDelete("delete/{roomId}")]
    public async Task<ActionResult> DeleteRoom(Guid roomId) {
        // TODO : Make sure only creator can delete room
        // TODO : Using DAL layer entity object(Room) outside. Bad practice
        DataLayer.Entities.Room roomToDelete = await _geoChatRepository.GetRoomDetailsAsync(roomId);
        if(roomToDelete == null) {
            return NotFound();
        }

        _geoChatRepository.DeleteRoom(roomToDelete);
        bool saved = await _geoChatRepository.SaveChangesAsync();
        if(!saved) {
            _logger.LogError("DeleteRoom : Failed to delete room from DB");
            return StatusCode(500);
        }

        _logger.LogInformation($"DeleteRoom : Deleted room {roomId} from db");
        return NoContent();
    }
}
