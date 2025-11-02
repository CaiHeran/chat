namespace WebServer.Modules.Gomoku.Models;

/// <summary>
/// 前端传输使用的落子信息 DTO（用于 JSON 序列化，配合 camelCase）
/// </summary>
public class MoveDto
{
    public int X { get; set; }
    public int Y { get; set; }
    public string PlayerId { get; set; } = string.Empty;
}
