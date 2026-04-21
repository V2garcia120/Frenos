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
            panelTop = new Panel();
            lblAppTitle = new Label();
            lblCajero = new Label();
            lblTurno = new Label();
            lblFecha = new Label();
            lblPendientes = new Label();
            btnCatalogo = new Button();
            btnBuscarFactura = new Button();
            btnCierreDia = new Button();
            panelTotales = new Panel();
            lblSubtotalLabel = new Label();
            lblSubtotal = new Label();
            lblITBISLabel = new Label();
            lblITBIS = new Label();
            lblTotalLabel = new Label();
            lblTotal = new Label();
            btnCobrar = new Button();
            panelBuscar = new Panel();
            txtBuscar = new TextBox();
            panelAgregar = new Panel();
            btnAgregar = new Button();
            panelCarritoBtns = new Panel();
            btnQuitarItem = new Button();
            btnLimpiar = new Button();
            splitMain = new SplitContainer();
            lvCatalogo = new ListView();
            colNombre = new ColumnHeader();
            colPrecio = new ColumnHeader();
            colStock = new ColumnHeader();
            colCategoria = new ColumnHeader();
            panelCliente = new Panel();
            lblClienteTitle = new Label();
            lblClienteActual = new Label();
            txtBuscarCliente = new TextBox();
            btnBuscarCliente = new Button();
            btnClienteAnonimo = new Button();
            lblCatalogoTitle = new Label();
            lvCarrito = new ListView();
            colCNombre = new ColumnHeader();
            colCTipo = new ColumnHeader();
            colCCant = new ColumnHeader();
            colCPrecio = new ColumnHeader();
            colCSubtotal = new ColumnHeader();
            lblItemsCount = new Label();
            lblCarritoTitle = new Label();
            panelTop.SuspendLayout();
            panelTotales.SuspendLayout();
            panelBuscar.SuspendLayout();
            panelAgregar.SuspendLayout();
            panelCarritoBtns.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            panelCliente.SuspendLayout();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.FromArgb(15, 23, 42);
            panelTop.Controls.Add(lblAppTitle);
            panelTop.Controls.Add(lblCajero);
            panelTop.Controls.Add(lblTurno);
            panelTop.Controls.Add(lblFecha);
            panelTop.Controls.Add(lblPendientes);
            panelTop.Controls.Add(btnCatalogo);
            panelTop.Controls.Add(btnBuscarFactura);
            panelTop.Controls.Add(btnCierreDia);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Margin = new Padding(4, 5, 4, 5);
            panelTop.Name = "panelTop";
            panelTop.Padding = new Padding(14, 0, 14, 0);
            panelTop.Size = new Size(1800, 93);
            panelTop.TabIndex = 1;
            // 
            // lblAppTitle
            // 
            lblAppTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblAppTitle.ForeColor = Color.White;
            lblAppTitle.Location = new Point(14, 0);
            lblAppTitle.Margin = new Padding(4, 0, 4, 0);
            lblAppTitle.Name = "lblAppTitle";
            lblAppTitle.Size = new Size(371, 93);
            lblAppTitle.TabIndex = 0;
            lblAppTitle.Text = "TALLER DE FRENOS — CAJA";
            lblAppTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblCajero
            // 
            lblCajero.AutoSize = true;
            lblCajero.Font = new Font("Segoe UI", 9F);
            lblCajero.ForeColor = Color.FromArgb(148, 163, 184);
            lblCajero.Location = new Point(400, 30);
            lblCajero.Margin = new Padding(4, 0, 4, 0);
            lblCajero.Name = "lblCajero";
            lblCajero.Size = new Size(0, 25);
            lblCajero.TabIndex = 1;
            // 
            // lblTurno
            // 
            lblTurno.AutoSize = true;
            lblTurno.Font = new Font("Segoe UI", 9F);
            lblTurno.ForeColor = Color.FromArgb(148, 163, 184);
            lblTurno.Location = new Point(614, 30);
            lblTurno.Margin = new Padding(4, 0, 4, 0);
            lblTurno.Name = "lblTurno";
            lblTurno.Size = new Size(0, 25);
            lblTurno.TabIndex = 2;
            // 
            // lblFecha
            // 
            lblFecha.AutoSize = true;
            lblFecha.Font = new Font("Segoe UI", 9F);
            lblFecha.ForeColor = Color.FromArgb(148, 163, 184);
            lblFecha.Location = new Point(786, 30);
            lblFecha.Margin = new Padding(4, 0, 4, 0);
            lblFecha.Name = "lblFecha";
            lblFecha.Size = new Size(0, 25);
            lblFecha.TabIndex = 3;
            // 
            // lblPendientes
            // 
            lblPendientes.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblPendientes.AutoSize = true;
            lblPendientes.Font = new Font("Segoe UI", 9F);
            lblPendientes.ForeColor = Color.FromArgb(251, 146, 60);
            lblPendientes.Location = new Point(1086, 30);
            lblPendientes.Margin = new Padding(4, 0, 4, 0);
            lblPendientes.Name = "lblPendientes";
            lblPendientes.Size = new Size(0, 25);
            lblPendientes.TabIndex = 4;
            // 
            // btnCatalogo
            // 
            btnCatalogo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCatalogo.BackColor = Color.FromArgb(30, 41, 59);
            btnCatalogo.Cursor = Cursors.Hand;
            btnCatalogo.FlatAppearance.BorderSize = 0;
            btnCatalogo.FlatStyle = FlatStyle.Flat;
            btnCatalogo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCatalogo.ForeColor = Color.White;
            btnCatalogo.Location = new Point(1100, 20);
            btnCatalogo.Margin = new Padding(4, 5, 4, 5);
            btnCatalogo.Name = "btnCatalogo";
            btnCatalogo.Size = new Size(214, 53);
            btnCatalogo.TabIndex = 5;
            btnCatalogo.Text = "📦 Mostrar catálogo";
            btnCatalogo.UseVisualStyleBackColor = false;
            btnCatalogo.Click += btnCatalogo_Click;
            // 
            // btnBuscarFactura
            // 
            btnBuscarFactura.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBuscarFactura.BackColor = Color.FromArgb(59, 130, 246);
            btnBuscarFactura.Cursor = Cursors.Hand;
            btnBuscarFactura.FlatAppearance.BorderSize = 0;
            btnBuscarFactura.FlatStyle = FlatStyle.Flat;
            btnBuscarFactura.Font = new Font("Segoe UI", 9F);
            btnBuscarFactura.ForeColor = Color.White;
            btnBuscarFactura.Location = new Point(1329, 20);
            btnBuscarFactura.Margin = new Padding(4, 5, 4, 5);
            btnBuscarFactura.Name = "btnBuscarFactura";
            btnBuscarFactura.Size = new Size(207, 53);
            btnBuscarFactura.TabIndex = 6;
            btnBuscarFactura.Text = "🔍 Buscar Factura";
            btnBuscarFactura.UseVisualStyleBackColor = false;
            btnBuscarFactura.Click += btnBuscarFactura_Click;
            // 
            // btnCierreDia
            // 
            btnCierreDia.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCierreDia.BackColor = Color.FromArgb(239, 68, 68);
            btnCierreDia.Cursor = Cursors.Hand;
            btnCierreDia.FlatAppearance.BorderSize = 0;
            btnCierreDia.FlatStyle = FlatStyle.Flat;
            btnCierreDia.Font = new Font("Segoe UI", 9F);
            btnCierreDia.ForeColor = Color.White;
            btnCierreDia.Location = new Point(1550, 20);
            btnCierreDia.Margin = new Padding(4, 5, 4, 5);
            btnCierreDia.Name = "btnCierreDia";
            btnCierreDia.Size = new Size(186, 53);
            btnCierreDia.TabIndex = 7;
            btnCierreDia.Text = "⏹ Cerrar Turno";
            btnCierreDia.UseVisualStyleBackColor = false;
            btnCierreDia.Click += btnCierreDia_Click;
            // 
            // panelTotales
            // 
            panelTotales.BackColor = Color.FromArgb(15, 23, 42);
            panelTotales.Controls.Add(lblSubtotalLabel);
            panelTotales.Controls.Add(lblSubtotal);
            panelTotales.Controls.Add(lblITBISLabel);
            panelTotales.Controls.Add(lblITBIS);
            panelTotales.Controls.Add(lblTotalLabel);
            panelTotales.Controls.Add(lblTotal);
            panelTotales.Controls.Add(btnCobrar);
            panelTotales.Dock = DockStyle.Bottom;
            panelTotales.Location = new Point(0, 824);
            panelTotales.Margin = new Padding(4, 5, 4, 5);
            panelTotales.Name = "panelTotales";
            panelTotales.Padding = new Padding(21, 17, 21, 17);
            panelTotales.Size = new Size(906, 283);
            panelTotales.TabIndex = 2;
            // 
            // lblSubtotalLabel
            // 
            lblSubtotalLabel.Font = new Font("Segoe UI", 11F);
            lblSubtotalLabel.ForeColor = Color.FromArgb(148, 163, 184);
            lblSubtotalLabel.Location = new Point(21, 20);
            lblSubtotalLabel.Margin = new Padding(4, 0, 4, 0);
            lblSubtotalLabel.Name = "lblSubtotalLabel";
            lblSubtotalLabel.Size = new Size(200, 43);
            lblSubtotalLabel.TabIndex = 0;
            lblSubtotalLabel.Text = "Subtotal:";
            // 
            // lblSubtotal
            // 
            lblSubtotal.Font = new Font("Segoe UI", 11F);
            lblSubtotal.ForeColor = Color.White;
            lblSubtotal.Location = new Point(286, 20);
            lblSubtotal.Margin = new Padding(4, 0, 4, 0);
            lblSubtotal.Name = "lblSubtotal";
            lblSubtotal.Size = new Size(229, 43);
            lblSubtotal.TabIndex = 1;
            lblSubtotal.Text = "RD$ 0.00";
            lblSubtotal.TextAlign = ContentAlignment.TopRight;
            // 
            // lblITBISLabel
            // 
            lblITBISLabel.Font = new Font("Segoe UI", 11F);
            lblITBISLabel.ForeColor = Color.FromArgb(148, 163, 184);
            lblITBISLabel.Location = new Point(21, 73);
            lblITBISLabel.Margin = new Padding(4, 0, 4, 0);
            lblITBISLabel.Name = "lblITBISLabel";
            lblITBISLabel.Size = new Size(257, 43);
            lblITBISLabel.TabIndex = 2;
            lblITBISLabel.Text = "ITBIS (18%):";
            // 
            // lblITBIS
            // 
            lblITBIS.Font = new Font("Segoe UI", 11F);
            lblITBIS.ForeColor = Color.FromArgb(251, 146, 60);
            lblITBIS.Location = new Point(286, 73);
            lblITBIS.Margin = new Padding(4, 0, 4, 0);
            lblITBIS.Name = "lblITBIS";
            lblITBIS.Size = new Size(229, 43);
            lblITBIS.TabIndex = 3;
            lblITBIS.Text = "RD$ 0.00";
            lblITBIS.TextAlign = ContentAlignment.TopRight;
            // 
            // lblTotalLabel
            // 
            lblTotalLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTotalLabel.ForeColor = Color.White;
            lblTotalLabel.Location = new Point(21, 130);
            lblTotalLabel.Margin = new Padding(4, 0, 4, 0);
            lblTotalLabel.Name = "lblTotalLabel";
            lblTotalLabel.Size = new Size(200, 57);
            lblTotalLabel.TabIndex = 4;
            lblTotalLabel.Text = "TOTAL:";
            // 
            // lblTotal
            // 
            lblTotal.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTotal.ForeColor = Color.FromArgb(74, 222, 128);
            lblTotal.Location = new Point(200, 130);
            lblTotal.Margin = new Padding(4, 0, 4, 0);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(314, 57);
            lblTotal.TabIndex = 5;
            lblTotal.Text = "RD$ 0.00";
            lblTotal.TextAlign = ContentAlignment.TopRight;
            // 
            // btnCobrar
            // 
            btnCobrar.BackColor = Color.FromArgb(22, 163, 74);
            btnCobrar.Cursor = Cursors.Hand;
            btnCobrar.Enabled = false;
            btnCobrar.FlatAppearance.BorderSize = 0;
            btnCobrar.FlatStyle = FlatStyle.Flat;
            btnCobrar.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnCobrar.ForeColor = Color.White;
            btnCobrar.Location = new Point(21, 200);
            btnCobrar.Margin = new Padding(4, 5, 4, 5);
            btnCobrar.Name = "btnCobrar";
            btnCobrar.Size = new Size(493, 67);
            btnCobrar.TabIndex = 6;
            btnCobrar.Text = "💳  COBRAR";
            btnCobrar.UseVisualStyleBackColor = false;
            btnCobrar.Click += btnCobrar_Click;
            // 
            // panelBuscar
            // 
            panelBuscar.BackColor = Color.WhiteSmoke;
            panelBuscar.Controls.Add(txtBuscar);
            panelBuscar.Dock = DockStyle.Top;
            panelBuscar.Location = new Point(0, 60);
            panelBuscar.Margin = new Padding(4, 5, 4, 5);
            panelBuscar.Name = "panelBuscar";
            panelBuscar.Padding = new Padding(11, 10, 11, 10);
            panelBuscar.Size = new Size(885, 73);
            panelBuscar.TabIndex = 2;
            // 
            // txtBuscar
            // 
            txtBuscar.BorderStyle = BorderStyle.FixedSingle;
            txtBuscar.Dock = DockStyle.Fill;
            txtBuscar.Font = new Font("Segoe UI", 11F);
            txtBuscar.Location = new Point(11, 10);
            txtBuscar.Margin = new Padding(4, 5, 4, 5);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "🔍 Buscar producto o servicio...";
            txtBuscar.Size = new Size(863, 37);
            txtBuscar.TabIndex = 0;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            // 
            // panelAgregar
            // 
            panelAgregar.BackColor = Color.WhiteSmoke;
            panelAgregar.Controls.Add(btnAgregar);
            panelAgregar.Dock = DockStyle.Bottom;
            panelAgregar.Location = new Point(0, 961);
            panelAgregar.Margin = new Padding(4, 5, 4, 5);
            panelAgregar.Name = "panelAgregar";
            panelAgregar.Padding = new Padding(11, 10, 11, 10);
            panelAgregar.Size = new Size(885, 73);
            panelAgregar.TabIndex = 1;
            // 
            // btnAgregar
            // 
            btnAgregar.BackColor = Color.FromArgb(37, 99, 235);
            btnAgregar.Cursor = Cursors.Hand;
            btnAgregar.Dock = DockStyle.Fill;
            btnAgregar.FlatAppearance.BorderSize = 0;
            btnAgregar.FlatStyle = FlatStyle.Flat;
            btnAgregar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAgregar.ForeColor = Color.White;
            btnAgregar.Location = new Point(11, 10);
            btnAgregar.Margin = new Padding(4, 5, 4, 5);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.Size = new Size(863, 53);
            btnAgregar.TabIndex = 0;
            btnAgregar.Text = "➕ Agregar al carrito (o doble clic)";
            btnAgregar.UseVisualStyleBackColor = false;
            btnAgregar.Click += btnAgregar_Click;
            // 
            // panelCarritoBtns
            // 
            panelCarritoBtns.BackColor = Color.WhiteSmoke;
            panelCarritoBtns.Controls.Add(btnQuitarItem);
            panelCarritoBtns.Controls.Add(btnLimpiar);
            panelCarritoBtns.Dock = DockStyle.Bottom;
            panelCarritoBtns.Location = new Point(0, 761);
            panelCarritoBtns.Margin = new Padding(4, 5, 4, 5);
            panelCarritoBtns.Name = "panelCarritoBtns";
            panelCarritoBtns.Padding = new Padding(11, 8, 11, 8);
            panelCarritoBtns.Size = new Size(906, 63);
            panelCarritoBtns.TabIndex = 1;
            // 
            // btnQuitarItem
            // 
            btnQuitarItem.BackColor = Color.FromArgb(239, 68, 68);
            btnQuitarItem.Cursor = Cursors.Hand;
            btnQuitarItem.FlatAppearance.BorderSize = 0;
            btnQuitarItem.FlatStyle = FlatStyle.Flat;
            btnQuitarItem.Font = new Font("Segoe UI", 9F);
            btnQuitarItem.ForeColor = Color.White;
            btnQuitarItem.Location = new Point(11, 8);
            btnQuitarItem.Margin = new Padding(4, 5, 4, 5);
            btnQuitarItem.Name = "btnQuitarItem";
            btnQuitarItem.Size = new Size(229, 47);
            btnQuitarItem.TabIndex = 0;
            btnQuitarItem.Text = "🗑 Quitar seleccionado";
            btnQuitarItem.UseVisualStyleBackColor = false;
            btnQuitarItem.Click += btnQuitarItem_Click;
            // 
            // btnLimpiar
            // 
            btnLimpiar.BackColor = Color.FromArgb(100, 116, 139);
            btnLimpiar.Cursor = Cursors.Hand;
            btnLimpiar.FlatAppearance.BorderSize = 0;
            btnLimpiar.FlatStyle = FlatStyle.Flat;
            btnLimpiar.Font = new Font("Segoe UI", 9F);
            btnLimpiar.ForeColor = Color.White;
            btnLimpiar.Location = new Point(257, 8);
            btnLimpiar.Margin = new Padding(4, 5, 4, 5);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(129, 47);
            btnLimpiar.TabIndex = 1;
            btnLimpiar.Text = "\U0001f9f9 Limpiar";
            btnLimpiar.UseVisualStyleBackColor = false;
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // splitMain
            // 
            splitMain.Dock = DockStyle.Fill;
            splitMain.Location = new Point(0, 93);
            splitMain.Margin = new Padding(4, 5, 4, 5);
            splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            splitMain.Panel1.Controls.Add(lvCatalogo);
            splitMain.Panel1.Controls.Add(panelAgregar);
            splitMain.Panel1.Controls.Add(panelBuscar);
            splitMain.Panel1.Controls.Add(panelCliente);
            splitMain.Panel1.Controls.Add(lblCatalogoTitle);
            splitMain.Panel1MinSize = 400;
            // 
            // splitMain.Panel2
            // 
            splitMain.Panel2.Controls.Add(lvCarrito);
            splitMain.Panel2.Controls.Add(panelCarritoBtns);
            splitMain.Panel2.Controls.Add(panelTotales);
            splitMain.Panel2.Controls.Add(lblItemsCount);
            splitMain.Panel2.Controls.Add(lblCarritoTitle);
            splitMain.Panel2MinSize = 380;
            splitMain.Size = new Size(1800, 1107);
            splitMain.SplitterDistance = 885;
            splitMain.SplitterWidth = 9;
            splitMain.TabIndex = 0;
            // 
            // lvCatalogo
            // 
            lvCatalogo.BorderStyle = BorderStyle.None;
            lvCatalogo.Columns.AddRange(new ColumnHeader[] { colNombre, colPrecio, colStock, colCategoria });
            lvCatalogo.Dock = DockStyle.Fill;
            lvCatalogo.Font = new Font("Segoe UI", 10F);
            lvCatalogo.FullRowSelect = true;
            lvCatalogo.GridLines = true;
            lvCatalogo.Location = new Point(0, 133);
            lvCatalogo.Margin = new Padding(4, 5, 4, 5);
            lvCatalogo.Name = "lvCatalogo";
            lvCatalogo.Size = new Size(885, 828);
            lvCatalogo.TabIndex = 0;
            lvCatalogo.UseCompatibleStateImageBehavior = false;
            lvCatalogo.View = View.Details;
            lvCatalogo.DoubleClick += lvCatalogo_DoubleClick;
            // 
            // colNombre
            // 
            colNombre.Text = "Nombre";
            colNombre.Width = 250;
            // 
            // colPrecio
            // 
            colPrecio.Text = "Precio";
            colPrecio.Width = 120;
            // 
            // colStock
            // 
            colStock.Text = "Disponible";
            colStock.Width = 90;
            // 
            // colCategoria
            // 
            colCategoria.Text = "Categoría";
            colCategoria.Width = 120;
            // 
            // panelCliente
            // 
            panelCliente.BackColor = Color.FromArgb(241, 245, 249);
            panelCliente.Controls.Add(lblClienteTitle);
            panelCliente.Controls.Add(lblClienteActual);
            panelCliente.Controls.Add(txtBuscarCliente);
            panelCliente.Controls.Add(btnBuscarCliente);
            panelCliente.Controls.Add(btnClienteAnonimo);
            panelCliente.Dock = DockStyle.Bottom;
            panelCliente.Location = new Point(0, 1034);
            panelCliente.Margin = new Padding(4, 5, 4, 5);
            panelCliente.Name = "panelCliente";
            panelCliente.Padding = new Padding(11, 10, 11, 10);
            panelCliente.Size = new Size(885, 73);
            panelCliente.TabIndex = 3;
            // 
            // lblClienteTitle
            // 
            lblClienteTitle.AutoSize = true;
            lblClienteTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblClienteTitle.ForeColor = Color.FromArgb(71, 85, 105);
            lblClienteTitle.Location = new Point(11, 10);
            lblClienteTitle.Margin = new Padding(4, 0, 4, 0);
            lblClienteTitle.Name = "lblClienteTitle";
            lblClienteTitle.Size = new Size(76, 25);
            lblClienteTitle.TabIndex = 0;
            lblClienteTitle.Text = "Cliente:";
            // 
            // lblClienteActual
            // 
            lblClienteActual.AutoSize = true;
            lblClienteActual.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblClienteActual.ForeColor = Color.FromArgb(100, 116, 139);
            lblClienteActual.Location = new Point(93, 10);
            lblClienteActual.Margin = new Padding(4, 0, 4, 0);
            lblClienteActual.Name = "lblClienteActual";
            lblClienteActual.Size = new Size(154, 25);
            lblClienteActual.TabIndex = 1;
            lblClienteActual.Text = "Cliente Anónimo";
            // 
            // txtBuscarCliente
            // 
            txtBuscarCliente.BorderStyle = BorderStyle.FixedSingle;
            txtBuscarCliente.Font = new Font("Segoe UI", 9F);
            txtBuscarCliente.Location = new Point(11, 47);
            txtBuscarCliente.Margin = new Padding(4, 5, 4, 5);
            txtBuscarCliente.Name = "txtBuscarCliente";
            txtBuscarCliente.PlaceholderText = "Nombre o cédula...";
            txtBuscarCliente.Size = new Size(342, 31);
            txtBuscarCliente.TabIndex = 2;
            // 
            // btnBuscarCliente
            // 
            btnBuscarCliente.BackColor = Color.FromArgb(59, 130, 246);
            btnBuscarCliente.Cursor = Cursors.Hand;
            btnBuscarCliente.FlatAppearance.BorderSize = 0;
            btnBuscarCliente.FlatStyle = FlatStyle.Flat;
            btnBuscarCliente.Font = new Font("Segoe UI", 9F);
            btnBuscarCliente.ForeColor = Color.White;
            btnBuscarCliente.Location = new Point(364, 43);
            btnBuscarCliente.Margin = new Padding(4, 5, 4, 5);
            btnBuscarCliente.Name = "btnBuscarCliente";
            btnBuscarCliente.Size = new Size(143, 43);
            btnBuscarCliente.TabIndex = 3;
            btnBuscarCliente.Text = "Buscar";
            btnBuscarCliente.UseVisualStyleBackColor = false;
            btnBuscarCliente.Click += btnBuscarCliente_Click;
            // 
            // btnClienteAnonimo
            // 
            btnClienteAnonimo.BackColor = Color.FromArgb(100, 116, 139);
            btnClienteAnonimo.Cursor = Cursors.Hand;
            btnClienteAnonimo.FlatAppearance.BorderSize = 0;
            btnClienteAnonimo.FlatStyle = FlatStyle.Flat;
            btnClienteAnonimo.Font = new Font("Segoe UI", 9F);
            btnClienteAnonimo.ForeColor = Color.White;
            btnClienteAnonimo.Location = new Point(517, 43);
            btnClienteAnonimo.Margin = new Padding(4, 5, 4, 5);
            btnClienteAnonimo.Name = "btnClienteAnonimo";
            btnClienteAnonimo.Size = new Size(171, 43);
            btnClienteAnonimo.TabIndex = 4;
            btnClienteAnonimo.Text = "Anónimo";
            btnClienteAnonimo.UseVisualStyleBackColor = false;
            btnClienteAnonimo.Click += btnClienteAnonimo_Click;
            // 
            // lblCatalogoTitle
            // 
            lblCatalogoTitle.BackColor = Color.FromArgb(30, 41, 59);
            lblCatalogoTitle.Dock = DockStyle.Top;
            lblCatalogoTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblCatalogoTitle.ForeColor = Color.White;
            lblCatalogoTitle.Location = new Point(0, 0);
            lblCatalogoTitle.Margin = new Padding(4, 0, 4, 0);
            lblCatalogoTitle.Name = "lblCatalogoTitle";
            lblCatalogoTitle.Size = new Size(885, 60);
            lblCatalogoTitle.TabIndex = 4;
            lblCatalogoTitle.Text = "  📦 CATÁLOGO DE PRODUCTOS Y SERVICIOS";
            lblCatalogoTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lvCarrito
            // 
            lvCarrito.BorderStyle = BorderStyle.None;
            lvCarrito.Columns.AddRange(new ColumnHeader[] { colCNombre, colCTipo, colCCant, colCPrecio, colCSubtotal });
            lvCarrito.Dock = DockStyle.Fill;
            lvCarrito.Font = new Font("Segoe UI", 10F);
            lvCarrito.FullRowSelect = true;
            lvCarrito.GridLines = true;
            lvCarrito.Location = new Point(0, 100);
            lvCarrito.Margin = new Padding(4, 5, 4, 5);
            lvCarrito.Name = "lvCarrito";
            lvCarrito.Size = new Size(906, 661);
            lvCarrito.TabIndex = 0;
            lvCarrito.UseCompatibleStateImageBehavior = false;
            lvCarrito.View = View.Details;
            lvCarrito.DoubleClick += lvCarrito_DoubleClick;
            lvCarrito.KeyDown += lvCarrito_KeyDown;
            // 
            // colCNombre
            // 
            colCNombre.Text = "Descripción";
            colCNombre.Width = 200;
            // 
            // colCTipo
            // 
            colCTipo.Text = "Tipo";
            colCTipo.Width = 70;
            // 
            // colCCant
            // 
            colCCant.Text = "Cant.";
            colCCant.Width = 55;
            // 
            // colCPrecio
            // 
            colCPrecio.Text = "P. Unit.";
            colCPrecio.Width = 100;
            // 
            // colCSubtotal
            // 
            colCSubtotal.Text = "Subtotal";
            colCSubtotal.Width = 105;
            // 
            // lblItemsCount
            // 
            lblItemsCount.BackColor = Color.WhiteSmoke;
            lblItemsCount.Dock = DockStyle.Top;
            lblItemsCount.Font = new Font("Segoe UI", 9F);
            lblItemsCount.ForeColor = Color.FromArgb(100, 116, 139);
            lblItemsCount.Location = new Point(0, 60);
            lblItemsCount.Margin = new Padding(4, 0, 4, 0);
            lblItemsCount.Name = "lblItemsCount";
            lblItemsCount.Padding = new Padding(0, 0, 14, 0);
            lblItemsCount.Size = new Size(906, 40);
            lblItemsCount.TabIndex = 3;
            lblItemsCount.Text = "0 items";
            lblItemsCount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCarritoTitle
            // 
            lblCarritoTitle.BackColor = Color.FromArgb(30, 41, 59);
            lblCarritoTitle.Dock = DockStyle.Top;
            lblCarritoTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblCarritoTitle.ForeColor = Color.White;
            lblCarritoTitle.Location = new Point(0, 0);
            lblCarritoTitle.Margin = new Padding(4, 0, 4, 0);
            lblCarritoTitle.Name = "lblCarritoTitle";
            lblCarritoTitle.Size = new Size(906, 60);
            lblCarritoTitle.TabIndex = 4;
            lblCarritoTitle.Text = "  \U0001f6d2 DETALLE DE VENTA";
            lblCarritoTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // frmCobro
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1800, 1200);
            Controls.Add(splitMain);
            Controls.Add(panelTop);
            Margin = new Padding(4, 5, 4, 5);
            MinimumSize = new Size(1419, 963);
            Name = "frmCobro";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Taller de Frenos — Pantalla de Cobro";
            WindowState = FormWindowState.Maximized;
            Load += frmCobro_Load;
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            panelTotales.ResumeLayout(false);
            panelBuscar.ResumeLayout(false);
            panelBuscar.PerformLayout();
            panelAgregar.ResumeLayout(false);
            panelCarritoBtns.ResumeLayout(false);
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            panelCliente.ResumeLayout(false);
            panelCliente.PerformLayout();
            ResumeLayout(false);
        }

        // Controls
        private Panel panelTop, panelTotales, panelCliente, panelBuscar, panelAgregar, panelCarritoBtns;
        private SplitContainer splitMain;
        private Label lblAppTitle, lblCajero, lblTurno, lblFecha, lblPendientes;
        private Button btnCatalogo, btnBuscarFactura, btnCierreDia;
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
