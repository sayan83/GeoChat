using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GeoChat.RoomsAPI.Filters;

public class RoomExceptionFilterAttribute : IExceptionFilter
{
    private readonly ILogger<RoomExceptionFilterAttribute> _logger;
    public RoomExceptionFilterAttribute(ILogger<RoomExceptionFilterAttribute> logger) {
        _logger = logger;
    }
    public void OnException(ExceptionContext context)
    {
        _logger.LogError("Unhandled exceptin encountered - {0}",context.Exception.ToString());
        context.Result = new ObjectResult(null) {
            StatusCode = (int)HttpStatusCode.InternalServerError
        };
    }
}
