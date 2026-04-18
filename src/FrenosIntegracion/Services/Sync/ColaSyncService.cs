using FrenosIntegracion.Data;
using FrenosIntegracion.Services.Core;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace FrenosIntegracion.Services.Sync
{
    public class ColaSyncService(IntegracionDbContext db, ICoreService core) : IColaSyncService
    {
        public async Task ProcesarPendientesAsync()
        {
            var pendientes = await db.ColaPendiente
                .Where(c => c.Estado == "Pendiente" &&
                            c.Intentos < c.MaxIntentos &&
                            (c.ProximoIntento == null || c.ProximoIntento <= DateTime.UtcNow))
                .OrderBy(c => c.FechaCreacion)
                .Take(20)   // procesar de a 20 para no sobrecargar el Core
                .ToListAsync();

            var coreDisponible = await core.EstaDisponibleAsync();
            if (!coreDisponible) return;

            foreach (var item in pendientes)
            {
                try
                {
                    item.Estado = "Procesando";
                    item.Intentos += 1;
                    await db.SaveChangesAsync();

                    var respuesta = await ReenviarAlCoreAsync(item);
                    item.Estado = "Completado";
                    item.RespuestaCore = respuesta;
                    item.FechaProcesado = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    // Backoff exponencial: 2^(intentos-1) minutos
                    var minutosEspera = Math.Pow(2, item.Intentos - 1);
                    item.Estado = item.Intentos >= item.MaxIntentos ? "Fallido" : "Pendiente";
                    item.ErrorDetalle = ex.Message;
                    item.ProximoIntento = DateTime.UtcNow.AddMinutes(minutosEspera);
                }
                await db.SaveChangesAsync();
            }
        }

        private async Task<string> ReenviarAlCoreAsync(Models.Entities.ColaPendiente item)
        {
            // Aquí irá la lógica para llamar al CoreService dependiendo de si es un cobro o una orden
            return "Respuesta simulada del Core";
        }

        public async Task<string> EncolarOperacionAsync(string canal, string tipo, object payload)
        {
            var idLocal = Guid.NewGuid().ToString();

            var nuevaTarea = new Models.Entities.ColaPendiente
            {
                IdLocal = idLocal,
                Canal = canal,
                TipoOperacion = tipo,
                // Convertimos el objeto a texto JSON para guardarlo en la BD
                PayloadJson = JsonSerializer.Serialize(payload),
                Estado = "Pendiente",
                Intentos = 0,
                MaxIntentos = 5,
                FechaCreacion = DateTime.UtcNow
            };

            db.ColaPendiente.Add(nuevaTarea);
            await db.SaveChangesAsync();

            return idLocal;
        }
    }
}
