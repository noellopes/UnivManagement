using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AcademicManagement;

/*
Student:
 * IdStudent    int
 * NumStudent   string      (1000000–2000000)
 * Name         string      (60 characters max)
 * Gender       string      (1 character, 'M' / 'F')
 * Nationality  string      (20 characters max)
 * BirthDate    DateOnly    (Date type without Time)
 * Address      string      (60 characters max)
 * NIF          string      (9 digits max)
 * Email        string      (45 characters max)
 * CountryPhoneCode string      (2–4 chars, format +XXX)
 * Telephone    string      (4-14 digit characters)
*/

/// <summary>
/// Student model class from Academic Management domain.
/// </summary>
public class Student {
    /* ID: */
    public int StudentId { get; set; }

    /* STUDENT NUMBER: */
    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [Display(Name = "Número de Aluno")]
    [RegularExpression(@"^(1[0-9]{6}|2000000)$", ErrorMessage = "O {0} deve estar entre 1000000 e 2000000.")]
    public required string NumStudent { get; set; }

    /* NAME: */
    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [StringLength(60, ErrorMessage = "O {0} não pode ter mais de {1} caracteres.")]
    [Display(Name = "Nome")]
    public required string Name { get; set; }

    /* GENDER: */
    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [StringLength(1, ErrorMessage = "O {0} deve ter apenas {1} carácter.")]
    [RegularExpression("^[MF]$", ErrorMessage = "Sexo deve ser 'M' ou 'F'.")]
    [Display(Name = "Sexo")]
    public required string Gender { get; set; }

    /* NATIONALITY: */
    [Required(ErrorMessage = "A {0} é obrigatória.")]
    [StringLength(20, ErrorMessage = "A {0} não pode ter mais de {1} caracteres.")]
    [Display(Name = "Nacionalidade")]
    public required string Nationality { get; set; }

    /* BIRTH DATE: */
    [Required(ErrorMessage = "A {0} é obrigatória.")]
    [DataType(DataType.Date)]
    [Display(Name = "Data de Nascimento")]
    public DateOnly? BirthDate { get; set; }

    /* ADDRESS: */
    [Required(ErrorMessage = "A {0} é obrigatória.")]
    [StringLength(60, ErrorMessage = "A {0} não pode ter mais de {1} caracteres.")]
    [Display(Name = "Morada")]
    public required string Address { get; set; }

    /* NIF: */
    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [Display(Name = "NIF")]
    [RegularExpression(@"^[0-9]{1,9}$", ErrorMessage = "O {0} deve conter apenas dígitos (máximo 9).")]
    public required string NIF { get; set; }

    /* EMAIL: */
    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [StringLength(45, ErrorMessage = "O {0} não pode ter mais de {1} caracteres.")]
    [EmailAddress(ErrorMessage = "O {0} não é válido.")]
    [Display(Name = "Email")]
    public required string Email { get; set; }

    /* Country Phone Code: */
    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [RegularExpression(@"^\+[1-9][0-9]{0,2}$", ErrorMessage = "{0} internacional inválido (ex: +351).")]
    [Display(Name = "Indicativo")]
    public required string CountryPhoneCode { get; set; }

    /* TELEPHONE: */
    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [Display(Name = "Telefone")]
    [RegularExpression("^[0-9]{4,14}$", ErrorMessage = "O {0} deve conter entre 4 e 14 dígitos.")]
    public required string Telephone { get; set; }
}