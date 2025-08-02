using WebServer.Areas.Identity.Data;

namespace WebServer.Models;

public class RoomListItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ActiveMembersCount { get; set; }
    public int UnreadMessagesCount { get; set; }
    public bool IsUserMember { get; set; }
    public DateTime? LastMessageTime { get; set; }
    public string? LastMessageContent { get; set; }
    public string? LastMessageSender { get; set; }
}