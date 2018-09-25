using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CapaAccesoDatos;

namespace CapaLogicaNegocios
{
    public class ClsProveedor
    {
        public int Id { set; get; }
        public string Nombre { set; get; }
        public string DireccionSupmza { set; get; }
        public string DireccionManzana { set; get; }
        public string DireccionLote { set; get; }
        public string DireccionCalle { set; get; }
        public string DireccionComplemento { set; get; }
        public string Telefono { set; get; }
        public string Celular { set; get; }
        public string CorreoElectronico { set; get; }
        public int IdUsuarioAlta { set; get; }
        public DateTime FechaAlta { set; get; }
        public int IdUsuarioModifico { set; get; }
        public DateTime FechaModificacion { set; get; }
        public bool Activo { set; get; }

        public ClsManejador CLSManejador { set; get; }

        //--------------constructor
        public ClsProveedor()
        {
            this.Id = 0;
            this.Nombre = String.Empty;
            this.DireccionSupmza = String.Empty;
            this.DireccionManzana = String.Empty;
            this.DireccionLote = String.Empty;
            this.DireccionCalle = String.Empty;
            this.DireccionComplemento = String.Empty;
            this.Telefono = String.Empty;
            this.Celular = String.Empty;
            this.CorreoElectronico = String.Empty;

            this.IdUsuarioAlta = 0;
            this.FechaAlta = DateTime.MinValue;
            this.IdUsuarioModifico = 0;
            this.FechaModificacion = DateTime.MinValue;
            this.Activo = true;

            this.CLSManejador = new ClsManejador();
        }

        //--------------methods
        public DataTable Proveedor_SelectTodosActivos()
        {
            List<ClsParametros> lst = new List<ClsParametros>();
            lst.Add(new ClsParametros("@parametroNoNecesario", 1));

            return (CLSManejador.Listado("Proveedor_SelectTodosActivos", lst));
        }

        public DataTable Proveedor_BuscarXNombreYQueEstenActivos()
        {
            List<ClsParametros> lst = new List<ClsParametros>();
            lst.Add(new ClsParametros("@nombreProveedorBuscado", this.Nombre));

            return (CLSManejador.Listado("Proveedor_BuscarXNombreYQueEstenActivos", lst));
        }

    }
}
