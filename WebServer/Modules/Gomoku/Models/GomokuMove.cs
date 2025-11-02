namespace WebServer.Modules.Gomoku.Models;

/// <summary>
/// 五子棋的单步落子记录
/// </summary>
public class GomokuMove
{
    /// <summary>落子ID</summary>
    public int Id { get; set; }
    
    /// <summary>所属对局ID</summary>
    public string GameId { get; set; } = string.Empty;
    
    /// <summary>落子玩家ID</summary>
    public string PlayerId { get; set; } = string.Empty;
    
    /// <summary>棋子行坐标 (0-based)</summary>
    public int X { get; set; }
    
    /// <summary>棋子列坐标 (0-based)</summary>
    public int Y { get; set; }
  
    /// <summary>第几手 (1-based)</summary>
    public int MoveNumber { get; set; }
    
    /// <summary>落子时间</summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    /// <summary>导航属性：所属对局</summary>
    public GomokuGame? Game { get; set; }
}
