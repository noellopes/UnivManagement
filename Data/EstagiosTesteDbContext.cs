using Microsoft.EntityFrameworkCore;
using UnivManagement.Models.TestesEstagios;

namespace UnivManagement.Data
{
    public class EstagiosTesteDbContext : DbContext
    {
        public EstagiosTesteDbContext(
            DbContextOptions<EstagiosTesteDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Student { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<OrientadorEstagio> OrientadoresEstagio { get; set; }
        public DbSet<Estagio> Estagios { get; set; }
    }
}