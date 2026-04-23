using TallerCaja.Helpers;
using TallerCaja.Models.Entities;
using TallerCaja.Services;

namespace TallerCaja.Forms
{
    public partial class frmInicioDia : Form
    {
        private readonly IIntegracionService _integracion;
        private readonly ICajaLocalService _local;
        public int TurnoIdResultante { get; private set; }

        public frmInicioDia()
        {
            _integracion = null!;
            _local = null!;
            InitializeComponent();
        }

        public frmInicioDia(IIntegracionService integracion, ICajaLocalService local)
        {
            _integracion = integracion;
            _local = local;
            InitializeComponent();
        }

        private void frmInicioDia_Load(object sender, EventArgs e)
        {
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
            {
                lblCajero.Text = "Cajero: demo";
                lblFecha.Text = $"Fecha: {DateTime.Now:dddd, dd MMMM yyyy}";
                return;
            }

            lblCajero.Text = $"Cajero: {SessionManager.CajeroNombre}";
            lblFecha.Text = $"Fecha: {DateTime.Now:dddd, dd MMMM yyyy}";
            txtMontoInicial.Focus();

            // Verificar si ya hay un turno abierto localmente
            var turnoExistente = _local.ObtenerTurnoActivo(SessionManager.CajeroId);
            if (turnoExistente != null)
            {
                TurnoIdResultante = turnoExistente.Id;
                SessionManager.AbrirTurno(turnoExistente.TurnoIdCore ?? turnoExistente.Id);
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private async void btnAbrir_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtMontoInicial.Text, out decimal monto) || monto < 0)
            {
                MessageBox.Show("Ingresa un monto inicial válido.", "Dato inválido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnAbrir.Enabled = false;
            btnAbrir.Text = "Abriendo turno...";

            try
            {
                var request = new Models.DTOs.AbrirTurnoRequest
                {
                    CajeroId = SessionManager.CajeroId,
                    MontoInicial = monto
                };

                var resp = await _integracion.AbrirTurnoAsync(request);

                var turnoLocal = new TurnoLocal
                {
                    CajeroId = SessionManager.CajeroId,
                    CajeroNombre = SessionManager.CajeroNombre,
                    MontoInicial = monto,
                    Estado = "Abierto",
                    FechaApertura = DateTime.Now,
                    TurnoIdCore = resp?.TurnoId
                };

                _local.GuardarTurnoLocal(turnoLocal);
                SessionManager.AbrirTurno(resp?.TurnoId ?? turnoLocal.Id);
                TurnoIdResultante = turnoLocal.Id;

                MessageBox.Show($"Turno abierto correctamente.\nMonto inicial: {MonedaHelper.Formatear(monto)}",
                    "Turno abierto", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception)
            {
                // Si falla Integración, guardar solo en local
                var turnoLocal = new TurnoLocal
                {
                    CajeroId = SessionManager.CajeroId,
                    CajeroNombre = SessionManager.CajeroNombre,
                    MontoInicial = monto,
                    Estado = "Abierto",
                    FechaApertura = DateTime.Now
                };
                _local.GuardarTurnoLocal(turnoLocal);
                SessionManager.AbrirTurno(turnoLocal.Id);
                TurnoIdResultante = turnoLocal.Id;

                MessageBox.Show($"Turno abierto en modo offline.\nSe sincronizará cuando haya conexión.",
                    "Modo Offline", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                DialogResult = DialogResult.OK;
                Close();
            }
            finally
            {
                btnAbrir.Enabled = true;
                btnAbrir.Text = "Abrir Turno";
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
