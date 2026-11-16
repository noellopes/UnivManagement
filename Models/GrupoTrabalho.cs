using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class GrupoTrabalho
    {
        public int GrupoTrabalhoId { get; set; }

        [Required(ErrorMessage = "Indica o nome do grupo.")]
        [StringLength(100)]
        [Display(Name = "Nome do grupo")]
        public string Nome { get; set; } = string.Empty;

        public int TrabalhoId { get; set; }
        public Trabalho? Trabalho { get; set; }
    }
}