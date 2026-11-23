using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AcademicManagement;

/// <summary>
/// Academic degree reference entity from Academic Management domain.
/// Records are predefined and must be seeded at database initialization.
/// </summary>
public class AcademicDegree
{
    public int AcademicDegreeId { get; set; }

    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [StringLength(100, ErrorMessage = "O {0} não pode ter mais de {1} caracteres.")]
    [Display(Name = "Grau Académico")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "A {0} não pode ter mais de {1} caracteres.")]
    [Display(Name = "Descrição")]
    public string? Description { get; set; }
}
