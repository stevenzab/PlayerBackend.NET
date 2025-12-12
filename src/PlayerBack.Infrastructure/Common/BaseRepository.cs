using MongoDB.Driver;
using PlayerBack.Infrastructure.Models;

namespace PlayerBack.Infrastructure.Common
{
    public class BaseRepository<T> : IBaseRepository<T> where T : RepositoryCollection
    {
        private readonly IMongoDatabase database;
        private readonly IMongoCollection<T> collection;

        public BaseRepository(IMongoDatabase database)
        {
            this.database = database;
            collection = database.GetCollection<T>(typeof(T).Name);
        }

        public IQueryable<T> AsQueryable() => database.GetCollection<T>(typeof(T).Name).AsQueryable();

        public async Task AddAsync(T entity)
        {
            entity.Created = DateTime.Now;
            await collection.InsertOneAsync(entity);
        }
    }
}