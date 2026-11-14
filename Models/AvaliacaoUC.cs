
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class AvaliacaoUC
    {
        [Key]
        public int AvaliacaoID { get; set; }

        [Required]
        [ForeignKey(nameof(TipoAvaliacao))]
        public int T_Avaliacao { get; set; }

        public TipoAvaliacao TipoAvaliacao { get; set; } = null!;

        [Required]
        public int UcID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }
    }
}

