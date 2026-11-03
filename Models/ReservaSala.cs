
using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class ReservaSala
    {
        public int ReservaSalaId { get; set; }

        [Required(ErrorMessage = "Por favor, insira o {0}.")]
        [StringLength(200, ErrorMessage = "O campo {0} não pode exceder {1} carateres.")]
        [Display(Name = "Responsável")]
        public required string Responsavel { get; set; }

        [Required(ErrorMessage = "Por favor, insira o {0}.")]
        [StringLength(300, ErrorMessage = "O campo {0} não pode exceder {1} carateres.")]
        public required string Motivo { get; set; }

        [Required(ErrorMessage = "Por favor, insira a {0}.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Data e hora de início")]
        public DateTime? DataInicio { get; set; }

        [Required(ErrorMessage = "Por favor, insira a {0}.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Data e hora de fim")]
        public DateTime? DataFim { get; set; }

        [Required(ErrorMessage = "Por favor, insira o {0}.")]
        [StringLength(50, ErrorMessage = "O campo {0} não pode exceder {1} carateres.")]
        public string Estado { get; set; } = "Pendente";

        public int SalaFk { get; set; }
    }
}
