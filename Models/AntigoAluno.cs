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

        public Carreira Carreira { get; set; }
        public ICollection<Candidatura> Candidaturas { get; set; } = new List<Candidatura>();
        public ICollection<Estagio> Estagios { get; set; } = new List<Estagio>();

        public void CriarConta() { }
        public void IniciarSessao() { }
        public void TerminarSessao() { }
        public void AtualizarPerfil() { }
        public void ConsultarOfertas() { }
        public void CandidatarOferta() { }
        public void ConsultarCandidaturas() { }
    }
}
