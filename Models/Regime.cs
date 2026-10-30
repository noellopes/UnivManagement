using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class Regime
    {
        public long  RegimeId { get; set; }

        [Required(ErrorMessage = "Por favor, insira o {0}.")]
        [StringLength(50, ErrorMessage = "O campo {0} não pode exceder {1} carateres.")]
        public required string Nome { get; set; }

        public Datatime DataCriacao { get; set; } = DateTime.Now;

        public bool Ativo { get; set; } = true; 
    }
}
