using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class Exemplar
    {
        [Key]
        public int IdExemplar { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public string Estado { get; set; } = string.Empty;

        public int IdCatalogoFK { get; set; }

        [ForeignKey(nameof(IdCatalogoFK))]
        public Catalogo? Catalogo { get; set; }
    }
}
