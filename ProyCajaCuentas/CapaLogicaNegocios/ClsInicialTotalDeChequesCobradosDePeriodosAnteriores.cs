using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CapaAccesoDatos;

namespace CapaLogicaNegocios
{
    public class ClsInicialTotalDeChequesCobradosDePeriodosAnteriores
    {
        //--------------Properties
        public int Id { set; get; }
        public decimal Total { set; get; }
        public int IdUsuarioAlta { set; get; }
        public DateTime FechaAlta { set; get; }
        public int IdUsuarioModifico { set; get; }
        public DateTime FechaModificacion { set; get; }
        public bool Activo { set; get; }
        public ClsManejador CLSManejador { set; get; }


        //--------------------Constructor
        public ClsInicialTotalDeChequesCobradosDePeriodosAnteriores()
        {
            this.Id = 0;
            this.Total = 0.0m;
            this.IdUsuarioAlta = 0;
            this.FechaAlta = DateTime.MinValue;
            this.IdUsuarioModifico = 0;
            this.FechaModificacion = DateTime.MinValue;
            this.Activo = false;

            this.CLSManejador = new ClsManejador();
        }

        //----------------------Methods
        public DataTable InicialTotalDeChequesCobradosDePeriodosAnteriores_BuscarActivo()
        {
            List<ClsParametros> lst = new List<ClsParametros>();
            lst.Add(new ClsParametros("@parametroNoNecesario", 1));

            return CLSManejador.Listado("InicialTotalDeChequesCobradosDePeriodosAnteriores_BuscarActivo", lst);
        }

        public string InicialTotalDeChequesCobradosDePeriodosAnteriores_Update()
        {           
            string mensaje = "";
            List<ClsParametros> lst = new List<ClsParametros>();

            //Parametros de entrada
            lst.Add(new ClsParametros("@total", this.Total));
            lst.Add(new ClsParametros("@idUsuarioOperador", this.IdUsuarioModifico));

            //Parametro de salida
            lst.Add(new ClsParametros("@mensaje", SqlDbType.VarChar, 50));
            CLSManejador.Ejecutar_sp("InicialTotalDeChequesCobradosDePeriodosAnteriores_update", lst);

            //Regresar el valor almacenado en el parametro de salida
            mensaje = lst[2].Valor.ToString();

            return (mensaje);          
        }

    }
}
