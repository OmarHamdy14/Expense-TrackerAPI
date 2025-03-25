using ExpenseTrackerAPI.Data.Base.Interfaces;
using ExpenseTrackerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace ExpenseTrackerAPI.Data.Base.Implementation
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T> where T : class
    {
        private readonly AppDbContext _appDbContext;
        internal DbSet<T> _dbSet;
        public EntityBaseRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _dbSet = _appDbContext.Set<T>();
        }
        public async Task<T?> Get(Expression<Func<T, bool>> filter, string? IncludeProperties = null, bool tracked = false)
        {
            IQueryable<T> query;
            if (tracked) query = _dbSet;
            else query = _dbSet.AsNoTracking();

            query = query.Where(filter);

            foreach (var prop in IncludeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query.Include(prop);
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task<List<T>> GetAll(Expression<Func<T, bool>>? filter, string? IncludeProperties = null)
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var prop in IncludeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query.Include(prop);
            }
            return await query.ToListAsync();
        }
        public async Task Create(T entity)
        {
            _dbSet.Add(entity);
            await _appDbContext.SaveChangesAsync();
        }
        public async Task Update(T entity)
        {
            EntityEntry entityEntry = _appDbContext.Entry(entity);
            entityEntry.State = EntityState.Modified;
            await _appDbContext.SaveChangesAsync();
        }
        public async Task Remove(T entity)
        {
            _dbSet.Remove(entity);
            await _appDbContext.SaveChangesAsync();
            //EntityEntry entityEntry = _appDbContext.Entry<T>(entity);
            //entityEntry.State = EntityState.Deleted;
        }
        public async Task RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            await _appDbContext.SaveChangesAsync();
            //foreach(var entity in entities)
            //{
            //    EntityEntry entityEntry = _appDbContext.Entry<T>(entity);
            //    entityEntry.State = EntityState.Deleted;
            //}
        }
    }
}
