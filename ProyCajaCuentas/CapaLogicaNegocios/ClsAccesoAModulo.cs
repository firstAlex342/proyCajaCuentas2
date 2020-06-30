using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CapaAccesoDatos;

namespace CapaLogicaNegocios
{
    public class ClsAccesoAModulo
    {
        //---------properties
        public int IdUsuario { set; get; }
        public int IdModulo { set; get; }
        public bool Activo { set; get; }

        public ClsManejador CLSManejador { set; get; }

        //--------Constructor
        public ClsAccesoAModulo()
        {
            this.IdUsuario = 0;
            this.IdModulo = 0;
            this.Activo = true;
            this.CLSManejador = new ClsManejador();
        }


        //----------Methods
        public DataTable AccesoAModulo_Modulo_InnerJoin()
        {
            List<ClsParametros> lst = new List<ClsParametros>();
            lst.Add(new ClsParametros("@idUsuarioBuscado", this.IdUsuario));

            return (CLSManejador.Listado("AccesoAModulo_Modulo_InnerJoin", lst));
        }

        public string AccesoAModulo_create()
        {
            string mensaje = "";
            List<ClsParametros> lst = new List<ClsParametros>();

            //Parametros de entrada
            lst.Add(new ClsParametros("@IdUsuario", this.IdUsuario));
            lst.Add(new ClsParametros("@IdModulo", this.IdModulo));


            //Parametro de salida
            lst.Add(new ClsParametros("@mensaje", SqlDbType.VarChar, 50));
            CLSManejador.Ejecutar_sp("AccesoAModulo_create", lst);

            //Regresar el valor almacenado en el parametro de salida
            mensaje = lst[2].Valor.ToString();

            return (mensaje);
        }

        public string AccesoAModulo_update()
        {

            string mensaje = "";
            List<ClsParametros> lst = new List<ClsParametros>();

            //Parametros de entrada
            lst.Add(new ClsParametros("@idUsuario", this.IdUsuario));
            lst.Add(new ClsParametros("@idModulo", this.IdModulo));
            lst.Add(new ClsParametros("@newEstado", this.Activo));


            //Parametro de salida
            lst.Add(new ClsParametros("@mensaje", SqlDbType.VarChar, 50));
            CLSManejador.Ejecutar_sp("AccesoAModulo_update", lst);

            //Regresar el valor almacenado en el parametro de salida
            mensaje = lst[3].Valor.ToString();

            return (mensaje);
        }

        public string AccesoAModulo_Update_Collection(DataTable listaIdModulosYEstados)
        {
            string mensaje = "";
            List<ClsParametros> lst = new List<ClsParametros>();

            //Parametros de entrada
            lst.Add(new ClsParametros("@idUsuarioAActualizar", this.IdUsuario));
            lst.Add(new ClsParametros("@idModulosYEstado", listaIdModulosYEstados));

            //Parametro de salida
            lst.Add(new ClsParametros("@mensaje", SqlDbType.VarChar, 50));
            CLSManejador.Ejecutar_sp("AccesoAModulo_Update_Collection", lst);

            //Regresar el valor almacenado en el parametro de salida
            mensaje = lst[2].Valor.ToString();

            return (mensaje);
        }

    }
}
