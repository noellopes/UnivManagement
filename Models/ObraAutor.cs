namespace UnivManagement.Models
{
    public class ObraAutor
    {
        public int ObraAutorId { get; set; }

        public int ObraId { get; set; }
        public Obra? Obra { get; set; }

        public int AutorId { get; set; }
        public Autor? Autor { get; set; }


    }
}
