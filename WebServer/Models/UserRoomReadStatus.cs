using WebServer.Areas.Identity.Data;

namespace WebServer.Models;

public class UserRoomReadStatus
{
    public int Id { get; set; }
    
    public string UserId { get; set; } = string.Empty;
    public AppUser? User { get; set; }
    
    public int RoomId { get; set; }
    public Room? Room { get; set; }
    
    public DateTime LastReadTime { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}