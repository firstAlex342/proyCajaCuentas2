using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CapaAccesoDatos;

namespace CapaLogicaNegocios
{
    public class ClsProveedorProveeElementoElementoProveido
    {
        //--------------Properties
        public int IdProveedor { set; get; }
        public string NombreElemento { set; get; }
        public int IdUsuarioAlta { set; get; }

        public ClsManejador CLSManejador { set; get; }

        //---------------Constructor
        public ClsProveedorProveeElementoElementoProveido()
        {
            this.IdProveedor = 0;
            this.NombreElemento = "";
            this.IdUsuarioAlta = 0;

            this.CLSManejador = new ClsManejador();
        }//parameterless constructor

        //----------------Methods
        public string Proveedor_ProveeElemento_ElementoProveido_create()
        {
            string mensaje = "";

            List<ClsParametros> lst = new List<ClsParametros>();

            //Parametros de entrada
            lst.Add(new ClsParametros("@nombreElemento", this.NombreElemento));
            lst.Add(new ClsParametros("@idProveedor", this.IdProveedor));
            lst.Add(new ClsParametros("@idUsuarioOperador", this.IdUsuarioAlta));

            //Parametro de salida
            lst.Add(new ClsParametros("@mensaje", SqlDbType.VarChar, 60));

            CLSManejador.Ejecutar_sp("Proveedor_ProveeElemento_ElementoProveido_create", lst);

            //Regresar el valor almacenado en el parametro de salida
            mensaje = lst[3].Valor.ToString();

            return (mensaje);
        }
    }
}
