using PlayerBack.Domain.Models;

namespace PlayerBack.Application.Services.PlayerNs.DataAccess
{
    public interface IPlayerDataAccess
    {
        Task<IList<Player>> GetPlayersAsync(CancellationToken cancellationToken);
    }
}