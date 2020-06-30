using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CapaAccesoDatos;

namespace CapaLogicaNegocios
{
    public class ClsUsuario
    {
        public int Id { set; get; }
        public string Nombre { set; get; }
        public string Usuario { set; get; }
        public string Password { set; get; }
        public int IdUsuarioAlta { set; get; }
        public DateTime FechaAlta { set; get; }
        public int IdUsuarioModifico { set; get; } 
        public  DateTime FechaModificacion { set; get; }
        public  bool Activo { set; get; }

        public ClsManejador CLSManejador { set; get; }

        //--------------Constructor
        public ClsUsuario()
        {
            this.Id = 0;
            this.Nombre = String.Empty;
            this.Usuario = String.Empty;
            this.Password = String.Empty;
            this.IdUsuarioAlta = 0;
            this.FechaAlta = new DateTime();
            this.IdUsuarioModifico = 0;
            this.FechaModificacion = new DateTime();
            this.Activo = true;

            this.CLSManejador = new ClsManejador();
        }
        
        
        //----------------Methods
        public DataTable Usuario_BuscarXUsuarioYPassword()
        {
            List<ClsParametros> lst = new List<ClsParametros>();
            lst.Add(new ClsParametros("@usuarioBuscado",this.Usuario));
            lst.Add(new ClsParametros("@password", this.Password));


            return (CLSManejador.Listado("Usuario_BuscarXUsuarioYPassword", lst));
        }

        public DataTable Usuario_BuscarXId()
        {
            List<ClsParametros> lst = new List<ClsParametros>();
            lst.Add(new ClsParametros("@idUsuarioBuscado", this.Id));

            return (CLSManejador.Listado("Usuario_BuscarXId", lst));
        }

        public DataTable Usuario_create()
        {

            List<ClsParametros> lst = new List<ClsParametros>();

            //Parametros de entrada
            lst.Add(new ClsParametros("@nombre", this.Nombre));
            lst.Add(new ClsParametros("@usuario", this.Usuario));
            lst.Add(new ClsParametros("@password", this.Password));
            lst.Add(new ClsParametros("@idUsuarioOperador", this.IdUsuarioAlta));


 
            return(CLSManejador.Listado("Usuario_create", lst));


        }

        public DataTable Usuario_Select_Id_Nombre_Usuario_Pass_Activo_DeTodos()
        {
            List<ClsParametros> lst = new List<ClsParametros>();
            lst.Add(new ClsParametros("@parametroNoNecesario", 0));

            return (CLSManejador.Listado("Usuario_Select_Id_Nombre_Usuario_Pass_Activo_DeTodos", lst));
        }

        public DataTable Usuario_BuscarXUsuario()
        {
            List<ClsParametros> lst = new List<ClsParametros>();
            lst.Add(new ClsParametros("@usuarioBuscado", this.Usuario));

            return (CLSManejador.Listado("Usuario_BuscarXUsuario", lst));
        }


        public string Usuario_update()
        {
            string mensaje = "";
            List<ClsParametros> lst = new List<ClsParametros>();

            //Parametros de entrada
            lst.Add(new ClsParametros("@idUsuarioBuscado", this.Id));
            lst.Add(new ClsParametros("@newNombre", this.Nombre));
            lst.Add(new ClsParametros("@newUsuario", this.Usuario));
            lst.Add(new ClsParametros("@newPassword", this.Password));
            lst.Add(new ClsParametros("@idUsuarioOperador", this.IdUsuarioModifico));

            //Parametro de salida
            lst.Add(new ClsParametros("@mensaje", SqlDbType.VarChar, 50));
            CLSManejador.Ejecutar_sp("Usuario_update", lst);

            //Regresar el valor almacenado en el parametro de salida
            mensaje = lst[5].Valor.ToString();

            return (mensaje);
        }

        public DataTable Usuario_SumarContenidosDeFolio()
        {
            List<ClsParametros> lst = new List<ClsParametros>();
            lst.Add(new ClsParametros("@fechaInicio", this.FechaAlta));
            lst.Add(new ClsParametros("@fechaFin", this.FechaModificacion));

            return (CLSManejador.Listado("Usuario_SumarContenidosDeFolio", lst));
        }

        public string Usuario_AccesoAModulo_Create()
        {
            string mensaje = "";
            List<ClsParametros> lst = new List<ClsParametros>();

            //Parametros de entrada
            lst.Add(new ClsParametros("@nombre", this.Nombre));
            lst.Add(new ClsParametros("@usuario", this.Usuario));
            lst.Add(new ClsParametros("@password", this.Password));
            lst.Add(new ClsParametros("@idUsuarioOperador", this.IdUsuarioAlta));

            //Parametro de salida
            lst.Add(new ClsParametros("@mensaje", SqlDbType.VarChar, 50));
            CLSManejador.Ejecutar_sp("Usuario_AccesoAModulo_Create", lst);

            //Regresar el valor almacenado en el parametro de salida
            mensaje = lst[4].Valor.ToString();

            return (mensaje);
        }
    }
}
