using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Services.Interfaces
{
    public interface IGuidEntitiesService<T> : IService<T> where T : class, IGuidId
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<bool> ExistsByIdAsync(Guid id);
    }
}
