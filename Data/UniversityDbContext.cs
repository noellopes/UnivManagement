using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;

namespace UnivManagement.Data {
    public class UniversityDbContext(DbContextOptions<UniversityDbContext> options) : DbContext(options) {
        public DbSet<UnivManagement.Models.Convidado> Convidado { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            // ServiceRequestType: two request types cannot have the same name
            modelBuilder.Entity<Models.ServiceRequestType>()
                .HasIndex(t => t.Name)
                .IsUnique();

            // ServiceRequestStatus: two statuses cannot have the same name
            modelBuilder.Entity<Models.ServiceRequestStatus>()
                .HasIndex(s => s.Name)
                .IsUnique();

            // ServiceRequest: a type or status that is used by a request cannot be deleted
            modelBuilder.Entity<Models.ServiceRequest>()
                .HasOne(r => r.Type)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.ServiceRequest>()
                .HasOne(r => r.Status)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

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

            // Evita duplicações na tabela CourseStudyPlanYear:
            modelBuilder.Entity<Models.AcademicManagement.CourseStudyPlanYear>()
                .HasIndex(c => new { c.CourseId, c.StudyPlanId, c.AcademicYearId })
                .IsUnique();
        }
      
        public DbSet<UnivManagement.Models.ServiceRequestType> ServiceRequestTypes { get; set; } = default!;
        public DbSet<UnivManagement.Models.ServiceRequestStatus> ServiceRequestStatuses { get; set; } = default!;
        public DbSet<UnivManagement.Models.ServiceRequest> ServiceRequests { get; set; } = default!;

        public DbSet<UnivManagement.Models.AcademicManagement.Student> Student { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.AcademicYear> AcademicYear { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.UC> UC { get; set; } = default!;

        // Grupo 3
        public DbSet<UnivManagement.Models.AvContinua> AvContinua { get; set; } = default!;
    }
}
