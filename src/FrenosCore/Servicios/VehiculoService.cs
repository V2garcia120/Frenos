using FrenosCore.Data;
using FrenosCore.Helpers;
using FrenosCore.Modelos.Dtos.Log;
using FrenosCore.Modelos.Dtos.Vehiculo;
using FrenosCore.Modelos.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace FrenosCore.Servicios
{
    public class VehiculoService : IVehiculoService
    {
        private readonly AppDbContext _context;
        private readonly IAudtiLog _auditLog;
        private readonly IUsuarioActualService _usuarioActual;
        private readonly ILogger<VehiculoService> _logger;

        public VehiculoService(AppDbContext context, IAudtiLog auditLog, IUsuarioActualService usuarioActual, ILogger<VehiculoService> logger)
        {
            _context = context;
            _auditLog = auditLog;
            _usuarioActual = usuarioActual;
            _logger = logger;
        }

        public async Task<VehiculoResponse> RegistrarAsync(RegistrarVehiculoRequest request)
        {
            _logger.LogInformation("Registrando vehículo con placa: {Placa}", request.Placa);

            var clienteExiste = await _context.Cliente.AnyAsync(c => c.Id == request.ClienteId);
            if (!clienteExiste)
                throw new KeyNotFoundException($"Cliente con ID {request.ClienteId} no encontrado.");

            var placa = request.Placa.Trim().ToUpper();
            var placaExiste = await _context.Vehiculo.AnyAsync(v => v.Placa == placa);
            if (placaExiste)
                throw new InvalidOperationException($"Ya existe un vehículo con la placa {request.Placa}.");

            var vehiculo = new Vehiculo
            {
                ClienteId = request.ClienteId,
                Placa = placa,
                Marca = request.Marca.Trim(),
                Modelo = request.Modelo.Trim(),
                Anno = request.Anno,
                Color = request.Color?.Trim() ?? string.Empty,
                VIN = request.VIN?.Trim().ToUpper() ?? string.Empty,
                TipoCombustible = request.TipoCombustible?.Trim() ?? string.Empty,
                KmActual = request.KmActual,
                Nota = request.Nota?.Trim() ?? string.Empty,
                FechaCreacion = DateTime.Now
            };

            _context.Vehiculo.Add(vehiculo);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Vehículo registrado: {VehiculoId}", vehiculo.Id);
            await RegistrarAuditoriaAsync(vehiculo.Id, "Crear", "Vehiculo", string.Empty, JsonSerializer.Serialize(ToResponse(vehiculo)));

            return ToResponse(vehiculo);
        }

        public async Task<IEnumerable<VehiculoResponse>> ListarPorClienteAsync(int clienteId, bool soloActivos = true)
        {
            var clienteExiste = await _context.Cliente.AnyAsync(c => c.Id == clienteId);
            if (!clienteExiste)
                throw new KeyNotFoundException($"Cliente con ID {clienteId} no encontrado.");
            var query = _context.Vehiculo
                .AsNoTracking()
                .Where(v => v.ClienteId == clienteId);

            if (soloActivos)
                query = query.Where(v => v.Activo);

            return await query
                .OrderByDescending(v => v.FechaCreacion)
                .ThenBy(v => v.Placa)
                .Select(v => ToResponse(v))
                .ToListAsync();
        }

        public async Task<IReadOnlyList<HistorialReparacionResponse>> ListarHistorialReparacionesAsync(int vehiculoId)
        {
            var vehiculoExiste = await _context.Vehiculo.AnyAsync(v => v.Id == vehiculoId);
            if (!vehiculoExiste)
                throw new KeyNotFoundException($"Vehículo con ID {vehiculoId} no encontrado.");

            return await _context.HistorialReparacion
                .AsNoTracking()
                .Where(h => h.VehiculoId == vehiculoId)
                .Include(h => h.Tecnico)
                .Include(h => h.Orden)
                .OrderByDescending(h => h.Fecha)
                .Select(h => new HistorialReparacionResponse(
                    h.Id,
                    h.VehiculoId,
                    h.OrdenId,
                    h.TecnicoId,
                    h.Tecnico.Nombre,
                    h.KmAlServicio,
                    h.TrabajosRealizados,
                    h.ProximoServicioKm,
                    h.ProximoServicioFecha,
                    h.GarantiaDias,
                    h.GarantiaHasta,
                    h.Fecha))
                .ToListAsync();
        }

        public async Task<VehiculoResponse> ActualizarAsync(int id, ActualizarVehiculoRequest request)
        {
            var vehiculo = await _context.Vehiculo
                .FirstOrDefaultAsync(v => v.Id == id)
                ?? throw new KeyNotFoundException($"Vehículo con ID {id} no encontrado.");

            var antes = JsonSerializer.Serialize(ToResponse(vehiculo));

            if (!string.IsNullOrWhiteSpace(request.Placa))
            {
                var nuevaPlaca = request.Placa.Trim().ToUpper();
                var placaExiste = await _context.Vehiculo.AnyAsync(v => v.Placa == nuevaPlaca && v.Id != id);
                if (placaExiste)
                    throw new InvalidOperationException($"Ya existe un vehículo con la placa {request.Placa}.");

                vehiculo.Placa = nuevaPlaca;
            }

            if (request.Marca is not null) vehiculo.Marca = request.Marca.Trim();
            if (request.Modelo is not null) vehiculo.Modelo = request.Modelo.Trim();
            if (request.Anno.HasValue) vehiculo.Anno = request.Anno.Value;
            if (request.Color is not null) vehiculo.Color = request.Color.Trim();
            if (request.VIN is not null) vehiculo.VIN = request.VIN.Trim().ToUpper();
            if (request.TipoCombustible is not null) vehiculo.TipoCombustible = request.TipoCombustible.Trim();
            if (request.KmActual.HasValue) vehiculo.KmActual = request.KmActual.Value;
            if (request.Nota is not null) vehiculo.Nota = request.Nota.Trim();

            await _context.SaveChangesAsync();

            _logger.LogInformation("Vehículo actualizado: {VehiculoId}", id);
            await RegistrarAuditoriaAsync(id, "Actualizar", "Vehiculo", antes, JsonSerializer.Serialize(ToResponse(vehiculo)));

            return ToResponse(vehiculo);
        }

        public async Task DesactivarAsync(int id)
        {
            var vehiculo = await _context.Vehiculo
                .FirstOrDefaultAsync(v => v.Id == id)
                ?? throw new KeyNotFoundException($"Vehículo con ID {id} no encontrado.");

            var antes = JsonSerializer.Serialize(ToResponse(vehiculo));

            vehiculo.Activo = false;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Vehículo desactivado: {VehiculoId}", id);
            await RegistrarAuditoriaAsync(id, "Eliminar", "Vehiculo", antes, JsonSerializer.Serialize(ToResponse(vehiculo)));
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

        private static VehiculoResponse ToResponse(Vehiculo vehiculo)
        {
            return new VehiculoResponse(
                vehiculo.Id,
                vehiculo.ClienteId,
                vehiculo.Placa,
                vehiculo.Marca,
                vehiculo.Modelo,
                vehiculo.Anno,
                vehiculo.Color,
                vehiculo.VIN,
                vehiculo.TipoCombustible,
                vehiculo.KmActual,
                vehiculo.Nota,
                vehiculo.FechaCreacion);
        }
    }
}
