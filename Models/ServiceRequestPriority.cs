using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    // How fast a request has to be answered (low, medium, high, urgent...)
    public class ServiceRequestPriority
    {
        public int ServiceRequestPriorityId { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The {0} must have at least 3 chars and no more than 50.")]
        public required string Name { get; set; }

        [Range(1, 10, ErrorMessage = "The level must be between 1 and 10.")]
        public int Level { get; set; }

        [Range(1, 60, ErrorMessage = "The response days must be between 1 and 60.")]
        public int ResponseDays { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [StringLength(20, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public required string Color { get; set; }

        // From the class diagram: the date by which a request created on startDate must be answered
        public DateTime CalculateDueDate(DateTime startDate)
        {
            return startDate.AddDays(ResponseDays);
        }
    }
}
