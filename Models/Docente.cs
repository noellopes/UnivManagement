using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class Docente
    {

        public int DocenteId { get; set; }

        [Required(ErrorMessage = "Por favor, insira o {0}.")]
        [Range(1, 999, ErrorMessage = "O {0} deve estar entre {1} e {2}.")]
        public int NumDocente { get; set; }

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

        public bool Ativo { get; set; } = true;

        [DataType(DataType.Date)]
        public DateTime DataAdmissao { get; set; }

        [Required(ErrorMessage = "Por favor, insira o {0}.")]
        [StringLength(100, ErrorMessage = "O campo {0} não pode exceder {1} carateres.")]
        public required string Morada { get; set; }

        // Função Docente (FK) (Opcional)
        public int? FuncaoDocenteId { get; set; }
        public DocenteFuncao? FuncaoDocente { get; set; } // Navigation property

        // Regime (FK) (Opcional)
        public int? RegimeId { get; set; }
        public Regime? Regime { get; set; } // Navigation property

        // Departamento (FK) (Opcional)
        public int? DepartamentoId { get; set; }
        public Departamento? Departamento { get; set; } // Navigation property

    }
}
