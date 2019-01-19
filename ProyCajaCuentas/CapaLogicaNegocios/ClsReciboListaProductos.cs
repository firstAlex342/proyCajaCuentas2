using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CapaAccesoDatos;

namespace CapaLogicaNegocios
{
    public class ClsReciboListaProductos
    {
        //----------properties
        public string Folio { set; get; }
        public int IdUsuarioAlta { set; get; }
        public DateTime FechaAlta { set; get; }
        public int IdUsuarioModifico { set; get; }
        public DateTime FechaModificacion { set; get; }
        public bool Activo { set; get; }
        public ClsManejador CLSManejador { set; get; }

        //----------------constructor
        public ClsReciboListaProductos()
        {
            this.Folio = String.Empty;
            this.IdUsuarioAlta = 0;
            this.FechaAlta = new DateTime();
            this.IdUsuarioModifico = 0;
            this.FechaModificacion = new DateTime();
            this.Activo = true;
            this.CLSManejador = new ClsManejador();
        }

        //---------------methods
        public DataTable ReciboListaProductos_BuscarFolio()
        {
            List<ClsParametros> lst = new List<ClsParametros>();
            lst.Add(new ClsParametros("@folioBuscado", this.Folio));

            return (CLSManejador.Listado("ReciboListaProductos_BuscarFolio", lst));
        }

        public string ReciboListaProductos_UpdateActivoACero()
        {
            string mensaje = "";
            List<ClsParametros> lst = new List<ClsParametros>();

            //Parametros de entrada
            lst.Add(new ClsParametros("@folioBuscado", this.Folio));
            lst.Add(new ClsParametros("@idUsuarioModifico", this.IdUsuarioModifico));


            //Parametro de salida
            lst.Add(new ClsParametros("@mensaje", SqlDbType.VarChar, 50));
            CLSManejador.Ejecutar_sp("ReciboListaProductos_UpdateActivoACero", lst);

            //Regresar el valor almacenado en el parametro de salida
            mensaje = lst[2].Valor.ToString();

            return (mensaje);
        }

    }
}
