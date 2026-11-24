using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace UnivManagement.Models
{
    public class Room
    {
        public int RoomId { get; set; }

        [Required(ErrorMessage = "Por favor, indique o {0}.")]
        [Display(Name = "Número / Identificador da Sala")]
        [StringLength(20, ErrorMessage = "O {0} não pode exceder {1} caracteres.")]
        public required string NumeroSala { get; set; }

        [Required(ErrorMessage = "Por favor, indique o {0}.")]
        [Display(Name = "Piso / Andar")]
        public int Piso { get; set; }

        [Required(ErrorMessage = "Por favor, indique a {0}.")]
        [Display(Name = "Lotação")]
        [Range(1, 500, ErrorMessage = "A {0} deve ser entre {1} e {2} lugares.")]
        public int Lotacao { get; set; }

        [Required(ErrorMessage = "Por favor, indique o {0}.")]
        [Display(Name = "Estado Operacional")]
        [StringLength(30)]
        public string Estado { get; set; } = "Ativa";

        // relacao com edificio
        [Display(Name = "Edifício")]
        public int BuildingId { get; set; }

        [ForeignKey("BuildingId")]
        public Building? Edificio { get; set; }

        // relacao tipo de sala
        [Display(Name = "Tipo de Sala")]
        public int RoomTypeId { get; set; }

        [ForeignKey("RoomTypeId")]
        public RoomType? TipoSala { get; set; }

    }
}