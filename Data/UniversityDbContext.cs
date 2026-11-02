using Microsoft.EntityFrameworkCore;

namespace UnivManagement.Data
{
    public class UniversityDbContext(DbContextOptions<UniversityDbContext> options) : DbContext(options)
    {

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            // Room: relações com RoomType e RoomStatus (sem apagar quartos em cascata):
            modelBuilder.Entity<Models.AccommodationManagement.Room>()
                .HasOne(r => r.RoomType)
                .WithMany(t => t.Rooms)
                .HasForeignKey(r => r.RoomTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.AccommodationManagement.Room>()
                .HasOne(r => r.RoomStatus)
                .WithMany(s => s.Rooms)
                .HasForeignKey(r => r.RoomStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            // RoomType: nome único:
            modelBuilder.Entity<Models.AccommodationManagement.RoomType>()
                .HasIndex(t => t.Name)
                .IsUnique();

            // RoomStatus: nome único:
            modelBuilder.Entity<Models.AccommodationManagement.RoomStatus>()
                .HasIndex(s => s.Name)
                .IsUnique();

            // Seed dos valores que antes eram enums:
            modelBuilder.Entity<Models.AccommodationManagement.RoomType>().HasData(
                new Models.AccommodationManagement.RoomType { RoomTypeId = 1, Name = "Single" },
                new Models.AccommodationManagement.RoomType { RoomTypeId = 2, Name = "Double" },
                new Models.AccommodationManagement.RoomType { RoomTypeId = 3, Name = "Triple" });

            modelBuilder.Entity<Models.AccommodationManagement.RoomStatus>().HasData(
                new Models.AccommodationManagement.RoomStatus { RoomStatusId = 1, Name = "Available" },
                new Models.AccommodationManagement.RoomStatus { RoomStatusId = 2, Name = "Maintenance" });
        }

        public DbSet<UnivManagement.Models.AcademicManagement.Student> Student { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.AcademicYear> AcademicYear { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.UC> UC { get; set; } = default!;

        public DbSet<UnivManagement.Models.AccommodationManagement.Room> Room { get; set; } = default!;
        public DbSet<UnivManagement.Models.AccommodationManagement.RoomType> RoomType { get; set; } = default!;
        public DbSet<UnivManagement.Models.AccommodationManagement.RoomStatus> RoomStatus { get; set; } = default!;
    }
}