using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Repositories;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Data;

namespace ProjectManager.Infrastructure.Repositories
{
    public class EmployeesRepository : GuidEntitiesRepository<Employee>, IEmployeesRepository
    {
        public EmployeesRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _dbSet.Include(x => x.Teams).ToListAsync();
        }

        public override async Task DeleteAsync(Employee employee, bool save)
        {
            _context.EmployeeTeam.RemoveRange(_context.EmployeeTeam.Where(x => x.EmployeeId == employee.Id));
            _dbSet.Remove(employee);

            if (save) await SaveAsync();
        }

        public async Task<IEnumerable<Employee>> SearchByQueryAsync(string query)
        {
            return (await _dbSet
                .ToListAsync())
                .Where(x => $"{x.FirstName!.ToLower()} {x.MiddleName?.ToLower()} {x.LastName!.ToLower()} {x.Email!.ToLower()}"
                .Contains(query.ToLower()));
        }

        public async Task<Employee?> GetByIdWithTeamsAsync(Guid id)
        {
            return await _dbSet.Include(x => x.Teams)
                               .ThenInclude(x => x.Project)
                               .Where(x => x.Id == id)
                               .FirstOrDefaultAsync();
        }
    }
}
