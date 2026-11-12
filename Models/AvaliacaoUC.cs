using System;
using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class AvaliacaoUC
    {
        [Key]
        public int AvaliacaoID { get; set; }

        [Required]
        public int T_Avaliacao { get; set; }

        [Required]
        public int UcID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }
    }
}
