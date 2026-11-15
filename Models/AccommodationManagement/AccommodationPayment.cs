using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AccommodationManagement
{
    /*
    AccommodationPayment:
     * AccommodationPaymentId     int
     * AccommodationContractId    int
     * DueDate                    DateOnly
     * Amount                     decimal
     * PaidAmount                 decimal
     * PaidAt                     DateTime?
     * Status                     PaymentStatus
    */

    /// AccommodationPayment model class from Accommodation Management domain.

    public class AccommodationPayment
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
    }

    public enum PaymentStatus
    {
        Pending,
        Paid,
        Overdue
    }
}