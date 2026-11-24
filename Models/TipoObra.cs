
namespace UnivManagement.Models
{
    public class TipoObra
    {
        public int TipoObraId { get; set; }

        public required string NomeTipo { get; set;  }

        public ICollection<Obra> Obras { get; set; } = new List<Obra>();
    }
}
