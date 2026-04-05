using FrenosCore.Data;
using FrenosCore.Helpers;
using FrenosCore.Modelos.Dtos.Auth;
using FrenosCore.Modelos.Entidades;
using Microsoft.EntityFrameworkCore;

namespace FrenosCore.Servicios
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtHelper _jwtHelper;

        public AuthService(AppDbContext context, JwtHelper jwtHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
        }

        public async Task<IniciarSesionResponse> IniciarSesionUsuario(IniciarSesionRequest req)
        {
            var usuario = await ObtenerPorEmailAsync(req.Email);
            if (usuario is null)
                return new IniciarSesionResponse(false, null);

            if (!ValidarCredenciales(usuario.PasswordHash, req.Password))
                return new IniciarSesionResponse(false, null);

            usuario.UltimoLogin = DateTime.Now;
            await _context.SaveChangesAsync();

            var rol = usuario.Rol?.Nombre ?? "Usuario";
            var token = _jwtHelper.GenerarToken(usuario.Id.ToString(), usuario.Email, rol);
            return new IniciarSesionResponse(true, token);
        }

        public async Task<IniciarSesionResponse> IniciarSesionCliente(IniciarSesionRequest req)
        {
            var cliente = await ObtenerClientePorEmailAsync(req.Email);
            if (cliente is null)
                return new IniciarSesionResponse(false, null);

            if (!ValidarCredenciales(cliente.PasswordHash, req.Password))
                return new IniciarSesionResponse(false, null);

            var token = _jwtHelper.GenerarToken(cliente.Id.ToString(), cliente.Email, "Cliente");
            return new IniciarSesionResponse(true, token);
        }

        private bool ValidarCredenciales(string passwordHash, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

        private async Task<Usuario?> ObtenerPorEmailAsync(string email)
        {
            var emailNorm = email.Trim().ToLowerInvariant();
            return await _context.Usuario
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Email == emailNorm && u.Activo);
        }

        private async Task<Cliente?> ObtenerClientePorEmailAsync(string email)
        {
            var emailNorm = email.Trim().ToLowerInvariant();
            return await _context.Cliente
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Email == emailNorm);
        }
    }
}
