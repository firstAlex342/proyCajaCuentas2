using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CapaAccesoDatos;


namespace CapaLogicaNegocios
{
    public class ClsPagoProducto
    {
        public int Id { set; get; }
        public int IdSocio { set; get; }
        public int IdProducto { set; get; }
        public decimal CantidadPagada { set; get; }
        public string TipoDescuento { set; get; }
        public decimal CantidadDescuento { set; get; }
        public int IdUsuarioAlta { set; get; }
        public DateTime FechaAlta { set; get; }
        public int IdUsuarioModififico { set; get; }
        public DateTime FechaModificacion { set; get; }
        public bool Activo { set; get; }

        public ClsManejador CLSManejador { set; get; }

        //-----------------Constructor
        public ClsPagoProducto()
        {
            this.Id = 0;
            this.IdSocio = 0;
            this.IdProducto = 0;
            this.CantidadPagada = 0.0M;
            this.TipoDescuento = String.Empty;
            this.CantidadDescuento = 0.0M;
            this.IdUsuarioAlta = 0;
            this.FechaAlta = new DateTime();
            this.IdUsuarioModififico = 0;
            this.FechaModificacion = new DateTime();
            this.Activo = true;

            this.CLSManejador = new ClsManejador();
        }

        //--------------------Methods
        public string PagoProducto_create()
        {
            string mensaje;
            List<ClsParametros> lst = new List<ClsParametros>();

            lst.Add(new ClsParametros("@idSocio",this.IdSocio ));
            lst.Add(new ClsParametros("@idProducto", this.IdProducto));
            lst.Add(new ClsParametros("@cantidadPagada", this.CantidadPagada));
            lst.Add(new ClsParametros("@tipoDescuento",this.TipoDescuento));
            lst.Add(new ClsParametros("@cantidadDescuento", this.CantidadDescuento));
            lst.Add(new ClsParametros("@idUsuarioOperador", this.IdUsuarioAlta));

            //Parametro de salida
            lst.Add(new ClsParametros("@mensaje", SqlDbType.VarChar, 50));
            CLSManejador.Ejecutar_sp("PagoProducto_create", lst);

            //Regresar el valor almacenado en el parametro de salida
            mensaje = lst[6].Valor.ToString();

            return (mensaje);
        }

        public DataTable PagoProducto_BuscarXMesAlta()
        {
            List<ClsParametros> lst = new List<ClsParametros>();
            lst.Add(new ClsParametros("@MesBuscado", this.FechaAlta.Month));


            return (CLSManejador.Listado("PagoProducto_BuscarXMesAlta", lst));
        }
    }

}

