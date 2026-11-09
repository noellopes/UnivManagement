using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class OrientadorEstagio
    {
        [Key]
        public int IdOrientador { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public ICollection<Estagio> Estagios { get; set; } = new List<Estagio>();
        public ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();

        public void IniciarSessao() { }
        public void ConsultarEstagios() { }
        public void AcompanharEstagio() { }
        public void RegistrarObservacoes() { }
        public void AvaliarEstagio() { }
        public void ConsultarRelatorios() { }
    }
}
