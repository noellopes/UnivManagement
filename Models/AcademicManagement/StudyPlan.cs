using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AcademicManagement;

/// <summary>
/// Study plan model class from Academic Management domain.
/// </summary>
public class StudyPlan {
    public int StudyPlanId { get; set; }

    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [StringLength(6, MinimumLength = 1, ErrorMessage = "O {0} não pode ter mais de {1} caracteres.")]
    [RegularExpression(@"^[A-Za-z0-9]{1,6}$", ErrorMessage = "O {0} deve conter entre 1 e 6 caracteres alfanuméricos.")]
    [Display(Name = "Código")]
    public string Code { get; set; } = string.Empty;

    [Range(0, 500, ErrorMessage = "O {0} deve estar entre {1} e {2}.")]
    [Display(Name = "ECTS Total")]
    public int EctsTotal { get; set; }

    [Range(1, 5, ErrorMessage = "O {0} deve estar entre {1} e {2}.")]
    [Display(Name = "Número de Anos")]
    public int NumYears { get; set; }

    [Required(ErrorMessage = "A {0} é obrigatória.")]
    [DataType(DataType.Date)]
    [Display(Name = "Data de Criação")]
    public DateOnly CreationDate { get; set; }

    [Display(Name = "Início de Vigor")]
    public int StartVigor { get; set; }

    [Display(Name = "Fim de Vigor")]
    public int EndVigor { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "O {0} é obrigatório.")]
    [Display(Name = "Curso")]
    public int CourseId { get; set; }

    public Course? Course { get; set; }
}
