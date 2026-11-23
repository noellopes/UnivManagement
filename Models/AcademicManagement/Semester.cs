using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AcademicManagement;

/// <summary>
/// Semester model class from Academic Management domain.
/// Represents one academic period within a SchoolCalendar.
/// </summary>
public class Semester : IValidatableObject
{
    public int SemesterId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "O {0} é obrigatório.")]
    [Display(Name = "Calendário Escolar")]
    public int SchoolCalendarId { get; set; }

    public SchoolCalendar? SchoolCalendar { get; set; }

    [Range(1, 10, ErrorMessage = "O {0} deve estar entre {1} e {2}.")]
    [Display(Name = "Número do Semestre")]
    public int OrderNum { get; set; }

    [Required(ErrorMessage = "A {0} é obrigatória.")]
    [DataType(DataType.Date)]
    [Display(Name = "Início do Semestre")]
    public DateOnly StartSemester { get; set; }

    [Required(ErrorMessage = "A {0} é obrigatória.")]
    [DataType(DataType.Date)]
    [Display(Name = "Fim do Semestre")]
    public DateOnly EndSemester { get; set; }

    [Required(ErrorMessage = "A {0} é obrigatória.")]
    [DataType(DataType.Date)]
    [Display(Name = "Início dos Exames de Frequência")]
    public DateOnly StartFreqExam { get; set; }

    [Required(ErrorMessage = "A {0} é obrigatória.")]
    [DataType(DataType.Date)]
    [Display(Name = "Fim dos Exames de Frequência")]
    public DateOnly EndFreqExam { get; set; }

    [Required(ErrorMessage = "A {0} é obrigatória.")]
    [DataType(DataType.Date)]
    [Display(Name = "Início dos Exames de Recurso")]
    public DateOnly StartResitExam { get; set; }

    [Required(ErrorMessage = "A {0} é obrigatória.")]
    [DataType(DataType.Date)]
    [Display(Name = "Fim dos Exames de Recurso")]
    public DateOnly EndResitExam { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndSemester < StartSemester)
        {
            yield return new ValidationResult(
                "A data de fim do semestre não pode ser anterior à data de início.",
                new[] { nameof(EndSemester) });
        }

        if (EndFreqExam < StartFreqExam)
        {
            yield return new ValidationResult(
                "A data de fim dos exames de frequência não pode ser anterior à data de início.",
                new[] { nameof(EndFreqExam) });
        }

        if (EndResitExam < StartResitExam)
        {
            yield return new ValidationResult(
                "A data de fim dos exames de recurso não pode ser anterior à data de início.",
                new[] { nameof(EndResitExam) });
        }

        if (EndFreqExam >= StartResitExam)
        {
            yield return new ValidationResult(
                "O período de exames de frequência deve terminar antes do início dos exames de recurso.",
                new[] { nameof(StartResitExam) });
        }
    }
}
