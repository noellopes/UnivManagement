using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AcademicManagement;

/// <summary>
/// Academic year model class from Academic Management domain.
/// Represents a consecutive academic year interval, e.g. 2024/2025.
/// </summary>
public class AcademicYear : IValidatableObject
{
    public int AcademicYearId { get; set; }

    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [Display(Name = "Início")]
    public int StartYear { get; set; }

    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [Display(Name = "Fim")]
    public int EndYear { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndYear != StartYear + 1)
        {
            yield return new ValidationResult(
                "O ano de fim deve ser exatamente o ano de início mais um.",
                new[] { nameof(EndYear) });
        }
    }
}
