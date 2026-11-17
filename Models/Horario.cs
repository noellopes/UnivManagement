using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UnivManagement.Models.AcademicManagement;
using UnivManagement.Models.AccommodationManagement;
namespace UnivManagement.Models
{
    public class Horario : IValidatableObject
    {
        public int HorarioId { get; set; }

        [Required(ErrorMessage = "Por favor, indique o {0}.")]
        [Display(Name = "Semestre")]
        [Range(1, 2, ErrorMessage = "O {0} deve ser {1} ou {2}.")]
        public int Semestre { get; set; }

        [Required(ErrorMessage = "Por favor, indique o {0}.")]
        [Display(Name = "Dia da Semana")]
        [Range(1, 7, ErrorMessage = "O {0} deve ser entre {1} (Segunda) e {2} (Domingo).")]
        public int DiaSemana { get; set; }

        [Required(ErrorMessage = "Por favor, indique a {0}.")]
        [Display(Name = "Hora de Início")]
        [DataType(DataType.Time)]
        public TimeOnly HoraInicio { get; set; }

        [Required(ErrorMessage = "Por favor, indique a {0}.")]
        [Display(Name = "Hora de Fim")]
        [DataType(DataType.Time)]
        public TimeOnly HoraFim { get; set; }

        [Required(ErrorMessage = "Por favor, indique o {0}.")]
        [Display(Name = "Tipo de Aula")]
        [StringLength(10, ErrorMessage = "O {0} não pode exceder {1} caracteres.")]
        public string TipoAula { get; set; } = "T"; // T, TP, P, PL, OT, S

        [Required(ErrorMessage = "Por favor, indique a {0}.")]
        [Display(Name = "Periodicidade")]
        [StringLength(30, ErrorMessage = "A {0} não pode exceder {1} caracteres.")]
        public string Periodicidade { get; set; } = "Semanal"; // Semanal, Quinzenal (Par), Quinzenal (Ímpar)

        [Required(ErrorMessage = "Por favor, indique o {0}.")]
        [Display(Name = "Estado")]
        [StringLength(30)]
        public string Estado { get; set; } = "Provisório"; // Provisório, Confirmado, Cancelado

        [Display(Name = "Observações")]
        [StringLength(500, ErrorMessage = "As {0} não podem exceder {1} caracteres.")]
        public string? Observacoes { get; set; }

        [Display(Name = "Data de Criação")]
        [DataType(DataType.Date)]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        // relacao com unidade curricular
        [Display(Name = "Unidade Curricular")]
        public int UcId { get; set; }

        [ForeignKey("UcId")]
        public UC? Uc { get; set; }

        // relacao com departamento
        [Display(Name = "Departamento")]
        public int DepartamentoId { get; set; }

        [ForeignKey("DepartamentoId")]
        public Departamento? Departamento { get; set; }

        // relacao com turma
        [Display(Name = "Turma")]
        public int TurmaId { get; set; }

        [ForeignKey("TurmaId")]
        //public Turma? Turma { get; set; } aqui e sem cometario mas a tabela do meu  colega ainda nao esta aqui

        // relacao com sala (opcional: aula online ou ainda sem sala)
        [Display(Name = "Sala")]
        public int? SalaId { get; set; }

        [ForeignKey("SalaId")]
        public Room? Sala { get; set; }

        // relacao com ano letivo
        [Display(Name = "Ano Letivo")]
        public int AcademicYearId { get; set; }

        [ForeignKey("AcademicYearId")]
        public AcademicYear? AnoLetivo { get; set; }

        // relacao com docentes (tabela HorarioDocente)
        public ICollection<HorarioDocente> Docentes { get; set; } = new List<HorarioDocente>();

        // validacao: hora de fim depois da hora de inicio
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (HoraFim <= HoraInicio)
            {
                yield return new ValidationResult(
                    "A Hora de Fim deve ser posterior à Hora de Início.",
                    new[] { nameof(HoraFim) });
            }
        }
    }
}
