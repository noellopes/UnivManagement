using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class Estudante
    {
        [Key]
        public int IdAluno { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Curso { get; set; } = string.Empty;

        public string SituacaoProfissional { get; set; } = string.Empty;

        // Relacionamentos / Navegação
        public ICollection<Carreira> Carreiras { get; set; } = new List<Carreira>();
        public ICollection<Candidatura> Candidaturas { get; set; } = new List<Candidatura>();
        public ICollection<Estagio> Estagios { get; set; } = new List<Estagio>();
    }
}