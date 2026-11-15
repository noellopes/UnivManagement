using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class TipoBolsa
    {
        public int TipoBolsaId { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "A descrição deve ter entre 10 e 200 caracteres.")]
        public required string Descricao { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "O rendimento máximo não pode ser negativo.")]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? RendimentoMaximo { get; set; }

        [Range(0, 360, ErrorMessage = "Os ECTS mínimos devem estar entre 0 e 360.")]
        public int? EctsMinimos { get; set; }

        [Range(0, 20, ErrorMessage = "A média mínima deve estar entre 0 e 20 valores.")]
        [Column(TypeName = "decimal(4, 2)")]
        public decimal? MediaMinima { get; set; }

        [StringLength(500, ErrorMessage = "Os critérios de elegibilidade não podem exceder 500 caracteres.")]
        public string? CriteriosElegibilidade { get; set; }
    }
}