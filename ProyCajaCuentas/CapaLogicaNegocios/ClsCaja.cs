using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CapaAccesoDatos;

namespace CapaLogicaNegocios
{
    public class ClsCaja
    {
        //-----properties
        public int Id { set; get; }
        public int IdUsuarioAlta { set; get; }
        public DateTime FechaAlta { set; get; }
        public int IdUsuarioModifico { set; get; }
        public DateTime FechaModificacion { set; get; }
        public bool Activo { set; get; }

        public ClsManejador CLSManejador { set; get; }


        //---------------Constructor
        public ClsCaja()
        {
            this.Id = 0;
            this.IdUsuarioAlta = 0;
            this.FechaAlta = new DateTime();
            this.IdUsuarioModifico = 0;
            this.FechaModificacion = new DateTime();
            this.Activo = true;

            this.CLSManejador = new ClsManejador();
        }

        //---------------Methods
        public DataTable Caja_create()
        {
            List<ClsParametros> lst = new List<ClsParametros>();

            lst.Add(new ClsParametros("@idUsuarioOperador", this.IdUsuarioAlta));

            //Regresa el SP una unica fila con una unica columna
            DataTable respuesta = CLSManejador.Listado("Caja_create", lst);
            return (respuesta);
        }

    }
}
