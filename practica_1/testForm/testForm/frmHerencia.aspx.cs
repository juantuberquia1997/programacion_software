//import libreria
using libraryFigura2d;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace testForm
{
    public partial class frmHerencia : System.Web.UI.Page
    {

        //Variables globales
        static int intFigure; //  1 = Triangulo, 2 rectangulo, 3 rombo
        static string ruta = "~/datos/resultados.txt";



        //metodos propios
        //guardar el valor ingresado en el input
        private void Message(string message) {
            lblMsj.Text = message;
        }

        private void cargarImagen(string name) {
            string dir = "~/imagenes/" + name;

            imgFigura.ImageUrl = dir; 
        }

        private void cleaner() { 
            txtLadoA.Text = string.Empty;
            txtLadoB.Text = string.Empty;
            txtAngulo.Text = string.Empty;

            lblPerim.Text = string.Empty;
            lblArea.Text = string.Empty;

            txtLado1.Text = string.Empty;
            txtLado2.Text = string.Empty;

            txtDiagMn.Text = string.Empty;
            txtDiagMy.Text = string.Empty;
            txtLado.Text = string.Empty;

            Message(string.Empty);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            //ispostback cuando se carga la page por primera vez, !negada es una recarga
            if (!IsPostBack) {
                intFigure = 1;
                cargarImagen("triangulo.jpg");
            }
        }


        // radio button list
        protected void rblFiguras_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.pnlTriangulo.Visible = false;
            this.pnlRectangulo.Visible = false;
            this.pnlRpta.Visible = false;
            this.pnlRombo.Visible = false;
            this.cleaner();

            intFigure = this.rblFiguras.SelectedIndex + 1;

            switch (intFigure)
            {
                case 1:
                    this.pnlTriangulo.Visible = true; 
                    
                    cargarImagen("triangulo.jpg");
                    this.txtLadoA.Focus();
                    break;

                case 2:
                    this.pnlRectangulo.Visible = true;
                    cargarImagen("rectangulo.jpg");
                    this.txtLado1.Focus();
                    break;

                default:
                    this.pnlRombo.Visible = true;
                    cargarImagen("rombo.jpg");
                    this.txtDiagMy.Focus();
                    break; 
            }
        }

        protected void txtLadoA_TextChanged(object sender, EventArgs e)
        {
        }

        protected void txtLadoB_TextChanged(object sender, EventArgs e)
        {
        }

        protected void TextBox2_TextChanged(object sender, EventArgs e)
        {
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            float data1 = 0, data2 = 0, data3 = 0;

            switch (intFigure)
            {

                case 1:
                    data1 = Convert.ToSingle(this.txtLadoA.Text);
                    data2 = Convert.ToSingle(this.txtLadoB.Text);
                    data3 = Convert.ToSingle(this.txtAngulo.Text);

                    //instanciar clase 
                    clsTriangulo objetcTringulo = new clsTriangulo(data1, data2, data3);

                    if (!objetcTringulo.hallarArea() || !objetcTringulo.hallarPerimetro()) {
                        Message(objetcTringulo.Error);
                        objetcTringulo = null;
                        return;
                    }

                    this.lblArea.Text = Convert.ToString(objetcTringulo.Area);
                    this.lblPerim.Text = Convert.ToString(objetcTringulo.Perimetro);

                    pnlRpta.Visible = true;


                    string script = $"console.log('El área es: {objetcTringulo.Area}'); console.log('El perímetro es: {objetcTringulo.Perimetro}');";
                    ClientScript.RegisterStartupScript(this.GetType(), "debugLog", script, true);


                    using (StreamWriter sw = new StreamWriter(Server.MapPath(ruta), true))
                    {
                        sw.WriteLine("Area:" + objetcTringulo.Area);
                        sw.WriteLine("Perimetro:" + objetcTringulo.Perimetro);
                    }

                    break;
                case 2:

                    data1 = Convert.ToSingle(txtLado1.Text);
                    data2 = Convert.ToSingle(txtLado2.Text);

                    clsRectangulo objectRectangulo = new clsRectangulo(data1, data2);

                    if (!objectRectangulo.hallarPerimetro() || !objectRectangulo.hallarArea()) {

                        Message(objectRectangulo.Error);
                        objectRectangulo = null;
                        return;
                    }

                    lblArea.Text = Convert.ToString(objectRectangulo.Area);
                    lblPerim.Text = Convert.ToString(objectRectangulo.Perimetro);
                    pnlRpta.Visible = true;
                    break;

                default:
                    data1 = Convert.ToSingle(txtDiagMn.Text);
                    data2 = Convert.ToSingle(txtDiagMy.Text);
                    data3 = Convert.ToSingle(txtLado.Text);

                    clsRombo objectRombo = new clsRombo(data3, data2, data1);

                    if (!objectRombo.hallarPerimetro() || !objectRombo.hallarArea()) {

                        Message(objectRombo.Error);
                        objectRombo = null;
                        return;
                    }

                    this.lblArea.Text = Convert.ToString(objectRombo.Area);
                    this.lblPerim.Text = Convert.ToString(objectRombo.Perimetro);
                    pnlRpta.Visible = true;

                    break;
            }

        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            cleaner();
        }
    }
}