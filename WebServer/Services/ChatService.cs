using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebServer.Data;
using WebServer.Hubs;
using WebServer.Models;

namespace WebServer.Services;

public class ChatService
{
    private readonly ChatDbContext _context;
    private readonly IHubContext<ChatHub> _hubContext;

    public ChatService(ChatDbContext context, IHubContext<ChatHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    // 创建房间
    public async Task<Room> CreateRoomAsync(string name, string? description, string creatorId)
    {
        var room = new Room
        {
            Name = name,
            Description = description,
            CreatorId = creatorId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();

        // 创建者自动加入房间
        await JoinRoomAsync(room.Id, creatorId);

        return room;
    }

    // 获取房间信息
    public async Task<Room?> GetRoomAsync(int roomId)
    {
        return await _context.Rooms
            .Include(r => r.Creator)
            .Include(r => r.Members.Where(m => m.IsActive))
                .ThenInclude(m => m.User)
            .Include(r => r.Messages.OrderBy(m => m.Time))
                .ThenInclude(m => m.Sender)
            .FirstOrDefaultAsync(r => r.Id == roomId);
    }

    // 获取所有房间列表
    public async Task<List<Room>> GetRoomsAsync()
    {
        return await _context.Rooms
            .Include(r => r.Creator)
            .Include(r => r.Members.Where(m => m.IsActive))
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    // 加入房间
    public async Task<bool> JoinRoomAsync(int roomId, string userId)
    {
        // 检查房间是否存在
        var room = await _context.Rooms.FindAsync(roomId);
        if (room == null) return false;

        // 检查用户是否已在房间中
        var existingMember = await _context.RoomMembers
            .FirstOrDefaultAsync(rm => rm.RoomId == roomId && rm.UserId == userId && rm.IsActive);
        
        if (existingMember != null) return true; // 已经在房间中

        // 添加新成员
        var roomMember = new RoomMember
        {
            RoomId = roomId,
            UserId = userId,
            JoinedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.RoomMembers.Add(roomMember);
        await _context.SaveChangesAsync();

        // 通知房间其他成员
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            await _hubContext.Clients.Group(roomId.ToString())
                .SendAsync("UserJoined", user.UserName, DateTime.UtcNow);
        }

        return true;
    }

    // 离开房间
    public async Task<bool> LeaveRoomAsync(int roomId, string userId)
    {
        var roomMember = await _context.RoomMembers
            .FirstOrDefaultAsync(rm => rm.RoomId == roomId && rm.UserId == userId && rm.IsActive);

        if (roomMember == null) return false;

        roomMember.IsActive = false;
        await _context.SaveChangesAsync();

        // 通知房间其他成员
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            await _hubContext.Clients.Group(roomId.ToString())
                .SendAsync("UserLeft", user.UserName, DateTime.UtcNow);
        }

        return true;
    }

    // 创建并发送消息
    public async Task<Message> CreateAndSendMessageAsync(int roomId, string senderId, string content)
    {
        // 验证用户是否在房间中
        var isMember = await _context.RoomMembers
            .AnyAsync(rm => rm.RoomId == roomId && rm.UserId == senderId && rm.IsActive);

        if (!isMember)
        {
            throw new InvalidOperationException("User is not a member of this room");
        }

        // 创建消息
        var message = new Message
        {
            RoomId = roomId,
            SenderId = senderId,
            Content = content,
            Time = DateTime.UtcNow,
            Type = 0 // 文本消息
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        // 加载发送者信息
        await _context.Entry(message)
            .Reference(m => m.Sender)
            .LoadAsync();

        // 通过 SignalR 广播消息
        await _hubContext.Clients.Group(roomId.ToString())
            .SendAsync("ReceiveMessage", message.Sender?.UserName ?? "Unknown", message.Content, message.Time);

        return message;
    }

    // 检查用户是否在房间中
    public async Task<bool> IsUserInRoomAsync(int roomId, string userId)
    {
        return await _context.RoomMembers
            .AnyAsync(rm => rm.RoomId == roomId && rm.UserId == userId && rm.IsActive);
    }
}