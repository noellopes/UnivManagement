using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class Autor
    {
        [Key]
        public int AutorId { get; set; }

        public required string Nome { get; set; }

        public ICollection<Catalogo> Catalogos { get; set; }
         = new List<Catalogo>();
    }
}