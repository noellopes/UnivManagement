using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.Scheduling_Group_2
{
    public class ReservationStatus
    {
        public int ReservationStatusId { get; set; }

        [Required(ErrorMessage = "Por favor, indique o {0}.")]
        [StringLength(50, ErrorMessage = "O {0} não pode exceder {1} caracteres.")]
        [Display(Name = "Estado da Reserva")]
        public string Name { get; set; } = string.Empty;
    }
}