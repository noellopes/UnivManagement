using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new UniversityDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<UniversityDbContext>>()))
            {
                // 1. Verifica se já existem departamentos na base de dados
                if (context.Departamento.Any())
                {
                    return; // Se já houver dados, não faz nada para evitar duplicações
                }

                // 2. Adiciona os registos estáticos iniciais
                context.Departamento.AddRange(
                    new Departamento { Nome = "Informática" },
                    new Departamento { Nome = "Matemática" },
                    new Departamento { Nome = "Gestão" }
                );

                // 3. Guarda as alterações na base de dados
                context.SaveChanges();
            }
        }
    }
}
