using ProjectManager.Domain.Entities;
using ProjectManager.Application.Repositories;
using ProjectManager.Application.Services.Interfaces;

namespace ProjectManager.Application.Services
{
    public class ClientsService : GuidEntityService<Client>, IClientsService
    {
        public ClientsService(IClientsRepository repository) : base(repository)
        {
        }
    }
}
