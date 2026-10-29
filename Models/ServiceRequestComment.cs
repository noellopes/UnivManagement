using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class ServiceRequestComment
    {
        public int ServiceRequestCommentId { get; set; }

        public int ServiceRequestId { get; set; }

        public ServiceRequest? ServiceRequest { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [StringLength(200, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public required string Author { get; set; }

        [Display(Name = "Comment")]
        [Required(ErrorMessage = "The {0} is required.")]
        [StringLength(1000, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public required string Text { get; set; }

        [Display(Name = "Posted at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
