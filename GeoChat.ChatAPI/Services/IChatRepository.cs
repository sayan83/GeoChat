using GeoChat.ChatAPI.Models;

namespace GeoChat.ChatAPI;

public interface IChatRepository
{
    void AddNewMessage(string UserId, Guid RoomId, string Message);
    Task<IEnumerable<ChatDto>> FetchMessagesAsync(Guid RoomId, int StartRange, int noOfMessages);
    Task<bool> SaveChangesAsync();
}
