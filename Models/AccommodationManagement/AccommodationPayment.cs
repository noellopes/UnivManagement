using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AccommodationManagement
{
    public class AccommodationPayment : IValidatableObject
    {
        /* ID: */
        public int AccommodationPaymentId { get; set; }

        /* ACCOMMODATION CONTRACT: */
        [Required(ErrorMessage = "Please select a valid {0}.")]
        [Display(Name = "contract ID")]
        public int? AccommodationContractId { get; set; }

        /* DUE DATE: */
        [Required(ErrorMessage = "Please fill in a {0}")]
        [DataType(DataType.Date)]
        [Display(Name = "due date")]
        public DateOnly? DueDate { get; set; }

        /* AMOUNT: */
        [Required(ErrorMessage = "{0} missing.")]
        [Range(0.01, 1000000, ErrorMessage = "The {0} must be greater than zero.")]
        [Display(Name = "Amount")]
        public decimal? Amount { get; set; }

        /* PAID AMOUNT: */
        [Required(ErrorMessage = "{0} missing.")]
        [Range(0, 1000000, ErrorMessage = "The {0} cannot be negative.")]
        [Display(Name = "Paid Amount")]
        public decimal? PaidAmount { get; set; }

        /* PAYMENT DATE: */
        [DataType(DataType.DateTime)]
        [Display(Name = "Paid At")]
        public DateTime? PaidAt { get; set; }

        /* STATUS: */
        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Status")]
        public PaymentStatus Status { get; set; }

        /* RULE: the paid amount cannot be greater than the amount due */
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PaidAmount > Amount)
            {
                yield return new ValidationResult(
                    "The paid amount cannot be greater than the amount due.",
                    new[] { nameof(PaidAmount) });
            }
        }
    }

    public enum PaymentStatus
    {
        Pending,
        Paid,
        Overdue
    }
}
