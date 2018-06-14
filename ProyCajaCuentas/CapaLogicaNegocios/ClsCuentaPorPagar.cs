using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CapaAccesoDatos;

namespace CapaLogicaNegocios
{
    public class ClsCuentaPorPagar
    {
        //-----------Properties
        public int Id { set; get; }
        public DateTime FechaVencimiento { set; get; }
        public int IdUsuarioAlta { set; get; }
        public DateTime FechaAlta { set; get; }
        public int IdUsuarioModifico { set; get; }
        public DateTime FechaModificacion { set; get; }

        public ClsManejador CLSManejador { set; get; }


        //--------------Constructor
        public ClsCuentaPorPagar()
        {
            this.Id = 0;
            this.FechaVencimiento = new DateTime();
            this.IdUsuarioAlta = 0;
            this.FechaAlta = new DateTime();
            this.IdUsuarioModifico = 0;
            this.FechaModificacion = new DateTime();
            this.CLSManejador = new ClsManejador();
        }


        //------------Methods
        public DataTable CuentaPorPagar_BuscarXId()
        {
            List<ClsParametros> lst = new List<ClsParametros>();
            lst.Add(new ClsParametros("@idBuscado", this.Id));


            return (CLSManejador.Listado("CuentaPorPagar_BuscarXId", lst));
        }

        public DataTable CuentaPorPagar_create2()
        {
            List<ClsParametros> lst = new List<ClsParametros>();

            //Parametros de entrada
            lst.Add(new ClsParametros("@fechaVencimiento", this.FechaVencimiento));
            lst.Add(new ClsParametros("@idUsuarioOperador", this.IdUsuarioAlta));

            return (CLSManejador.Listado("CuentaPorPagar_create2", lst));
        }

    }
}
