using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CapaAccesoDatos;

namespace CapaLogicaNegocios
{
    public class ClsProveedorProveeElemento
    { 
       //----------Properties
        public int IdProveedor { set; get; }
        public int IdElementoProveido { set; get; }
        public int IdUsuarioAlta { set; get; }
        public DateTime FechaAlta { set; get; }
        public int IdUsuarioModifico { set; get; }
        public DateTime FechaModificacion { set; get; }
        public bool Activo { set; get; }
        public ClsManejador CLSManejador { set; get; }

        //---------Constructor
        public ClsProveedorProveeElemento()
        {
            this.IdProveedor = 0;
            this.IdElementoProveido = 0;
            this.IdUsuarioAlta = 0;
            this.FechaAlta = DateTime.MinValue;
            this.IdUsuarioModifico = 0;
            this.FechaModificacion = DateTime.MinValue;
            this.Activo = false;
            this.CLSManejador = new ClsManejador();
        }

        //--------------Methods
        public string ProveedorProveeElemento_Update_ElementoProveido_ActivoACero()
        {
            string mensaje = "";
            List<ClsParametros> lst = new List<ClsParametros>();

            lst.Add(new ClsParametros("@idProveedor", this.IdProveedor));
            lst.Add(new ClsParametros("@idElementoProveido", this.IdElementoProveido));
            lst.Add(new ClsParametros("@idUsuarioOperador", this.IdUsuarioModifico));


            //Parametro de salida
            lst.Add(new ClsParametros("@mensaje", SqlDbType.VarChar, 50));
            CLSManejador.Ejecutar_sp("ProveedorProveeElemento_Update_ElementoProveido_ActivoACero", lst);

            //Regresar el valor almacenado en el parametro de salida
            mensaje = lst[3].Valor.ToString();

            return (mensaje);
        }



    }
}
