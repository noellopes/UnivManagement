using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class Uc
    {

        public int IdUc { get; set; }

        [Required(ErrorMessage = "O {0} da UC é obrigatório. | The UC {0} is required.")]
        [StringLength(10, ErrorMessage = "O {0} não pode ter mais de {1} caracteres. | The {0} cannot be longer than {1} characters.")]
        [Display(Name = "Código")]
        public required string Code { get; set; }

        [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
        [StringLength(100, ErrorMessage = "O {0} não pode ter mais de {1} caracteres. | The {0} cannot be longer than {1} characters.")]
        [Display(Name = "Nome da UC")]
        public required string Name { get; set; }

        [StringLength(10, ErrorMessage = "A {0} não pode ter mais de {1} caracteres. | The {0} cannot be longer than {1} characters.")]
        [Display(Name = "Sigla")]
        public string? Acronym { get; set; }

        [StringLength(255, ErrorMessage = "A {0} não pode ter mais de {1} caracteres. | The {0} cannot be longer than {1} characters.")]
        [Display(Name = "Descrição")]
        public string? Description { get; set; }
    }
}
