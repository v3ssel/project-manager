using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Domain.Entities
{
    public class Employee : IGuidId
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string? FirstName { get; set; }
        
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string? LastName { get; set; }

        [Required, EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }

        public IList<Team> Teams { get; set; } = new List<Team>();
        public IList<Team> LeadTeams { get; set; } = new List<Team>();
    }
}
