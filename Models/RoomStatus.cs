using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class RoomStatus
    {
        public int RoomStatusId { get; set; }

        [Display(Name = "Estado Operacional")]
        [StringLength(30, ErrorMessage = "O {0} não pode exceder {1} caracteres.")]
        public required string Designacao { get; set; }
    }
}
