using MongoDB.Driver;
using PlayerBack.Domain.Models;

namespace PlayerBack.Infrastructure.Common
{
    public class BaseRepository : IBaseRepository
    {
        private readonly IMongoDatabase database;

        public BaseRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        public IQueryable<T> AsQueryable<T>() => database.GetCollection<T>(typeof(T).Name).AsQueryable();

        public async Task AddAsync<T>(T entity) where T : RepositoryCollection
        {
            var collection = database.GetCollection<T>(typeof(T).Name);
            entity.Created = DateTime.Now;
            await collection.InsertOneAsync(entity);
        }
    }
}