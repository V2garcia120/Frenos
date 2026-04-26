using Microsoft.Extensions.Configuration;
using TallerCaja.Models.DTOs;
using TallerCaja.Models.Entities;
using Newtonsoft.Json;

namespace TallerCaja.Helpers
{
    // ── Configuración de la app ───────────────────────────────────────────────
    public static class AppConfig
    {
        private static IConfiguration? _config;

        public static void Init()
        {
            _config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        public static string IntegracionBaseUrl =>
            _config?["Integracion:BaseUrl"] ?? "https://localhost:7223";

        public static int TimeoutSeconds =>
            int.Parse(_config?["Integracion:TimeoutSeconds"] ?? "10");

        public static string NombreTaller =>
            _config?["App:NombreTaller"] ?? "Taller de Frenos";

        public static decimal ITBIS =>
            decimal.Parse(_config?["App:ITBIS"] ?? "0.18");

        public static string LocalDbPath =>
            _config?["Database:LocalDbPath"] ?? "tallercaja_local.db";
    }

    // ── Sesión del cajero autenticado ─────────────────────────────────────────
    public static class SessionManager
    {
        public static string? Token { get; private set; }
        public static int CajeroId { get; private set; }
        public static string CajeroNombre { get; private set; } = string.Empty;
        public static int TurnoId { get; private set; }
        public static bool TurnoAbierto { get; private set; }
        public static bool Autenticado => !string.IsNullOrEmpty(Token);

        public static void IniciarSesion(LoginCajeroResponse datos)
        {
            Token = datos.Token;
            CajeroId = datos.CajeroId;
            CajeroNombre = datos.Nombre;
            TurnoAbierto = false;
            Console.WriteLine($"[SessionManager] Sesión iniciada para {CajeroNombre} (ID: {CajeroId}), Token: {Token}, Expira: {datos.Expira}");
        }

        public static void AbrirTurno(int turnoId)
        {
            TurnoId = turnoId;
            TurnoAbierto = true;
        }

        public static void CerrarTurno()
        {
            TurnoId = 0;
            TurnoAbierto = false;
        }

        public static void CerrarSesion()
        {
            Token = null;
            CajeroId = 0;
            CajeroNombre = string.Empty;
            TurnoId = 0;
            TurnoAbierto = false;
        }
    }

    // ── Calculadora de facturas con ITBIS dominicano ──────────────────────────
    public static class FacturaCalculator
    {
        /// <summary>ITBIS República Dominicana = 18%</summary>
        public const decimal TASA_ITBIS = 0.18m;

        public static decimal CalcularSubtotal(IEnumerable<ItemCobroDto> items)
            => items.Sum(i => i.PrecioSnapshot * i.Cantidad);

        public static decimal CalcularITBIS(decimal subtotal)
            => Math.Round(subtotal * TASA_ITBIS, 2);

        public static decimal CalcularTotal(decimal subtotal)
            => subtotal + CalcularITBIS(subtotal);

        public static decimal CalcularCambio(decimal total, decimal montoPagado)
            => montoPagado >= total ? montoPagado - total : 0m;

        public static (decimal Subtotal, decimal ITBIS, decimal Total) Calcular(IEnumerable<ItemCobroDto> items)
        {
            var subtotal = CalcularSubtotal(items);
            var itbis = CalcularITBIS(subtotal);
            return (subtotal, itbis, subtotal + itbis);
        }
    }

    // ── Monitor de conexión con Integración ───────────────────────────────────
    public class ConexionMonitor
    {
        private static bool _online = false;
        private static bool _modoCache = false;
        private static DateTime? _ultimaSync;
        private readonly HttpClient _httpClient;

        public static bool Online => _online;
        public static bool ModoCache => _modoCache;
        public static DateTime? UltimaSync => _ultimaSync;

        public ConexionMonitor(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> VerificarConexionAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/int/auth/health");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<ApiResponse<HealthCheckDto>>(json);
                    if (resultado?.Data != null)
                    {
                        _online = true;
                        _modoCache = resultado.Data.ModoCache;
                        _ultimaSync = resultado.Data.UltimaSync;
                        return true;
                    }
                }
            }
            catch { /* Si falla, estamos offline */ }

            _online = false;
            _modoCache = true;
            return false;
        }
    }

    // ── Cola offline de transacciones pendientes ──────────────────────────────
    public class OfflineQueue
    {
        private readonly Data.CajaDbContext _db;

        public OfflineQueue(Data.CajaDbContext db)
        {
            _db = db;
        }

        public void Encolar(string tipo, object payload)
        {
            _db.TransaccionesPendientes.Add(new TransaccionPendiente
            {
                IdLocal = Guid.NewGuid().ToString(),
                Tipo = tipo,
                Payload = JsonConvert.SerializeObject(payload),
                FechaLocal = DateTime.Now,
                Procesada = false
            });
            _db.SaveChanges();
        }

        public List<TransaccionPendiente> ObtenerPendientes()
            => _db.TransaccionesPendientes
                  .Where(t => !t.Procesada)
                  .OrderBy(t => t.FechaLocal)
                  .ToList();

        public void MarcarProcesada(int id)
        {
            var item = _db.TransaccionesPendientes.Find(id);
            if (item != null)
            {
                item.Procesada = true;
                _db.SaveChanges();
            }
        }

        public int ContarPendientes()
            => _db.TransaccionesPendientes.Count(t => !t.Procesada);
    }

    // ── Formateador de moneda RD$ ─────────────────────────────────────────────
    public static class MonedaHelper
    {
        public static string Formatear(decimal valor) =>
            $"RD$ {valor:N2}";
    }
}
