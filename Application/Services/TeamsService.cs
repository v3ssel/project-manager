using ProjectManager.Application.Repositories;
using ProjectManager.Application.Services.Interfaces;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Services
{
    public class TeamsService : BaseService<Team>, ITeamsService
    {
        private readonly ITeamsRepository _teamRepository;
        private readonly IEmployeesRepository _employeesRepository;

        public TeamsService(ITeamsRepository repository, IEmployeesRepository employeesRepository) : base(repository)
        {
            _teamRepository = repository;
            _employeesRepository = employeesRepository;
        }

        public override async Task AddAsync(Team team)
        {
            var lead = await _employeesRepository.GetByIdAsync(team.LeadId);
            if (lead == null)
            {
                throw new ArgumentException($"Team Lead {team.LeadId} not found.");
            }

            await _teamRepository.AddAsync(team, true);
        }

        public override async Task UpdateAsync(Team team)
        {
            var oldTeam = await GetByIdAsync(team.Id);

            if (oldTeam == null)
            {
                throw new ArgumentException($"Team with id {team.Id} not found");
            }

            if (oldTeam!.LeadId != team.LeadId && !oldTeam.Members.Any(x => x.Id == team.LeadId))
            {
                var lead = await _employeesRepository.GetByIdAsync(team.LeadId);
                if (lead == null)
                {
                    throw new ArgumentException($"New team lead not found. Id: {team.LeadId}");
                }

                oldTeam.Members.Add(lead);
            }

            oldTeam.Name = team.Name;
            oldTeam.LeadId = team.LeadId;
            oldTeam.ProjectId = team.ProjectId;

            await _teamRepository.UpdateAsync(oldTeam, true);
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
            if (await _teamRepository.IsEmployeeInTeamAsync(team, employee))
            {
                throw new ArgumentException($"Employee {employee.Id} is already in team {team.Id}");
            }

            await _teamRepository.AddEmployeeToTeamAsync(team, employee, true);
        }

        public async Task RemoveEmployeeFromTeamAsync(Team team, Employee employee)
        {
            if (!await _teamRepository.IsEmployeeInTeamAsync(team, employee))
            {
                throw new ArgumentException($"Employee {employee.Id} not found in team {team.Id}");
            }

            await _teamRepository.RemoveEmployeeFromTeamAsync(team, employee, true);
        }

        public async Task<bool> IsEmployeeInTeamAsync(Team team, Employee employee)
        {
            return await _teamRepository.IsEmployeeInTeamAsync(team, employee);
        }
    }
}
