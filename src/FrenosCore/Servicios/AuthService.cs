using Azure.Core;
using BCrypt.Net;
using FrenosCore.Data;
using FrenosCore.Helpers;
using FrenosCore.Modelos.Dtos.Auth;
using FrenosCore.Modelos.Dtos.Usuario;
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
            Usuario usuario = ObtenerPorEmailAsync(req.Email).Result;
            if (usuario == null)
            {
                return new IniciarSesionResponse(false, null);
            }
            if (ValidarCredenciales(usuario.PasswordHash, req.Password))
            {
                string token = _jwtHelper.GenerarToken(usuario.Id.ToString(), usuario.Email, usuario.Rol.Nombre);
                return new IniciarSesionResponse(true, token);
            }
            return new IniciarSesionResponse(false, null);
        }
        public async Task<IniciarSesionResponse> IniciarSesionCliente(IniciarSesionRequest req)
        {
            Cliente cliente = ObtenerClientePorEmailAsync(req.Email).Result;
            if (cliente == null)
            {
                return new IniciarSesionResponse(false, null);
            }
            if (ValidarCredenciales(cliente.PasswordHash, req.Password))
            {
                string token = _jwtHelper.GenerarToken(cliente.Id.ToString(), cliente.Email, "Cliente");
                return new IniciarSesionResponse(true, token);
            }
            return new IniciarSesionResponse(false, null);
        }
        private bool ValidarCredenciales(string passwordHash, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
        private async Task<Usuario> ObtenerPorEmailAsync(string email)
        {
            var emailNorm = email.Trim().ToLower();
            var usuario = await _context.Usuario
                .AsNoTracking()
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Email == emailNorm && u.Activo)
                ?? throw new KeyNotFoundException($"Usuario con email {email} no encontrado.");
            return usuario;
        }
        private async Task<Cliente> ObtenerClientePorEmailAsync(string email)
        {
            var emailNorm = email.Trim().ToLower();
            var cliente = await _context.Cliente
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Email == emailNorm)
                ?? throw new KeyNotFoundException($"Cliente con email {email} no encontrado.");
            return cliente;
        }
    }
}
