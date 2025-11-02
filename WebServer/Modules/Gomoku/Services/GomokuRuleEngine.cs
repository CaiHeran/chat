using WebServer.Modules.Gomoku.Models;

namespace WebServer.Modules.Gomoku.Services;

/// <summary>
/// 五子棋规则判定引擎
/// </summary>
public class GomokuRuleEngine
{
    /// <summary>
    /// 判定是否获胜（检查最新落子点是否形成五连）
    /// </summary>
    public bool IsWin(List<(int X, int Y, string PlayerId)> moves, int lastX, int lastY, string lastPlayerId)
    {
        // 检查四个方向：水平、竖直、斜线1、斜线2
        return CheckHorizontal(moves, lastX, lastY, lastPlayerId)
            || CheckVertical(moves, lastX, lastY, lastPlayerId)
            || CheckDiagonalLeft(moves, lastX, lastY, lastPlayerId)
            || CheckDiagonalRight(moves, lastX, lastY, lastPlayerId);
    }

    /// <summary>
    /// 检查水平方向的连子
    /// </summary>
    private bool CheckHorizontal(List<(int X, int Y, string PlayerId)> moves, int x, int y, string playerId)
    {
        int count = 1;

        // 向左查
        for (int i = 1; i < GameRules.WinningLineLength; i++)
        {
            if (moves.Any(m => m.X == x - i && m.Y == y && m.PlayerId == playerId))
                count++;
            else
                break;
        }

        // 向右查
        for (int i = 1; i < GameRules.WinningLineLength; i++)
        {
            if (moves.Any(m => m.X == x + i && m.Y == y && m.PlayerId == playerId))
                count++;
            else
                break;
        }

        return count >= GameRules.WinningLineLength;
    }

    /// <summary>
    /// 检查竖直方向的连子
    /// </summary>
    private bool CheckVertical(List<(int X, int Y, string PlayerId)> moves, int x, int y, string playerId)
    {
        int count = 1;

        // 向上查
        for (int i = 1; i < GameRules.WinningLineLength; i++)
        {
            if (moves.Any(m => m.X == x && m.Y == y - i && m.PlayerId == playerId))
                count++;
            else
                break;
        }

        // 向下查
        for (int i = 1; i < GameRules.WinningLineLength; i++)
        {
            if (moves.Any(m => m.X == x && m.Y == y + i && m.PlayerId == playerId))
                count++;
            else
                break;
        }

        return count >= GameRules.WinningLineLength;
    }

    /// <summary>
    /// 检查左斜线 (\) 方向的连子
    /// </summary>
    private bool CheckDiagonalLeft(List<(int X, int Y, string PlayerId)> moves, int x, int y, string playerId)
    {
        int count = 1;

        // 向左上查
        for (int i = 1; i < GameRules.WinningLineLength; i++)
        {
            if (moves.Any(m => m.X == x - i && m.Y == y - i && m.PlayerId == playerId))
                count++;
            else
                break;
        }

        // 向右下查
        for (int i = 1; i < GameRules.WinningLineLength; i++)
        {
            if (moves.Any(m => m.X == x + i && m.Y == y + i && m.PlayerId == playerId))
                count++;
            else
                break;
        }

        return count >= GameRules.WinningLineLength;
    }

    /// <summary>
    /// 检查右斜线 (/) 方向的连子
    /// </summary>
    private bool CheckDiagonalRight(List<(int X, int Y, string PlayerId)> moves, int x, int y, string playerId)
    {
        int count = 1;

        // 向右上查
        for (int i = 1; i < GameRules.WinningLineLength; i++)
        {
            if (moves.Any(m => m.X == x + i && m.Y == y - i && m.PlayerId == playerId))
                count++;
            else
                break;
        }

        // 向左下查
        for (int i = 1; i < GameRules.WinningLineLength; i++)
        {
            if (moves.Any(m => m.X == x - i && m.Y == y + i && m.PlayerId == playerId))
                count++;
            else
                break;
        }

        return count >= GameRules.WinningLineLength;
    }
}
