using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebServer.Services;
using WebServer.Models;

namespace WebServer.Controllers
{
    [Authorize] // 要求用户登录
    public class RoomsController : Controller
    {
        private readonly ChatService _chatService;

        public RoomsController(ChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var rooms = await _chatService.GetRoomsWithUnreadCountAsync(userId);
            return View(rooms);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoomModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var room = await _chatService.CreateRoomAsync(model.Name, model.Description, userId);
                return RedirectToAction("Index", "Room", new { roomid = room.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"创建房间失败: {ex.Message}");
                return View(model);
            }
        }

        public async Task<IActionResult> Join(int? roomid)
        {
            if (roomid == null)
            {
                return RedirectToAction("Index");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // 检查房间是否存在
            var room = await _chatService.GetRoomAsync(roomid.Value);
            if (room == null)
            {
                TempData["Error"] = "房间不存在";
                return RedirectToAction("Index");
            }

            // 如果用户不在房间中，先加入房间
            var isInRoom = await _chatService.IsUserInRoomAsync(roomid.Value, userId);
            if (!isInRoom)
            {
                await _chatService.JoinRoomAsync(roomid.Value, userId);
            }

            return RedirectToAction("Index", "Room", new { roomid = roomid });
        }

        [HttpGet]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Json(new { unreadCount = 0 });
            }

            var unreadCount = await _chatService.GetTotalUnreadCountAsync(userId);
            return Json(new { unreadCount });
        }
    }
}
