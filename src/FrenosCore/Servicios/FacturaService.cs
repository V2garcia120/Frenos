using FrenosCore.Data;
using FrenosCore.Helpers;
using FrenosCore.Modelos.Dtos;
using FrenosCore.Modelos.Dtos.Factura;
using FrenosCore.Modelos.Dtos.Log;
using FrenosCore.Modelos.Entidades;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json;


namespace FrenosCore.Servicios;

public class FacturaService(
    AppDbContext db,
    IAudtiLog auditLog,
    IUsuarioActualService usuarioActual,
    ILogger<FacturaService> logger) : IFacturaService
{
    private const decimal TasaItbis = 0.18m;

    public async Task<PaginadoResponse<FacturaResponse>> ListarAsync(
        int pagina, int tam, string? estado, string? numero, DateTime? fecha, string? tipoOrigen)
    {
        pagina = Math.Max(1, pagina);
        tam = Math.Clamp(tam, 1, 100);

        var query = db.Factura
            .AsNoTracking()
            .Include(f => f.Cliente)
            .Include(f => f.Orden)
                .ThenInclude(o => o.Vehiculo)
            .Include(f => f.EmitidaPorUsuario)
            .Include(f => f.Items)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(estado))
            query = query.Where(f => f.Estado == estado);

        if (!string.IsNullOrWhiteSpace(numero))
        {
            var n = numero.Trim().ToLower();
            query = query.Where(f => f.Numero.ToLower().Contains(n));
        }

        if (fecha.HasValue)
        {
            var inicio = fecha.Value.Date;
            var fin = inicio.AddDays(1);
            query = query.Where(f => f.Fecha >= inicio && f.Fecha < fin);
        }

        if (!string.IsNullOrWhiteSpace(tipoOrigen))
            query = query.Where(f => f.TipoOrigen == tipoOrigen);

        var totalItems = await query.CountAsync();

        var items = await query
            .OrderByDescending(f => f.Fecha)
            .Skip((pagina - 1) * tam)
            .Take(tam)
            .ToListAsync();

        return new PaginadoResponse<FacturaResponse>(
            Items: items.Select(ToResponse),
            PaginaActual: pagina,
            TamPagina: tam,
            TotalItems: totalItems,
            TotalPaginas: (int)Math.Ceiling(totalItems / (double)tam));
    }


    public async Task<FacturaResponse> ObtenerPorIdAsync(int id)
    {
        var factura = await db.Factura
            .AsNoTracking()
            .Include(f => f.Cliente)
            .Include(f => f.Orden)
                .ThenInclude(o => o.Vehiculo)
            .Include(f => f.EmitidaPorUsuario)
            .Include(f => f.Items)
            .FirstOrDefaultAsync(f => f.Id == id)
            ?? throw new KeyNotFoundException($"Factura {id} no encontrada.");

        return ToResponse(factura);
    }

    public async Task<FacturaPendienteDto> ObtenerFacturaPendientesAsync(string? placa = null, string? numeroFactura = null) 
    {
        if (string.IsNullOrWhiteSpace(placa) && string.IsNullOrWhiteSpace(numeroFactura))
            throw new ArgumentException("Se requiere placa o número de factura para buscar facturas pendientes.");
        Log.Information($"Buscando facturas pendientes con placa='{placa}' o numeroFactura='{numeroFactura}'");
        var facturas = await db.Factura
        .Where(f => f.Estado == "Pendiente" &&
            (f.Orden.Vehiculo.Placa == placa || f.Numero == numeroFactura))
        .Select(f => new
        {
            f.Id,
            f.Numero,
            Cliente = f.Cliente.Nombre,
            Vehiculo = f.Orden.Vehiculo.Marca + " " + f.Orden.Vehiculo.Modelo + " " + f.Orden.Vehiculo.Anno + " · " + f.Orden.Vehiculo.Placa, 
            f.Total,
            f.Estado,
            
            Items = f.Items.Select(i => new
            {
                Nombre = i.Tipo == "Producto"
                    ? db.Producto
                        .Where(p => p.Id == i.ItemId)
                        .Select(p => p.Nombre)
                        .FirstOrDefault()
                    : db.Servicio
                        .Where(s => s.Id == i.ItemId)
                        .Select(s => s.Nombre)
                        .FirstOrDefault(),
                Precio = i.PrecioUnitario,
                i.Cantidad,
                i.Subtotal,
                
            }).ToList()
        })
        .ToListAsync();
        if (facturas.Count == 0 || facturas.All(f => f == null))
            throw new KeyNotFoundException("No se encontraron facturas pendientes con los criterios proporcionados.");
        return facturas.Select(f => new FacturaPendienteDto(f.Id, f.Numero, f.Cliente, f.Vehiculo, f.Total, f.Estado, f.Items.Select(i => new ItemFacturaDto(Convert.ToString(i.Nombre), i.Cantidad, i.Precio, i.Subtotal)).ToList())).FirstOrDefault();

    }
    public async Task<FacturaResponse> GenerarDesdeOrdenAsync(int ordenId, int emisorId, string? metodoPago)
    {
        logger.LogInformation("Generando factura desde orden {OrdenId}", ordenId);
   
        var orden = await db.Orden
            .Include(o => o.Cliente)
            .Include(o => o.Vehiculo)
            .Include(o => o.Cotizacion)
                .ThenInclude(c => c!.Items)
            .FirstOrDefaultAsync(o => o.Id == ordenId)
            ?? throw new KeyNotFoundException($"Orden {ordenId} no encontrada.");


        if (orden.Cotizacion is null)
            throw new InvalidOperationException(
                $"La orden {ordenId} no tiene cotización asociada. " +
                "No se puede generar la factura.");

        if (orden.Cotizacion.Estado != "Aprobada")
            throw new InvalidOperationException(
                $"La cotización de la orden {ordenId} no está aprobada. " +
                "El cliente debe aprobar la cotización antes de generar la factura.");

        var facturaExistente = await db.Factura.AnyAsync(f => f.OrdenId == ordenId);
        if (facturaExistente)
            throw new InvalidOperationException(
                $"La orden {ordenId} ya tiene una factura generada.");

 
        var numero = await GenerarNumeroFacturaAsync();


        var items = orden.Cotizacion.Items.Select(ci => new FacturaItem
        {
            Tipo = ci.Tipo,
            ItemId = ci.ItemId,
            Descripcion = ci.Descripcion,
            Cantidad = ci.Cantidad,
            PrecioUnitario = ci.PrecioUnitario,
            Subtotal = ci.Subtotal
        }).ToList();

        var subtotal = items.Sum(i => i.Subtotal);
        var itbis = Math.Round(subtotal * TasaItbis, 2);

        var factura = new Factura
        {
            OrdenId = ordenId,
            TipoOrigen = "OrdenReparacion",
            ClienteId = orden.ClienteId,
            EmitidaPor = emisorId,
            Numero = numero,
            Fecha = DateTime.UtcNow,
            Subtotal = subtotal,
            Itbis = itbis,
            Total = subtotal + itbis,
            Estado = "Pendiente",
            MetodoPago = metodoPago ?? "pendiente",
            Items = items
        };

        db.Factura.Add(factura);

 
        await DescontarStockAsync(items);

        await db.SaveChangesAsync();

   
        await db.Entry(factura).Reference(f => f.Cliente).LoadAsync();
        await db.Entry(factura).Reference(f => f.EmitidaPorUsuario).LoadAsync();

        await RegistrarAuditoriaAsync(
            factura.Id,
            "Crear",
            "Factura",
            string.Empty,
            JsonSerializer.Serialize(new
            {
                factura.Id,
                factura.OrdenId,
                factura.Numero,
                factura.Estado,
                factura.Total
            }));

        logger.LogInformation("Factura generada {FacturaId} para orden {OrdenId}", factura.Id, ordenId);

        return ToResponse(factura, orden);
    }

  
    public async Task<CobroDirectoResponse> CobrarDirectoAsync(CobroDirectoRequest request)
    {
        string[] metodosValidos = ["Efectivo", "Tarjeta", "Transferencia", "Credito"];
        if (!metodosValidos.Contains(request.MetodoPago))
            throw new ArgumentException($"Método de pago '{request.MetodoPago}' no válido.");

        var turno = await db.TurnoCaja.FindAsync(request.TurnoId)
            ?? throw new ArgumentException($"Turno {request.TurnoId} no encontrado.");

        if (!await db.Cliente.AnyAsync(c => c.Id == request.ClienteId))
            throw new ArgumentException($"Cliente {request.ClienteId} no encontrado.");

        var items = new List<FacturaItem>();
        foreach (var i in request.Items)
        {
            var descripcion = i.Tipo == "Producto"
                ? await db.Producto.Where(p => p.Id == i.ItemId).Select(p => p.Nombre).FirstOrDefaultAsync()
                : await db.Servicio.Where(s => s.Id == i.ItemId).Select(s => s.Nombre).FirstOrDefaultAsync();

            items.Add(new FacturaItem
            {
                Tipo = i.Tipo,
                ItemId = i.ItemId,
                Descripcion = descripcion ?? $"{i.Tipo} #{i.ItemId}",
                Cantidad = i.Cantidad,
                PrecioUnitario = i.PrecioSnapshot,
                Subtotal = i.PrecioSnapshot * i.Cantidad
            });
        }

        var subtotal = items.Sum(i => i.Subtotal);
        var itbis = Math.Round(subtotal * TasaItbis, 2);
        var total = subtotal + itbis;

        if (request.MetodoPago == "Efectivo" && request.MontoPagado < total)
            throw new InvalidOperationException(
                $"Monto pagado ({request.MontoPagado:N2}) insuficiente. Total: {total:N2}.");

        var numero = await GenerarNumeroFacturaAsync();

        var factura = new Factura
        {
            TipoOrigen = "VentaDirecta",
            ClienteId = request.ClienteId,
            TurnoId = request.TurnoId,
            EmitidaPor = turno.CajeroId,
            Numero = numero,
            Fecha = DateTime.UtcNow,
            Subtotal = subtotal,
            Itbis = itbis,
            Total = total,
            Estado = request.MetodoPago == "Credito" ? "Pendiente" : "Pagada",
            MetodoPago = request.MetodoPago,
            Items = items
        };

        db.Factura.Add(factura);
        await DescontarStockAsync(items);
        await db.SaveChangesAsync();

        if (request.MetodoPago == "Credito")
        {
            db.CuentasPorCobrar.Add(new CuentasPorCobrar
            {
                ClienteId = factura.ClienteId,
                FacturaId = factura.Id,
                Monto = total,
                Saldo = total,
                Vencimiento = DateTime.UtcNow.AddDays(30),
                Estado = "Pendiente",
                CreadoEn = DateTime.UtcNow
            });
            await db.SaveChangesAsync();
        }

        await RegistrarAuditoriaAsync(
            factura.Id, "CobrarDirecto", "Factura", string.Empty,
            JsonSerializer.Serialize(new { factura.Id, factura.Numero, factura.Total, factura.MetodoPago }));

        var cambio = request.MetodoPago == "Efectivo" ? request.MontoPagado - total : 0m;

        return new CobroDirectoResponse(
            FacturaId: factura.Id,
            NumeroFactura: factura.Numero,
            Total: total,
            Cambio: cambio,
            Estado: factura.Estado
        );
    }

    public async Task<FacturaResponse> RegistrarPagoAsync(
        int id, RegistrarPagoRequest req)
    {
        var factura = await db.Factura
            .Include(f => f.Cliente)
            .Include(f => f.Orden)
                .ThenInclude(o => o.Vehiculo)
            .Include(f => f.EmitidaPorUsuario)
            .Include(f => f.Items)
            .FirstOrDefaultAsync(f => f.Id == id)
            ?? throw new KeyNotFoundException($"Factura {id} no encontrada.");

        var antes = JsonSerializer.Serialize(new
        {
            factura.Id,
            factura.Estado,
            factura.MetodoPago,
            factura.Total
        });

        if (factura.Estado != "Pendiente")
            throw new InvalidOperationException(
                $"La factura ya está en estado {factura.Estado}. " +
                "Solo se pueden pagar facturas Pendientes.");

        if (req.Monto < factura.Total)
            throw new InvalidOperationException(
                $"El monto pagado (${req.Monto:N2}) es menor al total " +
                $"de la factura (${factura.Total:N2}).");

        string[] metodosValidos = ["Efectivo", "Tarjeta", "Transferencia", "Credito"];
        if (!metodosValidos.Contains(req.Metodo))
            throw new ArgumentException(
                $"Método de pago '{req.Metodo}' no válido. " +
                $"Valores permitidos: {string.Join(", ", metodosValidos)}.");

        factura.MetodoPago = req.Metodo;
        factura.TurnoId = req.TurnoId;

        if (req.Metodo == "Credito")
        {
            factura.Estado = "Pendiente";

            var existeCxC = await db.CuentasPorCobrar.AnyAsync(c => c.FacturaId == factura.Id);
            if (!existeCxC)
            {
                var cxc = new CuentasPorCobrar
                {
                    ClienteId = factura.ClienteId,
                    FacturaId = factura.Id,
                    Monto = factura.Total,
                    Saldo = factura.Total,
                    Vencimiento = DateTime.UtcNow.AddDays(30),
                    Estado = "Pendiente",
                    CreadoEn = DateTime.UtcNow
                };
                db.CuentasPorCobrar.Add(cxc);
            }
        }
        else
        {
            factura.Estado = "Pagada";

            var cxcAbierta = await db.CuentasPorCobrar
                .FirstOrDefaultAsync(c => c.FacturaId == factura.Id && c.Estado == "Pendiente");
            if (cxcAbierta != null)
            {
                cxcAbierta.Saldo = 0;
                cxcAbierta.Estado = "Pagada";
            }
        }

        await db.SaveChangesAsync();

        await RegistrarAuditoriaAsync(
            factura.Id,
            "RegistrarPago",
            "Factura",
            antes,
            JsonSerializer.Serialize(new
            {
                factura.Id,
                factura.Estado,
                factura.MetodoPago,
                factura.Total
            }));

        logger.LogInformation("Pago registrado en factura {FacturaId} con método {Metodo}", factura.Id, factura.MetodoPago);

        return ToResponse(factura);
    }

    public async Task AnularAsync(int id)
    {
        var factura = await db.Factura
            .Include(f => f.Items)
            .FirstOrDefaultAsync(f => f.Id == id)
            ?? throw new KeyNotFoundException($"Factura {id} no encontrada.");

        var antes = JsonSerializer.Serialize(new
        {
            factura.Id,
            factura.Estado,
            factura.Numero
        });

        if (factura.Estado != "Pendiente")
            throw new InvalidOperationException(
                $"Solo se pueden anular facturas en estado Pendiente. " +
                $"Estado actual: {factura.Estado}.");

  
        var tieneAbonos = await db.AbonoCxC
            .AnyAsync(a => a.CxC.FacturaId == id);

        if (tieneAbonos)
            throw new InvalidOperationException(
                "No se puede anular una factura que tiene abonos registrados.");


        await RevertirStockAsync(factura.Items);

        factura.Estado = "Anulada";
        await db.SaveChangesAsync();

        await RegistrarAuditoriaAsync(
            factura.Id,
            "Anular",
            "Factura",
            antes,
            JsonSerializer.Serialize(new
            {
                factura.Id,
                factura.Estado,
                factura.Numero
            }));

        logger.LogInformation("Factura anulada {FacturaId}", factura.Id);
    }

    public async Task<IEnumerable<FacturaResponse>> ObtenerMisFacturasAsync(int clienteId)
    {
        var lista = await db.Factura
            .AsNoTracking()
            .Include(f => f.Cliente)
            .Include(f => f.Orden).ThenInclude(o => o!.Vehiculo)
            .Include(f => f.EmitidaPorUsuario)
            .Include(f => f.Items)
            .Where(f => f.ClienteId == clienteId)
            .OrderByDescending(f => f.Fecha)
            .ToListAsync();

        return lista.Select(ToResponse);
    }

    public async Task<IEnumerable<object>> ObtenerHistorialClienteAsync(int clienteId)
    {
        var lista = await db.Factura
            .AsNoTracking()
            .Include(f => f.Orden)
                .ThenInclude(o => o!.Vehiculo)
            .Include(f => f.Items)
            .Where(f => f.ClienteId == clienteId)
            .OrderByDescending(f => f.Fecha)
            .ToListAsync();

        return lista.Select(f => (object)new
        {
            NumeroOrden  = f.Numero,
            Fecha        = f.Fecha,
            TipoServicio = ResolverTipoServicio(f),
            Vehiculo     = f.Orden?.Vehiculo is { } v
                             ? $"{v.Marca} {v.Modelo} {v.Anno}"
                             : "N/A",
            Placa        = f.Orden?.Vehiculo?.Placa ?? "N/A",
            Total        = f.Total,
            EstadoServicio = ResolverEstadoServicio(f),
            EstadoPago   = f.Estado,
            EsServicio   = f.TipoOrigen == "OrdenReparacion"
        });
    }

    private static string ResolverTipoServicio(Factura f)
    {
        if (f.TipoOrigen == "OrdenReparacion")
        {
            var svc = f.Items
                .Where(i => i.Tipo == "Servicio")
                .Select(i => i.Descripcion)
                .FirstOrDefault();
            return svc ?? "Servicio de Reparación";
        }
        var descripciones = f.Items.Take(2).Select(i => i.Descripcion);
        return "Venta: " + string.Join(", ", descripciones);
    }

    private static string ResolverEstadoServicio(Factura f)
    {
        if (f.TipoOrigen != "OrdenReparacion") return "Entregado";
        return f.Orden?.Estado switch
        {
            "Recibido"      => "Pendiente",
            "EnDiagnostico" => "En diagnóstico",
            "Aprobado"      => "En diagnóstico",
            "EnReparacion"  => "En reparación",
            "Lista"         => "Listo",
            "Entregada"     => "Entregado",
            _               => "Pendiente"
        };
    }

    private async Task RegistrarAuditoriaAsync(int registroId, string accion, string tabla, string valorAntes, string valorDespues)
    {
        try
        {
            await auditLog.RegistrarAsync(new AuditEntry(
                UsuarioId: usuarioActual.Id,
                ResgistroId: registroId,
                Accion: accion,
                Tabla: tabla,
                Ip: usuarioActual.Ip,
                ValorAntes: valorAntes,
                ValorDespues: valorDespues));
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "No se pudo registrar auditoría en {Tabla} para registro {RegistroId}", tabla, registroId);
        }
    }


    private async Task<string> GenerarNumeroFacturaAsync()
    {
        var anno = DateTime.UtcNow.Year;

        var ultimaFactura = await db.Factura
            .Where(f => f.Fecha.Year == anno)
            .OrderByDescending(f => f.Id)
            .Select(f => f.Numero)
            .FirstOrDefaultAsync();

        int consecutivo = 1;
        if (ultimaFactura is not null)
        {

            var partes = ultimaFactura.Split('-');
            if (partes.Length == 3 && int.TryParse(partes[2], out int ultimo))
                consecutivo = ultimo + 1;
        }

        return $"FAC-{anno}-{consecutivo:D4}";
    }


    private async Task DescontarStockAsync(IEnumerable<FacturaItem> items)
    {
        foreach (var item in items.Where(i => i.Tipo == "Producto"))
        {
            var producto = await db.Producto.FindAsync(item.ItemId);
            if (producto is null) continue;

            if (producto.Stock < item.Cantidad)
                throw new InvalidOperationException(
                    $"Stock insuficiente para '{producto.Nombre}'. " +
                    $"Disponible: {producto.Stock}, requerido: {item.Cantidad}.");

            producto.Stock -= item.Cantidad;
        }
    }

    private async Task RevertirStockAsync(IEnumerable<FacturaItem> items)
    {
        foreach (var item in items.Where(i => i.Tipo == "Producto"))
        {
            var producto = await db.Producto.FindAsync(item.ItemId);
            if (producto is null) continue;
            producto.Stock += item.Cantidad;
        }
    }



    private static FacturaResponse ToResponse(Factura f) => new(
        Id: f.Id,
        OrdenId: f.OrdenId,
        TipoOrigen: f.TipoOrigen,
        ClienteId: f.ClienteId,
        ClienteNombre: f.Cliente?.Nombre ?? string.Empty,
        VehiculoInfo: f.Orden?.Vehiculo is not null
                            ? $"{f.Orden.Vehiculo.Marca} {f.Orden.Vehiculo.Modelo} " +
                              $"{f.Orden.Vehiculo.Anno} · {f.Orden.Vehiculo.Placa}"
                            : string.Empty,
        Numero: f.Numero,
        Fecha: f.Fecha,
        Subtotal: f.Subtotal,
        Itbis: f.Itbis,
        Total: f.Total,
        Estado: f.Estado,
        MetodoPago: f.MetodoPago,
        EmitidaPorNombre: f.EmitidaPorUsuario?.Nombre ?? string.Empty,
        Items: f.Items.Select(i => new FacturaItemResponse(
                             i.Id, i.Tipo, i.Descripcion,
                             i.Cantidad, i.PrecioUnitario, i.Subtotal)));

    private static FacturaResponse ToResponse(Factura f, Orden o) => new(
        Id: f.Id,
        OrdenId: f.OrdenId,
        TipoOrigen: f.TipoOrigen,
        ClienteId: f.ClienteId,
        ClienteNombre: o.Cliente.Nombre,
        VehiculoInfo: $"{o.Vehiculo.Marca} {o.Vehiculo.Modelo} " +
                         $"{o.Vehiculo.Anno} · {o.Vehiculo.Placa}",
        Numero: f.Numero,
        Fecha: f.Fecha,
        Subtotal: f.Subtotal,
        Itbis: f.Itbis,
        Total: f.Total,
        Estado: f.Estado,
        MetodoPago: f.MetodoPago,
        EmitidaPorNombre: f.EmitidaPorUsuario?.Nombre ?? string.Empty,
        Items: f.Items.Select(i => new FacturaItemResponse(
                             i.Id, i.Tipo, i.Descripcion,
                             i.Cantidad, i.PrecioUnitario, i.Subtotal)));
}
