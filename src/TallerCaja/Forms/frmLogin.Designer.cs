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
            picLogo = new PictureBox();
            panelFondo.SuspendLayout();
            panelCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            SuspendLayout();

            // panelFondo
            panelFondo.BackColor = Color.FromArgb(15, 23, 42);
            panelFondo.Dock = DockStyle.Fill;
            panelFondo.Controls.Add(panelCard);

            // panelCard
            panelCard.BackColor = Color.FromArgb(30, 41, 59);
            panelCard.BorderStyle = BorderStyle.None;
            panelCard.Size = new Size(420, 520);
            panelCard.Location = new Point(190, 60);
            panelCard.Controls.AddRange(new Control[] {
                lblTitulo, lblSubtitulo, lblEmailLabel, txtEmail,
                lblPassLabel, txtPassword, btnLogin, lblEstado, lblVersion, picLogo
            });

            // picLogo (ícono visual)
            picLogo.Size = new Size(64, 64);
            picLogo.Location = new Point(178, 30);
            picLogo.BackColor = Color.FromArgb(59, 130, 246);
            picLogo.SizeMode = PictureBoxSizeMode.CenterImage;

            // lblTitulo
            lblTitulo.AutoSize = false;
            lblTitulo.Size = new Size(380, 36);
            lblTitulo.Location = new Point(20, 110);
            lblTitulo.Text = "TALLER DE FRENOS";
            lblTitulo.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;

            // lblSubtitulo
            lblSubtitulo.AutoSize = false;
            lblSubtitulo.Size = new Size(380, 24);
            lblSubtitulo.Location = new Point(20, 148);
            lblSubtitulo.Text = "Sistema de Caja";
            lblSubtitulo.Font = new Font("Segoe UI", 11F);
            lblSubtitulo.ForeColor = Color.FromArgb(148, 163, 184);
            lblSubtitulo.TextAlign = ContentAlignment.MiddleCenter;

            // lblEmailLabel
            lblEmailLabel.AutoSize = true;
            lblEmailLabel.Location = new Point(30, 200);
            lblEmailLabel.Text = "Correo electrónico";
            lblEmailLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblEmailLabel.ForeColor = Color.FromArgb(148, 163, 184);

            // txtEmail
            txtEmail.Location = new Point(30, 220);
            txtEmail.Size = new Size(360, 36);
            txtEmail.Font = new Font("Segoe UI", 11F);
            txtEmail.BackColor = Color.FromArgb(51, 65, 85);
            txtEmail.ForeColor = Color.White;
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.Text = "cajero@taller.com";

            // lblPassLabel
            lblPassLabel.AutoSize = true;
            lblPassLabel.Location = new Point(30, 275);
            lblPassLabel.Text = "Contraseña";
            lblPassLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPassLabel.ForeColor = Color.FromArgb(148, 163, 184);

            // txtPassword
            txtPassword.Location = new Point(30, 295);
            txtPassword.Size = new Size(360, 36);
            txtPassword.Font = new Font("Segoe UI", 11F);
            txtPassword.PasswordChar = '●';
            txtPassword.BackColor = Color.FromArgb(51, 65, 85);
            txtPassword.ForeColor = Color.White;
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.KeyDown += txtPassword_KeyDown;
            txtPassword.Text = "demo123";

            // btnLogin
            btnLogin.Location = new Point(30, 360);
            btnLogin.Size = new Size(360, 48);
            btnLogin.Text = "Ingresar";
            btnLogin.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnLogin.BackColor = Color.FromArgb(37, 99, 235);
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.Click += btnLogin_Click;

            // lblEstado
            lblEstado.AutoSize = false;
            lblEstado.Size = new Size(380, 24);
            lblEstado.Location = new Point(20, 425);
            lblEstado.Text = "● Verificando conexión...";
            lblEstado.Font = new Font("Segoe UI", 9F);
            lblEstado.ForeColor = Color.DarkOrange;
            lblEstado.TextAlign = ContentAlignment.MiddleCenter;

            // lblVersion
            lblVersion.AutoSize = false;
            lblVersion.Size = new Size(380, 20);
            lblVersion.Location = new Point(20, 460);
            lblVersion.Font = new Font("Segoe UI", 8F);
            lblVersion.ForeColor = Color.FromArgb(100, 116, 139);
            lblVersion.TextAlign = ContentAlignment.MiddleCenter;

            // frmLogin
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 640);
            Controls.Add(panelFondo);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "frmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Taller de Frenos - Caja";
            Load += frmLogin_Load;

            panelFondo.ResumeLayout(false);
            panelCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
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
        private PictureBox picLogo;
    }
}
