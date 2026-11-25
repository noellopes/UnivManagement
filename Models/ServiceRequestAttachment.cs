using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    // A document attached to a service request (only its data is stored, not the file itself)
    public class ServiceRequestAttachment
    {
        public int ServiceRequestAttachmentId { get; set; }

        [Display(Name = "Request")]
        [Range(1, int.MaxValue, ErrorMessage = "Choose a request.")]
        public int ServiceRequestId { get; set; }

        public ServiceRequest? ServiceRequest { get; set; }

        [Display(Name = "File name")]
        [Required(ErrorMessage = "The {0} is required.")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "The {0} must have between {2} and {1} characters.")]
        [RegularExpression(@"^.+\.(?i:pdf|png|jpg|jpeg|docx)$", ErrorMessage = "Allowed files: pdf, png, jpg, jpeg or docx.")]
        public required string FileName { get; set; }

        [Display(Name = "Size (KB)")]
        [Range(1, 5120, ErrorMessage = "The {0} must be between {1} and {2}.")]
        public int SizeInKb { get; set; }

        [StringLength(300, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public string? Description { get; set; }

        // Set by the server, so it is not part of the form
        [Display(Name = "Uploaded at")]
        public DateTime UploadedAt { get; set; } = DateTime.Now;
    }
}