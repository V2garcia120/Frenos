using FrenosCore.Data;
using FrenosCore.Modelos.Dtos.Vehiculo;
using FrenosCore.Modelos.Entidades;
using Microsoft.EntityFrameworkCore;

namespace FrenosCore.Servicios
{
    public class VehiculoService : IVehiculoService
    {
        private readonly AppDbContext _context;

        public VehiculoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<VehiculoResponse> RegistrarAsync(RegistrarVehiculoRequest request)
        {
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

        public async Task<VehiculoResponse> ActualizarAsync(int id, ActualizarVehiculoRequest request)
        {
            var vehiculo = await _context.Vehiculo
                .FirstOrDefaultAsync(v => v.Id == id)
                ?? throw new KeyNotFoundException($"Vehículo con ID {id} no encontrado.");

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

            return ToResponse(vehiculo);
        }

        public async Task DesactivarAsync(int id)
        {
            var vehiculo = await _context.Vehiculo
                .FirstOrDefaultAsync(v => v.Id == id)
                ?? throw new KeyNotFoundException($"Vehículo con ID {id} no encontrado.");

            vehiculo.Activo = false;
            await _context.SaveChangesAsync();
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
