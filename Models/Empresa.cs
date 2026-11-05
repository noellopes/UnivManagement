using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class Empresa
    {
        [Key]
        public int IdEmpresa { get; set; }

        [Required]
        public string Nome { get; set; }

        public string Setor { get; set; }
        public string Localizacao { get; set; }
        public string Contacto { get; set; }

        public ICollection<Empregador> Empregadores { get; set; } = new List<Empregador>();
        public ICollection<Oferta> Ofertas { get; set; } = new List<Oferta>();

        public void AtualizarDados() { }
        public void ConsultarDados() { }
    }
}
