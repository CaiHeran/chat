using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WebServer.Modules.Gomoku.Services;

namespace WebServer.Modules.Gomoku.Pages;

[Authorize]
public class GomokuPageModel : PageModel
{
    private readonly IGomokuService _gomokuService;
    private readonly ILogger<GomokuPageModel> _logger;

    public string? GameId { get; set; }
    public string? CurrentUserId { get; set; }

    public GomokuPageModel(IGomokuService gomokuService, ILogger<GomokuPageModel> logger)
    {
        _gomokuService = gomokuService;
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync(string gameId)
    {
        if (string.IsNullOrEmpty(gameId))
        {
            return NotFound();
        }

        CurrentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(CurrentUserId))
        {
            return Unauthorized();
        }

        var gameState = await _gomokuService.GetGameStateAsync(gameId);
        if (gameState == null)
        {
            return NotFound("对局不存在");
        }

        GameId = gameId;
        _logger.LogInformation($"用户 {CurrentUserId} 访问对局 {gameId}");

        return Page();
    }
}
