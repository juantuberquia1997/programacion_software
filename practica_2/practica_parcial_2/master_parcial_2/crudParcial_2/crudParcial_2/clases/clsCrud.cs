using libCnxBD;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace crudParcial_2.clases
{
    public class clsCrud
    {
        //LAPTOP-VV1NA63H, SALAK406-35
        private string strCnx = "Server= LAPTOP-VV1NA63H; Database= admin_producto; Integrated Security= True; TrustServerCertificate= True;"; // cadena de conexion
        private string strSql; //transaccion
        private clsCnxBD objCnx;
        private SqlDataReader readerLocal;
        public string nombre { get; set; }
        public string Error { get; set; }
        public int documento { get; set; }

        public int Respuesta { get; set; }

        public int codigo { get; set; }



        public clsCrud(string nombre,  int cod, int documento)
        {
            this.nombre = nombre;
            this.codigo = cod;
            this.documento = documento;

        }

        public clsCrud()
        {
            nombre = string.Empty;
            codigo = 0;
            documento = 0;

        }

        private bool Validar(string strNombre, int documento)
        {

            bool result;

            if (string.IsNullOrEmpty(strNombre) || documento <= 0)
            {
                result = false;
            }
            else
            {
                result = true;
            }

            return result;
        }


        public bool buscarMaestro(int cod)
        {
            try
            {
                if (cod <= 0)
                {
                    Error = "Código inválido";
                    return false;
                }

                strSql = "EXEC USP_Usu_BuscarXCodigo '" + cod + "'; ";

                objCnx = new clsCnxBD();
                objCnx.cadCnx = strCnx;
                objCnx.SQL = strSql;

                if (!objCnx.Consultar(false))
                {
                    Error = objCnx.Error;
                    objCnx = null;
                    return false;
                }

                readerLocal = objCnx.readerLleno;

                if (!readerLocal.HasRows)
                {
                    Error = "No se encontró el registro en la BD: " + cod;
                    readerLocal.Close();
                    objCnx = null;
                    return false;
                }

                readerLocal.Read();

                nombre = readerLocal.GetString(0);
                documento = readerLocal.GetInt32(1);

                readerLocal.Close();
                objCnx = null;
                return true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return false;
            }
        }

        //recibe dos valores 1-insertar o 2 - modificar,
        private bool Grabar(int Accion)
        {
            try
            {
                objCnx = new clsCnxBD();
                objCnx.cadCnx = strCnx;
                objCnx.SQL = strSql;

                if (Accion == 2)
                {
                    if (!objCnx.addParam("@intCodigo", codigo))
                    {
                        Error = objCnx.Error;
                        objCnx = null;
                        return false;
                    }
                }
                else if (Accion != 1)
                {
                    Error = "Acción no válida";
                    return false;
                }

                if (!objCnx.addParam("@strNombre", nombre))
                {
                    Error = objCnx.Error;
                    objCnx = null;
                    return false;
                }

                if (!objCnx.addParam("@intDocumento", documento))
                {
                    Error = objCnx.Error;
                    objCnx = null;
                    return false;
                }


                /* if (!objCnx.addParam("@intNroDoc", nroDoc))
                 {
                     strError = objCnx.Error;
                     objCnx = null;
                     return false;
                  */


                // Ejecutar stored procedure asincrónicamente --> true , este vrunico retorma 0, 1 o -1
                if (!objCnx.vrUnico(true))
                {
                    Error = objCnx.Error;
                    objCnx = null;
                    return false;
                }

                // aqui retorna el -1, 0 o el numero de registro que quedo grabado
                Respuesta = Convert.ToInt32(objCnx.vrRpta);
                objCnx = null;
                return true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return false;
            }
        }


        public bool grabarMaestro()
        {
            if (!Validar(nombre, documento))
            {
                Error = "Datos inválidos, favor validar";
                return false;
            }

            strSql = "USP_Usu_Grabar";
            return Grabar(1);
        }

        public bool modificarMaestro()
        {
            if (!Validar(nombre, documento))
            {
                Error = "Datos inválidos, favor validar";
                return false;
            }

            if (codigo <= 0)
            {
                Error = "Numero de código no válido";
                return false;
            }

            strSql = "USP_Usu_Modificar";
            return Grabar(2);
        }
    }
}