using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebServer.Services;

namespace WebServer.Hubs;

[Authorize] // 要求用户登录
public class ChatHub : Hub
{
    private readonly ChatService _chatService;

    public ChatHub(ChatService chatService)
    {
        _chatService = chatService;
    }

    // 当客户端发送消息时，此方法会被调用
    public async Task SendMessageToRoom(string roomId, string message)
    {
        var userId = Context.UserIdentifier;
        if (userId == null) 
        {
            await Clients.Caller.SendAsync("Error", "User not authenticated");
            return;
        }

        if (!int.TryParse(roomId, out var roomIdInt)) return;

        try
        {
            // 使用 ChatService 创建并发送消息
            await _chatService.CreateAndSendMessageAsync(roomIdInt, userId, message);
        }
        catch (InvalidOperationException)
        {
            // 用户不在房间中，发送错误消息给客户端
            await Clients.Caller.SendAsync("Error", "You are not a member of this room");
        }
    }

    // 允许客户端加入特定房间的组
    public async Task AddToRoom(string roomId)
    {
        var userId = Context.UserIdentifier;
        if (userId == null) 
        {
            await Clients.Caller.SendAsync("Error", "User not authenticated");
            return;
        }

        if (!int.TryParse(roomId, out var roomIdInt)) return;

        // 验证用户是否有权加入该房间
        var isInRoom = await _chatService.IsUserInRoomAsync(roomIdInt, userId);
        if (isInRoom)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.Caller.SendAsync("JoinedRoom", roomId);
        }
        else
        {
            await Clients.Caller.SendAsync("Error", "You are not a member of this room");
        }
    }

    // 离开房间组
    public async Task RemoveFromRoom(string roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
    }

    // 加入全局通知组（用于接收新消息通知）
    public async Task JoinGlobalNotifications()
    {
        var userId = Context.UserIdentifier;
        if (userId != null)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId}");
            await Clients.Caller.SendAsync("JoinedGlobalNotifications");
        }
    }

    // 当客户端连接时调用
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        if (userId != null)
        {
            // 自动加入全局通知组
            await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId}");
            Console.WriteLine($"✅ User {userId} connected and joined global notifications");
        }
        else
        {
            Console.WriteLine($"❌ Unauthenticated connection attempt");
            Context.Abort(); // 终止未认证的连接
            return;
        }
        
        await base.OnConnectedAsync();
    }

    // 当客户端断开连接时调用
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        if (userId != null)
        {
            Console.WriteLine($"User {userId} disconnected");
        }
        await base.OnDisconnectedAsync(exception);
    }
}
