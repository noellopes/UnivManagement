using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;

namespace UnivManagement.Data {
    public class UniversityDbContext : DbContext {


        public UniversityDbContext(DbContextOptions<UniversityDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //faz referencia a chave primaria da tabela UC
            modelBuilder.Entity<Uc>().HasKey(u => u.UcId);

            //indica uqe o atributo code é unico, não podendo haver duas UCs com o mesmo código
            modelBuilder.Entity<Uc>()
                .HasIndex(u => u.Code)
                .IsUnique();
        }

        public DbSet<Uc> Uc { get; set; } = default!;

    }
}
