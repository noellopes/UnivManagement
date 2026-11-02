using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AcademicManagement
{
    public class UC
    {

        public int UcId { get; set; }

        [Required(ErrorMessage = "O {0} da UC é obrigatório.")]
        [RegularExpression(@"^[A-Z0-9]{3,10}$", ErrorMessage = "O {0} deve conter entre 3 e 10 caracteres (apenas letras maiúsculas e números).")]
        [Display(Name = "Código")]
        public required string Code { get; set; }

        [Required(ErrorMessage = "O {0} é obrigatório.")]
        [StringLength(100, ErrorMessage = "O {0} não pode ter mais de {1} caracteres.")]
        [Display(Name = "Nome da UC")]
        public required string Name { get; set; }

        [RegularExpression(@"^[A-Z0-9]{1,6}$", ErrorMessage = "A {0} deve ter até 6 caracteres e conter apenas letras maiúsculas e números.")]
        [Display(Name = "Sigla")]
        public string? Acronym { get; set; }

        [StringLength(255, ErrorMessage = "A {0} não pode ter mais de {1} caracteres.")]
        [Display(Name = "Descrição")]
        public string? Description { get; set; }
    }
}
