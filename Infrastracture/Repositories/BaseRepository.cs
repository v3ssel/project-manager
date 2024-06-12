using Microsoft.EntityFrameworkCore;
using ProjectManager.Infrastructure.Data;

namespace ProjectManager.Infrastructure.Repositories
{
    public class BaseRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task AddAsync(T entity, bool save)
        {
            _dbSet.Add(entity);

            if (save) await SaveAsync();
        }

        public virtual async Task UpdateAsync(T entity, bool save)
        {
            _dbSet.Update(entity);

            if (save) await SaveAsync();
        }
        
        public virtual async Task DeleteAsync(T entity, bool save)
        {
            _dbSet.Remove(entity);

            if (save) await SaveAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual IEnumerable<T> GetWhere(Func<T, bool> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public virtual async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
