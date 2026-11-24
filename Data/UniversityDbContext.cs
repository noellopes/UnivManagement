using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using AcademicManagement = UnivManagement.Models.AcademicManagement;
using AccommodationManagement = UnivManagement.Models.AccommodationManagement;

namespace UnivManagement.Data
{
    public class UniversityDbContext : DbContext
    {
        public UniversityDbContext(
            DbContextOptions<UniversityDbContext> options)
            : base(options)
        {
        }

        public DbSet<Convidado> Convidado { get; set; } = default!;

        public DbSet<ServiceRequestType> ServiceRequestTypes { get; set; } = default!;
        public DbSet<ServiceRequestStatus> ServiceRequestStatuses { get; set; } = default!;
        public DbSet<ServiceRequest> ServiceRequests { get; set; } = default!;

        public DbSet<TipoDocumento> TipoDocumento { get; set; } = default!;
        public DbSet<TipoCandidatura> TipoCandidatura { get; set; } = default!;
        public DbSet<TipoBolsa> TipoBolsa { get; set; } = default!;
        public DbSet<CriterioElegibilidade> CriterioElegibilidade { get; set; } = default!;

        public DbSet<AcademicManagement.Student> Student { get; set; } = default!;
        public DbSet<AcademicManagement.AcademicYear> AcademicYear { get; set; } = default!;
        public DbSet<AcademicManagement.UC> UC { get; set; } = default!;
        public DbSet<AcademicManagement.SchoolCalendar> SchoolCalendar { get; set; } = default!;
        public DbSet<AcademicManagement.Course> Course { get; set; } = default!;
        public DbSet<AcademicManagement.InscrUC> InscrUC { get; set; } = default!;
        public DbSet<AcademicManagement.InscrCourse> InscrCourse { get; set; } = default!;
        public DbSet<AcademicManagement.StudyPlan> StudyPlan { get; set; } = default!;
        public DbSet<AcademicManagement.StudyPlanUC> StudyPlanUC { get; set; } = default!;
        public DbSet<AcademicManagement.CourseStudyPlanYear> CourseStudyPlanYear { get; set; } = default!;

        public DbSet<Refeicao> Refeicoes { get; set; } = default!;
        public DbSet<TipoRefeicao> TiposRefeicao { get; set; } = default!;

        public DbSet<AccommodationManagement.StayRecord> StayRecord { get; set; } = default!;
        public DbSet<AccommodationManagement.AccommodationPayment> AccommodationPayment { get; set; } = default!;
        public DbSet<AccommodationManagement.AccommodationContract> AccommodationContract { get; set; } = default!;
        public DbSet<AccommodationManagement.ApplicationStatusHistory> ApplicationStatusHistory { get; set; } = default!;
        public DbSet<AccommodationManagement.AccommodationApplication> AccommodationApplication { get; set; } = default!;
        public DbSet<AccommodationManagement.Room> Room { get; set; } = default!;

        public DbSet<ServicoFuncionario> ServicoFuncionario { get; set; } = default!;

        public DbSet<AntigoAluno> AntigosAlunos { get; set; } = default!;
        public DbSet<Carreira> Carreiras { get; set; } = default!;
        public DbSet<Empresa> Empresas { get; set; } = default!;
        public DbSet<Empregador> Empregadores { get; set; } = default!;
        public DbSet<Oferta> Ofertas { get; set; } = default!;
        public DbSet<Candidatura> Candidaturas { get; set; } = default!;
        public DbSet<Estagio> Estagios { get; set; } = default!;
        public DbSet<GestorEstagio> GestoresEstagio { get; set; } = default!;
        public DbSet<OrientadorEstagio> OrientadoresEstagio { get; set; } = default!;
        public DbSet<Avaliacao> Avaliacoes { get; set; } = default!;
        public DbSet<RelatorioEstagio> RelatoriosEstagio { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AcademicManagement.CourseStudyPlanYear>()
                .HasOne(item => item.Course)
                .WithMany()
                .HasForeignKey(item => item.CourseId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AcademicManagement.CourseStudyPlanYear>()
                .HasIndex(item => new
                {
                    item.CourseId,
                    item.StudyPlanId,
                    item.AcademicYearId
                })
                .IsUnique();

            modelBuilder.Entity<ServiceRequestType>()
                .HasIndex(type => type.Name)
                .IsUnique();

            modelBuilder.Entity<ServiceRequestStatus>()
                .HasIndex(status => status.Name)
                .IsUnique();

            modelBuilder.Entity<ServiceRequest>()
                .HasOne(request => request.Type)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ServiceRequest>()
                .HasOne(request => request.Status)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AcademicManagement.Student>()
                .HasIndex(student => student.NumStudent)
                .IsUnique();

            modelBuilder.Entity<AcademicManagement.Student>()
                .HasIndex(student => student.NIF)
                .IsUnique();

            modelBuilder.Entity<AcademicManagement.Student>()
                .HasIndex(student => student.Email)
                .IsUnique();

            modelBuilder.Entity<AcademicManagement.Student>()
                .HasIndex(student => student.Telephone)
                .IsUnique();

            modelBuilder.Entity<AcademicManagement.UC>()
                .HasIndex(uc => uc.Code)
                .IsUnique();

            modelBuilder.Entity<AcademicManagement.Course>()
                .HasIndex(course => course.Code)
                .IsUnique();

            modelBuilder.Entity<AccommodationManagement.Room>()
                .HasIndex(room => new { room.ResidenceId, room.Number })
                .IsUnique();
        }
    }
}