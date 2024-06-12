using ProjectManager.Application.Repositories;
using ProjectManager.Application.Services.Interfaces;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Services
{
    public class EmployeesService : GuidEntityService<Employee>, IEmployeesService
    {
        private readonly IEmployeesRepository _employeeRepository;

        public EmployeesService(IEmployeesRepository repository) : base(repository)
        {
            _employeeRepository = repository;
        }

        public async Task<IEnumerable<Employee>> SearchByQueryAsync(string query)
        {
            return await _employeeRepository.SearchByQueryAsync(query);
        }

        public async Task<Employee?> GetByIdWithTeamsAsync(Guid id)
        {
            return await _employeeRepository.GetByIdWithTeamsAsync(id);
        }
    }
}
