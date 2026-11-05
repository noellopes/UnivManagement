
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;

namespace UnivManagement.Data
{
    public class UniversityDbContext(DbContextOptions<UniversityDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Biblioteca: impedir associações duplicadas
            // entre livros e autores
            modelBuilder.Entity<LivroAutor>()
                .HasIndex(la => new { la.LivroId, la.AutorId })
                .IsUnique();

            // ServiceRequestType: nomes únicos
            modelBuilder.Entity<Models.ServiceRequestType>()
                .HasIndex(t => t.Name)
                .IsUnique();

            // ServiceRequestStatus: nomes únicos
            modelBuilder.Entity<Models.ServiceRequestStatus>()
                .HasIndex(s => s.Name)
                .IsUnique();

            // ServiceRequest: impedir eliminação de tipos
            // ou estados utilizados por pedidos
            modelBuilder.Entity<Models.ServiceRequest>()
                .HasOne(r => r.Type)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.ServiceRequest>()
                .HasOne(r => r.Status)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            // Student: atributos únicos
            modelBuilder.Entity<Models.AcademicManagement.Student>()
                .HasIndex(u => u.NumStudent)
                .IsUnique();

            modelBuilder.Entity<Models.AcademicManagement.Student>()
                .HasIndex(u => u.NIF)
                .IsUnique();

            modelBuilder.Entity<Models.AcademicManagement.Student>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Models.AcademicManagement.Student>()
                .HasIndex(u => u.Telephone)
                .IsUnique();

            // UC: código único
            modelBuilder.Entity<Models.AcademicManagement.UC>()
                .HasIndex(u => u.Code)
                .IsUnique();

            // Room: número único por residência
            modelBuilder.Entity<Models.AccommodationManagement.Room>()
                .HasIndex(r => new { r.ResidenceId, r.Number })
                .IsUnique();

            // Course: código único
            modelBuilder.Entity<Models.AcademicManagement.Course>()
                .HasIndex(c => c.Code)
                .IsUnique();
        }

        // Biblioteca
        public DbSet<Livro> Livros { get; set; }
        public DbSet<Exemplar> Exemplares { get; set; }
        public DbSet<Autor> Autores { get; set; }
        public DbSet<LivroAutor> LivrosAutores { get; set; }

        // Pedidos de serviço
        public DbSet<UnivManagement.Models.ServiceRequestType> ServiceRequestTypes { get; set; } = default!;
        public DbSet<UnivManagement.Models.ServiceRequestStatus> ServiceRequestStatuses { get; set; } = default!;
        public DbSet<UnivManagement.Models.ServiceRequest> ServiceRequests { get; set; } = default!;

        // Gestão académica
        public DbSet<UnivManagement.Models.AcademicManagement.Student> Student { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.AcademicYear> AcademicYear { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.UC> UC { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.SchoolCalendar> SchoolCalendar { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.Course> Course { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.InscrUC> InscrUC { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.InscrCourse> InscrCourse { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.StudyPlan> StudyPlan { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.StudyPlanUC> StudyPlanUC { get; set; } = default!;

        // Refeições
        public DbSet<Refeicao> Refeicoes { get; set; }

        // Alojamento
        public DbSet<UnivManagement.Models.AccommodationManagement.StayRecord> StayRecord { get; set; } = default!;
        public DbSet<UnivManagement.Models.AccommodationManagement.AccommodationPayment> AccommodationPayment { get; set; } = default!;
        public DbSet<UnivManagement.Models.AccommodationManagement.AccommodationContract> AccommodationContract { get; set; } = default!;
        public DbSet<UnivManagement.Models.AccommodationManagement.ApplicationStatusHistory> ApplicationStatusHistory { get; set; } = default!;
        public DbSet<UnivManagement.Models.AccommodationManagement.AccommodationApplication> AccommodationApplication { get; set; } = default!;
        public DbSet<UnivManagement.Models.AccommodationManagement.Room> Room { get; set; } = default!;

        // Funcionários
        public DbSet<UnivManagement.Models.ServicoFuncionario> ServicoFuncionario { get; set; } = default!;
    }
}
