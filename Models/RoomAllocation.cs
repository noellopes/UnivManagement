using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    /*
    RoomAllocation:
     * RoomAllocationId              int
     * AccommodationApplicationId    int
     * StudentId                     int
     * RoomId                        int
     * StartDate                     DateOnly
     * EndDate                       DateOnly            (must be after StartDate)
     * Status                        AllocationStatus    (Active, Ended, Cancelled)
    */


    /// RoomAllocation model class from Accommodation Management domain.

    public class RoomAllocation : IValidatableObject
    {
        /* ID: */
        public int RoomAllocationId { get; set; }

        /* ACCOMMODATION APPLICATION: */
        [Required(ErrorMessage = "The {0} is required.")]
        public int AccommodationApplicationId { get; set; }

        /* STUDENT: */
        [Required(ErrorMessage = "The {0} is required.")]
        public int StudentId { get; set; }

        /* ROOM: */
        [Required(ErrorMessage = "The {0} is required.")]
        public int RoomId { get; set; }

        /* START DATE: */
        [Required(ErrorMessage = "The {0} is required.")]
        [DataType(DataType.Date)]
        public DateOnly StartDate { get; set; }

        /* END DATE: */
        [Required(ErrorMessage = "The {0} is required.")]
        [DataType(DataType.Date)]
        public DateOnly EndDate { get; set; }

        /* STATUS: */
        [Required(ErrorMessage = "The {0} is required.")]
        public AllocationStatus Status { get; set; }

        /* RULE: the end date must be after the start date */
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndDate <= StartDate)
            {
                yield return new ValidationResult(
                    "The end date must be after the start date.",
                    new[] { nameof(EndDate) });
            }
        }
    }

    public enum AllocationStatus { Active, Ended, Cancelled }
}