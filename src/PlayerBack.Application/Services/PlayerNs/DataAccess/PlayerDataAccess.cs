using MongoDB.Bson;
using MongoDB.Driver;
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

        private async Task<int> GetNextPlayerIdAsync()
        {
            var counters = baseRepository.GetCollection<BsonDocument>("counters");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", "playerId");
            var update = Builders<BsonDocument>.Update.Inc("sequence_value", 1);
            var options = new FindOneAndUpdateOptions<BsonDocument>
            {
                ReturnDocument = ReturnDocument.After,
                IsUpsert = true
            };

            var result = await counters.FindOneAndUpdateAsync(filter, update, options);
            return result["sequence_value"].AsInt32;
        }


        public async Task CreatePlayerAsync(Player player)
        {
            await baseRepository.AddAsync(player);
        }

        public async Task<Player> GetPlayerByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await baseRepository
                .GetCollection<Player>(typeof(Player).Name)
                .Find(p => p.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}