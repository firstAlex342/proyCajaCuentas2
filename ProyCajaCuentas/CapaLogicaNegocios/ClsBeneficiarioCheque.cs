using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaAccesoDatos;
using System.Data;

namespace CapaLogicaNegocios
{
    public class ClsBeneficiarioCheque
    {
        private int id;
        private string nombre;
        private int idUsuarioAlta;
        private DateTime fechaAlta;
        private int idUsuarioModifico;
        private DateTime fechaModificacion;
        private bool activo;
        private ClsManejador clsManejador;

        //------------Constructor
        public ClsBeneficiarioCheque()
        {
            this.Id = 0;
            this.Nombre = String.Empty;
            this.IdUsuarioAlta = 0;
            this.FechaAlta = new DateTime();
            this.IdUsuarioModifico = 0;
            this.FechaModificacion = new DateTime();
            this.Activo = true;
            this.CLSManejador = new ClsManejador();
        }//parameterless constructor


        //-------------------Methods
        public string BeneficiarioCheque_create()
        {
            string mensaje = "";
            List<ClsParametros> lst = new List<ClsParametros>();

            //Parametros de entrada
            lst.Add(new ClsParametros("@nombreBeneficiario", this.Nombre));
            lst.Add(new ClsParametros("@idUsuarioOperador", this.IdUsuarioAlta));

            //Parametro de salida
            lst.Add(new ClsParametros("@mensaje", SqlDbType.VarChar, 50));
            CLSManejador.Ejecutar_sp("BeneficiarioCheque_create", lst);

            //Regresar el valor almacenado en el parametro de salida
            mensaje = lst[2].Valor.ToString();

            return (mensaje);
        }

        public DataTable BeneficiarioCheque_Select_Activos()
        {
            List<ClsParametros> lst = new List<ClsParametros>();
            lst.Add(new ClsParametros("@parametroNoNecesario", 1));

            return (CLSManejador.Listado("BeneficiarioCheque_Select_Activos", lst));
        }

        public System.String BeneficiarioCheque_UpdateActivoACero()
        {
            string mensaje = "";
            List<ClsParametros> lst = new List<ClsParametros>();

            //Parametros de entrada
            lst.Add(new ClsParametros("@idBeneficiarioCheque", this.Id));
            lst.Add(new ClsParametros("@idUsuarioOperador", this.IdUsuarioModifico));

            //Parametro de salida
            lst.Add(new ClsParametros("@mensaje", SqlDbType.VarChar, 50));
            CLSManejador.Ejecutar_sp("BeneficiarioCheque_UpdateActivoACero", lst);

            //Regresar el valor almacenado en el parametro de salida
            mensaje = lst[2].Valor.ToString();
            return (mensaje);
        }


        //-----------properties
        public int Id
        {
            set { id = value; }
            get { return id; }
        }

        public string Nombre
        {
            set { nombre = value; }
            get { return nombre; }
        }

        public int IdUsuarioAlta
        {
            set { idUsuarioAlta = value; }
            get { return idUsuarioAlta;  }
        }

        public DateTime FechaAlta
        {
            set { fechaAlta = value; }
            get { return fechaAlta; }
        }

        public int IdUsuarioModifico
        {
            set { idUsuarioModifico = value; }
            get { return idUsuarioModifico; }
        }

        public DateTime FechaModificacion
        {
            set { fechaModificacion = value; }
            get { return fechaModificacion; }
        }

        public bool Activo
        {
            set { activo = value; }
            get { return activo; }
        }

        public ClsManejador CLSManejador
        {
            set { clsManejador = value; }
            get { return clsManejador; }
        }
    }
}
