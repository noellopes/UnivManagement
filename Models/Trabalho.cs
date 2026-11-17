using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UnivManagement.Models.AcademicManagement;

namespace UnivManagement.Models
{
    public class Trabalho
    {
        [Column("TrabalhoID")]
        public int TrabalhoId { get; set; }

        public int DocenteId { get; set; }
        public Docente? Docente { get; set; }

        public int UcId { get; set; }
        public UC? UC { get; set; }

        [Column("E_individual")]
        public bool EIndividual { get; set; }

        [Required(ErrorMessage = "Indica o título do trabalho.")]
        [Display(Name = "Título")]
        public required string Titulo { get; set; }

        [Required(ErrorMessage = "Indica a descrição do trabalho.")]
        [Display(Name = "Descrição")]
        public required string Descricao { get; set; }

        [Column("Data_Limite")]
        [DataType(DataType.Date)]
        [Display(Name = "Data-limite de entrega")]
        public DateTime DataLimite { get; set; }
    }
}