using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class GestorSalas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Utilizador")]
        public int UtilizadorId { get; set; }

        [Required(ErrorMessage = "Por favor, indique o {0}.")]
        [Display(Name = "Gabinete")]
        [StringLength(20, ErrorMessage = "O {0} não pode exceder {1} caracteres.")]
        public required string Gabinete { get; set; }
    }
}