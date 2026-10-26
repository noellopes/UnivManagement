using Microsoft.EntityFrameworkCore;

namespace UnivManagement.Data {
    public class UniversityDbContext(DbContextOptions<UniversityDbContext> options) : DbContext(options) {
        public DbSet<UnivManagement.Models.Student> Student { get; set; } = default!;
        public DbSet<UnivManagement.Models.AcademicYear> AcademicYear { get; set; } = default!;
    }
}
