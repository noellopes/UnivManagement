using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AccommodationManagement;

public class Room1
{
    public int RoomId { get; set; }

    [Display(Name = "Residência")]
    public int ResidenceId { get; set; }

    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [StringLength(10, ErrorMessage = "O {0} não pode ter mais de {1} caracteres.")]
    [Display(Name = "Número")]
    public required string Number { get; set; }

    [Display(Name = "Piso")]
    public int Floor { get; set; }

    [Range(0, 6, ErrorMessage = "A {0} deve estar entre {1} e {2}.")]
    [Display(Name = "Ocupação Atual")]
    public int CurrentOccupancy { get; set; }

    [Range(1, 6, ErrorMessage = "A {0} deve estar entre {1} e {2}.")]
    [Display(Name = "Capacidade")]
    public int Capacity { get; set; }

    [Display(Name = "Tipo")]
    public int RoomTypeId { get; set; }
    public RoomType? RoomType { get; set; }

    [Display(Name = "Estado")]
    public int RoomStatusId { get; set; }
    public RoomStatus? RoomStatus { get; set; }
}

public class RoomType
{
    public int RoomTypeId { get; set; }

    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [StringLength(50, ErrorMessage = "O {0} não pode ter mais de {1} caracteres.")]
    [Display(Name = "Nome")]
    public required string Name { get; set; }

    public ICollection<Room1> Rooms { get; set; } = new List<Room1>();
}

public class RoomStatus
{
    public int RoomStatusId { get; set; }

    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [StringLength(50, ErrorMessage = "O {0} não pode ter mais de {1} caracteres.")]
    [Display(Name = "Nome")]
    public required string Name { get; set; }

    public ICollection<Room1> Rooms { get; set; } = new List<Room1>();
}