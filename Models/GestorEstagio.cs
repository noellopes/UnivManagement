using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class GestorEstagio
    {
        [Key]
        public int IdGestor { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public ICollection<Estagio> Estagios { get; set; } = new List<Estagio>();

        public void IniciarSessao() { }
        public void GerirOfertas() { }
        public void ConsultarCandidaturas() { }
        public void AtribuirOrientador() { }
        public void AtualizarEstagio() { }
        public void AcompanharEstagios() { }
        public void ConsultarRelatorios() { }
    }
}
