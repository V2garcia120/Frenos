using FrenosIntegracion.Data;
using FrenosIntegracion.Models.DTOs;
using FrenosIntegracion.Services.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FrenosIntegracion.Services.Cache
{
    public class CacheService(IntegracionDbContext db, ICoreService core) : ICacheService
    {
        public DateTime UltimaActualizacion { get; private set; } = DateTime.MinValue;
        public async Task<IEnumerable<ServicioDto>> ObtenerServiciosAsync()
        {
            try
            {
                var servicios = await core.ObtenerServiciosAsync();
                await ActualizarServiciosCacheAsync(servicios);
                return servicios;
            }
            catch
            {
                return await db.ServiciosCache
                    .AsNoTracking()
                    .Where(s => s.Activo)
                    .Select(s => new ServicioDto(
                        s.Id, s.Nombre, s.Precio, s.DuracionMin, s.Categoria, s.Activo))
                    .ToListAsync();
            }
        }

        // 2. Lógica para guardar productos en la BD local
        private async Task ActualizarProductosCacheAsync(IEnumerable<ProductoDto> productos)
        {
            // Limpiamos lo viejo para no tener datos duplicados o desactualizados
            var viejos = await db.ProductosCache.ToListAsync();
            db.ProductosCache.RemoveRange(viejos);

            var nuevos = productos.Select(p => new Models.Entities.ProductoCache
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Precio = p.Precio,
                Stock = p.Stock,
                Categoria = p.Categoria,
                Activo = p.Activo,
                UltimaActualizacion = DateTime.UtcNow
            });

            await db.ProductosCache.AddRangeAsync(nuevos);
            await db.SaveChangesAsync();
        }

        // 3. Lógica para guardar servicios en la BD local
        private async Task ActualizarServiciosCacheAsync(IEnumerable<ServicioDto> servicios)
        {
            var viejos = await db.ServiciosCache.ToListAsync();
            db.ServiciosCache.RemoveRange(viejos);

            var nuevos = servicios.Select(s => new Models.Entities.ServicioCache
            {
                Id = s.Id,
                Nombre = s.Nombre,
                Precio = s.Precio,
                DuracionMin = s.DuracionMin,
                Categoria = s.Categoria,
                Activo = s.Activo,
                UltimaActualizacion = DateTime.UtcNow
            });

            await db.ServiciosCache.AddRangeAsync(nuevos);
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductoDto>> ObtenerProductosAsync()
        {
            try
            {
                var productos = await core.ObtenerProductosAsync();
                await ActualizarProductosCacheAsync(productos);
                return productos;
            }
            catch
            {
                // Core no disponible — responder con caché local
                return await db.ProductosCache
                    .AsNoTracking()
                    .Where(p => p.Activo)
                    .Select(p => new ProductoDto(
                        p.Id, p.Nombre, p.Precio, p.Stock, p.Categoria, p.Activo))
                    .ToListAsync();
            }
        }

        public async Task RefrescarAsync()
        {
            try
            {
                var productos = await core.ObtenerProductosAsync();
                await ActualizarProductosCacheAsync(productos);
                var servicios = await core.ObtenerServiciosAsync();
                await ActualizarServiciosCacheAsync(servicios);
                UltimaActualizacion = DateTime.UtcNow;
            }
            catch { /* El caché anterior sigue siendo válido */ }
        }
    }
}
