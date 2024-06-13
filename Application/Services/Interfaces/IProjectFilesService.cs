using ProjectManager.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace ProjectManager.Application.Services.Interfaces
{
    public interface IProjectFilesService
    {
        Task<IEnumerable<ProjectFile>> GetProjectFilesAsync(Guid projectId);
        Task<ProjectFile?> GetProjectFileAsync(Guid projectId, int fileId);

        Task SaveProjectFilesAsync(string uploadDir, IEnumerable<IFormFile> files, Project project);
        Task DeleteAllProjectFilesAsync(Project project);
        void DeleteProjectFile(ProjectFile file);
    }
}
