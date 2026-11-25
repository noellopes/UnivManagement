namespace UnivManagement.Models
{
    public class DocenteQuadro
    {
        public int DocenteId { get; set; }
        public string Nome { get; set; } = "";
    }

    public class QuadroProfessorViewModel
    {
        public int DocenteId { get; set; }
        public string Nome { get; set; } = "";
        public int Semestre { get; set; }
        public int MinutosAula { get; set; }
        public int MinutosAtendimento { get; set; }   // 1/3 das horas de aula, arredondado para cima a 30 min
        public List<BlocoAula> Blocos { get; set; } = new List<BlocoAula>();
    }
}