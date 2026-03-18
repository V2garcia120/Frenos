using FrenosCore.Data;
using FrenosCore.Modelos.Dtos;
using FrenosCore.Modelos.Dtos.Producto;
using FrenosCore.Modelos.Entidades;
using Microsoft.EntityFrameworkCore;

namespace FrenosCore.Servicios
{
    public class ProductoService : IProductoService
    {
        private readonly AppDbContext _context;

        public ProductoService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ProductoResponse> CrearProductoAsync(CrearProductoRequest request)
        {
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

            if (request.Nombre is not null) producto.Nombre = request.Nombre.Trim();
            if (request.Descripcion is not null) producto.Descripcion = request.Descripcion.Trim();
            if (request.Precio.HasValue) producto.Precio = request.Precio.Value;
            if (request.Costo.HasValue) producto.Costo = request.Costo.Value;
            if (request.Stock.HasValue) producto.Stock = request.Stock.Value;
            if (request.StockMinimo.HasValue) producto.StockMinimo = request.StockMinimo.Value;
            if (request.Categoria is not null) producto.Categoria = request.Categoria.Trim();
            if (request.Activo.HasValue) producto.Activo = request.Activo.Value;

            await _context.SaveChangesAsync();

            return ToResponse(producto);
        }
        public async Task EliminarProductoAsync(int id)
        {
            var producto = await _context.Producto.FirstOrDefaultAsync(p => p.Id == id && p.Activo)
                ?? throw new KeyNotFoundException($"Producto con ID {id} no encontrado.");

            producto.Activo = false;
            await _context.SaveChangesAsync();
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
