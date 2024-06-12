using System.ComponentModel.DataAnnotations;
using ProjectManager.Domain.Entities;

namespace Web.Models
{
    public class ProjectViewModel
    {
        [Required]
        public Project Project { get; set; } = null!;

        public IList<IFormFile>? ProjectFiles { get; set; } = new List<IFormFile>();
    }
}
