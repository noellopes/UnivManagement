using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using Models = global::UnivManagement.Models;

namespace UnivManagement.Data
{
    public class UniversityDbContext(
        DbContextOptions<UniversityDbContext> options) : DbContext(options)
    {
        public DbSet<Convidado> Convidado { get; set; } = default!;

        public DbSet<Models.ServiceRequestType> ServiceRequestTypes { get; set; } = default!;
        public DbSet<Models.ServiceRequestStatus> ServiceRequestStatuses { get; set; } = default!;
        public DbSet<Models.ServiceRequest> ServiceRequests { get; set; } = default!;

        public DbSet<Models.AcademicManagement.Student> Student { get; set; } = default!;
        public DbSet<Models.AcademicManagement.AcademicYear> AcademicYear { get; set; } = default!;
        public DbSet<Models.AcademicManagement.UC> UC { get; set; } = default!;
        public DbSet<Models.AcademicManagement.SchoolCalendar> SchoolCalendar { get; set; } = default!;
        public DbSet<Models.AcademicManagement.Course> Course { get; set; } = default!;
        public DbSet<Models.AcademicManagement.InscrUC> InscrUC { get; set; } = default!;
        public DbSet<Models.AcademicManagement.InscrCourse> InscrCourse { get; set; } = default!;
        public DbSet<Models.AcademicManagement.StudyPlan> StudyPlan { get; set; } = default!;
        public DbSet<Models.AcademicManagement.StudyPlanUC> StudyPlanUC { get; set; } = default!;
        public DbSet<Models.AcademicManagement.CourseStudyPlanYear> CourseStudyPlanYear { get; set; } = default!;

        public DbSet<Refeicao> Refeicoes { get; set; } = default!;
        public DbSet<TipoRefeicao> TiposRefeicao { get; set; } = default!;

        public DbSet<Models.AccommodationManagement.StayRecord> StayRecord { get; set; } = default!;
        public DbSet<Models.AccommodationManagement.AccommodationPayment> AccommodationPayment { get; set; } = default!;
        public DbSet<Models.AccommodationManagement.AccommodationContract> AccommodationContract { get; set; } = default!;
        public DbSet<Models.AccommodationManagement.ApplicationStatusHistory> ApplicationStatusHistory { get; set; } = default!;
        public DbSet<Models.AccommodationManagement.AccommodationApplication> AccommodationApplication { get; set; } = default!;
        public DbSet<Models.AccommodationManagement.Room> Room { get; set; } = default!;

        public DbSet<Models.ServicoFuncionario> ServicoFuncionario { get; set; } = default!;

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

        public DbSet<TipoCandidatura> TipoCandidatura { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Models.AcademicManagement.CourseStudyPlanYear>()
    .HasOne(cspy => cspy.Course)
    .WithMany()
    .HasForeignKey(cspy => cspy.CourseId)
    .OnDelete(DeleteBehavior.NoAction);

            // ServiceRequestType: two request types cannot have the same name
            modelBuilder.Entity<Models.ServiceRequestType>()
                .HasIndex(type => type.Name)
                .IsUnique();

            modelBuilder.Entity<Models.ServiceRequestStatus>()
                .HasIndex(status => status.Name)
                .IsUnique();

            modelBuilder.Entity<Models.ServiceRequest>()
                .HasOne(request => request.Type)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.ServiceRequest>()
                .HasOne(request => request.Status)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.AcademicManagement.Student>()
                .HasIndex(student => student.NumStudent)
                .IsUnique();

            modelBuilder.Entity<Models.AcademicManagement.Student>()
                .HasIndex(student => student.NIF)
                .IsUnique();

            modelBuilder.Entity<Models.AcademicManagement.Student>()
                .HasIndex(student => student.Email)
                .IsUnique();

            modelBuilder.Entity<Models.AcademicManagement.Student>()
                .HasIndex(student => student.Telephone)
                .IsUnique();

            modelBuilder.Entity<Models.AcademicManagement.UC>()
                .HasIndex(uc => uc.Code)
                .IsUnique();

            modelBuilder.Entity<Models.AccommodationManagement.Room>()
                .HasIndex(room => new { room.ResidenceId, room.Number })
                .IsUnique();

            modelBuilder.Entity<Models.AcademicManagement.Course>()
                .HasIndex(course => course.Code)
                .IsUnique();

            modelBuilder.Entity<Models.AcademicManagement.CourseStudyPlanYear>()
                .HasIndex(item => new
                {
                    item.CourseId,
                    item.StudyPlanId,
                    item.AcademicYearId
                })
                .IsUnique();

            // Avoid SQL Server multiple cascade paths.
            modelBuilder.Entity<Models.AcademicManagement.CourseStudyPlanYear>()
                .HasOne(item => item.Course)
                .WithMany()
                .HasForeignKey(item => item.CourseId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}