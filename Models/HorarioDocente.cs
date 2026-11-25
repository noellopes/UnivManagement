using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace UnivManagement.Models
{
    [PrimaryKey(nameof(HorarioId), nameof(DocenteId))]
    public class HorarioDocente
    {
        public int HorarioId { get; set; }

        [ForeignKey("HorarioId")]
        public Horario? Horario { get; set; }

        public int DocenteId { get; set; }

        [ForeignKey("DocenteId")]
        public Docente? Docente { get; set; }
    }
}
