using ProjectManager.Application.Repositories;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Repositories
{
    public interface IClientsRepository : IGuidEntitiesRepository<Client>
    {
    }
}
