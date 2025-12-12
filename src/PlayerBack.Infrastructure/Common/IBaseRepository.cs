using PlayerBack.Infrastructure.Models;

namespace PlayerBack.Infrastructure.Common
{
    public interface IBaseRepository<T> where T : RepositoryCollection
    {
        Task AddAsync(T entity);

        IQueryable<T> AsQueryable();
    }
}