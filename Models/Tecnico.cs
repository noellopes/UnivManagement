using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class Tecnico
    {
        public int TecnicoId { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(200, ErrorMessage = "O nome não pode exceder {1} carateres.")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        [StringLength(150, ErrorMessage = "O email não pode exceder {1} carateres.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "O departamento é obrigatório.")]
        public int DepartamentoId { get; set; }

        [ForeignKey("DepartamentoId")]
        public Departamento? Departamento { get; set; }
    }
}