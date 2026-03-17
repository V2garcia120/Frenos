using BCrypt.Net;
using FrenosCore.Modelos.Entidades;
using Microsoft.EntityFrameworkCore;

namespace FrenosCore.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await context.Database.MigrateAsync();

            if (!await context.Rol.AnyAsync())
            {
                context.Rol.AddRange(
                    new Rol { Nombre = "Administrador", Descripcion = "Acceso completo al sistema" },
                    new Rol { Nombre = "Tecnico", Descripcion = "Gestiona diagnósticos y reparaciones" },
                    new Rol { Nombre = "Caja", Descripcion = "Gestiona cobros y cierres de caja" }
                );

                await context.SaveChangesAsync();
            }

            if (!await context.Usuario.AnyAsync())
            {
                var rolAdministradorId = await context.Rol
                    .Where(r => r.Nombre == "Administrador")
                    .Select(r => r.Id)
                    .FirstAsync();

                context.Usuario.Add(new Usuario
                {
                    Nombre = "Administrador",
                    Email = "admin@frenos.local",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123*"),
                    RolId = rolAdministradorId,
                    Activo = true,
                    FechaCreacion = DateTime.Now
                });

                await context.SaveChangesAsync();
            }

            if (!await context.Servicio.AnyAsync())
            {
                context.Servicio.AddRange(
                    new Servicio
                    {
                        Nombre = "Cambio de bandas delanteras",
                        Descripcion = "Sustitución de bandas y ajuste del sistema de frenos delanteros",
                        Precio = 1800m,
                        DuracionMinutos = 90,
                        Categoria = "Frenos",
                        Activo = true
                    },
                    new Servicio
                    {
                        Nombre = "Rectificado de discos",
                        Descripcion = "Rectificado de discos para mejorar frenado y reducir vibraciones",
                        Precio = 2500m,
                        DuracionMinutos = 120,
                        Categoria = "Frenos",
                        Activo = true
                    },
                    new Servicio
                    {
                        Nombre = "Mantenimiento general",
                        Descripcion = "Revisión general del sistema de frenos y componentes relacionados",
                        Precio = 1500m,
                        DuracionMinutos = 60,
                        Categoria = "Mantenimiento",
                        Activo = true
                    }
                );

                await context.SaveChangesAsync();
            }

            if (!await context.Producto.AnyAsync())
            {
                context.Producto.AddRange(
                    new Producto
                    {
                        Nombre = "Bandas cerámicas delanteras",
                        Descripcion = "Juego de bandas cerámicas para vehículos livianos",
                        Precio = 3200m,
                        Costo = 2300m,
                        Stock = 25,
                        StockMinimo = 8,
                        Categoria = "Repuestos",
                        Activo = true,
                        CreadoEn = DateTime.Now
                    },
                    new Producto
                    {
                        Nombre = "Líquido de frenos DOT 4",
                        Descripcion = "Líquido de frenos de alto rendimiento 355ml",
                        Precio = 650m,
                        Costo = 420m,
                        Stock = 40,
                        StockMinimo = 12,
                        Categoria = "Fluidos",
                        Activo = true,
                        CreadoEn = DateTime.Now
                    },
                    new Producto
                    {
                        Nombre = "Disco de freno ventilado",
                        Descripcion = "Disco de freno ventilado para eje delantero",
                        Precio = 4200m,
                        Costo = 3000m,
                        Stock = 15,
                        StockMinimo = 5,
                        Categoria = "Repuestos",
                        Activo = true,
                        CreadoEn = DateTime.Now
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
