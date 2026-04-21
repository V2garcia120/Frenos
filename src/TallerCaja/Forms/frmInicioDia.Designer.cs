namespace TallerCaja.Forms
{
    partial class frmInicioDia
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
            lblCajero = new Label();
            lblFecha = new Label();
            panelBody = new Panel();
            lblDescripcion = new Label();
            lblMontoLabel = new Label();
            lblRD = new Label();
            txtMontoInicial = new TextBox();
            btnAbrir = new Button();
            btnCancelar = new Button();
            panelHeader.SuspendLayout();
            panelBody.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(15, 23, 42);
            panelHeader.Controls.Add(lblTitulo);
            panelHeader.Controls.Add(lblCajero);
            panelHeader.Controls.Add(lblFecha);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Margin = new Padding(4, 5, 4, 5);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(714, 150);
            panelHeader.TabIndex = 1;
            // 
            // lblTitulo
            // 
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(29, 17);
            lblTitulo.Margin = new Padding(4, 0, 4, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(629, 60);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "APERTURA DE TURNO";
            // 
            // lblCajero
            // 
            lblCajero.AutoSize = true;
            lblCajero.Font = new Font("Segoe UI", 10F);
            lblCajero.ForeColor = Color.FromArgb(148, 163, 184);
            lblCajero.Location = new Point(29, 87);
            lblCajero.Margin = new Padding(4, 0, 4, 0);
            lblCajero.Name = "lblCajero";
            lblCajero.Size = new Size(0, 28);
            lblCajero.TabIndex = 1;
            // 
            // lblFecha
            // 
            lblFecha.AutoSize = true;
            lblFecha.Font = new Font("Segoe UI", 10F);
            lblFecha.ForeColor = Color.FromArgb(148, 163, 184);
            lblFecha.Location = new Point(357, 87);
            lblFecha.Margin = new Padding(4, 0, 4, 0);
            lblFecha.Name = "lblFecha";
            lblFecha.Size = new Size(0, 28);
            lblFecha.TabIndex = 2;
            // 
            // panelBody
            // 
            panelBody.BackColor = Color.FromArgb(248, 250, 252);
            panelBody.Controls.Add(lblDescripcion);
            panelBody.Controls.Add(lblMontoLabel);
            panelBody.Controls.Add(lblRD);
            panelBody.Controls.Add(txtMontoInicial);
            panelBody.Controls.Add(btnAbrir);
            panelBody.Controls.Add(btnCancelar);
            panelBody.Dock = DockStyle.Fill;
            panelBody.Location = new Point(0, 150);
            panelBody.Margin = new Padding(4, 5, 4, 5);
            panelBody.Name = "panelBody";
            panelBody.Padding = new Padding(43, 50, 43, 50);
            panelBody.Size = new Size(714, 467);
            panelBody.TabIndex = 0;
            // 
            // lblDescripcion
            // 
            lblDescripcion.Font = new Font("Segoe UI", 10F);
            lblDescripcion.ForeColor = Color.FromArgb(71, 85, 105);
            lblDescripcion.Location = new Point(43, 33);
            lblDescripcion.Margin = new Padding(4, 0, 4, 0);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(629, 83);
            lblDescripcion.TabIndex = 0;
            lblDescripcion.Text = "Registra el efectivo inicial disponible en caja para comenzar el turno de trabajo.";
            // 
            // lblMontoLabel
            // 
            lblMontoLabel.AutoSize = true;
            lblMontoLabel.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblMontoLabel.ForeColor = Color.FromArgb(30, 41, 59);
            lblMontoLabel.Location = new Point(43, 150);
            lblMontoLabel.Margin = new Padding(4, 0, 4, 0);
            lblMontoLabel.Name = "lblMontoLabel";
            lblMontoLabel.Size = new Size(268, 30);
            lblMontoLabel.TabIndex = 1;
            lblMontoLabel.Text = "Monto inicial en efectivo";
            // 
            // lblRD
            // 
            lblRD.AutoSize = true;
            lblRD.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblRD.ForeColor = Color.FromArgb(71, 85, 105);
            lblRD.Location = new Point(43, 203);
            lblRD.Margin = new Padding(4, 0, 4, 0);
            lblRD.Name = "lblRD";
            lblRD.Size = new Size(72, 38);
            lblRD.TabIndex = 2;
            lblRD.Text = "RD$";
            // 
            // txtMontoInicial
            // 
            txtMontoInicial.BorderStyle = BorderStyle.FixedSingle;
            txtMontoInicial.Font = new Font("Segoe UI", 14F);
            txtMontoInicial.Location = new Point(114, 201);
            txtMontoInicial.Margin = new Padding(4, 5, 4, 5);
            txtMontoInicial.Name = "txtMontoInicial";
            txtMontoInicial.Size = new Size(285, 45);
            txtMontoInicial.TabIndex = 3;
            txtMontoInicial.Text = "5000.00";
            txtMontoInicial.TextAlign = HorizontalAlignment.Right;
            // 
            // btnAbrir
            // 
            btnAbrir.BackColor = Color.FromArgb(22, 163, 74);
            btnAbrir.Cursor = Cursors.Hand;
            btnAbrir.FlatAppearance.BorderSize = 0;
            btnAbrir.FlatStyle = FlatStyle.Flat;
            btnAbrir.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnAbrir.ForeColor = Color.White;
            btnAbrir.Location = new Point(43, 308);
            btnAbrir.Margin = new Padding(4, 5, 4, 5);
            btnAbrir.Name = "btnAbrir";
            btnAbrir.Size = new Size(286, 80);
            btnAbrir.TabIndex = 4;
            btnAbrir.Text = "Abrir Turno";
            btnAbrir.UseVisualStyleBackColor = false;
            btnAbrir.Click += btnAbrir_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.FromArgb(239, 68, 68);
            btnCancelar.Cursor = Cursors.Hand;
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 11F);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(350, 308);
            btnCancelar.Margin = new Padding(4, 5, 4, 5);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(171, 80);
            btnCancelar.TabIndex = 5;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // frmInicioDia
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(714, 617);
            Controls.Add(panelBody);
            Controls.Add(panelHeader);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmInicioDia";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Apertura de Turno";
            Load += frmInicioDia_Load;
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelBody.ResumeLayout(false);
            panelBody.PerformLayout();
            ResumeLayout(false);
        }

        private Panel panelHeader;
        private Label lblTitulo, lblCajero, lblFecha;
        private Panel panelBody;
        private Label lblDescripcion, lblMontoLabel, lblRD;
        private TextBox txtMontoInicial;
        private Button btnAbrir, btnCancelar;
    }
}
