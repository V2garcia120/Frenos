using FrenosCore.Data;
using FrenosCore.Modelos.Dtos;
using FrenosCore.Modelos.Dtos.Factura;
using FrenosCore.Modelos.Entidades;
using Microsoft.EntityFrameworkCore;


namespace FrenosCore.Servicios;

public class FacturaService(AppDbContext db) : IFacturaService
{
    private const decimal TasaItbis = 0.18m;

    public async Task<PaginadoResponse<FacturaResponse>> ListarAsync(
        int pagina, int tam, string? estado)
    {
        pagina = Math.Max(1, pagina);
        tam = Math.Clamp(tam, 1, 100);

        var query = db.Factura
            .AsNoTracking()
            .Include(f => f.Cliente)
            .Include(f => f.Orden)
                .ThenInclude(o => o.Vehiculo)
            .Include(f => f.EmitidaPor)
            .Include(f => f.Items)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(estado))
            query = query.Where(f => f.Estado == estado);

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
            .Include(f => f.EmitidaPor)
            .Include(f => f.Items)
            .FirstOrDefaultAsync(f => f.Id == id)
            ?? throw new KeyNotFoundException($"Factura {id} no encontrada.");

        return ToResponse(factura);
    }


    public async Task<FacturaResponse> GenerarDesdeOrdenAsync(int ordenId, int emisorId)
    {
   
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
            ClienteId = orden.ClienteId,
            EmitidaPor = emisorId,
            Numero = numero,
            Fecha = DateTime.UtcNow,
            Subtotal = subtotal,
            Itbis = itbis,
            Total = subtotal + itbis,
            Estado = "Pendiente",
            Items = items
        };

        db.Factura.Add(factura);

 
        await DescontarStockAsync(items);

        await db.SaveChangesAsync();

   
        await db.Entry(factura).Reference(f => f.Cliente).LoadAsync();
        await db.Entry(factura).Reference(f => f.EmitidaPorUsuario).LoadAsync();

        return ToResponse(factura, orden);
    }

  
    public async Task<FacturaResponse> RegistrarPagoAsync(
        int id, RegistrarPagoRequest req)
    {
        var factura = await db.Factura
            .Include(f => f.Cliente)
            .Include(f => f.Orden)
                .ThenInclude(o => o.Vehiculo)
            .Include(f => f.EmitidaPor)
            .Include(f => f.Items)
            .FirstOrDefaultAsync(f => f.Id == id)
            ?? throw new KeyNotFoundException($"Factura {id} no encontrada.");

        if (factura.Estado != "Pendiente")
            throw new InvalidOperationException(
                $"La factura ya está en estado {factura.Estado}. " +
                "Solo se pueden pagar facturas Pendientes.");

 
        if (req.MontoPagado < factura.Total)
            throw new InvalidOperationException(
                $"El monto pagado (${req.MontoPagado:N2}) es menor al total " +
                $"de la factura (${factura.Total:N2}).");

        string[] metodosValidos = ["Efectivo", "Tarjeta", "Transferencia", "Credito"];
        if (!metodosValidos.Contains(req.MetodoPago))
            throw new ArgumentException(
                $"Método de pago '{req.MetodoPago}' no válido. " +
                $"Valores permitidos: {string.Join(", ", metodosValidos)}.");

        factura.Estado = "Pagada";
        factura.MetodoPago = req.MetodoPago;

 
        if (req.MetodoPago == "Credito")
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

        await db.SaveChangesAsync();
        return ToResponse(factura);
    }

    public async Task AnularAsync(int id)
    {
        var factura = await db.Factura
            .Include(f => f.Items)
            .FirstOrDefaultAsync(f => f.Id == id)
            ?? throw new KeyNotFoundException($"Factura {id} no encontrada.");

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
