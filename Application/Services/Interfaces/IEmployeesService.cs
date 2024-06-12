using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Services.Interfaces
{
    public interface IEmployeesService : IGuidEntitiesService<Employee>
    {
        Task<Employee?> GetByIdWithTeamsAsync(Guid id);
        Task<IEnumerable<Employee>> SearchByQueryAsync(string query);
    }
}
