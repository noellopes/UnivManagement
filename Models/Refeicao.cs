using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class Refeicao
    {

        // 1. Identificador único da refeição
        public int Id { get; set; }

        // 2. Nome da refeição com validação (obrigatório e tamanho máximo)
        [Required(ErrorMessage = "Por favor, insira o nome da refeição.")]
        [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public required string Nome { get; set; }

        // 3. Tipo de refeição (ex: Almoço, Jantar, Sopa, Sobremesa)
        [Required(ErrorMessage = "Por favor, especifique o tipo de refeição.")]
        public required string Tipo { get; set; }

        // 4. Preço com validação de valor positivo (visto nos slides de validações/SportsStore)
        [Required(ErrorMessage = "Por favor, indique o preço.")]
        [Range(0.01, 50.00, ErrorMessage = "O preço deve ser um valor positivo entre 0.01€ e 50.00€.")]
        public decimal Preco
        {
            get; set; 
        }
    }
}

