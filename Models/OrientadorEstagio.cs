using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class OrientadorEstagios
    {
        [Key]
        public int IdOrientador { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public ICollection<Estagios> Estagios { get; set; } = new List<Estagios>();
        public ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();

        public void IniciarSessao() { }
        public void ConsultarEstagios() { }
        public void AcompanharEstagio() { }
        public void RegistrarObservacoes() { }
        public void AvaliarEstagio() { }
        public void ConsultarRelatorios() { }
    }
}
