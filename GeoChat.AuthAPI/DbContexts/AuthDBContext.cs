using GeoChat.AuthAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GeoChat.AuthAPI.DbContexts;

public class AuthDBContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public AuthDBContext(DbContextOptions<AuthDBContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<User>().HasData(
            new User("sayan83","Sayantan","sayantan"),
            new User("slave1","Slave I Am","slave"),
            new User("agent1","Agent It Is","agent") 
        );
    }

}
