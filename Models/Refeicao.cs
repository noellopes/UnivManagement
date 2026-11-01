using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class Refeicao
    {
        // O EF Core reconhece 'Id' como PK automaticamente por convenção (sem [Key])
        public int Id { get; set; }

        [Required(ErrorMessage = "Por favor, especifique o nome da refeição.")]
        public required string Nome { get; set; }

        [Range(0.01, 100.00, ErrorMessage = "O preço deve ser superior a zero.")]
        public decimal Preco { get; set; }

        // Chave Estrangeira para TipoRefeicao
        [Display(Name = "Tipo de Refeição")]
        public int TipoRefeicaoId { get; set; }

        // Propriedade de Navegação
        public TipoRefeicao? TipoRefeicao { get; set; }
    }
}