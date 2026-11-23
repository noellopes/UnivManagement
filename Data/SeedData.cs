using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Models.AcademicManagement;

namespace UnivManagement.Data
{
    public class SeedData
    {
        internal static void Populate(UniversityDbContext? db)
        {
            if (db == null)
                throw new ArgumentNullException(nameof(db));

            string[] tipos = {
                "Avaliação Contínua",
                "Exame Normal",
                "Exame de Recurso",
                "Exame Época Especial"
            };

            foreach (var tipo in tipos)
            {
                if (!db.TiposAvaliacao.Any(t => t.TipoProva == tipo))
                {
                    db.TiposAvaliacao.Add(new TipoAvaliacao
                    {
                        TipoProva = tipo
                    });
                }
            }

            db.SaveChanges();

            // Inserir as UCs
            if (!db.UC.Any())
            {
                db.UC.AddRange(
                    new UC { Code = "ES2", Name = "Engenharia de Software II", Acronym = "ES2", Description = "Processos de desenvolvimento, requisitos, modelação UML e testes de software." },
                    new UC { Code = "IA", Name = "Inteligência Artificial", Acronym = "IA", Description = "Procura, representação do conhecimento e aprendizagem automática." },
                    new UC { Code = "CG", Name = "Computação Gráfica", Acronym = "CG", Description = "Transformações geométricas, modelação 3D e iluminação." },
                    new UC { Code = "CGE", Name = "Criação e Gestão de Empresas", Acronym = "CGE", Description = "Plano de negócios, gestão financeira e empreendedorismo." }
                );

                db.SaveChanges();
            }

            // Inserir os anos letivos
            if (!db.AcademicYear.Any())
            {
                db.AcademicYear.AddRange(
                    new AcademicYear { AcademicYearId = 0, StartYear = 2024, EndYear = 2025 },
                    new AcademicYear { AcademicYearId = 0, StartYear = 2025, EndYear = 2026 },
                    new AcademicYear { AcademicYearId = 0, StartYear = 2026, EndYear = 2027 }
                );

                db.SaveChanges();
            }

            // Inserir os alunos
            if (!db.Student.Any())
            {
                db.Student.AddRange(
                    new Student { StudentId = 0, NumStudent = 1710231, Name = "Rita Sofia Marques Pinto", Gender = "F", Nationality = "Portuguesa", BirthDate = new DateTime(2005, 4, 12), Address = "Rua Francisco de Passos, 18, 6300-559 Guarda", NIF = 254318760, Email = "1710231@ipg.pt", Telephone = "912345671" },
                    new Student { StudentId = 0, NumStudent = 1710245, Name = "Tiago André Ferreira Lopes", Gender = "M", Nationality = "Portuguesa", BirthDate = new DateTime(2004, 9, 3), Address = "Avenida Cidade de Salamanca, 42, 6300-538 Guarda", NIF = 251907432, Email = "1710245@ipg.pt", Telephone = "913456782" },
                    new Student { StudentId = 0, NumStudent = 1710258, Name = "Inês Carolina Gomes Ribeiro", Gender = "F", Nationality = "Portuguesa", BirthDate = new DateTime(2005, 1, 27), Address = "Rua Marquês de Pombal, 7, 6200-140 Covilhã", NIF = 256120894, Email = "1710258@ipg.pt", Telephone = "914567893" }
                );

                db.SaveChanges();
            }

            var anoLetivo = db.AcademicYear.OrderByDescending(a => a.StartYear).First();
            var ucs = db.UC.ToList();

            // Inserir as inscrições dos alunos no curso
            if (!db.InscrCourse.Any())
            {
                foreach (var aluno in db.Student.ToList())
                {
                    db.InscrCourse.Add(
                        new InscrCourse
                        {
                            InscrCourseId = 0,
                            CoursePlanYearId = 1,
                            StudentId = aluno.StudentId,
                            AcademicYearId = anoLetivo.AcademicYearId,
                            StateInscr = "ONGOING",
                            DateInscr = new DateOnly(2026, 9, 14),
                            FinalAvgGrade = 13.4m,
                            CourseFeePaid = true
                        }
                    );
                }

                db.SaveChanges();
            }

            // Inserir as inscrições dos alunos nas UCs (cada aluno inscrito em duas UCs)
            if (!db.InscrUC.Any())
            {
                var inscricoesCurso = db.InscrCourse.ToList();

                for (int i = 0; i < inscricoesCurso.Count; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        db.InscrUC.Add(
                            new InscrUC
                            {
                                InscrUCId = 0,
                                UcId = ucs[(i + j) % ucs.Count].UcId,
                                InscrCourseId = inscricoesCurso[i].InscrCourseId,
                                AcademicYearId = anoLetivo.AcademicYearId,
                                StateInscr = "ONGOING",
                                DateInscr = new DateOnly(2026, 9, 21),
                                Grade = 0
                            }
                        );
                    }
                }

                db.SaveChanges();
            }


            // Inserir os docentes
            if (!db.Set<Docente>().Any())
            {
                db.Set<Docente>().AddRange(
                    new Docente { Nome = "Ana Margarida Sousa Carvalho", Nif = "198765430", Email = "anacarvalho@ipg.pt", Telefone = "271220100", FuncaoDocenteFK = "Professor Adjunto", RegimeFK = "Dedicação Exclusiva", Ativo = true, DataAdmissao = new DateTime(2012, 9, 1), Morada = "Rua Dr. Francisco dos Prazeres, 25, 6300-690 Guarda", DepartamentoFk = 1 },
                    new Docente { Nome = "Paulo Jorge Antunes Rebelo", Nif = "187654329", Email = "paulorebelo@ipg.pt", Telefone = "271220101", FuncaoDocenteFK = "Professor Coordenador", RegimeFK = "Dedicação Exclusiva", Ativo = true, DataAdmissao = new DateTime(2004, 10, 1), Morada = "Avenida Rainha D. Amélia, 3, 6300-749 Guarda", DepartamentoFk = 1 },
                    new Docente { Nome = "Marta Isabel Correia Fonseca", Nif = "223456781", Email = "martafonseca@ipg.pt", Telefone = "271220102", FuncaoDocenteFK = "Assistente Convidado", RegimeFK = "Tempo Parcial", Ativo = true, DataAdmissao = new DateTime(2022, 2, 14), Morada = "Rua do Comércio, 11, 6270-480 Seia", DepartamentoFk = 1 }
                );

                db.SaveChanges();
            }
        }
    }
}
