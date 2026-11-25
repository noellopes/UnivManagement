using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class ServiceRequestCatalogItem
    {
        public int ServiceRequestCatalogItemId { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The {0} must have at least 3 chars and no more than 100.")]
        public required string Name { get; set; }

        [StringLength(500, ErrorMessage = "The description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Choose a request type.")]
        public int ServiceRequestTypeId { get; set; }

        public ServiceRequestType? Type { get; set; }

        [Range(1, 60, ErrorMessage = "The processing days must be between 1 and 60.")]
        public int ProcessingDays { get; set; }

        [StringLength(500, ErrorMessage = "The required documents cannot exceed 500 characters.")]
        public string? RequiredDocuments { get; set; }

        public bool IsActive { get; set; }
    }
}
