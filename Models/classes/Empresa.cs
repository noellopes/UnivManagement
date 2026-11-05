using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class Empresa
    {
        [Key]
        public int IdEmpresa { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        public string Setor { get; set; } = string.Empty;
        public string Localizacao { get; set; } = string.Empty;
        public string Contacto { get; set; } = string.Empty;

        // Relacionamentos
        public ICollection<Oferta> Ofertas { get; set; } = new List<Oferta>();
        public ICollection<Estagio> Estagios { get; set; } = new List<Estagio>();
    }
}