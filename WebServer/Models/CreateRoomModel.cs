using System.ComponentModel.DataAnnotations;

namespace WebServer.Models;

public class CreateRoomModel
{
    [Required(ErrorMessage = "房间名称是必填的")]
    [StringLength(100, ErrorMessage = "房间名称不能超过100个字符")]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(500, ErrorMessage = "房间描述不能超过500个字符")]
    public string? Description { get; set; }
}
