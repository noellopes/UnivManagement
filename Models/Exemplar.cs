using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class Exemplar
    {
        [Key]
        public int ExemplarId { get; set; }

        [Required(ErrorMessage = "O estado é obrigatório.")]
        [StringLength(50)]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Disponível";
        public string Codigo { get; set; } = string.Empty;

        public int IdCatalogoFK { get; set; }

        [ForeignKey(nameof(IdCatalogoFK))]
        public Catalogo? Catalogo { get; set; }
    }
}