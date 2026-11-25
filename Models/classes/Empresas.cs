using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class Empresas
    {
        [Key]
        public int IdEmpresa { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        public string Setor { get; set; } = string.Empty;
        public string Localizacao { get; set; } = string.Empty;
        public string Contacto { get; set; } = string.Empty;

        // Relacionamentos
        public ICollection<Ofertas> Ofertas { get; set; } = new List<Ofertas>();
        public ICollection<Estagios> Estagios { get; set; } = new List<Estagios>();
    }
}