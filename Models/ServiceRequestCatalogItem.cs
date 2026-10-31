using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    // A service offered by the administrative services, e.g. "Enrolment certificate"
    public class ServiceRequestCatalogItem
    {
        public int ServiceRequestCatalogItemId { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "The {0} must have between {2} and {1} characters.")]
        public required string Name { get; set; }

        [StringLength(1000, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public string? Description { get; set; }

        // The requests of this service get this type
        [Display(Name = "Request type")]
        [Required(ErrorMessage = "The {0} is required.")]
        public int ServiceRequestTypeId { get; set; }

        [Display(Name = "Request type")]
        public ServiceRequestType? Type { get; set; }

        [Display(Name = "Processing days")]
        [Range(1, 60, ErrorMessage = "The {0} must be between {1} and {2}.")]
        public int ProcessingDays { get; set; } = 5;

        [Display(Name = "Required documents")]
        [StringLength(500, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public string? RequiredDocuments { get; set; }

        // Unavailable services are kept for the history but cannot be requested
        [Display(Name = "Available")]
        public bool IsActive { get; set; } = true;

        public ICollection<ServiceRequest> Requests { get; set; } = new List<ServiceRequest>();
    }
}
