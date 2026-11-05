using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    // A step in the life of a request (submitted, in progress, resolved...), managed by the administrative services
    public class ServiceRequestStatus
    {
        // Bootstrap colours that can be used for the badge of a status
        public static readonly string[] BadgeColors = { "primary", "secondary", "success", "danger", "warning", "info", "dark" };

        public int ServiceRequestStatusId { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The {0} must have between {2} and {1} characters.")]
        public required string Name { get; set; }

        [StringLength(300, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public string? Description { get; set; }

        // Statuses are listed in this order; new requests start with the first one that does not close the request
        [Display(Name = "Order")]
        [Range(1, 99, ErrorMessage = "The {0} must be between {1} and {2}.")]
        public int DisplayOrder { get; set; } = 1;

        // For example resolved, rejected or cancelled
        [Display(Name = "Closes the request")]
        public bool IsFinal { get; set; }

        // For example rejected or waiting for information: the requester must know the reason
        [Display(Name = "Note required")]
        public bool RequiresNote { get; set; }

        [Display(Name = "Colour")]
        [Required(ErrorMessage = "The {0} is required.")]
        [RegularExpression("^(primary|secondary|success|danger|warning|info|dark)$", ErrorMessage = "Please choose a colour from the list.")]
        public string Color { get; set; } = "secondary";
    }
}
