using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AccommodationManagement;

public class AccommodationApplication
{
    public int AccommodationApplicationId { get; set; }

    [Display(Name = "Estudante")]
    public int StudentId { get; set; }

    [Display(Name = "Ano Letivo")]
    public int AcademicYearId { get; set; }

    [Display(Name = "Estado")]
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Submitted;

    [Display(Name = "Submetida em")]
    public DateTime SubmittedAt { get; set; } = DateTime.Now;

    [Display(Name = "Prioridade de Bolsa")]
    public bool HasScholarshipPriority { get; set; }

    [Display(Name = "Verificada em")]
    public DateTime? PriorityCheckedAt { get; set; }

    [StringLength(200, ErrorMessage = "O {0} não pode ter mais de {1} caracteres. | The {0} cannot be longer than {1} characters.")]
    [Display(Name = "Motivo")]
    public string? PriorityOverrideReason { get; set; }
}

public enum ApplicationStatus
{
    Submitted,
    UnderReview,
    Approved,
    Rejected,
    Withdrawn
}