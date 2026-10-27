using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class Departamento
    {
        public int DepartamentoId { get; set; }

        [Required(ErrorMessage = "O nome do departamento é obrigatório.")]
        [StringLength(200, ErrorMessage = "O nome do departamento não pode exceder {1} carateres.")]
        public required string Nome { get; set; }

        public bool Ativo { get; set; } = true;

        [DataType(DataType.Date)]
        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}
