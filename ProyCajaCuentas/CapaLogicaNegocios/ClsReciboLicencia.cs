﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CapaAccesoDatos;

namespace CapaLogicaNegocios
{
    public class ClsReciboLicencia
    {
        //----------properties
        public string Folio { set; get; }
        public int IdUsuarioAlta { set; get; }
        public DateTime FechaAlta { set; get; }
        public int IdUsuarioModifico { set; get; }
        public DateTime FechaModificacion { set; get; }
        public bool Activo { set; get; }
        public ClsManejador CLSManejador { set; get; }

        //-------constructor
        public ClsReciboLicencia()
        {
            this.Folio = String.Empty;
            this.IdUsuarioAlta = 0;
            this.FechaAlta = new DateTime();
            this.IdUsuarioModifico = 0;
            this.FechaModificacion = new DateTime();
            this.Activo = true;
            this.CLSManejador = new ClsManejador();
        }//parameterless constructor


        //-------------Methods
        public DataTable ReciboLicencia_BuscarFolio()
        {
            List<ClsParametros> lst = new List<ClsParametros>();
            lst.Add(new ClsParametros("@folioBuscado",this.Folio ));  

            return (CLSManejador.Listado("ReciboLicencia_BuscarFolio", lst));
        }
    }
}
