using FrenosCore.Data;
using FrenosCore.Modelos.Dtos;

using FrenosCore.Modelos.Dtos.Orden;
using FrenosCore.Modelos.Dtos.Diagnostico;
using FrenosCore.Modelos.Dtos.Factura;
using FrenosCore.Modelos.Entidades;

using Microsoft.EntityFrameworkCore;

namespace FrenosCore.Servicios
{

    
    public class OrdenService(
    AppDbContext db,
    IFacturaService facturas,
    ICotizacionService cotizaciones) : IOrdenService
    {
        // Estados válidos y sus transiciones permitidas
        private static readonly Dictionary<string, string[]> _transiciones = new()
        {
            ["Recibido"] = ["EnDiagnostico"],
            ["EnDiagnostico"] = ["Aprobado", "Recibido"],   // Recibido si el cliente rechaza
            ["Aprobado"] = ["EnReparacion"],
            ["EnReparacion"] = ["Lista"],
            ["Lista"] = ["Entregada"],
            ["Entregada"] = []                          
        };

      
        public async Task<PaginadoResponse<OrdenResponse>> ListarAsync(
            int pagina, int tam, string? estado, string? prioridad, int? tecnicoId, DateTime? fecha)
        {
            pagina = Math.Max(1, pagina);
            tam = Math.Clamp(tam, 1, 100);

            var query = db.Orden
                .AsNoTracking()
                .Include(o => o.Cliente)
                .Include(o => o.Vehiculo)
                .Include(o => o.TecnicoAsignado)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(estado))
                query = query.Where(o => o.Estado == estado);

            if (!string.IsNullOrWhiteSpace(prioridad))
                query = query.Where(o => o.Prioridad == prioridad);

            if (tecnicoId.HasValue)
                query = query.Where(o => o.TecnicoId == tecnicoId.Value);

            if (fecha.HasValue)
            {
                var inicio = fecha.Value.Date;
                var fin = inicio.AddDays(1);
                query = query.Where(o => o.FechaCreacion >= inicio && o.FechaCreacion < fin);
            }

            var totalItems = await query.CountAsync();

            var items = await query
                .OrderByDescending(o => o.FechaCreacion)
                .Skip((pagina - 1) * tam)
                .Take(tam)
                .Select(o => new OrdenResponse(
                    o.Id,
                    o.ClienteId,
                    o.Cliente.Nombre,
                    o.VehiculoId,
                    $"{o.Vehiculo.Marca} {o.Vehiculo.Modelo} {o.Vehiculo.Anno} · {o.Vehiculo.Placa}",
                    o.TecnicoId,
                    o.TecnicoAsignado != null ? o.TecnicoAsignado.Nombre : null,
                    o.CotizacionId,
                    o.Estado,
                    o.Prioridad,
                    o.FechaCreacion,
                    o.FechaEntregaEstima,
                    o.FechaEntregaReal,
                    o.Notas,
                    o.Diagnostico != null,
                    o.Factura != null))
                .ToListAsync();

            return new PaginadoResponse<OrdenResponse>(
                Items: items,
                PaginaActual: pagina,
                TamPagina: tam,
                TotalItems: totalItems,
                TotalPaginas: (int)Math.Ceiling(totalItems / (double)tam));
        }


        public async Task<OrdenDetalleResponse> ObtenerPorIdAsync(int id)
        {
            var orden = await db.Orden
                .AsNoTracking()
                .Include(o => o.Cliente)
                .Include(o => o.Vehiculo)
                .Include(o => o.TecnicoAsignado)
                .Include(o => o.Diagnostico)
                    .ThenInclude(d => d!.Items)
                        .ThenInclude(i => i.ServicioSugerido)
                .Include(o => o.Diagnostico)
                    .ThenInclude(d => d!.Items)
                        .ThenInclude(i => i.ProductoSugerido)
                .Include(o => o.Diagnostico)
                    .ThenInclude(d => d!.Tecnico)
                .Include(o => o.Factura)
                .FirstOrDefaultAsync(o => o.Id == id)
                ?? throw new KeyNotFoundException($"Orden {id} no encontrada.");

            DiagnosticoResponse? diagResponse = null;
            if (orden.Diagnostico is not null)
                diagResponse = MapearDiagnostico(orden.Diagnostico, orden);

            return new OrdenDetalleResponse(
                Id: orden.Id,
                ClienteId: orden.ClienteId,
                ClienteNombre: orden.Cliente.Nombre,
                VehiculoId: orden.VehiculoId,
                VehiculoInfo: $"{orden.Vehiculo.Marca} {orden.Vehiculo.Modelo} {orden.Vehiculo.Anno} · {orden.Vehiculo.Placa}",
                TecnicoId: orden.TecnicoId,
                TecnicoNombre: orden.TecnicoAsignado?.Nombre,
                CotizacionId: orden.CotizacionId,
                Estado: orden.Estado,
                Prioridad: orden.Prioridad,
                FechaIngreso: orden.FechaCreacion,
                FechaEntregaEstimada: orden.FechaEntregaEstima,
                FechaEntregaReal: orden.FechaEntregaReal,
                Notas: orden.Notas,
                Diagnostico: diagResponse,
                FacturaId: orden.Factura?.Id);
        }

        
        public async Task<OrdenResponse> CrearAsync(CrearOrdenRequest req)
        {

            var cliente = await db.Cliente.FindAsync(req.ClienteId)
                ?? throw new KeyNotFoundException(
                    $"Cliente {req.ClienteId} no encontrado.");

 
            var vehiculo = await db.Vehiculo
                .FirstOrDefaultAsync(v =>
                    v.Id == req.VehiculoId && v.ClienteId == req.ClienteId && v.Activo)
                ?? throw new KeyNotFoundException(
                    $"Vehículo {req.VehiculoId} no encontrado, inactivo " +
                    $"o no pertenece al cliente {req.ClienteId}.");

       
            string[] prioridadesValidas = ["Normal", "Alta", "Urgente"];
            if (!prioridadesValidas.Contains(req.Prioridad))
                throw new ArgumentException(
                    $"Prioridad '{req.Prioridad}' no válida. " +
                    $"Valores permitidos: {string.Join(", ", prioridadesValidas)}.");

            Usuario? tecnicoAsignado = null;
            if (req.TecnicoId.HasValue)
            {
                tecnicoAsignado = await db.Usuario
                    .Include(u => u.Rol)
                    .FirstOrDefaultAsync(u => u.Id == req.TecnicoId.Value && u.Activo)
                    ?? throw new KeyNotFoundException($"Técnico {req.TecnicoId.Value} no encontrado o inactivo.");

                var esTecnico = tecnicoAsignado.Rol.Nombre.Contains("tecnico", StringComparison.OrdinalIgnoreCase)
                                || tecnicoAsignado.Rol.Nombre.Contains("técnico", StringComparison.OrdinalIgnoreCase);
                if (!esTecnico)
                    throw new InvalidOperationException("El usuario asignado no tiene rol de técnico.");
            }


            if (req.CotizacionId.HasValue)
            {
                var cotizacion = await db.Cotizacion
                    .FirstOrDefaultAsync(c =>
                        c.Id == req.CotizacionId.Value &&
                        c.ClienteId == req.ClienteId)
                    ?? throw new KeyNotFoundException(
                        $"Cotización {req.CotizacionId} no encontrada " +
                        $"o no pertenece al cliente {req.ClienteId}.");

                if (cotizacion.Estado != "Aprobada")
                    throw new InvalidOperationException(
                        "Solo se puede asociar una cotización en estado Aprobada a una orden.");
            }

            var orden = new Orden
            {
                ClienteId = req.ClienteId,
                VehiculoId = req.VehiculoId,
                TecnicoId = req.TecnicoId,
                CotizacionId = req.CotizacionId,
                Estado = "Recibido",
                Prioridad = req.Prioridad,
                FechaCreacion = DateTime.UtcNow,
                FechaEntregaEstima = req.FechaEntregaEstimada,
                Notas = req.Notas?.Trim(),
            };

            db.Orden.Add(orden);
            await db.SaveChangesAsync();

            return new OrdenResponse(
                orden.Id, orden.ClienteId, cliente.Nombre,
                orden.VehiculoId,
                $"{vehiculo.Marca} {vehiculo.Modelo} {vehiculo.Anno} · {vehiculo.Placa}",
                orden.TecnicoId,
                tecnicoAsignado?.Nombre,
                orden.CotizacionId, orden.Estado, orden.Prioridad,
                orden.FechaCreacion, orden.FechaEntregaEstima,
                orden.FechaEntregaReal, orden.Notas,
                TieneDiagnostico: false, TieneFactura: false);
        }


        public async Task<OrdenResponse> CambiarEstadoAsync(
            int id, CambiarEstadoOrdenRequest req)
        {
            var orden = await db.Orden
                .Include(o => o.Cliente)
                .Include(o => o.Vehiculo)
                .Include(o => o.TecnicoAsignado)
                .Include(o => o.Diagnostico)
                .FirstOrDefaultAsync(o => o.Id == id)
                ?? throw new KeyNotFoundException($"Orden {id} no encontrada.");

            
            if (!_transiciones.TryGetValue(orden.Estado, out var estadosSiguientes))
                throw new InvalidOperationException(
                    $"Estado actual '{orden.Estado}' no reconocido.");

            if (!estadosSiguientes.Contains(req.Estado))
                throw new InvalidOperationException(
                    $"No se puede pasar de '{orden.Estado}' a '{req.Estado}'. " +
                    $"Transiciones permitidas: {string.Join(", ", estadosSiguientes)}.");

            
            if (req.Estado == "Aprobado")
            {
                if (orden.Diagnostico is null)
                    throw new InvalidOperationException(
                        "La orden requiere un diagnóstico aprobado por el cliente " +
                        "antes de pasar al estado Aprobado.");

                if (!orden.Diagnostico.AprobadoPorCliente)
                    throw new InvalidOperationException(
                        "La orden requiere un diagnóstico aprobado por el cliente " +
                        "antes de pasar al estado Aprobado.");
            }

            orden.Estado = req.Estado;

            
            if (req.Estado == "Entregada")
                orden.FechaEntregaReal = DateTime.UtcNow;

            await db.SaveChangesAsync();

            return new OrdenResponse(
                orden.Id, orden.ClienteId, orden.Cliente.Nombre,
                orden.VehiculoId,
                $"{orden.Vehiculo.Marca} {orden.Vehiculo.Modelo} {orden.Vehiculo.Anno} · {orden.Vehiculo.Placa}",
                orden.TecnicoId,
                orden.TecnicoAsignado?.Nombre,
                orden.CotizacionId, orden.Estado, orden.Prioridad,
                orden.FechaCreacion, orden.FechaEntregaEstima,
                orden.FechaEntregaReal, orden.Notas,
                orden.Diagnostico != null, TieneFactura: false);
        }

        
        public async Task<CerrarOrdenResponse> CerrarAsync(int id, CerrarOrdenRequest req)
        {
            var orden = await db.Orden
                .Include(o => o.Vehiculo)
                .Include(o => o.Diagnostico)
                .Include(o => o.Cotizacion)
                    .ThenInclude(c => c!.Items)
                .FirstOrDefaultAsync(o => o.Id == id)
                ?? throw new KeyNotFoundException($"Orden {id} no encontrada.");

            if (orden.Estado != "Lista")
                throw new InvalidOperationException(
                    "Solo se puede cerrar una orden en estado Lista.");

            if (orden.Diagnostico is null)
                throw new InvalidOperationException(
                    "No se puede cerrar la orden porque no tiene diagnóstico registrado.");

            if (!orden.Diagnostico.AprobadoPorCliente)
                throw new InvalidOperationException(
                    "No se puede cerrar la orden porque el diagnóstico no ha sido aprobado por el cliente.");

            if (orden.Diagnostico.Estado != "Completado" && orden.Diagnostico.Estado != "Aprobado")
                throw new InvalidOperationException(
                    "No se puede cerrar la orden porque el diagnóstico debe estar en estado Aprobado o Completado.");


            var tecnico = await db.Usuario
                .FirstOrDefaultAsync(u => u.Id == req.TecnicoId && u.Activo)
                ?? throw new KeyNotFoundException(
                    $"Técnico {req.TecnicoId} no encontrado o inactivo.");


            var tieneFactura = await db.Factura.AnyAsync(f => f.OrdenId == id);
            if (tieneFactura)
                throw new InvalidOperationException(
                    $"La orden {id} ya tiene una factura generada.");

            using var transaccion = await db.Database.BeginTransactionAsync();
            try
            {

                var garantiaHasta = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(req.GarantiaDias));
                var historial = new HistorialReparacion
                {
                    VehiculoId = orden.VehiculoId,
                    OrdenId = orden.Id,
                    TecnicoId = req.TecnicoId,
                    KmAlServicio = req.KmAlServicio,
                    TrabajosRealizados = req.TrabajosRealizados.Trim(),
                    ProximoServicioKm = req.ProximoServicioKm,
                    ProximoServicioFecha = req.ProximoServicioFecha,
                    GarantiaDias = req.GarantiaDias,
                    GarantiaHasta = garantiaHasta,
                    Fecha = DateTime.UtcNow
                };
                db.HistorialReparacion.Add(historial);


                if (req.KmAlServicio.HasValue)
                    orden.Vehiculo.KmActual = req.KmAlServicio.Value;

                if (!orden.CotizacionId.HasValue)
                {
                    var cotizacion = await cotizaciones.GenerarDesdeDiagnosticoAsync(orden.Diagnostico.Id);
                    await cotizaciones.AprobarAsync(cotizacion.Id);
                    orden.CotizacionId = cotizacion.Id;
                }
                else if (orden.Cotizacion is not null && orden.Cotizacion.Estado != "Aprobada")
                {
                    await cotizaciones.AprobarAsync(orden.CotizacionId.Value);
                }


                orden.Estado = "Lista";

                await db.SaveChangesAsync();


                var factura = await facturas.GenerarDesdeOrdenAsync(orden.Id, req.TecnicoId, req.MetodoPago);

                await facturas.RegistrarPagoAsync(
                    factura.Id,
                    new RegistrarPagoRequest(req.MetodoPago, factura.Total));

                await transaccion.CommitAsync();

                return new CerrarOrdenResponse(
                    HistorialId: historial.Id,
                    FacturaId: factura.Id,
                    NumeroFactura: factura.Numero,
                    OrdenId: orden.Id,
                    GarantiaHasta: garantiaHasta.ToString("yyyy-MM-dd"));
            }
            catch
            {
                await transaccion.RollbackAsync();
                throw;
            }
        }

        private static DiagnosticoResponse MapearDiagnostico(Diagnostico d, Orden o) => new(
            Id: d.Id,
            OrdenId: d.OrdenId,
            VehiculoInfo: $"{o.Vehiculo.Marca} {o.Vehiculo.Modelo} {o.Vehiculo.Anno} · {o.Vehiculo.Placa}",
            ClienteNombre: o.Cliente.Nombre,
            TecnicoId: d.TecnicoId,
            TecnicoNombre: d.Tecnico?.Nombre ?? string.Empty,
            KmIngreso: d.KmIngreso,
            DescripcionGeneral: d.DescripcionGeneral,
            Estado: d.Estado,
            RequiereAtencionUrgente: d.RequiereAtencionUrgente,
            AprobadoPorCliente: d.AprobadoPorCliente,
            FechaAprobacion: d.FechaAprobacion,
            ObservacionesTecnico: d.ObservacionesTecnico,
            FechaDiagnostico: d.FechaDiagnostico,
            Items: d.Items.Select(i => new DiagnosticoItemResponse(
                i.Id, i.SistemaVehiculo, i.Componente, i.Condicion,
                i.AccionRecomendada, i.Descripcion,
                i.ServicioSugeridoId, i.ServicioSugerido?.Nombre,
                i.ProductoSugeridoId, i.ProductoSugerido?.Nombre,
                i.EsUrgente)));
    }
}

