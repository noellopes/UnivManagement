namespace UnivManagement.Models
{
    public class LivroAutor
    {
        public int LivroAutorId { get; set; }

        public int LivroId { get; set; }
        public Livro? Livro { get; set; }

        public int AutorId { get; set; }
        public Autor? Autor { get; set; }


    }
}
