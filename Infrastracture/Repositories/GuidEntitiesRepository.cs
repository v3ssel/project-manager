using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Repositories;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Data;

namespace ProjectManager.Infrastructure.Repositories
{
    public class GuidEntitiesRepository<T> : BaseRepository<T>, IGuidEntitiesRepository<T> where T : class, IGuidId
    {
        public GuidEntitiesRepository(ApplicationDbContext context) : base(context)
        {
        }
        
        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }
     
        public virtual async Task<bool> ExistsByIdAsync(Guid id)
        {
            return await _dbSet.AnyAsync(x => x.Id == id);
        }
    }
}
