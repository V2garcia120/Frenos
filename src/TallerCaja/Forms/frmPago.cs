using TallerCaja.Helpers;
using TallerCaja.Models.DTOs;
using TallerCaja.Models.Entities;
using TallerCaja.Services;
using Newtonsoft.Json;

namespace TallerCaja.Forms
{
    public partial class frmPago : Form
    {
        private readonly IIntegracionService _integracion;
        private readonly ICajaLocalService _local;
        private readonly OfflineQueue _queue;
        private readonly List<ItemCobroDto> _items;
        private readonly ClienteDto _cliente;
        private readonly decimal _total;
        private readonly int _localTurnoId;

        private decimal _subtotal;
        private decimal _itbis;
        private CobroResponse? _ultimaRespuesta;
        private CobroRequest? _ultimoRequest;
        private string _metodoPagoActual = "Efectivo";

        public frmPago()
        {
            _integracion = null!;
            _local = null!;
            _queue = null!;
            _items = new List<ItemCobroDto>();
            _cliente = new ClienteDto { Id = 0, Nombre = "Cliente de prueba", EsAnonimo = true };
            _total = 0m;
            _localTurnoId = 0;
            InitializeComponent();
        }

        public frmPago(IIntegracionService integracion, ICajaLocalService local,
            OfflineQueue queue, List<ItemCobroDto> items, ClienteDto cliente,
            decimal total, int localTurnoId)
        {
            _integracion = integracion;
            _local = local;
            _queue = queue;
            _items = items;
            _cliente = cliente;
            _total = total;
            _localTurnoId = localTurnoId;
            InitializeComponent();
        }

        private void frmPago_Load(object sender, EventArgs e)
        {
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
            {
                lblClienteVal.Text = "Cliente de prueba";
                lblSubtotalVal.Text = "RD$ 0.00";
                lblITBISVal.Text = "RD$ 0.00";
                lblTotalVal.Text = "RD$ 0.00";
                txtMontoPagado.Text = "0.00";
                lblCambioVal.Text = "RD$ 0.00";
                return;
            }

            (_subtotal, _itbis, _) = FacturaCalculator.Calcular(_items);
            MostrarResumen();
            SeleccionarMetodoPago("Efectivo");
            txtMontoPagado.Focus();
            txtMontoPagado.SelectAll();
        }

        private void MostrarResumen()
        {
            lblClienteVal.Text = _cliente.Nombre;
            lblSubtotalVal.Text = MonedaHelper.Formatear(_subtotal);
            lblITBISVal.Text = MonedaHelper.Formatear(_itbis);
            lblTotalVal.Text = MonedaHelper.Formatear(_total);
            txtMontoPagado.Text = _total.ToString("N2");

            lvItems.Items.Clear();
            foreach (var item in _items)
            {
                lvItems.Items.Add(new ListViewItem(new[]
                {
                    item.NombreSnapshot,
                    item.Tipo,
                    item.Cantidad.ToString(),
                    MonedaHelper.Formatear(item.PrecioSnapshot),
                    MonedaHelper.Formatear(item.PrecioSnapshot * item.Cantidad)
                }));
            }
        }

        private void SeleccionarMetodoPago(string metodo)
        {
            _metodoPagoActual = metodo;

            // Reset botones
            foreach (var btn in new[] { btnEfectivo, btnTarjeta, btnTransferencia, btnCredito })
            {
                btn.BackColor = Color.FromArgb(51, 65, 85);
                btn.ForeColor = Color.FromArgb(148, 163, 184);
            }

            // Activar botón seleccionado
            Button btnActivo = metodo switch
            {
                "Tarjeta" => btnTarjeta,
                "Transferencia" => btnTransferencia,
                "Credito" => btnCredito,
                _ => btnEfectivo
            };
            btnActivo.BackColor = Color.FromArgb(37, 99, 235);
            btnActivo.ForeColor = Color.White;

            // Lógica por método de pago
            switch (metodo)
            {
                case "Efectivo":
                    txtMontoPagado.Enabled = true;
                    txtMontoPagado.Text = Math.Ceiling(_total).ToString("N2");
                    lblMontoPagadoLabel.Text = "Monto recibido (efectivo):";
                    panelCambio.Visible = true;
                    lblInfoMetodo.Text = "💵 Ingresa el monto recibido del cliente para calcular el cambio.";
                    lblInfoMetodo.ForeColor = Color.FromArgb(74, 222, 128);
                    break;

                case "Tarjeta":
                    txtMontoPagado.Enabled = false;
                    txtMontoPagado.Text = _total.ToString("N2");
                    lblMontoPagadoLabel.Text = "Monto a cobrar (tarjeta):";
                    panelCambio.Visible = false;
                    lblInfoMetodo.Text = "💳 Procesar cobro en terminal de punto de venta. No hay cambio.";
                    lblInfoMetodo.ForeColor = Color.FromArgb(96, 165, 250);
                    break;

                case "Transferencia":
                    txtMontoPagado.Enabled = false;
                    txtMontoPagado.Text = _total.ToString("N2");
                    lblMontoPagadoLabel.Text = "Monto a transferir:";
                    panelCambio.Visible = false;
                    lblInfoMetodo.Text = "🏦 Pago por transferencia bancaria. Verificar comprobante antes de confirmar.";
                    lblInfoMetodo.ForeColor = Color.FromArgb(167, 139, 250);
                    break;

                case "Credito":
                    txtMontoPagado.Enabled = false;
                    txtMontoPagado.Text = _total.ToString("N2");
                    lblMontoPagadoLabel.Text = "Monto a crédito:";
                    panelCambio.Visible = false;
                    lblInfoMetodo.Text = "📋 Venta a crédito. Se registrará una Cuenta por Cobrar al cliente.";
                    lblInfoMetodo.ForeColor = Color.FromArgb(251, 146, 60);
                    break;
            }

            ActualizarCambio();
        }

        private void txtMontoPagado_TextChanged(object sender, EventArgs e)
        {
            ActualizarCambio();
        }

        private void ActualizarCambio()
        {
            if (_metodoPagoActual != "Efectivo") { lblCambioVal.Text = "RD$ 0.00"; return; }

            if (decimal.TryParse(txtMontoPagado.Text, out decimal monto))
            {
                var cambio = FacturaCalculator.CalcularCambio(_total, monto);
                lblCambioVal.Text = MonedaHelper.Formatear(cambio);
                lblCambioVal.ForeColor = cambio >= 0 ? Color.FromArgb(74, 222, 128) : Color.FromArgb(239, 68, 68);

                if (monto < _total)
                {
                    lblCambioVal.Text = "⚠ Monto insuficiente";
                    lblCambioVal.ForeColor = Color.FromArgb(239, 68, 68);
                    btnConfirmar.Enabled = false;
                }
                else
                {
                    btnConfirmar.Enabled = true;
                }
            }
        }

        private void btnEfectivo_Click(object s, EventArgs e) => SeleccionarMetodoPago("Efectivo");
        private void btnTarjeta_Click(object s, EventArgs e) => SeleccionarMetodoPago("Tarjeta");
        private void btnTransferencia_Click(object s, EventArgs e) => SeleccionarMetodoPago("Transferencia");
        private void btnCredito_Click(object s, EventArgs e) => SeleccionarMetodoPago("Credito");

        private async void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtMontoPagado.Text, out decimal montoPagado))
            {
                MessageBox.Show("Ingresa un monto válido.", "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_metodoPagoActual == "Efectivo" && montoPagado < _total)
            {
                MessageBox.Show($"El monto recibido ({MonedaHelper.Formatear(montoPagado)}) es menor al total ({MonedaHelper.Formatear(_total)}).",
                    "Monto insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnConfirmar.Enabled = false;
            btnConfirmar.Text = "Procesando...";

            var request = new CobroRequest
            {
                TurnoId = SessionManager.TurnoId,
                ClienteId = _cliente.Id,
                Items = _items,
                MetodoPago = _metodoPagoActual,
                MontoPagado = montoPagado
            };

            try
            {
                var resp = await _integracion.ProcesarCobroAsync(request);
                if (resp == null)
                    throw new InvalidOperationException("Sin respuesta del servidor.");
                {
                    _ultimaRespuesta = resp;
                    _ultimoRequest = request;

                    // Guardar venta local
                    _local.GuardarVentaLocal(new VentaLocal
                    {
                        IdLocal = string.IsNullOrEmpty(resp.IdLocal) ? Guid.NewGuid().ToString() : resp.IdLocal,
                        FacturaIdCore = resp.FacturaId,
                        NumeroFactura = resp.NumeroFactura,
                        TurnoId = _localTurnoId,
                        ClienteId = _cliente.Id,
                        ClienteNombre = _cliente.Nombre,
                        Subtotal = _subtotal,
                        ITBIS = _itbis,
                        Total = _total,
                        MetodoPago = _metodoPagoActual,
                        MontoPagado = montoPagado,
                        Cambio = resp.Cambio,
                        Estado = resp.Estado,
                        FechaVenta = DateTime.Now
                    });

                    MostrarRecibo(resp, request);
                    DialogResult = DialogResult.OK;
                }
            }
            catch
            {
                // Modo offline: guardar en cola y generar recibo local
                var idLocal = Guid.NewGuid().ToString();
                var cambio = _metodoPagoActual == "Efectivo" ? FacturaCalculator.CalcularCambio(_total, montoPagado) : 0m;

                _queue.Encolar("cobro", request);

                var respLocal = new CobroResponse
                {
                    FacturaId = null,
                    NumeroFactura = null,
                    Total = _total,
                    Cambio = cambio,
                    Estado = "PendienteSync",
                    IdLocal = idLocal
                };

                _local.GuardarVentaLocal(new VentaLocal
                {
                    IdLocal = idLocal,
                    TurnoId = _localTurnoId,
                    ClienteId = _cliente.Id,
                    ClienteNombre = _cliente.Nombre,
                    Subtotal = _subtotal,
                    ITBIS = _itbis,
                    Total = _total,
                    MetodoPago = _metodoPagoActual,
                    MontoPagado = montoPagado,
                    Cambio = cambio,
                    Estado = "PendienteSync",
                    FechaVenta = DateTime.Now
                });

                _ultimaRespuesta = respLocal;
                _ultimoRequest = request;
                MostrarRecibo(respLocal, request);
                DialogResult = DialogResult.OK;
            }
        }

        private void MostrarRecibo(CobroResponse resp, CobroRequest request)
        {
            var svc = new ReciboService();
            var texto = svc.GenerarTextoRecibo(resp, request, _cliente.Nombre);

            using var frmRec = new frmRecibo(texto, resp);
            frmRec.ShowDialog();
        }

        private void btnCancelar_Click(object s, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void lblMetodoPagoTitle_Click(object sender, EventArgs e)
        {

        }
    }
}
