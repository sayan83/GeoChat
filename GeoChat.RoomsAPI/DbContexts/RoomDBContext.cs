using GeoChat.RoomsAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace GeoChat.RoomsAPI.DbContexts;

public class RoomDBContext : DbContext
{
    public DbSet<Room> Rooms { get; set;}
    public DbSet<RoomParticipant> Participants { get; set;}

    public RoomDBContext(DbContextOptions<RoomDBContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Room>().HasData(
            new Room("Room1") {
                RoomId = new Guid("0d0b9a01-7246-4fb1-ac70-2f2c5d5f084b"),
                Description = "My first room",
                UserId = "agent1",
                Latitude = 22.5869952,  
                Longitude = 88.4393111,
                Range = 5000
            },
            new Room("Kings Room") {
                RoomId = new Guid("65958d08-4e33-40da-a0ac-54442fba0abe"),
                Description = "Private King room",
                UserId = "sayan83",
                Latitude = 22.5869952,  
                Longitude = 88.4393111,
                Range = 1000
            });
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
            });
    }
}
