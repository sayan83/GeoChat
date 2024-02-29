using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GeoChat.ChatAPI.Filters;

public class ChatSocketExceptionFilterAttribute : IExceptionFilter
{
    private readonly ILogger<ChatSocketExceptionFilterAttribute> _logger;
    public ChatSocketExceptionFilterAttribute(ILogger<ChatSocketExceptionFilterAttribute> logger) {
        _logger = logger;
    }
    public void OnException(ExceptionContext context)
    {
        _logger.LogError("Unhandled Exception encountered - {0}",context.Exception.ToString());
        // TODO : Figure out if response is possible in case of socket connection
        // context.Result = new ObjectResult(null) {
        //     StatusCode = (int)HttpStatusCode.InternalServerError
        // };
    }
}
