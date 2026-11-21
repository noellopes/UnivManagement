using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class Empregador
    {
        [Key]
        public int IdEmpregador { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [ForeignKey("Empresa")]
        public int IdEmpresa { get; set; }
        public Empresa Empresa { get; set; }

        public ICollection<Estagio> Estagios { get; set; } = new List<Estagio>();

        public void CriarConta() { }
        public void IniciarSessao() { }
        public void TerminarSessao() { }
        public void AtualizarEmpresa() { }
        public void PublicarOferta() { }
        public void EditarOferta() { }
        public void RemoverOferta() { }
        public void ConsultarCandidaturas() { }
    }
}
