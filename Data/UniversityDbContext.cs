using Microsoft.EntityFrameworkCore;

namespace UnivManagement.Data {
    public class UniversityDbContext(DbContextOptions<UniversityDbContext> options) : DbContext(options) {

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            // Student: atributo NumStudent é único:
            modelBuilder.Entity<Models.AcademicManagement.Student>()
                .HasIndex(u => u.NumStudent)
                .IsUnique();

            // Student: atributo NIF é único:
            modelBuilder.Entity<Models.AcademicManagement.Student>()
                .HasIndex(u => u.NIF)
                .IsUnique();

            // Student: atributo Email é único:
            modelBuilder.Entity<Models.AcademicManagement.Student>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Student: atributo Telephone é único:
            modelBuilder.Entity<Models.AcademicManagement.Student>()
                .HasIndex(u => u.Telephone)
                .IsUnique();
          
            // Indica que o atributo code é unico, não podendo haver duas UCs com o mesmo código:
            modelBuilder.Entity<Models.AcademicManagement.UC>()
                .HasIndex(u => u.Code)
                .IsUnique();
        }
      
        public DbSet<UnivManagement.Models.AcademicManagement.Student> Student { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.AcademicYear> AcademicYear { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.UC> UC { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.CourseStudyPlanYear> CourseStudyPlanYear { get; set; } = default!;
    }
}
