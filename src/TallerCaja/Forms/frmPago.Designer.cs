namespace TallerCaja.Forms
{
    partial class frmPago
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelHeader = new Panel();
            lblTitulo = new Label();
            splitPago = new SplitContainer();
            lvItems = new ListView();
            colDesc = new ColumnHeader();
            colTipo = new ColumnHeader();
            colCant = new ColumnHeader();
            colPUnit = new ColumnHeader();
            colSub = new ColumnHeader();
            panelResumen = new Panel();
            lblClienteLabel = new Label();
            lblClienteVal = new Label();
            lblSubtotalLabel2 = new Label();
            lblSubtotalVal = new Label();
            lblITBISLabel2 = new Label();
            lblITBISVal = new Label();
            lblTotalLabel2 = new Label();
            lblTotalVal = new Label();
            lblDetalleTitle = new Label();
            panelPagoContent = new Panel();
            lblMetodoPagoTitle = new Label();
            btnEfectivo = new Button();
            btnTarjeta = new Button();
            btnTransferencia = new Button();
            btnCredito = new Button();
            lblInfoMetodo = new Label();
            lblMontoPagadoLabel = new Label();
            lblRDSign = new Label();
            txtMontoPagado = new TextBox();
            panelCambio = new Panel();
            lblCambioLabel = new Label();
            lblCambioVal = new Label();
            btnConfirmar = new Button();
            btnCancelar = new Button();
            lblPagoTitle = new Label();
            panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitPago).BeginInit();
            splitPago.Panel1.SuspendLayout();
            splitPago.Panel2.SuspendLayout();
            splitPago.SuspendLayout();
            panelResumen.SuspendLayout();
            panelPagoContent.SuspendLayout();
            panelCambio.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(15, 23, 42);
            panelHeader.Controls.Add(lblTitulo);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1460, 66);
            panelHeader.TabIndex = 1;
            // 
            // lblTitulo
            // 
            lblTitulo.Dock = DockStyle.Fill;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(1460, 66);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "  💳  PROCESAR COBRO";
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // splitPago
            // 
            splitPago.Dock = DockStyle.Fill;
            splitPago.Location = new Point(0, 66);
            splitPago.Margin = new Padding(4, 5, 4, 5);
            splitPago.Name = "splitPago";
            // 
            // splitPago.Panel1
            // 
            splitPago.Panel1.Controls.Add(lvItems);
            splitPago.Panel1.Controls.Add(panelResumen);
            splitPago.Panel1.Controls.Add(lblDetalleTitle);
            splitPago.Panel1MinSize = 700;
            // 
            // splitPago.Panel2
            // 
            splitPago.Panel2.Controls.Add(panelPagoContent);
            splitPago.Panel2.Controls.Add(lblPagoTitle);
            splitPago.Panel2MinSize = 560;
            splitPago.Size = new Size(1460, 854);
            splitPago.SplitterDistance = 880;
            splitPago.SplitterWidth = 6;
            splitPago.TabIndex = 0;
            // 
            // lvItems
            // 
            lvItems.BorderStyle = BorderStyle.None;
            lvItems.Columns.AddRange(new ColumnHeader[] { colDesc, colTipo, colCant, colPUnit, colSub });
            lvItems.Dock = DockStyle.Fill;
            lvItems.Font = new Font("Segoe UI", 10F);
            lvItems.FullRowSelect = true;
            lvItems.GridLines = true;
            lvItems.Location = new Point(0, 42);
            lvItems.Name = "lvItems";
            lvItems.Size = new Size(880, 662);
            lvItems.TabIndex = 0;
            lvItems.UseCompatibleStateImageBehavior = false;
            lvItems.View = View.Details;
            // 
            // colDesc
            // 
            colDesc.Text = "Descripción";
            colDesc.Width = 270;
            // 
            // colTipo
            // 
            colTipo.Text = "Tipo";
            colTipo.Width = 90;
            // 
            // colCant
            // 
            colCant.Text = "Cant.";
            // 
            // colPUnit
            // 
            colPUnit.Text = "P. Unit.";
            colPUnit.Width = 120;
            // 
            // colSub
            // 
            colSub.Text = "Subtotal";
            colSub.Width = 130;
            // 
            // panelResumen
            // 
            panelResumen.BackColor = Color.FromArgb(30, 41, 59);
            panelResumen.Controls.Add(lblClienteLabel);
            panelResumen.Controls.Add(lblClienteVal);
            panelResumen.Controls.Add(lblSubtotalLabel2);
            panelResumen.Controls.Add(lblSubtotalVal);
            panelResumen.Controls.Add(lblITBISLabel2);
            panelResumen.Controls.Add(lblITBISVal);
            panelResumen.Controls.Add(lblTotalLabel2);
            panelResumen.Controls.Add(lblTotalVal);
            panelResumen.Dock = DockStyle.Bottom;
            panelResumen.Location = new Point(0, 704);
            panelResumen.Name = "panelResumen";
            panelResumen.Padding = new Padding(16, 12, 16, 12);
            panelResumen.Size = new Size(880, 150);
            panelResumen.TabIndex = 1;
            // 
            // lblClienteLabel
            // 
            lblClienteLabel.Font = new Font("Segoe UI", 9F);
            lblClienteLabel.ForeColor = Color.FromArgb(148, 163, 184);
            lblClienteLabel.Location = new Point(16, 10);
            lblClienteLabel.Name = "lblClienteLabel";
            lblClienteLabel.Size = new Size(80, 26);
            lblClienteLabel.TabIndex = 0;
            lblClienteLabel.Text = "Cliente:";
            // 
            // lblClienteVal
            // 
            lblClienteVal.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblClienteVal.ForeColor = Color.FromArgb(74, 222, 128);
            lblClienteVal.Location = new Point(96, 10);
            lblClienteVal.Name = "lblClienteVal";
            lblClienteVal.Size = new Size(540, 26);
            lblClienteVal.TabIndex = 1;
            // 
            // lblSubtotalLabel2
            // 
            lblSubtotalLabel2.Font = new Font("Segoe UI", 10F);
            lblSubtotalLabel2.ForeColor = Color.FromArgb(148, 163, 184);
            lblSubtotalLabel2.Location = new Point(16, 42);
            lblSubtotalLabel2.Name = "lblSubtotalLabel2";
            lblSubtotalLabel2.Size = new Size(130, 24);
            lblSubtotalLabel2.TabIndex = 2;
            lblSubtotalLabel2.Text = "Subtotal:";
            // 
            // lblSubtotalVal
            // 
            lblSubtotalVal.Font = new Font("Segoe UI", 10F);
            lblSubtotalVal.ForeColor = Color.White;
            lblSubtotalVal.Location = new Point(500, 42);
            lblSubtotalVal.Name = "lblSubtotalVal";
            lblSubtotalVal.Size = new Size(300, 24);
            lblSubtotalVal.TabIndex = 3;
            lblSubtotalVal.TextAlign = ContentAlignment.TopRight;
            // 
            // lblITBISLabel2
            // 
            lblITBISLabel2.Font = new Font("Segoe UI", 10F);
            lblITBISLabel2.ForeColor = Color.FromArgb(148, 163, 184);
            lblITBISLabel2.Location = new Point(16, 72);
            lblITBISLabel2.Name = "lblITBISLabel2";
            lblITBISLabel2.Size = new Size(130, 24);
            lblITBISLabel2.TabIndex = 4;
            lblITBISLabel2.Text = "ITBIS (18%):";
            // 
            // lblITBISVal
            // 
            lblITBISVal.Font = new Font("Segoe UI", 10F);
            lblITBISVal.ForeColor = Color.FromArgb(251, 146, 60);
            lblITBISVal.Location = new Point(500, 72);
            lblITBISVal.Name = "lblITBISVal";
            lblITBISVal.Size = new Size(300, 24);
            lblITBISVal.TabIndex = 5;
            lblITBISVal.TextAlign = ContentAlignment.TopRight;
            // 
            // lblTotalLabel2
            // 
            lblTotalLabel2.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTotalLabel2.ForeColor = Color.White;
            lblTotalLabel2.Location = new Point(16, 104);
            lblTotalLabel2.Name = "lblTotalLabel2";
            lblTotalLabel2.Size = new Size(220, 30);
            lblTotalLabel2.TabIndex = 6;
            lblTotalLabel2.Text = "TOTAL A COBRAR:";
            // 
            // lblTotalVal
            // 
            lblTotalVal.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTotalVal.ForeColor = Color.FromArgb(74, 222, 128);
            lblTotalVal.Location = new Point(460, 104);
            lblTotalVal.Name = "lblTotalVal";
            lblTotalVal.Size = new Size(340, 30);
            lblTotalVal.TabIndex = 7;
            lblTotalVal.TextAlign = ContentAlignment.TopRight;
            // 
            // lblDetalleTitle
            // 
            lblDetalleTitle.BackColor = Color.FromArgb(30, 41, 59);
            lblDetalleTitle.Dock = DockStyle.Top;
            lblDetalleTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDetalleTitle.ForeColor = Color.White;
            lblDetalleTitle.Location = new Point(0, 0);
            lblDetalleTitle.Name = "lblDetalleTitle";
            lblDetalleTitle.Size = new Size(880, 42);
            lblDetalleTitle.TabIndex = 2;
            lblDetalleTitle.Text = "  Detalle de la venta";
            lblDetalleTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panelPagoContent
            // 
            panelPagoContent.BackColor = Color.FromArgb(248, 250, 252);
            panelPagoContent.Controls.Add(lblMetodoPagoTitle);
            panelPagoContent.Controls.Add(btnEfectivo);
            panelPagoContent.Controls.Add(btnTarjeta);
            panelPagoContent.Controls.Add(btnTransferencia);
            panelPagoContent.Controls.Add(btnCredito);
            panelPagoContent.Controls.Add(lblInfoMetodo);
            panelPagoContent.Controls.Add(lblMontoPagadoLabel);
            panelPagoContent.Controls.Add(lblRDSign);
            panelPagoContent.Controls.Add(txtMontoPagado);
            panelPagoContent.Controls.Add(panelCambio);
            panelPagoContent.Controls.Add(btnConfirmar);
            panelPagoContent.Controls.Add(btnCancelar);
            panelPagoContent.Dock = DockStyle.Fill;
            panelPagoContent.Location = new Point(0, 42);
            panelPagoContent.Name = "panelPagoContent";
            panelPagoContent.Padding = new Padding(16);
            panelPagoContent.Size = new Size(574, 812);
            panelPagoContent.TabIndex = 0;
            // 
            // lblMetodoPagoTitle
            // 
            lblMetodoPagoTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblMetodoPagoTitle.ForeColor = Color.FromArgb(30, 41, 59);
            lblMetodoPagoTitle.Location = new Point(16, 16);
            lblMetodoPagoTitle.Name = "lblMetodoPagoTitle";
            lblMetodoPagoTitle.Size = new Size(520, 28);
            lblMetodoPagoTitle.TabIndex = 0;
            lblMetodoPagoTitle.Text = "Selecciona el método de pago:";
            // 
            // btnEfectivo
            // 
            ConfigurarBotonMetodoPago(btnEfectivo, 16, 52, "Efectivo", "💵");
            btnEfectivo.Name = "btnEfectivo";
            btnEfectivo.TabIndex = 1;
            btnEfectivo.Click += btnEfectivo_Click;
            // 
            // btnTarjeta
            // 
            ConfigurarBotonMetodoPago(btnTarjeta, 188, 52, "Tarjeta", "💳");
            btnTarjeta.Name = "btnTarjeta";
            btnTarjeta.TabIndex = 2;
            btnTarjeta.Click += btnTarjeta_Click;
            // 
            // btnTransferencia
            // 
            ConfigurarBotonMetodoPago(btnTransferencia, 360, 52, "Transferencia", "🏦");
            btnTransferencia.Name = "btnTransferencia";
            btnTransferencia.TabIndex = 3;
            btnTransferencia.Click += btnTransferencia_Click;
            // 
            // btnCredito
            // 
            ConfigurarBotonMetodoPago(btnCredito, 16, 120, "Crédito", "📋");
            btnCredito.Name = "btnCredito";
            btnCredito.TabIndex = 4;
            btnCredito.Click += btnCredito_Click;
            // 
            // lblInfoMetodo
            // 
            lblInfoMetodo.Font = new Font("Segoe UI", 9F);
            lblInfoMetodo.ForeColor = Color.FromArgb(74, 222, 128);
            lblInfoMetodo.Location = new Point(16, 188);
            lblInfoMetodo.Name = "lblInfoMetodo";
            lblInfoMetodo.Size = new Size(520, 52);
            lblInfoMetodo.TabIndex = 5;
            // 
            // lblMontoPagadoLabel
            // 
            lblMontoPagadoLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblMontoPagadoLabel.ForeColor = Color.FromArgb(30, 41, 59);
            lblMontoPagadoLabel.Location = new Point(16, 250);
            lblMontoPagadoLabel.Name = "lblMontoPagadoLabel";
            lblMontoPagadoLabel.Size = new Size(520, 24);
            lblMontoPagadoLabel.TabIndex = 6;
            lblMontoPagadoLabel.Text = "Monto recibido (efectivo):";
            // 
            // lblRDSign
            // 
            lblRDSign.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblRDSign.ForeColor = Color.FromArgb(71, 85, 105);
            lblRDSign.Location = new Point(16, 280);
            lblRDSign.Name = "lblRDSign";
            lblRDSign.Size = new Size(52, 34);
            lblRDSign.TabIndex = 7;
            lblRDSign.Text = "RD$";
            // 
            // txtMontoPagado
            // 
            txtMontoPagado.BorderStyle = BorderStyle.FixedSingle;
            txtMontoPagado.Font = new Font("Segoe UI", 14F);
            txtMontoPagado.Location = new Point(74, 278);
            txtMontoPagado.Name = "txtMontoPagado";
            txtMontoPagado.Size = new Size(240, 45);
            txtMontoPagado.TabIndex = 8;
            txtMontoPagado.TextAlign = HorizontalAlignment.Right;
            txtMontoPagado.TextChanged += txtMontoPagado_TextChanged;
            // 
            // panelCambio
            // 
            panelCambio.BackColor = Color.FromArgb(220, 252, 231);
            panelCambio.Controls.Add(lblCambioLabel);
            panelCambio.Controls.Add(lblCambioVal);
            panelCambio.Location = new Point(16, 332);
            panelCambio.Name = "panelCambio";
            panelCambio.Size = new Size(520, 52);
            panelCambio.TabIndex = 9;
            // 
            // lblCambioLabel
            // 
            lblCambioLabel.AutoSize = true;
            lblCambioLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCambioLabel.ForeColor = Color.FromArgb(21, 128, 61);
            lblCambioLabel.Location = new Point(12, 14);
            lblCambioLabel.Name = "lblCambioLabel";
            lblCambioLabel.Size = new Size(193, 28);
            lblCambioLabel.TabIndex = 0;
            lblCambioLabel.Text = "Cambio a entregar:";
            // 
            // lblCambioVal
            // 
            lblCambioVal.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCambioVal.ForeColor = Color.FromArgb(22, 163, 74);
            lblCambioVal.Location = new Point(300, 14);
            lblCambioVal.Name = "lblCambioVal";
            lblCambioVal.Size = new Size(200, 24);
            lblCambioVal.TabIndex = 1;
            lblCambioVal.Text = "RD$ 0.00";
            lblCambioVal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btnConfirmar
            // 
            btnConfirmar.BackColor = Color.FromArgb(22, 163, 74);
            btnConfirmar.Cursor = Cursors.Hand;
            btnConfirmar.FlatAppearance.BorderSize = 0;
            btnConfirmar.FlatStyle = FlatStyle.Flat;
            btnConfirmar.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnConfirmar.ForeColor = Color.White;
            btnConfirmar.Location = new Point(16, 400);
            btnConfirmar.Name = "btnConfirmar";
            btnConfirmar.Size = new Size(360, 58);
            btnConfirmar.TabIndex = 10;
            btnConfirmar.Text = "✔  CONFIRMAR COBRO";
            btnConfirmar.UseVisualStyleBackColor = false;
            btnConfirmar.Click += btnConfirmar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.FromArgb(239, 68, 68);
            btnCancelar.Cursor = Cursors.Hand;
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 10F);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(388, 400);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(148, 58);
            btnCancelar.TabIndex = 11;
            btnCancelar.Text = "✕ Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // lblPagoTitle
            // 
            lblPagoTitle.BackColor = Color.FromArgb(30, 41, 59);
            lblPagoTitle.Dock = DockStyle.Top;
            lblPagoTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPagoTitle.ForeColor = Color.White;
            lblPagoTitle.Location = new Point(0, 0);
            lblPagoTitle.Name = "lblPagoTitle";
            lblPagoTitle.Size = new Size(574, 42);
            lblPagoTitle.TabIndex = 1;
            lblPagoTitle.Text = "  Método de Pago";
            lblPagoTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // frmPago
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1460, 920);
            Controls.Add(splitPago);
            Controls.Add(panelHeader);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Margin = new Padding(4, 5, 4, 5);
            MinimizeBox = false;
            Name = "frmPago";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Procesar Cobro";
            Load += frmPago_Load;
            panelHeader.ResumeLayout(false);
            splitPago.Panel1.ResumeLayout(false);
            splitPago.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitPago).EndInit();
            splitPago.ResumeLayout(false);
            panelResumen.ResumeLayout(false);
            panelPagoContent.ResumeLayout(false);
            panelPagoContent.PerformLayout();
            panelCambio.ResumeLayout(false);
            panelCambio.PerformLayout();
            ResumeLayout(false);
        }

        private static void ConfigurarBotonMetodoPago(Button b, int x, int y, string text, string emoji)
        {
            b.Location = new Point(x, y);
            b.Size = new Size(160, 60);
            b.Text = $"{emoji}\n{text}";
            b.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            b.BackColor = Color.FromArgb(51, 65, 85);
            b.ForeColor = Color.FromArgb(148, 163, 184);
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.BorderColor = Color.FromArgb(71, 85, 105);
            b.Cursor = Cursors.Hand;
        }

        private Panel panelHeader, panelResumen, panelCambio, panelPagoContent;
        private Label lblTitulo;
        private SplitContainer splitPago;
        private Label lblDetalleTitle;
        private ListView lvItems;
        private ColumnHeader colDesc, colTipo, colCant, colPUnit, colSub;
        private Label lblClienteLabel, lblClienteVal;
        private Label lblSubtotalLabel2, lblSubtotalVal;
        private Label lblITBISLabel2, lblITBISVal;
        private Label lblTotalLabel2, lblTotalVal;
        private Label lblPagoTitle, lblMetodoPagoTitle;
        private Button btnEfectivo, btnTarjeta, btnTransferencia, btnCredito;
        private Label lblInfoMetodo;
        private Label lblMontoPagadoLabel;
        private Label lblRDSign;
        private TextBox txtMontoPagado;
        private Label lblCambioLabel, lblCambioVal;
        private Button btnConfirmar, btnCancelar;
    }
}
