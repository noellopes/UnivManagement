using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class FuncaoDocente
    {
        public int FuncaoDocenteId { get; set; }

        [Required(ErrorMessage = "Por favor, insira a {0}.")]
        [StringLength(60, ErrorMessage = "O campo {0} não pode exceder {1} carateres.")]
        [Display(Name = "Função de Docente")]
        public required string Nome { get; set; }

        public bool Ativo { get; set; } = true;

        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}
