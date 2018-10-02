using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CapaAccesoDatos;

namespace CapaLogicaNegocios
{
    public class ClsCheque
    {
        public int Id { set; get; }
        public string NumCheque { set; get; }
        public string Beneficiario { set; get; }
        public decimal Cantidad { set; get; }
        public DateTime FechaDeCheque { set; get; }
        public DateTime FechaDeCobro { set; get; }
        public DateTime FechaAlta { set; get; }
        public int IdUsuarioAlta { set; get; }
        public DateTime FechaModificacion { set; get; }
        public int IdUsuarioModifico { set; get; }
        public bool Activo { set; get;  }
        public ClsManejador CLSManejador { set; get; }

        //-----------------Constructor
        public ClsCheque()
        {
            this.Id = 0;
            this.NumCheque = String.Empty;
            this.Beneficiario = String.Empty;
            this.Cantidad = 0.0m;
            this.FechaDeCheque = DateTime.MinValue;
            this.FechaDeCobro = DateTime.MinValue;
            this.FechaAlta = DateTime.MinValue;
            this.IdUsuarioAlta = 0;
            this.FechaModificacion = DateTime.MinValue;
            this.IdUsuarioModifico = 0;
            this.Activo = true;

            this.CLSManejador = new ClsManejador();
        }

        public DataTable Cheque_BuscarDetallesCheque()
        {
            List<ClsParametros> lst = new List<ClsParametros>();
            lst.Add(new ClsParametros("@numChequeBuscado", this.NumCheque));

            return CLSManejador.Listado("Cheque_BuscarDetallesCheque", lst);
        }

        public string Cheque_DescripcionDeCheque_ConceptoEnCheque_UpdateActivo()
        {
            string mensaje = "";
            List<ClsParametros> lst = new List<ClsParametros>();

            lst.Add(new ClsParametros("@numCheque", this.NumCheque));
            lst.Add(new ClsParametros("@nuevoValorDeActivo",this.Activo));


            //Parametro de salida
            lst.Add(new ClsParametros("@mensaje", SqlDbType.VarChar, 50));
            CLSManejador.Ejecutar_sp("Cheque_DescripcionDeCheque_ConceptoEnCheque_UpdateActivo", lst);

            //Regresar el valor almacenado en el parametro de salida
            mensaje = lst[2].Valor.ToString();

            return (mensaje);
        }

    }
}
