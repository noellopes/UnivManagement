using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UnivManagement.Models.AcademicManagement;

namespace UnivManagement.Models
{
    public class Nota
    {
        public int NotaId { get; set; }

        [Required(ErrorMessage = "Por favor, escolha a {0}.")]
        [Display(Name = "Avaliação")]
        public int AvaliacaoId { get; set; }

        [ForeignKey("AvaliacaoId")]
        public AvaliacaoUC? AvaliacaoUC { get; set; }

        [Required(ErrorMessage = "Por favor, escolha a {0}.")]
        [Display(Name = "Inscrição na UC")]
        public int InscrUcId { get; set; }

        [ForeignKey("InscrUcId")]
        public InscrUC? InscrUC { get; set; }

        [Required(ErrorMessage = "Por favor, insira a {0}.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data")]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "Por favor, insira a {0}.")]
        [Range(0, 20, ErrorMessage = "A {0} tem de estar entre {1} e {2}.")]
        [Display(Name = "Nota")]
        public float Valor { get; set; }

        [Display(Name = "Aprovado")]
        public bool Aprovado { get; set; }
    }
}