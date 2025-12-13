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
    }
}