using System.Globalization;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

// Gerador de atendimento: num so ficheiro para nao precisar de pasta nova.
// Contem: Intervalo + GeradorAtendimento (logica), DocenteAulas + IFonteAulas (contrato)
// e FonteAulasExemplo (aulas de exemplo, a trocar quando a Aula entrar no master).
namespace UnivManagement.Services
{
    // Um intervalo ocupado (ou livre) numa semana. Dia: 1 = Segunda ... 5 = Sexta
    public record Intervalo(int Dia, TimeSpan Inicio, TimeSpan Fim);

    // Logica pura (sem base de dados): descobre onde cabe o atendimento de um docente.
    public static class GeradorAtendimento
    {
        public static readonly TimeSpan InicioDia = TimeSpan.FromHours(9);
        public static readonly TimeSpan FimDia = TimeSpan.FromHours(18);
        private const int PassoMinutos = 30;

        // Um terco das horas de aula por semana, arredondado para cima a multiplos de 30 min
        public static int MinutosNecessarios(int minutosAulaPorSemana)
        {
            if (minutosAulaPorSemana <= 0)
            {
                return 0;
            }

            var umTerco = minutosAulaPorSemana / 3.0;
            return (int)(Math.Ceiling(umTerco / PassoMinutos) * PassoMinutos);
        }

        // Primeiro intervalo livre (segunda a sexta, 09:00-18:00) que nao toca em nenhum ocupado.
        // Devolve null se nao houver espaco.
        public static Intervalo? ProcurarIntervalo(int minutosAulaPorSemana, IEnumerable<Intervalo> ocupados)
        {
            var duracao = TimeSpan.FromMinutes(MinutosNecessarios(minutosAulaPorSemana));
            if (duracao == TimeSpan.Zero)
            {
                return null;
            }

            var lista = ocupados.ToList();
            for (var dia = 1; dia <= 5; dia++)
            {
                for (var inicio = InicioDia; inicio + duracao <= FimDia; inicio += TimeSpan.FromMinutes(PassoMinutos))
                {
                    var fim = inicio + duracao;
                    var livre = !lista.Any(o => o.Dia == dia && o.Inicio < fim && o.Fim > inicio);
                    if (livre)
                    {
                        return new Intervalo(dia, inicio, fim);
                    }
                }
            }

            return null;
        }
    }

    // Tudo o que o gerador precisa de saber de UM docente.
    // AulasDosAlunos = aulas dos alunos dos cursos/anos onde o docente leciona (ja somadas).
    public record DocenteAulas(
        int DocenteId,
        string Nome,
        string Gabinete,
        IReadOnlyList<Intervalo> AulasDoDocente,
        IReadOnlyList<Intervalo> AulasDosAlunos);

    // De onde vem o horario de aulas. Hoje: exemplos (FonteAulasExemplo).
    // Quando a Aula/Turma entrarem no master, cria-se outra classe que le da base de dados
    // e troca-se UMA linha no controlador. O gerador nao muda.
    public interface IFonteAulas
    {
        Task<List<DocenteAulas>> ObterDocentesAsync(int semestre);
    }

    // Aulas de EXEMPLO (as mesmas do quadro de horarios: Docente A, Docente B, ...).
    // Liga cada nome ao Docente da base de dados que tem o mesmo Nome.
    // Quando a Aula/Turma entrarem no master, troca-se esta classe por uma que leia a base de dados.
    public class FonteAulasExemplo : IFonteAulas
    {
        private sealed record AulaExemplo(int Curso, int Ano, int Sem, int Dia, string Ini, string Fim, string Docente);

        private static AulaExemplo A(int curso, int ano, int sem, int dia, string ini, string fim, string docente) =>
            new AulaExemplo(curso, ano, sem, dia, ini, fim, docente);

        private static readonly AulaExemplo[] Aulas =
        {
            // Engenharia Informatica, 1.o ano, 1.o semestre
            A(1,1,1, 1,"09:00","11:00","Docente A"),
            A(1,1,1, 1,"14:00","16:00","Docente B"),
            A(1,1,1, 2,"09:00","11:00","Docente A"),
            A(1,1,1, 2,"09:00","11:00","Docente C"),
            A(1,1,1, 2,"11:00","13:00","Docente D"),
            A(1,1,1, 3,"10:00","12:00","Docente B"),
            A(1,1,1, 3,"14:00","16:00","Docente E"),
            A(1,1,1, 4,"09:00","11:00","Docente D"),
            A(1,1,1, 5,"11:00","13:00","Docente F"),

            // Engenharia Informatica, 1.o ano, 2.o semestre
            A(1,1,2, 1,"09:00","11:00","Docente A"),
            A(1,1,2, 2,"14:00","16:00","Docente G"),
            A(1,1,2, 3,"09:00","11:00","Docente A"),
            A(1,1,2, 4,"14:00","17:00","Docente G"),

            // Engenharia Informatica, 2.o ano, 1.o semestre
            A(1,2,1, 1,"11:00","13:00","Docente H"),
            A(1,2,1, 2,"09:00","12:00","Docente I"),
            A(1,2,1, 4,"14:00","16:00","Docente J"),
            A(1,2,1, 5,"09:00","11:00","Docente H"),

            // Gestao, 1.o ano, 1.o semestre
            A(2,1,1, 1,"09:00","11:00","Docente K"),
            A(2,1,1, 2,"11:00","13:00","Docente L"),
            A(2,1,1, 3,"14:00","16:00","Docente M"),
            A(2,1,1, 4,"09:00","11:00","Docente K"),
            A(2,1,1, 5,"10:00","12:00","Docente N"),

            // Design de Comunicacao, 1.o ano, 1.o semestre
            A(3,1,1, 1,"14:00","18:00","Docente O"),
            A(3,1,1, 2,"09:00","11:00","Docente P"),
            A(3,1,1, 3,"09:00","12:00","Docente Q"),
            A(3,1,1, 5,"14:00","16:00","Docente R")
        };

        private readonly UniversityDbContext _context;

        public FonteAulasExemplo(UniversityDbContext context)
        {
            _context = context;
        }

        public async Task<List<DocenteAulas>> ObterDocentesAsync(int semestre)
        {
            var nomes = Aulas.Where(a => a.Sem == semestre).Select(a => a.Docente).Distinct().ToList();

            // so gera para docentes que existem na base de dados com esse nome
            var docentesBd = await _context.Set<Docente>()
                .Where(d => nomes.Contains(d.Nome))
                .ToListAsync();

            var resultado = new List<DocenteAulas>();
            foreach (var d in docentesBd.OrderBy(d => d.Nome))
            {
                var doDocente = Aulas.Where(a => a.Sem == semestre && a.Docente == d.Nome).ToList();

                // cursos/anos onde o docente leciona -> aulas dos alunos desses cursos/anos
                var pares = doDocente.Select(a => (a.Curso, a.Ano)).Distinct().ToList();
                var dosAlunos = Aulas
                    .Where(a => a.Sem == semestre && pares.Contains((a.Curso, a.Ano)))
                    .Select(Para)
                    .ToList();

                resultado.Add(new DocenteAulas(
                    d.DocenteId,
                    d.Nome,
                    "B2." + (10 + resultado.Count),
                    doDocente.Select(Para).ToList(),
                    dosAlunos));
            }

            return resultado;
        }

        private static Intervalo Para(AulaExemplo a) =>
            new Intervalo(a.Dia,
                TimeSpan.Parse(a.Ini, CultureInfo.InvariantCulture),
                TimeSpan.Parse(a.Fim, CultureInfo.InvariantCulture));
    }
}
