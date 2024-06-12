using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Domain.Entities
{
    public class Project : IGuidId
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Project name is required.")]
        public string? Name { get; set; }
        public string? Description { get; set; }

        [Required(ErrorMessage = "Invalid start time.")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "Invalid end time.")]
        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "Please, set project priority.")]
        public int Priority { get; set; }

        [Required(ErrorMessage = "Please, select a client.")]
        public Guid? ClientId { get; set; }
        public Client? Client { get; set; }

        public IList<ProjectFile> ProjectFiles { get; set; } = new List<ProjectFile>();
    }
}
