using FrenosCore.Data;
using FrenosCore.Modelos.Dtos;
using FrenosCore.Modelos.Dtos.CuentasPorCobrar;
using Microsoft.EntityFrameworkCore;

namespace FrenosCore.Servicios
{
    public class CuentasPorCobrarService : ICuentasPorCobrarService
    {
        private readonly AppDbContext _context;

        public CuentasPorCobrarService(AppDbContext context)
        {
            _context = context;
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

            return await ObtenerPorIdAsync(id);
        }
    }
}
