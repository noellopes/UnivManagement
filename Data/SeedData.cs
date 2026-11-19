using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Models
{
    public static class SeedData
    {
        public static void Populate(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<UniversityDbContext>();

                // 1. Verifica se já existem departamentos na base de dados
                if (context.Departamentos != null && context.Departamentos.Any())
                {
                    return; // Se já houver dados, não faz nada para evitar duplicações
                }

                // 2. Adiciona os registos estáticos iniciais
                context.Departamentos.AddRange(
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
