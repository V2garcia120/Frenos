using FrenosCore.Modelos.Entidades;
using Microsoft.EntityFrameworkCore;


namespace FrenosCore.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuario => Set<Usuario>();
        public DbSet<Rol> Rol => Set<Rol>();
        public DbSet<Cliente> Cliente => Set<Cliente>();
        public DbSet<Vehiculo> Vehiculo => Set<Vehiculo>();
        public DbSet<Cotizacion> Cotizacion => Set<Cotizacion>();
        public DbSet<CotizacionItem> CotizacionItem => Set<CotizacionItem>();
        public DbSet<Orden> Orden => Set<Orden>();
        public DbSet<Diagnostico> Diagnostico => Set<Diagnostico>();
        public DbSet<DiagnosticoItem> DiagnosticoItem => Set<DiagnosticoItem>();
        public DbSet<Factura> Factura => Set<Factura>();
        public DbSet<AbonoCxC> AbonoCxC => Set<AbonoCxC>();
        public DbSet<CuentasPorCobrar> CuentasPorCobrar => Set<CuentasPorCobrar>();
        public DbSet<HistorialReparacion> HistorialReparacion => Set<HistorialReparacion>();
        public DbSet<FacturaItem> FacturaItem => Set<FacturaItem>();
        public DbSet<Servicio> Servicio => Set<Servicio>();
        public DbSet<Producto> Producto => Set<Producto>();
        public DbSet<AuditLog> AuditLog => Set<AuditLog>();

        public DbSet<TurnoCaja> TurnoCaja => Set<TurnoCaja>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Usuario>(e =>
            {
                e.ToTable("Usuario");
                e.HasKey(u => u.Id);

                e.Property(u => u.Nombre).IsRequired().HasMaxLength(150);
                e.Property(u => u.Email).IsRequired().HasMaxLength(200);
                e.Property(u => u.PasswordHash).IsRequired().HasMaxLength(500);
                e.Property(u => u.FechaCreacion).HasDefaultValueSql("GETDATE()");

                e.HasIndex(u => u.Email)
                 .IsUnique()
                 .HasDatabaseName("IX_Usuarios_Email");

                e.HasOne(u => u.Rol)
                 .WithMany(r => r.Usuarios)
                 .HasForeignKey(u => u.RolId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Rol>(e =>
            {
                e.ToTable("Rol");
                e.HasKey(r => r.Id);
                e.Property(r => r.Nombre).IsRequired().HasMaxLength(50);
                e.Property(r => r.Descripcion).HasMaxLength(300);
                e.HasIndex(r => r.Nombre).IsUnique();
            });

            builder.Entity<Cliente>(e =>
            {
                e.ToTable("Cliente");
                e.HasKey(c => c.Id);

                e.Property(c => c.Nombre).IsRequired().HasMaxLength(200);
                e.Property(c => c.Cedula).IsRequired().HasMaxLength(20);
                e.Property(c => c.Telefono).IsRequired().HasMaxLength(20);
                e.Property(c => c.Email).HasMaxLength(200);
                e.Property(c => c.Direccion).HasMaxLength(300);
                e.Property(c => c.EsAnonimo);
                e.Property(c => c.CreadoEn).HasDefaultValueSql("GETDATE()");

                e.HasIndex(c => c.Cedula).IsUnique();

                e.HasMany(c => c.Vehiculos)
                 .WithOne(v => v.Cliente)
                 .HasForeignKey(v => v.ClienteId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasMany(c => c.Ordenes)
                 .WithOne(o => o.Cliente)
                 .HasForeignKey(o => o.ClienteId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasMany(c => c.Cotizaciones)
                 .WithOne(cz => cz.Cliente)
                 .HasForeignKey(cz => cz.ClienteId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasMany(c => c.Facturas)
                 .WithOne(f => f.Cliente)
                 .HasForeignKey(f => f.ClienteId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasMany(c => c.CuentasPorCorbrar)
                 .WithOne(cx => cx.Cliente)
                 .HasForeignKey(cx => cx.ClienteId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Vehiculo>(e =>
            {
                e.ToTable("Vehiculo");
                e.HasKey(v => v.Id);

                e.Property(v => v.Placa).IsRequired().HasMaxLength(15);
                e.Property(v => v.Marca).IsRequired().HasMaxLength(80);
                e.Property(v => v.Modelo).IsRequired().HasMaxLength(80);
                e.Property(v => v.Color).HasMaxLength(40);
                e.Property(v => v.VIN).HasMaxLength(17);
                e.Property(v => v.TipoCombustible).HasMaxLength(20);
                e.Property(v => v.Nota).HasMaxLength(500);
                e.Property(v => v.FechaCreacion).HasDefaultValueSql("GETDATE()");

                e.HasIndex(v => v.Placa).IsUnique();
            });

            builder.Entity<Cotizacion>(e =>
            {
                e.ToTable("Cotizacion");
                e.HasKey(c => c.Id);

                e.Property(c => c.Estado).IsRequired().HasMaxLength(30);
                e.Property(c => c.Notas).HasMaxLength(500);
                e.Property(c => c.Subtotal).HasPrecision(10, 2);
                e.Property(c => c.Itbis).HasPrecision(10, 2);
                e.Property(c => c.Total).HasPrecision(10, 2);
            });

            builder.Entity<CotizacionItem>(e =>
            {
                e.ToTable("CotizacionItem");
                e.HasKey(ci => ci.Id);

                e.Property(ci => ci.Tipo).IsRequired().HasMaxLength(20);
                e.Property(ci => ci.Descripcion).IsRequired().HasMaxLength(250);
                e.Property(ci => ci.PrecioUnitario).HasPrecision(18, 2);
                e.Property(ci => ci.Subtotal).HasPrecision(18, 2);

                e.HasOne(ci => ci.Cotizacion)
                 .WithMany(c => c.Items)
                 .HasForeignKey(ci => ci.CotizacionId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Orden>(e =>
            {
                e.ToTable("Orden");
                e.HasKey(o => o.Id);

                e.Property(o => o.Estado).IsRequired().HasMaxLength(30);
                e.Property(o => o.Prioridad).IsRequired().HasMaxLength(20);
                e.Property(o => o.Notas).HasMaxLength(500);

                e.HasOne(o => o.Vehiculo)
                 .WithMany(v => v.Ordenes)
                 .HasForeignKey(o => o.VehiculoId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(o => o.TecnicoAsignado)
                 .WithMany(u => u.OrdenesAsignadas)
                 .HasForeignKey(o => o.TecnicoId)
                 .OnDelete(DeleteBehavior.SetNull);

                e.HasOne(o => o.Cotizacion)
                 .WithOne(c => c.Orden)
                 .HasForeignKey<Orden>(o => o.CotizacionId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Diagnostico>(e =>
            {
                e.ToTable("Diagnostico");
                e.HasKey(d => d.Id);

                e.Property(d => d.DescripcionGeneral);
                e.Property(d => d.Estado).IsRequired().HasMaxLength(20);
                e.Property(d => d.ObservacionesTecnico);
                e.Property(d => d.FechaDiagnostico).HasDefaultValueSql("GETDATE()");

                e.HasOne(d => d.Orden)
                 .WithOne(o => o.Diagnostico)
                 .HasForeignKey<Diagnostico>(d => d.OrdenId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(d => d.Tecnico)
                 .WithMany(u => u.DiagnosticosAsignados)
                 .HasForeignKey(d => d.TecnicoId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<DiagnosticoItem>(e =>
            {
                e.ToTable("DiagnosticoItem");
                e.HasKey(di => di.Id);

                e.Property(di => di.SistemaVehiculo).IsRequired().HasMaxLength(80);
                e.Property(di => di.Componente).IsRequired().HasMaxLength(100);
                e.Property(di => di.Condicion).IsRequired().HasMaxLength(20);
                e.Property(di => di.AccionRecomendada).IsRequired().HasMaxLength(20);
                e.Property(di => di.Descripcion);

                e.HasOne(di => di.Diagnostico)
                 .WithMany(d => d.Items)
                 .HasForeignKey(di => di.DiagnosticoId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(di => di.ServicioSugerido)
                 .WithMany(s => s.DiagnosticoItemsSugeridos)
                 .HasForeignKey(di => di.ServicioSugeridoId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(di => di.ProductoSugerido)
                 .WithMany(p => p.DiagnosticoItemsSugeridos)
                 .HasForeignKey(di => di.ProductoSugeridoId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Factura>(e =>
            {
                e.ToTable("Factura");
                e.HasKey(f => f.Id);

                e.Property(f => f.Numero).IsRequired().HasMaxLength(30);
                e.Property(f => f.Subtotal).HasPrecision(10, 2);
                e.Property(f => f.Itbis).HasPrecision(10, 2);
                e.Property(f => f.Total).HasPrecision(10, 2);
                e.Property(f => f.TipoOrigen).IsRequired().HasMaxLength(30);
                e.Property(f => f.Estado).IsRequired().HasMaxLength(20);
                e.Property(f => f.MetodoPago).HasMaxLength(40);

                e.HasIndex(f => f.Numero).IsUnique();

                e.HasOne(f => f.Orden)
                 .WithOne(o => o.Factura)
                 .HasForeignKey<Factura>(f => f.OrdenId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(f => f.Turno)
                 .WithMany(t => t.Facturas)
                 .HasForeignKey(f => f.TurnoId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(f => f.EmitidaPorUsuario)
                 .WithMany(u => u.FacturasEmitidas)
                 .HasForeignKey(f => f.EmitidaPor)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<FacturaItem>(e =>
            {
                e.ToTable("FacturaItem");
                e.HasKey(fi => fi.Id);

                e.Property(fi => fi.Tipo).IsRequired().HasMaxLength(20);
                e.Property(fi => fi.Descripcion).IsRequired().HasMaxLength(200);
                e.Property(fi => fi.PrecioUnitario).HasPrecision(10, 2);
                e.Property(fi => fi.Subtotal).HasPrecision(10, 2);

                e.HasOne(fi => fi.Factura)
                 .WithMany(f => f.Items)
                 .HasForeignKey(fi => fi.FacturaId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<CuentasPorCobrar>(e =>
            {
                e.ToTable("CuentasPorCorbrar");
                e.HasKey(cx => cx.Id);

                e.Property(cx => cx.Monto).IsRequired().HasPrecision(10, 2);
                e.Property(cx => cx.Saldo).IsRequired().HasPrecision(10, 2);
                e.Property(cx => cx.Estado).IsRequired().HasMaxLength(50);
                e.Property(cx => cx.CreadoEn).HasDefaultValueSql("GETDATE()");

                e.HasIndex(cx => cx.FacturaId).IsUnique();

                e.HasOne(cx => cx.Factura)
                 .WithOne(f => f.CuentasPorCorbrar)
                 .HasForeignKey<CuentasPorCobrar>(cx => cx.FacturaId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<AbonoCxC>(e =>
            {
                e.ToTable("AbonoCxC");
                e.HasKey(a => a.Id);

                e.Property(a => a.Monto).IsRequired().HasPrecision(10, 2);
                e.Property(a => a.MetodoPago).IsRequired().HasMaxLength(40);

                e.HasOne(a => a.CxC)
                 .WithMany(cx => cx.Abonos)
                 .HasForeignKey(a => a.CxCId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(a => a.RegistradoPorUsuario)
                 .WithMany(u => u.AbonosRegistrados)
                 .HasForeignKey(a => a.RegistradoPor)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<HistorialReparacion>(e =>
            {
                e.ToTable("HistorialReparaciones");
                e.HasKey(h => h.Id);

                e.Property(h => h.TrabajosRealizados).IsRequired();

                e.HasOne(h => h.Vehiculo)
                 .WithMany(v => v.HistorialReparacion)
                 .HasForeignKey(h => h.VehiculoId)
                 .OnDelete(DeleteBehavior.Restrict);


                e.HasOne(h => h.Tecnico)
                 .WithMany(u => u.HistorialesReparacionTecnico)
                 .HasForeignKey(h => h.TecnicoId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Servicio>(e =>
            {
                e.ToTable("Servicio");
                e.HasKey(s => s.Id);

                e.Property(s => s.Nombre).IsRequired().HasMaxLength(150);
                e.Property(s => s.Descripcion).HasMaxLength(500);
                e.Property(s => s.Precio).HasPrecision(18, 2);
                e.Property(s => s.Categoria).HasMaxLength(100);
            });

            builder.Entity<Producto>(e =>
            {
                e.ToTable("Producto");
                e.HasKey(p => p.Id);

                e.Property(p => p.Nombre).IsRequired().HasMaxLength(150);
                e.Property(p => p.Descripcion).HasMaxLength(500);
                e.Property(p => p.Precio).HasPrecision(18, 2);
                e.Property(p => p.Costo).HasPrecision(18, 2);
                e.Property(p => p.Categoria).HasMaxLength(100);
                e.Property(p => p.CreadoEn).HasDefaultValueSql("GETDATE()");
            });

            builder.Entity<AuditLog>(e =>
            {
                e.ToTable("AuditLog");
                e.HasKey(a => a.id);

                e.Property(a => a.Accion).IsRequired().HasMaxLength(100);
                e.Property(a => a.Tabla).IsRequired().HasMaxLength(100);
                e.Property(a => a.RegistroId).HasMaxLength(100);
                e.Property(a => a.ValorAntes).HasMaxLength(4000);
                e.Property(a => a.ValorDespues).HasMaxLength(4000);
                e.Property(a => a.Ip).HasMaxLength(50);
                e.Property(a => a.FechaHora).HasDefaultValueSql("GETDATE()");
            });

            base.OnModelCreating(builder);
        }
    }
}

