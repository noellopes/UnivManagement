using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    // àrea na qual se insere uma oferta
    public class Area
    {
        public int AreaId { get; set; }

        [Required(ErrorMessage = "Por favor insira o {0} da area.")]
        [StringLength(200, ErrorMessage = "O {0} da area não pode exceder {1} caracteres.")]
        public required string Nome { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Por favor insira a {0} da area.")]
        [StringLength(2000, ErrorMessage = "A {0} da area não pode exceder {1} caracteres.")]
        public required string Descricao { get; set; }
    }
}
