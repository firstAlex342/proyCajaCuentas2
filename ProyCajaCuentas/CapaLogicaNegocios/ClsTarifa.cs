using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaAccesoDatos;
using System.Data;

namespace CapaLogicaNegocios
{
    public class ClsTarifa
    {
        public int Id { set; get; }
        public decimal Cantidad { set; get; }
        public int IdUsuarioAlta { set; get; }
        public DateTime FechaAlta { set; get; }
        public int IdUsuarioModifico { set; get; }
        public DateTime FechaModificacion { set; get; }
        public bool Activo { set; get; }

        public ClsManejador CLSManejador { set; get; }

        //--------------Constructor
        public ClsTarifa()
        {
            this.Id = 0;
            this.Cantidad = 0.0M;
            this.IdUsuarioAlta = 0;
            this.FechaAlta = new DateTime();
            this.IdUsuarioModifico = 0;
            this.FechaModificacion = new DateTime();
            this.Activo = true;

            this.CLSManejador = new ClsManejador();
        }

        //------------Methods
        public DataTable Tarifa_create()
        {            
            List<ClsParametros> lst = new List<ClsParametros>();

            lst.Add(new ClsParametros("@cantidad", this.Cantidad ));
            lst.Add(new ClsParametros("@IdUsuarioOperador", this.IdUsuarioAlta ));
            
            //Regresa el SP una unica fila con una unica columna
            DataTable respuesta = CLSManejador.Listado("Tarifa_create", lst);

            return (respuesta);
        }

        public string Tarifa_update()
        {
            string mensaje = "";
            List<ClsParametros> lst = new List<ClsParametros>();

            lst.Add(new ClsParametros("@idTarifaBuscada", this.Id));
            lst.Add(new ClsParametros("@newCantidad", this.Cantidad));
            lst.Add(new ClsParametros("@idUsuarioOperador", this.IdUsuarioModifico));


            //Parametro de salida
            lst.Add(new ClsParametros("@mensaje", SqlDbType.VarChar, 50));
            CLSManejador.Ejecutar_sp("Tarifa_update", lst);

            //Regresar el valor almacenado en el parametro de salida
            mensaje = lst[3].Valor.ToString();

            return (mensaje);
        }
    }
}
