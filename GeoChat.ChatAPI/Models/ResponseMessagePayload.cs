namespace GeoChat.ChatAPI.Models;

public class ResponseMessagePayload
{
    public string Type {get; set;} = string.Empty;
    public IEnumerable<ChatDto> Chats {get; set;} = new List<ChatDto>();
}
