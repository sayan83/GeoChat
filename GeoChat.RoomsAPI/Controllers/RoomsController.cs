using GeoChat.RoomsAPI.Filters;
using GeoChat.RoomsAPI.Models;
using GeoChat.RoomsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeoChat.RoomsAPI.Controllers;

// TODO : Add versioning info 
[ApiController]
[Route("/api/rooms")]
[TypeFilter<RoomExceptionFilterAttribute>]
public class RoomsController : ControllerBase
{
    private readonly ILogger<RoomsController> _logger;
    private readonly IRoomRepository _roomRepository; 
    public RoomsController(ILogger<RoomsController> logger, IRoomRepository roomRepository) {
        _logger = logger;
        _roomRepository = roomRepository;
    }

    [HttpGet("{roomId}")]
    public async Task<ActionResult<RoomDto>> GetRoomInfo(Guid roomId) {
        RoomDto roomInfo = await _roomRepository.GetRoomDetailsAsync(roomId);
        if(roomInfo == null) {
            return NotFound();
        }

        return Ok(roomInfo);
    }
    
    [HttpPost("search")]
    public async Task<ActionResult<IEnumerable<RoomDto>>> SearchRooms(LocationInfoDto locInfo) {
        // TODO : Calculate lat/long distances. For now just showing all rooms
        // TODO : WARNING do not use datalayer entity object(Room) here
        IEnumerable<RoomDto> roomsAvailable = await _roomRepository.ShowRoomsAsync();
        if(roomsAvailable == null) {
            return NotFound();
        }
        return Ok(roomsAvailable);
    }


    [HttpPost("create")]
    public async Task<ActionResult<RoomDto>> CreateRoom(RoomCreationDto roomInfo) {
        Guid roomId = _roomRepository.CreateNewroom(roomInfo);
        bool saved = await _roomRepository.SaveChangesAsync();
        if(!saved) {
            _logger.LogError("CreateRoom : Room adding to DB failed");
            return StatusCode(500);
        }

        _logger.LogInformation($"CreateRoom : New room created {roomInfo.RoomName}");
        return CreatedAtAction("GetRoomInfo", 
                                new { roomId },
                                new RoomDto {
                                    RoomId = roomId,
                                    RoomName = roomInfo.RoomName,
                                    Description = roomInfo.Description,
                                    CreatedBy = roomInfo.CreatedBy,
                                    Range = roomInfo.Range,
                                    LocInfo = new LocationInfoDto {
                                        Latitude = roomInfo.LocInfo.Latitude,
                                        Longitude = roomInfo.LocInfo.Longitude
                                }});
        
    }


    [HttpPost("join/{roomId}/{userId}")]
    public async Task<ActionResult> JoinRoom(Guid roomId,string userId, LocationInfoDto locInfo) {
        // TODO : Check if room and user exists
        // TODO : Check if location info matches within the range of room
        _roomRepository.JoinRoom(roomId,userId);
        bool saved = await _roomRepository.SaveChangesAsync();
        if(!saved) {
            _logger.LogError("JoinRoom : Adding participant to DB failed");
            return StatusCode(500);
        }
        return NoContent(); 
    }

    [HttpPost("leave/{roomId}/user/{userId}")]
    public async Task<ActionResult> LeaveRoom(Guid roomId, string userId) {
        bool participantValid = await _roomRepository.CheckParticipantValid(roomId,userId); 
        if(!participantValid) {
            _logger.LogInformation("LeaveRoom : User not part of room");
            return BadRequest();
        }

        _roomRepository.LeaveRoom(roomId,userId);
        bool saved = await _roomRepository.SaveChangesAsync();
        if(!saved) {
            _logger.LogError("LeaveRoom : Error saving changes to DB");
            return StatusCode(500);
        }
        return NoContent();
    }

    [HttpDelete("delete/{roomId}")]
    public async Task<ActionResult> DeleteRoom(Guid roomId) {
        // TODO : Make sure only creator can delete room
        RoomDto roomToDelete = await _roomRepository.GetRoomDetailsAsync(roomId);
        if(roomToDelete == null) {
            return NotFound();
        }

        _roomRepository.DeleteRoom(roomToDelete);
        bool saved = await _roomRepository.SaveChangesAsync();
        if(!saved) {
            _logger.LogError("DeleteRoom : Failed to delete room from DB");
            return StatusCode(500);
        }

        _logger.LogInformation($"DeleteRoom : Deleted room {roomId} from db");
        return NoContent();
    }
}
