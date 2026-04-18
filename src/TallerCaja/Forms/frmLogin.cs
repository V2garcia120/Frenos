using TallerCaja.Helpers;
using TallerCaja.Services;

namespace TallerCaja.Forms
{
    public partial class frmLogin : Form
    {
        private readonly IIntegracionService _integracion;
        private readonly ConexionMonitor _monitor;

        public frmLogin(IIntegracionService integracion, ConexionMonitor monitor)
        {
            _integracion = integracion;
            _monitor = monitor;
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            lblVersion.Text = $"v{AppConfig.NombreTaller} - Módulo Caja v1.0";
            txtEmail.Focus();
            _ = VerificarConexionAsync();
        }

        private async Task VerificarConexionAsync()
        {
            lblEstado.Text = "Verificando conexión...";
            lblEstado.ForeColor = Color.DarkOrange;
            var online = await _monitor.VerificarConexionAsync();
            if (online)
            {
                lblEstado.Text = $"● Sistema online{(ConexionMonitor.ModoCache ? " (modo caché)" : "")}";
                lblEstado.ForeColor = Color.Green;
            }
            else
            {
                lblEstado.Text = "● Modo OFFLINE - trabajando con datos locales";
                lblEstado.ForeColor = Color.Crimson;
            }
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Ingresa correo y contraseña.", "Campos requeridos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnLogin.Enabled = false;
            btnLogin.Text = "Autenticando...";

            try
            {
                var resp = await _integracion.LoginCajeroAsync(txtEmail.Text.Trim(), txtPassword.Text);
                if (resp != null)
                {
                    SessionManager.IniciarSesion(resp);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Credenciales incorrectas. Verifica tu correo y contraseña.",
                        "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar con el sistema:\n{ex.Message}",
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLogin.Enabled = true;
                btnLogin.Text = "Ingresar";
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) btnLogin_Click(sender, e);
        }
    }
}
