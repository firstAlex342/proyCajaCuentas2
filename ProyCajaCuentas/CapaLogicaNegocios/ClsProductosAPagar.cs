using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CapaAccesoDatos;

namespace CapaLogicaNegocios
{
    public class ClsProductosAPagar
    {
        //------------Properties
        public int IdCuenta { set; get;  }
        public decimal Limite { set; get; }
        public int IdProducto { set; get; }
        public int IdUsuarioAlta { set; get; }
        public DateTime FechaAlta { set; get; }
        public int IdUsuarioModifico { set; get; }
        public DateTime FechaModificacion { set; get; }

        private ClsManejador CLSManejador { set; get; }


        //----------------Constructor
        public ClsProductosAPagar()
        {
            this.IdCuenta = 0;
            this.Limite = 0.0M;
            this.IdProducto = 0;
            this.IdUsuarioAlta = 0;
            this.FechaAlta = new DateTime();
            this.IdUsuarioModifico = 0;
            this.FechaModificacion = new DateTime();

            this.CLSManejador = new ClsManejador();
        }



        public string ProductosAPagar_create()
        {
            string mensaje = "";
            List<ClsParametros> lst = new List<ClsParametros>();

            //Parametros de entrada
            lst.Add(new ClsParametros("@idCuentaPorPagar",this.IdCuenta ));
            lst.Add(new ClsParametros("@idProducto",this.IdProducto ));
            lst.Add(new ClsParametros("@limite",this.Limite ));
            lst.Add(new ClsParametros("@idUsuarioOperador",this.IdUsuarioAlta ));

            //Parametro de salida
            lst.Add(new ClsParametros("@mensaje", SqlDbType.VarChar, 50));
            CLSManejador.Ejecutar_sp("ProductosAPagar_create", lst);

            //Regresar el valor almacenado en el parametro de salida
            mensaje = lst[4].Valor.ToString();

            return (mensaje);
        }

        public DataTable ProductosAPagar_BuscarDetallesProductoXIdCuenta()
        {
            List<ClsParametros> lst = new List<ClsParametros>();
            lst.Add(new ClsParametros("@idCuentaBuscada", this.IdCuenta));


            return CLSManejador.Listado("ProductosAPagar_BuscarDetallesProductoXIdCuenta", lst);
        }
    }
}
