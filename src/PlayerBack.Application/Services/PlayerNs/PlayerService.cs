using PlayerBack.Application.Services.PlayerNs.DataAccess;
using PlayerBack.Domain.Dtos;
using PlayerBack.Domain.Mapping;

namespace PlayerBack.Application.Services.PlayerNs
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerDataAccess playerDataAccess;

        public PlayerService(IPlayerDataAccess playerDataAccess)
        {
            this.playerDataAccess = playerDataAccess;
        }

        public async Task<IList<PlayerDto>> GetPlayersAsync(CancellationToken cancellationToken)
        {
            var players = await playerDataAccess.GetPlayersAsync(cancellationToken);

            var playerDtos = players.Select(player => player.MapToDto()).ToList();

            return playerDtos;
        }

        public async Task<PlayerDto> GetPlayerByIdAsync(string id, CancellationToken cancellationToken)
        {
            var players = await playerDataAccess.GetPlayerByIdAsync(id, cancellationToken);

            var playerDto = players.FirstOrDefault()?.MapToDto();

            return playerDto;
        }
    }
}