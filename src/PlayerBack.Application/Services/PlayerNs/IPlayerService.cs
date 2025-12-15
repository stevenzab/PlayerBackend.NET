using PlayerBack.Domain.Dtos;
using PlayerBack.Domain.Models;

namespace PlayerBack.Application.Services.PlayerNs
{
    public interface IPlayerService
    {
        Task<IList<PlayerDto>> GetPlayersAsync(CancellationToken cancellationToken);
        Task<PlayerDto> GetPlayerByIdAsync(int id, CancellationToken cancellationToken);

        Task<PlayerDto> CreatePlayerAsync(PlayerDto player);

        Task<StatisticsModel> GetStatisticsAsync(CancellationToken cancellationToken);
        CountryWinRatioModel ComputeCountryWithHighestWinRatio(IEnumerable<Player> players);
        double ComputeAverageBmi(IEnumerable<Player> players);
        double ComputeMedianHeight(IEnumerable<Player> players);
    }
}