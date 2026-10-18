using crud_form.clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebSuperMarket
{
    public partial class frmUsuario : System.Web.UI.Page
    {

        static int option; // 0 nada, 1 grabar, 2 actualizar
        int intCodigo, intNroDoc, intTipDoc, intGenero;
        string strNombre, strApellido, strObservacion;

        bool bEnabled = false;

        private void Mensaje(string txt) { 
            this.lblMsj.Text = txt;
        }

        private void Grabar() {

            try
            {
                //crear registro
                if (option == 1)
                {

                    intNroDoc = Convert.ToInt32(this.txtNDoc.Text);
                    intTipDoc = Convert.ToInt32(this.ddlTDoc.SelectedValue);
                    intGenero = Convert.ToInt32(this.ddlGene.SelectedValue);
                    strNombre = this.txtNomb.Text.Trim();
                    strApellido = this.txtApel.Text.Trim();
                    strObservacion = this.txtObse.Text.Trim();
                    bEnabled = this.chkAct.Checked;

                    clsUsuario objectGrabar = new clsUsuario(intCodigo, intNroDoc, intTipDoc, strNombre, strApellido, intGenero, bEnabled, strObservacion);

                    if (!objectGrabar.grabarMaestro())
                    {
                        Mensaje("error objeto" + objectGrabar.strError);
                        return;
                    }

                    Mensaje("Grabado con exito");
                    cleaner();

                }
                else {
                    //modificar registro

                    intCodigo = Convert.ToInt32(this.txtCodi.Text);
                    intNroDoc = Convert.ToInt32(this.txtNDoc.Text);
                    intTipDoc = Convert.ToInt32(this.ddlTDoc.SelectedValue);
                    intGenero = Convert.ToInt32(this.ddlGene.SelectedValue);
                    strNombre = this.txtNomb.Text.Trim();
                    strApellido = this.txtApel.Text.Trim();
                    strObservacion = this.txtObse.Text.Trim();
                    bEnabled = this.chkAct.Checked;
    
                    clsUsuario objectModificar= new clsUsuario( intCodigo, intNroDoc, intTipDoc, strNombre, strApellido, intGenero, bEnabled, strObservacion);

                    
                    if (!objectModificar.modificarMaestro()) {
                        Mensaje(objectModificar.strError);
                        return;
                    }

                    //esta entrando aca, por ende no modifica
                    if (objectModificar.Respuesta == -1)
                    {
                        Mensaje("Ya existe un registro con ese Nro. Doc, intenta con otro");
                        return;
                    }

                    if (objectModificar.Respuesta == 0) { 
                        Mensaje("Error al ejecutar, reintente nuevamente");
                        return;
                    }

                    this.txtCodi.Text = objectModificar.Respuesta.ToString();
                    iBtnBusc_Click(null, null); // ejecuta lo que esta actualizando de un registro

                    Mensaje("Se realiza modificacion de usuario " + objectModificar.Respuesta);

                }

            }
            catch 
            {
                Mensaje("Error en el proceso de grabar");
            }
        }

        private void cleaner() { 

            this.txtCodi.Text = string.Empty;
            this.txtNDoc.Text = string.Empty;
            this.ddlTDoc.SelectedIndex = 0;
            this.txtNomb.Text = string.Empty;
            this.txtApel.Text = string.Empty;
            this.ddlGene.SelectedIndex = 0;
            this.chkAct.Checked = false;
            this.txtObse.Text = string.Empty;
        }

        private void llenarTipoDoc()
        {

            ddlTDoc.Items.Add(new ListItem("Seleccione un tipo de documento", "0"));
            ddlTDoc.Items.Add(new ListItem("Cedula de ciudadania", "1"));
            ddlTDoc.Items.Add(new ListItem("Cedula de extranjeria", "3"));
            ddlTDoc.Items.Add(new ListItem("Pasaporte", "6"));
            ddlTDoc.Items.Add(new ListItem("NIT", "7"));
            ddlTDoc.Items.Add(new ListItem("NUIP", "8"));
            ddlTDoc.Items.Add(new ListItem("Permiso de extranjeria", "9"));
            ddlTDoc.SelectedIndex = 0;
        }

        private void llenarGenero()
        {
            ddlGene.Items.Add(new ListItem("Seleccione un genero", "0"));
            ddlGene.Items.Add(new ListItem("Masculino", "10"));
            ddlGene.Items.Add(new ListItem("Femenino", "20"));
            ddlGene.SelectedIndex = 0;

        }

        protected void iBtnBusc_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                intCodigo = Convert.ToInt32(this.txtCodi.Text);

                if (intCodigo <= 0) {
                    cleaner();
                    Mensaje("El codigo es incorrecto");
                    this.iBtnBusc.Visible = false;
                    this.txtCodi.ReadOnly = true;
                    return;
                   }

                clsUsuario objectUsuario = new clsUsuario();


                if (!objectUsuario.buscarMaestro(intCodigo)) {

                    Mensaje(objectUsuario.strError);
                    objectUsuario = null;
                    this.txtNDoc.Focus();
                    return;

                  }

                this.txtNDoc.Text =  objectUsuario.nroDoc.ToString();
                this.ddlTDoc.SelectedValue = objectUsuario.tipoDoc.ToString();
                this.txtNomb.Text = objectUsuario.Nombre;
                this.txtApel.Text = objectUsuario.Apellido;
                this.txtObse.Text = objectUsuario.Observacion;
                this.ddlGene.SelectedValue = objectUsuario.Genero.ToString();
                this.chkAct.Checked = objectUsuario.Activo;

                this.iBtnBusc.Visible = false;
                this.txtCodi.ReadOnly = true;
                lblMsj.Text = "";
            }

            catch (Exception)
            {
                this.iBtnBusc.Visible = false;
                this.txtCodi.ReadOnly = true;
                Mensaje("error dCodigo no valido ");

                throw;
            }
        }
       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                option = 0;
                llenarTipoDoc();
                llenarGenero();
            }

        }

       protected void mnuOpci_MenuItemClick(object sender, MenuEventArgs e)
        {

            string strOption = e.Item.Text.ToLower();
            Mensaje(string.Empty);
            this.iBtnBusc.Visible = false;
            this.txtCodi.ReadOnly = true;

            switch (strOption)
            {
                case "agregar":
                    cleaner();
                    option = 1;
                    this.txtNDoc.Focus();
                    break;

                case "modificar":
                    option = 2;
                    this.txtNDoc.Focus();
                    break;

                case "buscar":
                    cleaner();
                    option = 0;

                    //visible y permite lectura el input solo cuando es buscar
                    this.txtCodi.ReadOnly = false;
                    this.iBtnBusc.Visible = true;
                    this.txtCodi.Focus();

                    break;

                case "grabar":
                    Mensaje("option = " + option);
                    if (option == 1 || option == 2)
                    {

                        Grabar();
                        this.txtCodi.Focus();

                    }
                    else {
                        Mensaje("Seleccione primero Agregar o Modificar");
                    }

                    break;

                default:
                    cleaner();
                    option = 0;
                    break;
            }

        }

        protected void ddlTDoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            Mensaje("Id tipo doc from tipo doc: " + ddlTDoc.SelectedValue);

        }
    }
}