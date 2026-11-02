using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class Catalogo
    {
        [Key]
        public int CatalogoId { get; set; }

        public  required string ISBN { get; set; }

        public required string Titulo { get; set; }

        public int AutorId { get; set; }


        public Autor Autor { get; set; } = null!;

        public ICollection<Exemplar> Exemplares { get; set; }
            = new List<Exemplar>();

    }
}
