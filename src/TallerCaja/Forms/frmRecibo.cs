using TallerCaja.Models.DTOs;

namespace TallerCaja.Forms
{
    public partial class frmRecibo : Form
    {
        private readonly string _textoRecibo;
        private readonly CobroResponse _cobro;

        public frmRecibo()
        {
            _textoRecibo = "=== RECIBO DEMO ===";
            _cobro = new CobroResponse { Estado = "Completado", NumeroFactura = "FAC-DEMO" };
            InitializeComponent();
        }

        public frmRecibo(string textoRecibo, CobroResponse cobro)
        {
            _textoRecibo = textoRecibo;
            _cobro = cobro;
            InitializeComponent();
        }

        private void frmRecibo_Load(object sender, EventArgs e)
        {
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
            {
                lblEstadoBadge.Text = "✓ COBRO COMPLETADO — FAC-DEMO";
                lblEstadoBadge.BackColor = Color.FromArgb(22, 163, 74);
                txtRecibo.Text = _textoRecibo;
                return;
            }

            txtRecibo.Text = _textoRecibo;
            if (_cobro.Estado == "PendienteSync")
            {
                lblEstadoBadge.Text = "⚠ PENDIENTE DE SINCRONIZACIÓN (offline)";
                lblEstadoBadge.BackColor = Color.FromArgb(251, 146, 60);
            }
            else
            {
                lblEstadoBadge.Text = $"✓ COBRO COMPLETADO — {_cobro.NumeroFactura}";
                lblEstadoBadge.BackColor = Color.FromArgb(22, 163, 74);
            }
        }

        private void btnCopiar_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(_textoRecibo);
            MessageBox.Show("Recibo copiado al portapapeles.", "Copiado", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCerrar_Click(object sender, EventArgs e) => Close();
    }
}
