namespace FrenosIntegracion.Services.Sync
{
    public interface IColaSyncService
    {
        // Este es el método que usa el Worker cada 30 segundos
        Task ProcesarPendientesAsync();

        // Este lo usaremos luego para que la Caja guarde cosas en la cola
        Task<string> EncolarOperacionAsync(string canal, string tipo, object payload);
    }
}
