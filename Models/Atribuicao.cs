
using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class Atribuicao
    {
        [Key]
        public int IdAtribuicao { get; set; }

        public DateTime DataAtribuicao { get; set; }

        [Range(typeof(decimal), "0", "999999999")]
        public decimal ValorMensal { get; set; }

        [Range(1, int.MaxValue)]
        public int DuracaoMeses { get; set; }

        public int IdCandidatura { get; set; }
    }
}
