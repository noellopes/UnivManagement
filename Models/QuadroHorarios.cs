namespace UnivManagement.Models
{
    public class CursoQuadro
    {
        public int CursoId { get; set; }
        public string Nome { get; set; } = "";
        public string Sigla { get; set; } = "";
        public string Grau { get; set; } = "";
    }

    
    public class BlocoAula
    {
        public int CursoId { get; set; }
        public int Ano { get; set; }
        public int Semestre { get; set; }
        public int Dia { get; set; }              
        public TimeSpan Inicio { get; set; }
        public TimeSpan Fim { get; set; }
        public string Uc { get; set; } = "";
        public string Tipo { get; set; } = "T";   // T, TP, PL
        public string Turma { get; set; } = "";
        public string Sala { get; set; } = "";
        public string Docente { get; set; } = "";

        
        public int Topo { get; set; }
        public int Altura { get; set; }
        public int Esq { get; set; }
        public int Larg { get; set; } = 100;
    }

    public class QuadroCursoViewModel
    {
        public CursoQuadro Curso { get; set; } = new CursoQuadro();
        public int Ano { get; set; }
        public int Semestre { get; set; }
        public List<int> Anos { get; set; } = new List<int>();
        public List<BlocoAula> Blocos { get; set; } = new List<BlocoAula>();
    }
}
