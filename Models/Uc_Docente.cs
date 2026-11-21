using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models.AcademicManagement;

namespace UnivManagement.Models
{
    [Table("UC_Docente")]
    [PrimaryKey(nameof(CursoId), nameof(UcId), nameof(DocenteId), nameof(AcademicYearId))]
    public class UC_Docente
    {
        [Required(ErrorMessage = "Por favor, selecione o {0}.")]
        [Display(Name = "Curso")]
        public int CursoId { get; set; }

        [ForeignKey("CursoId")]
        public Course? Curso { get; set; }

        [Required(ErrorMessage = "Por favor, selecione a {0}.")]
        [Display(Name = "Unidade Curricular")]
        public int UcId { get; set; }

        [ForeignKey("UcId")]
        public UC? Uc { get; set; }

        [Required(ErrorMessage = "Por favor, selecione o {0}.")]
        [Display(Name = "Docente")]
        public int DocenteId { get; set; }

        [ForeignKey("DocenteId")]
        public Docente? Docente { get; set; }

        [Required(ErrorMessage = "Por favor, selecione o {0}.")]
        [Display(Name = "Ano Letivo")]
        public int AcademicYearId { get; set; }

        [ForeignKey("AcademicYearId")]
        public AcademicYear? AcademicYear { get; set; }
    }
}