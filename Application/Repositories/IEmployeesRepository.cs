using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Repositories
{
    public interface IEmployeesRepository : IGuidEntitiesRepository<Employee>
    {
        Task<Employee?> GetByIdWithTeamsAsync(Guid id);
        Task<IEnumerable<Employee>> SearchByQueryAsync(string query);
    }
}
