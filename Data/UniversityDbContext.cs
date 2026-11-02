using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;

namespace UnivManagement.Data
{
    public class UniversityDbContext(DbContextOptions<UniversityDbContext> options) : DbContext(options)
    {
        public DbSet<Catalogo> Catalogos { get; set; }
        public DbSet<Exemplar> Exemplares { get; set; }
    }
}