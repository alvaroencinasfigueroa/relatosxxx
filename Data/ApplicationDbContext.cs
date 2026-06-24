using Microsoft.EntityFrameworkCore;
using Relatosxxx.Models;

namespace Relatosxxx.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Relato> Relatos { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<ImagenDecorativa> ImagenDecorativas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Usuario: email único + índice
            modelBuilder.Entity<Usuario>(e =>
            {
                e.HasIndex(u => u.Email).IsUnique();
                e.Property(u => u.Email).HasMaxLength(256);
                e.Property(u => u.Nombre).HasMaxLength(100);
            });

            // Relato: índice por categoría (para filtrar rápido) y por fecha
            modelBuilder.Entity<Relato>(e =>
            {
                e.HasIndex(r => r.Categoria);
                e.HasIndex(r => r.FechaCreacion);
                e.Property(r => r.Titulo).HasMaxLength(300);
                e.Property(r => r.Categoria).HasMaxLength(50);
            });

            // Pago: índice único en Identificador+Metodo (evita doble procesamiento)
            modelBuilder.Entity<Pago>(e =>
            {
                e.HasIndex(p => new { p.Identificador, p.Metodo }).IsUnique();
                e.HasOne(p => p.Usuario)
                 .WithMany(u => u.Pagos)
                 .HasForeignKey(p => p.UsuarioId)
                 .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}