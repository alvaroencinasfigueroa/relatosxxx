using Microsoft.EntityFrameworkCore;
using Relatosxxx.Models;

namespace Relatosxxx.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Relato> Relatos { get; set; }
    }
}