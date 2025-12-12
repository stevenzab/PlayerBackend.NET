using MongoDB.Driver.Linq;
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
                .ToListAsync(cancellationToken);
        }
    }
}