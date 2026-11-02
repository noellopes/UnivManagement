
using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class Pagamento
    {
        [Key]
        public int IdPagamento { get; set; }

        public DateTime DataPagamento { get; set; }

        [Range(typeof(decimal), "0", "999999999")]
        public decimal Valor { get; set; }

        [Required]
        [StringLength(34)]
        public required string IbanDestino { get; set; }

        [Required]
        [StringLength(50)]
        public required string EstadoPagamento { get; set; }

        public int IdAtribuicao { get; set; }
    }
}
