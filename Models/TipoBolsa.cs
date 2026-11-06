using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class TipoBolsa
    {
        public int TipoBolsaId { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "o nome deve conter no min 3 letras e 100 no max.")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "Descrição é obrigatória.")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "A descrição deve conter no min 10 caracteres e 200 no max.")]
        public required string Descricao { get; set; }

        public ICollection<CriterioElegibilidade> CriteriosElegibilidade { get; set; } = new List<CriterioElegibilidade>();
    }
}
