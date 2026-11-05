using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class Oferta
    {
        [Key]
        public int IdOferta { get; set; }

        [ForeignKey("Empresa")]
        public int IdEmpresa { get; set; }
        public Empresa? Empresa { get; set; }

        [ForeignKey("Area")]
        public int IdArea { get; set; }
        public Area? Area { get; set; }

        [Required]
        public string Titulo { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Localizacao { get; set; } = string.Empty;
        public string Requisitos { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public double Remuneracao { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataLimite { get; set; }

        public string Estado { get; set; } = string.Empty;

        // Relacionamentos
        public ICollection<Candidatura> Candidaturas { get; set; } = new List<Candidatura>();
    }
}