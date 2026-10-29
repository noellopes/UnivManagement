using Microsoft.EntityFrameworkCore;

namespace UnivManagement.Data {
    public class UniversityDbContext(DbContextOptions<UniversityDbContext> options) : DbContext(options) {
        public DbSet<UnivManagement.Models.AccommodationManagement.AccommodationPayment> AccommodationPayment { get; set; } = default!;
    }
}
