using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using crudParcial_2.clases;

namespace crudParcial_2
{
    public partial class crudForm : System.Web.UI.Page

    {
        static int option; // 0 nada, 1 grabar, 2 actualizar
        int intCodigo, intDocumento;
        string strNombre;

        private void Mensaje(string txt)
        {
            this.lblMsj.Text = txt;
        }

        private void Grabar()
        {

            try
            {
                //crear registro
                if (option == 1)
                {

                    //intTipDoc = Convert.ToInt32(this.ddlTDoc.SelectedValue);
                    strNombre = this.txtNomb.Text.Trim();
                    intDocumento = Convert.ToInt32(this.txtNDoc.Text.Trim());


                    clsCrud objectGrabar = new clsCrud(strNombre, intCodigo, intDocumento);

                    if (!objectGrabar.grabarMaestro())
                    {
                        Mensaje("error objeto" + objectGrabar.Error);
                        return;
                    }

                    if (objectGrabar.Respuesta == -1)
                    {
                        Mensaje("Ya existe un registro con ese Nro. Doc, intenta con otro");
                        return;
                    }

                    Mensaje("Grabado con exito");
                    cleaner();

                }
                else
                {
                    //modificar registro

                    intCodigo = Convert.ToInt32(this.txtCodi.Text);
                    strNombre = this.txtNomb.Text.Trim();
                    intDocumento = Convert.ToInt32(this.txtNDoc.Text.Trim());

                    clsCrud objectModificar = new clsCrud(strNombre, intCodigo, intDocumento);


                    if (!objectModificar.modificarMaestro())
                    {
                        Mensaje(objectModificar.Error);
                        return;
                    }

                    //esta entrando aca, por ende no modifica
                    if (objectModificar.Respuesta == -1)
                    {
                        Mensaje("Ya existe un registro con ese Nro. Doc, intenta con otro");
                        return;
                    }

                    if (objectModificar.Respuesta == 0)
                    {
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

        private void cleaner()
        {

            this.txtCodi.Text = string.Empty;
            this.txtNomb.Text = string.Empty;
            this.txtNDoc.Text = string.Empty;

            //this.ddlTDoc.SelectedIndex = 0;
        }




        protected void iBtnBusc_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                intCodigo = Convert.ToInt32(this.txtCodi.Text);

                if (intCodigo <= 0)
                {
                    cleaner();
                    Mensaje("El codigo es incorrecto");
                    this.iBtnBusc.Visible = false;
                    this.txtCodi.ReadOnly = true;
                    return;
                }

                clsCrud objectCrud = new clsCrud();


                if (!objectCrud.buscarMaestro(intCodigo))
                {

                    Mensaje(objectCrud.Error);
                    objectCrud = null;
                    this.txtNDoc.Focus();
                    return;

                }

                //this.ddlTDoc.SelectedValue = objectCrud.tipoDoc.ToString();
                this.txtNomb.Text = objectCrud.nombre;
                this.txtNDoc.Text = objectCrud.documento.ToString();
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
                    else
                    {
                        Mensaje("Seleccione primero Agregar o Modificar");
                    }

                    break;

                default:
                    cleaner();
                    option = 0;
                    break;
            }

        }


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                option = 0;
                //llenarTipoDoc();
                //llenarGenero();
            }

        }

        protected void ddlTDoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            Mensaje("Id tipo doc from tipo doc: " + ddlTDoc.SelectedValue);

        }

        protected void txtObse_TextChanged(object sender, EventArgs e)
        {

        }
    }
}