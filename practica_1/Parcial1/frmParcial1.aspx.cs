using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using clsParcial1;
using System.IO;

namespace frmParcial1
{
    public partial class frmParcial1 : System.Web.UI.Page
    {

        static string ruta = "~/datosTxt/estPreg.txt";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void inputN2_TextChanged(object sender, EventArgs e)
        { 


        }

        protected void calcular_Click(object sender, EventArgs e)
        {

            float parserNota2 = 0, nota1, auxiliar1, auxiliar2, nota3, nota4;

            parserNota2 = Convert.ToSingle(this.inputN2.Text);
            parserNota2 = Convert.ToSingle(this.inputN2.Text);

            nota1 =  Math.Abs( parserNota2 - (float)(6 * Math.Sqrt(parserNota2)));

            auxiliar1 = parserNota2 + (float)Math.Sqrt(nota1) + ((parserNota2 / 9));

            auxiliar2 = (parserNota2 - (parserNota2 % 10)) / 10;

            nota3 = auxiliar1 + auxiliar2;

            nota4 = nota3 + (5 * parserNota2) + (nota1 / 4);


            clsPregrado objectParcial1 = new clsPregrado(nota1, parserNota2, nota3, nota4);

            string script = $"console.log('auxiliar1 __ es: {objectParcial1}');";
            ClientScript.RegisterStartupScript(this.GetType(), "debugLog", script, true);

            if (objectParcial1.Grabar()) {

                using (StreamWriter sw = new StreamWriter(Server.MapPath(ruta), true))
                {
                    sw.WriteLine("nota 1:" + nota1);
                    sw.WriteLine("nota 2:" + parserNota2);
                    sw.WriteLine("nota 3:" + nota3);
                    sw.WriteLine("nota 4:" + nota4);
                    sw.WriteLine("&");

                }

            }
          

        }
    }
}