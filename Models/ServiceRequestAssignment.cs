using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    // Who is in charge of a request: an employee and/or a department (Grupo 6), from AssignedAt until EndedAt
    public class ServiceRequestAssignment
    {
        public int ServiceRequestAssignmentId { get; set; }

        [Display(Name = "Request")]
        [Required(ErrorMessage = "The {0} is required.")]
        public int ServiceRequestId { get; set; }

        [Display(Name = "Request")]
        public ServiceRequest? ServiceRequest { get; set; }

        // Funcionario and Departamento are not in this DbContext yet, so for now only their number is kept
        [Display(Name = "Employee number")]
        public int? FuncionarioId { get; set; }

        [Display(Name = "Department number")]
        public int? DepartamentoId { get; set; }

        [Display(Name = "Assigned at")]
        public DateTime AssignedAt { get; set; } = DateTime.Now;

        [Display(Name = "Ended at")]
        public DateTime? EndedAt { get; set; }

        [StringLength(500, ErrorMessage = "The {0} cannot exceed {1} characters.")]
        public string? Note { get; set; }

        // Methods from the class diagram
        public bool IsCurrent()
        {
            return EndedAt == null;
        }

        public void End()
        {
            EndedAt = DateTime.Now;
        }
    }
}
