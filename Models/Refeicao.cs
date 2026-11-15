using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class Refeicao
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Por favor, insira o nome da refeição.")]
        [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "Por favor, especifique o tipo de refeição.")]
        public required string Tipo { get; set; }

        [Required(ErrorMessage = "Por favor, indique o preço.")]
        [Range(0.01, 50.00, ErrorMessage = "O preço deve ser um valor positivo entre 0.01€ e 50.00€.")]
        public decimal Preco { get; set; }
    }
}