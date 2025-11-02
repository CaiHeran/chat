using WebServer.Modules.Gomoku.Models;

namespace WebServer.Modules.Gomoku.Services;

/// <summary>
/// 五子棋服务公共接口
/// </summary>
public interface IGomokuService
{
    /// <summary>
    /// 创建新的五子棋对局
    /// </summary>
    Task<GomokuGame> CreateGameAsync(int roomId, string creatorId);
    
    /// <summary>
    /// 加入一个对局（成为白方）
    /// </summary>
    Task<GomokuGame?> JoinGameAsync(string gameId, string playerId);
 
    /// <summary>
    /// 开始对局（由房主或双方之一确认）
    /// </summary>
    Task<bool> StartGameAsync(string gameId, string playerId);
    
    /// <summary>
    /// 落子操作
    /// </summary>
    Task<bool> MakeMoveAsync(string gameId, string playerId, int x, int y);
    
    /// <summary>
    /// 获取对局当前状态
    /// </summary>
    Task<GameState?> GetGameStateAsync(string gameId);

    /// <summary>
    /// 获取某房间的活动对局（等待中或进行中，若同时存在，取最新一局）
    /// </summary>
    Task<GameState?> GetActiveGameInRoomAsync(int roomId);
    
    /// <summary>
    /// 玩家认输
    /// </summary>
    Task<bool> ResignAsync(string gameId, string playerId);
}
