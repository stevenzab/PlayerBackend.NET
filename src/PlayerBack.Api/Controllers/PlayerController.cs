using Microsoft.AspNetCore.Mvc;
using PlayerBack.Application.Services.PlayerNs;

namespace PlayerBack.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService playerService;

        public PlayerController(IPlayerService playerService)
        {
            this.playerService = playerService;
        }

        [HttpGet("Players")]
        public async Task<IActionResult> GetPlayersAsync(CancellationToken cancellationToken)
        {
            var result = await playerService.GetPlayersAsync(cancellationToken);

            if (result == null || result.Count == 0)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("Player/{id}")]
        public async Task<IActionResult> GetPlayerByIdAsync(string id, CancellationToken cancellationToken)
        {
            var result = await playerService.GetPlayerByIdAsync(id, cancellationToken);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}