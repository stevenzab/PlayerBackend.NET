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
    }
}