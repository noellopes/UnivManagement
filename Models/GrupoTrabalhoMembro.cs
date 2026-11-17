using UnivManagement.Models.AcademicManagement;

namespace UnivManagement.Models
{
    public class GrupoTrabalhoMembro
    {
        public int GrupoTrabalhoMembroId { get; set; }

        public GrupoTrabalho? GrupoTrabalho { get; set; }
        public int TrabalhoId { get; set; }

        public int StudentId { get; set; }
        public Student? Student { get; set; }
    }
}