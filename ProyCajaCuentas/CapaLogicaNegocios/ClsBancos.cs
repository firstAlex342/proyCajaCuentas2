using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CapaAccesoDatos;

namespace CapaLogicaNegocios
{
    public class ClsBancos
    {
        public int Id { set; get; }
        public string PeriodoMes { set; get; }
        public int PeriodoAnio { set; get; }
        public decimal DisponibleEnBancos { set; get; }
        public int IdUsuarioAlta { set; get; }
        public DateTime FechaAlta { set; get; }
        public int IdUsuarioModifico { set; get; }
        public DateTime FechaModificacion { set; get; }
        public bool Activo { set; get; }

        private ClsManejador CLSManejador { set; get; }

        //-----------Constructor
        public ClsBancos()
        {
            this.Id = 0;
            this.PeriodoMes = "";
            this.PeriodoAnio = 0;
            this.DisponibleEnBancos = 0.0m;
            this.IdUsuarioAlta = 0;
            this.FechaAlta = new DateTime();
            this.IdUsuarioModifico = 0;
            this.FechaModificacion = new DateTime();
            this.Activo = false;

            this.CLSManejador = new ClsManejador();
        }

        //-------------------Methods
        public string Bancos_create()
        {
            string mensaje = "";
            List<ClsParametros> lst = new List<ClsParametros>();

            //Parametros de entrada
            lst.Add(new ClsParametros("@mesPeriodo", this.PeriodoMes));
            lst.Add(new ClsParametros("@anioPeriodo", this.PeriodoAnio));
            lst.Add(new ClsParametros("@disponibleEnBancos", this.DisponibleEnBancos));
            lst.Add(new ClsParametros("@idUsuarioOperador", this.IdUsuarioAlta));


            //Parametro de salida
            lst.Add(new ClsParametros("@mensaje", SqlDbType.VarChar, 50));
            CLSManejador.Ejecutar_sp("Bancos_create", lst);

            //Regresar el valor almacenado en el parametro de salida
            mensaje = lst[4].Valor.ToString();

            return (mensaje);
        }

        public DataTable Bancos_SelectActivos()
        {
            List<ClsParametros> lst = new List<ClsParametros>();
            lst.Add(new ClsParametros("@parametroNoNecesario", true));

            return (CLSManejador.Listado("Bancos_SelectActivos", lst));
        }

        public string Bancos_Update_DisponibleEnBancos()
        {
            string mensaje = "";
            List<ClsParametros> lst = new List<ClsParametros>();

            //Parametros de entrada
            lst.Add(new ClsParametros("@idBuscado", this.Id));
            lst.Add(new ClsParametros("@newMesPeriodo", this.PeriodoMes));
            lst.Add(new ClsParametros("@newAnioPeriodo", this.PeriodoAnio));
            lst.Add(new ClsParametros("@newDisponibleEnBancos", this.DisponibleEnBancos));
            lst.Add(new ClsParametros("@idUsuarioOperador", this.IdUsuarioModifico));

            //Parametro de salida
            lst.Add(new ClsParametros("@mensaje", SqlDbType.VarChar, 50));
            CLSManejador.Ejecutar_sp("Bancos_Update_DisponibleEnBancos", lst);

            //Regresar el valor almacenado en el parametro de salida
            mensaje = lst[5].Valor.ToString();

            return (mensaje);
        }
    }
}
