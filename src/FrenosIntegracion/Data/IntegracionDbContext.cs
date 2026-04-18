using FrenosIntegracion.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FrenosIntegracion.Data
{
    public class IntegracionDbContext : DbContext
    {
        // Constructor tradicional
        public IntegracionDbContext(DbContextOptions<IntegracionDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProductoCache> ProductosCache { get; set; }
        public DbSet<ServicioCache> ServiciosCache { get; set; }
        public DbSet<ColaPendiente> ColaPendiente { get; set; }
        public DbSet<LogPeticion> LogPeticiones { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // Buena práctica incluir la base

            builder.Entity<ColaPendiente>(e =>
            {
                e.HasIndex(c => c.IdLocal).IsUnique();
                e.HasIndex(c => c.Estado);
                e.HasIndex(c => c.ProximoIntento);
            });

            builder.Entity<LogPeticion>(e =>
            {
                e.HasIndex(l => l.FechaHora);
                e.HasIndex(l => l.StatusCode);
            });
        }
    }
}