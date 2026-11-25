
using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AccommodationManagement
{
    public class RoomAllocation : IValidatableObject
    {
        /* ID: */
        public int RoomAllocationId { get; set; }

        /* ACCOMMODATION APPLICATION: */
        [Required(ErrorMessage = "Please select a valid {0}.")]
        [Display(Name = "Accommodation Application")]
        public int? AccommodationApplicationId { get; set; }

        /* STUDENT: */
        [Required(ErrorMessage = "Please select a valid {0}.")]
        [Display(Name = "Student")]
        public int? StudentId { get; set; }

        /* ROOM: */
        [Required(ErrorMessage = "Please select a valid {0}.")]
        [Display(Name = "Room")]
        public int? RoomId { get; set; }

        /* START DATE: */
        [Required(ErrorMessage = "Please fill in a {0}.")]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateOnly? StartDate { get; set; }

        /* END DATE: */
        [Required(ErrorMessage = "Please fill in a {0}.")]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateOnly? EndDate { get; set; }

        /* STATUS: */
        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Status")]
        public AllocationStatus Status { get; set; }

        /* RULE: the end date cannot be before the start date */
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate.HasValue && EndDate.HasValue &&
                EndDate.Value < StartDate.Value)
            {
                yield return new ValidationResult(
                    "The end date cannot be before the start date.",
                    new[] { nameof(EndDate) });
            }
        }
    }

    public enum AllocationStatus
    {
        Pending,
        Confirmed,
        Rejected
    }
}
