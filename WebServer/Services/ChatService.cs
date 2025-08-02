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

    // 获取用户的房间列表（包含未读消息数）
    public async Task<List<RoomListItemViewModel>> GetRoomsWithUnreadCountAsync(string userId)
    {
        var rooms = await _context.Rooms
            .Include(r => r.Members.Where(m => m.IsActive))
            .Include(r => r.Messages.OrderByDescending(m => m.Time).Take(1))
                .ThenInclude(m => m.Sender)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        var result = new List<RoomListItemViewModel>();

        foreach (var room in rooms)
        {
            var isUserMember = room.Members.Any(m => m.UserId == userId && m.IsActive);
            
            // 获取用户在此房间的最后阅读时间
            var lastReadTime = await GetLastReadTimeAsync(userId, room.Id);
            
            // 只有当用户是房间成员时才计算未读消息数
            // 计算从最后阅读时间之后的其他用户发送的消息
            var unreadCount = 0;
            if (isUserMember)
            {
                unreadCount = await _context.Messages
                    .CountAsync(m => m.RoomId == room.Id && 
                                   m.Time > lastReadTime && 
                                   m.SenderId != userId); // 只计算其他用户的消息
            }

            var lastMessage = room.Messages.FirstOrDefault();
            
            result.Add(new RoomListItemViewModel
            {
                Id = room.Id,
                Name = room.Name,
                Description = room.Description,
                CreatedAt = room.CreatedAt,
                ActiveMembersCount = room.Members.Count(m => m.IsActive),
                UnreadMessagesCount = unreadCount,
                IsUserMember = isUserMember,
                LastMessageTime = lastMessage?.Time,
                LastMessageContent = lastMessage?.Content,
                LastMessageSender = lastMessage?.Sender?.UserName
            });
        }

        return result;
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
        
        // 初始化用户阅读状态
        await UpdateLastReadTimeAsync(userId, roomId);
        
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

        // 发送消息意味着用户在房间中且活跃，立即更新最后阅读时间
        // 这样发送者就能看到房间中的所有消息都已读
        await UpdateLastReadTimeAsync(senderId, roomId);

        // 加载发送者信息
        await _context.Entry(message)
            .Reference(m => m.Sender)
            .LoadAsync();

        // 通过 SignalR 广播消息到房间成员
        await _hubContext.Clients.Group(roomId.ToString())
            .SendAsync("ReceiveMessage", message.Sender?.UserName ?? "Unknown", message.Content, message.Time, message.SenderId);
            
        // 通知所有登录用户有新消息（用于更新未读计数）
        // 但不通知发送者自己，因为发送者已经在房间中
        var roomMembers = await _context.RoomMembers
            .Where(rm => rm.RoomId == roomId && rm.IsActive)
            .Select(rm => rm.UserId)
            .ToListAsync();
            
        foreach (var memberId in roomMembers)
        {
            if (memberId != senderId) // 不通知发送者自己
            {
                await _hubContext.Clients.User(memberId)
                    .SendAsync("NewMessageNotification", roomId, message.Sender?.UserName ?? "Unknown", message.Content);
            }
        }

        return message;
    }

    // 检查用户是否在房间中
    public async Task<bool> IsUserInRoomAsync(int roomId, string userId)
    {
        return await _context.RoomMembers
            .AnyAsync(rm => rm.RoomId == roomId && rm.UserId == userId && rm.IsActive);
    }

    // 验证用户ID是否在数据库中存在（安全检查）
    public async Task<bool> IsValidUserAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId)) return false;
        
        try
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                _logger.LogWarning("Invalid user ID attempted access: {UserId}", userId);
            }
            return userExists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating user {UserId}", userId);
            return false;
        }
    }

    // 更新用户最后阅读时间
    public async Task UpdateLastReadTimeAsync(string userId, int roomId)
    {
        var readStatus = await _context.UserRoomReadStatuses
            .FirstOrDefaultAsync(urrs => urrs.UserId == userId && urrs.RoomId == roomId);

        if (readStatus == null)
        {
            readStatus = new UserRoomReadStatus
            {
                UserId = userId,
                RoomId = roomId,
                LastReadTime = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.UserRoomReadStatuses.Add(readStatus);
        }
        else
        {
            readStatus.LastReadTime = DateTime.UtcNow;
            readStatus.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
    }

    // 用户进入房间时，标记所有消息为已读
    public async Task MarkAllMessagesAsReadAsync(string userId, int roomId)
    {
        // 检查用户是否在房间中
        var isMember = await IsUserInRoomAsync(roomId, userId);
        if (!isMember)
        {
            _logger.LogWarning("User {UserId} attempted to mark messages as read for room {RoomId} but is not a member", userId, roomId);
            return;
        }

        // 更新最后阅读时间为当前时间，这样所有之前的消息都被标记为已读
        await UpdateLastReadTimeAsync(userId, roomId);
        
        _logger.LogInformation("User {UserId} marked all messages as read in room {RoomId}", userId, roomId);
    }

    // 获取用户最后阅读时间
    public async Task<DateTime> GetLastReadTimeAsync(string userId, int roomId)
    {
        var readStatus = await _context.UserRoomReadStatuses
            .FirstOrDefaultAsync(urrs => urrs.UserId == userId && urrs.RoomId == roomId);

        return readStatus?.LastReadTime ?? DateTime.MinValue;
    }

    // 获取用户总未读消息数
    public async Task<int> GetTotalUnreadCountAsync(string userId)
    {
        var userRooms = await _context.RoomMembers
            .Where(rm => rm.UserId == userId && rm.IsActive)
            .Select(rm => rm.RoomId)
            .ToListAsync();

        var totalUnread = 0;
        foreach (var roomId in userRooms)
        {
            var lastReadTime = await GetLastReadTimeAsync(userId, roomId);
            var unreadCount = await _context.Messages
                .CountAsync(m => m.RoomId == roomId && 
                               m.Time > lastReadTime && 
                               m.SenderId != userId); // 排除自己发送的消息
            totalUnread += unreadCount;
        }

        return totalUnread;
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