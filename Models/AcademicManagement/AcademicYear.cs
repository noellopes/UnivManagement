using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AcademicManagement;

/// <summary>
/// Academic year model class from Academic Management domain.
/// Represents a consecutive academic year interval, e.g. 2024/2025.
/// </summary>
public class AcademicYear {
    public int AcademicYearId { get; set; }

    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [Display(Name = "Início")]
    public int StartYear { get; set; }

    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [Display(Name = "Fim")]
    public int EndYear { get; set; }
}