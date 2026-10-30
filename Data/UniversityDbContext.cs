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

            // Room: número único por residência:
            modelBuilder.Entity<Models.AccommodationManagement.Room>()
                .HasIndex(r => new { r.ResidenceId, r.Number })
                .IsUnique();

            // indica que o atributo Code é único na tabela Course:
            modelBuilder.Entity<Models.AcademicManagement.Course>()
                .HasIndex(c => c.Code)
                .IsUnique();
        }
      
        public DbSet<UnivManagement.Models.AcademicManagement.Student> Student { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.AcademicYear> AcademicYear { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.UC> UC { get; set; } = default!;
        public DbSet<UnivManagement.Models.AccommodationManagement.StayRecord> StayRecord { get; set; } = default!;

        public DbSet<UnivManagement.Models.AccommodationManagement.AccommodationContract> AccommodationContract { get; set; } = default!;
        public DbSet<UnivManagement.Models.AccommodationManagement.ApplicationStatusHistory> ApplicationStatusHistory { get; set; } = default!;

        public DbSet<UnivManagement.Models.AccommodationManagement.AccommodationApplication> AccommodationApplication { get; set; } = default!;
        public DbSet<UnivManagement.Models.AccommodationManagement.Room> Room { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.SchoolCalendar> SchoolCalendar { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.Course> Course { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.InscrUC> InscrUC { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.InscrCourse> InscrCourse { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.StudyPlan> StudyPlan { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.StudyPlanUC> StudyPlanUC { get; set; } = default!;
    }
}
