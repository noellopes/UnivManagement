using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class OrientadorEstagio
    {
        [Key]
        public int OrientadorId { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Introduza um email válido.")]
        public string Email { get; set; }

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