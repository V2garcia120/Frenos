using TallerCaja.Models.DTOs;
using System.Drawing.Printing;

namespace TallerCaja.Forms
{
    public partial class frmRecibo : Form
    {
        private readonly string _textoRecibo;
        private readonly CobroResponse _cobro;

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
            btnImprimir = new Button();
            btnCerrar = new Button();
            printDocument = new PrintDocument();
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

            btnImprimir.Location = new Point(426, 526);
            btnImprimir.Size = new Size(110, 36);
            btnImprimir.Text = "Imprimir";
            btnImprimir.Click += btnImprimir_Click;

            btnCerrar.Location = new Point(662, 526);
            btnCerrar.Size = new Size(110, 36);
            btnCerrar.Text = "Cerrar";
            btnCerrar.Click += btnCerrar_Click;

            printDocument.PrintPage += printDocument_PrintPage;

            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 574);
            Controls.Add(btnCerrar);
            Controls.Add(btnImprimir);
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
            _lineasRecibo = _textoRecibo.Replace("\r", string.Empty).Split('\n');
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
