using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    // One entry of the history of a request: what happened and when
    public class ServiceRequestHistory
    {
        public int ServiceRequestHistoryId { get; set; }

        [Display(Name = "Request")]
        [Required(ErrorMessage = "The {0} is required.")]
        public int ServiceRequestId { get; set; }

        public ServiceRequest? ServiceRequest { get; set; }

        [Display(Name = "Previous status")]
        public int? OldStatusId { get; set; }

        public ServiceRequestStatus? OldStatus { get; set; }

        [Display(Name = "New status")]
        public int? NewStatusId { get; set; }

        public ServiceRequestStatus? NewStatus { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [StringLength(100, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public required string Action { get; set; }

        [StringLength(500, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public string? Note { get; set; }

        [Display(Name = "Changed at")]
        public DateTime ChangedAt { get; set; } = DateTime.Now;
    }
}