namespace WebServer.Modules.Gomoku.Models;

/// <summary>
/// 棋局规则常量和配置
/// </summary>
public static class GameRules
{
    /// <summary>默认棋盘大小</summary>
    public const int DefaultBoardSize = 15;
    
    /// <summary>获胜需要的连子数</summary>
    public const int WinningLineLength = 5;
    
    /// <summary>棋盘最小尺寸</summary>
    public const int MinBoardSize = 5;
    
    /// <summary>棋盘最大尺寸</summary>
    public const int MaxBoardSize = 19;
}
