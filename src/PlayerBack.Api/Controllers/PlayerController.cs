using Microsoft.AspNetCore.Mvc;
using PlayerBack.Application.Services.PlayerNs;
using PlayerBack.Domain.Dtos;
using PlayerBack.Domain.Models;

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

        [HttpGet("Player/{id}", Name = "GetPlayerById")]
        public async Task<IActionResult> GetPlayerByIdAsync(string id, CancellationToken cancellationToken)
        {
            var result = await playerService.GetPlayerByIdAsync(id, cancellationToken);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost("CreatePlayer")]
        public async Task<IActionResult> CreatePlayerAsync([FromBody] PlayerDto player)
        {
            var createdPlayer = await playerService.CreatePlayerAsync(player);

            return CreatedAtRoute(
                "GetPlayerById",
                new { id = createdPlayer.PlayerId },
                createdPlayer);
        }

        [HttpGet("GetStatistics")]
        public async Task<ActionResult<StatisticsModel>> GetPlayerStatisticsAsync(CancellationToken cancellationToken)
        {
            var stats = await playerService.GetStatisticsAsync(cancellationToken);
            if (stats == null)
                return NotFound();

            return Ok(stats);
        }
    }
}