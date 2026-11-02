using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;

namespace UnivManagement.Data
{
    public class UniversityDbContext(DbContextOptions<UniversityDbContext> options) : DbContext(options)
    {
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
