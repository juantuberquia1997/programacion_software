namespace frmParcial1
{
    partial class frmParcial1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.Label lblNroDoc;
        private System.Windows.Forms.TextBox txtNroDoc;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblSede;
        private System.Windows.Forms.TextBox txtSede;
        private System.Windows.Forms.Label lblPrograma;
        private System.Windows.Forms.TextBox txtPrograma;
        private System.Windows.Forms.Label lblN2;
        private System.Windows.Forms.TextBox txtN2;
        private System.Windows.Forms.Button btnCalcular;
        private System.Windows.Forms.Label lblTituloNotas;
        private System.Windows.Forms.Label lblT1;
        private System.Windows.Forms.Label lblResN1;
        private System.Windows.Forms.Label lblT3;
        private System.Windows.Forms.Label lblResN3;
        private System.Windows.Forms.Label lblT4;
        private System.Windows.Forms.Label lblResN4;
        private System.Windows.Forms.Label lblError;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblCodigo = new System.Windows.Forms.Label();
            this.txtCodigo = new System.Windows.Forms.TextBox();
            this.lblNroDoc = new System.Windows.Forms.Label();
            this.txtNroDoc = new System.Windows.Forms.TextBox();
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblSede = new System.Windows.Forms.Label();
            this.txtSede = new System.Windows.Forms.TextBox();
            this.lblPrograma = new System.Windows.Forms.Label();
            this.txtPrograma = new System.Windows.Forms.TextBox();
            this.lblN2 = new System.Windows.Forms.Label();
            this.txtN2 = new System.Windows.Forms.TextBox();
            this.btnCalcular = new System.Windows.Forms.Button();
            this.lblTituloNotas = new System.Windows.Forms.Label();
            this.lblT1 = new System.Windows.Forms.Label();
            this.lblResN1 = new System.Windows.Forms.Label();
            this.lblT3 = new System.Windows.Forms.Label();
            this.lblResN3 = new System.Windows.Forms.Label();
            this.lblT4 = new System.Windows.Forms.Label();
            this.lblResN4 = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            this.SuspendLayout();
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.Location = new System.Drawing.Point(20, 24);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.TabIndex = 0;
            this.lblCodigo.Text = "Código:";
            this.txtCodigo.Location = new System.Drawing.Point(130, 20);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Size = new System.Drawing.Size(220, 20);
            this.txtCodigo.TabIndex = 1;
            this.lblNroDoc.AutoSize = true;
            this.lblNroDoc.Location = new System.Drawing.Point(20, 52);
            this.lblNroDoc.Name = "lblNroDoc";
            this.lblNroDoc.TabIndex = 2;
            this.lblNroDoc.Text = "No. Documento:";
            this.txtNroDoc.Location = new System.Drawing.Point(130, 48);
            this.txtNroDoc.Name = "txtNroDoc";
            this.txtNroDoc.Size = new System.Drawing.Size(220, 20);
            this.txtNroDoc.TabIndex = 3;
            this.lblNombre.AutoSize = true;
            this.lblNombre.Location = new System.Drawing.Point(20, 80);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.TabIndex = 4;
            this.lblNombre.Text = "Nombre:";
            this.txtNombre.Location = new System.Drawing.Point(130, 76);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(220, 20);
            this.txtNombre.TabIndex = 5;
            this.lblSede.AutoSize = true;
            this.lblSede.Location = new System.Drawing.Point(20, 108);
            this.lblSede.Name = "lblSede";
            this.lblSede.TabIndex = 6;
            this.lblSede.Text = "Sede:";
            this.txtSede.Location = new System.Drawing.Point(130, 104);
            this.txtSede.Name = "txtSede";
            this.txtSede.Size = new System.Drawing.Size(220, 20);
            this.txtSede.TabIndex = 7;
            this.lblPrograma.AutoSize = true;
            this.lblPrograma.Location = new System.Drawing.Point(20, 136);
            this.lblPrograma.Name = "lblPrograma";
            this.lblPrograma.TabIndex = 8;
            this.lblPrograma.Text = "Programa:";
            this.txtPrograma.Location = new System.Drawing.Point(130, 132);
            this.txtPrograma.Name = "txtPrograma";
            this.txtPrograma.Size = new System.Drawing.Size(220, 20);
            this.txtPrograma.TabIndex = 9;
            this.lblN2.AutoSize = true;
            this.lblN2.Location = new System.Drawing.Point(20, 164);
            this.lblN2.Name = "lblN2";
            this.lblN2.TabIndex = 10;
            this.lblN2.Text = "Nota 2 (n2):";
            this.txtN2.Location = new System.Drawing.Point(130, 160);
            this.txtN2.Name = "txtN2";
            this.txtN2.Size = new System.Drawing.Size(220, 20);
            this.txtN2.TabIndex = 11;
            this.btnCalcular.Location = new System.Drawing.Point(130, 195);
            this.btnCalcular.Name = "btnCalcular";
            this.btnCalcular.Size = new System.Drawing.Size(220, 30);
            this.btnCalcular.TabIndex = 12;
            this.btnCalcular.Text = "Calcular y Grabar";
            this.btnCalcular.UseVisualStyleBackColor = true;
            this.btnCalcular.Click += new System.EventHandler(this.btnCalcular_Click);
            this.lblTituloNotas.AutoSize = true;
            this.lblTituloNotas.Location = new System.Drawing.Point(20, 243);
            this.lblTituloNotas.Name = "lblTituloNotas";
            this.lblTituloNotas.TabIndex = 13;
            this.lblTituloNotas.Text = "Notas calculadas:";
            this.lblT1.AutoSize = true;
            this.lblT1.Location = new System.Drawing.Point(20, 271);
            this.lblT1.Name = "lblT1";
            this.lblT1.TabIndex = 14;
            this.lblT1.Text = "Nota 1:";
            this.lblResN1.AutoSize = true;
            this.lblResN1.Location = new System.Drawing.Point(130, 271);
            this.lblResN1.Name = "lblResN1";
            this.lblResN1.TabIndex = 15;
            this.lblResN1.Text = "-";
            this.lblT3.AutoSize = true;
            this.lblT3.Location = new System.Drawing.Point(20, 299);
            this.lblT3.Name = "lblT3";
            this.lblT3.TabIndex = 16;
            this.lblT3.Text = "Nota 3:";
            this.lblResN3.AutoSize = true;
            this.lblResN3.Location = new System.Drawing.Point(130, 299);
            this.lblResN3.Name = "lblResN3";
            this.lblResN3.TabIndex = 17;
            this.lblResN3.Text = "-";
            this.lblT4.AutoSize = true;
            this.lblT4.Location = new System.Drawing.Point(20, 327);
            this.lblT4.Name = "lblT4";
            this.lblT4.TabIndex = 18;
            this.lblT4.Text = "Nota 4:";
            this.lblResN4.AutoSize = true;
            this.lblResN4.Location = new System.Drawing.Point(130, 327);
            this.lblResN4.Name = "lblResN4";
            this.lblResN4.TabIndex = 19;
            this.lblResN4.Text = "-";
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(20, 360);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(350, 50);
            this.lblError.TabIndex = 20;
            this.AcceptButton = this.btnCalcular;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 420);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmParcial1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Parcial 1 - Estudiante Pregrado";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
