using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class ServiceRequest
    {
        public int ServiceRequestId { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [StringLength(150, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        public required string Description { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        public required string Type { get; set; }

        public string Status { get; set; } = "Submitted";

        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
