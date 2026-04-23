namespace TallerCaja.Forms
{
    partial class frmLogin
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelFondo = new Panel();
            panelCard = new Panel();
            lblTitulo = new Label();
            lblSubtitulo = new Label();
            lblEmailLabel = new Label();
            txtEmail = new TextBox();
            lblPassLabel = new Label();
            txtPassword = new TextBox();
            btnLogin = new Button();
            lblEstado = new Label();
            lblVersion = new Label();
            panelFondo.SuspendLayout();
            panelCard.SuspendLayout();
            SuspendLayout();
            // 
            // panelFondo
            // 
            panelFondo.BackColor = Color.FromArgb(15, 23, 42);
            panelFondo.Controls.Add(panelCard);
            panelFondo.Dock = DockStyle.Fill;
            panelFondo.Location = new Point(0, 0);
            panelFondo.Margin = new Padding(4, 5, 4, 5);
            panelFondo.Name = "panelFondo";
            panelFondo.Size = new Size(1143, 1067);
            panelFondo.TabIndex = 0;
            // 
            // panelCard
            // 
            panelCard.BackColor = Color.FromArgb(30, 41, 59);
            panelCard.Controls.Add(lblTitulo);
            panelCard.Controls.Add(lblSubtitulo);
            panelCard.Controls.Add(lblEmailLabel);
            panelCard.Controls.Add(txtEmail);
            panelCard.Controls.Add(lblPassLabel);
            panelCard.Controls.Add(txtPassword);
            panelCard.Controls.Add(btnLogin);
            panelCard.Controls.Add(lblEstado);
            panelCard.Controls.Add(lblVersion);
            panelCard.Location = new Point(271, 100);
            panelCard.Margin = new Padding(4, 5, 4, 5);
            panelCard.Name = "panelCard";
            panelCard.Size = new Size(600, 867);
            panelCard.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(29, 148);
            lblTitulo.Margin = new Padding(4, 0, 4, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(543, 60);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "TALLER DE FRENOS";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.Font = new Font("Segoe UI", 11F);
            lblSubtitulo.ForeColor = Color.FromArgb(148, 163, 184);
            lblSubtitulo.Location = new Point(29, 212);
            lblSubtitulo.Margin = new Padding(4, 0, 4, 0);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(543, 40);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Sistema de Caja";
            lblSubtitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblEmailLabel
            // 
            lblEmailLabel.AutoSize = true;
            lblEmailLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblEmailLabel.ForeColor = Color.FromArgb(148, 163, 184);
            lblEmailLabel.Location = new Point(43, 333);
            lblEmailLabel.Margin = new Padding(4, 0, 4, 0);
            lblEmailLabel.Name = "lblEmailLabel";
            lblEmailLabel.Size = new Size(169, 25);
            lblEmailLabel.TabIndex = 2;
            lblEmailLabel.Text = "Correo electrónico";
            // 
            // txtEmail
            // 
            txtEmail.BackColor = Color.FromArgb(51, 65, 85);
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.Font = new Font("Segoe UI", 11F);
            txtEmail.ForeColor = Color.White;
            txtEmail.Location = new Point(43, 367);
            txtEmail.Margin = new Padding(4, 5, 4, 5);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(513, 37);
            txtEmail.TabIndex = 3;
            txtEmail.Text = "cajero@taller.com";
            // 
            // lblPassLabel
            // 
            lblPassLabel.AutoSize = true;
            lblPassLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPassLabel.ForeColor = Color.FromArgb(148, 163, 184);
            lblPassLabel.Location = new Point(43, 458);
            lblPassLabel.Margin = new Padding(4, 0, 4, 0);
            lblPassLabel.Name = "lblPassLabel";
            lblPassLabel.Size = new Size(108, 25);
            lblPassLabel.TabIndex = 4;
            lblPassLabel.Text = "Contraseña";
            // 
            // txtPassword
            // 
            txtPassword.BackColor = Color.FromArgb(51, 65, 85);
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Font = new Font("Segoe UI", 11F);
            txtPassword.ForeColor = Color.White;
            txtPassword.Location = new Point(43, 492);
            txtPassword.Margin = new Padding(4, 5, 4, 5);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '●';
            txtPassword.Size = new Size(513, 37);
            txtPassword.TabIndex = 5;
            txtPassword.Text = "demo123";
            txtPassword.KeyDown += txtPassword_KeyDown;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.FromArgb(37, 99, 235);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(43, 600);
            btnLogin.Margin = new Padding(4, 5, 4, 5);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(514, 80);
            btnLogin.TabIndex = 6;
            btnLogin.Text = "Ingresar";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // lblEstado
            // 
            lblEstado.Font = new Font("Segoe UI", 9F);
            lblEstado.ForeColor = Color.DarkOrange;
            lblEstado.Location = new Point(29, 708);
            lblEstado.Margin = new Padding(4, 0, 4, 0);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(543, 40);
            lblEstado.TabIndex = 7;
            lblEstado.Text = "● Verificando conexión...";
            lblEstado.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblVersion
            // 
            lblVersion.Font = new Font("Segoe UI", 8F);
            lblVersion.ForeColor = Color.FromArgb(100, 116, 139);
            lblVersion.Location = new Point(29, 767);
            lblVersion.Margin = new Padding(4, 0, 4, 0);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(543, 33);
            lblVersion.TabIndex = 8;
            lblVersion.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // frmLogin
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1143, 1067);
            Controls.Add(panelFondo);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            Name = "frmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Taller de Frenos - Caja";
            Load += frmLogin_Load;
            panelFondo.ResumeLayout(false);
            panelCard.ResumeLayout(false);
            panelCard.PerformLayout();
            ResumeLayout(false);
        }

        private Panel panelFondo;
        private Panel panelCard;
        private Label lblTitulo;
        private Label lblSubtitulo;
        private Label lblEmailLabel;
        private TextBox txtEmail;
        private Label lblPassLabel;
        private TextBox txtPassword;
        private Button btnLogin;
        private Label lblEstado;
        private Label lblVersion;
    }
}
