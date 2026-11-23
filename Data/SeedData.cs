using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;

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
            // 1. Garantir que existem Turmas (ClassGroup) na BD
            if (!db.ClassGroup.Any())
            {
                var turmas = new List<Group>
                {
                    new Group { NomeTurma = "LEI - 1º Ano - Turma A", NumeroAlunos = 35, Ativo = true },
                    new Group { NomeTurma = "LEI - 2º Ano - Turma A", NumeroAlunos = 28, Ativo = true },
                    new Group { NomeTurma = "LEI - 3º Ano - Turma B", NumeroAlunos = 22, Ativo = true },
                    new Group { NomeTurma = "Eng. Civil - 1º Ano - Turma A", NumeroAlunos = 18, Ativo = false }
                };

                db.ClassGroup.AddRange(turmas);
                db.SaveChanges();
            }

            // 2. Se a tabela ClassSession já tiver registos, não faz nada
            if (db.ClassSession.Any()) return;

            // 3. Obter as turmas criadas/existentes para associar os IDs reais
            var turma1Lei = db.ClassGroup.FirstOrDefault(g => g.NomeTurma == "LEI - 1º Ano - Turma A");
            var turma2Lei = db.ClassGroup.FirstOrDefault(g => g.NomeTurma == "LEI - 2º Ano - Turma A");
            var turma3Lei = db.ClassGroup.FirstOrDefault(g => g.NomeTurma == "LEI - 3º Ano - Turma B");

            // 4. Adicionar lista de Sessões (ClassSession) iniciais
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
                    IdTurma = turma1Lei?.GroupId ?? 1
                },
                new Session
                {
                    DiaSemana = "Segunda-feira",
                    HoraInicio = new TimeSpan(11, 0, 0),
                    HoraFim = new TimeSpan(13, 0, 0),
                    TipoAula = "Prática Laboratorial",
                    Ativo = true,
                    IdSala = 204,
                    IdTurma = turma2Lei?.GroupId ?? 1
                },
                new Session
                {
                    DiaSemana = "Quarta-feira",
                    HoraInicio = new TimeSpan(14, 0, 0),
                    HoraFim = new TimeSpan(16, 30, 0),
                    TipoAula = "Teórico-Prática",
                    Ativo = true,
                    IdSala = 105,
                    IdTurma = turma3Lei?.GroupId ?? 1
                },
                new Session
                {
                    DiaSemana = "Sexta-feira",
                    HoraInicio = new TimeSpan(16, 30, 0),
                    HoraFim = new TimeSpan(18, 30, 0),
                    TipoAula = "Seminário",
                    Ativo = false,
                    IdSala = 301,
                    IdTurma = turma1Lei?.GroupId ?? 1
                }
            };

            db.ClassSession.AddRange(sessoes);
            db.SaveChanges();
        }
    }
}
