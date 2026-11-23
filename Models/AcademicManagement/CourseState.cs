using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AcademicManagement;

/// <summary>
/// Lifecycle state of an academic course.
/// Records are predefined and must be seeded at database initialization.
/// </summary>
public class CourseState
{
    public int CourseStateId { get; set; }

    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [StringLength(50, ErrorMessage = "O {0} não pode ter mais de {1} caracteres.")]
    [Display(Name = "Estado do Curso")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "A {0} não pode ter mais de {1} caracteres.")]
    [Display(Name = "Descrição")]
    public string? Description { get; set; }
}
