using FrenosCore.Data;
using FrenosCore.Helpers;
using FrenosCore.Modelos.Dtos;
using FrenosCore.Modelos.Dtos.CuentasPorCobrar;
using FrenosCore.Modelos.Dtos.Log;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace FrenosCore.Servicios
{
    public class CuentasPorCobrarService : ICuentasPorCobrarService
    {
        private readonly AppDbContext _context;
        private readonly IAudtiLog _auditLog;
        private readonly IUsuarioActualService _usuarioActual;
        private readonly ILogger<CuentasPorCobrarService> _logger;

        public CuentasPorCobrarService(AppDbContext context, IAudtiLog auditLog, IUsuarioActualService usuarioActual, ILogger<CuentasPorCobrarService> logger)
        {
            _context = context;
            _auditLog = auditLog;
            _usuarioActual = usuarioActual;
            _logger = logger;
        }

        public async Task<PaginadoResponse<CuentaPorCobrarResponse>> ListarAsync(int pagina, int tam, string? estado)
        {
            pagina = Math.Max(1, pagina);
            tam = Math.Clamp(tam, 1, 100);

            var query = _context.CuentasPorCobrar
                .AsNoTracking()
                .Include(c => c.Cliente)
                .Include(c => c.Factura)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(estado))
                query = query.Where(c => c.Estado == estado);

            var totalItems = await query.CountAsync();

            var items = await query
                .OrderByDescending(c => c.Vencimiento)
                .Skip((pagina - 1) * tam)
                .Take(tam)
                .Select(c => new CuentaPorCobrarResponse(
                    c.Id,
                    c.ClienteId,
                    c.Cliente.Nombre,
                    c.FacturaId,
                    c.Factura.Numero,
                    c.Monto,
                    c.Saldo,
                    c.Vencimiento,
                    c.Estado,
                    c.CreadoEn))
                .ToListAsync();

            return new PaginadoResponse<CuentaPorCobrarResponse>(
                items,
                pagina,
                tam,
                totalItems,
                (int)Math.Ceiling(totalItems / (double)tam));
        }

        public async Task<CuentaPorCobrarResponse?> ObtenerPorFacturaIdAsync(int facturaId)
        {
            return await _context.CuentasPorCobrar
                .AsNoTracking()
                .Include(c => c.Cliente)
                .Include(c => c.Factura)
                .Where(c => c.FacturaId == facturaId)
                .Select(c => new CuentaPorCobrarResponse(
                    c.Id,
                    c.ClienteId,
                    c.Cliente.Nombre,
                    c.FacturaId,
                    c.Factura.Numero,
                    c.Monto,
                    c.Saldo,
                    c.Vencimiento,
                    c.Estado,
                    c.CreadoEn))
                .FirstOrDefaultAsync();
        }

        public async Task<CuentaPorCobrarDetalleResponse> ObtenerPorIdAsync(int id)
        {
            var cxc = await _context.CuentasPorCobrar
                .AsNoTracking()
                .Include(c => c.Cliente)
                .Include(c => c.Factura)
                .Include(c => c.Abonos)
                    .ThenInclude(a => a.RegistradoPorUsuario)
                .FirstOrDefaultAsync(c => c.Id == id)
                ?? throw new KeyNotFoundException($"Cuenta por cobrar {id} no encontrada.");

            var antes = JsonSerializer.Serialize(new
            {
                cxc.Id,
                cxc.Saldo,
                cxc.Estado
            });

            return new CuentaPorCobrarDetalleResponse(
                cxc.Id,
                cxc.ClienteId,
                cxc.Cliente.Nombre,
                cxc.FacturaId,
                cxc.Factura.Numero,
                cxc.Monto,
                cxc.Saldo,
                cxc.Vencimiento,
                cxc.Estado,
                cxc.CreadoEn,
                cxc.Abonos
                    .OrderByDescending(a => a.Fecha)
                    .Select(a => new AbonoCxCResponse(
                        a.Id,
                        a.Monto,
                        a.Fecha,
                        a.MetodoPago,
                        a.RegistradoPor,
                        a.RegistradoPorUsuario.Nombre))
                    .ToList());
        }

        public async Task<CuentaPorCobrarDetalleResponse> RegistrarAbonoAsync(int id, decimal monto, string metodoPago, int registradoPor)
        {
            var cxc = await _context.CuentasPorCobrar
                .Include(c => c.Cliente)
                .Include(c => c.Factura)
                .Include(c => c.Abonos)
                .FirstOrDefaultAsync(c => c.Id == id)
                ?? throw new KeyNotFoundException($"Cuenta por cobrar {id} no encontrada.");

            var antes = JsonSerializer.Serialize(new
            {
                cxc.Id,
                cxc.Saldo,
                cxc.Estado
            });

            if (cxc.Estado == "Anulada")
                throw new InvalidOperationException("No se pueden registrar abonos a una cuenta anulada.");

            if (cxc.Saldo <= 0)
                throw new InvalidOperationException("La cuenta ya está saldada.");

            if (monto <= 0)
                throw new ArgumentException("El monto del abono debe ser mayor que cero.");

            if (monto > cxc.Saldo)
                throw new InvalidOperationException("El monto del abono no puede exceder el saldo pendiente.");

            string[] metodosValidos = ["Efectivo", "Tarjeta", "Transferencia", "Credito"];
            if (!metodosValidos.Contains(metodoPago))
                throw new ArgumentException(
                    $"Método de pago '{metodoPago}' no válido. Valores permitidos: {string.Join(", ", metodosValidos)}.");

            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(u => u.Id == registradoPor && u.Activo)
                ?? throw new KeyNotFoundException($"Usuario {registradoPor} no encontrado o inactivo.");

            var abono = new Modelos.Entidades.AbonoCxC
            {
                CxCId = cxc.Id,
                Monto = monto,
                Fecha = DateTime.UtcNow,
                MetodoPago = metodoPago,
                RegistradoPor = registradoPor
            };

            _context.AbonoCxC.Add(abono);

            cxc.Saldo -= monto;
            if (cxc.Saldo <= 0)
            {
                cxc.Saldo = 0;
                cxc.Estado = "Pagada";
                cxc.Factura.Estado = "Pagada";
            }
            else
            {
                cxc.Estado = cxc.Vencimiento.Date < DateTime.UtcNow.Date ? "Vencida" : "Pendiente";
                cxc.Factura.Estado = "Pendiente";
            }

            await _context.SaveChangesAsync();

            await RegistrarAuditoriaAsync(
                cxc.Id,
                "RegistrarAbono",
                "CuentasPorCobrar",
                antes,
                JsonSerializer.Serialize(new
                {
                    cxc.Id,
                    cxc.Saldo,
                    cxc.Estado,
                    MontoAbono = monto,
                    MetodoPago = metodoPago
                }));

            _logger.LogInformation("Abono registrado en CxC {CxCId} por monto {Monto}", cxc.Id, monto);

            return await ObtenerPorIdAsync(id);
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
    }
}
