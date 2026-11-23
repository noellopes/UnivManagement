using Microsoft.EntityFrameworkCore;
using UnivManagement.Models.AcademicManagement;

namespace UnivManagement.Data {
    public class SeedData {
        internal static void Populate(UniversityDbContext? db) {
            if (db == null) throw new ArgumentNullException(nameof(db));

            PopulateStateInscrUC(db);
            PopulateStateEnrollment(db);
            PopulateAcademicDegrees(db);
            PopulateCourseStates(db);
            PopulateUCs(db);
            PopulateCourses(db);
        }

        internal static void PopulateStateInscrUC(UniversityDbContext db) {
            if (db.StateInscrUC.Any()) return;

            db.StateInscrUC.AddRange(
                new StateInscrUC {
                    Name = "ONGOING",
                    Description = "A inscrição na unidade curricular está ativa."
                },
                new StateInscrUC {
                    Name = "CANCELED",
                    Description = "A inscrição na unidade curricular foi cancelada."
                },
                new StateInscrUC {
                    Name = "CONCLUDED",
                    Description = "A unidade curricular foi concluída."
                }
            );

            db.SaveChanges();
        }

        internal static void PopulateStateEnrollment(UniversityDbContext db) {
            if (db.StateEnrollment.Any()) return;

            db.StateEnrollment.AddRange(
                new StateEnrollment {
                    Name = "UNPAID",
                    Description = "A inscrição ainda não foi marcada como paga."
                },
                new StateEnrollment {
                    Name = "ONGOING",
                    Description = "A inscrição está ativa."
                },
                new StateEnrollment {
                    Name = "CANCELED",
                    Description = "A inscrição foi cancelada."
                },
                new StateEnrollment {
                    Name = "CONCLUDED",
                    Description = "A inscrição foi concluída."
                }
            );

            db.SaveChanges();
        }

        internal static void PopulateAcademicDegrees(UniversityDbContext db) {
            if (db.AcademicDegree.Any()) return;

            db.AcademicDegree.AddRange(
                new AcademicDegree {
                    Name = "CTeSP",
                    Description = "Curso Técnico Superior Profissional."
                },
                new AcademicDegree {
                    Name = "Licenciatura",
                    Description = "Licenciatura."
                },
                new AcademicDegree {
                    Name = "Mestrado",
                    Description = "Mestrado."
                },
                new AcademicDegree {
                    Name = "Doutoramento",
                    Description = "Doutoramento."
                }
            );

            db.SaveChanges();
        }

        internal static void PopulateCourseStates(UniversityDbContext db) {
            if (db.CourseState.Any()) return;

            db.CourseState.AddRange(
                new CourseState {
                    Name = "DRAFT",
                    Description = "Curso em preparação, ainda não publicado."
                },
                new CourseState {
                    Name = "ACTIVE",
                    Description = "Curso a decorrer normalmente."
                },
                new CourseState {
                    Name = "SUSPENDED",
                    Description = "Curso temporariamente suspenso."
                },
                new CourseState {
                    Name = "CONCLUDED",
                    Description = "Curso terminado, sem novas inscrições."
                },
                new CourseState {
                    Name = "ARCHIVED",
                    Description = "Curso arquivado historicamente."
                }
            );

            db.SaveChanges();
        }

        internal static void PopulateUCs(UniversityDbContext db) {
            if (db.UC.Any()) return;

            db.UC.AddRange(
                new UC {
                    Code = "PROG01",
                    Name = "Programação I",
                    Acronym = "PROG1",
                    Description = "Introdução aos fundamentos da programação imperativa.",
                    ValueECTS = 6,
                    NumHours = 60,
                    Semester = 1,
                    Year = 1
                },
                new UC {
                    Code = "PROG02",
                    Name = "Programação II",
                    Acronym = "PROG2",
                    Description = "Programação orientada a objetos e estruturas de dados.",
                    ValueECTS = 6,
                    NumHours = 60,
                    Semester = 1,
                    Year = 1
                },
                new UC {
                    Code = "MAT01",
                    Name = "Análise Matemática",
                    Acronym = "AMAT",
                    Description = "Cálculo diferencial e integral.",
                    ValueECTS = 6,
                    NumHours = 60,
                    Semester = 1,
                    Year = 1
                },
                new UC {
                    Code = "MAT02",
                    Name = "Álgebra Linear",
                    Acronym = "ALIN",
                    Description = "Matrizes, sistemas lineares e espaços vetoriais.",
                    ValueECTS = 6,
                    NumHours = 60,
                    Semester = 1,
                    Year = 1
                },
                new UC {
                    Code = "BD01",
                    Name = "Bases de Dados",
                    Acronym = "BD",
                    Description = "Modelação relacional, SQL e normalização.",
                    ValueECTS = 6,
                    NumHours = 60,
                    Semester = 1,
                    Year = 1
                },
                new UC {
                    Code = "SO01",
                    Name = "Sistemas Operativos",
                    Acronym = "SO",
                    Description = "Processos, threads, gestão de memória e sistemas de ficheiros.",
                    ValueECTS = 6,
                    NumHours = 60,
                    Semester = 1,
                    Year = 1
                },
                new UC {
                    Code = "RC01",
                    Name = "Redes de Computadores",
                    Acronym = "RC",
                    Description = "Modelo OSI/TCP-IP, routing e switching.",
                    ValueECTS = 6,
                    NumHours = 60,
                    Semester = 1,
                    Year = 1
                },
                new UC {
                    Code = "EST01",
                    Name = "Estatística",
                    Acronym = "EST",
                    Description = "Probabilidade, inferência estatística e amostragem.",
                    ValueECTS = 6,
                    NumHours = 60,
                    Semester = 1,
                    Year = 1
                },
                new UC {
                    Code = "ES01",
                    Name = "Engenharia de Software",
                    Acronym = "ES",
                    Description = "Ciclo de vida, requisitos, arquitetura e testes.",
                    ValueECTS = 6,
                    NumHours = 60,
                    Semester = 1,
                    Year = 1
                },
                new UC {
                    Code = "IA01",
                    Name = "Inteligência Artificial",
                    Acronym = "IA",
                    Description = "Pesquisa, representação do conhecimento e machine learning introdutório.",
                    ValueECTS = 6,
                    NumHours = 60,
                    Semester = 1,
                    Year = 1
                },
                new UC {
                    Code = "SEG01",
                    Name = "Segurança Informática",
                    Acronym = "SEG",
                    Description = "Criptografia, autenticação e segurança de aplicações.",
                    ValueECTS = 6,
                    NumHours = 60,
                    Semester = 1,
                    Year = 1
                },
                new UC {
                    Code = "PDM01",
                    Name = "Programação para Dispositivos Móveis",
                    Acronym = "PDM",
                    Description = "Desenvolvimento de aplicações móveis.",
                    ValueECTS = 6,
                    NumHours = 60,
                    Semester = 1,
                    Year = 1
                }
            );

            db.SaveChanges();
        }

        internal static void PopulateCourses(UniversityDbContext db) {
            if (db.Course.Any()) return;

            // Lookups – depends on PopulateAcademicDegrees and PopulateCourseStates having run first.
            var ctesp = db.AcademicDegree.First(d => d.Name == "CTeSP");
            var licenciatura = db.AcademicDegree.First(d => d.Name == "Licenciatura");
            var mestrado = db.AcademicDegree.First(d => d.Name == "Mestrado");

            var draft = db.CourseState.First(s => s.Name == "DRAFT");
            var active = db.CourseState.First(s => s.Name == "ACTIVE");
            var suspended = db.CourseState.First(s => s.Name == "SUSPENDED");

            db.Course.AddRange(
                new Course {
                    Code = "LEI",
                    Name = "Licenciatura em Engenharia Informática",
                    Acronym = "LEI",
                    CreationDate = new DateOnly(2010, 9, 1),
                    Description = "Formação em programação, sistemas, redes e engenharia de software.",
                    AcademicDegreeId = licenciatura.AcademicDegreeId,
                    CourseStateId = active.CourseStateId
                },
                new Course {
                    Code = "LEMat",
                    Name = "Licenciatura em Matemática",
                    Acronym = "LMAT",
                    CreationDate = new DateOnly(2012, 9, 1),
                    Description = "Formação sólida em matemática pura e aplicada.",
                    AcademicDegreeId = licenciatura.AcademicDegreeId,
                    CourseStateId = active.CourseStateId
                },
                new Course {
                    Code = "LEC",
                    Name = "Licenciatura em Engenharia Civil",
                    Acronym = "LEC",
                    CreationDate = new DateOnly(2008, 9, 1),
                    Description = "Estruturas, materiais, geotecnia e construção.",
                    AcademicDegreeId = licenciatura.AcademicDegreeId,
                    CourseStateId = active.CourseStateId
                },
                new Course {
                    Code = "LEG",
                    Name = "Licenciatura em Gestão",
                    Acronym = "LGES",
                    CreationDate = new DateOnly(2015, 9, 1),
                    Description = "Gestão de empresas, contabilidade e marketing.",
                    AcademicDegreeId = licenciatura.AcademicDegreeId,
                    CourseStateId = suspended.CourseStateId
                },
                new Course {
                    Code = "MEI",
                    Name = "Mestrado em Engenharia Informática",
                    Acronym = "MEI",
                    CreationDate = new DateOnly(2014, 9, 1),
                    Description = "Especialização em engenharia de software, IA e sistemas distribuídos.",
                    AcademicDegreeId = mestrado.AcademicDegreeId,
                    CourseStateId = active.CourseStateId
                },
                new Course {
                    Code = "MES",
                    Name = "Mestrado em Engenharia de Software",
                    Acronym = "MES",
                    CreationDate = new DateOnly(2026, 9, 1),
                    Description = "Mestrado em preparação, arranque previsto para 2026/2027.",
                    AcademicDegreeId = mestrado.AcademicDegreeId,
                    CourseStateId = draft.CourseStateId
                },
                new Course {
                    Code = "TPSI",
                    Name = "CTeSP em Tecnologias e Programação de Sistemas de Informação",
                    Acronym = "TPSI",
                    CreationDate = new DateOnly(2018, 9, 1),
                    Description = "Curso técnico superior profissional em desenvolvimento de software.",
                    AcademicDegreeId = ctesp.AcademicDegreeId,
                    CourseStateId = active.CourseStateId
                },
                new Course {
                    Code = "REDES",
                    Name = "CTeSP em Redes e Infraestruturas",
                    Acronym = "CRINF",
                    CreationDate = new DateOnly(2020, 9, 1),
                    Description = "Curso técnico superior profissional em administração de redes.",
                    AcademicDegreeId = ctesp.AcademicDegreeId,
                    CourseStateId = active.CourseStateId
                }
            );

            db.SaveChanges();
        }
    }
}
