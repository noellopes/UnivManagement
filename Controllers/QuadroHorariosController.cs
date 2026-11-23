using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Controllers
{
    // Quadro de horarios: consulta. Dados de exemplo dentro do codigo (demo).
    // Quando a Aula/Turma/AtribuicaoDocente estiverem prontas, so TodosBlocos() e ObterCursos() mudam.
    // O atendimento (rosa) vem da tabela HorarioAtendimento, se existir na base de dados.
    public class QuadroHorariosController : Controller
    {
        private const int HoraInicioGrelha = 8; // 08:00

        private readonly UniversityDbContext _context;

        public QuadroHorariosController(UniversityDbContext context)
        {
            _context = context;
        }

        // ---------------- Cursos ----------------

        [HttpGet]
        [Route("Horarios")]
        public IActionResult Index()
        {
            return View(ObterCursos());
        }

        [HttpGet]
        [Route("Horarios/Curso/{cursoid}")]
        public IActionResult Curso(int cursoid, int ano = 1, int semestre = 1)
        {
            var curso = ObterCursos().FirstOrDefault(c => c.CursoId == cursoid);
            if (curso == null) return NotFound();

            var blocos = ObterBlocos(cursoid, ano, semestre);
            PosicionarBlocos(blocos);

            var vm = new QuadroCursoViewModel
            {
                Curso = curso,
                Ano = ano,
                Semestre = semestre,
                Anos = new List<int> { 1, 2, 3 },
                Blocos = blocos
            };
            return View(vm);
        }

        // ---------------- Professores ----------------

        [HttpGet]
        [Route("Horarios/Professores")]
        public IActionResult Professores()
        {
            return View(ObterDocentes());
        }

        [HttpGet]
        [Route("Horarios/Professor/{docenteid}")]
        public async Task<IActionResult> Professor(int docenteid, int semestre = 1)
        {
            var doc = ObterDocentes().FirstOrDefault(d => d.DocenteId == docenteid);
            if (doc == null) return NotFound();

            var blocos = TodosBlocos().Where(b => b.Docente == doc.Nome && b.Semestre == semestre).ToList();
            int minutosAula = blocos.Sum(b => (int)(b.Fim - b.Inicio).TotalMinutes);

            // atendimento gerado no HorarioAtendimento (desenhado como bloco "AT")
            var atendimentos = await ObterAtendimentosAsync(doc.Nome, semestre);
            foreach (var a in atendimentos)
            {
                blocos.Add(new BlocoAula
                {
                    CursoId = 0,
                    Ano = 0,
                    Semestre = semestre,
                    Dia = a.DiaSemana,
                    Inicio = a.HoraInicio,
                    Fim = a.HoraFim,
                    Uc = "Atendimento",
                    Tipo = "AT",
                    Turma = "—",
                    Sala = a.Observacoes ?? "",
                    Docente = doc.Nome
                });
            }

            PosicionarBlocos(blocos);

            var vm = new QuadroProfessorViewModel
            {
                DocenteId = docenteid,
                Nome = doc.Nome,
                Semestre = semestre,
                MinutosAula = minutosAula,
                MinutosAtendimento = MinutosNecessarios(minutosAula),
                Blocos = blocos
            };
            return View(vm);
        }

        // ---------------- Regra: um terco das horas de aula (arredondado para cima a 30 min) ----------------

        private static int MinutosNecessarios(int minutosAula)
        {
            if (minutosAula <= 0) return 0;
            return (int)Math.Ceiling(minutosAula / 3.0 / 30.0) * 30;
        }

        // ---------------- Atendimento: le a tabela HorarioAtendimento ----------------
        // Usa SQL direto de proposito: assim este ficheiro compila mesmo sem a classe HorarioAtendimento
        // (que so entra no master com o outro PR). Se a tabela nao existir, nao mostra atendimento.
        // Quando o PR do HorarioAtendimento estiver no master, pode trocar-se por _context.Set<HorarioAtendimento>().
        private async Task<List<AtendimentoLinha>> ObterAtendimentosAsync(string nomeDocente, int semestre)
        {
            try
            {
                return await _context.Database.SqlQuery<AtendimentoLinha>(
                    $@"SELECT a.DiaSemana, a.HoraInicio, a.HoraFim, a.Observacoes
                       FROM HorarioAtendimento a
                       INNER JOIN Docente d ON d.DocenteId = a.DocenteId
                       WHERE d.Nome = {nomeDocente} AND a.Semestre = {semestre}")
                    .ToListAsync();
            }
            catch (Exception)
            {
                return new List<AtendimentoLinha>();
            }
        }

        // ---------------- Fonte dos dados (trocar aqui depois) ----------------

        private static List<CursoQuadro> ObterCursos()
        {
            return new List<CursoQuadro>
            {
                new CursoQuadro { CursoId = 1, Nome = "Engenharia Informática", Sigla = "EI", Grau = "Licenciatura" },
                new CursoQuadro { CursoId = 2, Nome = "Gestão", Sigla = "GEST", Grau = "Licenciatura" },
                new CursoQuadro { CursoId = 3, Nome = "Design de Comunicação", Sigla = "DC", Grau = "Licenciatura" }
            };
        }

        private static List<DocenteQuadro> ObterDocentes()
        {
            return TodosBlocos()
                .Select(b => b.Docente).Distinct().OrderBy(n => n)
                .Select((n, i) => new DocenteQuadro { DocenteId = i + 1, Nome = n })
                .ToList();
        }

        private static List<BlocoAula> ObterBlocos(int cursoId, int ano, int semestre)
        {
            return TodosBlocos()
                .Where(b => b.CursoId == cursoId && b.Ano == ano && b.Semestre == semestre)
                .ToList();
        }

        private static List<BlocoAula> TodosBlocos()
        {
            return new List<BlocoAula>
            {
                // Engenharia Informática, 1.º ano, 1.º semestre
                B(1,1,1, 1,"09:00","11:00","Programação I","T","—","Sala 1.01","Docente A"),
                B(1,1,1, 1,"14:00","16:00","Matemática I","T","—","Sala 1.02","Docente B"),
                B(1,1,1, 2,"09:00","11:00","Programação I","PL","Turma A","Lab 2.01","Docente A"),
                B(1,1,1, 2,"09:00","11:00","Programação I","PL","Turma B","Lab 2.02","Docente C"),
                B(1,1,1, 2,"11:00","13:00","Arquitetura de Computadores","T","—","Sala 1.01","Docente D"),
                B(1,1,1, 3,"10:00","12:00","Matemática I","TP","Turma A","Sala 1.03","Docente B"),
                B(1,1,1, 3,"14:00","16:00","Inglês Técnico","TP","Turma A","Sala 1.04","Docente E"),
                B(1,1,1, 4,"09:00","11:00","Arquitetura de Computadores","PL","Turma A","Lab 2.01","Docente D"),
                B(1,1,1, 5,"11:00","13:00","Introdução à Engenharia","T","—","Sala 1.01","Docente F"),

                // Engenharia Informática, 1.º ano, 2.º semestre
                B(1,1,2, 1,"09:00","11:00","Programação II","T","—","Sala 1.01","Docente A"),
                B(1,1,2, 2,"14:00","16:00","Bases de Dados","T","—","Sala 1.02","Docente G"),
                B(1,1,2, 3,"09:00","11:00","Programação II","PL","Turma A","Lab 2.01","Docente A"),
                B(1,1,2, 4,"14:00","17:00","Bases de Dados","PL","Turma A","Lab 2.02","Docente G"),

                // Engenharia Informática, 2.º ano, 1.º semestre
                B(1,2,1, 1,"11:00","13:00","Estruturas de Dados","T","—","Sala 2.01","Docente H"),
                B(1,2,1, 2,"09:00","12:00","Programação Web","PL","Turma A","Lab 2.03","Docente I"),
                B(1,2,1, 4,"14:00","16:00","Redes de Computadores","T","—","Sala 2.02","Docente J"),
                B(1,2,1, 5,"09:00","11:00","Estruturas de Dados","TP","Turma A","Sala 2.03","Docente H"),

                // Gestão, 1.º ano, 1.º semestre
                B(2,1,1, 1,"09:00","11:00","Contabilidade I","T","—","Sala 3.01","Docente K"),
                B(2,1,1, 2,"11:00","13:00","Introdução à Gestão","T","—","Sala 3.01","Docente L"),
                B(2,1,1, 3,"14:00","16:00","Economia I","T","—","Sala 3.02","Docente M"),
                B(2,1,1, 4,"09:00","11:00","Contabilidade I","TP","Turma A","Sala 3.03","Docente K"),
                B(2,1,1, 5,"10:00","12:00","Matemática Aplicada","T","—","Sala 3.02","Docente N"),

                // Design de Comunicação, 1.º ano, 1.º semestre
                B(3,1,1, 1,"14:00","18:00","Atelier de Design I","PL","Turma A","Atelier 1","Docente O"),
                B(3,1,1, 2,"09:00","11:00","História da Arte","T","—","Sala 4.01","Docente P"),
                B(3,1,1, 3,"09:00","12:00","Desenho","PL","Turma A","Atelier 2","Docente Q"),
                B(3,1,1, 5,"14:00","16:00","Teoria da Imagem","T","—","Sala 4.01","Docente R"),
            };
        }

        private static BlocoAula B(int curso, int ano, int sem, int dia, string ini, string fim,
            string uc, string tipo, string turma, string sala, string docente)
        {
            return new BlocoAula
            {
                CursoId = curso,
                Ano = ano,
                Semestre = sem,
                Dia = dia,
                Inicio = TimeSpan.Parse(ini),
                Fim = TimeSpan.Parse(fim),
                Uc = uc,
                Tipo = tipo,
                Turma = turma,
                Sala = sala,
                Docente = docente
            };
        }

        // ---------------- Posicao na grelha (1 px por minuto) ----------------

        private static void PosicionarBlocos(List<BlocoAula> blocos)
        {
            foreach (var b in blocos)
            {
                b.Topo = (int)(b.Inicio.TotalMinutes - HoraInicioGrelha * 60);
                b.Altura = (int)(b.Fim - b.Inicio).TotalMinutes;

                // blocos sobrepostos no mesmo dia ficam lado a lado
                var sobrepostos = blocos
                    .Where(o => o.Dia == b.Dia && o.Inicio < b.Fim && b.Inicio < o.Fim)
                    .OrderBy(o => o.Inicio).ThenBy(o => o.Turma).ThenBy(o => o.Uc)
                    .ToList();

                int total = Math.Max(1, sobrepostos.Count);
                int indice = Math.Max(0, sobrepostos.IndexOf(b));
                b.Larg = 100 / total;
                b.Esq = b.Larg * indice;
            }
        }
    }

    // Linha lida da tabela HorarioAtendimento (so o que o quadro precisa)
    public class AtendimentoLinha
    {
        public int DiaSemana { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFim { get; set; }
        public string? Observacoes { get; set; }
    }
}
