
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.Scheduling_Group_2
{
    public class RoomReservation : IValidatableObject
    {
        public int RoomReservationId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Selecione uma sala válida.")]
        [Display(Name = "Sala")]
        public int RoomId { get; set; }

        public Room Room { get; set; } = null!;

        [Display(Name = "Data e Hora de Início")]
        [DataType(DataType.DateTime)]
        public DateTime StartTime { get; set; }

        [Display(Name = "Data e Hora de Fim")]
        [DataType(DataType.DateTime)]
        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "Por favor, indique o {0}.")]
        [StringLength(250, ErrorMessage = "O {0} não pode exceder {1} caracteres.")]
        [Display(Name = "Motivo")]
        public string Purpose { get; set; } = string.Empty;

        [Display(Name = "Estado da Reserva")]
        public int ReservationStatusId { get; set; } = 1;

        public ReservationStatus ReservationStatus { get; set; } = null!;

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (StartTime == default || EndTime == default)
            {
                yield return new ValidationResult(
                    "Indique datas e horas válidas.",
                    new[] { nameof(StartTime), nameof(EndTime) });
            }
            else if (EndTime <= StartTime)
            {
                yield return new ValidationResult(
                    "A data e hora de fim devem ser posteriores ao início.",
                    new[] { nameof(EndTime) });
            }
        }
    }
}
