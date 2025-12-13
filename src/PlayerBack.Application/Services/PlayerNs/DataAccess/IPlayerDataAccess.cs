using PlayerBack.Domain.Dtos;
using PlayerBack.Domain.Models;

namespace PlayerBack.Application.Services.PlayerNs.DataAccess
{
    public interface IPlayerDataAccess
    {
        Task<IList<Player>> GetPlayersAsync(CancellationToken cancellationToken);
        Task<IList<Player>> GetPlayerByIdAsync(int id, CancellationToken cancellationToken);

        Task CreatePlayerAsync(Player player);
    }
}