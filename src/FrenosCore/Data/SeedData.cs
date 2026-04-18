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

            var rolMantemineto = await context.Rol.FirstOrDefaultAsync(r => r.Nombre == "Mantemineto");
            var rolMantenimientoExistente = await context.Rol.FirstOrDefaultAsync(r => r.Nombre == "Mantenimiento");
            if (rolMantemineto is not null && rolMantenimientoExistente is null)
            {
                rolMantemineto.Nombre = "Mantenimiento";
                rolMantemineto.Descripcion = "Gestiona servicios y productos";
                await context.SaveChangesAsync();
            }
            else if (rolMantemineto is not null && rolMantenimientoExistente is not null)
            {
                var usuariosConRolErroneo = await context.Usuario
                    .Where(u => u.RolId == rolMantemineto.Id)
                    .ToListAsync();

                foreach (var usuario in usuariosConRolErroneo)
                    usuario.RolId = rolMantenimientoExistente.Id;

                context.Rol.Remove(rolMantemineto);
                await context.SaveChangesAsync();
            }

            await AsegurarRolAsync(context, "Administrador", "Acceso completo al sistema");
            await AsegurarRolAsync(context, "Tecnico", "Gestiona diagnósticos y reparaciones");
            await AsegurarRolAsync(context, "Caja", "Gestiona cobros y cierres de caja");
            await AsegurarRolAsync(context, "Mantenimiento", "Gestiona servicios y productos");

            var existeClienteAnonimo = await context.Cliente.AnyAsync(c => c.EsAnonimo);
            if (!existeClienteAnonimo)
            {
                var clienteAnonimo = new Cliente
                {
                    Nombre = "Cliente Anónimo",
                    Cedula = "00000000000",
                    Telefono = "000-000-0000",
                    Email = "anonimo@frenos.local",
                    Direccion = "N/A",
                    CreadoEn = DateTime.Now
                };

                context.Cliente.Add(clienteAnonimo);
                context.Entry(clienteAnonimo).Property(c => c.EsAnonimo).CurrentValue = true;

                await context.SaveChangesAsync();
            }

            var rolTecnicoId = await context.Rol
                .Where(r => r.Nombre == "Tecnico")
                .Select(r => r.Id)
                .FirstAsync();

            var rolMantenimientoId = await context.Rol
                .Where(r => r.Nombre == "Mantenimiento")
                .Select(r => r.Id)
                .FirstAsync();

            var rolAdministradorId = await context.Rol
                .Where(r => r.Nombre == "Administrador")
                .Select(r => r.Id)
                .FirstAsync();

            await AsegurarUsuarioAsync(
                context,
                email: "admin@frenos.local",
                nombre: "Administrador",
                password: "Admin123*",
                rolId: rolAdministradorId);

            await AsegurarUsuarioAsync(
                context,
                email: "tecnico@frenos.local",
                nombre: "Tecnico Frenos",
                password: "Tecnico123*",
                rolId: rolTecnicoId);

            await AsegurarUsuarioAsync(
                context,
                email: "mantenimiento@frenos.local",
                nombre: "Mantenimiento Frenos",
                password: "Mantenimiento123*",
                rolId: rolMantenimientoId);

            var tecnicoAsignadoId = await context.Usuario
                .Where(u => u.Email == "tecnico@frenos.local")
                .Select(u => u.Id)
                .FirstAsync();

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

            var clienteDemo = await context.Cliente.FirstOrDefaultAsync(c => c.Cedula == "001-0000001-1");
            if (clienteDemo is null)
            {
                clienteDemo = new Cliente
                {
                    Nombre = "Cliente Demo",
                    Cedula = "001-0000001-1",
                    Telefono = "809-555-0101",
                    Email = "cliente.demo@frenos.local",
                    Direccion = "Santo Domingo",
                    CreadoEn = DateTime.Now,
                    PasswordHash = string.Empty
                };

                context.Cliente.Add(clienteDemo);
                await context.SaveChangesAsync();
            }

            var vehiculoDemo = await context.Vehiculo.FirstOrDefaultAsync(v => v.Placa == "GH-4521-A");
            if (vehiculoDemo is null)
            {
                vehiculoDemo = new Vehiculo
                {
                    ClienteId = clienteDemo.Id,
                    Placa = "GH-4521-A",
                    Marca = "Toyota",
                    Modelo = "Corolla",
                    Anno = 2018,
                    Color = "Gris",
                    VIN = "1HGBH41JXMN109186",
                    TipoCombustible = "Gasolina",
                    KmActual = 85000,
                    Nota = "Vehículo de prueba",
                    Activo = true,
                    FechaCreacion = DateTime.Now
                };

                context.Vehiculo.Add(vehiculoDemo);
                await context.SaveChangesAsync();
            }

            if (!await context.Orden.AnyAsync())
            {
                var ordenRecibida = new Orden
                {
                    ClienteId = clienteDemo.Id,
                    VehiculoId = vehiculoDemo.Id,
                    TecnicoId = tecnicoAsignadoId,
                    Estado = "Recibido",
                    Prioridad = "Normal",
                    FechaCreacion = DateTime.Now.AddHours(-10),
                    FechaEntregaEstima = DateTime.Now.AddDays(1),
                    Notas = "Ruido al frenar en la parte delantera"
                };

                var ordenEnDiagnostico = new Orden
                {
                    ClienteId = clienteDemo.Id,
                    VehiculoId = vehiculoDemo.Id,
                    TecnicoId = tecnicoAsignadoId,
                    Estado = "EnDiagnostico",
                    Prioridad = "Alta",
                    FechaCreacion = DateTime.Now.AddHours(-8),
                    FechaEntregaEstima = DateTime.Now.AddDays(1),
                    Notas = "Vibración al frenar a alta velocidad"
                };

                var ordenEnReparacion = new Orden
                {
                    ClienteId = clienteDemo.Id,
                    VehiculoId = vehiculoDemo.Id,
                    TecnicoId = tecnicoAsignadoId,
                    Estado = "EnReparacion",
                    Prioridad = "Urgente",
                    FechaCreacion = DateTime.Now.AddHours(-6),
                    FechaEntregaEstima = DateTime.Now.AddHours(6),
                    Notas = "Cambio de pastillas y rectificado de discos"
                };

                var ordenCerradaCredito = new Orden
                {
                    ClienteId = clienteDemo.Id,
                    VehiculoId = vehiculoDemo.Id,
                    TecnicoId = tecnicoAsignadoId,
                    Estado = "Lista",
                    Prioridad = "Alta",
                    FechaCreacion = DateTime.Now.AddDays(-2),
                    FechaEntregaEstima = DateTime.Now.AddDays(-1),
                    Notas = "Reparación completada, pendiente de entrega"
                };

                var ordenCerradaPagada = new Orden
                {
                    ClienteId = clienteDemo.Id,
                    VehiculoId = vehiculoDemo.Id,
                    TecnicoId = tecnicoAsignadoId,
                    Estado = "Entregada",
                    Prioridad = "Normal",
                    FechaCreacion = DateTime.Now.AddDays(-4),
                    FechaEntregaEstima = DateTime.Now.AddDays(-3),
                    FechaEntregaReal = DateTime.Now.AddDays(-2),
                    Notas = "Servicio completado y vehículo entregado"
                };

                context.Orden.AddRange(
                    ordenRecibida,
                    ordenEnDiagnostico,
                    ordenEnReparacion,
                    ordenCerradaCredito,
                    ordenCerradaPagada);

                await context.SaveChangesAsync();

                context.Diagnostico.AddRange(
                    new Diagnostico
                    {
                        OrdenId = ordenEnDiagnostico.Id,
                        TecnicoId = tecnicoAsignadoId,
                        KmIngreso = vehiculoDemo.KmActual,
                        DescripcionGeneral = "Se detecta vibración por desgaste irregular de discos.",
                        Estado = "Borrador",
                        RequiereAtencionUrgente = true,
                        ObservacionesTecnico = "Requiere revisión de discos y pastillas.",
                        FechaDiagnostico = DateTime.Now.AddHours(-7),
                        Items =
                        [
                            new DiagnosticoItem
                            {
                                SistemaVehiculo = "Frenos",
                                Componente = "Disco delantero",
                                Condicion = "Malo",
                                AccionRecomendada = "Reemplazar",
                                Descripcion = "Superficie irregular con vibración.",
                                EsUrgente = true
                            }
                        ]
                    },
                    new Diagnostico
                    {
                        OrdenId = ordenEnReparacion.Id,
                        TecnicoId = tecnicoAsignadoId,
                        KmIngreso = vehiculoDemo.KmActual,
                        DescripcionGeneral = "Diagnóstico completado y aprobado para reparación.",
                        Estado = "Completado",
                        RequiereAtencionUrgente = true,
                        AprobadoPorCliente = true,
                        FechaAprobacion = DateTime.Now.AddHours(-5),
                        ObservacionesTecnico = "En proceso de instalación de nuevos componentes.",
                        FechaDiagnostico = DateTime.Now.AddHours(-6),
                        Items =
                        [
                            new DiagnosticoItem
                            {
                                SistemaVehiculo = "Frenos",
                                Componente = "Pastillas delanteras",
                                Condicion = "Malo",
                                AccionRecomendada = "Reemplazar",
                                Descripcion = "Pastillas por debajo del mínimo.",
                                EsUrgente = true
                            }
                        ]
                    },
                    new Diagnostico
                    {
                        OrdenId = ordenCerradaCredito.Id,
                        TecnicoId = tecnicoAsignadoId,
                        KmIngreso = vehiculoDemo.KmActual,
                        DescripcionGeneral = "Trabajo completado y listo para entrega.",
                        Estado = "Completado",
                        RequiereAtencionUrgente = false,
                        AprobadoPorCliente = true,
                        FechaAprobacion = DateTime.Now.AddDays(-2),
                        ObservacionesTecnico = "Vehículo probado y funcionando correctamente.",
                        FechaDiagnostico = DateTime.Now.AddDays(-2).AddHours(2),
                        Items =
                        [
                            new DiagnosticoItem
                            {
                                SistemaVehiculo = "Frenos",
                                Componente = "Sangrado de línea",
                                Condicion = "Regular",
                                AccionRecomendada = "Reparar",
                                Descripcion = "Se realizó purga y ajuste completo.",
                                EsUrgente = false
                            }
                        ]
                    },
                    new Diagnostico
                    {
                        OrdenId = ordenCerradaPagada.Id,
                        TecnicoId = tecnicoAsignadoId,
                        KmIngreso = vehiculoDemo.KmActual,
                        DescripcionGeneral = "Mantenimiento concluido y entregado.",
                        Estado = "Completado",
                        RequiereAtencionUrgente = false,
                        AprobadoPorCliente = true,
                        FechaAprobacion = DateTime.Now.AddDays(-4),
                        ObservacionesTecnico = "Cliente confirma mejoría total del frenado.",
                        FechaDiagnostico = DateTime.Now.AddDays(-4).AddHours(3),
                        Items =
                        [
                            new DiagnosticoItem
                            {
                                SistemaVehiculo = "Frenos",
                                Componente = "Líquido de frenos",
                                Condicion = "Regular",
                                AccionRecomendada = "Reemplazar",
                                Descripcion = "Cambio total de líquido DOT 4.",
                                EsUrgente = false
                            }
                        ]
                    });

                await context.SaveChangesAsync();

                var cotizacionCredito = new Cotizacion
                {
                    ClienteId = clienteDemo.Id,
                    VehiculoId = vehiculoDemo.Id,
                    Fecha = DateTime.Now.AddDays(-2),
                    Subtotal = 3500m,
                    Itbis = 630m,
                    Total = 4130m,
                    Estado = "Aprobada",
                    Notas = "Cotización aprobada para orden lista pendiente de entrega.",
                    ValidaHasta = DateTime.Now.AddDays(5),
                    Items =
                    [
                        new CotizacionItem
                        {
                            Tipo = "Servicio",
                            ItemId = 1,
                            Descripcion = "Servicio de reparación de frenos",
                            Cantidad = 1,
                            PrecioUnitario = 3500m,
                            Subtotal = 3500m
                        }
                    ]
                };

                var cotizacionPagada = new Cotizacion
                {
                    ClienteId = clienteDemo.Id,
                    VehiculoId = vehiculoDemo.Id,
                    Fecha = DateTime.Now.AddDays(-4),
                    Subtotal = 2200m,
                    Itbis = 396m,
                    Total = 2596m,
                    Estado = "Aprobada",
                    Notas = "Cotización aprobada para orden entregada.",
                    ValidaHasta = DateTime.Now.AddDays(-1),
                    Items =
                    [
                        new CotizacionItem
                        {
                            Tipo = "Producto",
                            ItemId = 1,
                            Descripcion = "Pastillas delanteras",
                            Cantidad = 1,
                            PrecioUnitario = 2200m,
                            Subtotal = 2200m
                        }
                    ]
                };

                context.Cotizacion.AddRange(cotizacionCredito, cotizacionPagada);
                await context.SaveChangesAsync();

                ordenCerradaCredito.CotizacionId = cotizacionCredito.Id;
                ordenCerradaPagada.CotizacionId = cotizacionPagada.Id;
                await context.SaveChangesAsync();

                var adminUsuarioId = await context.Usuario
                    .Where(u => u.Email == "admin@frenos.local")
                    .Select(u => u.Id)
                    .FirstAsync();

                var anno = DateTime.Now.Year;
                var facturaCredito = new Factura
                {
                    OrdenId = ordenCerradaCredito.Id,
                    TipoOrigen = "OrdenReparacion",
                    ClienteId = clienteDemo.Id,
                    Numero = $"FAC-{anno}-9001",
                    Fecha = DateTime.Now.AddDays(-1),
                    Subtotal = 3500m,
                    Itbis = 630m,
                    Total = 4130m,
                    Estado = "Pendiente",
                    MetodoPago = "Credito",
                    EmitidaPor = adminUsuarioId,
                    Items =
                    [
                        new FacturaItem
                        {
                            Tipo = "Servicio",
                            ItemId = 1,
                            Descripcion = "Servicio de reparación de frenos",
                            Cantidad = 1,
                            PrecioUnitario = 3500m,
                            Subtotal = 3500m
                        }
                    ]
                };

                var facturaPagada = new Factura
                {
                    OrdenId = ordenCerradaPagada.Id,
                    TipoOrigen = "OrdenReparacion",
                    ClienteId = clienteDemo.Id,
                    Numero = $"FAC-{anno}-9002",
                    Fecha = DateTime.Now.AddDays(-2),
                    Subtotal = 2200m,
                    Itbis = 396m,
                    Total = 2596m,
                    Estado = "Pagada",
                    MetodoPago = "Efectivo",
                    EmitidaPor = adminUsuarioId,
                    Items =
                    [
                        new FacturaItem
                        {
                            Tipo = "Producto",
                            ItemId = 1,
                            Descripcion = "Pastillas delanteras",
                            Cantidad = 1,
                            PrecioUnitario = 2200m,
                            Subtotal = 2200m
                        }
                    ]
                };

                context.Factura.AddRange(facturaCredito, facturaPagada);
                await context.SaveChangesAsync();

                context.CuentasPorCobrar.Add(new CuentasPorCobrar
                {
                    ClienteId = clienteDemo.Id,
                    FacturaId = facturaCredito.Id,
                    Monto = facturaCredito.Total,
                    Saldo = facturaCredito.Total,
                    Vencimiento = DateTime.Now.AddDays(30),
                    Estado = "Pendiente",
                    CreadoEn = DateTime.Now
                });

                await context.SaveChangesAsync();
            }
        }

        private static async Task AsegurarRolAsync(AppDbContext context, string nombre, string descripcion)
        {
            if (await context.Rol.AnyAsync(r => r.Nombre == nombre))
                return;

            context.Rol.Add(new Rol
            {
                Nombre = nombre,
                Descripcion = descripcion
            });

            await context.SaveChangesAsync();
        }

        private static async Task AsegurarUsuarioAsync(
            AppDbContext context,
            string email,
            string nombre,
            string password,
            int rolId)
        {
            var emailNorm = email.Trim().ToLowerInvariant();
            if (await context.Usuario.AnyAsync(u => u.Email == emailNorm))
                return;

            context.Usuario.Add(new Usuario
            {
                Nombre = nombre,
                Email = emailNorm,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                RolId = rolId,
                Activo = true,
                FechaCreacion = DateTime.Now
            });

            await context.SaveChangesAsync();
        }
    }
}
