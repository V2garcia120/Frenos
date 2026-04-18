using TallerCaja.Models.DTOs;

namespace TallerCaja.Forms
{
    public partial class frmRecibo : Form
    {
        private readonly string _textoRecibo;
        private readonly CobroResponse _cobro;

        public frmRecibo(string textoRecibo, CobroResponse cobro)
        {
            _textoRecibo = textoRecibo;
            _cobro = cobro;
            InitializeComponent();
        }

        private void frmRecibo_Load(object sender, EventArgs e)
        {
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

    partial class frmRecibo
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblEstadoBadge = new Label();
            txtRecibo = new RichTextBox();
            panelBtns = new Panel();
            btnCopiar = new Button();
            btnCerrar = new Button();

            SuspendLayout();

            lblEstadoBadge.Dock = DockStyle.Top;
            lblEstadoBadge.Height = 36;
            lblEstadoBadge.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblEstadoBadge.ForeColor = Color.White;
            lblEstadoBadge.TextAlign = ContentAlignment.MiddleCenter;

            txtRecibo.Dock = DockStyle.Fill;
            txtRecibo.Font = new Font("Consolas", 10F);
            txtRecibo.BackColor = Color.FromArgb(15, 23, 42);
            txtRecibo.ForeColor = Color.White;
            txtRecibo.ReadOnly = true;
            txtRecibo.BorderStyle = BorderStyle.None;
            txtRecibo.Padding = new Padding(10);

            panelBtns.Dock = DockStyle.Bottom;
            panelBtns.Height = 50;
            panelBtns.BackColor = Color.FromArgb(30, 41, 59);
            panelBtns.Padding = new Padding(10, 8, 10, 8);

            btnCopiar.Location = new Point(10, 8);
            btnCopiar.Size = new Size(140, 34);
            btnCopiar.Text = "📋 Copiar recibo";
            btnCopiar.Font = new Font("Segoe UI", 10F);
            btnCopiar.BackColor = Color.FromArgb(59, 130, 246);
            btnCopiar.ForeColor = Color.White;
            btnCopiar.FlatStyle = FlatStyle.Flat;
            btnCopiar.FlatAppearance.BorderSize = 0;
            btnCopiar.Cursor = Cursors.Hand;
            btnCopiar.Click += btnCopiar_Click;

            btnCerrar.Location = new Point(165, 8);
            btnCerrar.Size = new Size(120, 34);
            btnCerrar.Text = "✓ Nueva venta";
            btnCerrar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCerrar.BackColor = Color.FromArgb(22, 163, 74);
            btnCerrar.ForeColor = Color.White;
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.Cursor = Cursors.Hand;
            btnCerrar.Click += btnCerrar_Click;

            panelBtns.Controls.AddRange(new Control[] { btnCopiar, btnCerrar });

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(460, 580);
            Controls.Add(txtRecibo);
            Controls.Add(lblEstadoBadge);
            Controls.Add(panelBtns);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmRecibo";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Recibo de Venta";
            Load += frmRecibo_Load;
            ResumeLayout(false);
        }

        private Label lblEstadoBadge;
        private RichTextBox txtRecibo;
        private Panel panelBtns;
        private Button btnCopiar, btnCerrar;
    }
}
