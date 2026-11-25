using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class EstadoPossivel
    {
        public int EstadoPossivelId { get; set; }

        [Required(ErrorMessage = "O nome do estado é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome do estado deve ter entre 3 e 100 caracteres.")]
        public required string NomeEstado { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(500, ErrorMessage = "A descrição não pode exceder 500 caracteres.")]
        public required string Descricao { get; set; }
    }
}