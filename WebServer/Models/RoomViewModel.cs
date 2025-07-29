using WebServer.Areas.Identity.Data;

namespace WebServer.Models;

public class RoomViewModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public List<AppUser> Members { get; set; } = [];
    public List<RoomMessageModel> Messages { get; set; } = [];
}
