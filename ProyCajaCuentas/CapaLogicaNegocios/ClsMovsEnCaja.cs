using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CapaAccesoDatos;

namespace CapaLogicaNegocios
{
    public class ClsMovsEnCaja
    {
        public int IdMovimiento { set; get; }
        public int IdCaja { set; get; }
        public string TipoMovimiento { set; get; }
        public decimal Cantidad { set; get; }

        public int IdUsuarioAlta { set; get; }
        public DateTime FechaAlta { set; get; }
        public int IdUsuarioModififico { set; get; }
        public DateTime FechaModificacion { set; get; }
        public bool Activo { set; get; }

        public ClsManejador CLSManejador { set; get; }


        //----------------Constructor
        public ClsMovsEnCaja()
        {
            this.IdMovimiento = 0;
            this.IdCaja = 0;
            this.TipoMovimiento = "";
            this.Cantidad = 0.0M;

            this.IdUsuarioAlta = 0;
            this.FechaAlta = new DateTime();
            this.IdUsuarioModififico = 0;
            this.FechaModificacion = new DateTime();
            this.Activo = false;

            this.CLSManejador = new ClsManejador();
        }

        //-------------------Methods
        public DataTable MovsEnCaja_BuscarDetallesDeMovimiento()
        {
            List<ClsParametros> lst = new List<ClsParametros>();
            lst.Add(new ClsParametros("@idMovimientoBuscado", this.IdMovimiento));

            return (CLSManejador.Listado("MovsEnCaja_BuscarDetallesDeMovimiento", lst));
        }

        public DataTable MovsEnCaja_BuscarReciboLicenciaXIdMovimiento()
        {
            List<ClsParametros> lst = new List<ClsParametros>();
            lst.Add(new ClsParametros("@idMovimientoBuscado", this.IdMovimiento));

            return (CLSManejador.Listado("MovsEnCaja_BuscarReciboLicenciaXIdMovimiento", lst));
        }
    }
}
