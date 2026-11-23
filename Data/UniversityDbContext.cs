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

            // Room: número único por residência:
            modelBuilder.Entity<Models.AccommodationManagement.Room>()
                .HasIndex(r => new { r.ResidenceId, r.Number })
                .IsUnique();

            ConfigureAcademicManagement(modelBuilder);
        }

        private static void ConfigureAcademicManagement(ModelBuilder modelBuilder) {
            // --- Unique constraints ---
            modelBuilder.Entity<Models.AcademicManagement.Student>()
                .HasIndex(s => s.NumStudent)
                .IsUnique();

            modelBuilder.Entity<Models.AcademicManagement.AcademicDegree>()
                .HasIndex(a => a.Name)
                .IsUnique();

            modelBuilder.Entity<Models.AcademicManagement.CourseState>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<Models.AcademicManagement.StateInscrUC>()
                .HasIndex(s => s.Name)
                .IsUnique();

            modelBuilder.Entity<Models.AcademicManagement.StateEnrollment>()
                .HasIndex(s => s.Name)
                .IsUnique();

            modelBuilder.Entity<Models.AcademicManagement.AcademicYear>()
                .HasIndex(a => new { a.StartYear, a.EndYear })
                .IsUnique();

            modelBuilder.Entity<Models.AcademicManagement.Course>()
                .HasIndex(c => c.Code)
                .IsUnique();

            modelBuilder.Entity<Models.AcademicManagement.UC>()
                .HasIndex(u => u.Code)
                .IsUnique();

            modelBuilder.Entity<Models.AcademicManagement.StudyPlan>()
                .HasIndex(s => s.Code)
                .IsUnique();

            modelBuilder.Entity<Models.AcademicManagement.StudyPlanYear>()
                .HasIndex(y => new { y.StudyPlanId, y.NumYear })
                .IsUnique();

            modelBuilder.Entity<Models.AcademicManagement.Semester>()
                .HasIndex(s => new { s.SchoolCalendarId, s.OrderNum })
                .IsUnique();

            // Enforce the consecutive academic-year interval in the database where supported.
            modelBuilder.Entity<Models.AcademicManagement.AcademicYear>()
                .ToTable(t => t.HasCheckConstraint(
                    "CK_AcademicYear_ConsecutiveYears",
                    "[EndYear] = [StartYear] + 1"));

            // --- Reference relationships ---
            modelBuilder.Entity<Models.AcademicManagement.Course>()
                .HasOne(c => c.AcademicDegree)
                .WithMany()
                .HasForeignKey(c => c.AcademicDegreeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.AcademicManagement.Course>()
                .HasOne(c => c.CourseState)
                .WithMany()
                .HasForeignKey(c => c.CourseStateId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.AcademicManagement.InscrUC>()
                .HasOne(i => i.StateInscrUC)
                .WithMany()
                .HasForeignKey(i => i.StateInscrUCId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.AcademicManagement.Enrollment>()
                .HasOne(e => e.StateEnrollment)
                .WithMany()
                .HasForeignKey(e => e.StateEnrollmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- Course and study plan structure ---
            modelBuilder.Entity<Models.AcademicManagement.StudyPlan>()
                .HasOne(s => s.Course)
                .WithMany()
                .HasForeignKey(s => s.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.AcademicManagement.CourseCalendar>()
                .HasOne(cc => cc.Course)
                .WithMany()
                .HasForeignKey(cc => cc.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.AcademicManagement.StudyPlanYear>()
                .HasOne(y => y.StudyPlan)
                .WithMany()
                .HasForeignKey(y => y.StudyPlanId)
                .OnDelete(DeleteBehavior.Cascade);

            // UC is independent: deleting a StudyPlan must not delete its UCs.
            modelBuilder.Entity<Models.AcademicManagement.UC>()
                .HasOne(u => u.StudyPlan)
                .WithMany()
                .HasForeignKey(u => u.StudyPlanId)
                .OnDelete(DeleteBehavior.Restrict);

            // Course director belongs to another domain.
            modelBuilder.Entity<Models.AcademicManagement.Course>()
                .HasOne(c => c.Docente)
                .WithMany()
                .HasForeignKey(c => c.DocenteId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- Academic calendar ---
            modelBuilder.Entity<Models.AcademicManagement.SchoolCalendar>()
                .HasOne(s => s.AcademicYear)
                .WithMany()
                .HasForeignKey(s => s.AcademicYearId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.AcademicManagement.Semester>()
                .HasOne(s => s.SchoolCalendar)
                .WithMany()
                .HasForeignKey(s => s.SchoolCalendarId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Models.AcademicManagement.CourseCalendar>()
                .HasOne(cc => cc.SchoolCalendar)
                .WithMany()
                .HasForeignKey(cc => cc.SchoolCalendarId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- Enrollment and progression history ---
            modelBuilder.Entity<Models.AcademicManagement.Enrollment>()
                .HasOne(e => e.Student)
                .WithMany()
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.AcademicManagement.Enrollment>()
                .HasOne(e => e.StudyPlan)
                .WithMany()
                .HasForeignKey(e => e.StudyPlanId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.AcademicManagement.InscrUC>()
                .HasOne(i => i.Enrollment)
                .WithMany()
                .HasForeignKey(i => i.EnrollmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Models.AcademicManagement.EnrollmentYear>()
                .HasOne(y => y.Enrollment)
                .WithMany()
                .HasForeignKey(y => y.EnrollmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Models.AcademicManagement.InscrUC>()
                .HasOne(i => i.UC)
                .WithMany()
                .HasForeignKey(i => i.UcId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.AcademicManagement.EnrollmentYear>()
                .HasOne(y => y.AcademicYear)
                .WithMany()
                .HasForeignKey(y => y.AcademicYearId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        // --- Academic Management ---
        public DbSet<UnivManagement.Models.AcademicManagement.Student> Student { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.AcademicDegree> AcademicDegree { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.CourseState> CourseState { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.Course> Course { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.StudyPlan> StudyPlan { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.StudyPlanYear> StudyPlanYear { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.UC> UC { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.StateInscrUC> StateInscrUC { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.InscrUC> InscrUC { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.AcademicYear> AcademicYear { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.SchoolCalendar> SchoolCalendar { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.CourseCalendar> CourseCalendar { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.Semester> Semester { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.StateEnrollment> StateEnrollment { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.Enrollment> Enrollment { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.EnrollmentYear> EnrollmentYear { get; set; } = default!;

        // --- Other domains ---
        public DbSet<UnivManagement.Models.ServiceRequestType> ServiceRequestTypes { get; set; } = default!;
        public DbSet<UnivManagement.Models.ServiceRequestStatus> ServiceRequestStatuses { get; set; } = default!;
        public DbSet<UnivManagement.Models.ServiceRequest> ServiceRequests { get; set; } = default!;
        public DbSet<Refeicao> Refeicoes { get; set; }
        public DbSet<TipoRefeicao> TiposRefeicao { get; set; } = default!;
        public DbSet<UnivManagement.Models.AccommodationManagement.StayRecord> StayRecord { get; set; } = default!;
        public DbSet<UnivManagement.Models.AccommodationManagement.AccommodationPayment> AccommodationPayment { get; set; } = default!;
        public DbSet<UnivManagement.Models.AccommodationManagement.AccommodationContract> AccommodationContract { get; set; } = default!;
        public DbSet<UnivManagement.Models.AccommodationManagement.ApplicationStatusHistory> ApplicationStatusHistory { get; set; } = default!;
        public DbSet<UnivManagement.Models.AccommodationManagement.AccommodationApplication> AccommodationApplication { get; set; } = default!;
        public DbSet<UnivManagement.Models.AccommodationManagement.Room> Room { get; set; } = default!;
        public DbSet<UnivManagement.Models.ServicoFuncionario> ServicoFuncionario { get; set; } = default!;

        public DbSet<AntigoAluno> AntigosAlunos { get; set; }
        public DbSet<Carreira> Carreiras { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Empregador> Empregadores { get; set; }
        public DbSet<Oferta> Ofertas { get; set; }
        public DbSet<Candidatura> Candidaturas { get; set; }
        public DbSet<Estagio> Estagios { get; set; }
        public DbSet<GestorEstagio> GestoresEstagio { get; set; }
        public DbSet<OrientadorEstagio> OrientadoresEstagio { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<RelatorioEstagio> RelatoriosEstagio { get; set; }
    }
}
