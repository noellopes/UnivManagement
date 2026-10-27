using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class Funcionario
    {
        public int FuncionarioId { get; set; }

        [Required(ErrorMessage = "Por favor, insira o {0}.")]
        [StringLength(200, ErrorMessage = "O campo {0} não pode exceder {1} carateres.")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "Por favor, insira o {0}.")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "O {0} deve ter exatamente {1} dígitos.")]
        public required string Nif { get; set; }

        [Required(ErrorMessage = "Por favor, insira o {0}.")]
        [EmailAddress(ErrorMessage = "Insira um endereço de {0} válido.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Por favor, insira o {0}.")]
        [Phone(ErrorMessage = "Insira um {0} válido.")]
        public required string Telefone { get; set; }

        [Required(ErrorMessage = "Por favor, insira o {0}.")]
        public required string Regime { get; set; }

        public bool Ativo { get; set; } = true;

        [DataType(DataType.Date)]
        public DateTime DataAdmissao { get; set; }

        [Required(ErrorMessage = "Por favor, insira a {0}.")]
        public required string? Morada { get; set; }

  
        public int ServicoFk { get; set; }
       
    }
}
