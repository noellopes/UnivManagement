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
        public ICollection<Carreiras> Carreiras { get; set; } = new List<Carreiras>();
        public ICollection<Candidaturas> Candidaturas { get; set; } = new List<Candidaturas>();
        public ICollection<Estagios> Estagios { get; set; } = new List<Estagios>();
    }
}