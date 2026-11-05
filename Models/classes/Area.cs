using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class Area
    {
        [Key]
        public int IdArea { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        // Relacionamentos
        public ICollection<Oferta> Ofertas { get; set; } = new List<Oferta>();
    }
}