using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebServer.Modules.Gomoku.Data;

/// <summary>
/// 五子棋对局 EF Core 实体 (对应数据库表)
/// </summary>
[Table("GomokuGames")]
public class GomokuGameEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public int RoomId { get; set; }

    public string? BlackPlayerId { get; set; }

    public string? WhitePlayerId { get; set; }

    [Required]
    public int Status { get; set; } = 0; // GameStatus.Waiting

    [Required]
    public int BoardSize { get; set; } = 15;

    public string? CurrentTurnPlayerId { get; set; }

    public string? WinnerId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? FinishedAt { get; set; }

    // 导航属性
    public List<GomokuMoveEntity> Moves { get; set; } = [];
}
