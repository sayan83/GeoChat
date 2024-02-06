using GeoChat.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace GeoChat.DataLayer.DbContexts;

public class GeoChatDBContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<RoomParticipant> Participants { get; set; }
    public DbSet<Chat> Chats {get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("DataSource=../GeoChatDB.db");
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User("sayan83","Sayantan","sayantan"),
            new User("slave1","Slave I Am","slave"),
            new User("agent1","Agent It Is","agent") 
        );
        modelBuilder.Entity<Room>().HasData(
            new Room("Room1") {
                RoomId = new Guid("0d0b9a01-7246-4fb1-ac70-2f2c5d5f084b"),
                Description = "My first room",
                UserId = "agent1"
            },
            new Room("Kings Room") {
                RoomId = new Guid("65958d08-4e33-40da-a0ac-54442fba0abe"),
                Description = "Private King room",
                UserId = "sayan83"
            }
        );
        modelBuilder.Entity<RoomParticipant>().HasData(
            new RoomParticipant() {
                RoomId = new Guid("65958d08-4e33-40da-a0ac-54442fba0abe"),
                UserId = "sayan83"
            },
            new RoomParticipant() {
                RoomId = new Guid("65958d08-4e33-40da-a0ac-54442fba0abe"),
                UserId = "slave1"
            }, 
            new RoomParticipant() {
                RoomId = new Guid("0d0b9a01-7246-4fb1-ac70-2f2c5d5f084b"),
                UserId = "agent1"
            },
            new RoomParticipant() {
                RoomId = new Guid("0d0b9a01-7246-4fb1-ac70-2f2c5d5f084b"),
                UserId = "slave1"
            },
            new RoomParticipant() {
                RoomId = new Guid("0d0b9a01-7246-4fb1-ac70-2f2c5d5f084b"),
                UserId = "sayan83"
            }
        );
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
        base.OnModelCreating(modelBuilder);
    }
}
