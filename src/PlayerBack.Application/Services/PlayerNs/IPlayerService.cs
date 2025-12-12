using PlayerBack.Domain.Dtos;

namespace PlayerBack.Application.Services.PlayerNs
{
    public interface IPlayerService
    {
        Task<IList<PlayerDto>> GetPlayersAsync(CancellationToken cancellationToken);
    }
}