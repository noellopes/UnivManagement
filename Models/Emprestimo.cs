using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class Emprestimo
    {
        public int EmprestimoId { get; set; }
        public DateTime dataEmprestimo { get; set; }
        public DateTime dataDevolucao { get; set; }
        public string estado { get; set; } = string.Empty;
        public int ExemplarId { get; set; }

        [ForeignKey(nameof(ExemplarId))]
        public Exemplar? Exemplar { get; set; }

    }
}
