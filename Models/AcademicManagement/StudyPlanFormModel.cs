using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AcademicManagement;

/// <summary>
/// Form model used by the StudyPlan Create and Edit tree editor.
/// The editor posts the plan data, an ordered list of year cards, and a flat
/// list of UC rows. Semester groups are presentation-only and are not posted
/// as independent entities.
/// </summary>
public class StudyPlanFormModel {
    public int StudyPlanId { get; set; }

    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [StringLength(6, MinimumLength = 1, ErrorMessage = "O {0} não pode ter mais de {1} caracteres.")]
    [RegularExpression(@"^[A-Za-z0-9]{1,6}$", ErrorMessage = "O {0} deve conter entre 1 e 6 caracteres alfanuméricos.")]
    [Display(Name = "Código")]
    public string Code { get; set; } = string.Empty;

    [Required(ErrorMessage = "A {0} é obrigatória.")]
    [DataType(DataType.Date)]
    [Display(Name = "Data de Criação")]
    public DateOnly? CreationDate { get; set; }

    [Display(Name = "Início de Vigor")]
    public int? StartVigor { get; set; }

    [Display(Name = "Fim de Vigor")]
    public int? EndVigor { get; set; }

    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [Range(1, int.MaxValue, ErrorMessage = "O {0} é obrigatório.")]
    [Display(Name = "Curso")]
    public int? CourseId { get; set; }

    /// <summary>Ordered year cards. The card position defines <c>NumYear</c>.</summary>
    public List<StudyPlanYearFormModel> Years { get; set; } = new();

    /// <summary>Complete desired list of UC rows attributed to the plan.</summary>
    public List<StudyPlanUcFormModel> Ucs { get; set; } = new();


    /// <summary>
    /// Edit-only snapshot of the year ids that were present when the page was
    /// loaded. Used to detect concurrent changes without a rowversion column.
    /// </summary>
    public string? OriginalYearIds { get; set; }

    /// <summary>
    /// Edit-only snapshot of the UC ids attributed to the plan when the page was
    /// loaded.
    /// </summary>
    public string? OriginalUcIds { get; set; }

    public bool IsEdit => StudyPlanId > 0;
}

/// <summary>One curricular year card in the StudyPlan editor.</summary>
public class StudyPlanYearFormModel {
    /// <summary><c>0</c> means the card is new and must be inserted.</summary>
    public int StudyPlanYearId { get; set; }

    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [Range(0, 500, ErrorMessage = "O {0} deve estar entre {1} e {2}.")]
    [Display(Name = "ECTS Total do Ano")]
    public int? ECTSTotalYear { get; set; } = 60;

    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [Range(0, 500, ErrorMessage = "O {0} deve estar entre {1} e {2}.")]
    [Display(Name = "ECTS Mínimo")]
    public int? MinECTS { get; set; } = 0;
}

/// <summary>
/// One UC row in the StudyPlan editor. <c>Year</c> and <c>Semester</c> are
/// derived from the containing year card and semester group.
/// </summary>
public class StudyPlanUcFormModel {
    /// <summary><c>0</c> means the row has no UC selected and is ignored.</summary>
    public int UcId { get; set; }

    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [Range(1, 5, ErrorMessage = "O {0} deve estar entre {1} e {2}.")]
    [Display(Name = "Ano")]
    public int? Year { get; set; }

    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [Range(1, 10, ErrorMessage = "O {0} deve estar entre {1} e {2}.")]
    [Display(Name = "Semestre")]
    public int? Semester { get; set; }

    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [Range(0, 500, ErrorMessage = "O {0} deve estar entre {1} e {2}.")]
    [Display(Name = "ECTS")]
    public int? ValueECTS { get; set; }
}

/// <summary>
/// Collection-level rules that data annotations cannot express.
/// Field-level ranges remain on the form model properties.
/// </summary>
public static class StudyPlanFormValidator {
    public const int MaxYears = 5;
    public const int MaxEcts = 500;
    public const int MaxSemester = 10;

    public static IReadOnlyList<string> ValidateStructure(StudyPlanFormModel model) {
        var errors = new List<string>();

        var years = model.Years ?? new List<StudyPlanYearFormModel>();
        var ucs = model.Ucs ?? new List<StudyPlanUcFormModel>();

        if (years.Count == 0) {
            errors.Add("O plano de estudos deve ter pelo menos um ano.");
        }
        else if (years.Count > MaxYears) {
            errors.Add($"O plano de estudos não pode ter mais de {MaxYears} anos.");
        }

        if (years.Sum(y => y.ECTSTotalYear ?? 0) > MaxEcts) {
            errors.Add($"O total de ECTS do plano não pode ser superior a {MaxEcts}.");
        }

        if (years.Any(y => y.StudyPlanYearId < 0)) {
            errors.Add("Um dos anos do plano tem um identificador inválido.");
        }

        var selected = ucs.Where(u => u.UcId > 0).ToList();

        if (selected.GroupBy(u => u.UcId).Any(g => g.Count() > 1)) {
            errors.Add("A mesma unidade curricular não pode ser adicionada mais do que uma vez.");
        }

        if (selected.Any(u => !u.Year.HasValue
                || u.Year.Value < 1
                || u.Year.Value > years.Count)) {
            errors.Add("Cada unidade curricular tem de estar num ano existente do plano.");
        }

        return errors.Distinct().ToList();
    }
}
