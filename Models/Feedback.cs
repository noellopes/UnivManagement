using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackID { get; set; }

        [Required(ErrorMessage = "A submissão é obrigatória.")]
        [Display(Name = "Submissão")]
        public int SubmissaoID { get; set; }

        //[ForeignKey("SubmissaoID")]
        //public virtual SubmissaoTrabalho SubmissaoTrabalho { get; set; }

        [Required(ErrorMessage = "O docente é obrigatório.")]
        [Display(Name = "Docente")]
        public int DocenteID { get; set; }

        [ForeignKey("DocenteID")]
        public virtual Docente Docente { get; set; }

        [Required(ErrorMessage = "A data de feedback é obrigatória.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Feedback")]
        public DateTime Data_Feedback { get; set; }
    }
}