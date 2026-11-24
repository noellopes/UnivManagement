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

            // AccommodationContract: relação com ContractStatus sem eliminação em cascata:
            modelBuilder.Entity<Models.AccommodationManagement.AccommodationContract>()
                .HasOne(c => c.Status)
                .WithMany(s => s.AccommodationContracts)
                .HasForeignKey(c => c.ContractStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            // AccommodationContract: precisão dos valores monetários:
            modelBuilder.Entity<Models.AccommodationManagement.AccommodationContract>()
                .Property(c => c.MonthlyRent)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Models.AccommodationManagement.AccommodationContract>()
                .Property(c => c.Deposit)
                .HasPrecision(10, 2);

            // ContractStatus: atributo Name é único e dados iniciais (seed):
            modelBuilder.Entity<Models.AccommodationManagement.ContractStatus>()
                .HasIndex(s => s.Name)
                .IsUnique();

            modelBuilder.Entity<Models.AccommodationManagement.ContractStatus>().HasData(
    new Models.AccommodationManagement.ContractStatus { ContractStatusId = 1, Name = "Pendente" },
    new Models.AccommodationManagement.ContractStatus { ContractStatusId = 2, Name = "Ativo" },
    new Models.AccommodationManagement.ContractStatus { ContractStatusId = 3, Name = "Cancelado" },
    new Models.AccommodationManagement.ContractStatus { ContractStatusId = 4, Name = "Expirado" });
        }

        

        public DbSet<UnivManagement.Models.AcademicManagement.Student> Student { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.AcademicYear> AcademicYear { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.UC> UC { get; set; } = default!;

        public DbSet<UnivManagement.Models.AccommodationManagement.AccommodationContract> AccommodationContract { get; set; } = default!;
        public DbSet<UnivManagement.Models.AccommodationManagement.ContractStatus> ContractStatus { get; set; } = default!;
    }
}