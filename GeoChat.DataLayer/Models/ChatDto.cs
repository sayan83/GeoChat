namespace GeoChat.DataLayer.Models;

public class ChatDto
{
    public string Username {get; set;} = String.Empty;
    public string Message {get; set;} = String.Empty;
    public long Timestamp {get; set; }
}
