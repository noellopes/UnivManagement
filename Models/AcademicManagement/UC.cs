using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AcademicManagement;

/// <summary>
/// Curricular unit model class from Academic Management domain.
/// A UC is an independent entity associated with a StudyPlan.
/// </summary>
public class UC {
    public int UcId { get; set; }

    [Required(ErrorMessage = "O {0} da UC é obrigatório.")]
    [StringLength(6, MinimumLength = 1, ErrorMessage = "O {0} não pode ter mais de {1} caracteres.")]
    [RegularExpression(@"^[A-Za-z0-9]{1,6}$", ErrorMessage = "O {0} deve conter entre 1 e 6 caracteres alfanuméricos.")]
    [Display(Name = "Código")]
    public string Code { get; set; } = string.Empty;

    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [StringLength(100, ErrorMessage = "O {0} não pode ter mais de {1} caracteres.")]
    [Display(Name = "Nome da UC")]
    public string Name { get; set; } = string.Empty;

    [StringLength(10, ErrorMessage = "A {0} não pode ter mais de {1} caracteres.")]
    [Display(Name = "Sigla")]
    public string? Acronym { get; set; }

    [StringLength(500, ErrorMessage = "A {0} não pode ter mais de {1} caracteres.")]
    [Display(Name = "Descrição")]
    public string? Description { get; set; }

    [Range(0, 500, ErrorMessage = "O {0} deve estar entre {1} e {2}.")]
    [Display(Name = "Valor ECTS")]
    public int ValueECTS { get; set; }

    [Range(0, 1000, ErrorMessage = "O {0} deve estar entre {1} e {2}.")]
    [Display(Name = "Número de Horas")]
    public int NumHours { get; set; }

    [Range(1, 10, ErrorMessage = "O {0} deve estar entre {1} e {2}.")]
    [Display(Name = "Semestre")]
    public int Semester { get; set; }

    [Range(1, 5, ErrorMessage = "O {0} deve estar entre {1} e {2}.")]
    [Display(Name = "Ano")]
    public int Year { get; set; }

    [Display(Name = "Plano de Estudos ID")]
    public int? StudyPlanId { get; set; } // Optional.

    public StudyPlan? StudyPlan { get; set; }
}
