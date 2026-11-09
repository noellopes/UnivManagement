using System;
using System.Collections.Generic;
using System.Linq;
using UnivManagement.Models.TestesEstagios;

namespace UnivManagement.Data
{
    public class EstagiosTesteSeedData
    {
        internal static void Populate(EstagiosTesteDbContext? dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            // Alunos
            if (!dbContext.Student.Any())
            {
                dbContext.Student.AddRange(new List<Student>
                {
                    new Student { Name = "João Costa" },
                    new Student { Name = "Maria Silva" },
                    new Student { Name = "Nuno Gomes" },
                    new Student { Name = "Pedro Alves" },
                    new Student { Name = "Rita Santos" },
                    new Student { Name = "Capitolino Andrioleta dos Conselhos Bonifácios" }
                });

                dbContext.SaveChanges();
            }

            // Empresas
            if (!dbContext.Empresas.Any())
            {
                dbContext.Empresas.AddRange(new List<Empresa>
                {
                    new Empresa { Nome = "MEO" },
                    new Empresa { Nome = "Netflix" },
                    new Empresa { Nome = "Gemini" },
                    new Empresa { Nome = "Covipneus" },
                    new Empresa { Nome = "Steam" },
                    new Empresa { Nome = "Zé da Praça" }
                });

                dbContext.SaveChanges();
            }

            // Orientadores de Estágio
            if (!dbContext.OrientadoresEstagio.Any())
            {
                dbContext.OrientadoresEstagio.AddRange(
                    new List<OrientadorEstagio>
                    {
                        new OrientadorEstagio { Nome = "Noel Jesus Lopes" },
                        new OrientadorEstagio { Nome = "Maria Cecília Rosa" },
                        new OrientadorEstagio { Nome = "César Rafael Gonçalves" },
                        new OrientadorEstagio { Nome = "Fernando António Carvalho Marcos" },
                        new OrientadorEstagio { Nome = "Maria Cecília dos Santos" },
                        new OrientadorEstagio { Nome = "Natália Maria Oliveira Pratas Rodrigues" }
                    });

                dbContext.SaveChanges();
            }

            // Estágios
            if (!dbContext.Estagios.Any())
            {
                dbContext.Estagios.AddRange(new List<Estagio>
                {
                    new Estagio
                    {
                        DataInicio = new DateTime(2026, 9, 3, 15, 58, 0),
                        DataFim = new DateTime(2028, 1, 28, 15, 58, 0),
                        Duracao = 512,
                        Estado = "Em Curso",
                        PlanoTrabalhos = "Fingir que trabalha",
                        StudentId = 2,
                        IdEmpresa = 2,
                        IdOrientador = 6
                    },

                    new Estagio
                    {
                        DataInicio = new DateTime(2026, 10, 6, 16, 13, 0),
                        DataFim = new DateTime(2026, 11, 6, 16, 13, 0),
                        Duracao = 31,
                        Estado = "Concluído",
                        PlanoTrabalhos = "Sobreviver",
                        StudentId = 1,
                        IdEmpresa = 1,
                        IdOrientador = 5
                    },

                    new Estagio
                    {
                        DataInicio = new DateTime(2027, 9, 26, 16, 13, 0),
                        DataFim = new DateTime(2028, 1, 26, 16, 13, 0),
                        Duracao = 122,
                        Estado = "Pendente",
                        PlanoTrabalhos = "Acabar um jogo de minesweeper",
                        StudentId = 3,
                        IdEmpresa = 3,
                        IdOrientador = 4
                    },

                    new Estagio
                    {
                        DataInicio = new DateTime(2026, 11, 1, 15, 15, 0),
                        DataFim = new DateTime(2027, 11, 1, 16, 15, 0),
                        Duracao = 365,
                        Estado = "Em Curso",
                        PlanoTrabalhos = "Tirar finos com a espuma perfeita",
                        StudentId = 6,
                        IdEmpresa = 6,
                        IdOrientador = 1
                    }
                });

                dbContext.SaveChanges();
            }
        }
    }
}