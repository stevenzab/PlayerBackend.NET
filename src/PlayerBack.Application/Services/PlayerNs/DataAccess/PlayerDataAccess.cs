using MongoDB.Driver.Linq;
using PlayerBack.Domain.Dtos;
using PlayerBack.Domain.Models;
using PlayerBack.Infrastructure.Common;

namespace PlayerBack.Application.Services.PlayerNs.DataAccess
{
    public class PlayerDataAccess : IPlayerDataAccess
    {
        private readonly IBaseRepository baseRepository;

        public PlayerDataAccess(IBaseRepository baseRepository)
        {
            this.baseRepository = baseRepository;
        }

        public async Task<IList<Player>> GetPlayersAsync(CancellationToken cancellationToken)
        {
            return await baseRepository
                .AsQueryable<Player>()
                .OrderBy(p => p.Data.Rank)
                .ToListAsync(cancellationToken);
        }

        public async Task CreatePlayerAsync(Player player)
        {
             await baseRepository.AddAsync(player);
        }

        public async Task<IList<Player>> GetPlayerByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await baseRepository
                .AsQueryable<Player>()
                .Where(p => p.PlayerId.ToString() == id)
                .ToListAsync(cancellationToken);
        }
    }
}