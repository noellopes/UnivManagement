using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    // A comment written about a request, by the requester or by the administrative services
    public class ServiceRequestComment
    {
        public int ServiceRequestCommentId { get; set; }

        [Display(Name = "Request")]
        [Required(ErrorMessage = "The {0} is required.")]
        public int ServiceRequestId { get; set; }

        [Display(Name = "Request")]
        public ServiceRequest? ServiceRequest { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "The {0} must have between {2} and {1} characters.")]
        public required string Author { get; set; }

        [Display(Name = "Comment")]
        [Required(ErrorMessage = "The {0} is required.")]
        [StringLength(1000, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public required string Text { get; set; }

        [Display(Name = "Posted at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
