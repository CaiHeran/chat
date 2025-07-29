using System.ComponentModel.DataAnnotations;
using WebServer.Areas.Identity.Data;

namespace WebServer.Models;

public class Message
{
    public int Id { get; set; }
    
    public DateTime Time { get; set; } = DateTime.UtcNow;
    
    public int Type { get; set; } = 0; // 消息类型：0=文本消息，1=系统消息等
    
    [Required]
    public required string SenderId { get; set; }
    public AppUser? Sender { get; set; }
    
    public int RoomId { get; set; }
    public Room? Room { get; set; }
    
    [Required]
    [MaxLength(1000)]
    public required string Content { get; set; }
}