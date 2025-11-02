namespace WebServer.Modules.Gomoku.Models;

/// <summary>
/// 五子棋对局状态 DTO (用于客户端传输，与 DB 实体分离)
/// </summary>
public class GameState
{
    /// <summary>对局ID</summary>
    public string Id { get; set; } = string.Empty;
    
    /// <summary>棋盘大小</summary>
    public int BoardSize { get; set; } = GameRules.DefaultBoardSize;
    
    /// <summary>黑方玩家ID</summary>
    public string? BlackPlayerId { get; set; }
    
    /// <summary>白方玩家ID</summary>
    public string? WhitePlayerId { get; set; }
    
    /// <summary>对局状态</summary>
    public GameStatus Status { get; set; } = GameStatus.Waiting;
    
    /// <summary>当前回合玩家ID</summary>
    public string? CurrentTurnPlayerId { get; set; }
  
    /// <summary>赢家玩家ID</summary>
    public string? WinnerId { get; set; }
  
    /// <summary>所有落子列表 (坐标 + 玩家)</summary>
    public List<MoveDto> Moves { get; set; } = new();
}
