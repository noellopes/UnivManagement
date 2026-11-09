using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;

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
        public DbSet<Refeicao> Refeicoes { get; set; }
        public DbSet<TipoRefeicao> TiposRefeicao { get; set; } = default!;
        public DbSet<UnivManagement.Models.AccommodationManagement.StayRecord> StayRecord { get; set; } = default!;
        public DbSet<UnivManagement.Models.AccommodationManagement.AccommodationPayment> AccommodationPayment { get; set; } = default!;
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
        public DbSet<UnivManagement.Models.ServicoFuncionario> ServicoFuncionario { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicManagement.CourseStudyPlanYear> CourseStudyPlanYear { get; set; } = default!;
        
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
    public DbSet<UnivManagement.Models.Session> ClassSession { get; set; } = default!;
    public DbSet<UnivManagement.Models.Group> ClassGroup { get; set; } = default!;
    }
}
