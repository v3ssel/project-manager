using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity, bool save);
        Task UpdateAsync(T entity, bool save);
        Task DeleteAsync(T entity, bool save);
        Task<IEnumerable<T>> GetAllAsync();
        IEnumerable<T> GetWhere(Func<T, bool> predicate);

        Task SaveAsync();
    }
}
