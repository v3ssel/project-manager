using ProjectManager.Application.Repositories;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Services
{
    public class GuidEntityService<T> : BaseService<T> where T : class, IGuidId
    {
        private readonly IGuidEntitiesRepository<T> _repository;

        public GuidEntityService(IGuidEntitiesRepository<T> repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> ExistsByIdAsync(Guid id)
        {
            return await _repository.ExistsByIdAsync(id);
        }
    }
}
