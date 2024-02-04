namespace GeoChat.AuthAPI.Models;

public class NewUserDto
{
    public string UserId { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
}
