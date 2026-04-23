using TallerCaja.Helpers;
using TallerCaja.Models.DTOs;

namespace TallerCaja.Services
{
    public class SyncService : ISyncService
    {
        private readonly IIntegracionService _integracion;
        private readonly OfflineQueue _queue;

        public SyncService(IIntegracionService integracion, OfflineQueue queue)
        {
            _integracion = integracion;
            _queue = queue;
        }

        public async Task SincronizarPendientesAsync()
        {
            var pendientes = _queue.ObtenerPendientes();
            if (!pendientes.Any()) return;

            var request = new SyncRequest
            {
                Transacciones = pendientes.Select(p => new SyncItemDto
                {
                    IdLocal = p.IdLocal,
                    Tipo = p.Tipo,
                    Payload = p.Payload,
                    Fecha = p.FechaLocal
                }).ToList()
            };

            var resultado = await _integracion.SincronizarAsync(request);
            if (resultado == null) return;

            foreach (var r in resultado.Resultados.Where(r => r.Exitosa))
            {
                var item = pendientes.FirstOrDefault(p => p.IdLocal == r.IdLocal);
                if (item != null) _queue.MarcarProcesada(item.Id);
            }
        }
    }
}
