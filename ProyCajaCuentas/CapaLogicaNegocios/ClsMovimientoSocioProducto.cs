using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaAccesoDatos;
using System.Data;

namespace CapaLogicaNegocios
{
    public class ClsMovimientoSocioProducto
    {
        //--------properties
        public int IdSocio { set; get; }
        public int IdProducto { set; get; }
        public decimal Cantidad { set; get; }
        public DateTime FechaMovimiento { set; get; }
        public int IdCuentaPorPagar { set; get;  }
        public int IdUsuario { set; get;  }
        public int Id { set; get; }
        public int IdUsuarioAlta { set; get; }
        public DateTime FechaAlta { set; get; }
        public int IdUsuarioModifico { set; get; }
        public DateTime FechaModificacion { set; get; }

        public ClsManejador CLSManejador { set; get; }

        //-----------Constructor
        public ClsMovimientoSocioProducto()
        {
            this.IdSocio = 0;
            this.IdProducto = 0;
            this.Cantidad = 0.0M;
            this.FechaMovimiento = new DateTime();
            this.IdCuentaPorPagar = 0;
            this.IdUsuario = 0;
            this.Id = 0;
            this.IdUsuarioAlta = 0;
            this.FechaAlta = new DateTime();
            this.IdUsuarioModifico = 0;
            this.FechaModificacion = new DateTime();

            this.CLSManejador = new ClsManejador();
        }

        //-------------Methods
        public string MovimientoSocioProducto_create()
        {
            string mensaje = "";
            List<ClsParametros> lst = new List<ClsParametros>();

            //Parametros de entrada
            lst.Add(new ClsParametros("@idSocio", this.IdSocio ));
            lst.Add(new ClsParametros("@idProducto",this.IdProducto ));
            lst.Add(new ClsParametros("@idCuentaPorPagar", this.IdCuentaPorPagar));
            lst.Add(new ClsParametros("@idUsuario",this.IdUsuario ));
            lst.Add(new ClsParametros("@cantidad",this.Cantidad ));
            lst.Add(new ClsParametros("@fechaMovimiento",this.FechaMovimiento ));
            lst.Add(new ClsParametros("@idUsuarioOperador",this.IdUsuario ));

            //Parametro de salida
            lst.Add(new ClsParametros("@mensaje", SqlDbType.VarChar, 50));
            CLSManejador.Ejecutar_sp("MovimientoSocioProducto_create", lst);

            //Regresar el valor almacenado en el parametro de salida
            mensaje = lst[6].Valor.ToString();

            return (mensaje);
        }
    }
}
