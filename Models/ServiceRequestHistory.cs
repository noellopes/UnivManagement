using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    // One entry for each action taken on a request (created, status changed, assigned...)
    public class ServiceRequestHistory
    {
        public int ServiceRequestHistoryId { get; set; }

        public int ServiceRequestId { get; set; }

        public ServiceRequest? ServiceRequest { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [StringLength(200, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public required string Action { get; set; }

        // Only set when the action changed the status of the request
        [Display(Name = "Previous status")]
        public int? OldStatusId { get; set; }

        [Display(Name = "Previous status")]
        public ServiceRequestStatus? OldStatus { get; set; }

        [Display(Name = "New status")]
        public int? NewStatusId { get; set; }

        [Display(Name = "New status")]
        public ServiceRequestStatus? NewStatus { get; set; }

        [StringLength(500, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public string? Note { get; set; }

        [Display(Name = "Date")]
        public DateTime ChangedAt { get; set; } = DateTime.Now;
    }
}
