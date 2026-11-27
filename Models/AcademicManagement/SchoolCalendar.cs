using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AcademicManagement;

/// <summary>
/// School calendar model class from Academic Management domain.
/// A school calendar belongs to one AcademicYear and contains one or more Semesters.
/// </summary>
public class SchoolCalendar : IValidatableObject {
    public int SchoolCalendarId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "O {0} é obrigatório.")]
    [Display(Name = "Ano Letivo")]
    public int AcademicYearId { get; set; }
    public AcademicYear? AcademicYear { get; set; }

    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [RegularExpression(@"^[A-Za-z0-9]{1,6}$", ErrorMessage = "O {0} deve conter entre 1 e 6 caracteres alfanuméricos.")]
    [Display(Name = "Código")]
    public string Code { get; set; } = string.Empty;

    [Range(1, 10, ErrorMessage = "O {0} deve estar entre {1} e {2}.")]
    [Display(Name = "Total de Semestres")]
    public int TotalSemesters { get; set; }

    [Required(ErrorMessage = "A {0} é obrigatória.")]
    [DataType(DataType.Date)]
    [Display(Name = "Início do Calendário")]
    public DateOnly StartDate { get; set; }

    [Required(ErrorMessage = "A {0} é obrigatória.")]
    [DataType(DataType.Date)]
    [Display(Name = "Fim do Calendário")]
    public DateOnly EndDate { get; set; }

    [Required(ErrorMessage = "A {0} é obrigatória.")]
    [DataType(DataType.Date)]
    [Display(Name = "Início dos Exames Especiais")]
    public DateOnly StartSpecialExam { get; set; }

    [Required(ErrorMessage = "A {0} é obrigatória.")]
    [DataType(DataType.Date)]
    [Display(Name = "Fim dos Exames Especiais")]
    public DateOnly EndSpecialExam { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
        if (EndDate < StartDate) {
            yield return new ValidationResult(
                "A data de fim do calendário não pode ser anterior à data de início.",
                new[] { nameof(EndDate) });
        }

        if (EndSpecialExam < StartSpecialExam) {
            yield return new ValidationResult(
                "A data de fim dos exames especiais não pode ser anterior à data de início.",
                new[] { nameof(EndSpecialExam) });
        }
    }
}
