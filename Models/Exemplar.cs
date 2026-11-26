using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class Exemplar
    {
        [Key]
        public int ExemplarId { get; set; }

        [Required(ErrorMessage = "O código do exemplar é obrigatório.")]
        [StringLength(30, ErrorMessage = "O código não pode ter mais de 30 caracteres.")]
        [Display(Name = "Código")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O estado é obrigatório.")]
        [StringLength(20, ErrorMessage = "O estado não pode ter mais de 20 caracteres.")]
        [RegularExpression("^(Disponível|Emprestado)$", ErrorMessage = "O estado tem de ser 'Disponível' ou 'Emprestado'.")]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Disponível";

        [Required(ErrorMessage = "A obra é obrigatória.")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione uma obra válida.")]
        [Display(Name = "Obra")]
        public int ObraId { get; set; }

        [ForeignKey(nameof(ObraId))]
        public Obra? Obra { get; set; }

        public ICollection<Emprestimo> Emprestimos { get; set; } = new List<Emprestimo>();
    }
}