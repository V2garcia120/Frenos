using FrenosCore.Data;
using FrenosCore.Modelos.Dtos;
using FrenosCore.Modelos.Dtos.Cliente;
using FrenosCore.Modelos.Entidades;
using Microsoft.EntityFrameworkCore;

namespace FrenosCore.Servicios

{
    public class ClienteService : IClienteService
    {
        private readonly AppDbContext _context;
        public ClienteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaginadoResponse<ClienteResponse>> ListarAsync(int pagina, int tam, string? busqueda)
        {
            pagina = Math.Max(1, pagina);
            tam = Math.Clamp(tam, 1, 100);

            var query = _context.Cliente.AsNoTracking();

            // Búsqueda por nombre, cédula o email
            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                var termino = busqueda.Trim().ToLower();
                query = query.Where(c =>
                    c.Nombre.ToLower().Contains(termino) ||
                    (c.Cedula != null && c.Cedula.Contains(termino)) ||
                    (c.Email != null && c.Email.ToLower().Contains(termino)));
            }

            var totalItems = await query.CountAsync();

            var items = await query
                .OrderBy(c => c.Nombre)
                .Skip((pagina - 1) * tam)
                .Take(tam)
                .Select(c => new ClienteResponse(
                    c.Id,
                    c.Nombre,
                    c.Cedula,
                    c.Telefono,
                    c.Email,
                    c.Direccion,
                    c.EsAnonimo,
                    c.Vehiculos.Count,
                    c.Ordenes.Count,
                    c.CreadoEn))
                .ToListAsync();

            return new PaginadoResponse<ClienteResponse>(
                Items: items,
                PaginaActual: pagina,
                TamPagina: tam,
                TotalItems: totalItems,
                TotalPaginas: (int)Math.Ceiling(totalItems / (double)tam));
        }
        public async Task<ClienteDetalleResponse> ObtenerPorIdAsync(int id)
        {
            var cliente = await _context.Cliente
           .AsNoTracking()
           .Include(c => c.Vehiculos)
           .FirstOrDefaultAsync(c => c.Id == id)
           ?? throw new KeyNotFoundException($"Cliente {id} no encontrado.");

            return new ClienteDetalleResponse(
                Id: cliente.Id,
                Nombre: cliente.Nombre,
                Cedula: cliente.Cedula,
                Telefono: cliente.Telefono,
                Email: cliente.Email,
                Direccion: cliente.Direccion,
                EsAnonimo: cliente.EsAnonimo,
                CreadoEn: cliente.CreadoEn,
                Vehiculos: cliente.Vehiculos.Select(v => new VehiculoResumen(
                                v.Id, v.Placa, v.Marca, v.Modelo, v.Anno)));
        }

        public async Task<IEnumerable<ClienteResponse>> BuscarAsync(string termino)
        {
            if (string.IsNullOrWhiteSpace(termino))
                return [];

            var t = termino.Trim().ToLower();

            return await _context.Cliente
                .AsNoTracking()
                .Where(c =>
                    c.Nombre.ToLower().Contains(t) ||
                    (c.Cedula != null && c.Cedula.Contains(t)) ||
                    (c.Email != null && c.Email.ToLower().Contains(t)))
                .OrderBy(c => c.Nombre)
                .Take(20)                       
                .Select(c => ToResponse(c))
                .ToListAsync();
        }

        public async Task<ClienteResponse> ObtenerAnonimoAsync()
        {
            var anonimo = await _context.Cliente
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.EsAnonimo)
                ?? throw new InvalidOperationException(
                    "El cliente anónimo no existe. Verifique el Seed de datos.");

            return ToResponse(anonimo);
        }

        public async Task<ClienteResponse> CrearAsync(CrearClienteRequest req)
        {
            if (!string.IsNullOrWhiteSpace(req.Cedula))
            {
                var cedulaExiste = await _context.Cliente
                    .AnyAsync(c => c.Cedula == req.Cedula.Trim());

                if (cedulaExiste)
                    throw new InvalidOperationException(
                        $"Ya existe un cliente registrado con la cédula {req.Cedula}.");
            }

            var cliente = new Cliente
            {
                Nombre = req.Nombre.Trim(),
                Cedula = req.Cedula?.Trim(),
                Telefono = req.Telefono?.Trim(),
                Email = req.Email?.Trim().ToLower(),
                Direccion = req.Direccion?.Trim(),
            };

            _context.Cliente.Add(cliente);
            await _context.SaveChangesAsync();

            return ToResponse(cliente);

        }

        public async Task<ClienteResponse> ActualizarAsync(int id, ActualizarClienteRequest req)
        {
            var cliente = await _context.Cliente.FindAsync(id)
            ?? throw new KeyNotFoundException($"Cliente {id} no encontrado.");

  
            if (cliente.EsAnonimo)
                throw new InvalidOperationException(
                    "El cliente anónimo del sistema no puede ser modificado.");


            if (!string.IsNullOrWhiteSpace(req.Cedula) &&
                req.Cedula.Trim() != cliente.Cedula)
            {
                var cedulaExiste = await _context.Cliente
                    .AnyAsync(c => c.Cedula == req.Cedula.Trim() && c.Id != id);

                if (cedulaExiste)
                    throw new InvalidOperationException(
                        $"Ya existe un cliente registrado con la cédula {req.Cedula}.");
            }

            // Solo actualizar los campos que vienen en el request
            if (req.Nombre is not null) cliente.Nombre = req.Nombre.Trim();
            if (req.Cedula is not null) cliente.Cedula = req.Cedula.Trim();
            if (req.Telefono is not null) cliente.Telefono = req.Telefono.Trim();
            if (req.Email is not null) cliente.Email = req.Email.Trim().ToLower();
            if (req.Direccion is not null) cliente.Direccion = req.Direccion.Trim();

            await _context.SaveChangesAsync();

            return ToResponse(cliente);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var cliente = await _context.Cliente
                .Include(c => c.Facturas)
                .FirstOrDefaultAsync(c => c.Id == id)
                ?? throw new KeyNotFoundException($"Cliente {id} no encontrado.");

            if (cliente.EsAnonimo)
                throw new InvalidOperationException(
                    "El cliente anónimo del sistema no puede ser eliminado.");

            if (cliente.Facturas.Count > 0)
                throw new InvalidOperationException(
                    "No se puede eliminar un cliente que tiene facturas registradas.");

            _context.Cliente.Remove(cliente);
            await _context.SaveChangesAsync();
            return true;
        }

        private static ClienteResponse ToResponse(Cliente c) => new(
            c.Id,
            c.Nombre,
            c.Cedula,
            c.Telefono,
            c.Email,
            c.Direccion,
            c.EsAnonimo,
            TotalVehiculos: 0,   
            TotalOrdenes: 0,   
            c.CreadoEn);

    }
}
