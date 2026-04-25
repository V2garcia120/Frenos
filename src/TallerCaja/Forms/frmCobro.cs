using TallerCaja.Helpers;
using TallerCaja.Models.DTOs;
using TallerCaja.Services;

namespace TallerCaja.Forms
{
    public partial class frmCobro : Form
    {
        private readonly IIntegracionService _integracion;
        private readonly ICajaLocalService _local;
        private readonly OfflineQueue _queue;
        private readonly ISyncService _syncService;

        private List<ItemCobroDto> _carrito = new();
        private ClienteDto? _clienteSeleccionado;
        private List<ProductoDto> _productosCache = new();
        private List<ServicioDto> _serviciosCache = new();
        private int _localTurnoId;

        private void MostrarCatalogo(bool mostrar)
        {
            splitMain.Panel1Collapsed = !mostrar;
            btnCatalogo.Text = mostrar ? "Ocultar catálogo" : "Mostrar catálogo";
        }

        private void ConfigurarEstiloCatalogo()
        {
            lvCatalogo.BackColor = Color.White;
            lvCatalogo.ForeColor = Color.FromArgb(15, 23, 42);
            lvCatalogo.GridLines = false;
            lvCatalogo.ShowGroups = true;
            lvCatalogo.HotTracking = true;
            lvCatalogo.HoverSelection = true;
        }

        private static Color ObtenerColorFila(int indiceFila)
            => indiceFila % 2 == 0
                ? Color.FromArgb(248, 250, 252)
                : Color.White;

        public frmCobro()
        {
            _integracion = null!;
            _local = null!;
            _queue = null!;
            _syncService = null!;
            _localTurnoId = 0;
            InitializeComponent();
        }

        public frmCobro(IIntegracionService integracion, ICajaLocalService local, OfflineQueue queue, ISyncService syncService, int turnoLocalId)
        {
            _integracion = integracion;
            _local = local;
            _queue = queue;
            _syncService = syncService;
            _localTurnoId = turnoLocalId;
            InitializeComponent();
        }

        private async void frmCobro_Load(object sender, EventArgs e)
        {
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
            {
                lblCajero.Text = "Cajero: demo";
                lblTurno.Text = "Turno: #0";
                lblFecha.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                lblPendientes.Text = "Pendientes offline: 0";
                MostrarCatalogo(false);
                return;
            }

            MostrarCatalogo(false);

            lblCajero.Text = $"Cajero: {SessionManager.CajeroNombre}";
            lblTurno.Text = $"Turno: #{SessionManager.TurnoId}";
            lblFecha.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            lblPendientes.Text = $"Pendientes offline: {_queue.ContarPendientes()}";
            panelBusquedaCliente.Visible = false;
            ConfigurarEstiloCatalogo();

            // Cliente anónimo por defecto
            var anonimo = await _integracion.ObtenerClienteAnonimoAsync();
            _clienteSeleccionado = anonimo;
            ActualizarLabelCliente();

            await CargarCatalogoAsync();
            await SincronizarPendientesYNotificarAsync();
            ActualizarTotales();
        }

        private async Task SincronizarPendientesYNotificarAsync()
        {
            try
            {
                var resultado = await _syncService.SincronizarPendientesAsync();
                lblPendientes.Text = $"Pendientes offline: {_queue.ContarPendientes()}";

                if (resultado.Procesadas > 0)
                {
                    var mensaje = resultado.Procesadas == 1
                        ? "Se sincronizó 1 transacción pendiente."
                        : $"Se sincronizaron {resultado.Procesadas} transacciones pendientes.";

                    if (resultado.Fallidas > 0)
                        mensaje += $" Quedaron {resultado.Fallidas} fallidas por sincronizar.";

                    MessageBox.Show(mensaje, "Sincronización completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                // La caja sigue funcionando aunque no se pueda sincronizar en este momento.
            }
        }

        private async Task CargarCatalogoAsync()
        {
            try
            {
                _productosCache = await _integracion.ObtenerProductosAsync();
                _serviciosCache = await _integracion.ObtenerServiciosAsync();
                _local.SincronizarProductos(_productosCache);
                _local.SincronizarServicios(_serviciosCache);
            }
            catch
            {
                // Usar caché local si falla Integración
                _productosCache = _local.ObtenerProductosLocales()
                    .Select(p => new ProductoDto { Id = p.Id, Nombre = p.Nombre, Precio = p.Precio, Stock = p.Stock, Categoria = p.Categoria })
                    .ToList();
                _serviciosCache = _local.ObtenerServiciosLocales()
                    .Select(s => new ServicioDto { Id = s.Id, Nombre = s.Nombre, Precio = s.Precio, DuracionMin = s.DuracionMin, Categoria = s.Categoria })
                    .ToList();
            }
            MostrarResultadosBusqueda(_productosCache, _serviciosCache);
        }

        private void MostrarResultadosBusqueda(List<ProductoDto> productos, List<ServicioDto> servicios)
        {
            lvCatalogo.Items.Clear();
            lvCatalogo.Groups.Clear();

            var productosOrdenados = productos
                .OrderBy(p => p.Categoria)
                .ThenBy(p => p.Nombre)
                .ToList();

            var serviciosOrdenados = servicios
                .OrderBy(s => s.Categoria)
                .ThenBy(s => s.Nombre)
                .ToList();

            var indiceFila = 0;

            if (productosOrdenados.Any())
            {
                var gprod = new ListViewGroup($"Productos ({productosOrdenados.Count})", HorizontalAlignment.Left);
                lvCatalogo.Groups.Add(gprod);
                foreach (var p in productosOrdenados)
                {
                    var item = new ListViewItem(new[] { p.Nombre, MonedaHelper.Formatear(p.Precio), $"{p.Stock} uds", p.Categoria });
                    item.Group = gprod;
                    item.BackColor = ObtenerColorFila(indiceFila++);
                    if (p.Stock <= 5)
                        item.ForeColor = Color.FromArgb(185, 28, 28);
                    item.Tag = new ItemCobroDto { Tipo = "Producto", ItemId = p.Id, PrecioSnapshot = p.Precio, NombreSnapshot = p.Nombre, Cantidad = 1 };
                    lvCatalogo.Items.Add(item);
                }
            }

            if (serviciosOrdenados.Any())
            {
                var gserv = new ListViewGroup($"Servicios ({serviciosOrdenados.Count})", HorizontalAlignment.Left);
                lvCatalogo.Groups.Add(gserv);
                foreach (var s in serviciosOrdenados)
                {
                    var item = new ListViewItem(new[] { s.Nombre, MonedaHelper.Formatear(s.Precio), $"{s.DuracionMin} min", s.Categoria });
                    item.Group = gserv;
                    item.BackColor = ObtenerColorFila(indiceFila++);
                    item.Tag = new ItemCobroDto { Tipo = "Servicio", ItemId = s.Id, PrecioSnapshot = s.Precio, NombreSnapshot = s.Nombre, Cantidad = 1 };
                    lvCatalogo.Items.Add(item);
                }
            }

            lblCatalogoTitle.Text = $"  CATÁLOGO DE PRODUCTOS Y SERVICIOS ({productosOrdenados.Count + serviciosOrdenados.Count} ítems)";
        }

        private async void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            var q = txtBuscar.Text.Trim();
            if (q.Length == 0)
            {
                MostrarResultadosBusqueda(_productosCache, _serviciosCache);
                return;
            }
            if (q.Length < 2) return;

            try
            {
                var resultado = await _integracion.BuscarCatalogoAsync(q);
                if (resultado != null)
                    MostrarResultadosBusqueda(resultado.Productos, resultado.Servicios);
            }
            catch
            {
                var prodFiltrados = _productosCache.Where(p => p.Nombre.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
                var servFiltrados = _serviciosCache.Where(s => s.Nombre.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
                MostrarResultadosBusqueda(prodFiltrados, servFiltrados);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (lvCatalogo.SelectedItems.Count == 0) return;
            var item = lvCatalogo.SelectedItems[0].Tag as ItemCobroDto;
            if (item == null) return;

            var existente = _carrito.FirstOrDefault(c => c.ItemId == item.ItemId && c.Tipo == item.Tipo);
            if (existente != null)
                existente.Cantidad++;
            else
                _carrito.Add(new ItemCobroDto
                {
                    Tipo = item.Tipo, ItemId = item.ItemId,
                    PrecioSnapshot = item.PrecioSnapshot,
                    NombreSnapshot = item.NombreSnapshot,
                    Cantidad = 1
                });

            ActualizarCarritoUI();
            ActualizarTotales();
            MostrarCatalogo(false);
        }

        private void btnCatalogo_Click(object sender, EventArgs e)
        {
            MostrarCatalogo(splitMain.Panel1Collapsed);
        }

        private void lvCatalogo_DoubleClick(object sender, EventArgs e) => btnAgregar_Click(sender, e);

        private void btnQuitarItem_Click(object sender, EventArgs e)
        {
            if (lvCarrito.SelectedItems.Count == 0) return;
            var idx = lvCarrito.SelectedIndices[0];
            _carrito.RemoveAt(idx);
            ActualizarCarritoUI();
            ActualizarTotales();
        }

        private void lvCarrito_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete) btnQuitarItem_Click(sender, e);
        }

        private void lvCarrito_DoubleClick(object sender, EventArgs e)
        {
            if (lvCarrito.SelectedItems.Count == 0) return;
            var idx = lvCarrito.SelectedIndices[0];
            var item = _carrito[idx];

            using var frm = new frmCantidad(item.NombreSnapshot, item.Cantidad);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (frm.NuevaCantidad <= 0)
                    _carrito.RemoveAt(idx);
                else
                    item.Cantidad = frm.NuevaCantidad;
                ActualizarCarritoUI();
                ActualizarTotales();
            }
        }

        private void ActualizarCarritoUI()
        {
            lvCarrito.Items.Clear();
            foreach (var item in _carrito)
            {
                var li = new ListViewItem(new[]
                {
                    item.NombreSnapshot,
                    item.Tipo,
                    item.Cantidad.ToString(),
                    MonedaHelper.Formatear(item.PrecioSnapshot),
                    MonedaHelper.Formatear(item.PrecioSnapshot * item.Cantidad)
                });
                li.Tag = item;
                lvCarrito.Items.Add(li);
            }
        }

        private void ActualizarTotales()
        {
            var (subtotal, itbis, total) = FacturaCalculator.Calcular(_carrito);
            lblSubtotal.Text = MonedaHelper.Formatear(subtotal);
            lblITBIS.Text = MonedaHelper.Formatear(itbis);
            lblTotal.Text = MonedaHelper.Formatear(total);
            lblItemsCount.Text = $"{_carrito.Sum(i => i.Cantidad)} items";
            btnCobrar.Enabled = _carrito.Any();
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            panelBusquedaCliente.Visible = !panelBusquedaCliente.Visible;
            if (panelBusquedaCliente.Visible)
            {
                txtBuscarCliente.Focus();
                txtBuscarCliente.SelectAll();
            }
        }

        private async void btnBuscarCedula_Click(object sender, EventArgs e)
        {
            var q = txtBuscarCliente.Text.Trim();
            if (string.IsNullOrEmpty(q)) return;

            try
            {
                var clientes = await _integracion.BuscarClientesAsync(q);
                if (!clientes.Any())
                {
                    MessageBox.Show("No se encontró ningún cliente con ese término.",
                        "Sin resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using var selector = new frmSelectorCliente(clientes);
                if (selector.ShowDialog() == DialogResult.OK && selector.ClienteSeleccionado != null)
                {
                    _clienteSeleccionado = selector.ClienteSeleccionado;
                    ActualizarLabelCliente();
                    panelBusquedaCliente.Visible = false;
                }
            }
            catch
            {
                MessageBox.Show("No se pudo conectar para buscar clientes.",
                    "Sin conexión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSeleccionarAnonimo_Click(object sender, EventArgs e)
        {
            _clienteSeleccionado = new ClienteDto { Id = 1, Nombre = "Cliente Anónimo", EsAnonimo = true };
            ActualizarLabelCliente();
            panelBusquedaCliente.Visible = false;
        }

        private void btnCerrarPanelCliente_Click(object sender, EventArgs e)
        {
            panelBusquedaCliente.Visible = false;
        }

        private void ActualizarLabelCliente()
        {
            lblClienteActual.Text = _clienteSeleccionado?.Nombre ?? "Cliente Anónimo";
            lblClienteActual.ForeColor = _clienteSeleccionado?.EsAnonimo == true
                ? Color.FromArgb(100, 116, 139)
                : Color.FromArgb(22, 163, 74);

            lblClienteFacturando.Text = $"Facturando a: {lblClienteActual.Text}";
            lblClienteFacturando.ForeColor = _clienteSeleccionado?.EsAnonimo == true
                ? Color.FromArgb(251, 146, 60)
                : Color.FromArgb(74, 222, 128);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            if (_carrito.Any() && MessageBox.Show("¿Limpiar todos los items del carrito?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _carrito.Clear();
                ActualizarCarritoUI();
                ActualizarTotales();
            }
        }

        private void btnCobrar_Click(object sender, EventArgs e)
        {
            if (!_carrito.Any()) return;
            if (_clienteSeleccionado == null) _clienteSeleccionado = new ClienteDto { Id = 1, Nombre = "Cliente Anónimo", EsAnonimo = true };

            var (_, _, total) = FacturaCalculator.Calcular(_carrito);
            using var frmPago = new frmPago(_integracion, _local, _queue, _carrito, _clienteSeleccionado, total, _localTurnoId);
            if (frmPago.ShowDialog() == DialogResult.OK)
            {
                _carrito.Clear();
                ActualizarCarritoUI();
                ActualizarTotales();
                txtBuscarCliente.Clear();
                panelBusquedaCliente.Visible = false;
                _clienteSeleccionado = new ClienteDto { Id = 1, Nombre = "Cliente Anónimo", EsAnonimo = true };
                ActualizarLabelCliente();
                lblPendientes.Text = $"Pendientes offline: {_queue.ContarPendientes()}";
            }
        }

        private void btnCierreDia_Click(object sender, EventArgs e)
        {
            if (_carrito.Any())
            {
                MessageBox.Show("Tienes items en el carrito. Completa o limpia la venta antes de cerrar el turno.",
                    "Venta en proceso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var frm = new frmCierreDia(_integracion, _local, _localTurnoId);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                SessionManager.CerrarTurno();
                Application.Restart();
            }
        }

        private void btnBuscarFactura_Click(object sender, EventArgs e)
        {
            using var frm = new frmBuscarFactura(_integracion, _queue, _localTurnoId);
            frm.ShowDialog();
        }
    }
}
