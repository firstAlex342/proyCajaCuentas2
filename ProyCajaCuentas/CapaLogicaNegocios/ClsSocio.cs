using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaAccesoDatos;
using System.Data;

namespace CapaLogicaNegocios
{
    public class ClsSocio
    {
        //--------properties
        public int Id { set; get; }
        public string NumeroLicencia { set; get; }
        public string NombreComercial { set; get; }
        public string DireccionSupmza { set; get; }
        public string DireccionManzana { set; get; }
        public string DireccionLote { set; get; }
        public string DireccionCalle { set; get; }
        public string DireccionComplemento { set; get; }
        public string PropietarioPatente { set; get; }
        public string RFCPropietario { set; get; }
        public string Comodatario { set; get; }
        public string RFCComodatario { set; get; }
        public string Telefono { set; get; }
        public string Celular { set; get; }
        public string CorreoElectronico { set; get; }
        public int IdUsuarioAlta { set; get; }
        public DateTime FechaAlta { set; get; }
        public int IdUsuarioModifico { set; get; }
        public DateTime FechaModificacion { set; get; }

        public ClsManejador CLSManejador { set; get; }

        //-------------constructor
        public ClsSocio()
        {
            this.Id = 0;
            this.NumeroLicencia = String.Empty;
            this.NombreComercial = String.Empty;
            this.DireccionSupmza = String.Empty;
            this.DireccionManzana = String.Empty;
            this.DireccionLote = String.Empty;
            this.DireccionCalle = String.Empty;
            this.DireccionComplemento = String.Empty;
            this.PropietarioPatente = String.Empty;
            this.RFCPropietario = String.Empty;
            this.Comodatario = String.Empty;
            this.RFCComodatario = String.Empty;
            this.Telefono = String.Empty;
            this.Celular = String.Empty;
            this.CorreoElectronico = String.Empty;
            this.IdUsuarioAlta = 0;
            this.FechaAlta = new DateTime();
            this.IdUsuarioModifico = 0;
            this.FechaModificacion = new DateTime();

            this.CLSManejador = new ClsManejador();
        }


        //-------------------Methods
        public DataTable Socio_Select_Id_NombreComercial_DeTodos()
        {
            List<ClsParametros> lst = new List<ClsParametros>();
            lst.Add(new ClsParametros("@parametroNoNecesario", false));


            return CLSManejador.Listado("Socio_Select_Id_NombreComercial_DeTodos", lst);
        }


        public string Socio_create()
        {
            string mensaje = "";
            List<ClsParametros> lst = new List<ClsParametros>();

            //Parametros de entrada
            lst.Add(new ClsParametros("@NumeroLicencia", this.NumeroLicencia ));
            lst.Add(new ClsParametros("@NombreComercial", this.NombreComercial ));
            lst.Add(new ClsParametros("@DireccionSupmza", this.DireccionSupmza));
            lst.Add(new ClsParametros("@DireccionManzana", this.DireccionManzana ));
            lst.Add(new ClsParametros("@DireccionLote", this.DireccionLote ));
            lst.Add(new ClsParametros("@DireccionCalle", this.DireccionCalle));
            lst.Add(new ClsParametros("@DireccionComplemento", this.DireccionComplemento));
            lst.Add(new ClsParametros("@PropietarioPatente", this.PropietarioPatente ));
            lst.Add(new ClsParametros("@RFCPropietario", this.RFCPropietario ));
            lst.Add(new ClsParametros("@Comodatario", this.Comodatario));
            lst.Add(new ClsParametros("@RFCComodatario", this.RFCComodatario ));
            lst.Add(new ClsParametros("@Telefono", this.Telefono ));
            lst.Add(new ClsParametros("@Celular", this.Celular));
            lst.Add(new ClsParametros("@CorreoElectronico", this.CorreoElectronico));
            lst.Add(new ClsParametros("@IdUsuarioOperador", this.IdUsuarioAlta));


            //Parametro de salida
            lst.Add(new ClsParametros("@mensaje", SqlDbType.VarChar, 50));
            CLSManejador.Ejecutar_sp("Socio_create", lst);

            //Regresar el valor almacenado en el parametro de salida
            mensaje = lst[15].Valor.ToString();

            return (mensaje);
        }

    }
}
