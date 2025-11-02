namespace WebServer.Modules.Gomoku.Models;

/// <summary>
/// 五子棋对局领域模型
/// </summary>
public class GomokuGame
{
    /// <summary>对局ID (GUID)</summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    /// <summary>所属房间ID</summary>
    public int RoomId { get; set; }
 
    /// <summary>黑色方玩家ID (先手)</summary>
    public string? BlackPlayerId { get; set; }
    
    /// <summary>白色方玩家ID (后手)</summary>
    public string? WhitePlayerId { get; set; }
  
    /// <summary>对局状态</summary>
    public GameStatus Status { get; set; } = GameStatus.Waiting;
    
    /// <summary>棋盘大小 (默认15x15)</summary>
    public int BoardSize { get; set; } = GameRules.DefaultBoardSize;
    
    /// <summary>当前回合玩家ID (Black/White)</summary>
    public string? CurrentTurnPlayerId { get; set; }
    
    /// <summary>赢家玩家ID (null 表示未结束或平局)</summary>
    public string? WinnerId { get; set; }
    
    /// <summary>对局创建时间</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>对局更新时间</summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>对局结束时间</summary>
    public DateTime? FinishedAt { get; set; }
    
    /// <summary>所有落子记录</summary>
    public List<GomokuMove> Moves { get; set; } = [];
}
