using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models.AccommodationManagement
{
    public class AccommodationContract : IValidatableObject
    {
        public int AccommodationContractId { get; set; }

        [Required(ErrorMessage = "A {0} é obrigatória.")]
        [Display(Name = "Atribuição de Quarto")]
        public int RoomAllocationId { get; set; }
        // public RoomAllocation? RoomAllocation { get; set; } // Descomenta se a entidade existir

        [Required(ErrorMessage = "A {0} é obrigatória.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Início")]
        public DateOnly StartDate { get; set; }

        [Required(ErrorMessage = "A {0} é obrigatória.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Fim")]
        public DateOnly EndDate { get; set; }

        [Required(ErrorMessage = "A {0} é obrigatória.")]
        [Range(0, double.MaxValue, ErrorMessage = "A {0} não pode ser negativa.")]
        [Column(TypeName = "decimal(18,2)")]
        [DataType(DataType.Currency)]
        [Display(Name = "Renda Mensal")]
        public decimal MonthlyRent { get; set; }

        [Required(ErrorMessage = "A {0} é obrigatória.")]
        [Range(0, double.MaxValue, ErrorMessage = "A {0} não pode ser negativa.")]
        [Column(TypeName = "decimal(18,2)")]
        [DataType(DataType.Currency)]
        [Display(Name = "Caução")]
        public decimal Deposit { get; set; }

        [Required(ErrorMessage = "O {0} é obrigatório.")]
        [Display(Name = "Estado")]
        public int ContractStatusId { get; set; }

        [ForeignKey(nameof(ContractStatusId))]
        public ContractStatus? Status { get; set; }

        [Display(Name = "Documento Assinado")]
        public int? SignedDocumentId { get; set; }

        public decimal CalculateTotalRent()
        {
            if (EndDate <= StartDate) return 0;

            var months = (EndDate.Year - StartDate.Year) * 12 + (EndDate.Month - StartDate.Month);

            // Se o dia de fim for maior que o dia de início (ex: 01/09 a 30/11), soma mais 1 mês
            if (EndDate.Day > StartDate.Day)
            {
                months++;
            }

            return months * MonthlyRent;
        }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndDate <= StartDate)
            {
                yield return new ValidationResult(
                    "A data de fim tem de ser posterior à data de início.",
                    new[] { nameof(EndDate) });
            }
        }
    }

    public class ContractStatus
    {
        public int ContractStatusId { get; set; }

        [Required(ErrorMessage = "O {0} é obrigatório.")]
        [StringLength(50, ErrorMessage = "O {0} não pode ter mais de {1} caracteres.")]
        [Display(Name = "Nome")]
        public required string Name { get; set; }

        public ICollection<AccommodationContract> AccommodationContracts { get; set; } = new List<AccommodationContract>();
    }
}