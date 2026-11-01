using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class TipoRefeicao
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Nome { get; set; }
    }
}