using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace UnivManagement.Data {
    public class ApplicationUsersDbContext(DbContextOptions<ApplicationUsersDbContext> options) : IdentityDbContext(options) {
    public DbSet<UnivManagement.Models.Docente> Docente { get; set; } = default!;
    }
}
