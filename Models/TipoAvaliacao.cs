
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class TipoAvaliacao
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int T_Avaliacao { get; set; }

        [Required]
        [StringLength(50)]
        public string Tipo_Prova { get; set; } = string.Empty;
    }
}
