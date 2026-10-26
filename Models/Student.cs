using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models;

/*
Student:
 * IdStudent    int
 * NumStudent   decimal     (7 digits max)
 * Name         string      (60 characters max)
 * Gender       string      (1 character, 'M' / 'F')
 * Nationality  string      (20 characters max)
 * BirthDate    DateTime    (Date type without Time)
 * Address      string      (60 characters max)
 * NIF          decimal     (9 digits max)
 * Email        string      (45 characters max)
 * Telephone    string      (9 digit characters)
*/

/// <summary>
/// Student model class from Academic Management domain.
/// </summary>
public class Student {
    /* ID: */
    public required int StudentId { get; set; }

    /* STUDENT NUMBER: */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [Display(Name = "Número de Aluno")]
    [Range(1000000, 9999999, ErrorMessage = "Número de aluno deve ter 7 dígitos | Student number must have 7 digits.")]
    public required decimal NumStudent { get; set; }

    /* NAME: */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [StringLength(60, ErrorMessage = "O {0} não pode ter mais de {1} caracteres. | The {0} cannot be longer than {1} characters.")]
    [Display(Name = "Nome")]
    public required string Name { get; set; }

    /* GENDER: */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [StringLength(1, ErrorMessage = "O {0} deve ter apenas {1} carácter. | The {0} must contain only {1} character.")]
    [RegularExpression("^[MF]$", ErrorMessage = "Sexo deve ser 'M' ou 'F'. | Gender must be 'M' or 'F'.")]
    [Display(Name = "Sexo")]
    public required string Gender { get; set; }

    /* NATIONALITY: */
    [Required(ErrorMessage = "A {0} é obrigatória. | The {0} is required.")]
    [StringLength(20, ErrorMessage = "A {0} não pode ter mais de {1} caracteres. | The {0} cannot be longer than {1} characters.")]
    [Display(Name = "Nacionalidade")]
    public required string Nationality { get; set; }

    /* BIRTH DATE: */
    [Required(ErrorMessage = "A {0} é obrigatória. | The {0} is required.")]
    [DataType(DataType.Date)]
    [Display(Name = "Data de Nascimento")]
    public required DateTime BirthDate { get; set; }

    /* ADDRESS: */
    [Required(ErrorMessage = "A {0} é obrigatória. | The {0} is required.")]
    [StringLength(60, ErrorMessage = "A {0} não pode ter mais de {1} caracteres. | The {0} cannot be longer than {1} characters.")]
    [Display(Name = "Morada")]
    public required string Address { get; set; }

    /* NIF: */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [Display(Name = "NIF")]
    [Range(0, 999999999, ErrorMessage = "O {0} deve ter 9 dígitos no máximo | {0} must have the maximum of 9 digits.")]
    public required decimal NIF { get; set; }

    /* EMAIL: */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [StringLength(45, ErrorMessage = "O {0} não pode ter mais de {1} caracteres. | The {0} cannot be longer than {1} characters.")]
    [EmailAddress(ErrorMessage = "O {0} não é válido. | The {0} is not valid.")]
    [Display(Name = "Email")]
    public required string Email { get; set; }

    /* TELEPHONE: */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [Display(Name = "Telefone")]
    [RegularExpression("^[0-9]{9}$", ErrorMessage = "O {0} deve conter exatamente 9 dígitos. | The {0} must contain exactly 9 digits.")]
    public required string Telephone { get; set; }
}