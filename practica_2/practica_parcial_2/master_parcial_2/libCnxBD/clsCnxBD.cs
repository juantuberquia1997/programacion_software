using System;

//Referenciar y usar
using System.Data;
using System.Data.SqlClient; //ado.net para sql

namespace libCnxBD
{
    public class clsCnxBD
    {
    #region "Atríbutos"
        private bool blnHayCnx;         //Para saber si hay o no Cnx a la BD
        private SqlConnection objCnx;   //Para el objeto Conexión
        private SqlCommand    objCmd;   //Para el objeto Command (realiza la transacción)
        private SqlParameter  objParam; //Para el objeto Parameter (contiene valores para el parámetro utilizado por el command)
    #endregion

    #region "Propiedades"
        public string SQL { private get; set; }  //Para la sentencia SQL a ejecutarse en la BD
        public string cadCnx { private get; set; }  //Para la sentencia SQL a ejecutarse en la BD
        public object vrRpta { get; private set; }  //Para la captura y retorno del Vr. único(método: ConsultarValorUnico)) 
        public string Error { get; private set; }  //Para el mensaje de error
        //Objetos ADO.NET
        public SqlDataReader readerLleno { get; private set; }  //Para el objeto DataReader (contenedor de info)
    #endregion

    #region "Constructor"
        public clsCnxBD( )
        {
            blnHayCnx = false;
            //Objetos ADO.NET
            objCnx = new SqlConnection();  //Para la Conexión
            objCmd = new SqlCommand();     //Para la Transacción
            objParam = new SqlParameter();   //Para los parámetros del Command
        }
    #endregion

    #region "Métodos Privados"
        private bool abrirCnx( )
        {
            if ( string.IsNullOrEmpty( cadCnx ) )
                return false;
            objCnx.ConnectionString = cadCnx;
            try
            {
                objCnx.Open();
                blnHayCnx = true;
                return true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                blnHayCnx = false;
                return false;
            }
        }
    #endregion

    #region "Métodos Públicos"
        public void cerrarCnx()
        {
            try
            {
                objCnx.Close();
                objCnx = null;
                blnHayCnx = false;
            }
            catch (Exception ex)
            {
                Error = "Problemas en cierre o liberación de la conexión, " + ex.Message;
            }
        }
        public bool Consultar( bool tipoEjec )
        {
            try
            {
                if (string.IsNullOrEmpty(SQL) || string.IsNullOrEmpty(cadCnx))
                {
                    Error = "Faltan datos";
                    return false;
                }
                if ( ! blnHayCnx )
                    if ( ! abrirCnx( ) )
                        return false;
                //Preparar el Comando para ejecutar la transacción SQL en la BD
                objCmd.Connection = objCnx;
                objCmd.CommandText = SQL;

                if ( tipoEjec )
                    objCmd.CommandType = CommandType.StoredProcedure;  //Asincrónico
                else
                    objCmd.CommandType = CommandType.Text;             //Sincronico
                readerLleno = objCmd.ExecuteReader(); //Realizar la transacción en la BD
                return true;
            }
            catch ( Exception ex )
            {
                Error = ex.Message;
                return false;
            }
        }
        public bool vrUnico( bool tipoEjec )
        {
            try
            {
                if ( string.IsNullOrEmpty( SQL ) || string.IsNullOrEmpty( cadCnx ) )
                {
                    Error = "Faltan datos";
                    return false;
                }
                if ( ! blnHayCnx )
                    if ( ! abrirCnx( ) )
                        return false;
                //Preparar el Comando para ejecutar la transacción SQL en la BD
                objCmd.Connection = objCnx;
                objCmd.CommandText = SQL;
                if ( tipoEjec )
                    objCmd.CommandType = CommandType.StoredProcedure;
                else
                    objCmd.CommandType = CommandType.Text;
                vrRpta = objCmd.ExecuteScalar();  //Realizar la transacción en la BD
                return true;
            }
            catch ( Exception ex )
            {
                Error = ex.Message;
                return false;
            }
        }
        public bool execCmd( bool tipoEjec )
        {
            try
            {
                if ( string.IsNullOrEmpty( SQL ) || string.IsNullOrEmpty( cadCnx ) )
                {
                    Error = "Faltan datos";
                    return false;
                }
                if ( ! blnHayCnx )
                    if ( ! abrirCnx( ) )
                        return false;
                //Preparar el Comando para ejecutar la transacción SQL en la BD
                objCmd.Connection = objCnx;
                objCmd.CommandText = SQL;
                if ( tipoEjec )
                    objCmd.CommandType = CommandType.StoredProcedure;
                else
                    objCmd.CommandType = CommandType.Text;
                objCmd.ExecuteNonQuery();   //Realizar la transacción en la BD
                return true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return false;
            }
        }       
        public bool addParam( string nombreParam, object vrParam )
        {
            try
            {
                objCmd.Parameters.AddWithValue( nombreParam, vrParam ); //Adicionar el parámetro al command
                objParam = new SqlParameter();     //Crear un nuevo parámetro para el siguiente parámetro
                return true;
            }
            catch
            {
                Error = "Error en ingreso de dato.";
                return false;
            }
        }
    #endregion
    }
}
