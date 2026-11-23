using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Models.AcademicManagement;

namespace UnivManagement.Data
{
    public class SeedData
    {
        internal static void Populate(UniversityDbContext? db)
        {
            if (db == null) throw new ArgumentNullException(nameof(db));

            PopulateAulas(db);
        }

        private static void PopulateAulas(UniversityDbContext db)
        {
            // 1. Garantir que existem Anos Letivos (AcademicYear) na BD
            if (!db.AcademicYear.Any())
            {
                db.AcademicYear.AddRange(
                    new AcademicYear { AcademicYearId = 0, StartYear = 2025, EndYear = 2026 },
                    new AcademicYear { AcademicYearId = 0, StartYear = 2026, EndYear = 2027 }
                );
                db.SaveChanges();
            }

            var anoLetivoAtual = db.AcademicYear.FirstOrDefault(a => a.StartYear == 2026) ?? db.AcademicYear.First();

            // 2. Garantir que existem Cursos (Course) na BD
            if (!db.Course.Any())
            {
                // Garantir FK obrigatória: SchoolCalendar
                var calendario = db.SchoolCalendar.FirstOrDefault();
                if (calendario == null)
                {
                    calendario = new SchoolCalendar
                    {
                        StartYear = new DateTime(2026, 9, 15),
                        EndYear = new DateTime(2027, 7, 15),
                        StartSemesterS1 = new DateTime(2026, 9, 15),
                        EndSemesterS1 = new DateTime(2027, 1, 15),
                        StartFreqExamS1 = new DateTime(2027, 1, 18),
                        EndFreqExamS1 = new DateTime(2027, 1, 30),
                        StartResitExamS1 = new DateTime(2027, 2, 1),
                        EndResitExamS1 = new DateTime(2027, 2, 12),
                        StartSpecialExamS1 = new DateTime(2027, 2, 15),
                        EndSpecialExamS1 = new DateTime(2027, 2, 20),
                        StartSemesterS2 = new DateTime(2027, 2, 22),
                        EndSemesterS2 = new DateTime(2027, 6, 11),
                        StartFreqExamS2 = new DateTime(2027, 6, 14),
                        EndFreqExamS2 = new DateTime(2027, 6, 26),
                        StartResitExamS2 = new DateTime(2027, 6, 28),
                        EndResitExamS2 = new DateTime(2027, 7, 9),
                        StartSpecialExamS2 = new DateTime(2027, 7, 12),
                        EndSpecialExamS2 = new DateTime(2027, 7, 16),
                        AcademicYearId = anoLetivoAtual.AcademicYearId
                    };
                    db.SchoolCalendar.Add(calendario);
                    db.SaveChanges();
                }

                // Garantir FK obrigatória: Docente (Diretor de Curso)
                var docente = db.Set<Docente>().FirstOrDefault();
                if (docente == null)
                {
                    docente = new Docente
                    {
                        Nome = "Prof. Noel",
                        Nif = "123456789",
                        Email = "noel@ipg.pt",
                        Telefone = "912345678",
                        FuncaoDocenteFK = "Professor Adjunto",
                        RegimeFK = "Tempo Integral",
                        Ativo = true,
                        DataAdmissao = new DateTime(2010, 9, 1),
                        Morada = "Guarda",
                        DepartamentoFk = 1
                    };
                    db.Set<Docente>().Add(docente);
                    db.SaveChanges();
                }

                db.Course.AddRange(
                    new Course
                    {
                        Code = "LEI01",
                        Name = "Engenharia Informática",
                        AcademicDegree = "Licenciatura",
                        Acronym = "LEI",
                        CreationDate = new DateTime(2005, 9, 1),
                        CourseState = "Ativo",
                        SchoolCalendarId = calendario.SchoolCalendarId,
                        DocenteId = docente.DocenteId
                    },
                    new Course
                    {
                        Code = "LEC01",
                        Name = "Engenharia Civil",
                        AcademicDegree = "Licenciatura",
                        Acronym = "LEC",
                        CreationDate = new DateTime(2005, 9, 1),
                        CourseState = "Ativo",
                        SchoolCalendarId = calendario.SchoolCalendarId,
                        DocenteId = docente.DocenteId
                    }
                );
                db.SaveChanges();
            }

            // 3. Garantir que existem Turmas (ClassGroup) na BD (sem prefixo de curso)
            if (!db.ClassGroup.Any())
            {
                var turmas = new List<Group>
                {
                    new Group { NomeTurma = "1º Ano - Turma A", NumeroAlunos = 35, Ativo = true },
                    new Group { NomeTurma = "2º Ano - Turma A", NumeroAlunos = 28, Ativo = true },
                    new Group { NomeTurma = "3º Ano - Turma B", NumeroAlunos = 22, Ativo = true },
                    new Group { NomeTurma = "1º Ano - Turma B", NumeroAlunos = 18, Ativo = true }
                };

                db.ClassGroup.AddRange(turmas);
                db.SaveChanges();
            }

            // 4. Se a tabela ClassSession já tiver registos, não faz nada
            if (db.ClassSession.Any()) return;

            // 5. Obter os registos criados/existentes para associar os IDs reais
            var turma1A = db.ClassGroup.FirstOrDefault(g => g.NomeTurma == "1º Ano - Turma A");
            var turma2A = db.ClassGroup.FirstOrDefault(g => g.NomeTurma == "2º Ano - Turma A");
            var turma3B = db.ClassGroup.FirstOrDefault(g => g.NomeTurma == "3º Ano - Turma B");

            var cursoLei = db.Course.FirstOrDefault(c => c.Acronym == "LEI") ?? db.Course.First();

            // 6. Adicionar lista de Sessões (ClassSession) iniciais
            var sessoes = new List<Session>
            {
                new Session
                {
                    DiaSemana = "Segunda-feira",
                    HoraInicio = new TimeSpan(9, 0, 0),
                    HoraFim = new TimeSpan(11, 0, 0),
                    TipoAula = "Teórica",
                    Ativo = true,
                    IdSala = 101,
                    IdTurma = turma1A?.GroupId ?? 1,
                    CourseId = cursoLei.CourseId,
                    AcademicYearId = anoLetivoAtual.AcademicYearId,
                    Semestre = 1
                },
                new Session
                {
                    DiaSemana = "Segunda-feira",
                    HoraInicio = new TimeSpan(11, 0, 0),
                    HoraFim = new TimeSpan(13, 0, 0),
                    TipoAula = "Prática Laboratorial",
                    Ativo = true,
                    IdSala = 204,
                    IdTurma = turma2A?.GroupId ?? 1,
                    CourseId = cursoLei.CourseId,
                    AcademicYearId = anoLetivoAtual.AcademicYearId,
                    Semestre = 1
                },
                new Session
                {
                    DiaSemana = "Quarta-feira",
                    HoraInicio = new TimeSpan(14, 0, 0),
                    HoraFim = new TimeSpan(16, 30, 0),
                    TipoAula = "Teórico-Prática",
                    Ativo = true,
                    IdSala = 105,
                    IdTurma = turma3B?.GroupId ?? 1,
                    CourseId = cursoLei.CourseId,
                    AcademicYearId = anoLetivoAtual.AcademicYearId,
                    Semestre = 2
                },
                new Session
                {
                    DiaSemana = "Sexta-feira",
                    HoraInicio = new TimeSpan(16, 30, 0),
                    HoraFim = new TimeSpan(18, 30, 0),
                    TipoAula = "Seminário",
                    Ativo = false,
                    IdSala = 301,
                    IdTurma = turma1A?.GroupId ?? 1,
                    CourseId = cursoLei.CourseId,
                    AcademicYearId = anoLetivoAtual.AcademicYearId,
                    Semestre = 2
                }
            };

            db.ClassSession.AddRange(sessoes);
            db.SaveChanges();
        }
    }
}