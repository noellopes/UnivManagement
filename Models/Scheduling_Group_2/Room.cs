using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace UnivManagement.Models.Scheduling_Group_2
{
    public class Room
    {
        [Key]
        public int IdSala { get; set; }

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
        public int IdEdificio { get; set; }

        [ForeignKey("IdEdificio")]
        public Building? Edificio { get; set; }

        // relacao tipo de sala
        [Display(Name = "Tipo de Sala")]
        public int IdTipoSala { get; set; }

        [ForeignKey("IdTipoSala")]
        public RoomType? TipoSala { get; set; }
    }
}