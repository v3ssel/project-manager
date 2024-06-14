using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Repositories;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Data;

namespace ProjectManager.Infrastructure.Repositories
{
    public class ProjectsRepository : GuidEntitiesRepository<Project>, IProjectsRepository
    {
        public ProjectsRepository(ApplicationDbContext context) : base(context)
        {
        }
        public override async Task<Project?> GetByIdAsync(Guid id)
        {
            return await _dbSet.Include(x => x.Client)
                               .Include(x => x.ProjectFiles)
                               .FirstOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _dbSet.Include(x => x.Client)
                               .Include(x => x.ProjectFiles)
                               .ToListAsync();
        }

        public async Task<IEnumerable<Project>> SearchByNameAsync(string name)
        {
            return await _dbSet.Where(x => x.Name!.Contains(name))
                               .ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetInDateRange(DateTime start, DateTime end)
        {
            return await _dbSet.Include(x => x.Client)
                               .Include(x => x.ProjectFiles)
                               .Where(x => x.StartTime >= start && x.EndTime <= end)
                               .ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetByClientAsync(Client client)
        {
            return await _dbSet.Include(x => x.Client)
                               .Include(x => x.ProjectFiles)
                               .Where(x => x.ClientId == client.Id)
                               .ToListAsync();
        }

        public async Task<IEnumerable<ProjectFile>> GetProjectFilesAsync(Guid projectId)
        {
            return await _context.ProjectFiles.Where(x => x.ProjectId == projectId).ToListAsync();
        }

        public async Task<ProjectFile?> GetProjectFileAsync(Guid projectId, int fileId)
        {
            return await _context.ProjectFiles.FirstOrDefaultAsync(x => x.Id == fileId && x.ProjectId == projectId);
        }
    }
}
