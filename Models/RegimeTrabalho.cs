using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class RegimeTrabalho
    {
        public int RegimeTrabalhoId { get; set; }

        [Required(ErrorMessage = "Por favor, insira o {0}.")]
        [StringLength(60, ErrorMessage = "O campo {0} não pode exceder {1} carateres.")]
        [Display(Name = " Nome do Regime de Trabalho")]
        public required string Nome { get; set; }
    }
}
