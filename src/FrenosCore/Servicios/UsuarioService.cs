using FrenosCore.Data;
using FrenosCore.Modelos.Dtos.Usuario;
using FrenosCore.Modelos.Entidades;
using Microsoft.EntityFrameworkCore;

namespace FrenosCore.Servicios
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UsuarioResponse>> ListarAsync(string? busqueda)
        {
            var query = _context.Usuario
                .AsNoTracking()
                .Include(u => u.Rol)
                .Where(u => u.Activo);

            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                var t = busqueda.Trim().ToLower();
                query = query.Where(u =>
                    u.Nombre.ToLower().Contains(t) ||
                    u.Email.ToLower().Contains(t) ||
                    u.Rol.Nombre.ToLower().Contains(t));
            }

            return await query
                .OrderBy(u => u.Nombre)
                .Select(u => ToResponse(u))
                .ToListAsync();
        }

        public async Task<UsuarioResponse> ObtenerPorIdAsync(int id)
        {
            var usuario = await _context.Usuario
                .AsNoTracking()
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Id == id && u.Activo)
                ?? throw new KeyNotFoundException($"Usuario con ID {id} no encontrado.");

            return ToResponse(usuario);
        }

        public async Task<UsuarioResponse> CrearAsync(CrearUsuarioRequest request)
        {
            var email = request.Email.Trim().ToLower();

            var existeEmail = await _context.Usuario
                .AnyAsync(u => u.Email == email);

            if (existeEmail)
                throw new InvalidOperationException($"Ya existe un usuario con el email {request.Email}.");

            var rol = await _context.Rol
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == request.RolId)
                ?? throw new KeyNotFoundException($"Rol con ID {request.RolId} no encontrado.");

            var usuario = new Usuario
            {
                Nombre = request.Nombre.Trim(),
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                RolId = request.RolId,
                Activo = request.Activo,
                FechaCreacion = DateTime.Now
            };

            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();

            usuario.Rol = rol;
            return ToResponse(usuario);
        }

        public async Task<UsuarioResponse> ActualizarAsync(int id, ActualizarUsuarioRequest request)
        {
            var usuario = await _context.Usuario
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Id == id && u.Activo)
                ?? throw new KeyNotFoundException($"Usuario con ID {id} no encontrado.");

            if (request.Email is not null)
            {
                var nuevoEmail = request.Email.Trim().ToLower();
                var existeEmail = await _context.Usuario
                    .AnyAsync(u => u.Email == nuevoEmail && u.Id != id);

                if (existeEmail)
                    throw new InvalidOperationException($"Ya existe un usuario con el email {request.Email}.");

                usuario.Email = nuevoEmail;
            }

            if (request.RolId.HasValue)
            {
                var rol = await _context.Rol
                    .FirstOrDefaultAsync(r => r.Id == request.RolId.Value)
                    ?? throw new KeyNotFoundException($"Rol con ID {request.RolId.Value} no encontrado.");

                usuario.RolId = request.RolId.Value;
                usuario.Rol = rol;
            }

            if (request.Nombre is not null) usuario.Nombre = request.Nombre.Trim();
            if (request.Password is not null) usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            if (request.Activo.HasValue) usuario.Activo = request.Activo.Value;

            await _context.SaveChangesAsync();

            return ToResponse(usuario);
        }

        public async Task EliminarAsync(int id)
        {
            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(u => u.Id == id && u.Activo)
                ?? throw new KeyNotFoundException($"Usuario con ID {id} no encontrado.");

            usuario.Activo = false;
            await _context.SaveChangesAsync();
        }

        private static UsuarioResponse ToResponse(Usuario u)
        {
            return new UsuarioResponse(
                u.Id,
                u.Nombre,
                u.Email,
                u.RolId,
                u.Rol.Nombre,
                u.Activo,
                u.FechaCreacion,
                u.UltimoLogin);
        }
    }
}
