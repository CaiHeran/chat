using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebServer.Services;
using WebServer.Models;
using WebServer.Areas.Identity.Data;

namespace WebServer.Controllers;

[Authorize] // 要求用户登录
public class RoomController : Controller
{
    private readonly ChatService _chatService;

    public RoomController(ChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task<IActionResult> Index(int? roomid)
    {
        if (roomid == null) 
        {
            return RedirectToAction("Index", "Rooms");
        }

        var room = await _chatService.GetRoomAsync(roomid.Value);
        if (room == null) 
        {
            return RedirectToAction("Index", "Rooms");
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        // 检查用户是否在房间中
        var isInRoom = await _chatService.IsUserInRoomAsync(roomid.Value, userId);
        if (!isInRoom)
        {
            // 用户不在房间中，可以显示房间信息并提供加入选项
            ViewData["CanJoin"] = true;
            ViewData["IsInRoom"] = false;
        }
        else
        {
            ViewData["CanJoin"] = false;
            ViewData["IsInRoom"] = true;
            
            // 用户在房间中，标记所有消息为已读
            // 这表示用户正在查看房间，应该看到所有消息都已读
            await _chatService.MarkAllMessagesAsReadAsync(userId, roomid.Value);
        }

        ViewData["RoomId"] = room.Id;
        ViewData["RoomName"] = room.Name;
        
        // 将 Room 实体转换为 RoomViewModel
        var roomViewModel = new RoomViewModel
        {
            Id = room.Id,
            Name = room.Name,
            Description = room.Description,
            Members = room.Members?.Where(m => m.IsActive).Select(m => m.User).Where(u => u != null).Cast<AppUser>().ToList() ?? new List<AppUser>(),
            Messages = room.Messages?.Select(m => new RoomMessageModel
            {
                Id = m.Id,
                Time = m.Time,
                Type = m.Type,
                SenderId = m.SenderId,
                SenderName = m.Sender?.UserName ?? "Unknown",
                Content = m.Content
            }).ToList() ?? new List<RoomMessageModel>()
        };
        
        return View(roomViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> JoinRoom(int roomId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Json(new { success = false, message = "User not authenticated" });
        }

        var success = await _chatService.JoinRoomAsync(roomId, userId);
        if (success)
        {
            return Json(new { success = true });
        }
        
        return Json(new { success = false, message = "Failed to join room" });
    }

    [HttpPost]
    public async Task<IActionResult> LeaveRoom(int roomId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Json(new { success = false, message = "User not authenticated" });
        }

        var success = await _chatService.LeaveRoomAsync(roomId, userId);
        return Json(new { success });
    }

    [HttpPost]
    public async Task<IActionResult> MarkAsRead(int roomId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Json(new { success = false, message = "User not authenticated" });
        }

        // 验证用户ID是否有效
        if (!await _chatService.IsValidUserAsync(userId))
        {
            return Json(new { success = false, message = "Invalid user session" });
        }

        await _chatService.UpdateLastReadTimeAsync(userId, roomId);
        return Json(new { success = true });
    }

    [HttpGet]
    public async Task<IActionResult> GetOnlineMembers(int roomId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Json(new { success = false, message = "User not authenticated" });
        }

        var isInRoom = await _chatService.IsUserInRoomAsync(roomId, userId);
        if (!isInRoom)
        {
            return Json(new { success = false, message = "User not in room" });
        }

        var members = await _chatService.GetRoomMembersAsync(roomId);
        var onlineMembers = members
            .Where(m => m.IsActive)
            .Select(m => new 
   { 
       id = m.UserId,
                name = m.User?.UserName ?? "Unknown",
  email = m.User?.Email ?? ""
     })
            .ToList();

        return Json(new { success = true, members = onlineMembers });
    }
}
