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

            var lead = _context.Employees.Find(team.LeadId);
            if (lead == null)
            {
                throw new ArgumentException($"Team Lead {team.LeadId} not found.");
            }

            if (save) await SaveAsync();
            await AddEmployeeToTeamAsync(team, lead, true);
        }

        public override async Task UpdateAsync(Team team, bool save)
        {
            var oldTeam = await GetByIdAsync(team.Id);

            if (oldTeam == null)
            {
                throw new ArgumentException($"Team with id {team.Id} not found");
            }

            if (oldTeam!.LeadId != team.LeadId && !oldTeam.Members.Any(x => x.Id == team.LeadId))
            {
                var lead = await _context.Employees.FindAsync(team.LeadId);
                if (lead == null)
                {
                    throw new ArgumentException($"New team lead not found. Id: {team.LeadId}");
                }

                oldTeam.Members.Add(lead);
            }

            oldTeam.Name = team.Name;
            oldTeam.LeadId = team.LeadId;
            oldTeam.ProjectId = team.ProjectId;

            _dbSet.Update(oldTeam);

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
            if (team.Members.Where(x => x.Id == employee.Id).Any())
            {
                var msg = $"Team \'{team.Name}\' with member \'{employee.FirstName} {employee.LastName}\' already exists.";
                throw new ArgumentException(msg);
            }

            team.Members.Add(employee);
            _dbSet.Update(team);

            if (save) await SaveAsync();
        }

        public async Task RemoveEmployeeFromTeamAsync(Team team, Employee employee, bool save)
        {
            if (!team.Members.Where(x => x.Id == employee.Id).Any())
            {
                var msg = $"Team \'{team.Name}\' with employee \'{employee.FirstName} {employee.LastName}\' not found";
                throw new ArgumentException(msg);
            }

            team.Members.Remove(employee);
            if (save) await SaveAsync();
        }
    }
}
