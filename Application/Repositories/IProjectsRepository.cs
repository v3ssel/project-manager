using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Repositories
{
    public interface IProjectsRepository : IGuidEntitiesRepository<Project>
    {
        Task<IEnumerable<Project>> SearchByNameAsync(string name);
        Task<IEnumerable<Project>> GetInDateRange(DateTime start, DateTime end);
        Task<IEnumerable<Project>> GetByClientAsync(Client client);
        
        Task<IEnumerable<ProjectFile>> GetProjectFilesAsync(Guid projectId);
        Task<ProjectFile?> GetProjectFileAsync(Guid projectId, int fileId);
    }
}
