using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace UnivManagement.Data {
    public class ApplicationUsersDbContext(DbContextOptions<ApplicationUsersDbContext> options) : IdentityDbContext(options) {
    }
}
