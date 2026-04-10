using FrenosCore.Data;
using FrenosCore.Modelos.Dtos.Log;
using FrenosCore.Modelos.Entidades;

namespace FrenosCore.Servicios
{
    public class AuditLogService (AppDbContext context, ILogger<AuditLogService> logger) : IAudtiLog
    {
        private readonly AppDbContext _context = context;
        public async Task RegistrarAsync(AuditEntry entry)
        {
            _context.AuditLog.Add(new AuditLog
            {
                UsuarioId = entry.UsuarioId,
                RegistroId = entry.ResgistroId,
                Accion = entry.Accion,
                Tabla = entry.Tabla,
                Ip = entry.Ip,
                ValorAntes = entry.ValorAntes,
                ValorDespues = entry.ValorDespues,
                FechaHora = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
            logger.LogInformation("Auditoría registrada para {Tabla} #{RegistroId}", entry.Tabla, entry.ResgistroId);
        }
    }
}
