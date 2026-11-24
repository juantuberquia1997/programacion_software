using Antlr.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace webLavanderia.Paginas
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            this.Timer1.Enabled = false;
            //Timer1.Interval = 4500;  /*1seg = 1000*/
            Response.Redirect("/Paginas/frmLogueo.html");
        }
    }
}