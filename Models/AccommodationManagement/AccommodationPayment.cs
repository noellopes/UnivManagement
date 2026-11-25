using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AccommodationManagement
{
    public class AccommodationPayment : IValidatableObject
    {
        /* ID: */
        public int AccommodationPaymentId { get; set; }

        /* ACCOMMODATION CONTRACT: */
        [Required(ErrorMessage = "The {0} is required.")]
        public int AccommodationContractId { get; set; }

        /* DUE DATE: */
        [Required(ErrorMessage = "The {0} is required.")]
        [DataType(DataType.Date)]
        public DateOnly DueDate { get; set; }

        /* AMOUNT: */
        [Required(ErrorMessage = "The {0} is required.")]
        public decimal Amount { get; set; }

        /* PAID AMOUNT: */
        [Required(ErrorMessage = "The {0} is required.")]
        public decimal PaidAmount { get; set; }

        /* PAYMENT DATE: */
        [DataType(DataType.DateTime)]
        public DateTime? PaidAt { get; set; }

        /* STATUS: */
        [Required(ErrorMessage = "The {0} is required.")]
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