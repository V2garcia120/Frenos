using FrenosCore.Data;
using FrenosCore.Helpers;
using FrenosCore.Modelos.Dtos;
using FrenosCore.Modelos.Dtos.Log;
using FrenosCore.Modelos.Dtos.Producto;
using FrenosCore.Modelos.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace FrenosCore.Servicios
{
    public class ProductoService : IProductoService
    {
        private readonly AppDbContext _context;
        private readonly IAudtiLog _auditLog;
        private readonly IUsuarioActualService _usuarioActual;
        private readonly ILogger<ProductoService> _logger;

        public ProductoService(AppDbContext context, IAudtiLog auditLog, IUsuarioActualService usuarioActual, ILogger<ProductoService> logger)
        {
            _context = context;
            _auditLog = auditLog;
            _usuarioActual = usuarioActual;
            _logger = logger;
        }
        public async Task<ProductoResponse> CrearProductoAsync(CrearProductoRequest request)
        {
            _logger.LogInformation("Creando producto: {Nombre}", request.Nombre);

            var producto = new Producto
            {
                Nombre = request.Nombre.Trim(),
                Descripcion = request.Descripcion.Trim(),
                Precio = request.Precio,
                Costo = request.Costo,
                Stock = request.Stock,
                StockMinimo = request.StockMinimo,
                Categoria = request.Categoria.Trim(),
                Activo = request.Activo,
                CreadoEn = DateTime.Now
            };

            _context.Producto.Add(producto);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Producto creado: {ProductoId}", producto.Id);
            await RegistrarAuditoriaAsync(producto.Id, "Crear", "Producto", string.Empty, JsonSerializer.Serialize(ToResponse(producto)));

            return ToResponse(producto);
        }
        public async Task<ProductoResponse> ObtenerProductoPorIdAsync(int id)
        {
            var producto = await _context.Producto
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id && p.Activo);

            return producto == null ? throw new KeyNotFoundException($"Producto con ID {id} no encontrado.") : ToResponse(producto);
        }
        public async Task<IEnumerable<ProductoResponse>> ListarTodosAsync(string? categoria)
        {
            var query = _context.Producto
                .AsNoTracking()
                .Where(p => p.Activo);

            if (!string.IsNullOrWhiteSpace(categoria))
                query = query.Where(p => p.Categoria == categoria);

            return await query
                .OrderBy(p => p.Categoria)
                .ThenBy(p => p.Nombre)
                .Select(p => ToResponse(p))
                .ToListAsync();
        }
        public async Task<IEnumerable<ProductoResponse>> BuscarAsync(string? termino)
        {
            if (string.IsNullOrWhiteSpace(termino))
                return [];

            var t = termino.Trim();

            var query = _context.Producto
                .AsNoTracking()
                .Where(p => p.Activo)
                .Where(p => p.Nombre.Contains(t) || p.Descripcion.Contains(t));

            return await query
                .OrderBy(p => p.Categoria)
                .ThenBy(p => p.Nombre)
                .Select(p => ToResponse(p))
                .ToListAsync();
        }
        public async Task<ProductoResponse> ActualizarProductoAsync(int id, ActualizarProductoRequest request)
        {
            var producto = await _context.Producto.FirstOrDefaultAsync(p => p.Id == id && p.Activo)
                ?? throw new KeyNotFoundException($"Producto con ID {id} no encontrado.");

            var antes = JsonSerializer.Serialize(ToResponse(producto));

            if (request.Nombre is not null) producto.Nombre = request.Nombre.Trim();
            if (request.Descripcion is not null) producto.Descripcion = request.Descripcion.Trim();
            if (request.Precio.HasValue) producto.Precio = request.Precio.Value;
            if (request.Costo.HasValue) producto.Costo = request.Costo.Value;
            if (request.Stock.HasValue) producto.Stock = request.Stock.Value;
            if (request.StockMinimo.HasValue) producto.StockMinimo = request.StockMinimo.Value;
            if (request.Categoria is not null) producto.Categoria = request.Categoria.Trim();
            if (request.Activo.HasValue) producto.Activo = request.Activo.Value;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Producto actualizado: {ProductoId}", id);
            await RegistrarAuditoriaAsync(id, "Actualizar", "Producto", antes, JsonSerializer.Serialize(ToResponse(producto)));

            return ToResponse(producto);
        }
        public async Task EliminarProductoAsync(int id)
        {
            var producto = await _context.Producto.FirstOrDefaultAsync(p => p.Id == id && p.Activo)
                ?? throw new KeyNotFoundException($"Producto con ID {id} no encontrado.");

            var antes = JsonSerializer.Serialize(ToResponse(producto));

            producto.Activo = false;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Producto desactivado: {ProductoId}", id);
            await RegistrarAuditoriaAsync(id, "Eliminar", "Producto", antes, JsonSerializer.Serialize(ToResponse(producto)));
        }

        private async Task RegistrarAuditoriaAsync(int registroId, string accion, string tabla, string valorAntes, string valorDespues)
        {
            try
            {
                await _auditLog.RegistrarAsync(new AuditEntry(
                    UsuarioId: _usuarioActual.Id,
                    ResgistroId: registroId,
                    Accion: accion,
                    Tabla: tabla,
                    Ip: _usuarioActual.Ip,
                    ValorAntes: valorAntes,
                    ValorDespues: valorDespues));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo registrar auditoría en {Tabla} para registro {RegistroId}", tabla, registroId);
            }
        }
        public static ProductoResponse ToResponse(Modelos.Entidades.Producto producto)
        {
            return new ProductoResponse(
                Id: producto.Id,
                Nombre: producto.Nombre,
                Descripcion: producto.Descripcion,
                Precio: producto.Precio,
                Costo: producto.Costo,
                Stock: producto.Stock,
                StockMinimo: producto.StockMinimo,
                Categoria: producto.Categoria,
                Activo: producto.Activo,
                CreadoEn: producto.CreadoEn
            );
        }
    }
}
