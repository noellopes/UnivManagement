using Microsoft.EntityFrameworkCore;

namespace UnivManagement.Data {
    public class UniversityDbContext(DbContextOptions<UniversityDbContext> options) : DbContext(options) {

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

            // ServiceRequestPriority: two priorities cannot have the same name
            modelBuilder.Entity<Models.ServiceRequestPriority>()
                .HasIndex(p => p.Name)
                .IsUnique();

            // ServiceRequest: a priority that is used by a request cannot be deleted
            modelBuilder.Entity<Models.ServiceRequest>()
                .HasOne(r => r.Priority)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            // ServiceRequestCatalogItem: two services of the catalog cannot have the same name
            modelBuilder.Entity<Models.ServiceRequestCatalogItem>()
                .HasIndex(s => s.Name)
                .IsUnique();

            // ServiceRequestCatalogItem: a type that is used by a service cannot be deleted
            modelBuilder.Entity<Models.ServiceRequestCatalogItem>()
                .HasOne(s => s.Type)
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
        }
      
        public DbSet<UnivManagement.Models.ServiceRequestType> ServiceRequestTypes { get; set; } = default!;
        public DbSet<UnivManagement.Models.ServiceRequestStatus> ServiceRequestStatuses { get; set; } = default!;
        public DbSet<UnivManagement.Models.ServiceRequest> ServiceRequests { get; set; } = default!;
        public DbSet<UnivManagement.Models.ServiceRequestHistory> ServiceRequestHistories { get; set; } = default!;
        public DbSet<UnivManagement.Models.ServiceRequestComment> ServiceRequestComments { get; set; } = default!;
        public DbSet<UnivManagement.Models.ServiceRequestPriority> ServiceRequestPriorities { get; set; } = default!;
        public DbSet<UnivManagement.Models.ServiceRequestCatalogItem> ServiceRequestCatalogItems { get; set; } = default!;

        public DbSet<UnivManagement.Models.AcademicManagement.Student> Student { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.AcademicYear> AcademicYear { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.UC> UC { get; set; } = default!;
    }
}
