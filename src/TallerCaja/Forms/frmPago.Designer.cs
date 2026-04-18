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
            txtMontoPagado = new TextBox();
            lblRDSign = new Label();
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
            panelHeader.Margin = new Padding(4, 5, 4, 5);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1594, 83);
            panelHeader.TabIndex = 1;
            // 
            // lblTitulo
            // 
            lblTitulo.Dock = DockStyle.Fill;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Margin = new Padding(4, 0, 4, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(1594, 83);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "  💳  PROCESAR COBRO";
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // splitPago
            // 
            splitPago.Dock = DockStyle.Fill;
            splitPago.Location = new Point(0, 83);
            splitPago.Margin = new Padding(4, 5, 4, 5);
            splitPago.Name = "splitPago";
            // 
            // splitPago.Panel1
            // 
            splitPago.Panel1.Controls.Add(lvItems);
            splitPago.Panel1.Controls.Add(panelResumen);
            splitPago.Panel1.Controls.Add(lblDetalleTitle);
            // 
            // splitPago.Panel2
            // 
            splitPago.Panel2.Controls.Add(lblMetodoPagoTitle);
            splitPago.Panel2.Controls.Add(panelPagoContent);
            splitPago.Panel2.Controls.Add(lblPagoTitle);
            splitPago.Size = new Size(1594, 984);
            splitPago.SplitterDistance = 1285;
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
            lvItems.Location = new Point(0, 53);
            lvItems.Margin = new Padding(4, 5, 4, 5);
            lvItems.Name = "lvItems";
            lvItems.Size = new Size(1285, 764);
            lvItems.TabIndex = 0;
            lvItems.UseCompatibleStateImageBehavior = false;
            lvItems.View = View.Details;
            // 
            // colDesc
            // 
            colDesc.Text = "Descripción";
            colDesc.Width = 200;
            // 
            // colTipo
            // 
            colTipo.Text = "Tipo";
            colTipo.Width = 70;
            // 
            // colCant
            // 
            colCant.Text = "Cant.";
            colCant.Width = 50;
            // 
            // colPUnit
            // 
            colPUnit.Text = "P. Unit.";
            colPUnit.Width = 95;
            // 
            // colSub
            // 
            colSub.Text = "Subtotal";
            colSub.Width = 95;
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
            panelResumen.Location = new Point(0, 817);
            panelResumen.Margin = new Padding(4, 5, 4, 5);
            panelResumen.Name = "panelResumen";
            panelResumen.Padding = new Padding(21, 13, 21, 13);
            panelResumen.Size = new Size(1285, 167);
            panelResumen.TabIndex = 1;
            // 
            // lblClienteLabel
            // 
            lblClienteLabel.Font = new Font("Segoe UI", 9F);
            lblClienteLabel.ForeColor = Color.FromArgb(148, 163, 184);
            lblClienteLabel.Location = new Point(14, 10);
            lblClienteLabel.Margin = new Padding(4, 0, 4, 0);
            lblClienteLabel.Name = "lblClienteLabel";
            lblClienteLabel.Size = new Size(114, 33);
            lblClienteLabel.TabIndex = 0;
            lblClienteLabel.Text = "Cliente:";
            // 
            // lblClienteVal
            // 
            lblClienteVal.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblClienteVal.ForeColor = Color.FromArgb(74, 222, 128);
            lblClienteVal.Location = new Point(136, 10);
            lblClienteVal.Margin = new Padding(4, 0, 4, 0);
            lblClienteVal.Name = "lblClienteVal";
            lblClienteVal.Size = new Size(371, 33);
            lblClienteVal.TabIndex = 1;
            // 
            // lblSubtotalLabel2
            // 
            lblSubtotalLabel2.Font = new Font("Segoe UI", 10F);
            lblSubtotalLabel2.ForeColor = Color.FromArgb(148, 163, 184);
            lblSubtotalLabel2.Location = new Point(14, 47);
            lblSubtotalLabel2.Margin = new Padding(4, 0, 4, 0);
            lblSubtotalLabel2.Name = "lblSubtotalLabel2";
            lblSubtotalLabel2.Size = new Size(257, 33);
            lblSubtotalLabel2.TabIndex = 2;
            lblSubtotalLabel2.Text = "Subtotal:";
            // 
            // lblSubtotalVal
            // 
            lblSubtotalVal.Font = new Font("Segoe UI", 10F);
            lblSubtotalVal.ForeColor = Color.White;
            lblSubtotalVal.Location = new Point(286, 47);
            lblSubtotalVal.Margin = new Padding(4, 0, 4, 0);
            lblSubtotalVal.Name = "lblSubtotalVal";
            lblSubtotalVal.Size = new Size(229, 33);
            lblSubtotalVal.TabIndex = 3;
            lblSubtotalVal.TextAlign = ContentAlignment.TopRight;
            // 
            // lblITBISLabel2
            // 
            lblITBISLabel2.Font = new Font("Segoe UI", 10F);
            lblITBISLabel2.ForeColor = Color.FromArgb(148, 163, 184);
            lblITBISLabel2.Location = new Point(14, 83);
            lblITBISLabel2.Margin = new Padding(4, 0, 4, 0);
            lblITBISLabel2.Name = "lblITBISLabel2";
            lblITBISLabel2.Size = new Size(257, 33);
            lblITBISLabel2.TabIndex = 4;
            lblITBISLabel2.Text = "ITBIS (18%):";
            // 
            // lblITBISVal
            // 
            lblITBISVal.Font = new Font("Segoe UI", 10F);
            lblITBISVal.ForeColor = Color.FromArgb(251, 146, 60);
            lblITBISVal.Location = new Point(286, 83);
            lblITBISVal.Margin = new Padding(4, 0, 4, 0);
            lblITBISVal.Name = "lblITBISVal";
            lblITBISVal.Size = new Size(229, 33);
            lblITBISVal.TabIndex = 5;
            lblITBISVal.TextAlign = ContentAlignment.TopRight;
            // 
            // lblTotalLabel2
            // 
            lblTotalLabel2.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTotalLabel2.ForeColor = Color.White;
            lblTotalLabel2.Location = new Point(14, 120);
            lblTotalLabel2.Margin = new Padding(4, 0, 4, 0);
            lblTotalLabel2.Name = "lblTotalLabel2";
            lblTotalLabel2.Size = new Size(257, 40);
            lblTotalLabel2.TabIndex = 6;
            lblTotalLabel2.Text = "TOTAL A COBRAR:";
            // 
            // lblTotalVal
            // 
            lblTotalVal.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTotalVal.ForeColor = Color.FromArgb(74, 222, 128);
            lblTotalVal.Location = new Point(243, 120);
            lblTotalVal.Margin = new Padding(4, 0, 4, 0);
            lblTotalVal.Name = "lblTotalVal";
            lblTotalVal.Size = new Size(271, 40);
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
            lblDetalleTitle.Margin = new Padding(4, 0, 4, 0);
            lblDetalleTitle.Name = "lblDetalleTitle";
            lblDetalleTitle.Size = new Size(1285, 53);
            lblDetalleTitle.TabIndex = 2;
            lblDetalleTitle.Text = "  Detalle de la venta";
            lblDetalleTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panelPagoContent
            // 
            panelPagoContent.Controls.Add(btnEfectivo);
            panelPagoContent.Controls.Add(btnTarjeta);
            panelPagoContent.Controls.Add(btnTransferencia);
            panelPagoContent.Controls.Add(btnCredito);
            panelPagoContent.Controls.Add(lblInfoMetodo);
            panelPagoContent.Controls.Add(lblMontoPagadoLabel);
            panelPagoContent.Controls.Add(txtMontoPagado);
            panelPagoContent.Controls.Add(lblRDSign);
            panelPagoContent.Controls.Add(panelCambio);
            panelPagoContent.Controls.Add(btnConfirmar);
            panelPagoContent.Controls.Add(btnCancelar);
            panelPagoContent.Location = new Point(0, 53);
            panelPagoContent.Margin = new Padding(4, 5, 4, 5);
            panelPagoContent.Name = "panelPagoContent";
            panelPagoContent.Size = new Size(286, 114);
            panelPagoContent.TabIndex = 0;
            // 
            // lblMetodoPagoTitle
            // 
            lblMetodoPagoTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblMetodoPagoTitle.ForeColor = Color.FromArgb(30, 41, 59);
            lblMetodoPagoTitle.Location = new Point(4, 227);
            lblMetodoPagoTitle.Margin = new Padding(4, 0, 4, 0);
            lblMetodoPagoTitle.Name = "lblMetodoPagoTitle";
            lblMetodoPagoTitle.Size = new Size(299, 78);
            lblMetodoPagoTitle.TabIndex = 0;
            lblMetodoPagoTitle.Text = "Selecciona el método de pago:";
            lblMetodoPagoTitle.Click += lblMetodoPagoTitle_Click;
            // 
            // btnEfectivo
            // 
            btnEfectivo.Location = new Point(4, 48);
            btnEfectivo.Margin = new Padding(4, 5, 4, 5);
            btnEfectivo.Name = "btnEfectivo";
            btnEfectivo.Size = new Size(107, 38);
            btnEfectivo.TabIndex = 1;
            btnEfectivo.Click += btnEfectivo_Click;
            // 
            // btnTarjeta
            // 
            btnTarjeta.Location = new Point(119, 56);
            btnTarjeta.Margin = new Padding(4, 5, 4, 5);
            btnTarjeta.Name = "btnTarjeta";
            btnTarjeta.Size = new Size(107, 38);
            btnTarjeta.TabIndex = 2;
            btnTarjeta.Click += btnTarjeta_Click;
            // 
            // btnTransferencia
            // 
            btnTransferencia.Location = new Point(138, 8);
            btnTransferencia.Margin = new Padding(4, 5, 4, 5);
            btnTransferencia.Name = "btnTransferencia";
            btnTransferencia.Size = new Size(107, 38);
            btnTransferencia.TabIndex = 3;
            btnTransferencia.Click += btnTransferencia_Click;
            // 
            // btnCredito
            // 
            btnCredito.Location = new Point(4, 0);
            btnCredito.Margin = new Padding(4, 5, 4, 5);
            btnCredito.Name = "btnCredito";
            btnCredito.Size = new Size(107, 38);
            btnCredito.TabIndex = 4;
            btnCredito.Click += btnCredito_Click;
            // 
            // lblInfoMetodo
            // 
            lblInfoMetodo.Font = new Font("Segoe UI", 9F);
            lblInfoMetodo.ForeColor = Color.FromArgb(74, 222, 128);
            lblInfoMetodo.Location = new Point(29, 333);
            lblInfoMetodo.Margin = new Padding(4, 0, 4, 0);
            lblInfoMetodo.Name = "lblInfoMetodo";
            lblInfoMetodo.Size = new Size(514, 70);
            lblInfoMetodo.TabIndex = 5;
            // 
            // lblMontoPagadoLabel
            // 
            lblMontoPagadoLabel.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblMontoPagadoLabel.ForeColor = Color.FromArgb(30, 41, 59);
            lblMontoPagadoLabel.Location = new Point(29, 425);
            lblMontoPagadoLabel.Margin = new Padding(4, 0, 4, 0);
            lblMontoPagadoLabel.Name = "lblMontoPagadoLabel";
            lblMontoPagadoLabel.Size = new Size(514, 37);
            lblMontoPagadoLabel.TabIndex = 6;
            lblMontoPagadoLabel.Text = "Monto recibido (efectivo):";
            // 
            // txtMontoPagado
            // 
            txtMontoPagado.BorderStyle = BorderStyle.FixedSingle;
            txtMontoPagado.Font = new Font("Segoe UI", 16F);
            txtMontoPagado.Location = new Point(29, 467);
            txtMontoPagado.Margin = new Padding(4, 5, 4, 5);
            txtMontoPagado.Name = "txtMontoPagado";
            txtMontoPagado.Size = new Size(342, 50);
            txtMontoPagado.TabIndex = 7;
            txtMontoPagado.TextAlign = HorizontalAlignment.Right;
            txtMontoPagado.TextChanged += txtMontoPagado_TextChanged;
            // 
            // lblRDSign
            // 
            lblRDSign.Location = new Point(0, 0);
            lblRDSign.Margin = new Padding(4, 0, 4, 0);
            lblRDSign.Name = "lblRDSign";
            lblRDSign.Size = new Size(143, 38);
            lblRDSign.TabIndex = 8;
            // 
            // panelCambio
            // 
            panelCambio.BackColor = Color.FromArgb(220, 252, 231);
            panelCambio.Controls.Add(lblCambioLabel);
            panelCambio.Controls.Add(lblCambioVal);
            panelCambio.Location = new Point(29, 567);
            panelCambio.Margin = new Padding(4, 5, 4, 5);
            panelCambio.Name = "panelCambio";
            panelCambio.Size = new Size(514, 67);
            panelCambio.TabIndex = 9;
            // 
            // lblCambioLabel
            // 
            lblCambioLabel.AutoSize = true;
            lblCambioLabel.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblCambioLabel.ForeColor = Color.FromArgb(21, 128, 61);
            lblCambioLabel.Location = new Point(14, 17);
            lblCambioLabel.Margin = new Padding(4, 0, 4, 0);
            lblCambioLabel.Name = "lblCambioLabel";
            lblCambioLabel.Size = new Size(213, 30);
            lblCambioLabel.TabIndex = 0;
            lblCambioLabel.Text = "Cambio a entregar:";
            // 
            // lblCambioVal
            // 
            lblCambioVal.AutoSize = true;
            lblCambioVal.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblCambioVal.ForeColor = Color.FromArgb(22, 163, 74);
            lblCambioVal.Location = new Point(286, 17);
            lblCambioVal.Margin = new Padding(4, 0, 4, 0);
            lblCambioVal.Name = "lblCambioVal";
            lblCambioVal.Size = new Size(107, 30);
            lblCambioVal.TabIndex = 1;
            lblCambioVal.Text = "RD$ 0.00";
            // 
            // btnConfirmar
            // 
            btnConfirmar.BackColor = Color.FromArgb(22, 163, 74);
            btnConfirmar.Cursor = Cursors.Hand;
            btnConfirmar.FlatAppearance.BorderSize = 0;
            btnConfirmar.FlatStyle = FlatStyle.Flat;
            btnConfirmar.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btnConfirmar.ForeColor = Color.White;
            btnConfirmar.Location = new Point(29, 675);
            btnConfirmar.Margin = new Padding(4, 5, 4, 5);
            btnConfirmar.Name = "btnConfirmar";
            btnConfirmar.Size = new Size(371, 87);
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
            btnCancelar.Location = new Point(414, 675);
            btnCancelar.Margin = new Padding(4, 5, 4, 5);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(143, 87);
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
            lblPagoTitle.Margin = new Padding(4, 0, 4, 0);
            lblPagoTitle.Name = "lblPagoTitle";
            lblPagoTitle.Size = new Size(303, 53);
            lblPagoTitle.TabIndex = 1;
            lblPagoTitle.Text = "  Método de Pago";
            lblPagoTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // frmPago
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1594, 1067);
            Controls.Add(splitPago);
            Controls.Add(panelHeader);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
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
