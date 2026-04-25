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

        public async Task<SyncResponse> SincronizarPendientesAsync()
        {
            var pendientes = _queue.ObtenerPendientes();
            if (!pendientes.Any())
                return new SyncResponse();

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
            if (resultado == null)
                return new SyncResponse();

            var respuesta = new SyncResponse
            {
                Procesadas = resultado.Procesadas,
                Fallidas = resultado.Fallidas,
                Resultados = resultado.Resultados ?? new List<SyncResultadoDto>()
            };

            foreach (var r in resultado.Resultados.Where(r => r.Exitosa))
            {
                var item = pendientes.FirstOrDefault(p => p.IdLocal == r.IdLocal);
                if (item != null) _queue.MarcarProcesada(item.Id);
            }

            return respuesta;
        }
    }
}
