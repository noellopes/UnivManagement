using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class ServiceRequestAttachment
    {
        // 5 MB is enough for scanned documents and photos
        public const long MaxSize = 5 * 1024 * 1024;

        public static readonly string[] AllowedExtensions = { ".pdf", ".jpg", ".jpeg", ".png", ".docx", ".txt" };

        public int ServiceRequestAttachmentId { get; set; }

        public int ServiceRequestId { get; set; }

        public ServiceRequest? ServiceRequest { get; set; }

        [Display(Name = "File name")]
        [Required(ErrorMessage = "The {0} is required.")]
        [StringLength(255, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public required string FileName { get; set; }

        [Display(Name = "File type")]
        [Required(ErrorMessage = "The {0} is required.")]
        [StringLength(100, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public required string ContentType { get; set; }

        // Size of the file in bytes
        public long Size { get; set; }

        public required byte[] Content { get; set; }

        [Display(Name = "Uploaded at")]
        public DateTime UploadedAt { get; set; } = DateTime.Now;

        public string SizeText => Size < 1024 * 1024
            ? $"{Math.Max(1, Size / 1024)} KB"
            : $"{Size / (1024.0 * 1024.0):0.0} MB";

        public static bool IsAllowedExtension(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLowerInvariant();
            return AllowedExtensions.Contains(extension);
        }
    }
}
