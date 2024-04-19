using GeoChat.ChatAPI.Entities;
using GeoChat.ChatAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GeoChat.ChatAPI;

public class ChatRepository : IChatRepository
{
    private readonly ChatDBContext _context;
    public ChatRepository(ChatDBContext context) {
        _context = context;
    }
    public void AddNewMessage(string UserId, Guid RoomId, string Message)
    {
        Chat newChat = new Chat() {
            ChatId = Guid.NewGuid(),
            UserId = UserId,
            RoomId = RoomId,
            Message = Message,
            SendTime = DateTimeOffset.Now.ToUnixTimeSeconds()
        };
        _context.Chats.Add(newChat);
    }

    public async Task<IEnumerable<ChatDto>> FetchMessagesAsync(Guid roomId, int StartRange, int noOfMessages)
    {
        List<Chat> messages = await _context.Chats.Where(c => c.RoomId == roomId)
                                        .OrderByDescending(c => c.SendTime)
                                        .Skip(StartRange)
                                        .Take(noOfMessages).
                                        ToListAsync();

        List<ChatDto> chats = new List<ChatDto>();
        foreach(Chat message in messages) {
            chats.Add(new ChatDto{
                UserId = message.UserId,
                Message = message.Message,
                Timestamp = message.SendTime
            });
        }
        return chats;
    }
    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync() > 0); 
    }
}
