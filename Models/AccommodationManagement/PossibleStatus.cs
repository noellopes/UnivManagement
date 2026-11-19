using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AccommodationManagement;

public class PossibleStatus
{
    [Key]
    public int PossibleStatusId { get; set; }

    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [StringLength(50, ErrorMessage = "O {0} não pode ter mais de {1} caracteres. | The {0} cannot be longer than {1} characters.")]
    [Display(Name = "Estado")]
    public string Name { get; set; } = string.Empty;
}

