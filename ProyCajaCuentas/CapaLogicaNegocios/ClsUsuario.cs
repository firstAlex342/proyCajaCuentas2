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

    }
}
