using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Repositories
{
    public interface IGuidEntitiesRepository<T> : IRepository<T> where T : class, IGuidId
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<bool> ExistsByIdAsync(Guid id);
    }
}
