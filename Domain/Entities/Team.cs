using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Domain.Entities
{
    public class Team
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Team Name is required.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Please, select the project for a team.")]
        public Guid ProjectId { get; set; }
        public Project? Project { get; set; }

        [Required(ErrorMessage = "Please, select lead for the team.")]
        public Guid LeadId { get; set; }
        public Employee? Lead { get; set; }

        public IList<Employee> Members { get; set; } = new List<Employee>();
    }
}
