using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AccommodationManagement;

/// <summary>
/// AccommodationApplication model class from Accommodation Management domain.
/// </summary>
public class AccommodationApplication
{
    /* ID: */
    public int AccommodationApplicationId { get; set; }

    /* STUDENT ID: */
    [Display(Name = "Estudante")]
    public int StudentId { get; set; }

    /* ACADEMIC YEAR ID: */
    [Display(Name = "Ano Letivo")]
    public int AcademicYearId { get; set; }

    /* STATUS: */
    [Display(Name = "Estado")]
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Submitted;

    /* SUBMITTED AT: */
    [Display(Name = "Submetida em")]
    public DateTime SubmittedAt { get; set; } = DateTime.Now;

    /* HAS SCHOLARSHIP PRIORITY: */
    [Display(Name = "Prioridade de Bolsa")]
    public bool HasScholarshipPriority { get; set; }

    /* PRIORITY CHECKED AT (Opcional): */
    [Display(Name = "Verificada em")]
    public DateTime? PriorityCheckedAt { get; set; }

    /* PRIORITY OVERRIDE REASON (Opcional): */
    [StringLength(200, ErrorMessage = "O {0} não pode ter mais de {1} caracteres. | The {0} cannot be longer than {1} characters.")]
    [Display(Name = "Motivo de Alteração de Prioridade")]
    public string? PriorityOverrideReason { get; set; }
}

// Enum declarado no fim do ficheiro
public enum ApplicationStatus
{
    Submitted,
    UnderReview,
    Approved,
    Rejected,
    Withdrawn
}
