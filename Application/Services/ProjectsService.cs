using ProjectManager.Application.Repositories;
using ProjectManager.Domain.Entities;
using ProjectManager.Application.Services.Interfaces;

namespace ProjectManager.Application.Services
{
    public class ProjectsService : GuidEntityService<Project>, IProjectsService
    {
        private readonly IProjectsRepository _projectsRepository;

        public ProjectsService(IProjectsRepository repository) : base(repository)
        {
            _projectsRepository = repository;
        }

        public override async Task UpdateAsync(Project project)
        {
            var actualProject = await _projectsRepository.GetByIdAsync(project.Id);
            if (actualProject == null)
            {
                throw new ArgumentException($"Project {project.Id} not found.");
            }

            actualProject.ClientId = project.ClientId;
            actualProject.Name = project.Name;
            actualProject.Description = project.Description;
            actualProject.StartTime = project.StartTime;
            actualProject.EndTime = project.EndTime;
            actualProject.Priority = project.Priority;
            actualProject.ProjectFiles = actualProject.ProjectFiles
                                                      .Concat(project.ProjectFiles)
                                                      .ToList();

            await _projectsRepository.UpdateAsync(actualProject, true);
        }

        public async Task<IEnumerable<Project>> SearchByNameAsync(string name)
        {
            return await _projectsRepository.SearchByNameAsync(name);
        }

        public async Task<IEnumerable<ProjectFile>> GetProjectFilesAsync(Guid projectId)
        {
            return await _projectsRepository.GetProjectFilesAsync(projectId);
        }

        public async Task<ProjectFile?> GetProjectFileAsync(Guid projectId, int fileId)
        {
            return await _projectsRepository.GetProjectFileAsync(projectId, fileId);
        }

        public async Task<IEnumerable<Project>> GetInDateRange(DateTime start, DateTime end)
        {
            return await _projectsRepository.GetInDateRange(start, end);
        }
    }
}
