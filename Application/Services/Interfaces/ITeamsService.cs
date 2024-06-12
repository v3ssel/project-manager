using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Services.Interfaces
{
    public interface ITeamsService : IService<Team>
    {
        Task<Team?> GetByIdAsync(int id);
        Task<bool> ExistsByIdAsync(int id);

        Task AddEmployeeToTeamAsync(Team team, Employee employee);
        Task RemoveEmployeeFromTeamAsync(Team team, Employee employee);
    }
}
