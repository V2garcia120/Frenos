using FrenosCore.Data;
using FrenosCore.Helpers;
using FrenosCore.Modelos.Dtos.Auth;
using FrenosCore.Modelos.Dtos.Log;
using FrenosCore.Modelos.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace FrenosCore.Servicios
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtHelper _jwtHelper;
        private readonly IAudtiLog _auditLog;
        private readonly IUsuarioActualService _usuarioActual;
        private readonly ILogger<AuthService> _logger;

        public AuthService(AppDbContext context, JwtHelper jwtHelper, IAudtiLog auditLog, IUsuarioActualService usuarioActual, ILogger<AuthService> logger)
        {
            _context = context;
            _jwtHelper = jwtHelper;
            _auditLog = auditLog;
            _usuarioActual = usuarioActual;
            _logger = logger;
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

            await RegistrarAuditoriaAsync(
                usuario.Id,
                "Login",
                "Usuario",
                string.Empty,
                JsonSerializer.Serialize(new { usuario.Id, usuario.Email, Tipo = "Usuario" }));

            _logger.LogInformation("Inicio de sesión exitoso para usuario {UsuarioId}", usuario.Id);

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

            await RegistrarAuditoriaAsync(
                cliente.Id,
                "Login",
                "Cliente",
                string.Empty,
                JsonSerializer.Serialize(new { cliente.Id, cliente.Email, Tipo = "Cliente" }));

            _logger.LogInformation("Inicio de sesión exitoso para cliente {ClienteId}", cliente.Id);

            var token = _jwtHelper.GenerarToken(cliente.Id.ToString(), cliente.Email, "Cliente");
            return new IniciarSesionResponse(true, token);
        }
        public async Task<LoginCajeroResponse> IniciarSesionCajero(IniciarSesionRequest req)
        {
            var cajero = await ObtenerPorEmailAsync(req.Email);
            if (cajero is null || cajero.Rol?.Nombre != "Caja")
                return new LoginCajeroResponse { Token = string.Empty };
            if (!ValidarCredenciales(cajero.PasswordHash, req.Password))
                return new LoginCajeroResponse { Token = string.Empty };
            cajero.UltimoLogin = DateTime.Now;
            await _context.SaveChangesAsync();
            await RegistrarAuditoriaAsync(
                cajero.Id,
                "Login",
                "Usuario",
                string.Empty,
                JsonSerializer.Serialize(new { cajero.Id, cajero.Email, Tipo = "Cajero" }));
            _logger.LogInformation("Inicio de sesión exitoso para cajero {CajeroId}", cajero.Id);
            var token = _jwtHelper.GenerarToken(cajero.Id.ToString(), cajero.Email, "Cajero");
            return new LoginCajeroResponse
            {
                Token = token,
                Expira = DateTime.Now.AddHours(1),
                CajeroId = cajero.Id,
                Nombre = cajero.Nombre
            };
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
