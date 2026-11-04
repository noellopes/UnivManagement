using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;

namespace UnivManagement.Data {
    public class UniversityDbContext(DbContextOptions<UniversityDbContext> options) : DbContext(options) {

        public DbSet<TipoBolsa> TipoBolsa { get; set; } = default!;

        public DbSet<EstadoCandidatura> EstadoCandidatura { get; set; } = default!;
    }
}
