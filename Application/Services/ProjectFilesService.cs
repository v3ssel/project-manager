using ProjectManager.Domain.Entities;
using Microsoft.AspNetCore.Http;
using ProjectManager.Application.Services.Interfaces;
using ProjectManager.Application.Repositories;

namespace ProjectManager.Application.Services
{
    public class ProjectFilesService : IProjectFilesService
    {
        private readonly IProjectsRepository _projectsRepository;

        public ProjectFilesService(IProjectsRepository projectsRepository)
        {
            _projectsRepository = projectsRepository;
        }

        public async Task<IEnumerable<ProjectFile>> GetProjectFilesAsync(Guid projectId)
        {
            return await _projectsRepository.GetProjectFilesAsync(projectId);
        }

        public async Task<ProjectFile?> GetProjectFileAsync(Guid projectId, int fileId)
        {
            return await _projectsRepository.GetProjectFileAsync(projectId, fileId);
        }

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

        public void DeleteProjectFile(ProjectFile file)
        {
            File.Delete(file.FilePath!);
        }

        public async Task DeleteAllProjectFilesAsync(Project project)
        {
            var projectFiles = await GetProjectFilesAsync(project.Id);

            foreach (var file in projectFiles)
            {
                DeleteProjectFile(file);
            }
        }
    }
}
