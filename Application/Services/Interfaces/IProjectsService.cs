using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Services.Interfaces
{
    public interface IProjectsService : IGuidEntitiesService<Project>
    {
        Task<IEnumerable<Project>> SearchByNameAsync(string name);
        Task<IEnumerable<Project>> GetInDateRange(DateTime start, DateTime end);
        Task<IEnumerable<ProjectFile>> GetProjectFilesAsync(Guid projectId);
        Task<ProjectFile?> GetProjectFileAsync(Guid projectId, int fileId);
    }
}
