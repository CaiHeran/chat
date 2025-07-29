using WebServer.Areas.Identity.Data;

namespace WebServer.Models;

public class RoomMember
{
    public int Id { get; set; }
    
    public int RoomId { get; set; }
    public Room? Room { get; set; }
    
    public string UserId { get; set; } = string.Empty;
    public AppUser? User { get; set; }
    
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    
    public bool IsActive { get; set; } = true; // 是否仍在房间中
}