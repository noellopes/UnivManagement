using System;
using System.Linq;
using System.Collections.Generic;
using UnivManagement.Models.TestesEstagios;

namespace UnivManagement.Data
{
    internal class EstagiosTesteSeedData
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
            }

            dbContext.SaveChanges();
        }
    }
}