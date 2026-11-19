using System.ComponentModel.DataAnnotations;
using UnivManagement.Models.AcademicManagement;

namespace UnivManagement.Models
{
    public class CriterioAvaliacao
    {
        public int CriterioAvaliacaoId { get; set; }

        [Required(ErrorMessage = "Por favor, escolha a {0}.")]
        [Display(Name = "Unidade Curricular")]
        public int UcId { get; set; }

        public UC? UC { get; set; }

        [Required(ErrorMessage = "Por favor, indique o {0}.")]
        [Range(0, 100, ErrorMessage = "O {0} deve estar entre {1}% e {2}%.")]
        [Display(Name = "Peso dos Trabalhos (%)")]
        public float PesoTrab { get; set; }
    }
}