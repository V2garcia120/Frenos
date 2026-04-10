using FrenosCore.Data;
using FrenosCore.Helpers;
using FrenosCore.Modelos.Dtos.Log;
using FrenosCore.Modelos.Dtos.Servicio;
using FrenosCore.Modelos.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;


namespace FrenosCore.Servicios
{
    public class ServiciosService : IServiciciosService
    {
        public readonly AppDbContext _context;
        private readonly IAudtiLog _auditLog;
        private readonly IUsuarioActualService _usuarioActual;
        private readonly ILogger<ServiciosService> _logger;

        public ServiciosService(AppDbContext context, IAudtiLog auditLog, IUsuarioActualService usuarioActual, ILogger<ServiciosService> logger)
        {
            _context = context;
            _auditLog = auditLog;
            _usuarioActual = usuarioActual;
            _logger = logger;
        }

        public async Task<ServicioResponse> CrearAsync(CrearServicioRequest request)
        {
            _logger.LogInformation("Creando servicio: {Nombre}", request.Nombre);

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

            _logger.LogInformation("Servicio creado: {ServicioId}", servicio.Id);
            await RegistrarAuditoriaAsync(servicio.Id, "Crear", "Servicio", string.Empty, JsonSerializer.Serialize(ToResponse(servicio)));

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

            var antes = JsonSerializer.Serialize(ToResponse(servicio));

            servicio.Nombre = request.Nombre.Trim();
            servicio.Descripcion = request.Descripcion.Trim();
            servicio.Precio = request.Precio;
            servicio.DuracionMinutos = request.DuracionMinutos;
            servicio.Categoria = request.Categoria.Trim();
            servicio.Activo = request.Activo;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Servicio actualizado: {ServicioId}", id);
            await RegistrarAuditoriaAsync(id, "Actualizar", "Servicio", antes, JsonSerializer.Serialize(ToResponse(servicio)));

            return ToResponse(servicio);
        }
        public async Task<bool> EliminarAsync(int id)
        {
            var servicio = await _context.Servicio
                .FirstOrDefaultAsync(s => s.Id == id && s.Activo)
                ?? throw new KeyNotFoundException($"Servicio con ID {id} no encontrado.");

            var antes = JsonSerializer.Serialize(ToResponse(servicio));

            servicio.Activo = false;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Servicio desactivado: {ServicioId}", id);
            await RegistrarAuditoriaAsync(id, "Eliminar", "Servicio", antes, JsonSerializer.Serialize(ToResponse(servicio)));

            return true;
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
