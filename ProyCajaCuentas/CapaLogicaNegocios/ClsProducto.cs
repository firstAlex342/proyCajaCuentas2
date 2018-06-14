using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CapaAccesoDatos;

namespace CapaLogicaNegocios
{
    public class ClsProducto
    {
        //-----------Properties
        public int Id { set; get; }
        public string Nombre { set; get; }
        public string Descripcion { set; get;  }
        public int IdUsuarioAlta { set; get; }
        public DateTime FechaAlta { set; get; }
        public int IdUsuarioModifico { set; get; }
        public DateTime FechaModificacion { set; get; }

        public ClsManejador CLSManejador { set; get; }


        //---------------Constructor
        public ClsProducto()
        {
            this.Id = 0;
            this.Nombre = String.Empty;
            this.Descripcion = String.Empty;
            this.IdUsuarioAlta = 0;
            this.FechaAlta = new DateTime();
            this.IdUsuarioModifico = 0;
            this.FechaModificacion = new DateTime();
            this.CLSManejador = new ClsManejador();
        }

        //--------------Methods
        public DataTable Producto_Select_Id_Nombre_DeTodos()
        {
            List<ClsParametros> lst = new List<ClsParametros>();
            lst.Add(new ClsParametros("@parametroNoNecesario", false));


            return CLSManejador.Listado("Producto_Select_Id_Nombre_DeTodos", lst);
        }
    }
}
