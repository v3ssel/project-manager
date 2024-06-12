using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Domain.Entities
{
    public class Client : IGuidId
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Client Name is required.")]
        public string? Name { get; set; }
    }
}
