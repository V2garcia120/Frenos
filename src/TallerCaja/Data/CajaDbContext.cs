using Microsoft.EntityFrameworkCore;
using TallerCaja.Helpers;
using TallerCaja.Models.Entities;

namespace TallerCaja.Data
{
    public class CajaDbContext : DbContext
    {
        public DbSet<ProductoLocal> Productos { get; set; }
        public DbSet<ServicioLocal> Servicios { get; set; }
        public DbSet<TurnoLocal> Turnos { get; set; }
        public DbSet<TransaccionPendiente> TransaccionesPendientes { get; set; }
        public DbSet<VentaLocal> VentasLocales { get; set; }
        public DbSet<UsuariosLocal> Usuarios { get; set; }
        public DbSet<ClienteLocal> Clientes { get; set; }
        public DbSet<FacturasPendienteLocal> FacturasPendientes { get; set; }
       

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = AppConfig.LocalDbPath;
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductoLocal>().HasKey(p => p.Id);
            modelBuilder.Entity<ServicioLocal>().HasKey(s => s.Id);
            modelBuilder.Entity<TurnoLocal>().HasKey(t => t.Id);
            modelBuilder.Entity<TransaccionPendiente>().HasKey(t => t.Id);
            modelBuilder.Entity<VentaLocal>().HasKey(v => v.Id);
            modelBuilder.Entity<UsuariosLocal>().HasKey(u => u.Id);
            modelBuilder.Entity<ClienteLocal>().HasKey(c => c.Id);
            modelBuilder.Entity<FacturasPendienteLocal>().HasKey(f => f.Id);
            modelBuilder.Entity<CxCLocal>().HasKey(c => c.Id);

            modelBuilder.Entity<ProductoLocal>()
                .Property(p => p.Precio)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<FacturasPendienteLocal>()
                .Property(f => f.Total)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<CxCLocal>()
                .Property(c => c.Saldo)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<TurnoLocal>()
                .Property(t => t.MontoInicial)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<VentaLocal>()
                .Property(v => v.Subtotal).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<VentaLocal>()
                .Property(v => v.ITBIS).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<VentaLocal>()
                .Property(v => v.Total).HasColumnType("decimal(18,2)");
        }
    }
}
