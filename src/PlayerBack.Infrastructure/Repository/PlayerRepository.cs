using MongoDB.Driver;
using PlayerBack.Infrastructure.Common;
using PlayerBack.Infrastructure.Models;

namespace PlayerBack.Infrastructure.Repository
{
    public class PlayerRepository : BaseRepository<Player>, IPlayerRepository
    {
        public PlayerRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {
        }
    }
}