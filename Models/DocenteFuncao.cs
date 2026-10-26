using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class DocenteFuncao
    {
        public int FuncaoDocenteId { get; set; }

        [Required(ErrorMessage = "Por favor, insira o {0}.")]
        [StringLength(60, ErrorMessage = "O campo {0} não pode exceder {1} carateres.")]
        public required string Nome { get; set; }
    }
}
 