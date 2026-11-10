using System;
using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class AvContinua
    {
        [Key]
        public int Id_Cont { get; set; }

        [Required(ErrorMessage = "A data de registo é obrigatória.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Registo")]
        public DateTime Data_Registo { get; set; }
    }
}
