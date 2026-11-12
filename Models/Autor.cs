
namespace UnivManagement.Models
{
    public class Autor
    {
        
        public int AutorId { get; set;  }

        public required string Nome { get; set; }

        public ICollection<LivroAutor> LivrosAutores { get; set; }
        = new List<LivroAutor>();
    }
}
