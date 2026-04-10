using FrenosCore.Modelos.Dtos.Log;

namespace FrenosCore.Servicios
{
    public interface IAudtiLog
    {
        Task RegistrarAsync(AuditEntry entry);
    }
}
