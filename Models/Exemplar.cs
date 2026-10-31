namespace UnivManagement.Models
{
    public class Exemplar
    {
        public int IdExemplar { get; set; }

        public String Codigo { get; set; }

        public String Estado { get; set; }

        public int IdCatalogoFK { get; set; }

        public Catalogo Catalogo { get; set; }
    }
}
