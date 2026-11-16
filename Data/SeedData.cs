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

            PopulateStudents(db);
            PopulateEmpresas(db);
            PopulateOrientadores(db);
            PopulateEstagios(db);
        }

        private static void PopulateStudents(UniversityDbContext db)
        {
            var alunos = db.Student.ToList();

            string[] nomes =
            {
                "João Costa",
                "Maria Silva",
                "Pedro Almeida",
                "Ana Santos",
                "Miguel Ferreira",
                "Inês Oliveira",
                "Tiago Rodrigues",
                "Beatriz Sousa",
                "André Martins",
                "Rita Gomes",
                "Diogo Pereira",
                "Carolina Lopes",
                "Ricardo Ribeiro",
                "Mariana Carvalho",
                "Francisco Pinto",
                "Sofia Fernandes",
                "Gonçalo Mendes",
                "Leonor Alves",
                "Daniel Rocha",
                "Marta Correia",
                "Bruno Teixeira",
                "Catarina Moreira",
                "Rafael Nunes",
                "Joana Neves",
                "David Marques",
                "Laura Cardoso",
                "Nuno Azevedo",
                "Clara Reis",
                "Hugo Monteiro",
                "Matilde Ramos",
                "Tomás Barbosa",
                "Mafalda Castro",
                "Lucas Cunha",
                "Sara Duarte",
                "Vasco Matos",
                "Alice Fonseca",
                "Luís Vieira",
                "Júlia Coelho",
                "Eduardo Dias",
                "Adriana Batista",
                "Filipe Melo",
                "Diana Machado",
                "Samuel Campos",
                "Patrícia Faria",
                "António Simões"
            };

            int i = 0;

            while (alunos.Count < 45)
            {
                decimal numeroAluno = 2000001 + i;
                decimal nif = 900000001 + i;
                string email = "aluno" + numeroAluno + "@example.com";
                string telefone = (910000001 + i).ToString();

                if (!alunos.Any(a =>
                    a.NumStudent == numeroAluno ||
                    a.NIF == nif ||
                    a.Email == email ||
                    a.Telephone == telefone))
                {
                    Student aluno = new Student
                    {
                        StudentId = 0,
                        NumStudent = numeroAluno,
                        Name = nomes[i % nomes.Length],
                        Gender = i % nomes.Length % 2 == 0 ? "M" : "F",
                        Nationality = "Portuguesa",
                        BirthDate = new DateTime(
                            2000 + i % 6,
                            1 + i % 12,
                            1 + i % 28),
                        Address = "Rua das Flores, " + (i + 1),
                        NIF = nif,
                        Email = email,
                        Telephone = telefone
                    };

                    db.Student.Add(aluno);
                    alunos.Add(aluno);
                }

                i++;
            }

            db.SaveChanges();
        }

        private static void PopulateEmpresas(UniversityDbContext db)
        {
            if (db.Empresas.Any())
                return;

            db.Empresas.AddRange(new List<Empresa>
            {
                new Empresa
                {
                    Nome = "Lusocode",
                    Setor = "Informática",
                    Localizacao = "Coimbra",
                    Contacto = "lusocode@example.com"
                },
                new Empresa
                {
                    Nome = "Atlantic Systems",
                    Setor = "Tecnologia",
                    Localizacao = "Aveiro",
                    Contacto = "atlantic@example.com"
                },
                new Empresa
                {
                    Nome = "Norte Digital",
                    Setor = "Desenvolvimento de Software",
                    Localizacao = "Porto",
                    Contacto = "nortedigital@example.com"
                },
                new Empresa
                {
                    Nome = "Serra Solutions",
                    Setor = "Consultoria",
                    Localizacao = "Guarda",
                    Contacto = "serrasolutions@example.com"
                },
                new Empresa
                {
                    Nome = "Horizonte Engenharia",
                    Setor = "Engenharia",
                    Localizacao = "Lisboa",
                    Contacto = "horizonte@example.com"
                },
                new Empresa
                {
                    Nome = "Beira Network",
                    Setor = "Telecomunicações",
                    Localizacao = "Viseu",
                    Contacto = "beiranetwork@example.com"
                },
                new Empresa
                {
                    Nome = "Nova Data",
                    Setor = "Análise de Dados",
                    Localizacao = "Braga",
                    Contacto = "novadata@example.com"
                },
                new Empresa
                {
                    Nome = "Vector Automation",
                    Setor = "Automação Industrial",
                    Localizacao = "Leiria",
                    Contacto = "vector@example.com"
                },
                new Empresa
                {
                    Nome = "Pixel Studio",
                    Setor = "Design Digital",
                    Localizacao = "Castelo Branco",
                    Contacto = "pixel@example.com"
                },
                new Empresa
                {
                    Nome = "Securebyte",
                    Setor = "Cibersegurança",
                    Localizacao = "Covilhã",
                    Contacto = "securebyte@example.com"
                }
            });

            db.SaveChanges();
        }

        private static void PopulateOrientadores(UniversityDbContext db)
        {
            var orientadores = db.OrientadoresEstagio.ToList();

            string[] nomes =
            {
                "Carlos Andrade",
                "Helena Marques",
                "Manuel Tavares",
                "Isabel Correia",
                "Paulo Gonçalves",
                "Teresa Martins",
                "Jorge Almeida",
                "Cristina Ferreira",
                "Fernando Lopes",
                "Sandra Ribeiro",
                "António Cardoso",
                "Luísa Santos",
                "Rui Machado",
                "Susana Pereira",
                "José Oliveira"
            };

            int i = 0;

            while (orientadores.Count < 15)
            {
                string email = "orientador" + (i + 1) + "@example.com";

                if (!orientadores.Any(o => o.Email == email))
                {
                    OrientadorEstagio orientador = new OrientadorEstagio
                    {
                        Nome = nomes[i % nomes.Length],
                        Email = email,
                        Password = "Orientador" + (i + 1) + "!"
                    };

                    db.OrientadoresEstagio.Add(orientador);
                    orientadores.Add(orientador);
                }

                i++;
            }

            db.SaveChanges();
        }

        private static void PopulateEstagios(UniversityDbContext db)
        {
            var alunos = db.Student.ToList();
            var empresas = db.Empresas.ToList();
            var orientadores = db.OrientadoresEstagio.ToList();
            var estagios = db.Estagios.ToList();

            if (!alunos.Any() ||
                !empresas.Any() ||
                !orientadores.Any())
            {
                return;
            }

            string[] planos =
            {
                "Desenvolvimento de aplicações Web",
                "Manutenção de sistemas informáticos",
                "Desenvolvimento de bases de dados",
                "Análise e tratamento de dados",
                "Desenvolvimento de aplicações móveis",
                "Configuração de redes informáticas",
                "Testes e validação de software",
                "Desenvolvimento de interfaces gráficas",
                "Automação de processos industriais",
                "Implementação de sistemas de segurança",
                "Desenvolvimento de serviços Web",
                "Gestão de infraestrutura tecnológica",
                "Documentação técnica de sistemas",
                "Desenvolvimento de ferramentas internas",
                "Otimização de sistemas informáticos"
            };

            DateTime hoje = DateTime.Today;

            int i = 0;

            while (estagios.Count < 50)
            {
                DateTime dataInicio;
                DateTime dataFim;
                string estado;

                if (i % 3 == 0)
                {
                    dataInicio = hoje.AddDays(-180 - i);
                    dataFim = hoje.AddDays(-30 - i);
                    estado = "Concluído";
                }
                else if (i % 3 == 1)
                {
                    dataInicio = hoje.AddDays(150 + i);
                    dataFim = dataInicio.AddDays(120);
                    estado = "Pendente";
                }
                else
                {
                    dataInicio = hoje.AddDays(-30 - i);
                    dataFim = hoje.AddDays(180 + i);
                    estado = "Em Curso";
                }

                int studentId =
                    alunos[i % alunos.Count].StudentId;

                int idEmpresa =
                    empresas[i % empresas.Count].IdEmpresa;

                int orientadorId =
                    orientadores[i % orientadores.Count].OrientadorId;

                string plano = planos[i % planos.Length];

                if (!estagios.Any(e =>
                    e.StudentId == studentId &&
                    e.IdEmpresa == idEmpresa &&
                    e.OrientadorId == orientadorId &&
                    e.DataInicio == dataInicio &&
                    e.DataFim == dataFim &&
                    e.PlanoTrabalhos == plano))
                {
                    Estagio estagio = new Estagio
                    {
                        DataInicio = dataInicio,
                        DataFim = dataFim,
                        Duracao = (dataFim - dataInicio).Days,
                        Estado = estado,
                        PlanoTrabalhos = plano,
                        StudentId = studentId,
                        IdEmpresa = idEmpresa,
                        OrientadorId = orientadorId
                    };

                    db.Estagios.Add(estagio);
                    estagios.Add(estagio);
                }

                i++;
            }

            db.SaveChanges();
        }
    }
}