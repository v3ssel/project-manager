using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Entities
{
    public interface IGuidId
    {
        Guid Id { get; set; }
    }
}
