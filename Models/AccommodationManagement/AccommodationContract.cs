using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models.AccommodationManagement
{
    /// <summary>
    /// AccommodationContract model class from Accommodation Management domain.
    /// </summary>
    public class AccommodationContract : IValidatableObject
    {
        /* ID: */
        public int AccommodationContractId { get; set; }

        /* STUDENT ID: */
        [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
        [Display(Name = "Estudante")]
        public int StudentId { get; set; }

        /* ROOM ALLOCATION ID: */
        [Required(ErrorMessage = "A {0} é obrigatória. | The {0} is required.")]
        [Display(Name = "Atribuição de Quarto")]
        public int RoomAllocationId { get; set; }

        /* SIGNATURE DATE: */
        [DataType(DataType.Date)]
        [Display(Name = "Data de Assinatura")]
        public DateTime? SignatureDate { get; set; }

        /* START DATE: */
        [Required(ErrorMessage = "A {0} é obrigatória. | The {0} is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Início")]
        public DateTime StartDate { get; set; }

        /* END DATE: */
        [Required(ErrorMessage = "A {0} é obrigatória. | The {0} is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Fim")]
        public DateTime EndDate { get; set; }

        /* STATUS: */
        [Display(Name = "Estado")]
        public ContractStatus Status { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndDate <= StartDate)
            {
                yield return new ValidationResult(
                    "A data de fim tem de ser posterior à data de início. | The end date must be after the start date.",
                    new[] { nameof(EndDate) });
            }
        }
    }

    public enum ContractStatus
    {
        Pending,
        Active,
        Terminated,
        Expired
    }
}
