namespace UnivManagement.Models
{
    public class Catalogo
    {
        public int IdCatalogo { get; set; }

        public string ISBN { get; set; }

        public string Titulo { get; set; }

        public string Autor { get; set; }

        public ICollection<Exemplar> Exemplares { get; set; }
    }
}
