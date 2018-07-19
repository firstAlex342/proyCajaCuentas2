using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CapaAccesoDatos;

namespace CapaLogicaNegocios
{
    public class ClsOperaCaja
    {
        public int IdUsuario { set; get; }
        public int IdCaja { set; get; }
        public int IdUsuarioAlta { set; get; }
        public DateTime FechaAlta { set; get; }
        public int IdUsuarioModifico { set; get; }
        public DateTime FechaModificacion { set; get; }
        public bool Activo { set; get; }

        public ClsManejador CLSManejador { set; get; }

        //----------------Constructor
        public ClsOperaCaja()
        {
            this.IdUsuario = 0;
            this.IdCaja = 0;
            this.IdUsuarioAlta = 0;
            this.FechaAlta = new DateTime();
            this.IdUsuarioModifico = 0;
            this.FechaModificacion = new DateTime();
            this.Activo = true;

            this.CLSManejador = new ClsManejador();
        }//parameterless constructor


        //---------------------Methods
        public DataTable OperaCaja_BuscarCajasDelDiaDelUsuario()
        {
            List<ClsParametros> lst = new List<ClsParametros>();
            lst.Add(new ClsParametros("@idUsuarioOperador", this.IdUsuario));

            return (CLSManejador.Listado("OperaCaja_BuscarCajasDelDiaDelUsuario", lst));
        }

        public string OperaCaja_create()
        {
            string mensaje = "";
            List<ClsParametros> lst = new List<ClsParametros>();

            //Parametros de entrada
            lst.Add(new ClsParametros("@idUsuario",this.IdUsuario));
            lst.Add(new ClsParametros("@idCaja", this.IdCaja));
            lst.Add(new ClsParametros("@idUsuarioOperador",this.IdUsuarioAlta));

            //Parametro de salida
            lst.Add(new ClsParametros("@mensaje", SqlDbType.VarChar, 50));
            CLSManejador.Ejecutar_sp("OperaCaja_create", lst);

            //Regresar el valor almacenado en el parametro de salida
            mensaje = lst[3].Valor.ToString();

            return (mensaje);
        }
    }
}
