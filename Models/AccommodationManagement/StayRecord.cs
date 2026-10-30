using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AccommodationManagement;

public class StayRecord
{
    public int StayRecordId { get; set; }

    [Display(Name = "Alocação de Quarto")]
    public int RoomAllocationId { get; set; }

    [Display(Name = "Tipo")]
    public StayType Type { get; set; }

    [Display(Name = "Ocorrido em")]
    public DateTime OccurredAt { get; set; } = DateTime.Now;

    [StringLength(500, ErrorMessage = "O {0} não pode ter mais de {1} caracteres. | The {0} cannot be longer than {1} characters.")]
    [Display(Name = "Notas de Condição")]
    public string? ConditionNotes { get; set; }

    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [StringLength(100, ErrorMessage = "O {0} não pode ter mais de {1} caracteres. | The {0} cannot be longer than {1} characters.")]
    [Display(Name = "Registado por")]
    public required string RecordedBy { get; set; }
}

public enum StayType
{
    CheckIn,
    CheckOut
}