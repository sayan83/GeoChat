using GeoChat.ChatAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GeoChat.ChatAPI;

public class ChatDBContext : DbContext
{
    public DbSet<Chat> Chats { get; set;}

    public ChatDBContext(DbContextOptions<ChatDBContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Chat>().HasData(
            new Chat() {
                ChatId = new Guid("c0bc0ef8-5708-492f-aa49-da2d150e2835"),
                RoomId = new Guid("0d0b9a01-7246-4fb1-ac70-2f2c5d5f084b"),
                UserId = "agent1",
                Message = "Hi, I am your new agent.",
                SendTime = 1706942491 
            },
            new Chat() {
                ChatId = new Guid("c68605be-7e12-4710-ae48-bf5d70c89868"),
                RoomId = new Guid("0d0b9a01-7246-4fb1-ac70-2f2c5d5f084b"),
                UserId = "slave1",
                Message = "Hi Okk ...",
                SendTime = 1706942611
            },
            new Chat() {
                ChatId = new Guid("e0bc032f-5728-484f-ad49-da2d150e5415"),
                RoomId = new Guid("65958d08-4e33-40da-a0ac-54442fba0abe"),
                UserId = "sayan83",
                Message = "U have been declared a slave henceforth",
                SendTime = 1706882313
            },
            new Chat() {
                ChatId = new Guid("d44facf9-d3f3-4f2b-baa0-15d62ed3d656"),
                RoomId = new Guid("65958d08-4e33-40da-a0ac-54442fba0abe"),
                UserId = "slave1",
                Message = "Ok Boss",
                SendTime = 1706882333
            },
            new Chat() {
                ChatId = new Guid("3362357a-2811-4100-9207-bf77e5b991ed"),
                RoomId = new Guid("65958d08-4e33-40da-a0ac-54442fba0abe"),
                UserId = "sayan83",
                Message = "Good! Get me some whiskey",
                SendTime = 1706896187
            },
            new Chat() {
                ChatId = new Guid("0b62f0c5-f566-4700-b122-9f852eedf07e"),
                RoomId = new Guid("65958d08-4e33-40da-a0ac-54442fba0abe"),
                UserId = "slave1",
                Message = "Toki for u boss ...",
                SendTime = 1706896238
            }
        );
    }
}
