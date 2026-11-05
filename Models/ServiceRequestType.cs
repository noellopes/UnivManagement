using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    // Kind of request (certificate, official document, student support...), managed by the administrative services
    public class ServiceRequestType
    {
        public int ServiceRequestTypeId { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The {0} must have between {2} and {1} characters.")]
        public required string Name { get; set; }

        [StringLength(500, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public string? Description { get; set; }
    }
}
