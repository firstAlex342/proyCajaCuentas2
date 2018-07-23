﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CapaAccesoDatos;


namespace CapaLogicaNegocios
{
    /*Esta clase se usa para invocar un SP, dicho sp 
     * inserta info en 3 tablas
   */

    public class ClsPagoBasico
    {
        public int IdCaja { set; get; }
        public int IdSocio { set; get; }
        public decimal TotalPagado { set; get; }
        public int IdUsuarioOperador { set; get; }

        public DataTable ListaProductosAPagar { set; get; }
        public ClsManejador CLSManejador { set; get; }

        //-----------Constructor
        public ClsPagoBasico()
        {
            this.CLSManejador = new ClsManejador();
            this.ListaProductosAPagar = MakeTable();

        }
        
        //---------------------Methods

        public string MovsEnCaja_PagoProducto_DetallesProductosEnPago_create()
        {
            string mensaje = "";
            List<ClsParametros> lst = new List<ClsParametros>();

            //Parametros de entrada
            lst.Add(new ClsParametros("@idCaja",this.IdCaja ));
            lst.Add(new ClsParametros("@idSocio", this.IdSocio));
            lst.Add(new ClsParametros("@totalPagado", this.TotalPagado ));
            lst.Add(new ClsParametros("@productosAPagar",this.ListaProductosAPagar));
            lst.Add(new ClsParametros("@idUsuarioOperador", this.IdUsuarioOperador));


            //Parametro de salida
            lst.Add(new ClsParametros("@mensaje", SqlDbType.VarChar, 50));
            CLSManejador.Ejecutar_sp("MovsEnCaja_PagoProducto_DetallesProductosEnPago_create", lst);

            //Regresar el valor almacenado en el parametro de salida
            mensaje = lst[5].Valor.ToString();

            return (mensaje);
        }


        public void AddProductoAPagar(int idProducto, decimal tarifaElegida, string tipoDescuento, decimal cantidadDescuento, decimal cantidadPagada)
        {
            this.ListaProductosAPagar.Rows.Add(idProducto, tarifaElegida, tipoDescuento, cantidadDescuento, cantidadPagada);
        }

        private DataTable MakeTable()
        {
            DataTable tabla = new DataTable();
            tabla.Columns.Add("idProducto", typeof(int));
            tabla.Columns.Add("tarifaElegida", typeof(decimal));
            tabla.Columns.Add("tipoDescuento", typeof(string));
            tabla.Columns.Add("cantidadDescuento", typeof(decimal));
            tabla.Columns.Add("cantidadPagada", typeof(decimal));
            return (tabla);
        }

    }
}
