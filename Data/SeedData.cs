using Microsoft.AspNetCore.Identity;
using UnivManagement.Models;

namespace UnivManagement.Data
{
    public static class SeedData
    {
        public static void Initialize(UniversityDbContext context)
        {
            context.Database.EnsureCreated();

            // 1. Criar Estudantes (caso não existam)
            if (!context.Estudantes.Any())
            {
                var passwordHasher = new PasswordHasher<Estudante>();

                var aluno1 = new Estudante
                {
                    Nome = "Ana Martins",
                    Email = "ana.martins@univ.pt",
                    Curso = "Engenharia Informática",
                    SituacaoProfissional = "Estudante a Tempo Inteiro"
                };
                aluno1.Password = passwordHasher.HashPassword(aluno1, "Password123!");

                var aluno2 = new Estudante
                {
                    Nome = "Carlos Pereira",
                    Email = "carlos.pereira@univ.pt",
                    Curso = "Engenharia Informática",
                    SituacaoProfissional = "Trabalhador-Estudante"
                };
                aluno2.Password = passwordHasher.HashPassword(aluno2, "Password124!");

                var aluno3 = new Estudante
                {
                    Nome = "José Costa",
                    Email = "jose.costa@univ.pt",
                    Curso = "Engenharia Informática",
                    SituacaoProfissional = "Antigo-Estudante"
                };
                aluno3.Password = passwordHasher.HashPassword(aluno3, "Password125!");

                var aluno4 = new Estudante
                {
                    Nome = "Mariana Ferreira",
                    Email = "mariana.ferreira@univ.pt",
                    Curso = "Engenharia Informática",
                    SituacaoProfissional = "Arquitecta de Sistemas"
                };
                aluno4.Password = passwordHasher.HashPassword(aluno4, "Password126!");

                var aluno5 = new Estudante
                {
                    Nome = "Tiago Almeida",
                    Email = "tiago.almeida@univ.pt",
                    Curso = "Engenharia de Redes",
                    SituacaoProfissional = "Administrador de Redes"
                };
                aluno5.Password = passwordHasher.HashPassword(aluno5, "Password127!");

                var aluno6 = new Estudante
                {
                    Nome = "Beatriz Rodrigues",
                    Email = "beatriz.rodrigues@univ.pt",
                    Curso = "Engenharia Informática",
                    SituacaoProfissional = "Product Owner"
                };
                aluno6.Password = passwordHasher.HashPassword(aluno6, "Password128!");

                var aluno7 = new Estudante
                {
                    Nome = "Diogo Carvalho",
                    Email = "diogo.carvalho@univ.pt",
                    Curso = "Engenharia de Telecomunicações",
                    SituacaoProfissional = "Empregado (Conta de Outrem)"
                };
                aluno7.Password = passwordHasher.HashPassword(aluno7, "Password129!");

                var aluno8 = new Estudante
                {
                    Nome = "Sofia Mendes",
                    Email = "sofia.mendes@univ.pt",
                    Curso = "Engenharia Informática",
                    SituacaoProfissional = "Consultora de Segurança Informática"
                };
                aluno8.Password = passwordHasher.HashPassword(aluno8, "Password130!");

                context.Estudantes.AddRange(aluno1, aluno2, aluno3, aluno4, aluno5, aluno6, aluno7, aluno8);
                context.SaveChanges();
            }

            // 1b. Criar Antigos Alunos (caso não existam)
            if (!context.AntigosAlunos.Any())
            {
                var hasherAntigoAluno = new PasswordHasher<AntigoAluno>();

                var antigoAluno1 = new AntigoAluno
                {
                    Nome = "José Costa",
                    Email = "jose.costa@univ.pt",
                    Curso = "Engenharia Informática",
                    SituacaoProfissional = "Antigo-Estudante"
                };
                antigoAluno1.Password = hasherAntigoAluno.HashPassword(antigoAluno1, "Password125!");

                var antigoAluno2 = new AntigoAluno
                {
                    Nome = "Mariana Ferreira",
                    Email = "mariana.ferreira@univ.pt",
                    Curso = "Engenharia Informática",
                    SituacaoProfissional = "Arquitecta de Sistemas"
                };
                antigoAluno2.Password = hasherAntigoAluno.HashPassword(antigoAluno2, "Password126!");

                var antigoAluno3 = new AntigoAluno
                {
                    Nome = "Tiago Almeida",
                    Email = "tiago.almeida@univ.pt",
                    Curso = "Engenharia de Redes",
                    SituacaoProfissional = "Administrador de Redes"
                };
                antigoAluno3.Password = hasherAntigoAluno.HashPassword(antigoAluno3, "Password127!");

                var antigoAluno4 = new AntigoAluno
                {
                    Nome = "Beatriz Rodrigues",
                    Email = "beatriz.rodrigues@univ.pt",
                    Curso = "Engenharia Informática",
                    SituacaoProfissional = "Product Owner"
                };
                antigoAluno4.Password = hasherAntigoAluno.HashPassword(antigoAluno4, "Password128!");

                var antigoAluno5 = new AntigoAluno
                {
                    Nome = "Diogo Carvalho",
                    Email = "diogo.carvalho@univ.pt",
                    Curso = "Engenharia de Telecomunicações",
                    SituacaoProfissional = "Empregado (Conta de Outrem)"
                };
                antigoAluno5.Password = hasherAntigoAluno.HashPassword(antigoAluno5, "Password129!");

                var antigoAluno6 = new AntigoAluno
                {
                    Nome = "Sofia Mendes",
                    Email = "sofia.mendes@univ.pt",
                    Curso = "Engenharia Informática",
                    SituacaoProfissional = "Consultora de Segurança Informática"
                };
                antigoAluno6.Password = hasherAntigoAluno.HashPassword(antigoAluno6, "Password130!");


                context.AntigosAlunos.AddRange(antigoAluno1, antigoAluno2, antigoAluno3, antigoAluno4, antigoAluno5, antigoAluno6);
                context.SaveChanges();
            }

            // 2. Criar Empresas (caso não existam)
            if (!context.Empresas.Any())
            {
                context.Empresas.AddRange(
                    new Empresas { Nome = "Tech Corp Portugal" },
                    new Empresas { Nome = "Inovação e Redes SA" }
                );
                context.SaveChanges();
            }

            // 3. Criar Áreas (caso não existam)
            if (!context.Areas.Any())
            {
                context.Areas.AddRange(
                    new Area { Nome = "Desenvolvimento de Software" },
                    new Area { Nome = "Redes e Infraestruturas" }
                );
                context.SaveChanges();
            }

            var primeiraEmpresa = context.Empresas.FirstOrDefault();
            var primeiraArea = context.Areas.FirstOrDefault();

            // 4. Criar as 5 Ofertas (caso não existam)
            if (!context.Ofertas.Any() && primeiraEmpresa != null && primeiraArea != null)
            {
                var empresaId = primeiraEmpresa.IdEmpresa;
                var areaId = primeiraArea.IdArea;

                context.Ofertas.AddRange(
                    new Ofertas
                    {
                        IdEmpresa = empresaId,
                        IdArea = areaId,
                        Titulo = "Estágio Desenvolvimento Web ASP.NET",
                        Descricao = "Estágio curricular na área de tecnologias Microsoft.",
                        Tipo = "Estágio Curricular",
                        Localizacao = "Remoto",
                        Requisitos = "C# .NET, SQL Server, MVC",
                        Remuneracao = 850.00,
                        DataLimite = DateTime.Today.AddDays(30),
                        Estado = "Aberta"
                    },
                    new Ofertas
                    {
                        IdEmpresa = empresaId,
                        IdArea = areaId,
                        Titulo = "Estágio Engenharia de Redes",
                        Descricao = "Apoio na configuração de infraestruturas de rede.",
                        Tipo = "Estágio Profissional",
                        Localizacao = "Lisboa",
                        Requisitos = "CCNA, Cisco Packet Tracer, OSPF",
                        Remuneracao = 900.00,
                        DataLimite = DateTime.Today.AddDays(25),
                        Estado = "Aberta"
                    },
                    new Ofertas
                    {
                        IdEmpresa = empresaId,
                        IdArea = areaId,
                        Titulo = "Desenvolvimento de Aplicação Mobile Kotlin",
                        Descricao = "Criação de soluções móveis nativas e com Jetpack Compose.",
                        Tipo = "Estágio Curricular",
                        Localizacao = "Porto",
                        Requisitos = "Kotlin, Android Studio, APIs REST",
                        Remuneracao = 800.00,
                        DataLimite = DateTime.Today.AddDays(20),
                        Estado = "Aberta"
                    },
                    new Ofertas
                    {
                        IdEmpresa = empresaId,
                        IdArea = areaId,
                        Titulo = "Administração de Sistemas e Bases de Dados",
                        Descricao = "Gestão de instâncias SQL Server e otimização de consultas.",
                        Tipo = "Part-Time",
                        Localizacao = "Coimbra",
                        Requisitos = "SQL Server, T-SQL, Indexação",
                        Remuneracao = 950.00,
                        DataLimite = DateTime.Today.AddDays(15),
                        Estado = "Aberta"
                    },
                    new Ofertas
                    {
                        IdEmpresa = empresaId,
                        IdArea = areaId,
                        Titulo = "Programação de Sistemas Embutidos com Arduino",
                        Descricao = "Desenvolvimento de firmware e integração com sensores IoT.",
                        Tipo = "Estágio Curricular",
                        Localizacao = "Guarda",
                        Requisitos = "C/C++, Arduino, Eletrónica básica",
                        Remuneracao = 750.00,
                        DataLimite = DateTime.Today.AddDays(40),
                        Estado = "Aberta"
                    }
                );
                context.SaveChanges();
            }

            // 5. Criar Candidatura inicial (caso não exista)
            if (!context.Candidaturas.Any())
            {
                var primeiraOferta = context.Ofertas.FirstOrDefault();
                var primeiroAluno = context.Estudantes.FirstOrDefault();

                if (primeiraOferta != null && primeiroAluno != null)
                {
                    context.Candidaturas.Add(new Candidaturas
                    {
                        IdAluno = primeiroAluno.IdAluno,
                        IdOferta = primeiraOferta.IdOferta,
                        DataCandidatura = DateTime.Today,
                        Estado = "Submetida",
                        Curriculo = "CV_Ana_Martins.pdf"
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}