using System.ComponentModel.DataAnnotations;
namespace UnivManagement.Models
{
    public class Building
    {
        public int BuildingId { get; set; }

        [Required(ErrorMessage = "Por favor, indique o {0}.")]
        [Display(Name = "Nome do Edifício")]
        [StringLength(100, ErrorMessage = "O {0} não pode exceder {1} caracteres.")]
        public required string NomeEdificio { get; set; }

        [Required(ErrorMessage = "Por favor, indique a {0}.")]
        [Display(Name = "Localização")]
        [StringLength(150, ErrorMessage = "A {0} não pode exceder {1} caracteres.")]
        public required string Localizacao { get; set; }
    }
}
