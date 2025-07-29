using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebServer.Data;
using WebServer.Models;

namespace WebServer.Pages;

#if DEBUG
public class DebugModel : PageModel
{
    private readonly ChatDbContext _context;

    public DebugModel(ChatDbContext context)
    {
        _context = context;
    }

    public class DatabaseSummary
    {
        public int UsersCount { get; set; }
        public int RoomsCount { get; set; }
        public int MessagesCount { get; set; }
        public int ActiveMembersCount { get; set; }
    }

    public DatabaseSummary Summary { get; set; } = new();
    public List<Room> Rooms { get; set; } = new();
    public List<Message> RecentMessages { get; set; } = new();

    public async Task OnGetAsync()
    {
        // 获取数据库摘要
        Summary.UsersCount = await _context.Users.CountAsync();
        Summary.RoomsCount = await _context.Rooms.CountAsync();
        Summary.MessagesCount = await _context.Messages.CountAsync();
        Summary.ActiveMembersCount = await _context.RoomMembers.CountAsync(rm => rm.IsActive);

        // 获取房间列表（包含关联数据）
        Rooms = await _context.Rooms
            .Include(r => r.Creator)
            .Include(r => r.Members.Where(m => m.IsActive))
                .ThenInclude(m => m.User)
            .Include(r => r.Messages)
            .ToListAsync();

        // 获取最近的消息
        RecentMessages = await _context.Messages
            .Include(m => m.Sender)
            .Include(m => m.Room)
            .OrderByDescending(m => m.Time)
            .Take(20)
            .ToListAsync();
    }
}
#endif