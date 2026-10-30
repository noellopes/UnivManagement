
using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.Scheduling_Group_2
{
    public class RoomManager
    {
        [Key]
        public int RoomManagerId { get; set; }

        [Required(ErrorMessage = "Por favor, indique o {0}.")]
        [StringLength(100, ErrorMessage = "O {0} não pode exceder {1} caracteres.")]
        [Display(Name = "Gabinete")]
        public string Office { get; set; } = string.Empty;
    }
}
