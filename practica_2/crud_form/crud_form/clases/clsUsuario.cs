using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Runtime.Remoting.Messaging;
using libCnxBD;
using System.Linq;
using System.Data.SqlClient;// permite comunicar la app con la BD, consumir la info que esta almacenada alli

namespace crud_form.clases
{
    public class clsUsuario
    {
        //atributos
        private string strCnx = "Server= LAPTOP-VV1NA63H; Database= bdSuperMkdo; Integrated Security= True; TrustServerCertificate= True;"; // cadena de conexion
        private  string strSql; //transaccion
        private clsCnxBD objCnx; 
        private SqlDataReader readerLocal;


        //propiedades-atributos
        public int Codigo { get; set; }
        public int nroDoc { get; set; }
        public int tipoDoc { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Genero { get; set; }
        public bool Activo { get; set; }
        public string  Observacion { get; set; }
        public int Respuesta { get; private set; }
        public string strError { get; private set; }

        //metodo validar
        private bool validar(int nroDoc, int tipoDoc, string nombre, string apellido, int genero)
        {
            bool valid = false;
            int[] numbersAllowed = { 1, 3, 6, 7, 8, 9 };
            int[] generoAllowed = { 10, 20 };


            if (!numbersAllowed.Contains(tipoDoc)){
                valid = false;
                strError = "tipo de documento invalido";
                return valid;
            }

            if (!generoAllowed.Contains(genero))
            {
                valid = false;
                strError = "genero invalido";
                return valid;
            }


            if ((nroDoc <= 0) || (string.IsNullOrEmpty(nombre)) || (string.IsNullOrEmpty(apellido)))
            {
               valid = false;
            }
            else { 
              valid = true;
             }

            return valid;

        }
        //constructores
        public clsUsuario(int codigo, int documento, int tipoDocumento, string nombre, string apellido, int genero, bool active, string observacion)
        {
            Codigo = codigo;
            nroDoc = documento;
            tipoDoc = tipoDocumento;
            Nombre = nombre;
            Apellido = apellido;
            Genero = genero;
            Activo = active;
            Observacion = observacion;
            Respuesta = 0;
            strError = string.Empty;
        }

        public clsUsuario()
        {
            Codigo = 0;
            nroDoc = 0;
            tipoDoc = 0;
            Nombre = string.Empty;
            Apellido = string.Empty;
            Genero = 0;
            Activo = false;
            Observacion = string.Empty;
            Respuesta = 0;
            strError = string.Empty;
        }

        // buscar maestro
        public bool buscarMaestro(int cod)
        {
            try
            {
                if (cod <= 0)
                {
                    strError = "Carné/Código no válido";
                    return false;
                }

                strSql = "EXEC USP_Usu_BuscarXCodigo '" + cod + "'; ";

                objCnx = new clsCnxBD();
                objCnx.cadCnx = strCnx;
                objCnx.SQL = strSql;

                if (!objCnx.Consultar(false)) //Ejecuta sincrónicamente el método Consultar
                {
                    strError = objCnx.Error;
                    objCnx = null;
                    return false;
                }

                readerLocal = objCnx.readerLleno;

                if (!readerLocal.HasRows)
                {
                    strError = "No se encontró ningún registro: " + cod;
                    readerLocal.Close();
                    objCnx = null;
                    return false;
                }

                readerLocal.Read();

                nroDoc = readerLocal.GetInt32(0); // 0 = columna 1 y getInt32 es porque se que es int
                tipoDoc = readerLocal.GetInt32(1);
                Nombre = readerLocal.GetString(2);
                Apellido = readerLocal.GetString(3);
                Genero = readerLocal.GetInt32(4);
                Activo = readerLocal.GetBoolean(5);
                Observacion = readerLocal.GetString(6);

                readerLocal.Close();
                objCnx = null;
                return true;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }
        }

        //pasar el metodo grabar
        //los stored procedure de la pag 7,  --enviar el resto de los parametros--
        //recibe dos valores 1-insertar o 2-modificar, 
        private bool Grabar(int Accion)
        {
            try
            {
                objCnx = new clsCnxBD();
                objCnx.cadCnx = strCnx; // le asignamos la cadena de conexion que habiamos creado
                objCnx.SQL = strSql; // agregamos el sql que va a ejecutar si es grabar o modificar

                if (Accion == 2) // Modificar //Enviar parámetros requeridos por el USP
                {
                    if (!objCnx.addParam("@intCodigo", Codigo))
                    {
                        strError = objCnx.Error;
                        objCnx = null;
                        return false;
                    }
                }
                else if (Accion != 1)
                {
                    strError = "Acción no válida";
                    return false;
                }

                //Enviar el resto de los parámetros cada uno con el nombre del parámetro en el stored procedure
                if (!objCnx.addParam("@intNroDoc", nroDoc))
                {
                    strError = objCnx.Error;
                    objCnx = null;
                    return false;
                }
                if (!objCnx.addParam("@intTipDoc", tipoDoc))
                {
                    strError = objCnx.Error;
                    objCnx = null;
                    return false;
                }
                if (!objCnx.addParam("@strNombre", Nombre))
                {
                    strError = objCnx.Error;
                    objCnx = null;
                    return false;
                }
                if (!objCnx.addParam("@strApelli", Apellido))
                {
                    strError = objCnx.Error;
                    objCnx = null;
                    return false;
                }
                if (!objCnx.addParam("@intGenero", Genero))
                {
                    strError = objCnx.Error;
                    objCnx = null;
                    return false;
                }
                if (!objCnx.addParam("@bitActivo", Activo))
                {
                    strError = objCnx.Error;
                    objCnx = null;
                    return false;
                }
                if (!objCnx.addParam("@strObserv", Observacion))
                {
                    strError = objCnx.Error;
                    objCnx = null;
                    return false;
                }

                // Ejecutar stored procedure asincrónicamente --> true , este vrunico retorma 0, 1 o -1
                if (!objCnx.vrUnico(true)) 
                {
                    strError = objCnx.Error;
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
                strError = ex.Message;
                return false;
            }
        }

        //grabarMaestro 
        public bool grabarMaestro()
        {
            if (!validar(nroDoc, tipoDoc, Nombre, Apellido, Genero))
            {
                strError = "Datos incompletos o inválidos";
                return false;
            }

            strSql = "USP_Usu_Grabar";
            return Grabar(1);
        }

        //modificarMaestro 
        public bool modificarMaestro()
        {
            if (!validar(nroDoc, tipoDoc, Nombre, Apellido, Genero))
            {
                strError = "Datos incompletos o inválidos";
                return false;
            }

            if (Codigo <= 0)
            {
                strError = "Nro. de Código no válido";
                return false;
            }

            strSql = "USP_Usu_Modificar";
            return Grabar(2);
        }


    }
}