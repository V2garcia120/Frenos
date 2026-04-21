using TallerCaja.Models.DTOs;

namespace TallerCaja.Forms
{
    public partial class frmSelectorCliente : Form
    {
        private readonly List<ClienteDto> _clientes;

        private Label lblTitulo = null!;
        private ListView lvClientes = null!;
        private ColumnHeader colNombre = null!;
        private ColumnHeader colCedula = null!;
        private ColumnHeader colTelefono = null!;
        private Button btnSeleccionar = null!;
        private Button btnCancelar = null!;

        public ClienteDto? ClienteSeleccionado { get; private set; }

        public frmSelectorCliente()
        {
            _clientes = new List<ClienteDto>();
            InitializeComponent();
        }

        public frmSelectorCliente(List<ClienteDto> clientes)
        {
            _clientes = clientes ?? new List<ClienteDto>();
            InitializeComponent();
            CargarClientes();
        }

        private void InitializeComponent()
        {
            lblTitulo = new Label();
            lvClientes = new ListView();
            colNombre = new ColumnHeader();
            colCedula = new ColumnHeader();
            colTelefono = new ColumnHeader();
            btnSeleccionar = new Button();
            btnCancelar = new Button();
            SuspendLayout();

            lblTitulo.Location = new Point(12, 9);
            lblTitulo.Size = new Size(760, 30);
            lblTitulo.Text = "Selecciona un cliente";

            lvClientes.Columns.AddRange(new[] { colNombre, colCedula, colTelefono });
            lvClientes.FullRowSelect = true;
            lvClientes.GridLines = true;
            lvClientes.HideSelection = false;
            lvClientes.Location = new Point(12, 42);
            lvClientes.MultiSelect = false;
            lvClientes.Size = new Size(760, 340);
            lvClientes.View = View.Details;
            lvClientes.DoubleClick += lvClientes_DoubleClick;

            colNombre.Text = "Nombre";
            colNombre.Width = 300;
            colCedula.Text = "Cédula";
            colCedula.Width = 180;
            colTelefono.Text = "Teléfono";
            colTelefono.Width = 160;

            btnSeleccionar.Location = new Point(556, 392);
            btnSeleccionar.Size = new Size(104, 36);
            btnSeleccionar.Text = "Aceptar";
            btnSeleccionar.Click += btnSeleccionar_Click;

            btnCancelar.Location = new Point(668, 392);
            btnCancelar.Size = new Size(104, 36);
            btnCancelar.Text = "Cancelar";
            btnCancelar.Click += btnCancelar_Click;

            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 441);
            Controls.Add(btnCancelar);
            Controls.Add(btnSeleccionar);
            Controls.Add(lvClientes);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmSelectorCliente";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Seleccionar cliente";
            ResumeLayout(false);
        }

        private void CargarClientes()
        {
            lvClientes.Items.Clear();
            foreach (var cliente in _clientes)
            {
                var item = new ListViewItem(new[]
                {
                    cliente.Nombre,
                    cliente.Cedula,
                    cliente.Telefono
                })
                {
                    Tag = cliente
                };
                lvClientes.Items.Add(item);
            }
        }

        private void btnSeleccionar_Click(object? sender, EventArgs e)
        {
            if (lvClientes.SelectedItems.Count == 0)
            {
                MessageBox.Show("Selecciona un cliente para continuar.", "Cliente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ClienteSeleccionado = lvClientes.SelectedItems[0].Tag as ClienteDto;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void lvClientes_DoubleClick(object? sender, EventArgs e)
            => btnSeleccionar_Click(sender, e);

        private void btnCancelar_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
