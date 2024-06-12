using ProjectManager.Domain.Entities;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ProjectManager.Application.Services
{
    public class FileManagerService : IFileManagerService
    {
        public async Task SaveProjectFilesAsync(string uploadDir, IEnumerable<IFormFile> files, Project project)
        {
            var dir = Directory.CreateDirectory(uploadDir);
            foreach (var file in files)
            {
                var guid = Guid.NewGuid();
                var fileName = file.FileName;
                var filePath = $"{dir.FullName}/{file.FileName}-{guid}";

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                project.ProjectFiles.Add(new ProjectFile
                {
                    FileName = fileName,
                    FilePath = filePath,
                    ProjectId = project.Id,
                    Project = project
                });
            }
        }
        public void DeleteProjectFile(string path)
        {
            File.Delete(path);
        }
    }
}
