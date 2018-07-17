using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CapaAccesoDatos;

namespace CapaLogicaNegocios
{
    public class ClsModulo
    {
        public int Id { set; get; }
        public string Nombre { set; get; }

        public ClsManejador CLSManejador { set; get; }


        //-----------Constructor
        public ClsModulo()
        {
            this.Id = 0;
            this.Nombre = String.Empty;
            this.CLSManejador = new ClsManejador();
        }

        //----------Methods

        public DataTable Modulo_Select_Id_Nombre_DeTodos()
        {
            List<ClsParametros> lst = new List<ClsParametros>();
            lst.Add(new ClsParametros("@parametroNoNecesario", false));


            return CLSManejador.Listado("Modulo_Select_Id_Nombre_DeTodos", lst);
        }
    }
}
