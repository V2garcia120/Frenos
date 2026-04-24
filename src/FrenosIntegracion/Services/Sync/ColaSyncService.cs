using FrenosIntegracion.Data;
using FrenosIntegracion.Models.DTOs;
using FrenosIntegracion.Services.Core;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace FrenosIntegracion.Services.Sync
{
    public class ColaSyncService(IntegracionDbContext db, ICoreService core) : IColaSyncService
    {
        public async Task<bool> EstaDisponibleAsync() => await core.EstaDisponibleAsync();

        public async Task<object> AutenticarEmpleadoAsync(LoginRequest request)
        {
            // Simulación según Pág. 2
            return new { token = "JWT_SIMULADO", empleado = new { id = 1, nombre = "Diego", rol = "Cajero" } };
        }

        public async Task<object> AutenticarClienteAsync(LoginRequest request)
        {
            // Simulación según Pág. 7
            return new { token = "JWT_CLIENTE", clienteId = 5, nombre = "Diana" };
        }

        public async Task<object> AbrirTurnoAsync(int cajeroId, decimal montoInicial)
        {
            return new { turnoId = 100, estado = "Abierto" }; // Pág. 10
        }

        public async Task<object> RegistrarMovimientoEfectivoAsync(int turnoId, decimal monto, string motivo, string tipo)
        {
            return new { id = 50, tipo = tipo, fecha = DateTime.UtcNow }; // Pág. 11
        }

        public async Task<object> PagarFacturaAsync(int facturaId, int turnoId, string metodo, decimal monto)
        {
            return new { facturaId = facturaId, estado = "Pagada" }; // Pág. 12
        }

        public async Task<object> RegistrarAbonoAsync(int cxcId, int turnoId, decimal monto, string metodo)
        {
            return new { montoAbonado = monto, saldada = false }; // Pág. 13
        }

        // 1. Procesa el lote masivo enviado por la App de Caja (Pág. 13)
        public async Task<object> ProcesarLoteOfflineAsync(SyncRequest request)
        {
            var resultados = new List<object>();
            int procesadas = 0;
            int fallidas = 0;

            foreach (var transaccion in request.transacciones)
            {
                try
                {
                    // Encolamos cada transacción recibida en la base de datos local
                    await EncolarOperacionAsync(
                        canal: "Caja",
                        tipo: transaccion.tipo,
                        payload: transaccion.payload, // El payload ya viene como string JSON
                        idLocal: transaccion.idLocal
                    );

                    procesadas++;
                    resultados.Add(new { idLocal = transaccion.idLocal, exitosa = true });
                }
                catch (Exception ex)
                {
                    fallidas++;
                    resultados.Add(new { idLocal = transaccion.idLocal, exitosa = false, error = ex.Message });
                }
            }

            // Estructura de respuesta según manual (Pág. 13)
            return new
            {
                procesadas = procesadas,
                fallidas = fallidas,
                resultados = resultados
            };
        }

        // 2. Guarda una operación individual (Pág. 11)
        // Cambiamos la firma a Task (sin string) para que coincida con la interfaz que definimos
        public async Task<string> EncolarOperacionAsync(string canal, string tipo, object payload, string? idLocal = null)
        {
            var id = string.IsNullOrWhiteSpace(idLocal) ? Guid.NewGuid().ToString() : idLocal;

            // Si el payload ya es un string (viene de SyncRequest), lo usamos directo.
            // Si es un objeto (viene de un Controller), lo serializamos.
            string json = payload is string s ? s : JsonSerializer.Serialize(payload);

            var nuevaTarea = new Models.Entities.ColaPendiente
            {
                IdLocal = id,
                Canal = canal,
                TipoOperacion = tipo,
                PayloadJson = json,
                Estado = "Pendiente",
                Intentos = 0,
                MaxIntentos = 5,
                FechaCreacion = DateTime.UtcNow
            };

            db.ColaPendiente.Add(nuevaTarea);
            await db.SaveChangesAsync();
            return id;
        }

        public async Task ProcesarPendientesAsync()
        {
            var pendientes = await db.ColaPendiente
                .Where(c => c.Estado == "Pendiente" &&
                            c.Intentos < c.MaxIntentos &&
                            (c.ProximoIntento == null || c.ProximoIntento <= DateTime.UtcNow))
                .OrderBy(c => c.FechaCreacion)
                .Take(20)
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
            switch (item.TipoOperacion)
            {
                case "cobro":
                {
                    var request = JsonSerializer.Deserialize<CobroRequest>(item.PayloadJson, _json)
                        ?? throw new InvalidOperationException("Payload de cobro inválido.");
                    var token = ObtenerToken();
                    var resp = await core.ProcesarCobroAsync(request, token);
                    return JsonSerializer.Serialize(resp);
                }

                case "pago_factura":
                {
                    var request = JsonSerializer.Deserialize<PagoFacturaRequest>(item.PayloadJson, _json)
                        ?? JsonSerializer.Deserialize<PagoFacturaWrapper>(item.PayloadJson, _json)?.Request
                        ?? throw new InvalidOperationException("Payload de pago_factura inválido.");
                    var token = ObtenerToken();
                    var resp = await core.PagarFacturaAsync(request, token);
                    return JsonSerializer.Serialize(resp);
                }

                default:
                    throw new NotSupportedException($"Tipo de operación '{item.TipoOperacion}' no soportado.");
            }
        }

        private string ObtenerToken()
        {
            var key = new System.Security.Cryptography.HMACSHA256(
                System.Text.Encoding.ASCII.GetBytes("TallerFrenosClaveSecreta2026!MuyLargaParaHMAC256"));
            return GenerarJwtInterno();
        }

        private string GenerarJwtInterno()
        {
            var keyBytes = System.Text.Encoding.ASCII.GetBytes("TallerFrenosClaveSecreta2026!MuyLargaParaHMAC256");
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, "0"),
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, "sync@sistema.interno"),
                    new System.Security.Claims.Claim("Rol", "Administrador")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = "TallerCore",
                Audience = "TallerIntegracion",
                SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(
                    new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(keyBytes),
                    Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private static readonly JsonSerializerOptions _json = new() { PropertyNameCaseInsensitive = true };

        private record PagoFacturaWrapper(int FacturaId, PagoFacturaRequest Request);

    }
}