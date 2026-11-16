using Microsoft.AspNetCore.Identity;
using UnivManagement.Models;

namespace UnivManagement.Data
{
    public static class SeedData
    {
        public static void Initialize(UniversityDbContext context)
        {
            // Garante que a base de dados está criada
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

                context.Estudantes.AddRange(aluno1, aluno2, aluno3);
                context.SaveChanges();
            }

            // 2. Criar Empresas (caso não existam)
            if (!context.Empresas.Any())
            {
                context.Empresas.AddRange(
                    new Empresa { Nome = "Tech Corp Portugal" },
                    new Empresa { Nome = "Inovação e Redes SA" }
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

            // 4. Criar as 5 Ofertas (caso não existam)
            if (!context.Ofertas.Any())
            {
                var empresaId = context.Empresas.FirstOrDefault()?.IdEmpresa ?? 1;
                var areaId = context.Areas.FirstOrDefault()?.IdArea ?? 1;

                context.Ofertas.AddRange(
                    new Oferta
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
                    new Oferta
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
                    new Oferta
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
                    new Oferta
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
                    new Oferta
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
                    context.Candidaturas.Add(new Candidatura
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
