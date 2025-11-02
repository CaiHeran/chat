using Microsoft.EntityFrameworkCore;
using WebServer.Data;
using WebServer.Modules.Gomoku.Data;
using WebServer.Modules.Gomoku.Models;

namespace WebServer.Modules.Gomoku.Services;

/// <summary>
/// 五子棋核心服务实现
/// </summary>
public class GomokuService : IGomokuService
{
    private readonly ChatDbContext _context;
    private readonly GomokuRuleEngine _ruleEngine;

    // 内存缓存：gameId -> (GameState, SemaphoreSlim 用于并发锁)
    private static readonly Dictionary<string, (GameState State, SemaphoreSlim Semaphore)> GameStates = new();
    private static readonly object LockObj = new();

    public GomokuService(ChatDbContext context, GomokuRuleEngine ruleEngine)
    {
        _context = context;
        _ruleEngine = ruleEngine;
    }

    /// <summary>
    /// 创建新对局
    /// </summary>
    public async Task<GomokuGame> CreateGameAsync(int roomId, string creatorId)
    {
        // 房间最多一局：若存在未结束对局则直接返回该对局
        var existing = await _context.Set<GomokuGameEntity>()
            .Where(g => g.RoomId == roomId && g.Status != (int)GameStatus.Finished)
            .OrderByDescending(g => g.UpdatedAt)
            .FirstOrDefaultAsync();
        if (existing != null)
        {
            return await MapEntityToGameAsync(existing);
        }

        var gameId = Guid.NewGuid().ToString();

        var entity = new GomokuGameEntity {
            Id = gameId,
            RoomId = roomId,
            BlackPlayerId = creatorId,  // 创建者为黑方（先手）
            Status = (int)GameStatus.Waiting,
            BoardSize = GameRules.DefaultBoardSize,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Add(entity);
        await _context.SaveChangesAsync();

        // 初始化内存状态
        var gameState = new GameState {
            Id = gameId,
            BoardSize = GameRules.DefaultBoardSize,
            BlackPlayerId = creatorId,
            Status = GameStatus.Waiting,
            Moves = new()
        };

        lock (LockObj)
        {
            GameStates[gameId] = (gameState, new SemaphoreSlim(1, 1));
        }

        // 转换并返回领域模型
        return await MapEntityToGameAsync(entity);
    }

    /// <summary>
    /// 加入对局（成为白方）
    /// </summary>
    public async Task<GomokuGame?> JoinGameAsync(string gameId, string playerId)
    {
        var entity = await _context.Set<GomokuGameEntity>()
                    .FirstOrDefaultAsync(g => g.Id == gameId);

        if (entity == null || entity.Status != (int)GameStatus.Waiting)
            return null;

        if (!string.IsNullOrEmpty(entity.WhitePlayerId))
            return null;  // 白方已有人

        entity.WhitePlayerId = playerId;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.Update(entity);
        await _context.SaveChangesAsync();

        // 更新内存状态
        lock (LockObj)
        {
            if (GameStates.TryGetValue(gameId, out var state))
            {
                state.State.WhitePlayerId = playerId;
                state.State.Status = GameStatus.Waiting;
            }
        }

        return await MapEntityToGameAsync(entity);
    }

    /// <summary>
    /// 开始对局
    /// </summary>
    public async Task<bool> StartGameAsync(string gameId, string playerId)
    {
        var entity = await _context.Set<GomokuGameEntity>()
     .FirstOrDefaultAsync(g => g.Id == gameId);

        if (entity == null)
            return false;

        // 只有黑方可以开始
        if (entity.BlackPlayerId != playerId)
            return false;

        if (string.IsNullOrEmpty(entity.WhitePlayerId))
            return false;  // 还没有白方

        entity.Status = (int)GameStatus.Playing;
        entity.CurrentTurnPlayerId = entity.BlackPlayerId;  // 黑方先手
        entity.UpdatedAt = DateTime.UtcNow;

        _context.Update(entity);
        await _context.SaveChangesAsync();

        // 更新内存状态
        lock (LockObj)
        {
            if (GameStates.TryGetValue(gameId, out var state))
            {
                state.State.Status = GameStatus.Playing;
                state.State.CurrentTurnPlayerId = entity.BlackPlayerId;
            }
        }

        return true;
    }

    /// <summary>
    /// 落子
    /// </summary>
    public async Task<bool> MakeMoveAsync(string gameId, string playerId, int x, int y)
    {
        // 获取或创建信号量
        SemaphoreSlim semaphore;
        lock (LockObj)
        {
            if (!GameStates.TryGetValue(gameId, out var state))
                return false;
            semaphore = state.Semaphore;
        }

        // 加锁以防并发冲突
        await semaphore.WaitAsync();
        try
        {
            var entity = await _context.Set<GomokuGameEntity>()
     .Include(g => g.Moves)
                .FirstOrDefaultAsync(g => g.Id == gameId);

            if (entity == null || entity.Status != (int)GameStatus.Playing)
                return false;

            // 验证轮到该玩家
            if (entity.CurrentTurnPlayerId != playerId)
                return false;

            // 验证坐标合法
            if (x < 0 || x >= entity.BoardSize || y < 0 || y >= entity.BoardSize)
                return false;

            // 验证该位置未被占用
            if (entity.Moves.Any(m => m.X == x && m.Y == y))
                return false;

            // 创建落子记录
            var moveEntity = new GomokuMoveEntity {
                GameId = gameId,
                PlayerId = playerId,
                X = x,
                Y = y,
                MoveNumber = entity.Moves.Count + 1,
                Timestamp = DateTime.UtcNow
            };

            _context.Add(moveEntity);

            // 更新内存状态以便快速检查胜负
            lock (LockObj)
            {
                if (GameStates.TryGetValue(gameId, out var state))
                {
                    state.State.Moves.Add(new MoveDto { X = x, Y = y, PlayerId = playerId });
                }
            }

            // 检查是否获胜
            var movesList = entity.Moves.Select(m => (m.X, m.Y, m.PlayerId)).ToList();
            movesList.Add((x, y, playerId));

            bool hasWon = _ruleEngine.IsWin(movesList, x, y, playerId);

            if (hasWon)
            {
                // 对局结束
                entity.Status = (int)GameStatus.Finished;
                entity.WinnerId = playerId;
                entity.FinishedAt = DateTime.UtcNow;
                entity.UpdatedAt = DateTime.UtcNow;

                lock (LockObj)
                {
                    if (GameStates.TryGetValue(gameId, out var state))
                    {
                        state.State.Status = GameStatus.Finished;
                        state.State.WinnerId = playerId;
                    }
                }
            }
            else
            {
                // 切换回合
                entity.CurrentTurnPlayerId = playerId == entity.BlackPlayerId ?
                        entity.WhitePlayerId : entity.BlackPlayerId;
                entity.UpdatedAt = DateTime.UtcNow;

                lock (LockObj)
                {
                    if (GameStates.TryGetValue(gameId, out var state))
                    {
                        state.State.CurrentTurnPlayerId = entity.CurrentTurnPlayerId;
                    }
                }
            }

            _context.Update(entity);
            await _context.SaveChangesAsync();

            return true;
        }
        finally
        {
            semaphore.Release();
        }
    }

    /// <summary>
    /// 获取对局状态
    /// </summary>
    public async Task<GameState?> GetGameStateAsync(string gameId)
    {
        lock (LockObj)
        {
            if (GameStates.TryGetValue(gameId, out var state))
            {
                return state.State;
            }
        }

        // 如果内存中没有，从数据库加载
        var entity = await _context.Set<GomokuGameEntity>()
                .Include(g => g.Moves)
                .FirstOrDefaultAsync(g => g.Id == gameId);

        if (entity == null)
            return null;

        var gameState = new GameState {
            Id = entity.Id,
            BoardSize = entity.BoardSize,
            BlackPlayerId = entity.BlackPlayerId,
            WhitePlayerId = entity.WhitePlayerId,
            Status = (GameStatus)entity.Status,
            CurrentTurnPlayerId = entity.CurrentTurnPlayerId,
            WinnerId = entity.WinnerId,
            Moves = entity.Moves.Select(m => new MoveDto { X = m.X, Y = m.Y, PlayerId = m.PlayerId }).ToList()
        };

        lock (LockObj)
        {
            if (!GameStates.ContainsKey(gameId))
            {
                GameStates[gameId] = (gameState, new SemaphoreSlim(1, 1));
            }
        }

        return gameState;
    }

    /// <summary>
    /// 获取某房间的活动对局（等待中或进行中）
    /// </summary>
    public async Task<GameState?> GetActiveGameInRoomAsync(int roomId)
    {
        var entity = await _context.Set<GomokuGameEntity>()
            .Include(g => g.Moves)
            .Where(g => g.RoomId == roomId && g.Status != (int)GameStatus.Finished)
            .OrderByDescending(g => g.UpdatedAt)
            .FirstOrDefaultAsync();

        if (entity == null) return null;

        return new GameState {
            Id = entity.Id,
            BoardSize = entity.BoardSize,
            BlackPlayerId = entity.BlackPlayerId,
            WhitePlayerId = entity.WhitePlayerId,
            Status = (GameStatus)entity.Status,
            CurrentTurnPlayerId = entity.CurrentTurnPlayerId,
            WinnerId = entity.WinnerId,
            Moves = entity.Moves.Select(m => new MoveDto { X = m.X, Y = m.Y, PlayerId = m.PlayerId }).ToList()
        };
    }

    /// <summary>
    /// 认输
    /// </summary>
    public async Task<bool> ResignAsync(string gameId, string playerId)
    {
        var entity = await _context.Set<GomokuGameEntity>()
           .FirstOrDefaultAsync(g => g.Id == gameId);

        if (entity == null || entity.Status != (int)GameStatus.Playing)
            return false;

        if (entity.BlackPlayerId != playerId && entity.WhitePlayerId != playerId)
            return false;

        // 对方获胜
        entity.Status = (int)GameStatus.Finished;
        entity.WinnerId = entity.BlackPlayerId == playerId ?
                entity.WhitePlayerId : entity.BlackPlayerId;
        entity.FinishedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.Update(entity);
        await _context.SaveChangesAsync();

        lock (LockObj)
        {
            if (GameStates.TryGetValue(gameId, out var state))
            {
                state.State.Status = GameStatus.Finished;
                state.State.WinnerId = entity.WinnerId;
            }
        }

        return true;
    }

    /// <summary>
    /// 辅助方法：将 Entity 转换为领域模型
    /// </summary>
    private async Task<GomokuGame> MapEntityToGameAsync(GomokuGameEntity entity)
    {
        return new GomokuGame {
            Id = entity.Id,
            RoomId = entity.RoomId,
            BlackPlayerId = entity.BlackPlayerId,
            WhitePlayerId = entity.WhitePlayerId,
            Status = (GameStatus)entity.Status,
            BoardSize = entity.BoardSize,
            CurrentTurnPlayerId = entity.CurrentTurnPlayerId,
            WinnerId = entity.WinnerId,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            FinishedAt = entity.FinishedAt,
            Moves = entity.Moves?.Select(m => new GomokuMove {
                Id = m.Id,
                GameId = m.GameId,
                PlayerId = m.PlayerId,
                X = m.X,
                Y = m.Y,
                MoveNumber = m.MoveNumber,
                Timestamp = m.Timestamp
            }).ToList() ?? []
        };
    }
}
