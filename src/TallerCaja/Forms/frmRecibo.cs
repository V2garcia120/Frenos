using TallerCaja.Models.DTOs;

namespace TallerCaja.Forms
{
    public partial class frmRecibo : Form
    {
        private readonly string _textoRecibo;
        private readonly CobroResponse _cobro;

        private Label lblEstadoBadge = null!;
        private TextBox txtRecibo = null!;
        private Button btnCopiar = null!;
        private Button btnCerrar = null!;

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

        private void InitializeComponent()
        {
            lblEstadoBadge = new Label();
            txtRecibo = new TextBox();
            btnCopiar = new Button();
            btnCerrar = new Button();
            SuspendLayout();

            lblEstadoBadge.BackColor = Color.FromArgb(22, 163, 74);
            lblEstadoBadge.ForeColor = Color.White;
            lblEstadoBadge.Location = new Point(12, 12);
            lblEstadoBadge.Size = new Size(760, 34);
            lblEstadoBadge.TextAlign = ContentAlignment.MiddleLeft;

            txtRecibo.Font = new Font("Consolas", 10F);
            txtRecibo.Location = new Point(12, 56);
            txtRecibo.Multiline = true;
            txtRecibo.ReadOnly = true;
            txtRecibo.ScrollBars = ScrollBars.Vertical;
            txtRecibo.Size = new Size(760, 460);

            btnCopiar.Location = new Point(544, 526);
            btnCopiar.Size = new Size(110, 36);
            btnCopiar.Text = "Copiar";
            btnCopiar.Click += btnCopiar_Click;

            btnCerrar.Location = new Point(662, 526);
            btnCerrar.Size = new Size(110, 36);
            btnCerrar.Text = "Cerrar";
            btnCerrar.Click += btnCerrar_Click;

            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 574);
            Controls.Add(btnCerrar);
            Controls.Add(btnCopiar);
            Controls.Add(txtRecibo);
            Controls.Add(lblEstadoBadge);
            Name = "frmRecibo";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Recibo";
            Load += frmRecibo_Load;
            ResumeLayout(false);
            PerformLayout();
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
