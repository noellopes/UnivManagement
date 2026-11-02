using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class ApplicationUser
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Por favor, indique o {0}.")]
        [StringLength(100, ErrorMessage = "O {0} não pode exceder {1} caracteres.")]
        [Display(Name = "Nome Completo")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "Por favor, indique o {0}.")]
        [EmailAddress(ErrorMessage = "Por favor, insira um endereço de e-mail válido.")]
        [StringLength(150, ErrorMessage = "O {0} não pode exceder {1} caracteres.")]
        [Display(Name = "E-mail")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Por favor, indique a {0}.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A {0} deve ter entre {2} e {1} caracteres.")]
        [Display(Name = "Palavra-passe")]
        public required string PalavraPasse { get; set; }
    }
}