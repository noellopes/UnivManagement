using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class ServiceRequest
    {
        public int ServiceRequestId { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [StringLength(150, MinimumLength = 5, ErrorMessage = "The {0} must have between {2} and {1} characters.")]
        public required string Title { get; set; }

        // Optional: for many requests (e.g. a certificate) the title is enough
        [StringLength(2000, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public string? Description { get; set; }

        [Display(Name = "Request type")]
        [Required(ErrorMessage = "The {0} is required.")]
        public int ServiceRequestTypeId { get; set; }

        [Display(Name = "Request type")]
        public ServiceRequestType? Type { get; set; }

        // Set by the server: new requests start with the first status of the list
        [Display(Name = "Status")]
        public int ServiceRequestStatusId { get; set; }

        public ServiceRequestStatus? Status { get; set; }

        [Display(Name = "Requester name")]
        [Required(ErrorMessage = "The {0} is required.")]
        [StringLength(200, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public required string RequesterName { get; set; }

        [Display(Name = "Requester email")]
        [Required(ErrorMessage = "The {0} is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid {0}.")]
        [StringLength(256, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public required string RequesterEmail { get; set; }

        [Display(Name = "Assigned to")]
        [StringLength(200, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public string? AssignedTo { get; set; }

        [Display(Name = "Created at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Last updated")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "Closed at")]
        public DateTime? ClosedAt { get; set; }
    }
}
