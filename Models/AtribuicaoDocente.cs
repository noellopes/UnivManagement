using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models.AcademicManagement;

namespace UnivManagement.Models
{
    // Quem leciona cada unidade curricular em cada ano letivo.
    // E a fonte que o gerador de horarios le: ninguem introduz docentes nos blocos.
    // Indice unico: o mesmo docente nao pode ser atribuido duas vezes a mesma UC no mesmo ano.
    [Index(nameof(DocenteId), nameof(UcId), nameof(AcademicYearId), IsUnique = true)]
    public class AtribuicaoDocente
    {
        [Key]
        public int AtribuicaoDocenteId { get; set; }

        // relacao com docente
        [Required(ErrorMessage = "Por favor, indique o {0}.")]
        [Display(Name = "Docente")]
        public int DocenteId { get; set; }

        [ForeignKey("DocenteId")]
        public Docente? Docente { get; set; }

        // relacao com unidade curricular
        // CONFIRMAR: nome da classe (UC) e da chave (UcId) no codigo,
        // e o namespace (se nao for AcademicManagement, ajustar o using)
        [Required(ErrorMessage = "Por favor, indique a {0}.")]
        [Display(Name = "Unidade Curricular")]
        public int UcId { get; set; }

        [ForeignKey("UcId")]
        public UC? UnidadeCurricular { get; set; }

        // relacao com ano letivo
        [Required(ErrorMessage = "Por favor, indique o {0}.")]
        [Display(Name = "Ano Letivo")]
        public int AcademicYearId { get; set; }

        [ForeignKey("AcademicYearId")]
        public AcademicYear? AnoLetivo { get; set; }
    }
}
