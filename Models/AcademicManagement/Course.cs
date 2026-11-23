using System.ComponentModel.DataAnnotations;

using UnivManagement.Models; // Access to the external Docente model

namespace UnivManagement.Models.AcademicManagement;

/// <summary>
/// Academic course model class from Academic Management domain.
/// </summary>
public class Course {
    public int CourseId { get; set; }

    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [StringLength(6, MinimumLength = 1, ErrorMessage = "O {0} não pode ter mais de {1} caracteres.")]
    [RegularExpression(@"^[A-Za-z0-9]{1,6}$", ErrorMessage = "O {0} deve conter entre 1 e 6 caracteres alfanuméricos.")]
    [Display(Name = "Código")]
    public string Code { get; set; } = string.Empty;

    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [StringLength(100, ErrorMessage = "O {0} não pode ter mais de {1} caracteres.")]
    [Display(Name = "Nome do Curso")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [StringLength(10, ErrorMessage = "O {0} não pode ter mais de {1} caracteres.")]
    [Display(Name = "Sigla")]
    public string Acronym { get; set; } = string.Empty;

    [Required(ErrorMessage = "A {0} é obrigatória.")]
    [DataType(DataType.Date)]
    [Display(Name = "Data de Criação")]
    public DateOnly CreationDate { get; set; }

    [StringLength(1000, ErrorMessage = "A {0} não pode ter mais de {1} caracteres.")]
    [Display(Name = "Descrição")]
    public string? Description { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "O {0} é obrigatório.")]
    [Display(Name = "Grau Académico")]
    public int AcademicDegreeId { get; set; }

    public AcademicDegree? AcademicDegree { get; set; }

    [Display(Name = "Docente ID")]
    public int? DocenteId { get; set; } // Optional.

    public Docente? Docente { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "O {0} é obrigatório.")]
    [Display(Name = "Estado do Curso")]
    public int CourseStateId { get; set; }

    public CourseState? CourseState { get; set; }
}
