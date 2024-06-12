using ProjectManager.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Interfaces
{
    public interface IFileManagerService
    {
        Task SaveProjectFilesAsync(string uploadDir, IEnumerable<IFormFile> files, Project project);
        void DeleteProjectFile(string path);
    }
}
