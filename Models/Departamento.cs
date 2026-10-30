using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class Departamento
    {
        public long  DepartamentoId { get; set; }

        [Required(ErrorMessage = "O nome do departamento é obrigatório.")]
        [StringLength(200, ErrorMessage = "O nome do departamento não pode exceder {1} carateres.")]
        public required string Nome { get; set; } = string.Empty;

        public bool Ativo { get; set; } = true;

        [DataType(DataType.Date)]
        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}
