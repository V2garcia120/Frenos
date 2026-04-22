using TallerCaja.Helpers;
using TallerCaja.Models.DTOs;
using TallerCaja.Services;

namespace TallerCaja.Forms
{
    public partial class frmBuscarFactura : Form
    {
        private readonly IIntegracionService _integracion;
        private readonly OfflineQueue _queue;
        private readonly int _localTurnoId;

        private FacturaPendienteDto? _facturaActual;

        private Label lblPlaca = null!;
        private TextBox txtPlaca = null!;
        private Label lblNumero = null!;
        private TextBox txtNumero = null!;
        private Button btnBuscar = null!;
        private Label lblInfo = null!;
        private ListView lvItems = null!;
        private ColumnHeader colItem = null!;
        private ColumnHeader colCantidad = null!;
        private ColumnHeader colPrecio = null!;
        private ColumnHeader colSubtotal = null!;
        private Label lblTotalLabel = null!;
        private Label lblTotal = null!;
        private ComboBox cmbMetodoPago = null!;
        private Label lblMetodoPago = null!;
        private Button btnPagar = null!;
        private Button btnCerrar = null!;

        public frmBuscarFactura()
        {
            _integracion = null!;
            _queue = null!;
            _localTurnoId = 0;
            InitializeComponent();
        }

        public frmBuscarFactura(IIntegracionService integracion, OfflineQueue queue, int localTurnoId)
        {
            _integracion = integracion;
            _queue = queue;
            _localTurnoId = localTurnoId;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            lblPlaca = new Label();
            txtPlaca = new TextBox();
            lblNumero = new Label();
            txtNumero = new TextBox();
            btnBuscar = new Button();
            lblInfo = new Label();
            lvItems = new ListView();
            colItem = new ColumnHeader();
            colCantidad = new ColumnHeader();
            colPrecio = new ColumnHeader();
            colSubtotal = new ColumnHeader();
            lblTotalLabel = new Label();
            lblTotal = new Label();
            cmbMetodoPago = new ComboBox();
            lblMetodoPago = new Label();
            btnPagar = new Button();
            btnCerrar = new Button();
            SuspendLayout();

            lblPlaca.Location = new Point(12, 14);
            lblPlaca.Size = new Size(70, 25);
            lblPlaca.Text = "Placa:";

            txtPlaca.Location = new Point(86, 11);
            txtPlaca.Size = new Size(180, 31);

            lblNumero.Location = new Point(282, 14);
            lblNumero.Size = new Size(82, 25);
            lblNumero.Text = "Factura:";

            txtNumero.Location = new Point(366, 11);
            txtNumero.Size = new Size(180, 31);

            btnBuscar.Location = new Point(560, 10);
            btnBuscar.Size = new Size(120, 33);
            btnBuscar.Text = "Buscar";
            btnBuscar.Click += btnBuscar_Click;

            lblInfo.Location = new Point(12, 52);
            lblInfo.Size = new Size(780, 48);
            lblInfo.Text = "Ingrese placa o número de factura pendiente.";

            lvItems.Columns.AddRange(new[] { colItem, colCantidad, colPrecio, colSubtotal });
            lvItems.FullRowSelect = true;
            lvItems.GridLines = true;
            lvItems.HideSelection = false;
            lvItems.Location = new Point(12, 103);
            lvItems.Size = new Size(780, 285);
            lvItems.View = View.Details;

            colItem.Text = "Descripción";
            colItem.Width = 380;
            colCantidad.Text = "Cant.";
            colCantidad.Width = 90;
            colPrecio.Text = "Precio";
            colPrecio.Width = 130;
            colSubtotal.Text = "Subtotal";
            colSubtotal.Width = 140;

            lblTotalLabel.Location = new Point(492, 398);
            lblTotalLabel.Size = new Size(90, 28);
            lblTotalLabel.Text = "Total:";

            lblTotal.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTotal.Location = new Point(588, 396);
            lblTotal.Size = new Size(204, 30);
            lblTotal.Text = "RD$ 0.00";
            lblTotal.TextAlign = ContentAlignment.MiddleRight;

            lblMetodoPago.Location = new Point(12, 398);
            lblMetodoPago.Size = new Size(126, 28);
            lblMetodoPago.Text = "Método pago:";

            cmbMetodoPago.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMetodoPago.Items.AddRange(new object[] { "Efectivo", "Tarjeta", "Transferencia", "Credito" });
            cmbMetodoPago.Location = new Point(142, 395);
            cmbMetodoPago.Size = new Size(190, 33);
            cmbMetodoPago.SelectedIndex = 0;

            btnPagar.Enabled = false;
            btnPagar.Location = new Point(536, 438);
            btnPagar.Size = new Size(124, 38);
            btnPagar.Text = "Pagar";
            btnPagar.Click += btnPagar_Click;

            btnCerrar.Location = new Point(668, 438);
            btnCerrar.Size = new Size(124, 38);
            btnCerrar.Text = "Cerrar";
            btnCerrar.Click += btnCerrar_Click;

            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(804, 488);
            Controls.Add(btnCerrar);
            Controls.Add(btnPagar);
            Controls.Add(cmbMetodoPago);
            Controls.Add(lblMetodoPago);
            Controls.Add(lblTotal);
            Controls.Add(lblTotalLabel);
            Controls.Add(lvItems);
            Controls.Add(lblInfo);
            Controls.Add(btnBuscar);
            Controls.Add(txtNumero);
            Controls.Add(lblNumero);
            Controls.Add(txtPlaca);
            Controls.Add(lblPlaca);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmBuscarFactura";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Buscar factura pendiente";
            ResumeLayout(false);
            PerformLayout();
        }

        private async void btnBuscar_Click(object? sender, EventArgs e)
        {
            var placa = txtPlaca.Text.Trim();
            var numero = txtNumero.Text.Trim();
            if (string.IsNullOrEmpty(placa) && string.IsNullOrEmpty(numero))
            {
                MessageBox.Show("Ingresa placa o número de factura.", "Búsqueda", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            btnBuscar.Enabled = false;
            try
            {
                _facturaActual = await _integracion.BuscarFacturaPendienteAsync(
                    string.IsNullOrEmpty(placa) ? null : placa,
                    string.IsNullOrEmpty(numero) ? null : numero);

                if (_facturaActual == null)
                {
                    LimpiarDetalle();
                    lblInfo.Text = "No se encontró factura pendiente con los datos suministrados.";
                    MessageBox.Show("No se encontró factura pendiente.", "Sin resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                lblInfo.Text = $"Factura: {_facturaActual.Numero} | Cliente: {_facturaActual.ClienteNombre} | Vehículo: {_facturaActual.VehiculoInfo}";
                CargarDetalle(_facturaActual);
            }
            catch
            {
                LimpiarDetalle();
                lblInfo.Text = "No fue posible consultar la factura en este momento.";
                MessageBox.Show("Error de conexión al buscar la factura.", "Conexión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                btnBuscar.Enabled = true;
            }
        }

        private void CargarDetalle(FacturaPendienteDto factura)
        {
            lvItems.Items.Clear();
            foreach (var item in factura.Items)
            {
                lvItems.Items.Add(new ListViewItem(new[]
                {
                    item.Nombre,
                    item.Cantidad.ToString(),
                    MonedaHelper.Formatear(item.Precio),
                    MonedaHelper.Formatear(item.Subtotal)
                }));
            }

            lblTotal.Text = MonedaHelper.Formatear(factura.Total);
            btnPagar.Enabled = true;
        }

        private void LimpiarDetalle()
        {
            _facturaActual = null;
            lvItems.Items.Clear();
            lblTotal.Text = "RD$ 0.00";
            btnPagar.Enabled = false;
        }

        private async void btnPagar_Click(object? sender, EventArgs e)
        {
            if (_facturaActual == null) return;

            var metodoPago = cmbMetodoPago.SelectedItem?.ToString() ?? "Efectivo";
            var request = new PagoFacturaRequest
            {
                TurnoId = SessionManager.TurnoId != 0 ? SessionManager.TurnoId : _localTurnoId,
                MetodoPago = metodoPago,
                MontoPagado = _facturaActual.Total
            };

            btnPagar.Enabled = false;
            try
            {
                var respuesta = await _integracion.PagarFacturaAsync(_facturaActual.Id, request);
                if (respuesta == null)
                {
                    MessageBox.Show("No se pudo procesar el pago.", "Pago", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnPagar.Enabled = true;
                    return;
                }

                MostrarReciboPagoFactura(respuesta, request, metodoPago);
                MessageBox.Show($"Factura {respuesta.Numero} pagada correctamente.", "Pago aplicado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch
            {
                _queue.Encolar("pago_factura", new
                {
                    FacturaId = _facturaActual.Id,
                    Request = request
                });

                MessageBox.Show("No hay conexión. El pago quedó pendiente para sincronización.", "Modo offline", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnCerrar_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void MostrarReciboPagoFactura(PagoFacturaResponse respuesta, PagoFacturaRequest request, string metodoPago)
        {
            if (_facturaActual == null) return;

            var cobro = new CobroResponse
            {
                FacturaId = respuesta.FacturaId,
                NumeroFactura = respuesta.Numero,
                Total = respuesta.Total,
                Cambio = respuesta.Cambio,
                Estado = respuesta.Estado,
                IdLocal = Guid.NewGuid().ToString()
            };

            var cobroRequest = new CobroRequest
            {
                TurnoId = request.TurnoId,
                ClienteId = 0,
                MetodoPago = metodoPago,
                MontoPagado = request.MontoPagado,
                Items = _facturaActual.Items.Select(i => new ItemCobroDto
                {
                    Tipo = "Factura",
                    ItemId = 0,
                    Cantidad = i.Cantidad,
                    PrecioSnapshot = i.Precio,
                    NombreSnapshot = i.Nombre
                }).ToList()
            };

            var texto = new ReciboService().GenerarTextoRecibo(cobro, cobroRequest, _facturaActual.ClienteNombre);
            using var frmRecibo = new frmRecibo(texto, cobro);
            frmRecibo.ShowDialog(this);
        }
    }
}
