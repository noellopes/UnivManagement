
using System;
using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.Scheduling_Group_2
{
    public class ScheduleConflict
    {
        [Key]
        public int ScheduleConflictId { get; set; }

        [Required(ErrorMessage = "Por favor, indique a sala.")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione uma sala válida.")]
        [Display(Name = "Sala")]
        public int RoomId { get; set; }

        [Display(Name = "Data de Deteção")]
        [DataType(DataType.DateTime)]
        public DateTime DetectedAt { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Por favor, indique a {0}.")]
        [StringLength(500, ErrorMessage = "A {0} não pode exceder {1} caracteres.")]
        [Display(Name = "Descrição do Conflito")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Resolvido")]
        public bool IsResolved { get; set; } = false;
    }
}
