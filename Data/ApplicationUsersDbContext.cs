using Microsoft.EntityFrameworkCore;

namespace UnivManagement.Data {
    public class ApplicationUsersDbContext(DbContextOptions<ApplicationUsersDbContext> options) : IdentityDbContext(options) {
    }
}
