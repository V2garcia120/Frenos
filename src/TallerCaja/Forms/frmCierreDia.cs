using TallerCaja.Helpers;
using TallerCaja.Services;

namespace TallerCaja.Forms
{
    public partial class frmCierreDia : Form
    {
        private readonly IIntegracionService _integracion;
        private readonly ICajaLocalService _local;
        private readonly int _localTurnoId;

        public frmCierreDia(IIntegracionService integracion, ICajaLocalService local, int localTurnoId)
        {
            _integracion = integracion;
            _local = local;
            _localTurnoId = localTurnoId;
            InitializeComponent();
        }

        private void frmCierreDia_Load(object sender, EventArgs e)
        {
            lblCajero.Text = SessionManager.CajeroNombre;
            lblTurno.Text = $"#{SessionManager.TurnoId}";
            lblFechaApertura.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            var ventas = _local.ObtenerVentasDelTurno(_localTurnoId);
            var totalVentas = ventas.Sum(v => v.Total);
            var ventasEfectivo = ventas.Where(v => v.MetodoPago == "Efectivo").Sum(v => v.MontoPagado - v.Cambio);

            lblTotalVentas.Text = MonedaHelper.Formatear(totalVentas);
            lblVentasEfectivo.Text = MonedaHelper.Formatear(ventasEfectivo);
            lblCantVentas.Text = $"{ventas.Count} transacciones";
        }

        private void txtEfectivoContado_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtEfectivoContado.Text, out decimal contado))
            {
                var ventas = _local.ObtenerVentasDelTurno(_localTurnoId);
                var turno = _local.ObtenerTurnoActivo(SessionManager.CajeroId);
                var efectivoEsperado = (turno?.MontoInicial ?? 0) + ventas.Where(v => v.MetodoPago == "Efectivo").Sum(v => v.MontoPagado - v.Cambio);
                var diferencia = contado - efectivoEsperado;

                lblEfectivoEsperado.Text = MonedaHelper.Formatear(efectivoEsperado);
                lblDiferencia.Text = MonedaHelper.Formatear(Math.Abs(diferencia));
                lblDiferencia.ForeColor = diferencia >= 0 ? Color.FromArgb(22, 163, 74) : Color.FromArgb(239, 68, 68);
                lblSignoDif.Text = diferencia >= 0 ? "▲ Sobrante:" : "▼ Faltante:";
                lblSignoDif.ForeColor = lblDiferencia.ForeColor;
            }
        }

        private async void btnCerrar_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtEfectivoContado.Text, out decimal contado))
            {
                MessageBox.Show("Ingresa el efectivo contado.", "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("¿Confirmas el cierre del turno? Esta acción no se puede deshacer.",
                "Confirmar cierre", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            btnCerrar.Enabled = false;
            btnCerrar.Text = "Cerrando...";

            try
            {
                var request = new Models.DTOs.CerrarTurnoRequest
                {
                    TurnoId = SessionManager.TurnoId,
                    EfectivoContado = contado,
                    Observaciones = txtObservaciones.Text
                };

                var resp = await _integracion.CerrarTurnoAsync(request);

                var turno = _local.ObtenerTurnoActivo(SessionManager.CajeroId);
                if (turno != null)
                {
                    turno.Estado = "Cerrado";
                    turno.EfectivoContado = contado;
                    turno.FechaCierre = DateTime.Now;
                    turno.Observaciones = txtObservaciones.Text;
                    _local.ActualizarTurnoLocal(turno);
                }

                MessageBox.Show("Turno cerrado correctamente.", "Turno cerrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch
            {
                var turno = _local.ObtenerTurnoActivo(SessionManager.CajeroId);
                if (turno != null)
                {
                    turno.Estado = "Cerrado"; turno.EfectivoContado = contado;
                    turno.FechaCierre = DateTime.Now; turno.Observaciones = txtObservaciones.Text;
                    _local.ActualizarTurnoLocal(turno);
                }
                MessageBox.Show("Turno cerrado localmente. Se sincronizará cuando haya conexión.",
                    "Modo Offline", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnCancelar_Click(object s, EventArgs e) { DialogResult = DialogResult.Cancel; Close(); }
    }

    partial class frmCierreDia
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelH = new Panel();
            lblTitle = new Label();
            panelBody = new Panel();

            lblCajeroLbl = MkLbl("Cajero:", 20, 20, true); lblCajero = MkLbl("", 120, 20);
            lblTurnoLbl = MkLbl("Turno:", 20, 46, true); lblTurno = MkLbl("", 120, 46);
            lblFechaLbl = MkLbl("Apertura:", 20, 72, true); lblFechaApertura = MkLbl("", 120, 72);
            lblVentasLbl = MkLbl("Ventas del turno:", 20, 106, true); lblTotalVentas = MkLbl("", 220, 106);
            lblEfEspLbl = MkLbl("Efectivo en ventas:", 20, 132, true); lblVentasEfectivo = MkLbl("", 220, 132);
            lblCantLbl = MkLbl("Transacciones:", 20, 158, true); lblCantVentas = MkLbl("", 220, 158);

            var sep = new Label { AutoSize = false, Size = new Size(460, 2), Location = new Point(20, 185), BackColor = Color.FromArgb(226, 232, 240) };

            var lblContadoLbl = MkLbl("Efectivo contado (físico):", 20, 200, true);
            txtEfectivoContado = new TextBox { Location = new Point(20, 224), Size = new Size(160, 32), Font = new Font("Segoe UI", 13F), Text = "0.00", TextAlign = HorizontalAlignment.Right, BorderStyle = BorderStyle.FixedSingle };
            txtEfectivoContado.TextChanged += txtEfectivoContado_TextChanged;

            lblEfEspDetLbl = MkLbl("Efectivo esperado:", 20, 268, true);
            lblEfectivoEsperado = MkLbl("---", 220, 268);
            lblSignoDif = MkLbl("▲ Sobrante:", 20, 290, true);
            lblDiferencia = MkLbl("---", 220, 290);

            var lblObsLbl = MkLbl("Observaciones (opcional):", 20, 320, true);
            txtObservaciones = new TextBox { Location = new Point(20, 344), Size = new Size(440, 60), Multiline = true, BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 10F) };

            btnCerrar = new Button { Location = new Point(20, 428), Size = new Size(220, 44), Text = "⏹ Cerrar Turno", Font = new Font("Segoe UI", 12F, FontStyle.Bold) };
            btnCerrar.BackColor = Color.FromArgb(239, 68, 68); btnCerrar.ForeColor = Color.White;
            btnCerrar.FlatStyle = FlatStyle.Flat; btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.Cursor = Cursors.Hand; btnCerrar.Click += btnCerrar_Click;

            btnCancelar = new Button { Location = new Point(255, 428), Size = new Size(120, 44), Text = "Cancelar", Font = new Font("Segoe UI", 11F) };
            btnCancelar.BackColor = Color.FromArgb(100, 116, 139); btnCancelar.ForeColor = Color.White;
            btnCancelar.FlatStyle = FlatStyle.Flat; btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Cursor = Cursors.Hand; btnCancelar.Click += btnCancelar_Click;

            panelH.Dock = DockStyle.Top; panelH.Height = 50; panelH.BackColor = Color.FromArgb(15, 23, 42);
            lblTitle.Dock = DockStyle.Fill; lblTitle.Text = "  ⏹  CIERRE DE TURNO";
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold); lblTitle.ForeColor = Color.White;
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            panelH.Controls.Add(lblTitle);

            panelBody.Dock = DockStyle.Fill; panelBody.BackColor = Color.FromArgb(248, 250, 252);
            panelBody.Controls.AddRange(new Control[] {
                lblCajeroLbl, lblCajero, lblTurnoLbl, lblTurno, lblFechaLbl, lblFechaApertura,
                lblVentasLbl, lblTotalVentas, lblEfEspLbl, lblVentasEfectivo, lblCantLbl, lblCantVentas,
                sep, lblContadoLbl, txtEfectivoContado,
                lblEfEspDetLbl, lblEfectivoEsperado, lblSignoDif, lblDiferencia,
                lblObsLbl, txtObservaciones, btnCerrar, btnCancelar
            });

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(500, 500);
            Controls.Add(panelBody); Controls.Add(panelH);
            FormBorderStyle = FormBorderStyle.FixedDialog; MaximizeBox = false;
            Name = "frmCierreDia"; StartPosition = FormStartPosition.CenterParent;
            Text = "Cierre de Turno"; Load += frmCierreDia_Load;
            ResumeLayout(false);
        }

        private static Label MkLbl(string text, int x, int y, bool bold = false)
        {
            var l = new Label { AutoSize = true, Location = new Point(x, y), Text = text };
            l.Font = new Font("Segoe UI", 10F, bold ? FontStyle.Bold : FontStyle.Regular);
            l.ForeColor = bold ? Color.FromArgb(30, 41, 59) : Color.FromArgb(71, 85, 105);
            return l;
        }

        private Panel panelH, panelBody;
        private Label lblTitle;
        private Label lblCajeroLbl, lblCajero, lblTurnoLbl, lblTurno, lblFechaLbl, lblFechaApertura;
        private Label lblVentasLbl, lblTotalVentas, lblEfEspLbl, lblVentasEfectivo, lblCantLbl, lblCantVentas;
        private TextBox txtEfectivoContado;
        private Label lblEfEspDetLbl, lblEfectivoEsperado, lblSignoDif, lblDiferencia;
        private TextBox txtObservaciones;
        private Button btnCerrar, btnCancelar;
    }
}
