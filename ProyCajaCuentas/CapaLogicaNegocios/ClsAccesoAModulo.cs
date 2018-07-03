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

        public ClsManejador CLSManejador { set; get; }

        //--------Constructor
        public ClsAccesoAModulo()
        {
            this.IdUsuario = 0;
            this.IdModulo = 0;
            this.CLSManejador = new ClsManejador();
        }


        //----------Methods
        public DataTable AccesoAModulo_Modulo_InnerJoin()
        {
            List<ClsParametros> lst = new List<ClsParametros>();
            lst.Add(new ClsParametros("@idUsuarioBuscado", this.IdUsuario));

            return (CLSManejador.Listado("AccesoAModulo_Modulo_InnerJoin", lst));
        }
        
    }
}
