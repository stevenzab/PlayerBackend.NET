using MongoDB.Driver;
using PlayerBack.Domain.Models;

namespace PlayerBack.Infrastructure.Common
{
    public interface IBaseRepository
    {
        Task AddAsync<T>(T entity) where T : RepositoryCollection;

        IQueryable<T> AsQueryable<T>();

        IMongoCollection<T> GetCollection<T>(string name);
    }
}