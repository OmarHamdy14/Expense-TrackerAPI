using System.Linq.Expressions;

namespace ExpenseTrackerAPI.Data.Base.Interfaces
{
    public interface IEntityBaseRepository<T> where T : class
    {
        Task<T?> Get(Expression<Func<T, bool>> filter, string? IncludeProperties = null, bool tracked = false);
        Task<List<T>> GetAll(Expression<Func<T, bool>>? filter, string? IncludeProperties = null);
        Task Create(T entity);
        Task Update(T entity);
        Task Remove(T entity);
        Task RemoveRange(IEnumerable<T> entities);
    }
}
