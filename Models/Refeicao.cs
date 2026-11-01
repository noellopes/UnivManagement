using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class Refeicao
    {
        // O EF Core reconhece 'Id' como PK por convenção (sem anotação [Key])
        public int Id { get; set; }

        [Required(ErrorMessage = "Por favor, especifique o nome da refeição.")]
        public required string Nome { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Preco { get; set; }

        // Chave Estrangeira para TipoRefeicao (substitui a antiga string Tipo)
        [Display(Name = "Tipo de Refeição")]
        public int TipoRefeicaoId { get; set; }

        // Propriedade de Navegação
        public TipoRefeicao? TipoRefeicao { get; set; }
    }
}