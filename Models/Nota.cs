using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UnivManagement.Models.AcademicManagement;

namespace UnivManagement.Models
{
    public class Notas
    {
        [Key]
        public int NotaID { get; set; }

        [Required(ErrorMessage = "A avaliação é obrigatória.")]
        [Display(Name = "Avaliação")]
        public int AvaliacaoID { get; set; }

        [ForeignKey(nameof(AvaliacaoID))]
        public AvaliacaoUC? AvaliacaoUC { get; set; }

        [Required(ErrorMessage = "A inscrição na UC é obrigatória.")]
        [Display(Name = "Inscrição na UC")]
        public int InscrUcID { get; set; }

        [ForeignKey(nameof(InscrUcID))]
        public InscrUC? InscrUC { get; set; }

        [Required(ErrorMessage = "A data é obrigatória.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data")]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "A nota é obrigatória.")]
        [Range(0, 20, ErrorMessage = "A nota tem de estar entre 0 e 20.")]
        [Display(Name = "Nota")]
        public float Nota { get; set; }

        [Display(Name = "Aprovado")]
        public bool Aprovado { get; set; }
    }
}