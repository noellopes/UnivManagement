using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AcademicManagement;

/// <summary>
/// Student inscription in a curricular unit for a specific enrollment.
/// </summary>
public class InscrUC : IValidatableObject {
    public int InscrUCId { get; set; }

    [Required(ErrorMessage = "A {0} é obrigatória.")]
    [DataType(DataType.Date)]
    [Display(Name = "Data de Inscrição")]
    public DateOnly DateInscr { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Data de Fim")]
    public DateOnly? DateEnd { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "A {0} é obrigatória.")]
    [Display(Name = "Unidade Curricular")]
    public int UcId { get; set; }

    public UC? UC { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "A {0} é obrigatória.")]
    [Display(Name = "Inscrição no Curso")]
    public int EnrollmentId { get; set; }

    public Enrollment? Enrollment { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "O {0} é obrigatório.")]
    [Display(Name = "Estado da Inscrição")]
    public int StateInscrUCId { get; set; }

    public StateInscrUC? StateInscrUC { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
        if (DateEnd.HasValue && DateEnd.Value < DateInscr) {
            yield return new ValidationResult(
                "A data de fim não pode ser anterior à data de inscrição.",
                new[] { nameof(DateEnd) }
            );
        }
    }
}
