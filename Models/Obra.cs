
namespace UnivManagement.Models
{
    public class Obra
    {
        
        public int ObraId { get; set; }

        public  required string ISBN { get; set; }

        public required string Titulo { get; set; }

        public required string Tipo { get; set; }
        
        public required double ValorMulta { get; set; }

        public ICollection<Exemplar> Exemplares { get; set; }
            = new List<Exemplar>();

        public ICollection<ObraAutor> ObrasAutores { get; set; }
        = new List<ObraAutor>();

    }
}
