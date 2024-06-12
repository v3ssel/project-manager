using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Domain.Entities
{
    public class ProjectFile
    {
        public int Id { get; set; }

        [Required]
        public string? FileName { get; set; }
        [Required]
        public string? FilePath { get; set; }

        [Required]
        public Guid ProjectId { get; set; }
        public Project? Project { get; set; }
    }
}
