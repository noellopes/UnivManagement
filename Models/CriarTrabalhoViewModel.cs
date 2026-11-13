using System;
using System.ComponentModel.DataAnnotations;
namespace UnivManagement.Models
{
    public class CriarTrabalhoViewModel
    {
        [Required(ErrorMessage = "Escreve o título do trabalho.")]
        [StringLength(150)]
        [Display(Name = "Título")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Escreve o enunciado.")]
        public string Enunciado { get; set; } = string.Empty;

        [Required(ErrorMessage = "Indica o prazo de entrega.")]
        [Display(Name = "Prazo de entrega")]
        public DateTime? DataLimite { get; set; }

        [Display(Name = "Tipo de trabalho")]
        public bool EmGrupo { get; set; }
    }
}
