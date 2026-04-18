using TallerCaja.Helpers;
using TallerCaja.Models.DTOs;
using TallerCaja.Services;
using Newtonsoft.Json;

namespace TallerCaja.Forms
{
    // ── Formulario para cambiar cantidad de un item en el carrito ─────────────
    public class frmCantidad : Form
    {
        public int NuevaCantidad { get; private set; }
        private NumericUpDown numCantidad;
        private Button btnOk, btnCancelar;
        private Label lblDescripcion;

        public frmCantidad(string nombreItem, int cantidadActual)
        {
            NuevaCantidad = cantidadActual;
            Text = "Cambiar Cantidad";
            ClientSize = new Size(320, 190);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.FromArgb(248, 250, 252);

            lblDescripcion = new Label
            {
                AutoSize = false,
                Size = new Size(280, 40),
                Location = new Point(20, 16),
                Text = $"Cantidad para:\n{(nombreItem.Length > 40 ? nombreItem[..40] + "..." : nombreItem)}",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59)
            };

            numCantidad = new NumericUpDown
            {
                Location = new Point(20, 72),
                Size = new Size(120, 36),
                Font = new Font("Segoe UI", 14F),
                Minimum = 0,
                Maximum = 999,
                Value = cantidadActual,
                TextAlign = HorizontalAlignment.Center
            };

            var lblNota = new Label
            {
                AutoSize = true,
                Location = new Point(155, 80),
                Text = "(0 = eliminar del carrito)",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(100, 116, 139)
            };

            btnOk = new Button
            {
                Location = new Point(20, 130),
                Size = new Size(130, 40),
                Text = "✔ Aceptar",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnOk.FlatAppearance.BorderSize = 0;
            btnOk.Click += (s, e) =>
            {
                NuevaCantidad = (int)numCantidad.Value;
                DialogResult = DialogResult.OK;
                Close();
            };

            btnCancelar = new Button
            {
                Location = new Point(165, 130),
                Size = new Size(110, 40),
                Text = "Cancelar",
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(100, 116, 139),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            Controls.AddRange(new Control[] { lblDescripcion, numCantidad, lblNota, btnOk, btnCancelar });
        }
    }

    // ── Selector de cliente ───────────────────────────────────────────────────
    public class frmSelectorCliente : Form
    {
        public ClienteDto? ClienteSeleccionado { get; private set; }
        private ListView lvClientes;
        private Button btnSeleccionar, btnCancelar;

        public frmSelectorCliente(List<ClienteDto> clientes)
        {
            Text = "Seleccionar Cliente";
            ClientSize = new Size(520, 360);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.FromArgb(248, 250, 252);

            var lblTitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 40,
                Text = "  Selecciona un cliente",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(30, 41, 59),
                TextAlign = ContentAlignment.MiddleLeft
            };

            lvClientes = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                Font = new Font("Segoe UI", 10F),
                BorderStyle = BorderStyle.None
            };
            lvClientes.Columns.Add("Nombre", 200);
            lvClientes.Columns.Add("Cédula", 140);
            lvClientes.Columns.Add("Teléfono", 130);
            lvClientes.DoubleClick += (s, e) => SeleccionarYCerrar();

            foreach (var c in clientes)
            {
                lvClientes.Items.Add(new ListViewItem(new[] { c.Nombre, c.Cedula, c.Telefono }) { Tag = c });
            }

            var panelBtns = new Panel { Dock = DockStyle.Bottom, Height = 50, BackColor = Color.WhiteSmoke, Padding = new Padding(10, 8, 10, 8) };

            btnSeleccionar = new Button
            {
                Location = new Point(10, 8),
                Size = new Size(160, 34),
                Text = "✔ Seleccionar",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.FromArgb(22, 163, 74),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSeleccionar.FlatAppearance.BorderSize = 0;
            btnSeleccionar.Click += (s, e) => SeleccionarYCerrar();

            btnCancelar = new Button
            {
                Location = new Point(185, 8),
                Size = new Size(110, 34),
                Text = "Cancelar",
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(100, 116, 139),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            panelBtns.Controls.AddRange(new Control[] { btnSeleccionar, btnCancelar });
            Controls.Add(lvClientes);
            Controls.Add(panelBtns);
            Controls.Add(lblTitle);
        }

        private void SeleccionarYCerrar()
        {
            if (lvClientes.SelectedItems.Count == 0) return;
            ClienteSeleccionado = lvClientes.SelectedItems[0].Tag as ClienteDto;
            DialogResult = DialogResult.OK;
            Close();
        }
    }

    // ── Buscar y pagar factura pendiente (reparaciones) ───────────────────────
    public class frmBuscarFactura : Form
    {
        private readonly IIntegracionService _integracion;
        private readonly OfflineQueue _queue;
        private readonly int _turnoId;
        private FacturaPendienteDto? _facturaActual;

        private TextBox txtPlaca, txtNumero;
        private Button btnBuscar, btnPagar, btnCerrar;
        private Panel panelDetalle;
        private Label lblFacturaInfo, lblTotalFact;
        private ListView lvItemsFact;
        private ComboBox cmbMetodoPago;
        private TextBox txtMontoPago;
        private Label lblCambioFact;

        public frmBuscarFactura(IIntegracionService integracion, OfflineQueue queue, int turnoId)
        {
            _integracion = integracion;
            _queue = queue;
            _turnoId = turnoId;
            InicializarUI();
        }

        private void InicializarUI()
        {
            Text = "Buscar y Pagar Factura de Reparación";
            ClientSize = new Size(680, 600);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            BackColor = Color.FromArgb(248, 250, 252);

            var panelH = new Panel { Dock = DockStyle.Top, Height = 46, BackColor = Color.FromArgb(15, 23, 42) };
            var lblTitle = new Label { Dock = DockStyle.Fill, Text = "  🔍  PAGAR FACTURA DE REPARACIÓN", Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Color.White, TextAlign = ContentAlignment.MiddleLeft };
            panelH.Controls.Add(lblTitle);

            var panelBusqueda = new Panel { Dock = DockStyle.Top, Height = 70, BackColor = Color.White, Padding = new Padding(15, 10, 15, 10) };
            var lblPlacaLbl = new Label { AutoSize = true, Location = new Point(15, 12), Text = "Placa:", Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(71, 85, 105) };
            txtPlaca = new TextBox { Location = new Point(60, 8), Size = new Size(120, 28), Font = new Font("Segoe UI", 10F), PlaceholderText = "ABC123", CharacterCasing = CharacterCasing.Upper, BorderStyle = BorderStyle.FixedSingle };
            var lblOLbl = new Label { AutoSize = true, Location = new Point(192, 12), Text = "ó  Nº Factura:", Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(71, 85, 105) };
            txtNumero = new TextBox { Location = new Point(295, 8), Size = new Size(160, 28), Font = new Font("Segoe UI", 10F), PlaceholderText = "FAC-2026-0001", BorderStyle = BorderStyle.FixedSingle };
            btnBuscar = new Button { Location = new Point(470, 6), Size = new Size(140, 32), Text = "🔍 Buscar", Font = new Font("Segoe UI", 10F, FontStyle.Bold), BackColor = Color.FromArgb(37, 99, 235), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnBuscar.FlatAppearance.BorderSize = 0;
            btnBuscar.Click += async (s, e) => await BuscarFacturaAsync();
            panelBusqueda.Controls.AddRange(new Control[] { lblPlacaLbl, txtPlaca, lblOLbl, txtNumero, btnBuscar });

            panelDetalle = new Panel { Dock = DockStyle.Fill, BackColor = Color.WhiteSmoke, Padding = new Padding(15), Visible = false };

            lblFacturaInfo = new Label { AutoSize = false, Location = new Point(15, 10), Size = new Size(620, 22), Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59) };
            lblTotalFact = new Label { AutoSize = false, Location = new Point(15, 36), Size = new Size(300, 22), Font = new Font("Segoe UI", 10F), ForeColor = Color.FromArgb(71, 85, 105) };

            lvItemsFact = new ListView { Location = new Point(15, 68), Size = new Size(630, 160), View = View.Details, FullRowSelect = true, GridLines = true, Font = new Font("Segoe UI", 9F), BorderStyle = BorderStyle.FixedSingle };
            lvItemsFact.Columns.Add("Descripción", 310); lvItemsFact.Columns.Add("Cant.", 55); lvItemsFact.Columns.Add("Precio", 110); lvItemsFact.Columns.Add("Subtotal", 110);

            var lblMetLbl = new Label { AutoSize = true, Location = new Point(15, 245), Text = "Método de pago:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59) };
            cmbMetodoPago = new ComboBox { Location = new Point(160, 241), Size = new Size(180, 28), Font = new Font("Segoe UI", 10F), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbMetodoPago.Items.AddRange(new object[] { "Efectivo", "Tarjeta", "Transferencia", "Credito" });
            cmbMetodoPago.SelectedIndex = 0;
            cmbMetodoPago.SelectedIndexChanged += (s, e) => ActualizarCambioFact();

            var lblMontLbl = new Label { AutoSize = true, Location = new Point(15, 285), Text = "Monto recibido:", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59) };
            txtMontoPago = new TextBox { Location = new Point(160, 281), Size = new Size(160, 32), Font = new Font("Segoe UI", 12F), TextAlign = HorizontalAlignment.Right, BorderStyle = BorderStyle.FixedSingle };
            txtMontoPago.TextChanged += (s, e) => ActualizarCambioFact();

            lblCambioFact = new Label { AutoSize = false, Location = new Point(15, 325), Size = new Size(400, 26), Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(22, 163, 74) };

            btnPagar = new Button { Location = new Point(15, 365), Size = new Size(220, 48), Text = "💳  REGISTRAR PAGO", Font = new Font("Segoe UI", 12F, FontStyle.Bold), BackColor = Color.FromArgb(22, 163, 74), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnPagar.FlatAppearance.BorderSize = 0;
            btnPagar.Click += async (s, e) => await PagarFacturaAsync();

            btnCerrar = new Button { Location = new Point(550, 365), Size = new Size(100, 48), Text = "Cerrar", Font = new Font("Segoe UI", 10F), BackColor = Color.FromArgb(100, 116, 139), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.Click += (s, e) => Close();

            panelDetalle.Controls.AddRange(new Control[] { lblFacturaInfo, lblTotalFact, lvItemsFact, lblMetLbl, cmbMetodoPago, lblMontLbl, txtMontoPago, lblCambioFact, btnPagar, btnCerrar });

            Controls.Add(panelDetalle);
            Controls.Add(panelBusqueda);
            Controls.Add(panelH);
        }

        private async Task BuscarFacturaAsync()
        {
            var placa = txtPlaca.Text.Trim();
            var numero = txtNumero.Text.Trim();
            if (string.IsNullOrEmpty(placa) && string.IsNullOrEmpty(numero))
            {
                MessageBox.Show("Ingresa una placa o número de factura.", "Dato requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _facturaActual = await _integracion.BuscarFacturaPendienteAsync(
                    string.IsNullOrEmpty(placa) ? null : placa,
                    string.IsNullOrEmpty(numero) ? null : numero);

                if (_facturaActual == null)
                {
                    MessageBox.Show("No se encontró factura pendiente con esos datos.", "Sin resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    panelDetalle.Visible = false;
                    return;
                }

                lblFacturaInfo.Text = $"Factura: {_facturaActual.Numero}  |  Cliente: {_facturaActual.ClienteNombre}  |  Vehículo: {_facturaActual.VehiculoInfo}";
                lblTotalFact.Text = $"Total a cobrar: {MonedaHelper.Formatear(_facturaActual.Total)}";

                lvItemsFact.Items.Clear();
                foreach (var item in _facturaActual.Items)
                    lvItemsFact.Items.Add(new ListViewItem(new[] { item.Nombre, item.Cantidad.ToString(), MonedaHelper.Formatear(item.Precio), MonedaHelper.Formatear(item.Subtotal) }));

                txtMontoPago.Text = _facturaActual.Total.ToString("N2");
                panelDetalle.Visible = true;
                ActualizarCambioFact();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar factura: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarCambioFact()
        {
            if (_facturaActual == null) return;
            var metodo = cmbMetodoPago.SelectedItem?.ToString() ?? "Efectivo";
            if (metodo != "Efectivo") { lblCambioFact.Text = ""; return; }
            if (decimal.TryParse(txtMontoPago.Text, out decimal monto))
            {
                var cambio = FacturaCalculator.CalcularCambio(_facturaActual.Total, monto);
                lblCambioFact.Text = $"Cambio a entregar: {MonedaHelper.Formatear(cambio)}";
                lblCambioFact.ForeColor = cambio >= 0 ? Color.FromArgb(22, 163, 74) : Color.FromArgb(239, 68, 68);
            }
        }

        private async Task PagarFacturaAsync()
        {
            if (_facturaActual == null) return;
            if (!decimal.TryParse(txtMontoPago.Text, out decimal monto)) { MessageBox.Show("Monto inválido."); return; }
            var metodo = cmbMetodoPago.SelectedItem?.ToString() ?? "Efectivo";
            if (metodo == "Efectivo" && monto < _facturaActual.Total)
            {
                MessageBox.Show($"Monto insuficiente. Total: {MonedaHelper.Formatear(_facturaActual.Total)}", "Monto insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var req = new PagoFacturaRequest { TurnoId = SessionManager.TurnoId, MetodoPago = metodo, MontoPagado = monto };
                var resp = await _integracion.PagarFacturaAsync(_facturaActual.Id, req);
                if (resp != null)
                {
                    var cambio = metodo == "Efectivo" ? FacturaCalculator.CalcularCambio(_facturaActual.Total, monto) : 0m;
                    MessageBox.Show($"Pago registrado.\nFactura: {resp.Numero}\nTotal: {MonedaHelper.Formatear(resp.Total)}\nEstado: {resp.Estado}" + (metodo == "Efectivo" ? $"\nCambio: {MonedaHelper.Formatear(cambio)}" : ""),
                        "Pago registrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    panelDetalle.Visible = false;
                    txtPlaca.Clear(); txtNumero.Clear();
                    _facturaActual = null;
                }
            }
            catch
            {
                _queue.Encolar("pago_factura", new { facturaId = _facturaActual.Id, turnoId = SessionManager.TurnoId, metodoPago = metodo, montoPagado = monto });
                MessageBox.Show("Sin conexión. El pago fue guardado para sincronizar más tarde.", "Modo offline", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
            }
        }
    }
}
