using TallerCaja.Models.DTOs;
using System.Drawing.Printing;

namespace TallerCaja.Forms
{
    public partial class frmRecibo : Form
    {
        private readonly string _textoRecibo;
        private readonly CobroResponse _cobro;
        private readonly string? _badgePersonalizado;
        private readonly Color? _badgeColorPersonalizado;
        private readonly string _tituloVentana;

        private Label lblEstadoBadge = null!;
        private TextBox txtRecibo = null!;
        private Button btnCopiar = null!;
        private Button btnImprimir = null!;
        private Button btnCerrar = null!;
        private PrintDocument printDocument = null!;
        private int _lineaActualImpresion;
        private string[] _lineasRecibo = Array.Empty<string>();

        public frmRecibo()
        {
            _textoRecibo = "=== RECIBO DEMO ===";
            _cobro = new CobroResponse { Estado = "Completado", NumeroFactura = "FAC-DEMO" };
            _badgePersonalizado = null;
            _badgeColorPersonalizado = null;
            _tituloVentana = "Recibo";
            InitializeComponent();
        }

        public frmRecibo(string textoRecibo, CobroResponse cobro)
        {
            _textoRecibo = textoRecibo;
            _cobro = cobro;
            _badgePersonalizado = null;
            _badgeColorPersonalizado = null;
            _tituloVentana = "Recibo";
            InitializeComponent();
        }

        public frmRecibo(string textoRecibo, string badgeTexto, Color badgeColor, string tituloVentana)
        {
            _textoRecibo = textoRecibo;
            _cobro = new CobroResponse { Estado = "Completado", NumeroFactura = "" };
            _badgePersonalizado = badgeTexto;
            _badgeColorPersonalizado = badgeColor;
            _tituloVentana = tituloVentana;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            lblEstadoBadge = new Label();
            txtRecibo = new TextBox();
            btnCopiar = new Button();
            btnImprimir = new Button();
            btnCerrar = new Button();
            printDocument = new PrintDocument();
            SuspendLayout();

            lblEstadoBadge.BackColor = Color.FromArgb(22, 163, 74);
            lblEstadoBadge.ForeColor = Color.White;
            lblEstadoBadge.Location = new Point(12, 12);
            lblEstadoBadge.Size = new Size(896, 34);
            lblEstadoBadge.TextAlign = ContentAlignment.MiddleLeft;

            txtRecibo.Font = new Font("Consolas", 10F);
            txtRecibo.Location = new Point(12, 56);
            txtRecibo.Multiline = true;
            txtRecibo.ReadOnly = true;
            txtRecibo.WordWrap = false;
            txtRecibo.ScrollBars = ScrollBars.Both;
            txtRecibo.Size = new Size(896, 578);

            btnCopiar.Location = new Point(680, 644);
            btnCopiar.Size = new Size(110, 36);
            btnCopiar.Text = "Copiar";
            btnCopiar.Click += btnCopiar_Click;

            btnImprimir.Location = new Point(562, 644);
            btnImprimir.Size = new Size(110, 36);
            btnImprimir.Text = "Imprimir";
            btnImprimir.Click += btnImprimir_Click;

            btnCerrar.Location = new Point(798, 644);
            btnCerrar.Size = new Size(110, 36);
            btnCerrar.Text = "Cerrar";
            btnCerrar.Click += btnCerrar_Click;

            printDocument.PrintPage += printDocument_PrintPage;

            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(920, 692);
            Controls.Add(btnCerrar);
            Controls.Add(btnImprimir);
            Controls.Add(btnCopiar);
            Controls.Add(txtRecibo);
            Controls.Add(lblEstadoBadge);
            Name = "frmRecibo";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Text = _tituloVentana;
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
            _lineasRecibo = _textoRecibo.Replace("\r", string.Empty).Split('\n');
            AjustarTamanoVentana();
            if (!string.IsNullOrWhiteSpace(_badgePersonalizado) && _badgeColorPersonalizado.HasValue)
            {
                lblEstadoBadge.Text = _badgePersonalizado;
                lblEstadoBadge.BackColor = _badgeColorPersonalizado.Value;
                return;
            }

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

        private void AjustarTamanoVentana()
        {
            using var font = new Font("Consolas", 10F);
            var lineas = _lineasRecibo.Length > 0 ? _lineasRecibo : new[] { _textoRecibo };
            var maxLen = lineas.Max(l => l?.Length ?? 0);

            var anchoTexto = TextRenderer.MeasureText(new string('W', Math.Max(1, maxLen)), font).Width;
            var altoLinea = TextRenderer.MeasureText("Ag", font).Height + 2;

            var anchoRecibo = Math.Clamp(anchoTexto + 24, 420, 760);
            var altoRecibo = Math.Clamp((lineas.Length * altoLinea) + 24, 320, 620);

            txtRecibo.Size = new Size(anchoRecibo, altoRecibo);
            lblEstadoBadge.Size = new Size(anchoRecibo, 34);

            var anchoCliente = anchoRecibo + 24;
            var topBotones = txtRecibo.Bottom + 10;
            var margenDerecho = anchoCliente - 12;

            btnCerrar.Location = new Point(margenDerecho - btnCerrar.Width, topBotones);
            btnCopiar.Location = new Point(btnCerrar.Left - 8 - btnCopiar.Width, topBotones);
            btnImprimir.Location = new Point(btnCopiar.Left - 8 - btnImprimir.Width, topBotones);

            var altoCliente = btnCerrar.Bottom + 12;
            ClientSize = new Size(anchoCliente, altoCliente);
        }

        private void btnCopiar_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(_textoRecibo);
            MessageBox.Show("Recibo copiado al portapapeles.", "Copiado", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnImprimir_Click(object? sender, EventArgs e)
        {
            _lineaActualImpresion = 0;

            using var printDialog = new PrintDialog
            {
                Document = printDocument,
                UseEXDialog = true
            };

            if (printDialog.ShowDialog(this) == DialogResult.OK)
                printDocument.Print();
        }

        private void printDocument_PrintPage(object? sender, PrintPageEventArgs e)
        {
            using var font = new Font("Consolas", 10F);
            var lineHeight = font.GetHeight(e.Graphics) + 2;
            float y = e.MarginBounds.Top;

            while (_lineaActualImpresion < _lineasRecibo.Length)
            {
                if (y + lineHeight > e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }

                e.Graphics.DrawString(_lineasRecibo[_lineaActualImpresion], font, Brushes.Black, e.MarginBounds.Left, y);
                y += lineHeight;
                _lineaActualImpresion++;
            }

            _lineaActualImpresion = 0;
            e.HasMorePages = false;
        }

        private void btnCerrar_Click(object sender, EventArgs e) => Close();
    }
}
