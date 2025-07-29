using System.ComponentModel.DataAnnotations;
using WebServer.Areas.Identity.Data;

namespace WebServer.Models;

public class Room
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // 房主ID
    public string? CreatorId { get; set; }
    public AppUser? Creator { get; set; }
    
    // 房间成员
    public List<RoomMember> Members { get; set; } = [];
    
    // 房间消息
    public List<Message> Messages { get; set; } = [];
}