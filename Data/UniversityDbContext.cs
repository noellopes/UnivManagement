using Microsoft.EntityFrameworkCore;

namespace UnivManagement.Data {
    public class UniversityDbContext(DbContextOptions<UniversityDbContext> options) : DbContext(options) {
    public DbSet<UnivManagement.Models.Docente> Docente { get; set; } = default!;
    }
}
