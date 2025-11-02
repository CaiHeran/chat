namespace WebServer.Modules.Gomoku.Models;

/// <summary>
/// 五子棋对局状态
/// </summary>
public enum GameStatus
{
  /// <summary>等待玩家加入</summary>
    Waiting = 0,
    
    /// <summary>对局进行中</summary>
    Playing = 1,
    
    /// <summary>对局已结束</summary>
    Finished = 2
}
