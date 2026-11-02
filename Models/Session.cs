using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class Session
    {
        [Key]
        public int ClassSessionId { get; set; }

        [Required(ErrorMessage = "Por favor, indique o {0}.")]
        [Range(1, 7, ErrorMessage = "O {0} deve estar entre {1} (Domingo) e {2} (Sábado).")]
        [Display(Name = "Dia da Semana")]
        public int DiaSemana { get; set; }

        [Required(ErrorMessage = "Por favor, indique a {0}.")]
        [DataType(DataType.Time)]
        [Display(Name = "Hora de Início")]
        public TimeSpan HoraInicio { get; set; }

        [Required(ErrorMessage = "Por favor, indique a {0}.")]
        [DataType(DataType.Time)]
        [Display(Name = "Hora de Fim")]
        public TimeSpan HoraFim { get; set; }

        [Required(ErrorMessage = "Por favor, indique o {0}.")]
        [StringLength(50, ErrorMessage = "O {0} não pode exceder {1} caracteres.")]
        [Display(Name = "Tipo de Aula")]
        public required string TipoAula { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        [Required(ErrorMessage = "Por favor, selecione a {0}.")]
        [Display(Name = "Sala")]
        public int IdSala { get; set; }

        [ForeignKey("IdSala")]
        //public Room? Room { get; set; }

        [Required(ErrorMessage = "Por favor, selecione a {0}.")]
        [Display(Name = "Turma")]
        public int IdTurma { get; set; }

        [ForeignKey("IdTurma")]
        public Group? ClassGroup { get; set; }
    }
}