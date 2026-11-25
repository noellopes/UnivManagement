using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class Candidato
    {

        public int CandidatoId { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "o nome deve conter no min 3 letras e 100 no max.")]
        public string Nome { get; set; }


        [Required(ErrorMessage = "Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "O e-mail deve conter entre 5 e 100 caracteres.")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Telemovel é obrigatório.")]
        [Phone(ErrorMessage = "Número de telemóvel inválido.")]
        [StringLength(15, MinimumLength = 9, ErrorMessage = "O telemóvel deve ter entre 9 e 15 dígitos.")]
        public string Telemovel { get; set; }


        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        public DateOnly DataNascimento { get; set; }
        

        [Required(ErrorMessage = "O NIF é obrigatório.")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "O NIF deve conter exatamente 9 dígitos numéricos.")]
        public int NIF { get; set; }


        [Required(ErrorMessage = "A instituição de ensino é obrigatória.")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "A instituição deve ter entre 2 e 150 caracteres.")]
        public string instituicaoEnsino  { get; set; }


        [Required(ErrorMessage = "O curso é obrigatório.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O curso deve ter entre 2 e 100 caracteres.")]
        public string Curso { get; set; }


        [Required(ErrorMessage = "O rendimento agregado é obrigatório.")]
        [Range(0.00, 999999999.99, ErrorMessage = "O rendimento deve ser um valor positivo.")]
        public decimal RendimentoAgregado { get; set; }
    }
}
