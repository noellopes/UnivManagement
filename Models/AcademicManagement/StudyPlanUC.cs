using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AcademicManagement;

/*
StudyPlanUC:
 * StudyPlanUCId     int
 * StudyPlanId       int         (references StudyPlan)
 * UcId              int         (references UC)
 * Year              int         (value between 1 and 3)
 * Semester          int         (value between 1 and 2, i.e. First Semester / Second Semester)
 * ValueEcts         int         (value between 1 and 10)
*/

/// <summary>
/// Study Plan UC model class from Academic Management domain.
/// Serves to attribute various UCs to a Study Plan.
/// </summary>
public class StudyPlanUC {
    /* ID: */
    [Display(Name = "ID")]
    public required int StudyPlanUCId { get; set; }

    /* REF ID STUDY PLAN: */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [Display(Name = "ID Plano de Estudos")]
    public required int StudyPlanId { get; set; }

    /* REF ID UC: */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [Display(Name = "ID UC")]
    public required int UcId { get; set; }

    /* YEAR IN WHICH UC IS PLACED IN STUDY PLAN: */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [Range(1, 3, ErrorMessage = "O {0} deve estar entre {1} e {2} | The {0} should be between {1} and {2}.")]
    [Display(Name = "Ano")]
    public required int Year { get; set; }

    /* SEMESTER IN WHICH UC IS PLACED IN STUDY PLAN: */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [Range(1, 2, ErrorMessage = "O {0} deve estar entre {1} e {2} | The {0} should be between {1} and {2}.")]
    [Display(Name = "Semestre")]
    public required int Semester { get; set; }

    /* HOW MUCH ECTS THIS UC COST IN STUDY PLAN: */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [Range(1, 10, ErrorMessage = "O {0} deve estar entre {1} e {2} | The {0} should be between {1} and {2}.")]
    [Display(Name = "Valor ECTS")]
    public required int ValueEcts { get; set; }
}