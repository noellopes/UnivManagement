using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace UnivManagement.Models.AccommodationManagement;


/// Room model class from Accommodation Management domain.

public class Room
{
    /* ID: */
    public int RoomId { get; set; }

    /* RESIDENCE ID: */
    [Display(Name = "Residência")]
    public int ResidenceId { get; set; }
    

    /* NUMBER: */
    [Required(ErrorMessage = "O {0} é obrigatório. ")]
    [StringLength(10, ErrorMessage = "O {0} não pode ter mais de {1} caracteres. ")]
    [Display(Name = "Número")]
    public required string Number { get; set; }

    /* FLOOR: */
    [Display(Name = "Piso")]
    public int Floor { get; set; }

    /* CAPACITY: */
    [Range(1, 6, ErrorMessage = "A {0} deve estar entre {1} e {2}. ")]
    [Display(Name = "Capacidade")]
    public int Capacity { get; set; }

    /* ROOM TYPE ID: */
    [Display(Name = "Tipo")]
    public int RoomTypeId { get; set; }
    public RoomType? RoomType { get; set; }

    /* ROOM STATUS ID: */
    [Display(Name = "Estado")]
    public int RoomStatusId { get; set; }
    public RoomStatus? RoomStatus { get; set; }
}

/// Room type (e.g. Single, Double, Triple).

public class RoomType
{
    public int RoomTypeId { get; set; }

    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [StringLength(50, ErrorMessage = "O {0} não pode ter mais de {1} caracteres. ")]
    [Display(Name = "Nome")]
    public required string Name { get; set; }

    public ICollection<Room> Rooms { get; set; } = new List<Room>();
}


/// Room status (e.g. Available, Maintenance).

public class RoomStatus
{
    public int RoomStatusId { get; set; }

    [Required(ErrorMessage = "O {0} é obrigatório. ")]
    [StringLength(50, ErrorMessage = "O {0} não pode ter mais de {1} caracteres. ")]
    [Display(Name = "Nome")]
    public required string Name { get; set; }

    public ICollection<Room> Rooms { get; set; } = new List<Room>();
}