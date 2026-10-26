using Microsoft.EntityFrameworkCore;

namespace UnivManagement.Data {
    public class UniversityDbContext(DbContextOptions<UniversityDbContext> options) : DbContext(options) {

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //faz referencia a chave primaria da tabela UC
            modelBuilder.Entity<Models.AcademicManagement.UC>().HasKey(u => u.UcId);

            //indica uqe o atributo code é unico, não podendo haver duas UCs com o mesmo código
            modelBuilder.Entity<Models.AcademicManagement.UC>()
                .HasIndex(u => u.Code)
                .IsUnique();
        }

        public DbSet<UnivManagement.Models.AcademicManagement.UC> AcademicYear { get; set; } = default!;
    }
}
