using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;

namespace UnivManagement.Data {
    public class UniversityDbContext : DbContext {

        public UniversityDbContext(DbContextOptions<UniversityDbContext> options)
            : base(options){
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // indica a Chave primaria da tabela Ano Letivo
            modelBuilder.Entity<AnoLetivo>().HasKey(a => a.IdAnoLetivo);
        }

        public DbSet<AnoLetivo> AnoLetivo { get; set; } = default!;
    }
}
