using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AcademicManagement;

/// <summary>
/// Student model class from Academic Management domain.
/// </summary>
public class Student : IValidatableObject {
    public int StudentId { get; set; }

    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [RegularExpression(@"^(1[0-9]{6}|2000000)$", ErrorMessage = "O {0} deve ser um número entre 1000000 e 2000000.")]
    [Display(Name = "Número de Aluno")]
    public string NumStudent { get; set; } = string.Empty;

    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [StringLength(60, ErrorMessage = "O {0} não pode ter mais de {1} caracteres.")]
    [Display(Name = "Nome")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [RegularExpression("^[MF]$", ErrorMessage = "Sexo deve ser 'M' ou 'F'.")]
    [Display(Name = "Sexo")]
    public string Gender { get; set; } = string.Empty;

    [Required(ErrorMessage = "A {0} é obrigatória.")]
    [StringLength(20, ErrorMessage = "A {0} não pode ter mais de {1} caracteres.")]
    [Display(Name = "Nacionalidade")]
    public string Nationality { get; set; } = string.Empty;

    [Required(ErrorMessage = "A {0} é obrigatória.")]
    [DataType(DataType.Date)]
    [Display(Name = "Data de Nascimento")]
    public DateOnly BirthDate { get; set; }

    [Required(ErrorMessage = "A {0} é obrigatória.")]
    [StringLength(60, ErrorMessage = "A {0} não pode ter mais de {1} caracteres.")]
    [Display(Name = "Morada")]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [Display(Name = "NIF")]
    public string NIF { get; set; } = string.Empty;

    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [StringLength(100, ErrorMessage = "O {0} não pode ter mais de {1} caracteres.")]
    [EmailAddress(ErrorMessage = "O {0} não é válido.")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [RegularExpression(@"^\+[0-9]{1,4}$", ErrorMessage = "O {0} deve começar por '+' e conter no máximo 4 dígitos.")]
    [Display(Name = "Indicativo do País")]
    public string CountryPhoneCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [RegularExpression("^[0-9]{9}$", ErrorMessage = "O {0} deve conter exatamente 9 dígitos.")]
    [Display(Name = "Telefone")]
    public string Telephone { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
        // A obrigatoriedade é tratada pelo [Required].
        if (string.IsNullOrWhiteSpace(NIF)) {
            yield break;
        }

        if (NIF.Length != 9 || !NIF.All(char.IsAsciiDigit)) {
            yield return new ValidationResult(
                "O NIF deve conter exatamente 9 dígitos.",
                new[] { nameof(NIF) });
        }
        else if (!IsValidNif(NIF)) {
            yield return new ValidationResult(
                "O NIF não é válido.",
                new[] { nameof(NIF) });
        }
    }

    /// <summary>
    /// Checks the format (nine digits) and the Modulo 11 check digit
    /// described in the Portuguese NIF validation rules.
    /// </summary>
    public static bool IsValidNif(string? nif) {
        if (nif is null || nif.Length != 9) return false;
        
        var sum = 0;
        for (var i = 0; i < 8; i++) {
            if (!char.IsAsciiDigit(nif[i])) return false;

            sum += (nif[i] - '0') * (9 - i);
        }

        if (!char.IsAsciiDigit(nif[8])) return false;

        var remainder = sum % 11;
        var expectedCheckDigit = remainder < 2 ? 0 : 11 - remainder;

        return expectedCheckDigit == nif[8] - '0';
    }
}
