using GeoChat.AuthAPI.Models;
using GeoChat.DataLayer.Models;
using GeoChat.DataLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeoChat.AuthAPI;

[ApiController]
[Route("/api/auth")]
public class AuthController : ControllerBase
{
    private readonly IGeoChatRepository _geoChatRepository;
    public AuthController(IGeoChatRepository geoChatRepository) {
        _geoChatRepository = geoChatRepository;
    }
    
    [HttpGet("/userdetails/{userId}")] 
    public async Task<ActionResult<UserInfoDto>> GetUserInfo(string userId) {
        UserInfoDto user = await _geoChatRepository.GetUserAsync(userId);
        if(user == null)
            return NotFound();
        
        return Ok(user);
    }
    [HttpPost("/login")] 
    public async Task<ActionResult<UserDto>> Login(LogInDto logInInfo) {
        bool isValid = await _geoChatRepository.VerifyCredentialsAsync(logInInfo.UserId,logInInfo.Password);
        if(!isValid) {
            return Unauthorized();
        } 

        UserDto validatedUser = new UserDto {
            UserId = logInInfo.UserId,
        };
        return Ok(validatedUser);
    }
    
    [HttpPost("/new")]
    public async Task<ActionResult<UserDto>> NewAccount(NewUserDto userInfo) {
        if(await _geoChatRepository.VerifyUserIdExistsAsync(userInfo.UserId)) {
            return Conflict(); 
        }

        _geoChatRepository.AddNewUser(userInfo.UserId,userInfo.Name,userInfo.Password);
        bool saved = await _geoChatRepository.SaveChangesAsync();

        if(!saved) {
            return StatusCode(500);
        }
        UserDto createdUser = new UserDto {
            UserId = userInfo.UserId
        };
        return CreatedAtAction("GetUserInfo", new {
            userInfo.UserId
        }, createdUser );
    }
}
