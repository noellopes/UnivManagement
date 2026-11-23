using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AcademicManagement;

/// <summary>
/// Enrollment model class from Academic Management domain.
/// Represents a student's participation in a course and owns its UC inscriptions
/// and academic progression history.
/// </summary>
public class Enrollment : IValidatableObject
{
    public int EnrollmentId { get; set; }

    [Required(ErrorMessage = "A {0} é obrigatória.")]
    [DataType(DataType.Date)]
    [Display(Name = "Data de Inscrição")]
    public DateOnly DateInscr { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Data de Graduação")]
    public DateOnly? DateGrad { get; set; }

    [Display(Name = "Propina Paga")]
    public bool CourseFeePaid { get; set; }

    [Range(0, 300, ErrorMessage = "O {0} deve estar entre {1} e {2}.")]
    [Display(Name = "Soma de ECTS")]
    public int SumECTS { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "O {0} é obrigatório.")]
    [Display(Name = "Aluno")]
    public int StudentId { get; set; }

    public Student? Student { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "O {0} é obrigatório.")]
    [Display(Name = "Estado da Inscrição")]
    public int StateEnrollmentId { get; set; }

    public StateEnrollment? StateEnrollment { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "O {0} é obrigatório.")]
    [Display(Name = "Plano de Estudos")]
    public int StudyPlanId { get; set; }

    public StudyPlan? StudyPlan { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DateGrad.HasValue && DateGrad.Value < DateInscr)
        {
            yield return new ValidationResult(
                "A data de graduação não pode ser anterior à data de inscrição.",
                new[] { nameof(DateGrad) });
        }
    }
}
