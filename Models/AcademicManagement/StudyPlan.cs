using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AcademicManagement;

/*
StudyPlan:
 * StudyPlanId       int
 * EctsTotal         int         (value between 120 and 180)
 * EctsTotalYear     int         (value between 50 and 60)
 * MinEctsYear2      int         (value between 30 and 60)
 * MinEctsYear3      int         (value between 60 and 120)
 * NumYears          int         (value between 2 and 3)
 * CreationDate      DateOnly
*/

/// <summary>
/// Study Plan model class from Academic Management domain.
/// </summary>
public class StudyPlan {
    /* ID: */
    [Display(Name = "ID")]
    public required int StudyPlanId { get; set; }

    /* TOTAL ECTS: */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [Range(120, 180, ErrorMessage = "O {0} deve estar entre {1} e {2} | The {0} should be between {1} and {2}.")]
    [Display(Name = "ECTS Total")]
    public required int EctsTotal { get; set; }

    /* TOTAL ECTS PER YEAR: */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [Range(50, 60, ErrorMessage = "O {0} deve estar entre {1} e {2} | The {0} should be between {1} and {2}.")]
    [Display(Name = "ECTS Total por Ano")]
    public required int EctsTotalYear { get; set; }

    /* MIN ECTS TO PROGRESS TO THE 2º YEAR: */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [Range(30, 60, ErrorMessage = "O {0} deve estar entre {1} e {2} | The {0} should be between {1} and {2}.")]
    [Display(Name = "Min ECTS para 2º Ano")]
    public required int MinEctsYear2 { get; set; }

    /* MIN ECTS TO PROGRESS TO THE 3º YEAR: */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [Range(60, 120, ErrorMessage = "O {0} deve estar entre {1} e {2} | The {0} should be between {1} and {2}.")]
    [Display(Name = "Min ECTS para 3º Ano")]
    public required int MinEctsYear3 { get; set; }

    /* TOTAL NUMBER OF YEARS IN STUDY PLAN: */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [Range(2, 3, ErrorMessage = "O {0} deve estar entre {1} e {2} | The {0} should be between {1} and {2}.")]
    [Display(Name = "Número de Anos")]
    public required int NumYears { get; set; }

    /* STUDY PLAN CREATION DATE: */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [DataType(DataType.Date)]
    [Display(Name = "Data de Criação")]
    public required DateOnly CreationDate { get; set; }
}