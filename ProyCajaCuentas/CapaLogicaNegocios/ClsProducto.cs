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
        public bool Activo { set; get; }

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
            this.Activo = true;
            this.CLSManejador = new ClsManejador();
        }

        //--------------Methods
        public DataTable Producto_Select_Id_Nombre_DeTodos()
        {
            List<ClsParametros> lst = new List<ClsParametros>();
            lst.Add(new ClsParametros("@parametroNoNecesario", false));


            return CLSManejador.Listado("Producto_Select_Id_Nombre_DeTodos", lst);
        }

        public DataTable Producto_create()
        {
            
            List<ClsParametros> lst = new List<ClsParametros>();

            lst.Add(new ClsParametros("@Nombre", this.Nombre));
            lst.Add(new ClsParametros("@Descripcion", this.Descripcion));
            lst.Add(new ClsParametros("@IdUsuarioOperador", this.IdUsuarioAlta));

            //el sp regresa una fila con una unica columna.
            DataTable respuesta = CLSManejador.Listado("Producto_create", lst);

            return (respuesta);
        }

        public DataTable Producto_BuscarXId()
        {
            List<ClsParametros> lst = new List<ClsParametros>();
            lst.Add(new ClsParametros("@idBuscado", this.Id));

            return CLSManejador.Listado("Producto_BuscarXId", lst);
        }

        public string Producto_update()
        {
            string mensaje = "";
            List<ClsParametros> lst = new List<ClsParametros>();

            lst.Add(new ClsParametros("@idProductoBuscado", this.Id));
            lst.Add(new ClsParametros("@NombreNew", this.Nombre));
            lst.Add(new ClsParametros("@DescripcionNew", this.Descripcion));
            lst.Add(new ClsParametros("@IdUsuarioOperador", this.IdUsuarioModifico));

            //Parametro de salida
            lst.Add(new ClsParametros("@mensaje", SqlDbType.VarChar, 50));
            CLSManejador.Ejecutar_sp("Producto_update", lst);

            //Regresar el valor almacenado en el parametro de salida
            mensaje = lst[4].Valor.ToString();

            return (mensaje);
        }

    }
}
