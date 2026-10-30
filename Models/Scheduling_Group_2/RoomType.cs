using System.ComponentModel.DataAnnotations;
namespace UnivManagement.Models.Scheduling_Group_2
{
    public class RoomType
    {
        [Key]
        public int IdTipoSala { get; set; }

        [Required(ErrorMessage = "Por favor, indique a {0}.")]
        [Display(Name = "Designação")]
        [StringLength(50, ErrorMessage = "A {0} não pode exceder {1} caracteres.")]
        public required string Designacao { get; set; }

        [Display(Name = "Requer Computadores")]
        public bool RequerComputadores { get; set; } = false;
    }
}
