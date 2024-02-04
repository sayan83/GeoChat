using System.ComponentModel.DataAnnotations;

namespace GeoChat.DataLayer.Entities;

public class User
{
    [Key]
    public string UserId { get; set; }
    public string Name { get; set; }
    public string Password {get; set; }

    public User(string userId, string name, string password) {
        UserId = userId;
        Name = name;
        Password = password;
    }
}
