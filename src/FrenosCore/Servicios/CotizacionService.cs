using FrenosCore.Data;
using FrenosCore.Modelos.Dtos.Cotizacion;
using FrenosCore.Modelos.Entidades;
using Microsoft.EntityFrameworkCore;

namespace FrenosCore.Servicios
{
    public class CotizacionService
    {
        private readonly AppDbContext _context;

        public CotizacionService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<CotizacionResponse> ListarAsync(int pagina, int tam)
        {
            throw new NotImplementedException();
        }
        public async Task<CotizacionResponse> ObtenerPorIdAsync(int id)
        {
            var cotizacion = await _context.Cotizacion
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == id)
                ?? throw new KeyNotFoundException($"Cotización {id} no encontrada.");

            return ToResponse(cotizacion);
        }
        public async Task<CotizacionResponse> CrearAsync(CrearCotizacionRequest request)
        {
            throw new NotImplementedException();
        }
        public async Task<CotizacionResponse> ActualizarAsync(int id, ActualizarCotizacionRequest request)
        {
            throw new NotImplementedException();
        }
        public async Task<CotizacionResponse> ActualizarItemAsync(
            int cotizacionId, int itemId, ActualizarCotizacionItemRequest req)
        {
            var cotizacion = await _context.Cotizacion
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == cotizacionId)
                ?? throw new KeyNotFoundException("Cotización no encontrada.");

            if (cotizacion.Estado != "Pendiente")
                throw new InvalidOperationException(
                    "Solo se pueden modificar items de una cotización Pendiente.");

            var item = cotizacion.Items.FirstOrDefault(i => i.Id == itemId)
                ?? throw new KeyNotFoundException("Item no encontrado.");

            item.Cantidad = req.Cantidad;
            item.PrecioUnitario = req.PrecioUnitario;
            item.Subtotal = req.Cantidad * req.PrecioUnitario;

            cotizacion.Subtotal = cotizacion.Items.Sum(i => i.Subtotal);
            cotizacion.Itbis = Math.Round(cotizacion.Subtotal * 0.18m, 2);
            cotizacion.Total = cotizacion.Subtotal + cotizacion.Itbis;

            await _context.SaveChangesAsync();
            return ToResponse(cotizacion);
        }
        public async Task<CotizacionResponse> GenerarDesdeDiagnosticoAsync(int diagnosticoId)
        {
            var diagnostico = await _context.Diagnostico
                .AsNoTracking()
                .Include(d => d.Items)
                .Include(d => d.Orden)
                .FirstOrDefaultAsync(d => d.Id == diagnosticoId)
                ?? throw new KeyNotFoundException($"Diagnóstico {diagnosticoId} no encontrado.");
            
            if (diagnostico.Estado != "Completado") { 
                throw new InvalidOperationException($"Diagnóstico {diagnosticoId} no está completado.");
            }

            var items = new List<CotizacionItem>();

            foreach (var item in diagnostico.Items)
            {
                if (item.ServicioSugeridoId.HasValue)
                {
                    var servicio = await _context.Servicio
                        .FindAsync(item.ServicioSugeridoId.Value);

                    if (servicio is not null)
                        items.Add(new CotizacionItem
                        {
                            Tipo = "Servicio",
                            ItemId = servicio.Id,
                            Descripcion = servicio.Nombre,
                            Cantidad = 1,
                            PrecioUnitario = servicio.Precio,
                            Subtotal = servicio.Precio
                        });
                }

                if (item.ProductoSugeridoId.HasValue)
                {
                    var producto = await _context.Producto
                        .FindAsync(item.ProductoSugeridoId.Value);

                    if (producto is not null)
                        items.Add(new CotizacionItem
                        {
                            Tipo = "Producto",
                            ItemId = producto.Id,
                            Descripcion = producto.Nombre,
                            Cantidad = 1,
                            PrecioUnitario = producto.Precio,
                            Subtotal = producto.Precio
                        });
                }
            }

            var subtotal = items.Sum(i => i.Subtotal);
            var itbis = Math.Round(subtotal * 0.18m, 2);

            var cotizacion = new Cotizacion
            {
                ClienteId = diagnostico.Orden.ClienteId,
                VehiculoId = diagnostico.Orden.VehiculoId,
                Subtotal = subtotal,
                Itbis = itbis,
                Total = subtotal + itbis,
                Estado = "Pendiente",
                ValidaHasta = DateTime.UtcNow.AddDays(3),
                Items = items
            };

            _context.Cotizacion.Add(cotizacion);
            await _context.SaveChangesAsync();

            return ToResponse(cotizacion);
        }
        public async Task AprobarAsync(int id)
        {
            var cotizacion = await _context.Cotizacion.FindAsync(id)
                ?? throw new KeyNotFoundException($"Cotización {id} no encontrada.");

            cotizacion.Estado = "Aprobada";

            await _context.SaveChangesAsync();
        }
        public async Task RechazarAsync(int id)
        {
            var cotizacion = await _context.Cotizacion.FindAsync(id)
                ?? throw new KeyNotFoundException($"Cotización {id} no encontrada.");

            cotizacion.Estado = "Rechazada";
            await _context.SaveChangesAsync();
        }
        public async Task EliminarAsync(int id)
        {
            var cotizacion = await _context.Cotizacion.FindAsync(id)
                ?? throw new KeyNotFoundException($"Cotización {id} no encontrada.");

            _context.Cotizacion.Remove(cotizacion);
            await _context.SaveChangesAsync();
        }
        private static CotizacionResponse ToResponse(Cotizacion c) => new(
            c.Id, c.ClienteId, c.VehiculoId,
            c.Subtotal, c.Itbis, c.Total,
            c.Estado, c.ValidaHasta,
            c.Items.Select(i => new CotizacionItemResponse(
                i.Tipo, i.Descripcion, i.Cantidad, i.PrecioUnitario, i.Subtotal)));
    } 
}

