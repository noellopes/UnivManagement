using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class OrientadorEstagio
    {
        [Key]
        public int IdOrientador { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        // Relacionamentos
        public ICollection<Estagio> Estagios { get; set; } = new List<Estagio>();
    }
}