using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using System.Collections;
using CapaLogicaNegocios;
using CapaLogicaNegocios.ConvertNumALetras;

namespace CapaPresentacion
{
    public partial class FrmVisorReporte : Form
    {

        //-----------Constructor
        public FrmVisorReporte(DataRow filaUnicaDatosSocio, DataGridViewRowCollection filasConProductos, string totalAPagar, DataTable infoUsuarioTable)
        {
            InitializeComponent();

            try
            {
                if (GridContieneSoloAfiliacion(filasConProductos))
                {
                   
                    //Crear los 2 recibos, CRListaProductos con solo 1 elemento y CRAfiliacion
                    ArmarReciboListaProductosConSolo1Elemento(filaUnicaDatosSocio, filasConProductos, totalAPagar, infoUsuarioTable);
                    ArmarReciboLicencia(filaUnicaDatosSocio, infoUsuarioTable);
                }

                else if (GridContieneAfiliacionYOtros(filasConProductos))
                {
                    //Crear los 2 recibos, CRListaProductos con todos los elementos del Grid y CRAfiliacion
                    ArmarReciboListaProductosConVariosElementos(filaUnicaDatosSocio, filasConProductos, totalAPagar, infoUsuarioTable);
                    ArmarReciboLicencia(filaUnicaDatosSocio, infoUsuarioTable);
                }

                else if (GridNoContieneAfiliacionSoloOtros(filasConProductos))
                {
                    //Crear 1 recibo, CRListaProductos
                    ArmarReciboListaProductosConVariosElementos(filaUnicaDatosSocio, filasConProductos, totalAPagar, infoUsuarioTable);
                }

                else
                {
                    throw new ArgumentException("FrmVisorReporte, situación no considerada en el constructor");
                }

            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source);
            }


        }

        //----------------Methods

        //---------Utils
        public bool GridContieneSoloAfiliacion( DataGridViewRowCollection filasConProductos)
        {
            if (filasConProductos.Count == 1)
            {
                DataGridViewRow filaUnica = filasConProductos[0];
                ClsProductoViewModel clsProductoViewModel = (ClsProductoViewModel)filaUnica.Cells[1].Value;


                bool analisis1 = clsProductoViewModel.Nombre == "Afiliación" ? true : false;
                bool analisis2 = clsProductoViewModel.Nombre == "afiliación" ? true : false;
                bool analisis3 = clsProductoViewModel.Nombre == "Afiliacion" ? true : false;
                bool analisis4 = clsProductoViewModel.Nombre == "afiliacion" ? true : false;
                bool analisis5 = clsProductoViewModel.Nombre == "AFILIACIÓN" ? true : false;
                bool analisis6 = clsProductoViewModel.Nombre == "AFILIACION" ? true : false;

                if (analisis1 || analisis2 || analisis3 || analisis4 || analisis5 || analisis6)
                { return (true); }

                else
                { return (false); }
            }

            return false;
        }

        public bool GridContieneAfiliacionYOtros(DataGridViewRowCollection filasConProductos)
        {
            if(filasConProductos.Count > 1)
            {
                bool contieneAfiliacionYOtros = false;

                foreach(DataGridViewRow fila in filasConProductos)
                {
                    ClsProductoViewModel clsProductoViewModel = (ClsProductoViewModel)fila.Cells[1].Value;

                    bool analisis1 = clsProductoViewModel.Nombre == "Afiliación" ? true : false;
                    bool analisis2 = clsProductoViewModel.Nombre == "afiliación" ? true : false;
                    bool analisis3 = clsProductoViewModel.Nombre == "Afiliacion" ? true : false;
                    bool analisis4 = clsProductoViewModel.Nombre == "afiliacion" ? true : false;
                    bool analisis5 = clsProductoViewModel.Nombre == "AFILIACIÓN" ? true : false;
                    bool analisis6 = clsProductoViewModel.Nombre == "AFILIACION" ? true : false;

                    if (analisis1 || analisis2 || analisis3 || analisis4 || analisis5 || analisis6)
                    {
                      contieneAfiliacionYOtros = true;
                      break;
                    }
                }

                return (contieneAfiliacionYOtros);
            }

            else
            {
                return false;
            }
        }

        public bool GridNoContieneAfiliacionSoloOtros(DataGridViewRowCollection filasConProductos)
        {

                bool noContieneAfiliacion = true;
                foreach (DataGridViewRow fila in filasConProductos)
                {
                    ClsProductoViewModel clsProductoViewModel = (ClsProductoViewModel)fila.Cells[1].Value;

                    bool analisis1 = clsProductoViewModel.Nombre == "Afiliación" ? true : false;
                    bool analisis2 = clsProductoViewModel.Nombre == "afiliación" ? true : false;
                    bool analisis3 = clsProductoViewModel.Nombre == "Afiliacion" ? true : false;
                    bool analisis4 = clsProductoViewModel.Nombre == "afiliacion" ? true : false;
                    bool analisis5 = clsProductoViewModel.Nombre == "AFILIACIÓN" ? true : false;
                    bool analisis6 = clsProductoViewModel.Nombre == "AFILIACION" ? true : false;

                    if (analisis1 || analisis2 || analisis3 || analisis4 || analisis5 || analisis6)
                    {
                        noContieneAfiliacion = false;
                        break;
                    }
                }

                return (noContieneAfiliacion);          
        }


        private void ArmarReciboListaProductosConSolo1Elemento(DataRow filaUnicaDatosSocio, DataGridViewRowCollection filasConProductos, string totalAPagar, DataTable infoUsuarioTable)
        {
            CRListaProductosPagados crListaProductosPagados = new CRListaProductosPagados();

            //-------Rellenar el area de total a pagar
            TextObject totalAPagarTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text5"] as TextObject;
            totalAPagarTextObject.Text = totalAPagar;

            //-------Rellenar el area superior
            TextObject propietarioPatenteYComodatarioTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text1"] as TextObject;
            propietarioPatenteYComodatarioTextObject.Text = filaUnicaDatosSocio.Field<string>("PropietarioPatente") + " " + filaUnicaDatosSocio.Field<string>("Comodatario");

            TextObject nombreComercialTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text2"] as TextObject;
            nombreComercialTextObject.Text = filaUnicaDatosSocio.Field<string>("NombreComercial");

            ConversorATextoMonedaPesos conversorATextoMonedaPesos = new ConversorATextoMonedaPesos();
            TextObject totalAPagarEnLetrasTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text4"] as TextObject;
            totalAPagarEnLetrasTextObject.Text = conversorATextoMonedaPesos.Analizar( (Decimal.Parse(totalAPagar)).ToString("0.00")  );

            //--------Rellenar el area donde aparece Afiliación - total a pagar
            TextObject afiliacionTextObject;
            afiliacionTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text11"] as TextObject;
            afiliacionTextObject.Text = "Afiliación";

            DataGridViewRow filaDataGrid = filasConProductos[0];
            decimal cantidadAPagar = Decimal.Parse(filaDataGrid.Cells[4].EditedFormattedValue.ToString());
            
            TextObject totalAfiliacionTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text18"] as TextObject;
            totalAfiliacionTextObject.Text = cantidadAPagar.ToString();

            //---------Limpiar el reporte de los conceptos no usados y totales no usados
            TextObject conceptoNoUsadoTextObject;
            conceptoNoUsadoTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text12"] as TextObject;
            conceptoNoUsadoTextObject.Text = "";
            conceptoNoUsadoTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text13"] as TextObject;
            conceptoNoUsadoTextObject.Text = "";
            conceptoNoUsadoTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text14"] as TextObject;
            conceptoNoUsadoTextObject.Text = "";
            conceptoNoUsadoTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text15"] as TextObject;
            conceptoNoUsadoTextObject.Text = "";
            conceptoNoUsadoTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text16"] as TextObject;
            conceptoNoUsadoTextObject.Text = "";
            conceptoNoUsadoTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text17"] as TextObject;
            conceptoNoUsadoTextObject.Text = "";

            
            TextObject totalNoUsadoTextObject;
            totalNoUsadoTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text19"] as TextObject;
            totalNoUsadoTextObject.Text = "";
            totalNoUsadoTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text20"] as TextObject;
            totalNoUsadoTextObject.Text = "";
            totalNoUsadoTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text21"] as TextObject;
            totalNoUsadoTextObject.Text = "";
            totalNoUsadoTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text22"] as TextObject;
            totalNoUsadoTextObject.Text = "";
            totalNoUsadoTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text23"] as TextObject;
            totalNoUsadoTextObject.Text = "";
            totalNoUsadoTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text24"] as TextObject;
            totalNoUsadoTextObject.Text = "";


            //--------Establecer la fecha que irá en el reporte
            if(infoUsuarioTable.Rows.Count == 1)
            {
                var res = from s in infoUsuarioTable.AsEnumerable()
                          select s;

                DataRow filaUnica = res.First();
                DateTime fechaEnServidor = filaUnica.Field<DateTime>("FechaAlta");

                TextObject diaTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text6"] as TextObject;
                diaTextObject.Text = fechaEnServidor.Day.ToString();

                TextObject mesTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text7"] as TextObject;
                mesTextObject.Text = MesATexto(fechaEnServidor.Month);

                TextObject anioTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text8"] as TextObject;
                anioTextObject.Text = (fechaEnServidor.Year % 2000).ToString();

                //-------Mostrar el reporte
                crystalReportViewer1.ReportSource = crListaProductosPagados;
            }

            else
            {
                string mensaje = "FrmVisorReporte en ArmarReciboListaProductosConSolo1Elemento ";              
                throw new ArgumentException(mensaje);
            }

        }

        private void ArmarReciboLicencia(DataRow filaUnicaDatosSocio, DataTable infoUsuarioTable)
        {
            CRReciboLicencia crReciboLicencia = new CRReciboLicencia();

            TextObject nombreComercialTextObject = crReciboLicencia.ReportDefinition.ReportObjects["Text1"] as TextObject;
            nombreComercialTextObject.Text = filaUnicaDatosSocio.Field<string>("NombreComercial");

            TextObject propietarioPatenteTextObject = crReciboLicencia.ReportDefinition.ReportObjects["Text2"] as TextObject;
            propietarioPatenteTextObject.Text = filaUnicaDatosSocio.Field<string>("PropietarioPatente");

            TextObject rfcPropietarioTextObject = crReciboLicencia.ReportDefinition.ReportObjects["Text3"] as TextObject;
            rfcPropietarioTextObject.Text = filaUnicaDatosSocio.Field<string>("RFCPropietario");


            TextObject direccionTextObject = crReciboLicencia.ReportDefinition.ReportObjects["Text4"] as TextObject;
            string cadenaDirecc = "Supermanzana " + filaUnicaDatosSocio.Field<string>("DireccionSupmza") + " ";
            cadenaDirecc += "Manzana " + filaUnicaDatosSocio.Field<string>("DireccionManzana") + " ";
            cadenaDirecc += "Lote " + filaUnicaDatosSocio.Field<string>("DireccionLote") + " ";
            cadenaDirecc += "Calle " + filaUnicaDatosSocio.Field<string>("DireccionCalle");
            direccionTextObject.Text = cadenaDirecc;        
            //Por razones de espacio en el reporte no muestro la columna DirecciomComplemento

            TextObject telefonoTextObject = crReciboLicencia.ReportDefinition.ReportObjects["Text5"] as TextObject;
            telefonoTextObject.Text = filaUnicaDatosSocio.Field<string>("Telefono");

            TextObject emailTextObject = crReciboLicencia.ReportDefinition.ReportObjects["Text6"] as TextObject;
            emailTextObject.Text = filaUnicaDatosSocio.Field<string>("CorreoElectronico");

            TextObject nombreComodatarioTextObject = crReciboLicencia.ReportDefinition.ReportObjects["Text11"] as TextObject;
            nombreComodatarioTextObject.Text = filaUnicaDatosSocio.Field<string>("Comodatario");


            TextObject rfcComodatarioTextObject = crReciboLicencia.ReportDefinition.ReportObjects["Text12"] as TextObject;
            rfcComodatarioTextObject.Text = filaUnicaDatosSocio.Field<string>("RFCComodatario");

            TextObject numeroLicenciaTextObject = crReciboLicencia.ReportDefinition.ReportObjects["Text7"] as TextObject;
            numeroLicenciaTextObject.Text = filaUnicaDatosSocio.Field<string>("NumeroLicencia");

            if (infoUsuarioTable.Rows.Count == 1)
            {
                var res = from s in infoUsuarioTable.AsEnumerable()
                          select s;

                DataRow filaUnica = res.First();
                DateTime fechaEnServidor = filaUnica.Field<DateTime>("FechaAlta");

                TextObject diaTextObject = crReciboLicencia.ReportDefinition.ReportObjects["Text8"] as TextObject;
                diaTextObject.Text = fechaEnServidor.Day.ToString();

                TextObject mesTextObject = crReciboLicencia.ReportDefinition.ReportObjects["Text9"] as TextObject;
                mesTextObject.Text = MesATexto(fechaEnServidor.Month);

                TextObject anioTextObject = crReciboLicencia.ReportDefinition.ReportObjects["Text10"] as TextObject;
                anioTextObject.Text = fechaEnServidor.Year.ToString();

                crystalReportViewer2.ReportSource = crReciboLicencia;
            }

            else
            {
                string mensaje = "FrmVisorReporte en ArmarReciboLicencia ";              
                throw new ArgumentException(mensaje);
            }
        }

        private void ArmarReciboListaProductosConVariosElementos(DataRow filaUnicaDatosSocio, DataGridViewRowCollection filasConProductos, string totalAPagar, DataTable infoUsuarioTable)
        {
            CRListaProductosPagados crListaProductosPagados = new CRListaProductosPagados();

            //-------Rellenar el area de total a pagar
            TextObject totalAPagarTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text5"] as TextObject;
            totalAPagarTextObject.Text = totalAPagar;

            //-------Rellenar el area superior
            TextObject propietarioPatenteYComodatarioTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text1"] as TextObject;
            propietarioPatenteYComodatarioTextObject.Text = filaUnicaDatosSocio.Field<string>("PropietarioPatente") + " " + filaUnicaDatosSocio.Field<string>("Comodatario");

            TextObject nombreComercialTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text2"] as TextObject;
            nombreComercialTextObject.Text = filaUnicaDatosSocio.Field<string>("NombreComercial");

            ConversorATextoMonedaPesos conversorATextoMonedaPesos = new ConversorATextoMonedaPesos();
            TextObject totalAPagarEnLetrasTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text4"] as TextObject;
            totalAPagarEnLetrasTextObject.Text = conversorATextoMonedaPesos.Analizar((Decimal.Parse(totalAPagar)).ToString("0.00"));

            //-------Rellenar el area donde aparece Concepto - Total a pagar
            Hashtable tablaHash = RecuperarEtiquetasConceptoDelReporte(crListaProductosPagados);
            Hashtable totalesTablaHash = RecuperarEtiquetasTotalesDelReporte(crListaProductosPagados);

            int i = 0; 
            foreach (DataGridViewRow fila in filasConProductos)
            {
                //Rellenando la columna conceptos
                ClsProductoViewModel clsProductoViewModel = (ClsProductoViewModel)fila.Cells[1].Value;
                TextObject objTextObject = (TextObject)tablaHash[i.ToString()];
                objTextObject.Text = clsProductoViewModel.Nombre;

                //Rellenando la columna totales
                string totalEnCelda = fila.Cells[4].EditedFormattedValue.ToString();
                TextObject otroTextObject = (TextObject)totalesTablaHash[i.ToString()];
                otroTextObject.Text = totalEnCelda;

                i++;
            }

            AsignarVacioAEtiquetasQueContienen(tablaHash,"concepto"); //Quitar el texto "concepto" a las etiquetas  no usadas
            AsignarVacioAEtiquetasQueContienen(totalesTablaHash, "total");   //Quitar el texto "0" a las etiquetas no usadas


            //--------Establecer la fecha que irá en el reporte
            if(infoUsuarioTable.Rows.Count == 1)
            {
                var res = from s in infoUsuarioTable.AsEnumerable()
                          select s;

                DataRow filaUnica = res.First();
                DateTime fechaEnServidor = filaUnica.Field<DateTime>("FechaAlta");

                TextObject diaTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text6"] as TextObject;
                diaTextObject.Text = fechaEnServidor.Day.ToString();

                TextObject mesTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text7"] as TextObject;
                mesTextObject.Text = MesATexto(fechaEnServidor.Month);

                TextObject anioTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text8"] as TextObject;
                anioTextObject.Text = (fechaEnServidor.Year % 2000).ToString();

                //-------Mostrar el reporte
                crystalReportViewer1.ReportSource = crListaProductosPagados;
            }

            else
            {
                string mensaje = "FrmVisorReporte en ArmarReciboListaProductosConVariosElementos";
                throw new ArgumentException(mensaje);
            }
        }


        private Hashtable RecuperarEtiquetasConceptoDelReporte(CRListaProductosPagados crListaProductosPagados)
        {
            Hashtable tablaHash = new Hashtable();
            TextObject conceptoTextObject;

            conceptoTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text11"] as TextObject;
            tablaHash.Add("0", conceptoTextObject);

            conceptoTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text12"] as TextObject;
            tablaHash.Add("1", conceptoTextObject);

            conceptoTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text13"] as TextObject;
            tablaHash.Add("2", conceptoTextObject);

            conceptoTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text14"] as TextObject;
            tablaHash.Add("3", conceptoTextObject);

            conceptoTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text15"] as TextObject;
            tablaHash.Add("4", conceptoTextObject);

            conceptoTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text16"] as TextObject;
            tablaHash.Add("5", conceptoTextObject);

            conceptoTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text17"] as TextObject;
            tablaHash.Add("6", conceptoTextObject);

            return (tablaHash);
        }

        private void AsignarVacioAEtiquetasQueContienen(Hashtable tablaHash, string textoABuscar)
        {
            for (int i = 0; i <= 6; i++)
            {
                TextObject objetoTextObject = (TextObject)tablaHash[i.ToString()];
                if (objetoTextObject.Text.Contains(textoABuscar))
                {
                    objetoTextObject.Text = "";
                }
            }
        }


        private Hashtable RecuperarEtiquetasTotalesDelReporte(CRListaProductosPagados crListaProductosPagados)
        {
            Hashtable totalesHashtable = new Hashtable();
            TextObject totalTextObject;
            
            totalTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text18"] as TextObject;
            totalesHashtable.Add("0", totalTextObject);

            totalTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text19"] as TextObject;
            totalesHashtable.Add("1", totalTextObject);

            totalTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text20"] as TextObject;
            totalesHashtable.Add("2", totalTextObject);

            totalTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text21"] as TextObject;
            totalesHashtable.Add("3", totalTextObject);

            totalTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text22"] as TextObject;
            totalesHashtable.Add("4", totalTextObject);

            totalTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text23"] as TextObject;
            totalesHashtable.Add("5", totalTextObject);

            totalTextObject = crListaProductosPagados.ReportDefinition.ReportObjects["Text24"] as TextObject;
            totalesHashtable.Add("6", totalTextObject);

            return (totalesHashtable);
        }

        private string MesATexto(int mesBuscado)
        {
            Hashtable meses = new Hashtable();
            meses.Add(1, "Enero");
            meses.Add(2, "Febrero");
            meses.Add(3, "Marzo");
            meses.Add(4, "Abril");
            meses.Add(5, "Mayo");
            meses.Add(6, "Junio");
            meses.Add(7, "Julio");
            meses.Add(8, "Agosto");
            meses.Add(9, "Septiembre");
            meses.Add(10, "Octubre");
            meses.Add(11, "Noviembre");
            meses.Add(12, "Diciembre");

            return (meses[mesBuscado].ToString());
        }



    }
}
