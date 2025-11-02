using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebServer.Modules.Gomoku.Services;
using Microsoft.AspNetCore.SignalR;
using WebServer.Modules.Gomoku.Hubs;

namespace WebServer.Controllers;

/// <summary>
/// 五子棋游戏控制器
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class GomokuController : ControllerBase
{
    private readonly IGomokuService _gomokuService;
    private readonly ILogger<GomokuController> _logger;
    private readonly IHubContext<GomokuHub> _hubContext;

    public GomokuController(IGomokuService gomokuService, ILogger<GomokuController> logger, IHubContext<GomokuHub> hubContext)
    {
        _gomokuService = gomokuService;
        _logger = logger;
        _hubContext = hubContext;
    }

    /// <summary>
    /// 在指定房间创建五子棋对局
    /// </summary>
    [HttpPost("create")]
    public async Task<IActionResult> CreateGame([FromBody] CreateGameRequest request)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            if (request.RoomId <= 0)
            {
                return BadRequest("无效的房间ID");
            }

            var game = await _gomokuService.CreateGameAsync(request.RoomId, userId);
            _logger.LogInformation($"用户 {userId} 在房间 {request.RoomId} 创建了五子棋对局 {game.Id}");

            return Ok(new { gameId = game.Id, message = "对局创建成功" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "创建五子棋对局出错");
            return StatusCode(500, new { message = ex.Message });
        }
    }

    /// <summary>
    /// 加入五子棋对局
    /// </summary>
    [HttpPost("join")]
    public async Task<IActionResult> JoinGame([FromBody] JoinGameRequest request)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            if (string.IsNullOrEmpty(request.GameId))
            {
                return BadRequest("无效的对局ID");
            }

            var game = await _gomokuService.JoinGameAsync(request.GameId, userId);
            if (game == null)
            {
                return BadRequest("无法加入对局（可能对局不存在或已满人）");
            }

            _logger.LogInformation($"用户 {userId} 加入了对局 {request.GameId}");

            // Notify existing clients in the game group so they see the updated state immediately
            try
            {
                var state = await _gomokuService.GetGameStateAsync(game.Id);
                if (state != null)
                {
                    await _hubContext.Clients.Group($"game_{game.Id}").SendAsync("GameStateUpdated", state);
                    // also notify that a user joined so clients can proactively refresh if needed
                    await _hubContext.Clients.Group($"game_{game.Id}").SendAsync("UserJoined", userId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "向 Hub 广播加入事件时出错，但不影响加入流程");
            }

            return Ok(new { gameId = game.Id, message = "加入对局成功" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "加入对局出错");
            return StatusCode(500, new { message = ex.Message });
        }
    }

    /// <summary>
    /// 获取对局状态
    /// </summary>
    [HttpGet("{gameId}")]
    public async Task<IActionResult> GetGameState(string gameId)
    {
        try
        {
            if (string.IsNullOrEmpty(gameId))
            {
                return BadRequest("无效的对局ID");
            }

            var state = await _gomokuService.GetGameStateAsync(gameId);
            if (state == null)
            {
                return NotFound("对局不存在");
            }

            return Ok(state);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取对局状态出错");
            return StatusCode(500, new { message = ex.Message });
        }
    }

    /// <summary>
    /// 获取某房间的活动对局（等待中或进行中）
    /// </summary>
    [HttpGet("room/{roomId:int}/active")]
    public async Task<IActionResult> GetActiveGameForRoom([FromRoute] int roomId)
    {
        try
        {
            if (roomId <= 0)
            {
                return BadRequest("无效的房间ID");
            }

            var state = await _gomokuService.GetActiveGameInRoomAsync(roomId);
            if (state == null)
            {
                return Ok(new { hasGame = false });
            }

            return Ok(new { hasGame = true, state });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取房间活动对局出错");
            return StatusCode(500, new { message = ex.Message });
        }
    }
}

/// <summary>
/// 创建对局请求 DTO
/// </summary>
public class CreateGameRequest
{
    public int RoomId { get; set; }
}

/// <summary>
/// 加入对局请求 DTO
/// </summary>
public class JoinGameRequest
{
    public string GameId { get; set; } = string.Empty;
}
