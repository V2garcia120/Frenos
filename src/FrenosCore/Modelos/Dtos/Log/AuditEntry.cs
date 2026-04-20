namespace FrenosCore.Modelos.Dtos.Log
{
    public record AuditEntry
    (
        int UsuarioId,
        int ResgistroId,
        string Accion,
        string Tabla,
        string Ip,
        string ValorAntes,
        string ValorDespues
    );
    
}
