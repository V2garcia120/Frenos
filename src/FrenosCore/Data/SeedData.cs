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

            var rolCajaId = await context.Rol
                .Where(r => r.Nombre == "Caja")
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

            await AsegurarUsuarioAsync(
                context,
                email: "caja@frenos.local",
                nombre: "Caja Frenos",
                password: "Caja123*",
                rolId: rolCajaId);

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
                        Nombre = "Cambio de bandas traseras",
                        Descripcion = "Reemplazo de bandas traseras y calibración de freno",
                        Precio = 1700m,
                        DuracionMinutos = 80,
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
                        Nombre = "Cambio de líquido de frenos",
                        Descripcion = "Sustitución completa de líquido DOT 4 y purga de líneas",
                        Precio = 1200m,
                        DuracionMinutos = 45,
                        Categoria = "Fluidos",
                        Activo = true
                    },
                    new Servicio
                    {
                        Nombre = "Inspección de ABS",
                        Descripcion = "Diagnóstico de sensores ABS y lectura de códigos",
                        Precio = 2100m,
                        DuracionMinutos = 70,
                        Categoria = "Diagnóstico",
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
                    },
                    new Servicio
                    {
                        Nombre = "Cambio de kit de frenos completo",
                        Descripcion = "Reemplazo de discos, bandas, líquido y ajuste integral",
                        Precio = 6200m,
                        DuracionMinutos = 180,
                        Categoria = "Frenos",
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
                        Nombre = "Bandas semimetálicas traseras",
                        Descripcion = "Juego de bandas semimetálicas de larga duración",
                        Precio = 2850m,
                        Costo = 1980m,
                        Stock = 20,
                        StockMinimo = 7,
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
                    },
                    new Producto
                    {
                        Nombre = "Sensor ABS delantero",
                        Descripcion = "Sensor ABS universal para eje delantero",
                        Precio = 1950m,
                        Costo = 1320m,
                        Stock = 18,
                        StockMinimo = 6,
                        Categoria = "Electrónica",
                        Activo = true,
                        CreadoEn = DateTime.Now
                    },
                    new Producto
                    {
                        Nombre = "Kit de reparación de cáliper",
                        Descripcion = "Juego de sellos y guías para cáliper delantero",
                        Precio = 1450m,
                        Costo = 960m,
                        Stock = 22,
                        StockMinimo = 8,
                        Categoria = "Repuestos",
                        Activo = true,
                        CreadoEn = DateTime.Now
                    },
                    new Producto
                    {
                        Nombre = "Manguera hidráulica de freno",
                        Descripcion = "Manguera reforzada para sistema hidráulico",
                        Precio = 980m,
                        Costo = 640m,
                        Stock = 30,
                        StockMinimo = 10,
                        Categoria = "Repuestos",
                        Activo = true,
                        CreadoEn = DateTime.Now
                    },
                    new Producto
                    {
                        Nombre = "Grasa para pinzas de freno",
                        Descripcion = "Lubricante de alta temperatura para sistema de frenos",
                        Precio = 390m,
                        Costo = 240m,
                        Stock = 60,
                        StockMinimo = 15,
                        Categoria = "Consumibles",
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

            var clienteEmpresa = await context.Cliente.FirstOrDefaultAsync(c => c.Cedula == "001-0000002-2");
            if (clienteEmpresa is null)
            {
                clienteEmpresa = new Cliente
                {
                    Nombre = "Transporte del Caribe SRL",
                    Cedula = "001-0000002-2",
                    Telefono = "809-555-0202",
                    Email = "flota@caribe.local",
                    Direccion = "Santo Domingo Este",
                    CreadoEn = DateTime.Now,
                    PasswordHash = string.Empty
                };

                context.Cliente.Add(clienteEmpresa);
                await context.SaveChangesAsync();
            }

            var clienteParticular = await context.Cliente.FirstOrDefaultAsync(c => c.Cedula == "001-0000003-3");
            if (clienteParticular is null)
            {
                clienteParticular = new Cliente
                {
                    Nombre = "María Pérez",
                    Cedula = "001-0000003-3",
                    Telefono = "829-555-0303",
                    Email = "maria.perez@correo.local",
                    Direccion = "Santiago de los Caballeros",
                    CreadoEn = DateTime.Now,
                    PasswordHash = string.Empty
                };

                context.Cliente.Add(clienteParticular);
                await context.SaveChangesAsync();
            }

            var vehiculoEmpresa = await context.Vehiculo.FirstOrDefaultAsync(v => v.Placa == "L456789");
            if (vehiculoEmpresa is null)
            {
                vehiculoEmpresa = new Vehiculo
                {
                    ClienteId = clienteEmpresa.Id,
                    Placa = "L456789",
                    Marca = "Hyundai",
                    Modelo = "H100",
                    Anno = 2020,
                    Color = "Blanco",
                    VIN = "KMJWA37JDLU238471",
                    TipoCombustible = "Diesel",
                    KmActual = 126000,
                    Nota = "Vehículo de flotilla",
                    Activo = true,
                    FechaCreacion = DateTime.Now
                };

                context.Vehiculo.Add(vehiculoEmpresa);
                await context.SaveChangesAsync();
            }

            var vehiculoParticular = await context.Vehiculo.FirstOrDefaultAsync(v => v.Placa == "A908765");
            if (vehiculoParticular is null)
            {
                vehiculoParticular = new Vehiculo
                {
                    ClienteId = clienteParticular.Id,
                    Placa = "A908765",
                    Marca = "Kia",
                    Modelo = "Sportage",
                    Anno = 2021,
                    Color = "Azul",
                    VIN = "KNDPB3AC8M7092145",
                    TipoCombustible = "Gasolina",
                    KmActual = 63500,
                    Nota = "Uso familiar",
                    Activo = true,
                    FechaCreacion = DateTime.Now
                };

                context.Vehiculo.Add(vehiculoParticular);
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

                var ordenAprobadaParticular = new Orden
                {
                    ClienteId = clienteParticular.Id,
                    VehiculoId = vehiculoParticular.Id,
                    TecnicoId = tecnicoAsignadoId,
                    Estado = "Aprobado",
                    Prioridad = "Alta",
                    FechaCreacion = DateTime.Now.AddHours(-4),
                    FechaEntregaEstima = DateTime.Now.AddDays(2),
                    Notas = "Cliente aprueba reparación completa del eje delantero"
                };

                var ordenListaFlota = new Orden
                {
                    ClienteId = clienteEmpresa.Id,
                    VehiculoId = vehiculoEmpresa.Id,
                    TecnicoId = tecnicoAsignadoId,
                    Estado = "Lista",
                    Prioridad = "Urgente",
                    FechaCreacion = DateTime.Now.AddDays(-1),
                    FechaEntregaEstima = DateTime.Now,
                    Notas = "Unidad de flotilla lista para retiro"
                };

                var ordenCancelada = new Orden
                {
                    ClienteId = clienteEmpresa.Id,
                    VehiculoId = vehiculoEmpresa.Id,
                    TecnicoId = tecnicoAsignadoId,
                    Estado = "Cancelada",
                    Prioridad = "Normal",
                    FechaCreacion = DateTime.Now.AddDays(-6),
                    FechaEntregaEstima = DateTime.Now.AddDays(-5),
                    Notas = "Orden cancelada por cliente por cambio de presupuesto"
                };

                context.Orden.AddRange(
                    ordenRecibida,
                    ordenEnDiagnostico,
                    ordenEnReparacion,
                    ordenCerradaCredito,
                    ordenCerradaPagada,
                    ordenAprobadaParticular,
                    ordenListaFlota,
                    ordenCancelada);

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
                    },
                    new Diagnostico
                    {
                        OrdenId = ordenAprobadaParticular.Id,
                        TecnicoId = tecnicoAsignadoId,
                        KmIngreso = vehiculoParticular.KmActual,
                        DescripcionGeneral = "Desgaste severo de bandas delanteras y sensor ABS intermitente.",
                        Estado = "Completado",
                        RequiereAtencionUrgente = true,
                        AprobadoPorCliente = true,
                        FechaAprobacion = DateTime.Now.AddHours(-2),
                        ObservacionesTecnico = "A la espera de repuestos para iniciar reparación.",
                        FechaDiagnostico = DateTime.Now.AddHours(-3),
                        Items =
                        [
                            new DiagnosticoItem
                            {
                                SistemaVehiculo = "Frenos",
                                Componente = "Bandas delanteras",
                                Condicion = "Malo",
                                AccionRecomendada = "Reemplazar",
                                Descripcion = "Material de fricción agotado.",
                                EsUrgente = true
                            },
                            new DiagnosticoItem
                            {
                                SistemaVehiculo = "Frenos",
                                Componente = "Sensor ABS",
                                Condicion = "Regular",
                                AccionRecomendada = "Reparar",
                                Descripcion = "Falla intermitente en lectura de rueda delantera derecha.",
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

                var cotizacionAprobadaParticular = new Cotizacion
                {
                    ClienteId = clienteParticular.Id,
                    VehiculoId = vehiculoParticular.Id,
                    Fecha = DateTime.Now.AddHours(-3),
                    Subtotal = 5600m,
                    Itbis = 1008m,
                    Total = 6608m,
                    Estado = "Aprobada",
                    Notas = "Incluye bandas delanteras, sensor ABS y cambio de líquido.",
                    ValidaHasta = DateTime.Now.AddDays(7),
                    Items =
                    [
                        new CotizacionItem
                        {
                            Tipo = "Producto",
                            ItemId = 2,
                            Descripcion = "Bandas semimetálicas traseras",
                            Cantidad = 1,
                            PrecioUnitario = 2850m,
                            Subtotal = 2850m
                        },
                        new CotizacionItem
                        {
                            Tipo = "Producto",
                            ItemId = 4,
                            Descripcion = "Sensor ABS delantero",
                            Cantidad = 1,
                            PrecioUnitario = 1950m,
                            Subtotal = 1950m
                        },
                        new CotizacionItem
                        {
                            Tipo = "Servicio",
                            ItemId = 4,
                            Descripcion = "Cambio de líquido de frenos",
                            Cantidad = 1,
                            PrecioUnitario = 800m,
                            Subtotal = 800m
                        }
                    ]
                };

                context.Cotizacion.AddRange(cotizacionCredito, cotizacionPagada, cotizacionAprobadaParticular);
                await context.SaveChangesAsync();

                ordenCerradaCredito.CotizacionId = cotizacionCredito.Id;
                ordenCerradaPagada.CotizacionId = cotizacionPagada.Id;
                ordenAprobadaParticular.CotizacionId = cotizacionAprobadaParticular.Id;
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

                var facturaFlotaCredito = new Factura
                {
                    OrdenId = ordenListaFlota.Id,
                    TipoOrigen = "OrdenReparacion",
                    ClienteId = clienteEmpresa.Id,
                    Numero = $"FAC-{anno}-9003",
                    Fecha = DateTime.Now.AddHours(-12),
                    Subtotal = 5400m,
                    Itbis = 972m,
                    Total = 6372m,
                    Estado = "Pendiente",
                    MetodoPago = "Credito",
                    EmitidaPor = adminUsuarioId,
                    Items =
                    [
                        new FacturaItem
                        {
                            Tipo = "Servicio",
                            ItemId = 2,
                            Descripcion = "Rectificado de discos",
                            Cantidad = 1,
                            PrecioUnitario = 2500m,
                            Subtotal = 2500m
                        },
                        new FacturaItem
                        {
                            Tipo = "Producto",
                            ItemId = 3,
                            Descripcion = "Disco de freno ventilado",
                            Cantidad = 1,
                            PrecioUnitario = 2900m,
                            Subtotal = 2900m
                        }
                    ]
                };

                context.Factura.AddRange(facturaCredito, facturaPagada, facturaFlotaCredito);
                await context.SaveChangesAsync();

                var cxcDemo = new CuentasPorCobrar
                {
                    ClienteId = clienteDemo.Id,
                    FacturaId = facturaCredito.Id,
                    Monto = facturaCredito.Total,
                    Saldo = facturaCredito.Total,
                    Vencimiento = DateTime.Now.AddDays(30),
                    Estado = "Pendiente",
                    CreadoEn = DateTime.Now
                };

                var cxcFlota = new CuentasPorCobrar
                {
                    ClienteId = clienteEmpresa.Id,
                    FacturaId = facturaFlotaCredito.Id,
                    Monto = facturaFlotaCredito.Total,
                    Saldo = 3372m,
                    Vencimiento = DateTime.Now.AddDays(15),
                    Estado = "Pendiente",
                    CreadoEn = DateTime.Now
                };

                context.CuentasPorCobrar.AddRange(cxcDemo, cxcFlota);
                await context.SaveChangesAsync();

                context.AbonoCxC.Add(new AbonoCxC
                {
                    CxCId = cxcFlota.Id,
                    Monto = 3000m,
                    Fecha = DateTime.Now.AddHours(-6),
                    MetodoPago = "Transferencia",
                    RegistradoPor = adminUsuarioId
                });

                context.HistorialReparacion.AddRange(
                    new HistorialReparacion
                    {
                        VehiculoId = vehiculoDemo.Id,
                        OrdenId = ordenCerradaCredito.Id,
                        TecnicoId = tecnicoAsignadoId,
                        KmAlServicio = 85250,
                        TrabajosRealizados = "Rectificado de discos delanteros, purga de líneas y ajuste general del sistema de frenos.",
                        ProximoServicioKm = 92000,
                        ProximoServicioFecha = DateOnly.FromDateTime(DateTime.Now.AddMonths(6)),
                        GarantiaDias = 45,
                        GarantiaHasta = DateOnly.FromDateTime(DateTime.Now.AddDays(45)),
                        Fecha = DateTime.Now.AddDays(-1)
                    },
                    new HistorialReparacion
                    {
                        VehiculoId = vehiculoDemo.Id,
                        OrdenId = ordenCerradaPagada.Id,
                        TecnicoId = tecnicoAsignadoId,
                        KmAlServicio = 84800,
                        TrabajosRealizados = "Cambio de líquido DOT 4, sustitución de bandas delanteras y prueba de ruta.",
                        ProximoServicioKm = 90000,
                        ProximoServicioFecha = DateOnly.FromDateTime(DateTime.Now.AddMonths(5)),
                        GarantiaDias = 30,
                        GarantiaHasta = DateOnly.FromDateTime(DateTime.Now.AddDays(30)),
                        Fecha = DateTime.Now.AddDays(-2)
                    },
                    new HistorialReparacion
                    {
                        VehiculoId = vehiculoEmpresa.Id,
                        OrdenId = ordenListaFlota.Id,
                        TecnicoId = tecnicoAsignadoId,
                        KmAlServicio = 126100,
                        TrabajosRealizados = "Cambio de disco ventilado, reemplazo de manguera hidráulica y calibración de frenos traseros.",
                        ProximoServicioKm = 132000,
                        ProximoServicioFecha = DateOnly.FromDateTime(DateTime.Now.AddMonths(4)),
                        GarantiaDias = 60,
                        GarantiaHasta = DateOnly.FromDateTime(DateTime.Now.AddDays(60)),
                        Fecha = DateTime.Now.AddHours(-12)
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
