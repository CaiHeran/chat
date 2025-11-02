using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using WebServer.Modules.Gomoku.Models;
using WebServer.Modules.Gomoku.Services;

namespace WebServer.Modules.Gomoku.Hubs;

/// <summary>
/// 五子棋实时通信 Hub
/// </summary>
[Authorize]
public class GomokuHub : Hub
{
    private readonly IGomokuService _gomokuService;
    private readonly ILogger<GomokuHub> _logger;

    private const string GroupPrefix = "game_";

    public GomokuHub(IGomokuService gomokuService, ILogger<GomokuHub> logger)
    {
        _gomokuService = gomokuService;
        _logger = logger;
    }

    private string GetUserIdOrConnection()
    {
        return Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? Context.UserIdentifier
            ?? Context.ConnectionId;
    }

    public override Task OnConnectedAsync()
    {
        var uid = GetUserIdOrConnection();
        _logger.LogInformation("GomokuHub connected: {ConnectionId}, user: {User}", Context.ConnectionId, uid);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        if (exception != null)
        {
            _logger.LogWarning(exception, $"GomokuHub client disconnected with error: {Context.ConnectionId}");
        }
        else
        {
            _logger.LogInformation($"GomokuHub client disconnected: {Context.ConnectionId}");
        }
        return base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// 加入对局（加入 SignalR 分组，并推送当前状态）
    /// </summary>
    public async Task JoinGame(string gameId)
    {
        try
        {
            var userId = GetUserIdOrConnection();
            _logger.LogInformation("JoinGame called. gameId={GameId}, user={User}", gameId, userId);
            await Groups.AddToGroupAsync(Context.ConnectionId, $"{GroupPrefix}{gameId}");

            var state = await _gomokuService.GetGameStateAsync(gameId);
            if (state != null)
            {
                _logger.LogInformation("Pushing state to caller. gameId={GameId}, black={Black}, white={White}, status={Status}, moves={Moves}", state.Id, state.BlackPlayerId, state.WhitePlayerId, state.Status, state.Moves.Count);
                await Clients.Caller.SendAsync("GameStateUpdated", state);
            }
            else
            {
                _logger.LogWarning("JoinGame: state not found for {GameId}", gameId);
                await Clients.Caller.SendAsync("Error", "对局不存在");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "加入对局出错");
            await Clients.Caller.SendAsync("Error", ex.Message);
        }
    }

    /// <summary>
    /// 离开对局
    /// </summary>
    public async Task LeaveGame(string gameId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"{GroupPrefix}{gameId}");
    }

    /// <summary>
    /// 开始对局
    /// </summary>
    public async Task StartGame(string gameId)
    {
        try
        {
            var userId = GetUserIdOrConnection();
            var success = await _gomokuService.StartGameAsync(gameId, userId);

            if (success)
            {
                var state = await _gomokuService.GetGameStateAsync(gameId);
                await Clients.Group($"{GroupPrefix}{gameId}").SendAsync("GameStateUpdated", state);
            }
            else
            {
                await Clients.Caller.SendAsync("Error", "无法开始对局");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "开始对局出错");
            await Clients.Caller.SendAsync("Error", ex.Message);
        }
    }

    /// <summary>
    /// 落子
    /// </summary>
    public async Task MakeMove(string gameId, int x, int y)
    {
        try
        {
            var userId = GetUserIdOrConnection();
            var success = await _gomokuService.MakeMoveAsync(gameId, userId, x, y);

            if (success)
            {
                var state = await _gomokuService.GetGameStateAsync(gameId);
                await Clients.Group($"{GroupPrefix}{gameId}").SendAsync("GameStateUpdated", state);

                if (state?.Status == GameStatus.Finished)
                {
                    await Clients.Group($"{GroupPrefix}{gameId}").SendAsync("GameOver", new { 
                        WinnerId = state.WinnerId, 
                        BlackPlayerId = state.BlackPlayerId, 
                        WhitePlayerId = state.WhitePlayerId 
                    });
                }
            }
            else
            {
                await Clients.Caller.SendAsync("Error", "落子无效");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "落子出错");
            await Clients.Caller.SendAsync("Error", ex.Message);
        }
    }

    /// <summary>
    /// 认输
    /// </summary>
    public async Task Resign(string gameId)
    {
        try
        {
            var userId = GetUserIdOrConnection();
            var success = await _gomokuService.ResignAsync(gameId, userId);

            if (success)
            {
                var state = await _gomokuService.GetGameStateAsync(gameId);
                await Clients.Group($"{GroupPrefix}{gameId}").SendAsync("GameOver", new {
                    WinnerId = state?.WinnerId,
                    BlackPlayerId = state?.BlackPlayerId,
                    WhitePlayerId = state?.WhitePlayerId
                });
            }
            else
            {
                await Clients.Caller.SendAsync("Error", "认输失败");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "认输出错");
            await Clients.Caller.SendAsync("Error", ex.Message);
        }
    }

    /// <summary>
    /// 请求当前状态（用于重连）
    /// </summary>
    public async Task RequestState(string gameId)
    {
        try
        {
            var state = await _gomokuService.GetGameStateAsync(gameId);
            if (state != null)
            {
                await Clients.Caller.SendAsync("GameStateUpdated", state);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "请求状态出错");
            await Clients.Caller.SendAsync("Error", ex.Message);
        }
    }
}
