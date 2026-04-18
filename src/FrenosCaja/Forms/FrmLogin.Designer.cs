namespace FrenosCaja.Forms
{
    partial class FrmLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            txtEmail = new TextBox();
            txtPassword = new TextBox();
            panel1 = new Panel();
            btnConectar = new Button();
            label3 = new Label();
            label2 = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ActiveCaption;
            label1.Location = new Point(172, 16);
            label1.Name = "label1";
            label1.Size = new Size(140, 58);
            label1.TabIndex = 0;
            label1.Text = "Login";
            label1.Click += label1_Click;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(53, 146);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(395, 31);
            txtEmail.TabIndex = 1;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(53, 248);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(395, 31);
            txtPassword.TabIndex = 2;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ButtonFace;
            panel1.Controls.Add(btnConectar);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(txtEmail);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(txtPassword);
            panel1.Location = new Point(163, 43);
            panel1.Name = "panel1";
            panel1.Size = new Size(511, 385);
            panel1.TabIndex = 3;
            // 
            // btnConectar
            // 
            btnConectar.BackColor = Color.LightBlue;
            btnConectar.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnConectar.Location = new Point(172, 323);
            btnConectar.Name = "btnConectar";
            btnConectar.Size = new Size(140, 34);
            btnConectar.TabIndex = 5;
            btnConectar.Text = "Conectar";
            btnConectar.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(53, 211);
            label3.Name = "label3";
            label3.Size = new Size(84, 22);
            label3.TabIndex = 4;
            label3.Text = "Password";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(57, 108);
            label2.Name = "label2";
            label2.Size = new Size(52, 22);
            label2.TabIndex = 3;
            label2.Text = "Email";
            // 
            // FrmLogin
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveBorder;
            ClientSize = new Size(872, 510);
            Controls.Add(panel1);
            Name = "FrmLogin";
            Text = "FrmLogin";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private TextBox txtEmail;
        private TextBox txtPassword;
        private Panel panel1;
        private Button btnConectar;
        private Label label3;
        private Label label2;
    }
}