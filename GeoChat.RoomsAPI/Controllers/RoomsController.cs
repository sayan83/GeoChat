using GeoChat.RoomsAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace GeoChat.RoomsAPI.Controllers;

// TODO : Add versioning info 
[ApiController]
[Route("/api/rooms")]
public class RoomsController : ControllerBase
{
    [HttpPost("/search")]
    public ActionResult<IEnumerable<RoomDto>> SearchRooms(LocationInfoDto locInfo) {
        return Ok();
    }


    [HttpPost("/create")]
    public ActionResult<RoomDto> CreateRoom(RoomCreationDto roomInfo) {
        return Ok();
    }


    [HttpPost("join/{roomId}")]
    public ActionResult JoinRoom(Guid roomId, LocationInfoDto locInfo) {
        return Ok();
    }

    [HttpPost("leave/{roomId}")]
    public ActionResult LeaveRoom(Guid roomId) {
        return Ok();
    }
}
