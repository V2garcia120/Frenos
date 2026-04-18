using FrenosCore.Data;
using FrenosCore.Modelos.Dtos.Dashboard;
using Microsoft.EntityFrameworkCore;

namespace FrenosCore.Servicios
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AdminDashboardResponse> ObtenerDashboardAdminAsync()
        {
            var hoy = DateTime.Today;
            var manana = hoy.AddDays(1);
            var ayer = hoy.AddDays(-1);
            var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);
            var inicioMesAnterior = inicioMes.AddMonths(-1);
            var finMes = inicioMes.AddMonths(1);

            var facturadoHoy = await _context.Factura
                .AsNoTracking()
                .Where(f => f.Fecha >= hoy && f.Fecha < manana)
                .SumAsync(f => (decimal?)f.Total) ?? 0m;

            var facturadoAyer = await _context.Factura
                .AsNoTracking()
                .Where(f => f.Fecha >= ayer && f.Fecha < hoy)
                .SumAsync(f => (decimal?)f.Total) ?? 0m;

            var facturadoMes = await _context.Factura
                .AsNoTracking()
                .Where(f => f.Fecha >= inicioMes && f.Fecha < finMes)
                .SumAsync(f => (decimal?)f.Total) ?? 0m;

            var facturadoMesAnterior = await _context.Factura
                .AsNoTracking()
                .Where(f => f.Fecha >= inicioMesAnterior && f.Fecha < inicioMes)
                .SumAsync(f => (decimal?)f.Total) ?? 0m;

            var cxcPendiente = await _context.CuentasPorCobrar
                .AsNoTracking()
                .Where(c => c.Saldo > 0)
                .SumAsync(c => (decimal?)c.Saldo) ?? 0m;

            var cuentasVencidas = await _context.CuentasPorCobrar
                .AsNoTracking()
                .CountAsync(c => c.Saldo > 0 && c.Vencimiento.Date < hoy);

            var ordenesActivas = await _context.Orden
                .AsNoTracking()
                .CountAsync(o => o.FechaEntregaReal == null && o.Estado != "Entregada" && o.Estado != "Cancelada");

            var ordenesUrgentes = await _context.Orden
                .AsNoTracking()
                .CountAsync(o => o.FechaEntregaReal == null
                    && o.Estado != "Entregada"
                    && o.Estado != "Cancelada"
                    && o.Prioridad.ToLower().Contains("urg"));

            var serviciosMasSolicitadosRaw = await _context.FacturaItem
                .AsNoTracking()
                .Where(fi => fi.Tipo == "Servicio" && fi.Factura.Fecha >= inicioMes && fi.Factura.Fecha < finMes)
                .GroupBy(fi => fi.Descripcion)
                .Select(g => new { Nombre = g.Key, Cantidad = g.Sum(x => x.Cantidad) })
                .OrderByDescending(x => x.Cantidad)
                .Take(4)
                .ToListAsync();

            var serviciosMasSolicitados = serviciosMasSolicitadosRaw
                .Select(x => new DashboardServicioSolicitadoItem(x.Nombre, x.Cantidad))
                .ToList();

            var estadoOrdenesRaw = await _context.Orden
                .AsNoTracking()
                .Where(o => o.FechaEntregaReal == null && o.Estado != "Entregada" && o.Estado != "Cancelada")
                .GroupBy(o => o.Estado)
                .Select(g => new { Estado = g.Key, Cantidad = g.Count() })
                .OrderByDescending(x => x.Cantidad)
                .Take(4)
                .ToListAsync();

            var estadoOrdenes = estadoOrdenesRaw
                .Select(x => new DashboardEstadoOrdenItem(x.Estado, x.Cantidad))
                .ToList();

            var cxcProximas = await _context.CuentasPorCobrar
                .AsNoTracking()
                .Include(c => c.Cliente)
                .Where(c => c.Saldo > 0 && c.Vencimiento <= hoy.AddDays(7))
                .OrderBy(c => c.Vencimiento)
                .Take(3)
                .Select(c => new DashboardCuentaPorCobrarItem(c.Cliente.Nombre, c.Saldo, c.Vencimiento))
                .ToListAsync();

            var alertas = new List<DashboardAlertaItem>();

            var productoCritico = await _context.Producto
                .AsNoTracking()
                .Where(p => p.Activo && p.Stock <= p.StockMinimo)
                .OrderBy(p => p.Stock)
                .FirstOrDefaultAsync();

            if (productoCritico is not null)
            {
                alertas.Add(new DashboardAlertaItem(
                    "danger",
                    $"Stock crítico: {productoCritico.Nombre}",
                    $"{productoCritico.Stock} unidades · mínimo {productoCritico.StockMinimo}"));
            }

            var cxcVencida = await _context.CuentasPorCobrar
                .AsNoTracking()
                .Include(c => c.Cliente)
                .Where(c => c.Saldo > 0 && c.Vencimiento.Date < hoy)
                .OrderBy(c => c.Vencimiento)
                .FirstOrDefaultAsync();

            if (cxcVencida is not null)
            {
                var dias = (hoy - cxcVencida.Vencimiento.Date).Days;
                alertas.Add(new DashboardAlertaItem(
                    "danger",
                    $"CxC vencida: {cxcVencida.Cliente.Nombre} {cxcVencida.Saldo:C}",
                    $"Vencida hace {dias} días"));
            }

            var cxcProxima = await _context.CuentasPorCobrar
                .AsNoTracking()
                .Include(c => c.Cliente)
                .Where(c => c.Saldo > 0 && c.Vencimiento.Date >= hoy && c.Vencimiento.Date <= hoy.AddDays(2))
                .OrderBy(c => c.Vencimiento)
                .FirstOrDefaultAsync();

            if (cxcProxima is not null)
            {
                var dias = (cxcProxima.Vencimiento.Date - hoy).Days;
                alertas.Add(new DashboardAlertaItem(
                    "warning",
                    $"CxC próxima a vencer: {cxcProxima.Cliente.Nombre}",
                    $"{cxcProxima.Saldo:C} · vence en {dias} días"));
            }

            var vehiculosListos = await _context.Orden
                .AsNoTracking()
                .CountAsync(o => o.Estado == "Lista" && o.FechaEntregaReal == null);

            if (vehiculosListos > 0)
            {
                alertas.Add(new DashboardAlertaItem(
                    "info",
                    $"{vehiculosListos} vehículos listos para entregar",
                    "Clientes sin notificar"));
            }

            return new AdminDashboardResponse(
                facturadoHoy,
                facturadoAyer,
                facturadoMes,
                facturadoMesAnterior,
                cxcPendiente,
                cuentasVencidas,
                ordenesActivas,
                ordenesUrgentes,
                alertas,
                serviciosMasSolicitados,
                estadoOrdenes,
                cxcProximas);
        }

        public async Task<TecnicoDashboardResponse> ObtenerDashboardTecnicoAsync(int? tecnicoId)
        {
            var hoy = DateTime.Today;
            var manana = hoy.AddDays(1);

            var ordenesActivas = _context.Orden
                .AsNoTracking()
                .Where(o => o.FechaEntregaReal == null && o.Estado != "Entregada" && o.Estado != "Cancelada");

            if (tecnicoId.HasValue)
            {
                ordenesActivas = ordenesActivas.Where(o => o.TecnicoId == tecnicoId.Value);
            }

            var misOrdenesHoy = await ordenesActivas
                .CountAsync(o => o.FechaCreacion >= hoy && o.FechaCreacion < manana);

            var ordenesUrgentesHoy = await ordenesActivas
                .CountAsync(o => o.FechaCreacion >= hoy
                    && o.FechaCreacion < manana
                    && o.Prioridad.ToLower().Contains("urg"));

            var enProgresoAhora = await ordenesActivas
                .CountAsync(o => o.Estado.ToLower().Contains("diagn") || o.Estado.ToLower().Contains("repar"));

            var completadasQuery = _context.Orden
                .AsNoTracking()
                .Where(o => o.FechaEntregaReal >= hoy && o.FechaEntregaReal < manana);

            if (tecnicoId.HasValue)
            {
                completadasQuery = completadasQuery.Where(o => o.TecnicoId == tecnicoId.Value);
            }

            var completadasHoy = await completadasQuery.CountAsync();

            var colaRaw = await ordenesActivas
                .OrderByDescending(o => o.Prioridad.ToLower().Contains("urg"))
                .ThenBy(o => o.FechaCreacion)
                .Take(8)
                .Select(o => new
                {
                    o.Id,
                    o.Estado,
                    Cliente = o.Cliente.Nombre,
                    o.Notas,
                    Vehiculo = o.Vehiculo.Marca + " " + o.Vehiculo.Modelo,
                    o.Vehiculo.Placa,
                    o.FechaCreacion,
                    o.FechaEntregaEstima,
                    EsUrgente = o.Prioridad.ToLower().Contains("urg")
                })
                .ToListAsync();

            var colaTrabajo = colaRaw
                .Select(x => new TecnicoColaTrabajoItem(
                    x.Id,
                    x.Estado,
                    x.Cliente,
                    x.Vehiculo,
                    x.Placa,
                    string.IsNullOrWhiteSpace(x.Notas) ? "Sin detalle registrado" : x.Notas,
                    x.FechaCreacion,
                    x.FechaEntregaEstima,
                    x.EsUrgente))
                .ToList();

            var completadasRaw = await completadasQuery
                .OrderByDescending(o => o.FechaEntregaReal)
                .Take(6)
                .Select(o => new
                {
                    o.Id,
                    Cliente = o.Cliente.Nombre,
                    Vehiculo = o.Vehiculo.Marca + " " + o.Vehiculo.Modelo,
                    o.Vehiculo.Placa,
                    o.Notas,
                    FechaCompletada = o.FechaEntregaReal
                })
                .ToListAsync();

            var completadas = completadasRaw
                .Where(x => x.FechaCompletada.HasValue)
                .Select(x => new TecnicoCompletadaItem(
                    x.Id,
                    x.Cliente,
                    x.Vehiculo,
                    x.Placa,
                    string.IsNullOrWhiteSpace(x.Notas) ? "Orden completada" : x.Notas,
                    x.FechaCompletada!.Value))
                .ToList();

            return new TecnicoDashboardResponse(
                misOrdenesHoy,
                ordenesUrgentesHoy,
                enProgresoAhora,
                completadasHoy,
                colaTrabajo,
                completadas);
        }

        public async Task<MantenimientoDashboardResponse> ObtenerDashboardMantenimientoAsync()
        {
            var hoy = DateTime.Today;
            var manana = hoy.AddDays(1);

            var ordenesHoyQuery = _context.Orden
                .AsNoTracking()
                .Where(o => o.FechaCreacion >= hoy && o.FechaCreacion < manana);

            var ordenesHoy = await ordenesHoyQuery.CountAsync();

            var ordenesUrgentesHoy = await ordenesHoyQuery
                .CountAsync(o => o.Prioridad.ToLower().Contains("urg"));

            var ordenesListasHoy = await ordenesHoyQuery
                .CountAsync(o => o.Estado.ToLower().Contains("lista"));

            var pendientesAprobar = await _context.Orden
                .AsNoTracking()
                .CountAsync(o => o.FechaEntregaReal == null
                    && (o.Estado.ToLower().Contains("cotiz") || o.Estado.ToLower().Contains("aprob")));

            var listasSinEntregar = await _context.Orden
                .AsNoTracking()
                .CountAsync(o => o.FechaEntregaReal == null && o.Estado.ToLower().Contains("lista"));

            var requierenAccionRaw = await _context.Orden
                .AsNoTracking()
                .Where(o => o.FechaEntregaReal == null
                    && (o.Estado.ToLower().Contains("lista") || o.Estado.ToLower().Contains("cotiz") || o.Estado.ToLower().Contains("aprob")))
                .OrderByDescending(o => o.Estado.ToLower().Contains("lista"))
                .ThenBy(o => o.FechaCreacion)
                .Take(6)
                .Select(o => new
                {
                    o.Id,
                    o.Estado,
                    Cliente = o.Cliente.Nombre,
                    VehiculoPlaca = o.Vehiculo.Placa,
                    o.Notas,
                    FechaReferencia = o.FechaEntregaEstima ?? o.FechaCreacion
                })
                .ToListAsync();

            var requierenAccion = requierenAccionRaw
                .Select(o => new MantenimientoAccionItem(
                    o.Id,
                    o.Estado,
                    o.Cliente,
                    o.VehiculoPlaca,
                    string.IsNullOrWhiteSpace(o.Notas) ? "Requiere seguimiento" : o.Notas,
                    o.FechaReferencia))
                .ToList();

            var ordenesHoyDetalleRaw = await ordenesHoyQuery
                .OrderByDescending(o => o.Prioridad.ToLower().Contains("urg"))
                .ThenBy(o => o.FechaCreacion)
                .Take(10)
                .Select(o => new
                {
                    o.Id,
                    o.Estado,
                    Cliente = o.Cliente.Nombre,
                    VehiculoPlaca = o.Vehiculo.Placa,
                    o.Prioridad
                })
                .ToListAsync();

            var ordenesHoyDetalle = ordenesHoyDetalleRaw
                .Select(o => new MantenimientoOrdenItem(
                    o.Id,
                    o.Estado,
                    o.Cliente,
                    o.VehiculoPlaca,
                    EstadoDetalle(o.Estado),
                    o.Prioridad.ToLower().Contains("urg")))
                .ToList();

            return new MantenimientoDashboardResponse(
                ordenesHoy,
                ordenesUrgentesHoy,
                ordenesListasHoy,
                pendientesAprobar,
                listasSinEntregar,
                requierenAccion,
                ordenesHoyDetalle);
        }

        private static string EstadoDetalle(string estado)
        {
            var e = estado.Trim().ToLowerInvariant();
            if (e.Contains("repar")) return "En reparación";
            if (e.Contains("diagn")) return "En diagnóstico";
            if (e.Contains("aprob")) return "Aprobado";
            if (e.Contains("lista")) return "Lista";
            if (e.Contains("recib")) return "Recibido";
            if (e.Contains("cotiz")) return "Cotización";
            return estado;
        }
    }
}
