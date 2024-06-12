using ProjectManager.Application.Repositories;
using ProjectManager.Application.Services.Interfaces;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Services
{
    public class TeamsService : BaseService<Team>, ITeamsService
    {
        private readonly ITeamsRepository _teamRepository;

        public TeamsService(ITeamsRepository repository) : base(repository)
        {
            _teamRepository = repository;
        }

        public async Task<Team?> GetByIdAsync(int id)
        {
            return await _teamRepository.GetByIdAsync(id);
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _teamRepository.ExistsByIdAsync(id);
        }

        public async Task AddEmployeeToTeamAsync(Team team, Employee employee)
        {
            await _teamRepository.AddEmployeeToTeamAsync(team, employee, true);
        }

        public async Task RemoveEmployeeFromTeamAsync(Team team, Employee employee)
        {
            await _teamRepository.RemoveEmployeeFromTeamAsync(team, employee, true);
        }
    }
}
