namespace TallerCaja.Forms
{
    public partial class frmCantidad : Form
    {
        private Label lblTitulo = null!;
        private NumericUpDown nudCantidad = null!;
        private Button btnAceptar = null!;
        private Button btnCancelar = null!;

        public int NuevaCantidad { get; private set; }

        public frmCantidad()
        {
            NuevaCantidad = 1;
            InitializeComponent();
        }

        public frmCantidad(string descripcion, int cantidadActual)
        {
            NuevaCantidad = cantidadActual;
            InitializeComponent();
            lblTitulo.Text = $"Cantidad para: {descripcion}";
            nudCantidad.Value = Math.Max(0, cantidadActual);
        }

        private void InitializeComponent()
        {
            lblTitulo = new Label();
            nudCantidad = new NumericUpDown();
            btnAceptar = new Button();
            btnCancelar = new Button();
            ((System.ComponentModel.ISupportInitialize)nudCantidad).BeginInit();
            SuspendLayout();

            lblTitulo.AutoSize = false;
            lblTitulo.Location = new Point(12, 12);
            lblTitulo.Size = new Size(420, 40);
            lblTitulo.Text = "Cantidad";

            nudCantidad.Location = new Point(12, 60);
            nudCantidad.Maximum = 10000;
            nudCantidad.Size = new Size(420, 31);

            btnAceptar.Location = new Point(216, 108);
            btnAceptar.Size = new Size(104, 36);
            btnAceptar.Text = "Aceptar";
            btnAceptar.Click += btnAceptar_Click;

            btnCancelar.Location = new Point(328, 108);
            btnCancelar.Size = new Size(104, 36);
            btnCancelar.Text = "Cancelar";
            btnCancelar.Click += btnCancelar_Click;

            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(446, 160);
            Controls.Add(btnCancelar);
            Controls.Add(btnAceptar);
            Controls.Add(nudCantidad);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmCantidad";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Editar cantidad";
            ((System.ComponentModel.ISupportInitialize)nudCantidad).EndInit();
            ResumeLayout(false);
        }

        private void btnAceptar_Click(object? sender, EventArgs e)
        {
            NuevaCantidad = (int)nudCantidad.Value;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancelar_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
