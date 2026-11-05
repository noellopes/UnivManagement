using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    // How urgent a request is and how many days the administrative services have to resolve it
    public class ServiceRequestPriority
    {
        // Bootstrap colours that can be used for the badge of a priority
        public static readonly string[] BadgeColors = { "light", "info", "primary", "warning", "danger", "dark" };

        public int ServiceRequestPriorityId { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The {0} must have between {2} and {1} characters.")]
        public required string Name { get; set; }

        // A request with this priority is due this number of days after it was created
        [Display(Name = "Response time (days)")]
        [Range(1, 60, ErrorMessage = "The {0} must be between {1} and {2}.")]
        public int ResponseDays { get; set; } = 7;

        // Higher levels are more urgent and are listed first
        [Range(1, 10, ErrorMessage = "The {0} must be between {1} and {2}.")]
        public int Level { get; set; } = 1;

        [Display(Name = "Colour")]
        [Required(ErrorMessage = "The {0} is required.")]
        [RegularExpression("^(light|info|primary|warning|danger|dark)$", ErrorMessage = "Please choose a colour from the list.")]
        public string Color { get; set; } = "info";
    }
}
