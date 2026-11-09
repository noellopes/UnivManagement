using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;

namespace UnivManagement.Data
{
    public class UniversityDbContext(DbContextOptions<UniversityDbContext> options) : DbContext(options)
    {
        public DbSet<Room> Room{ get; set; } = default!;
        public DbSet<Building> Building { get; set; } = default!;
        public DbSet<RoomType> RoomType { get; set; } = default!;
    }
}