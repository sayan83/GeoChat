using RespChatDto = GeoChat.DataLayer.Models.ChatDto;

namespace GeoChat.ChatAPI.Models;

public class ResponseMessagePayload
{
    public string Type {get; set;} = string.Empty;
    public IEnumerable<RespChatDto> Chats {get; set;} = new List<RespChatDto>();
}
