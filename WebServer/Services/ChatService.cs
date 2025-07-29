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
    private readonly ILogger<ChatService> _logger;

    public ChatService(ChatDbContext context, IHubContext<ChatHub> hubContext, ILogger<ChatService> logger)
    {
        _context = context;
        _hubContext = hubContext;
        _logger = logger;
    }

    // 创建房间
    public async Task<Room> CreateRoomAsync(string name, string? description, string creatorId)
    {
        _logger.LogInformation("Creating room '{Name}' by user {CreatorId}", name, creatorId);
        
        var room = new Room
        {
            Name = name,
            Description = description,
            CreatorId = creatorId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Room created with ID {RoomId}", room.Id);

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
        _logger.LogInformation("User {UserId} attempting to join room {RoomId}", userId, roomId);
        
        // 检查房间是否存在
        var room = await _context.Rooms.FindAsync(roomId);
        if (room == null) 
        {
            _logger.LogWarning("Room {RoomId} not found", roomId);
            return false;
        }

        // 检查用户是否已在房间中
        var existingMember = await _context.RoomMembers
            .FirstOrDefaultAsync(rm => rm.RoomId == roomId && rm.UserId == userId && rm.IsActive);
        
        if (existingMember != null) 
        {
            _logger.LogInformation("User {UserId} is already in room {RoomId}", userId, roomId);
            return true; // 已经在房间中
        }

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

        _logger.LogInformation("User {UserId} successfully joined room {RoomId}", userId, roomId);

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
        _logger.LogInformation("User {UserId} attempting to leave room {RoomId}", userId, roomId);
        
        var roomMember = await _context.RoomMembers
            .FirstOrDefaultAsync(rm => rm.RoomId == roomId && rm.UserId == userId && rm.IsActive);

        if (roomMember == null) 
        {
            _logger.LogWarning("User {UserId} is not an active member of room {RoomId}", userId, roomId);
            return false;
        }

        roomMember.IsActive = false;
        await _context.SaveChangesAsync();

        _logger.LogInformation("User {UserId} successfully left room {RoomId}", userId, roomId);

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
        _logger.LogInformation("User {SenderId} sending message to room {RoomId}: {Content}", senderId, roomId, content);
        
        // 验证用户是否在房间中
        var isMember = await _context.RoomMembers
            .AnyAsync(rm => rm.RoomId == roomId && rm.UserId == senderId && rm.IsActive);

        if (!isMember)
        {
            _logger.LogWarning("User {SenderId} attempted to send message to room {RoomId} but is not a member", senderId, roomId);
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

        _logger.LogInformation("Message {MessageId} created in room {RoomId}", message.Id, roomId);

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

    // 调试方法：获取数据库统计信息
#if DEBUG
    public async Task<object> GetDatabaseStatsAsync()
    {
        return new
        {
            Users = await _context.Users.CountAsync(),
            Rooms = await _context.Rooms.CountAsync(),
            Messages = await _context.Messages.CountAsync(),
            ActiveMembers = await _context.RoomMembers.CountAsync(rm => rm.IsActive),
            RecentActivity = await _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Room)
                .OrderByDescending(m => m.Time)
                .Take(5)
                .Select(m => new
                {
                    m.Time,
                    Sender = m.Sender != null ? m.Sender.UserName : "Unknown",
                    Room = m.Room != null ? m.Room.Name : "Unknown",
                    m.Content
                })
                .ToListAsync()
        };
    }

    public async Task LogCurrentStateAsync()
    {
        var stats = await GetDatabaseStatsAsync();
        _logger.LogInformation("Current database state: {@Stats}", stats);
    }
#endif
}