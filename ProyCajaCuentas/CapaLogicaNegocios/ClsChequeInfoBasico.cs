using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CapaAccesoDatos;

namespace CapaLogicaNegocios
{
    public class ClsChequeInfoBasico
    {
        private int idCheque;
        private string numCheque;
        private string beneficiario;
        private decimal cantidad;
        private DateTime fechaDeCheque;
        private DateTime fechaDeCobro;
        private bool usarEnCalculosReporteEgresosIngresos;
        private int idUsuarioOperador;

        private DataTable listaConceptosEnCheque;
        private ClsManejador clsManejador;

        //-----------------Constructor
        public ClsChequeInfoBasico()
        {
            this.IdCheque = 0;
            this.NumCheque = String.Empty;
            this.Beneficiario = String.Empty;
            this.Cantidad = 0.0m;
            this.FechaDeCheque = new DateTime();
            this.FechaDeCobro = new DateTime();
            this.UsarEnCalculosReporteEgresosIngresos = true;
            this.ListaConceptosEnCheque = MakeDataTable();
            this.IdUsuarioOperador = 0;

            this.CLSManejador = new ClsManejador();
        }//parameterless constructor

        //-----------------Methods
        public string Cheque_DescripcionDeCheque_ConceptoEnCheque_create()
        {
            string mensaje = "";
            List<ClsParametros> lst = new List<ClsParametros>();

            //Parametros de entrada
            lst.Add(new ClsParametros("@numCheque", this.NumCheque));
            lst.Add(new ClsParametros("@beneficiario", this.Beneficiario));
            lst.Add(new ClsParametros("@cantidad", this.Cantidad));
            lst.Add(new ClsParametros("@fechaDeCheque", this.FechaDeCheque));
            lst.Add(new ClsParametros("@fechaDeCobro", this.FechaDeCobro));
            lst.Add(new ClsParametros("@conceptosEnCheque", this.ListaConceptosEnCheque));
            lst.Add(new ClsParametros("@idUsuarioOperador", this.IdUsuarioOperador ));


            //Parametro de salida
            lst.Add(new ClsParametros("@mensaje", SqlDbType.VarChar, 50));
            CLSManejador.Ejecutar_sp("Cheque_DescripcionDeCheque_ConceptoEnCheque_create", lst);

            //Regresar el valor almacenado en el parametro de salida
            mensaje = lst[7].Valor.ToString();

            return (mensaje);
        }

        public string Cheque__DescripcionDeCheque_ConceptoEnCheque_Update()
        {
            string mensaje = "";
            List<ClsParametros> lst = new List<ClsParametros>();

            //Parametros de entrada
            lst.Add(new ClsParametros("@idChequeOriginal", this.IdCheque));
            lst.Add(new ClsParametros("@numChequeAActualizar", this.NumCheque));
            lst.Add(new ClsParametros("@beneficiario", this.Beneficiario));
            lst.Add(new ClsParametros("@cantidad", this.Cantidad));
            lst.Add(new ClsParametros("@fechaDeCheque", this.FechaDeCheque));
            lst.Add(new ClsParametros("@fechaDeCobro", this.FechaDeCobro));
            lst.Add(new ClsParametros("@usarEnCalculosReporteEgresosIngresos", this.UsarEnCalculosReporteEgresosIngresos));
            lst.Add(new ClsParametros("@conceptosEnCheque", this.ListaConceptosEnCheque));
            lst.Add(new ClsParametros("@idUsuarioOperador", this.IdUsuarioOperador));


            //Parametro de salida
            lst.Add(new ClsParametros("@mensaje", SqlDbType.VarChar, 50));
            CLSManejador.Ejecutar_sp("Cheque__DescripcionDeCheque_ConceptoEnCheque_Update", lst);

            //Regresar el valor almacenado en el parametro de salida
            mensaje = lst[9].Valor.ToString();

            return (mensaje);
        }



        private DataTable MakeDataTable()
        {
            DataTable tabla = new DataTable();
            tabla.Columns.Add("NomProveedor", typeof(string));
            tabla.Columns.Add("NomConcepto", typeof(string));
            tabla.Columns.Add("Factura", typeof(string));
            tabla.Columns.Add("Importe", typeof(decimal));

            return (tabla);
        }

        public void AddConceptoAListaConceptosEnCheque(string nomProveedor, string nomConcepto, string factura, decimal importe)
        {
            this.ListaConceptosEnCheque.Rows.Add(nomProveedor, nomConcepto, factura, importe);
        }

        //---------------Properties
        public int IdCheque
        {
            set { idCheque = value; }
            get { return idCheque; }
        }

        public string NumCheque
        {
            set { numCheque = value; }
            get { return numCheque; }
        }

        public string Beneficiario
        {
            set { beneficiario = value; }
            get { return beneficiario; }
        }

        public decimal Cantidad
        {
            set { cantidad = value; }
            get { return cantidad; }
        }

        public DateTime FechaDeCheque
        {
            set { fechaDeCheque = value; }
            get { return fechaDeCheque; }
        }

        public DateTime FechaDeCobro
        {
            set { fechaDeCobro = value; }
            get { return fechaDeCobro; }
        }

        public DataTable ListaConceptosEnCheque
        {
            set { listaConceptosEnCheque = value; }
            get { return listaConceptosEnCheque; }
        }

        private ClsManejador CLSManejador
        {
            set { clsManejador = value; }
            get { return clsManejador; }
        }

        public int IdUsuarioOperador
        {
            set { idUsuarioOperador = value; }
            get { return idUsuarioOperador; }
        }

        public bool UsarEnCalculosReporteEgresosIngresos
        {
            set { usarEnCalculosReporteEgresosIngresos = value; }
            get { return usarEnCalculosReporteEgresosIngresos;  }
        }
    }
}
