using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;

namespace UnivManagement.Data
{
    public class UniversityDbContext(
        DbContextOptions<UniversityDbContext> options) : DbContext(options)
    {
        public DbSet<TipoProgramaBolsa> TiposProgramaBolsa { get; set; }

        public DbSet<ProgramaBolsa> ProgramasBolsa { get; set; }

        public DbSet<TipoCandidatura> TiposCandidatura { get; set; }

        public DbSet<Candidatura> Candidaturas { get; set; }
        public DbSet<CriterioElegibilidade> CriteriosElegibilidade { get; set; }

        public DbSet<Atribuicao> Atribuicoes { get; set; }

        public DbSet<Pagamento> Pagamentos { get; set; }
    }
}
