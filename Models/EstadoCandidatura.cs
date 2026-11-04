using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class EstadoCandidatura
    {
        public int EstadoCandidaturaId { get; set; }

        [Required(ErrorMessage = "O nome do estado é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public required string nome_estado { get; set; }

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
        public required string descricao { get; set; }
    }
}
