using System;
using System.ComponentModel.DataAnnotations;
using UnivManagement.Models.AcademicManagement;

namespace UnivManagement.Models
{
    public class Trabalho
    {
        [Key]
        public int TrabalhoID { get; set; }

        public int DocenteId { get; set; }
        public Docente? Docente { get; set; }

        public int UcId { get; set; }
        public UC? UC { get; set; }

        public bool E_individual { get; set; }

        [Required(ErrorMessage = "Indica o título do trabalho.")]
        [Display(Name = "Título")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Indica a descrição do trabalho.")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Data-limite de entrega")]
        public DateTime Data_Limite { get; set; }
    }
}