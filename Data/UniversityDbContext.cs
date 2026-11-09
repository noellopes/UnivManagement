using Microsoft.EntityFrameworkCore;

namespace UnivManagement.Data {
    public class UniversityDbContext(DbContextOptions<UniversityDbContext> options) : DbContext(options) {
    public DbSet<UnivManagement.Models.Teacher> Teacher { get; set; } = default!;
    public DbSet<UnivManagement.Models.ApplicationUser> ApplicationUser { get; set; } = default!;
    }
}
