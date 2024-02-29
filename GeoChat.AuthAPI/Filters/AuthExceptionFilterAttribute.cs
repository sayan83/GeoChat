using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GeoChat.AuthAPI.Filters;

public class AuthExceptionFilterAttribute : IExceptionFilter 
{
    private readonly ILogger<AuthExceptionFilterAttribute> _logger;
    public AuthExceptionFilterAttribute(ILogger<AuthExceptionFilterAttribute> logger) {
        _logger = logger;
    }
    public void OnException(ExceptionContext context)
    {
        _logger.LogError("Unhandled Exception encountered - {0}",context.Exception.ToString()); 
        context.Result = new ObjectResult(null) {
            StatusCode = (int)HttpStatusCode.InternalServerError
        };
    }
}
