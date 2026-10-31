using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;

namespace UnivManagement.Data {
    public class UniversityDbContext(DbContextOptions<UniversityDbContext> options) : DbContext(options) {
    public DbSet<UnivManagement.Models.Docente> Docente { get; set; } = default!;
        public DbSet<Refeicao> Refeicoes { get; set; }
    }
}
