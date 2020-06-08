using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;



namespace CapaAccesoDatos
{
    public class ClsManejador
    {
        static Settings1 conexionDatos = new Settings1();
        static string usuario = conexionDatos.Usuario;
        static string password = conexionDatos.Password;
        static string baseDeDatos = conexionDatos.BaseDeDatos;
        static string servidor = conexionDatos.Servidor;

        //Server=i3\\SQLEXPRESS; DataBase=tgcontrol;user=sa; password=Admin.2018; integrated security = true
        //Data source = 65.99.252.110; Initial Catalog = ctzmx_tgcontrol; User Id = ctzmx_sa; password=Inttesi#2018
        //Data source = 192.168.0.102; Initial Catalog = TGC; User Id = tgcontrol; password=gym2017
        //SqlConnection Conexion = new SqlConnection("Data source =CC-PC\\SQLEXPRESS; Initial Catalog =TGC; User Id =sa; password=sqlserver");
        //SqlConnection Conexion = new SqlConnection("Data source = LAPTOP-QIDIFK7G,49172; Initial Catalog = tgcontrol; User Id = sa; password=sqlserver.2018");
        //SqlConnection Conexion = new SqlConnection("Data source = tcp:RECEPCION-PC,49172; Initial Catalog = totalgym; User Id = sa; password=sqlserver.2018");
        //SqlConnection Conexion = new SqlConnection(@"Data source = CRUZ2-THINK; Initial Catalog = DBCajaCuentas2; User Id = sa; password=modomixto");
        SqlConnection Conexion;

        //Settings1 conexionDatos = new Settings1();

        //private SqlConnection Conexion()
        //{
        //    string usuario = conexionDatos.Usuario;
        //    string password = conexionDatos.Password;
        //    string baseDeDatos = conexionDatos.BaseDeDatos;
        //    string servidor = conexionDatos.Servidor;

        //    return Conexion;
        //}

        //----------------Constructor
        public ClsManejador()
        {
            Conexion = new SqlConnection(ObtenerCadenaConexionAppConfig());
        }


        public bool cambiarDatosConexion(string usuario, string password, string bd, string servidor)
        {
            try
            {
                conexionDatos.Usuario = usuario;
                conexionDatos.Password = password;
                conexionDatos.BaseDeDatos = bd;
                conexionDatos.Servidor = servidor;
                conexionDatos.Save();
                return validarConexion();
            }
            catch
            {
                return false;
            }

        }

        public string[] verDatosConexion()
        {
            string usuario = conexionDatos.Usuario;
            string password = conexionDatos.Password;
            string baseDeDatos = conexionDatos.BaseDeDatos;
            string servidor = conexionDatos.Servidor;
            string[] datos = new string[4] { usuario, password, baseDeDatos, servidor };
            return datos;
        }

        // metodo para abrir coneccion
        void abrir_conexion()
        {
            if (Conexion.State == ConnectionState.Closed)
                Conexion.Open();

        }

        void cerrar_conexion()
        {
            if (Conexion.State == ConnectionState.Open)
                Conexion.Close();
        }

        public bool validarConexion()
        {
            try
            {
                Conexion.Open();
                Conexion.Close();
                return true;
            }
            catch
            {
                Conexion.Close();
                return false;
            }
        }

        // metodo para ejecutar el SP  Insert, update, delete 
        public void Ejecutar_sp(string NombreSP, List<ClsParametros> lst)
        {
            SqlCommand cmd;

            try
            {


                abrir_conexion(); // intentamos abrir la conexion
                cmd = new SqlCommand(NombreSP, Conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                if (lst != null)
                {
                    for (int i = 0; i < lst.Count; i++)   //recorremos la lista de parametros del SP en tratamiento
                    {
                        if (lst[i].Direccion == ParameterDirection.Input)
                        {
                            cmd.Parameters.AddWithValue(lst[i].Nombre, lst[i].Valor);

                        }
                        if (lst[i].Direccion == ParameterDirection.Output)
                        {
                            cmd.Parameters.Add(lst[i].Nombre, lst[i].TipoDato, lst[i].Tamano).Direction = ParameterDirection.Output;
                        }

                    }
                }
                cmd.ExecuteNonQuery();
                for (int ii = 0; ii < lst.Count; ii++)
                        {
                            if (cmd.Parameters[ii].Direction == ParameterDirection.Output)
                                lst[ii].Valor = cmd.Parameters[ii].Value.ToString();  // else mensaje de salida del SP siempre va a ser cadena, mensaje de error
                        }

                
                
            }
            catch (Exception ex)
            {
                throw ex;   // si hay error cachamos el error

            }
            cerrar_conexion();  // si no hay error cerramos la conexion
        }


        // Metodo para listado o consultas  select
        public DataTable Listado(string NombreSP, List<ClsParametros> lst)
        {
            DataTable dt = new DataTable();  // tipo de dato de visual studio contenedor de una tabla
            SqlDataAdapter Da;               // tipo de dato de visual studio contenedor de una tabla de SQL
            try
            {
                Da = new SqlDataAdapter(NombreSP, Conexion);   // este adaptador lo reconoce la libreria de sql como un contenedor de datos
                Da.SelectCommand.CommandType = CommandType.StoredProcedure;  // le decimos al adaptador que es un tipo SP
                if (lst != null)
                {
                    for (int i = 0; i < lst.Count; i++)
                    {
                        //agregamos los parametros del SP al SQLAdapter para ser ejecutado por fill
                        Da.SelectCommand.Parameters.AddWithValue(lst[i].Nombre, lst[i].Valor);
                    }
                }
                Da.Fill(dt);
            }

            catch (Exception Ex)
            {
                throw Ex;
            }
            return dt;
        }



        /// <summary>
        /// Lee el App.config  ubicado en la capa de presentacion
        /// </summary>
        /// <returns>Regresa la cadena de conexión</returns>
        private string ObtenerCadenaConexionAppConfig()
        {
            SqlConnectionStringBuilder str = new SqlConnectionStringBuilder();
            str["Data Source"] = ConfigurationManager.AppSettings["servidor"];
            str["Initial Catalog"] = ConfigurationManager.AppSettings["nombreBd"];
            str["User Id"] = ConfigurationManager.AppSettings["username"];
            str["password"] = ConfigurationManager.AppSettings["password"];

            return (str.ConnectionString);
        }


        /// <summary>
        /// Regresa la cadena de conexión que tiene almacenado dentro de la propiedad Conexion.ConnectionString
        /// </summary>
        /// <returns></returns>
        public string ObtenerCadenaConexionDeCapaAccesoDatos()
        {
            return (Conexion.ConnectionString);
        }
    }
}

