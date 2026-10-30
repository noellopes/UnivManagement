using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AccommodationManagement;

    /// <summary>
    /// Room model class from Accommodation Management domain.
    /// </summary>
public class Room
{

    /* ID: */
    public int RoomId { get; set; }

    /* RESIDENCE ID: */
    [Display(Name = "Residência")]
    public int ResidenceId { get; set; }

    /* NUMBER: */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [StringLength(10, ErrorMessage = "O {0} não pode ter mais de {1} caracteres. | The {0} cannot be longer than {1} characters.")]
    [Display(Name = "Número")]
    public required string Number { get; set; }

    /* FLOOR: */
    [Display(Name = "Piso")]
    public int Floor { get; set; }

    /* CAPACITY: */
    [Range(1, 6, ErrorMessage = "A {0} deve estar entre {1} e {2}. | The {0} must be between {1} and {2}.")]
    [Display(Name = "Capacidade")]
    public int Capacity { get; set; }

    /* TYPE: */
    [Display(Name = "Tipo")]
    public RoomType Type { get; set; }

    /* STATUS: */
    [Display(Name = "Estado")]
    public RoomStatus Status { get; set; }
}

public enum RoomType { Single, Double, Triple }

public enum RoomStatus { Available, Maintenance }