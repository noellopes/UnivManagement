using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class AntigoAluno
    {
        [Key]
        public int IdAluno { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string Curso { get; set; }

        public string SituacaoProfissional { get; set; }

        public Carreiras Carreira { get; set; }
        public ICollection<Candidaturas> Candidaturas { get; set; } = new List<Candidaturas>();
        public ICollection<Estagios> Estagios { get; set; } = new List<Estagios>();

        public void CriarConta() { }
        public void IniciarSessao() { }
        public void TerminarSessao() { }
        public void AtualizarPerfil() { }
        public void ConsultarOfertas() { }
        public void CandidatarOferta() { }
        public void ConsultarCandidaturas() { }
    }
}
