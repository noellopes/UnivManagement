using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AccommodationManagement;

/// <summary>
/// Residence model class from Accommodation Management domain.
/// </summary>

public class Residence
{
    /* ID: */
    public int ResidenceId { get; set; }

    /* NOME: */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [StringLength(60, ErrorMessage = "O {0} não pode ter mais de {1} caracteres. | The {0} cannot be longer than {1} characters.")]
    [Display(Name = "Nome")]
    public required string Name { get; set; }

    /* MORADA: */

    [Required(ErrorMessage = "A {0} é obrigatória. | The {0} is required.")]
    [StringLength(100, ErrorMessage = "A {0} não pode ter mais de {1} caracteres. | The {0} cannot be longer than {1} characters.")]
    [Display(Name = "Morada")]
    public required string Address { get; set; }

    /* ESTA ATIVO: */

    [Display(Name = "Ativa")]
    public bool IsActive { get; set; } = true;
}