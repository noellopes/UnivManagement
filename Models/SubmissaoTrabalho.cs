using System.ComponentModel.DataAnnotations;
using UnivManagement.Models.AcademicManagement;

namespace UnivManagement.Models
{
    public class SubmissaoTrabalho
    {
        public int SubmissaoTrabalhoId { get; set; }

        [Required(ErrorMessage = "Por favor, escolha o {0}.")]
        [Display(Name = "Trabalho")]
        public int TrabalhoId { get; set; }

        public Trabalho? Trabalho { get; set; }

        [Required(ErrorMessage = "Por favor, escolha o {0}.")]
        [Display(Name = "Aluno responsável")]
        public int StudentId { get; set; }

        public Student? Student { get; set; }

        [Display(Name = "Grupo de trabalho")]
        public int? GrupoTrabalhoId { get; set; }

        public GrupoTrabalho? GrupoTrabalho { get; set; }

        [Required(ErrorMessage = "Por favor, indique a {0}.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Entrega")]
        public DateTime DataEntrega { get; set; }

        [Required(ErrorMessage = "Por favor, indique o {0}.")]
        [Display(Name = "Ficheiro entregue")]
        public required string CaminhoFicheiro { get; set; }
    }
}
