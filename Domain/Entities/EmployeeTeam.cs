using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Domain.Entities
{
    public class EmployeeTeam
    {
        public Guid EmployeeId { get; set; }
        public int TeamId { get; set; }
    }
}
