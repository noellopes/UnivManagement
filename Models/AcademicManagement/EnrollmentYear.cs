using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AcademicManagement;

/// <summary>
/// Records the curricular year an enrollment corresponds to during a specific AcademicYear.
/// Historical records must be preserved.
/// </summary>
public class EnrollmentYear
{
    public int EnrollmentYearId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "O {0} é obrigatório.")]
    [Display(Name = "Inscrição no Curso")]
    public int EnrollmentId { get; set; }

    public Enrollment? Enrollment { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "O {0} é obrigatório.")]
    [Display(Name = "Ano Letivo")]
    public int AcademicYearId { get; set; }

    public AcademicYear? AcademicYear { get; set; }

    [Range(1, 5, ErrorMessage = "O {0} deve estar entre {1} e {2}.")]
    [Display(Name = "Ano Curricular")]
    public int Year { get; set; }
}
