using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebServer.Modules.Gomoku.Data;

/// <summary>
/// 五子棋落子记录 EF Core 实体
/// </summary>
[Table("GomokuMoves")]
public class GomokuMoveEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string GameId { get; set; } = string.Empty;

    [Required]
    public string PlayerId { get; set; } = string.Empty;

    [Required]
    public int X { get; set; }

    [Required]
    public int Y { get; set; }

    [Required]
    public int MoveNumber { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    // 导航属性
    [ForeignKey(nameof(GameId))]
    public GomokuGameEntity? Game { get; set; }
}
