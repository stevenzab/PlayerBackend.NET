using PlayerBack.Application.Services.PlayerNs.DataAccess;
using PlayerBack.Domain.Dtos;
using PlayerBack.Domain.Mapping;
using PlayerBack.Domain.Models;

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

        public async Task<PlayerDto> GetPlayerByIdAsync(int id, CancellationToken cancellationToken)
        {
            var players = await playerDataAccess.GetPlayerByIdAsync(id, cancellationToken);

            var playerDto = players.FirstOrDefault()?.MapToDto();

            return playerDto;
        }

        public async Task<PlayerDto> CreatePlayerAsync(PlayerDto dto)
        {
            var player = new Player
            {
                PlayerId = dto.PlayerId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                ShortName = dto.ShortName,
                Sex = dto.Sex,
                Picture = dto.Picture,
                Country = new Country
                {
                    Picture = dto.Country?.Picture ?? string.Empty,
                    Code = dto.Country?.Code ?? string.Empty
                },
                Data = new PlayerData
                {
                    Rank = dto.Data?.Rank ?? 0,
                    Points = dto.Data?.Points ?? 0,
                    Weight = dto.Data?.Weight ?? 0,
                    Height = dto.Data?.Height ?? 0,
                    Age = dto.Data?.Age ?? 0,
                    Last = dto.Data?.Last ?? new List<int>()
                }
            };

            await playerDataAccess.CreatePlayerAsync(player);

            dto.PlayerId = player.PlayerId;

            return dto;
        }

        public async Task<StatisticsModel> GetStatisticsAsync(CancellationToken cancellationToken)
        {
            var players = await playerDataAccess.GetPlayersAsync(cancellationToken);
            if (players == null || players.Count == 0)
                return null;

            var ratioModel = ComputeCountryWithHighestWinRatio(players);
            var avgBmi = ComputeAverageBmi(players);
            var medianHeight = ComputeMedianHeight(players);

            return new StatisticsModel
            {
                CountryCodeWithHighestWinRatio = ratioModel.CountryCode,
                HighestWinRatio = Math.Round(ratioModel.Ratio, 4),
                AverageBmi = Math.Round(avgBmi, 2),
                MedianHeight = Math.Round(medianHeight, 2)
            };
        }

        private static CountryWinRatioModel ComputeCountryWithHighestWinRatio(IEnumerable<Player> players)
        {
            var countryStats = players
                .Where(p => !string.IsNullOrWhiteSpace(p.Country?.Code))
                .Select(p => new
                {
                    Code = p.Country?.Code,
                    Wins = p.Data?.Last?.Count(r => r == 1) ?? 0,
                    Matches = p.Data?.Last?.Count ?? 0
                })
                .GroupBy(x => x.Code)
                .Select(g => new
                {
                    Code = g.Key,
                    Wins = g.Sum(x => x.Wins),
                    Matches = g.Sum(x => x.Matches)
                })
                .Select(x => new CountryWinRatioModel
                {
                    CountryCode = x.Code,
                    Ratio = x.Matches > 0 ? (double)x.Wins / x.Matches : 0.0
                })
                .OrderByDescending(x => x.Ratio)
                .FirstOrDefault();

            return countryStats ?? new CountryWinRatioModel { CountryCode = null, Ratio = 0.0 };
        }

        private static double ComputeAverageBmi(IEnumerable<Player> players)
        {
            var bmis = players
                .Select(p =>
                {
                    var w = p.Data?.Weight ?? 0;
                    var h = p.Data?.Height ?? 0;
                    return (w > 0 && h > 0) ? (double?)(w / Math.Pow(h / 100.0, 2)) : null;
                })
                .Where(b => b.HasValue)
                .Select(b => b.Value);

            return bmis.Any() ? bmis.Average() : 0.0;
        }

        private static double ComputeMedianHeight(IEnumerable<Player> players)
        {
            var heights = players
                .Select(p => p.Data?.Height ?? 0)
                .Where(h => h > 0)
                .OrderBy(h => h)
                .ToArray();

            if (heights.Length == 0)
                return 0.0;

            int n = heights.Length;
            if (n % 2 == 1)
                return heights[n / 2];

            return (heights[(n / 2) - 1] + heights[n / 2]) / 2.0;
        }

    }
}