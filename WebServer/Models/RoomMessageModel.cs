using System.ComponentModel.DataAnnotations;

namespace WebServer.Models;

public class RoomMessageModel
{
    public int Id { get; set; }
    public DateTime Time { get; set; }
    public int Type { get; set; }
    public string SenderId { get; set; } = string.Empty; // 改为 string 以匹配 Identity 用户 ID
    public string SenderName { get; set; } = string.Empty; // 添加发送者名称字段
    public string? Content { get; set; }
}
