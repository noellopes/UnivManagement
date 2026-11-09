using Microsoft.EntityFrameworkCore;

namespace UnivManagement.Data {
    public class UniversityDbContext(DbContextOptions<UniversityDbContext> options) : DbContext(options) {
    }
}
