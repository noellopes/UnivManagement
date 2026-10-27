using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class Regime
    {
        public int RegimeId { get; set; }

        [Required(ErrorMessage = "Por favor, insira o {0}.")]
        [StringLength(50, ErrorMessage = "O campo {0} não pode exceder {1} carateres.")]
        public required string Nome { get; set; }
    }
}
