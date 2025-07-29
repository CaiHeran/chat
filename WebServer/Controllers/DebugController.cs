using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebServer.Data;
using WebServer.Models;
using System.Text.Json;

namespace WebServer.Controllers;

#if DEBUG
[Route("api/[controller]")]
[ApiController]
public class DebugController : ControllerBase
{
    private readonly ChatDbContext _context;

    public DebugController(ChatDbContext context)
    {
        _context = context;
    }

    [HttpGet("database-info")]
    public async Task<IActionResult> GetDatabaseInfo()
    {
        var result = new
        {
            Users = await _context.Users.CountAsync(),
            Rooms = await _context.Rooms.CountAsync(),
            Messages = await _context.Messages.CountAsync(),
            RoomMembers = await _context.RoomMembers.CountAsync()
        };

        return Ok(result);
    }

    [HttpGet("rooms")]
    public async Task<IActionResult> GetAllRooms()
    {
        var rooms = await _context.Rooms
            .Include(r => r.Creator)
            .Include(r => r.Members.Where(m => m.IsActive))
                .ThenInclude(m => m.User)
            .Include(r => r.Messages)
                .ThenInclude(m => m.Sender)
            .ToListAsync();

        var result = rooms.Select(r => new
        {
            r.Id,
            r.Name,
            r.Description,
            r.CreatedAt,
            Creator = r.Creator?.UserName,
            ActiveMembers = r.Members.Count(m => m.IsActive),
            MessagesCount = r.Messages.Count,
            Members = r.Members.Where(m => m.IsActive).Select(m => new
            {
                m.User?.UserName,
                m.JoinedAt
            }).ToList(),
            RecentMessages = r.Messages.OrderByDescending(m => m.Time).Take(5).Select(m => new
            {
                Sender = m.Sender?.UserName,
                m.Content,
                m.Time
            }).ToList()
        });

        return Ok(result);
    }

    [HttpGet("messages")]
    public async Task<IActionResult> GetAllMessages()
    {
        var messages = await _context.Messages
            .Include(m => m.Sender)
            .Include(m => m.Room)
            .OrderByDescending(m => m.Time)
            .Take(50)
            .ToListAsync();

        var result = messages.Select(m => new
        {
            m.Id,
            m.Content,
            m.Time,
            m.Type,
            Sender = m.Sender?.UserName,
            Room = m.Room?.Name,
            RoomId = m.RoomId
        });

        return Ok(result);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _context.Users.ToListAsync();
        
        var result = users.Select(u => new
        {
            u.Id,
            u.UserName,
            u.Email,
            u.EmailConfirmed
        });

        return Ok(result);
    }

    [HttpGet("room-members")]
    public async Task<IActionResult> GetRoomMembers()
    {
        var members = await _context.RoomMembers
            .Include(rm => rm.User)
            .Include(rm => rm.Room)
            .ToListAsync();

        var result = members.Select(rm => new
        {
            rm.Id,
            Room = rm.Room?.Name,
            User = rm.User?.UserName,
            rm.JoinedAt,
            rm.IsActive
        });

        return Ok(result);
    }

    [HttpGet("room/{roomId}")]
    public async Task<IActionResult> GetRoomDetails(int roomId)
    {
        var room = await _context.Rooms
            .Include(r => r.Creator)
            .Include(r => r.Members.Where(m => m.IsActive))
                .ThenInclude(m => m.User)
            .Include(r => r.Messages.OrderBy(m => m.Time))
                .ThenInclude(m => m.Sender)
            .FirstOrDefaultAsync(r => r.Id == roomId);

        if (room == null)
            return NotFound();

        var result = new
        {
            room.Id,
            room.Name,
            room.Description,
            room.CreatedAt,
            Creator = room.Creator?.UserName,
            Members = room.Members.Where(m => m.IsActive).Select(m => new
            {
                m.User?.UserName,
                m.User?.Email,
                m.JoinedAt
            }).ToList(),
            Messages = room.Messages.Select(m => new
            {
                m.Id,
                m.Content,
                m.Time,
                m.Type,
                Sender = m.Sender?.UserName
            }).ToList()
        };

        return Ok(result);
    }
}
#endif