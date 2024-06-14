using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Repositories;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Data;

namespace ProjectManager.Infrastructure.Repositories
{
    public class TeamsRepository : BaseRepository<Team>, ITeamsRepository
    {
        public TeamsRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Team>> GetAllAsync()
        {
            return await _dbSet.Include(x => x.Project)
                               .Include(x => x.Lead)
                               .Include(x => x.Members)
                               .ToListAsync();
        }

        public override async Task AddAsync(Team team, bool save)
        {
            _dbSet.Add(team);
            team.Members.Add(team.Lead!);

            if (save) await SaveAsync();
        }
        
        public override async Task DeleteAsync(Team team, bool save)
        {
            _context.EmployeeTeam.RemoveRange(_context.EmployeeTeam.Where(x => x.TeamId == team.Id));
            _dbSet.Remove(team);

            if (save) await SaveAsync();
        }

        public async Task<Team?> GetByIdAsync(int id)
        {
            return await _dbSet.Include(x => x.Project)
                               .Include(x => x.Lead)
                               .Include(x => x.Members)
                               .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _dbSet.AnyAsync(x => x.Id == id);
        }

        public async Task AddEmployeeToTeamAsync(Team team, Employee employee, bool save)
        {
            team.Members.Add(employee);
            _dbSet.Update(team);

            if (save) await SaveAsync();
        }

        public async Task RemoveEmployeeFromTeamAsync(Team team, Employee employee, bool save)
        {
            team.Members.Remove(employee);
            
            if (save) await SaveAsync();
        }

        public async Task<bool> IsEmployeeInTeamAsync(Team team, Employee employee)
        {
            return await _context.EmployeeTeam.AnyAsync(x => x.TeamId == team.Id && x.EmployeeId == employee.Id);
        }
    }
}
