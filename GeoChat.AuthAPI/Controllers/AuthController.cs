using GeoChat.AuthAPI.Filters;
using GeoChat.AuthAPI.Models;
using GeoChat.AuthAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeoChat.AuthAPI;

[ApiController]
[Route("/api/auth")]
[TypeFilter<AuthExceptionFilterAttribute>]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthRepository _authRepository;
    public AuthController(ILogger<AuthController> logger, IAuthRepository authRepository) {
        _logger = logger;
        _authRepository = authRepository; 
    }
    
    [HttpGet("userdetails/{userId}")] 
    public async Task<ActionResult<UserDto>> GetUserInfo(string userId) {
        UserDto user = await _authRepository.GetUserAsync(userId);
        if(user == null)
            return NotFound();
        
        return Ok(user);
    }
    [HttpPost("login")] 
    public async Task<ActionResult<UserDto>> Login(LogInDto logInInfo) {
        bool isValid = await _authRepository.VerifyCredentialsAsync(logInInfo.UserId,logInInfo.Password);
        if(!isValid) {
            return Unauthorized();
        } 


        _logger.LogInformation($"Login : User {logInInfo.UserId} logged in");
        UserDto validatedUser = new UserDto {
            UserId = logInInfo.UserId,
        };
        return Ok(validatedUser);
    }
    
    [HttpPost("new")]
    public async Task<ActionResult<UserDto>> NewAccount(NewUserDto userInfo) {
        if(await _authRepository.VerifyUserIdExistsAsync(userInfo.UserId)) {
            return Conflict(); 
        }

        _authRepository.AddNewUser(userInfo.UserId,userInfo.Name,userInfo.Password);
        bool saved = await _authRepository.SaveChangesAsync();

        if(!saved) {
            _logger.LogError("NewAccount : Failed to save new user info to DB");
            return StatusCode(500);
        }

        _logger.LogInformation($"NewAccount : New user {userInfo.UserId} created");
        UserDto createdUser = new UserDto {
            UserId = userInfo.UserId
        };
        return CreatedAtAction("GetUserInfo", new {
            userInfo.UserId
        }, createdUser );
    }
}
