using TallerCaja.Data;
using TallerCaja.Forms;
using TallerCaja.Helpers;
using TallerCaja.Services;

namespace TallerCaja
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Inicializar configuración
            AppConfig.Init();

            // ── Composición de dependencias ───────────────────────────────────
            // PUNTO DE CONEXIÓN: Cuando las APIs reales de Integración estén
            // disponibles, reemplaza 'IntegracionMockService' por 'IntegracionService'
            // y asegúrate de que AppConfig.IntegracionBaseUrl apunte al servidor real.
            // ─────────────────────────────────────────────────────────────────

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(AppConfig.IntegracionBaseUrl),
                Timeout = TimeSpan.FromSeconds(AppConfig.TimeoutSeconds)
            };

            // Para usar la API real: new IntegracionService(httpClient)
            // Para pruebas sin API:   new IntegracionMockService()
            IIntegracionService integracionService = new IntegracionService(httpClient);

            var dbContext = new CajaDbContext();
            dbContext.Database.EnsureCreated();

            var cajaLocalService = new CajaLocalService(dbContext);
            var offlineQueue = new OfflineQueue(dbContext);
            var syncService = new SyncService(integracionService, offlineQueue);
            var conexionMonitor = new ConexionMonitor(httpClient);

            // ── Flujo de inicio de sesión ─────────────────────────────────────
            var loginForm = new frmLogin(integracionService, conexionMonitor);
            if (loginForm.ShowDialog() != DialogResult.OK)
                return; // El usuario canceló el login

            // ── Apertura de turno ─────────────────────────────────────────────
            var inicioDia = new frmInicioDia(integracionService, cajaLocalService);
            if (inicioDia.ShowDialog() != DialogResult.OK)
                return; // El usuario canceló sin abrir turno

            int localTurnoId = inicioDia.TurnoIdResultante;

            // ── Pantalla principal de cobro ────────────────────────────────────
            Application.Run(new frmCobro(integracionService, cajaLocalService, offlineQueue, syncService, localTurnoId));
        }
    }
}
