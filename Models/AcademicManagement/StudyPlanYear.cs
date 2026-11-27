using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AcademicManagement;

/// <summary>
/// Curricular year within a study plan, including its ECTS requirements.
/// </summary>
public class StudyPlanYear
{
    public int StudyPlanYearId { get; set; }

    [Range(1, 5, ErrorMessage = "O {0} deve estar entre {1} e {2}.")]
    [Display(Name = "Ano Curricular")]
    public int NumYear { get; set; }

    [Range(0, 500, ErrorMessage = "O {0} deve estar entre {1} e {2}.")]
    [Display(Name = "ECTS Total do Ano")]
    public int ECTSTotalYear { get; set; }

    [Range(0, 500, ErrorMessage = "O {0} deve estar entre {1} e {2}.")]
    [Display(Name = "ECTS Mínimo para Progressão")]
    public int MinECTS { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "O {0} é obrigatório.")]
    [Display(Name = "Plano de Estudos")]
    public int StudyPlanId { get; set; }

    public StudyPlan? StudyPlan { get; set; }
}
