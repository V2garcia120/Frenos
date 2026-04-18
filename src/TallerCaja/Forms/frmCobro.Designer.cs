namespace TallerCaja.Forms
{
    partial class frmCobro
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // ── Layout principal ──────────────────────────────────────────────
            panelTop = new Panel();
            panelLeft = new Panel();
            panelRight = new Panel();
            panelTotales = new Panel();
            panelBotones = new Panel();
            panelBuscar = new Panel();
            panelAgregar = new Panel();
            panelCarritoBtns = new Panel();
            splitMain = new SplitContainer();

            // ── Panel superior ────────────────────────────────────────────────
            lblAppTitle = new Label();
            lblCajero = new Label();
            lblTurno = new Label();
            lblFecha = new Label();
            lblPendientes = new Label();
            btnBuscarFactura = new Button();
            btnCierreDia = new Button();

            // ── Panel izquierdo (catálogo) ────────────────────────────────────
            lblCatalogoTitle = new Label();
            txtBuscar = new TextBox();
            lvCatalogo = new ListView();
            colNombre = new ColumnHeader();
            colPrecio = new ColumnHeader();
            colStock = new ColumnHeader();
            colCategoria = new ColumnHeader();
            btnAgregar = new Button();

            // ── Cliente ───────────────────────────────────────────────────────
            panelCliente = new Panel();
            lblClienteTitle = new Label();
            txtBuscarCliente = new TextBox();
            btnBuscarCliente = new Button();
            btnClienteAnonimo = new Button();
            lblClienteActual = new Label();

            // ── Panel derecho (carrito) ───────────────────────────────────────
            lblCarritoTitle = new Label();
            lblItemsCount = new Label();
            lvCarrito = new ListView();
            colCNombre = new ColumnHeader();
            colCTipo = new ColumnHeader();
            colCCant = new ColumnHeader();
            colCPrecio = new ColumnHeader();
            colCSubtotal = new ColumnHeader();
            btnQuitarItem = new Button();
            btnLimpiar = new Button();

            // ── Totales ───────────────────────────────────────────────────────
            lblSubtotalLabel = new Label();
            lblSubtotal = new Label();
            lblITBISLabel = new Label();
            lblITBIS = new Label();
            lblTotalLabel = new Label();
            lblTotal = new Label();
            btnCobrar = new Button();

            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            SuspendLayout();

            // ── panelTop ──────────────────────────────────────────────────────
            panelTop.Dock = DockStyle.Top;
            panelTop.Height = 56;
            panelTop.BackColor = Color.FromArgb(15, 23, 42);
            panelTop.Padding = new Padding(10, 0, 10, 0);

            lblAppTitle.AutoSize = false;
            lblAppTitle.Size = new Size(260, 56);
            lblAppTitle.Location = new Point(10, 0);
            lblAppTitle.Text = "TALLER DE FRENOS — CAJA";
            lblAppTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblAppTitle.ForeColor = Color.White;
            lblAppTitle.TextAlign = ContentAlignment.MiddleLeft;

            lblCajero.AutoSize = true;
            lblCajero.Location = new Point(280, 18);
            lblCajero.Font = new Font("Segoe UI", 9F);
            lblCajero.ForeColor = Color.FromArgb(148, 163, 184);

            lblTurno.AutoSize = true;
            lblTurno.Location = new Point(430, 18);
            lblTurno.Font = new Font("Segoe UI", 9F);
            lblTurno.ForeColor = Color.FromArgb(148, 163, 184);

            lblFecha.AutoSize = true;
            lblFecha.Location = new Point(550, 18);
            lblFecha.Font = new Font("Segoe UI", 9F);
            lblFecha.ForeColor = Color.FromArgb(148, 163, 184);

            lblPendientes.AutoSize = true;
            lblPendientes.Location = new Point(700, 18);
            lblPendientes.Font = new Font("Segoe UI", 9F);
            lblPendientes.ForeColor = Color.FromArgb(251, 146, 60);

            btnBuscarFactura.Location = new Point(960, 12);
            btnBuscarFactura.Size = new Size(145, 32);
            btnBuscarFactura.Text = "🔍 Buscar Factura";
            btnBuscarFactura.Font = new Font("Segoe UI", 9F);
            btnBuscarFactura.BackColor = Color.FromArgb(59, 130, 246);
            btnBuscarFactura.ForeColor = Color.White;
            btnBuscarFactura.FlatStyle = FlatStyle.Flat;
            btnBuscarFactura.FlatAppearance.BorderSize = 0;
            btnBuscarFactura.Cursor = Cursors.Hand;
            btnBuscarFactura.Click += btnBuscarFactura_Click;

            btnCierreDia.Location = new Point(1115, 12);
            btnCierreDia.Size = new Size(130, 32);
            btnCierreDia.Text = "⏹ Cerrar Turno";
            btnCierreDia.Font = new Font("Segoe UI", 9F);
            btnCierreDia.BackColor = Color.FromArgb(239, 68, 68);
            btnCierreDia.ForeColor = Color.White;
            btnCierreDia.FlatStyle = FlatStyle.Flat;
            btnCierreDia.FlatAppearance.BorderSize = 0;
            btnCierreDia.Cursor = Cursors.Hand;
            btnCierreDia.Click += btnCierreDia_Click;

            panelTop.Controls.AddRange(new Control[] {
                lblAppTitle, lblCajero, lblTurno, lblFecha, lblPendientes, btnBuscarFactura, btnCierreDia
            });

            // ── SplitContainer ────────────────────────────────────────────────
            splitMain.Dock = DockStyle.Fill;
            splitMain.SplitterDistance = 620;
            splitMain.Panel1MinSize = 400;
            splitMain.Panel2MinSize = 380;

            // ── Panel izquierdo ───────────────────────────────────────────────
            lblCatalogoTitle.AutoSize = false;
            lblCatalogoTitle.Size = new Size(580, 36);
            lblCatalogoTitle.Dock = DockStyle.Top;
            lblCatalogoTitle.Text = "  📦 CATÁLOGO DE PRODUCTOS Y SERVICIOS";
            lblCatalogoTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblCatalogoTitle.ForeColor = Color.White;
            lblCatalogoTitle.BackColor = Color.FromArgb(30, 41, 59);
            lblCatalogoTitle.TextAlign = ContentAlignment.MiddleLeft;

            panelBuscar = new Panel { Dock = DockStyle.Top, Height = 44, BackColor = Color.WhiteSmoke, Padding = new Padding(8, 6, 8, 6) };
            txtBuscar.Dock = DockStyle.Fill;
            txtBuscar.Font = new Font("Segoe UI", 11F);
            txtBuscar.PlaceholderText = "🔍 Buscar producto o servicio...";
            txtBuscar.BorderStyle = BorderStyle.FixedSingle;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            panelBuscar.Controls.Add(txtBuscar);

            lvCatalogo.Dock = DockStyle.Fill;
            lvCatalogo.View = View.Details;
            lvCatalogo.FullRowSelect = true;
            lvCatalogo.GridLines = true;
            lvCatalogo.Font = new Font("Segoe UI", 10F);
            lvCatalogo.Columns.AddRange(new ColumnHeader[] { colNombre, colPrecio, colStock, colCategoria });
            lvCatalogo.DoubleClick += lvCatalogo_DoubleClick;
            lvCatalogo.BorderStyle = BorderStyle.None;
            colNombre.Text = "Nombre"; colNombre.Width = 250;
            colPrecio.Text = "Precio"; colPrecio.Width = 120;
            colStock.Text = "Disponible"; colStock.Width = 90;
            colCategoria.Text = "Categoría"; colCategoria.Width = 120;

            panelAgregar = new Panel { Dock = DockStyle.Bottom, Height = 44, BackColor = Color.WhiteSmoke, Padding = new Padding(8, 6, 8, 6) };
            btnAgregar.Dock = DockStyle.Fill;
            btnAgregar.Text = "➕ Agregar al carrito (o doble clic)";
            btnAgregar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAgregar.BackColor = Color.FromArgb(37, 99, 235);
            btnAgregar.ForeColor = Color.White;
            btnAgregar.FlatStyle = FlatStyle.Flat;
            btnAgregar.FlatAppearance.BorderSize = 0;
            btnAgregar.Cursor = Cursors.Hand;
            btnAgregar.Click += btnAgregar_Click;
            panelAgregar.Controls.Add(btnAgregar);

            // Panel cliente
            panelCliente.Dock = DockStyle.Bottom;
            panelCliente.Height = 60;
            panelCliente.BackColor = Color.FromArgb(241, 245, 249);
            panelCliente.Padding = new Padding(8, 6, 8, 6);
            lblClienteTitle.AutoSize = true;
            lblClienteTitle.Location = new Point(8, 6);
            lblClienteTitle.Text = "Cliente:";
            lblClienteTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblClienteTitle.ForeColor = Color.FromArgb(71, 85, 105);
            lblClienteActual.AutoSize = true;
            lblClienteActual.Location = new Point(65, 6);
            lblClienteActual.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblClienteActual.Text = "Cliente Anónimo";
            lblClienteActual.ForeColor = Color.FromArgb(100, 116, 139);
            txtBuscarCliente.Location = new Point(8, 28);
            txtBuscarCliente.Size = new Size(240, 24);
            txtBuscarCliente.Font = new Font("Segoe UI", 9F);
            txtBuscarCliente.PlaceholderText = "Nombre o cédula...";
            txtBuscarCliente.BorderStyle = BorderStyle.FixedSingle;
            btnBuscarCliente.Location = new Point(255, 26);
            btnBuscarCliente.Size = new Size(100, 26);
            btnBuscarCliente.Text = "Buscar";
            btnBuscarCliente.Font = new Font("Segoe UI", 9F);
            btnBuscarCliente.BackColor = Color.FromArgb(59, 130, 246);
            btnBuscarCliente.ForeColor = Color.White;
            btnBuscarCliente.FlatStyle = FlatStyle.Flat;
            btnBuscarCliente.FlatAppearance.BorderSize = 0;
            btnBuscarCliente.Cursor = Cursors.Hand;
            btnBuscarCliente.Click += btnBuscarCliente_Click;
            btnClienteAnonimo.Location = new Point(362, 26);
            btnClienteAnonimo.Size = new Size(120, 26);
            btnClienteAnonimo.Text = "Anónimo";
            btnClienteAnonimo.Font = new Font("Segoe UI", 9F);
            btnClienteAnonimo.BackColor = Color.FromArgb(100, 116, 139);
            btnClienteAnonimo.ForeColor = Color.White;
            btnClienteAnonimo.FlatStyle = FlatStyle.Flat;
            btnClienteAnonimo.FlatAppearance.BorderSize = 0;
            btnClienteAnonimo.Cursor = Cursors.Hand;
            btnClienteAnonimo.Click += btnClienteAnonimo_Click;
            panelCliente.Controls.AddRange(new Control[] { lblClienteTitle, lblClienteActual, txtBuscarCliente, btnBuscarCliente, btnClienteAnonimo });

            splitMain.Panel1.Controls.Add(lvCatalogo);
            splitMain.Panel1.Controls.Add(panelAgregar);
            splitMain.Panel1.Controls.Add(panelBuscar);
            splitMain.Panel1.Controls.Add(panelCliente);
            splitMain.Panel1.Controls.Add(lblCatalogoTitle);

            // ── Panel derecho (carrito + totales) ─────────────────────────────
            lblCarritoTitle.AutoSize = false;
            lblCarritoTitle.Dock = DockStyle.Top;
            lblCarritoTitle.Height = 36;
            lblCarritoTitle.Text = "  🛒 DETALLE DE VENTA";
            lblCarritoTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblCarritoTitle.ForeColor = Color.White;
            lblCarritoTitle.BackColor = Color.FromArgb(30, 41, 59);
            lblCarritoTitle.TextAlign = ContentAlignment.MiddleLeft;

            lblItemsCount.AutoSize = false;
            lblItemsCount.Dock = DockStyle.Top;
            lblItemsCount.Height = 24;
            lblItemsCount.Font = new Font("Segoe UI", 9F);
            lblItemsCount.ForeColor = Color.FromArgb(100, 116, 139);
            lblItemsCount.BackColor = Color.WhiteSmoke;
            lblItemsCount.Text = "0 items";
            lblItemsCount.TextAlign = ContentAlignment.MiddleRight;
            lblItemsCount.Padding = new Padding(0, 0, 10, 0);

            lvCarrito.Dock = DockStyle.Fill;
            lvCarrito.View = View.Details;
            lvCarrito.FullRowSelect = true;
            lvCarrito.GridLines = true;
            lvCarrito.Font = new Font("Segoe UI", 10F);
            lvCarrito.Columns.AddRange(new ColumnHeader[] { colCNombre, colCTipo, colCCant, colCPrecio, colCSubtotal });
            lvCarrito.KeyDown += lvCarrito_KeyDown;
            lvCarrito.DoubleClick += lvCarrito_DoubleClick;
            lvCarrito.BorderStyle = BorderStyle.None;
            colCNombre.Text = "Descripción"; colCNombre.Width = 200;
            colCTipo.Text = "Tipo"; colCTipo.Width = 70;
            colCCant.Text = "Cant."; colCCant.Width = 55;
            colCPrecio.Text = "P. Unit."; colCPrecio.Width = 100;
            colCSubtotal.Text = "Subtotal"; colCSubtotal.Width = 105;

            // Botones de carrito
            panelCarritoBtns = new Panel { Dock = DockStyle.Bottom, Height = 38, BackColor = Color.WhiteSmoke, Padding = new Padding(8, 5, 8, 5) };
            btnQuitarItem.Location = new Point(8, 5);
            btnQuitarItem.Size = new Size(160, 28);
            btnQuitarItem.Text = "🗑 Quitar seleccionado";
            btnQuitarItem.Font = new Font("Segoe UI", 9F);
            btnQuitarItem.BackColor = Color.FromArgb(239, 68, 68);
            btnQuitarItem.ForeColor = Color.White;
            btnQuitarItem.FlatStyle = FlatStyle.Flat;
            btnQuitarItem.FlatAppearance.BorderSize = 0;
            btnQuitarItem.Cursor = Cursors.Hand;
            btnQuitarItem.Click += btnQuitarItem_Click;
            btnLimpiar.Location = new Point(180, 5);
            btnLimpiar.Size = new Size(90, 28);
            btnLimpiar.Text = "🧹 Limpiar";
            btnLimpiar.Font = new Font("Segoe UI", 9F);
            btnLimpiar.BackColor = Color.FromArgb(100, 116, 139);
            btnLimpiar.ForeColor = Color.White;
            btnLimpiar.FlatStyle = FlatStyle.Flat;
            btnLimpiar.FlatAppearance.BorderSize = 0;
            btnLimpiar.Cursor = Cursors.Hand;
            btnLimpiar.Click += btnLimpiar_Click;
            panelCarritoBtns.Controls.AddRange(new Control[] { btnQuitarItem, btnLimpiar });

            // Totales
            panelTotales = new Panel { Dock = DockStyle.Bottom, Height = 170, BackColor = Color.FromArgb(15, 23, 42), Padding = new Padding(15, 10, 15, 10) };

            lblSubtotalLabel.AutoSize = false; lblSubtotalLabel.Location = new Point(15, 12);
            lblSubtotalLabel.Size = new Size(140, 26); lblSubtotalLabel.Text = "Subtotal:";
            lblSubtotalLabel.Font = new Font("Segoe UI", 11F); lblSubtotalLabel.ForeColor = Color.FromArgb(148, 163, 184);
            lblSubtotal.AutoSize = false; lblSubtotal.Location = new Point(200, 12);
            lblSubtotal.Size = new Size(160, 26); lblSubtotal.Text = "RD$ 0.00";
            lblSubtotal.Font = new Font("Segoe UI", 11F); lblSubtotal.ForeColor = Color.White;
            lblSubtotal.TextAlign = ContentAlignment.TopRight;

            lblITBISLabel.AutoSize = false; lblITBISLabel.Location = new Point(15, 44);
            lblITBISLabel.Size = new Size(180, 26); lblITBISLabel.Text = "ITBIS (18%):";
            lblITBISLabel.Font = new Font("Segoe UI", 11F); lblITBISLabel.ForeColor = Color.FromArgb(148, 163, 184);
            lblITBIS.AutoSize = false; lblITBIS.Location = new Point(200, 44);
            lblITBIS.Size = new Size(160, 26); lblITBIS.Text = "RD$ 0.00";
            lblITBIS.Font = new Font("Segoe UI", 11F); lblITBIS.ForeColor = Color.FromArgb(251, 146, 60);
            lblITBIS.TextAlign = ContentAlignment.TopRight;

            lblTotalLabel.AutoSize = false; lblTotalLabel.Location = new Point(15, 78);
            lblTotalLabel.Size = new Size(140, 34); lblTotalLabel.Text = "TOTAL:";
            lblTotalLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold); lblTotalLabel.ForeColor = Color.White;
            lblTotal.AutoSize = false; lblTotal.Location = new Point(140, 78);
            lblTotal.Size = new Size(220, 34); lblTotal.Text = "RD$ 0.00";
            lblTotal.Font = new Font("Segoe UI", 18F, FontStyle.Bold); lblTotal.ForeColor = Color.FromArgb(74, 222, 128);
            lblTotal.TextAlign = ContentAlignment.TopRight;

            btnCobrar.Location = new Point(15, 120);
            btnCobrar.Size = new Size(345, 40);
            btnCobrar.Text = "💳  COBRAR";
            btnCobrar.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnCobrar.BackColor = Color.FromArgb(22, 163, 74);
            btnCobrar.ForeColor = Color.White;
            btnCobrar.FlatStyle = FlatStyle.Flat;
            btnCobrar.FlatAppearance.BorderSize = 0;
            btnCobrar.Cursor = Cursors.Hand;
            btnCobrar.Enabled = false;
            btnCobrar.Click += btnCobrar_Click;

            panelTotales.Controls.AddRange(new Control[] {
                lblSubtotalLabel, lblSubtotal, lblITBISLabel, lblITBIS,
                lblTotalLabel, lblTotal, btnCobrar
            });

            splitMain.Panel2.Controls.Add(lvCarrito);
            splitMain.Panel2.Controls.Add(panelCarritoBtns);
            splitMain.Panel2.Controls.Add(panelTotales);
            splitMain.Panel2.Controls.Add(lblItemsCount);
            splitMain.Panel2.Controls.Add(lblCarritoTitle);

            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1260, 720);
            Controls.Add(splitMain);
            Controls.Add(panelTop);
            FormBorderStyle = FormBorderStyle.Sizable;
            MinimumSize = new Size(1000, 600);
            Name = "frmCobro";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Taller de Frenos — Pantalla de Cobro";
            WindowState = FormWindowState.Maximized;
            Load += frmCobro_Load;

            ResumeLayout(false);
        }

        // Controls
        private Panel panelTop, panelLeft, panelRight, panelTotales, panelBotones, panelCliente, panelBuscar, panelAgregar, panelCarritoBtns;
        private SplitContainer splitMain;
        private Label lblAppTitle, lblCajero, lblTurno, lblFecha, lblPendientes;
        private Button btnBuscarFactura, btnCierreDia;
        private Label lblCatalogoTitle;
        private TextBox txtBuscar;
        private ListView lvCatalogo;
        private ColumnHeader colNombre, colPrecio, colStock, colCategoria;
        private Button btnAgregar;
        private Label lblClienteTitle, lblClienteActual;
        private TextBox txtBuscarCliente;
        private Button btnBuscarCliente, btnClienteAnonimo;
        private Label lblCarritoTitle, lblItemsCount;
        private ListView lvCarrito;
        private ColumnHeader colCNombre, colCTipo, colCCant, colCPrecio, colCSubtotal;
        private Button btnQuitarItem, btnLimpiar;
        private Label lblSubtotalLabel, lblSubtotal, lblITBISLabel, lblITBIS, lblTotalLabel, lblTotal;
        private Button btnCobrar;
    }
}
