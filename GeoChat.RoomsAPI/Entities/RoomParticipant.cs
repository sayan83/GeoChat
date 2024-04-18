using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GeoChat.RoomsAPI.Entities;

[PrimaryKey(nameof(RoomId), nameof(UserId))]
public class RoomParticipant
{
    [ForeignKey("RoomId")]
    public Room RoomRef { get; set; }
    [Key]
    [Column(Order = 1)]
    public Guid RoomId { get; set; }
    // ForeignKey in users table
    [Key]
    [Column(Order = 2)]
    public string UserId { get; set; }
}
