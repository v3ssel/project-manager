using ProjectManager.Application.Repositories;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Data;

namespace ProjectManager.Infrastructure.Repositories
{
    public class ClientsRepository : GuidEntitiesRepository<Client>, IClientsRepository
    {
        public ClientsRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
