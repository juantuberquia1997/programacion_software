using System;
using System.Windows.Forms;
using libParcial1;

namespace frmParcial1
{
    public partial class frmParcial1 : Form
    {
        public frmParcial1()
        {
            InitializeComponent();
        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            lblError.Text = string.Empty;

            if (!int.TryParse(txtCodigo.Text, out int codigo))
            {
                lblError.Text = "El código debe ser numérico";
                return;
            }

            if (!int.TryParse(txtNroDoc.Text, out int nroDoc))
            {
                lblError.Text = "El número de documento debe ser numérico";
                return;
            }

            if (!float.TryParse(txtN2.Text, out float n2))
            {
                lblError.Text = "La nota 2 debe ser numérica";
                return;
            }

            clsPregrado objPregrado = new clsPregrado(
                codigo,
                nroDoc,
                txtNombre.Text.Trim(),
                txtSede.Text.Trim(),
                txtPrograma.Text.Trim(),
                n2);

            objPregrado.CalcularNotas();

            lblResN1.Text = objPregrado.N1.ToString("0.##");
            lblResN3.Text = objPregrado.N3.ToString("0.##");
            lblResN4.Text = objPregrado.N4.ToString("0.##");

            if (objPregrado.Grabar())
            {
                MessageBox.Show("Registro grabado en estPreg.txt", "Parcial 1",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                lblError.Text = objPregrado.Error;
            }
        }
    }
}
