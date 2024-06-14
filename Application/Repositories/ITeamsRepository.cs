using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Repositories
{
    public interface ITeamsRepository : IRepository<Team>
    {
        Task<Team?> GetByIdAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
        Task<bool> IsEmployeeInTeamAsync(Team team, Employee employee);

        Task AddEmployeeToTeamAsync(Team team, Employee employee, bool save);
        Task RemoveEmployeeFromTeamAsync(Team team, Employee employee, bool save);
    }
}
