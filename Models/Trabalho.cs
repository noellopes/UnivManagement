using System;
using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class Trabalho
    {
        [Key]
        public int TrabalhoID { get; set; }

        [Required]
        public int DocenteID { get; set; }

        [Required]
        public int UcId { get; set; }

        public bool E_individual { get; set; }

        [Required, StringLength(150)]
        public string Titulo { get; set; } = null!;

        public string? Descricao { get; set; }

        [DataType(DataType.Date)]
        public DateTime Data_Limite { get; set; }

        [StringLength(255)]
        public string? Caminho_ficheiro { get; set; }
    }
}
