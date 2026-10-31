using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class Catalogo
    {
        [Key]
        public required int IdCatalogo { get; set; }

        public required string ISBN { get; set; }

        public required string Titulo { get; set; }

        public required string Autor { get; set; }

        public ICollection<Exemplar> Exemplares { get; set; }

    }
}
