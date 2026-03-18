using FrenosCore.Data;
using FrenosCore.Modelos.Dtos.Servicio;
using FrenosCore.Modelos.Entidades;
using Microsoft.EntityFrameworkCore;


namespace FrenosCore.Servicios
{
    public class ServiciosService : IServiciciosService
    {
        public readonly AppDbContext _context;

        public ServiciosService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServicioResponse> CrearAsync(CrearServicioRequest request)
        {
            var servicio = new Servicio
            {
                Nombre = request.Nombre.Trim(),
                Descripcion = request.Descripcion.Trim(),
                Precio = request.Precio,
                DuracionMinutos = request.DuracionMinutos,
                Categoria = request.Categoria.Trim(),
                Activo = request.Activo,
                CreadoEn = DateTime.Now
            };

            _context.Servicio.Add(servicio);
            await _context.SaveChangesAsync();

            return ToResponse(servicio);
        }
        public async Task<IEnumerable<ServicioResponse>> ListarAsync()
        {
            var query = _context.Servicio
                .AsNoTracking()
                .Where(s => s.Activo);
            return await query
                .OrderBy(s => s.Categoria)
                .ThenBy(s => s.Nombre)
                .Select(s => ToResponse(s))
                .ToListAsync();

        }

        public async Task<IEnumerable<ServicioResponse>> BuscarAsync(string? termino)
        {
            if (string.IsNullOrWhiteSpace(termino))
                return [];

            var t = termino.Trim().ToLower();

            var query = _context.Servicio
                .AsNoTracking()
                .Where(s => s.Activo)
                .Where(s => s.Nombre.Contains(t) || s.Descripcion.Contains(t) || s.Categoria.Contains(t));

            return await query
                .OrderBy(s => s.Categoria)
                .ThenBy(s => s.Nombre)
                .Select(s => ToResponse(s))
                .ToListAsync();

        }
        public async Task<ServicioResponse> ObtenerPorIdAsync(int id)
        {
            var servicio = await _context.Servicio
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id && s.Activo);

            return servicio == null ? throw new KeyNotFoundException($"Servicio con ID {id} no encontrado.") : ToResponse(servicio);
        }
        public async Task<ServicioResponse> ActualizarAsync(int id, ActualizarServicioRequest request)
        {
            var servicio = await _context.Servicio
                .FirstOrDefaultAsync(s => s.Id == id && s.Activo)
                ?? throw new KeyNotFoundException($"Servicio con ID {id} no encontrado.");

            servicio.Nombre = request.Nombre.Trim();
            servicio.Descripcion = request.Descripcion.Trim();
            servicio.Precio = request.Precio;
            servicio.DuracionMinutos = request.DuracionMinutos;
            servicio.Categoria = request.Categoria.Trim();
            servicio.Activo = request.Activo;

            await _context.SaveChangesAsync();

            return ToResponse(servicio);
        }
        public async Task<bool> EliminarAsync(int id)
        {
            var servicio = await _context.Servicio
                .FirstOrDefaultAsync(s => s.Id == id && s.Activo)
                ?? throw new KeyNotFoundException($"Servicio con ID {id} no encontrado.");

            servicio.Activo = false;
            await _context.SaveChangesAsync();

            return true;
        }

        public static ServicioResponse ToResponse(Servicio servicio)
        {
            return new ServicioResponse(
                servicio.Id,
                servicio.Nombre,
                servicio.Descripcion,
                servicio.Precio,
                servicio.DuracionMinutos,
                servicio.Categoria,
                servicio.Activo,
                servicio.CreadoEn
            );
        }
    }
}
