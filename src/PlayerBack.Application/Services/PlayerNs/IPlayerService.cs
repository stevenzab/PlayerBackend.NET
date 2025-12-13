using PlayerBack.Domain.Dtos;
using PlayerBack.Domain.Models;

namespace PlayerBack.Application.Services.PlayerNs
{
    public interface IPlayerService
    {
        Task<IList<PlayerDto>> GetPlayersAsync(CancellationToken cancellationToken);
        Task<PlayerDto> GetPlayerByIdAsync(string id, CancellationToken cancellationToken);

        Task CreatePlayerAsync(PlayerDto player);
    }
}