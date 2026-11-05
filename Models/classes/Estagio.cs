using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class Estagio
    {
        [Key]
        public int IdEstagio { get; set; }

        [ForeignKey("Estudante")]
        public int IdAluno { get; set; }
        public Estudante? Estudante { get; set; }

        [ForeignKey("OrientadorEstagio")]
        public int IdOrientador { get; set; }
        public OrientadorEstagio? OrientadorEstagio { get; set; }

        [ForeignKey("GestorEstagio")]
        public int IdGestor { get; set; }
        public GestorEstagio? GestorEstagio { get; set; }

        [ForeignKey("Empresa")]
        public int IdEmpresa { get; set; }
        public Empresa? Empresa { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataInicio { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataFim { get; set; }

        public int Duracao { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string PlanoTrabalho { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime DataAvaliacao { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataEntrega { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public double Classificacao { get; set; }

        public string Observacoes { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Ficheiro { get; set; } = string.Empty;
    }
}