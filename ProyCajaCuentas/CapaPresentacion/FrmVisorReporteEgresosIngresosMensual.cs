using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CapaLogicaNegocios;

namespace CapaPresentacion
{
    public partial class FrmVisorReporteEgresosIngresosMensual : Form
    {
        //-------------------Constructor
        public FrmVisorReporteEgresosIngresosMensual()
        {
            InitializeComponent();
            CargarComboBoxAnios();
            SelectFirtsElementInComboBoxAnios();
            CargarComboBoxMeses();
            SelectFirstElementInComboBoxMeses();
        }

        //--------------------------Controllers
        private DataTable Bancos_BuscarPeriodoActivoController(int anio, string mes)
        {
            ClsBancos clsBancos = new ClsBancos();
            clsBancos.PeriodoAnio = anio;
            clsBancos.PeriodoMes = mes;

            return (clsBancos.Bancos_BuscarPeriodoActivo());
        }

        private DataTable MovsEnCaja_SumarPagoDeTodosProductosController(DateTime fechaInicio, DateTime fechaFin)
        {
            ClsMovsEnCaja clsMovsEnCaja = new ClsMovsEnCaja();
            clsMovsEnCaja.FechaAlta = fechaInicio;
            clsMovsEnCaja.FechaModificacion = fechaFin;

            return (clsMovsEnCaja.MovsEnCaja_SumarPagoDeTodosProductos());
        }


        private DataTable Cheque_SumarImporteDeChequesActivosController(DateTime fechaInicio, DateTime fechaFin)
        {
            ClsCheque clsCheque = new ClsCheque();
            clsCheque.FechaAlta = fechaInicio;
            clsCheque.FechaModificacion = fechaFin;

            return (clsCheque.Cheque_SumarImporteDeChequesActivos());
        }

        private DataTable Cheque_SumarImporteDeChequesCobradosController(DateTime fechaInicio, DateTime fechaFin)
        {
            ClsCheque clsCheque = new ClsCheque();
            clsCheque.FechaAlta = fechaInicio;
            clsCheque.FechaModificacion = fechaFin;

            return (clsCheque.Cheque_SumarImporteDeChequesCobrados());
        }

        private DataTable InicialTotalDeChequesCobradosDePeriodosAnteriores_BuscarActivoController()
        {
            ClsInicialTotalDeChequesCobradosDePeriodosAnteriores clsInicialTotalDeChequesCobradosDePeriodosAnteriores;
            clsInicialTotalDeChequesCobradosDePeriodosAnteriores = new ClsInicialTotalDeChequesCobradosDePeriodosAnteriores();

            return (clsInicialTotalDeChequesCobradosDePeriodosAnteriores.InicialTotalDeChequesCobradosDePeriodosAnteriores_BuscarActivo());
        }

        private DataTable Cheque_SumarImporteDeChequesNoCobradosController(DateTime fechaInicio, DateTime fechaFin)
        {
            ClsCheque clsCheque = new ClsCheque();
            clsCheque.FechaAlta = fechaInicio;
            clsCheque.FechaModificacion = fechaFin;

            return (clsCheque.Cheque_SumarImporteDeChequesNoCobrados());
        }

        //----------------------Methods
        private void CargarComboBoxAnios()
        {
            comboBox1.Items.Add("2018");
            comboBox1.Items.Add("2019");
            comboBox1.Items.Add("2020");
            comboBox1.Items.Add("2021");
            comboBox1.Items.Add("2022");
        }

        private void SelectFirtsElementInComboBoxAnios()
        {
            comboBox1.SelectedIndex = 0;
        }

        private void CargarComboBoxMeses()
        {
            List<string> meses = new List<string>();
            meses.Add("Enero");
            meses.Add("Febrero");
            meses.Add("Marzo");
            meses.Add("Abril");
            meses.Add("Mayo");
            meses.Add("Junio");
            meses.Add("Julio");
            meses.Add("Agosto");
            meses.Add("Septiembre");
            meses.Add("Octubre");
            meses.Add("Noviembre");
            meses.Add("Diciembre");

            comboBox2.DataSource = meses;
        }

        private void SelectFirstElementInComboBoxMeses()
        {
            comboBox2.SelectedIndex = 0;
        }

        private bool EstaSeleccionadoComboBoxAnioYComBoBoxMes()
        {
            bool res = (comboBox1.SelectedIndex >= 0) && (comboBox2.SelectedIndex >= 0) ? true : false;
            return (res);
        }


        private Hashtable GenerarParametrosParaReporte()
        {
            RangoFechasUsadasEnReporte enero2018 = new RangoFechasUsadasEnReporte(2018, 1, 1, 2018, 1, DateTime.DaysInMonth(2018, 1), "enero");
            RangoFechasUsadasEnReporte febrero2018 = new RangoFechasUsadasEnReporte(2018, 2, 1, 2018, 2, DateTime.DaysInMonth(2018, 2), "febrero");
            RangoFechasUsadasEnReporte marzo2018 = new RangoFechasUsadasEnReporte(2018, 3, 1, 2018, 3, DateTime.DaysInMonth(2018, 3), "marzo");
            RangoFechasUsadasEnReporte abril2018 = new RangoFechasUsadasEnReporte(2018, 4, 1, 2018, 4, DateTime.DaysInMonth(2018, 4), "abril");
            RangoFechasUsadasEnReporte mayo2018 = new RangoFechasUsadasEnReporte(2018, 5, 1, 2018, 5, DateTime.DaysInMonth(2018, 5), "mayo");
            RangoFechasUsadasEnReporte junio2018 = new RangoFechasUsadasEnReporte(2018, 6, 1, 2018, 6, DateTime.DaysInMonth(2018, 6), "junio");
            RangoFechasUsadasEnReporte julio2018 = new RangoFechasUsadasEnReporte(2018, 7, 1, 2018, 7, DateTime.DaysInMonth(2018, 7), "julio");
            RangoFechasUsadasEnReporte agosto2018 = new RangoFechasUsadasEnReporte(2018, 8, 1, 2018, 8, DateTime.DaysInMonth(2018, 8), "agosto");
            RangoFechasUsadasEnReporte septiembre2018 = new RangoFechasUsadasEnReporte(2018, 9, 1, 2018, 9, DateTime.DaysInMonth(2018, 9), "septiembre");
            RangoFechasUsadasEnReporte octubre2018 = new RangoFechasUsadasEnReporte(2018, 10, 1, 2018, 10, DateTime.DaysInMonth(2018, 10), "octubre");
            RangoFechasUsadasEnReporte noviembre2018 = new RangoFechasUsadasEnReporte(2018, 11, 1, 2018, 11, DateTime.DaysInMonth(2018, 11), "noviembre");
            RangoFechasUsadasEnReporte diciembre2018 = new RangoFechasUsadasEnReporte(2018, 12, 1, 2018, 12, DateTime.DaysInMonth(2018, 12), "diciembre");

            RangoFechasUsadasEnReporte enero2019 = new RangoFechasUsadasEnReporte(2019, 1, 1, 2019, 1, DateTime.DaysInMonth(2019, 1), "enero");
            RangoFechasUsadasEnReporte febrero2019 = new RangoFechasUsadasEnReporte(2019, 2, 1, 2019, 2, DateTime.DaysInMonth(2019, 2), "febrero");
            RangoFechasUsadasEnReporte marzo2019 = new RangoFechasUsadasEnReporte(2019, 3, 1, 2019, 3, DateTime.DaysInMonth(2019, 3), "marzo");
            RangoFechasUsadasEnReporte abril2019 = new RangoFechasUsadasEnReporte(2019, 4, 1, 2019, 4, DateTime.DaysInMonth(2019, 4), "abril");
            RangoFechasUsadasEnReporte mayo2019 = new RangoFechasUsadasEnReporte(2019, 5, 1, 2019, 5, DateTime.DaysInMonth(2019, 5), "mayo");
            RangoFechasUsadasEnReporte junio2019 = new RangoFechasUsadasEnReporte(2019, 6, 1, 2019, 6, DateTime.DaysInMonth(2019, 6), "junio");
            RangoFechasUsadasEnReporte julio2019 = new RangoFechasUsadasEnReporte(2019, 7, 1, 2019, 7, DateTime.DaysInMonth(2019, 7), "julio");
            RangoFechasUsadasEnReporte agosto2019 = new RangoFechasUsadasEnReporte(2019, 8, 1, 2019, 8, DateTime.DaysInMonth(2019, 8), "agosto");
            RangoFechasUsadasEnReporte septiembre2019 = new RangoFechasUsadasEnReporte(2019, 9, 1, 2019, 9, DateTime.DaysInMonth(2019, 9), "septiembre");
            RangoFechasUsadasEnReporte octubre2019 = new RangoFechasUsadasEnReporte(2019, 10, 1, 2019, 10, DateTime.DaysInMonth(2019, 10), "octubre");
            RangoFechasUsadasEnReporte noviembre2019 = new RangoFechasUsadasEnReporte(2019, 11, 1, 2019, 11, DateTime.DaysInMonth(2019, 11), "noviembre");
            RangoFechasUsadasEnReporte diciembre2019 = new RangoFechasUsadasEnReporte(2019, 12, 1, 2019, 12, DateTime.DaysInMonth(2019, 12), "diciembre");

            RangoFechasUsadasEnReporte enero2020 = new RangoFechasUsadasEnReporte(2020, 1, 1, 2020, 1, DateTime.DaysInMonth(2020, 1), "enero");
            RangoFechasUsadasEnReporte febrero2020 = new RangoFechasUsadasEnReporte(2020, 2, 1, 2020, 2, DateTime.DaysInMonth(2020, 2), "febrero");
            RangoFechasUsadasEnReporte marzo2020 = new RangoFechasUsadasEnReporte(2020, 3, 1, 2020, 3, DateTime.DaysInMonth(2020, 3), "marzo");
            RangoFechasUsadasEnReporte abril2020 = new RangoFechasUsadasEnReporte(2020, 4, 1, 2020, 4, DateTime.DaysInMonth(2020, 4), "abril");
            RangoFechasUsadasEnReporte mayo2020 = new RangoFechasUsadasEnReporte(2020, 5, 1, 2020, 5, DateTime.DaysInMonth(2020, 5), "mayo");
            RangoFechasUsadasEnReporte junio2020 = new RangoFechasUsadasEnReporte(2020, 6, 1, 2020, 6, DateTime.DaysInMonth(2020, 6), "junio");
            RangoFechasUsadasEnReporte julio2020 = new RangoFechasUsadasEnReporte(2020, 7, 1, 2020, 7, DateTime.DaysInMonth(2020, 7), "julio");
            RangoFechasUsadasEnReporte agosto2020 = new RangoFechasUsadasEnReporte(2020, 8, 1, 2020, 8, DateTime.DaysInMonth(2020, 8), "agosto");
            RangoFechasUsadasEnReporte septiembre2020 = new RangoFechasUsadasEnReporte(2020, 9, 1, 2020, 9, DateTime.DaysInMonth(2020, 9), "septiembre");
            RangoFechasUsadasEnReporte octubre2020 = new RangoFechasUsadasEnReporte(2020, 10, 1, 2020, 10, DateTime.DaysInMonth(2020, 10), "octubre");
            RangoFechasUsadasEnReporte noviembre2020 = new RangoFechasUsadasEnReporte(2020, 11, 1, 2020, 11, DateTime.DaysInMonth(2020, 11), "noviembre");
            RangoFechasUsadasEnReporte diciembre2020 = new RangoFechasUsadasEnReporte(2020, 12, 1, 2020, 12, DateTime.DaysInMonth(2020, 12), "diciembre");

            RangoFechasUsadasEnReporte enero2021 = new RangoFechasUsadasEnReporte(2021, 1, 1, 2021, 1, DateTime.DaysInMonth(2021, 1), "enero");
            RangoFechasUsadasEnReporte febrero2021 = new RangoFechasUsadasEnReporte(2021, 2, 1, 2021, 2, DateTime.DaysInMonth(2021, 2), "febrero");
            RangoFechasUsadasEnReporte marzo2021 = new RangoFechasUsadasEnReporte(2021, 3, 1, 2021, 3, DateTime.DaysInMonth(2021, 3), "marzo");
            RangoFechasUsadasEnReporte abril2021 = new RangoFechasUsadasEnReporte(2021, 4, 1, 2021, 4, DateTime.DaysInMonth(2021, 4), "abril");
            RangoFechasUsadasEnReporte mayo2021 = new RangoFechasUsadasEnReporte(2021, 5, 1, 2021, 5, DateTime.DaysInMonth(2021, 5), "mayo");
            RangoFechasUsadasEnReporte junio2021 = new RangoFechasUsadasEnReporte(2021, 6, 1, 2021, 6, DateTime.DaysInMonth(2021, 6), "junio");
            RangoFechasUsadasEnReporte julio2021 = new RangoFechasUsadasEnReporte(2021, 7, 1, 2021, 7, DateTime.DaysInMonth(2021, 7), "julio");
            RangoFechasUsadasEnReporte agosto2021 = new RangoFechasUsadasEnReporte(2021, 8, 1, 2021, 8, DateTime.DaysInMonth(2021, 8), "agosto");
            RangoFechasUsadasEnReporte septiembre2021 = new RangoFechasUsadasEnReporte(2021, 9, 1, 2021, 9, DateTime.DaysInMonth(2021, 9), "septiembre");
            RangoFechasUsadasEnReporte octubre2021 = new RangoFechasUsadasEnReporte(2021, 10, 1, 2021, 10, DateTime.DaysInMonth(2021, 10), "octubre");
            RangoFechasUsadasEnReporte noviembre2021 = new RangoFechasUsadasEnReporte(2021, 11, 1, 2021, 11, DateTime.DaysInMonth(2021, 11), "noviembre");
            RangoFechasUsadasEnReporte diciembre2021 = new RangoFechasUsadasEnReporte(2021, 12, 1, 2021, 12, DateTime.DaysInMonth(2021, 12), "diciembre");

            RangoFechasUsadasEnReporte enero2022 = new RangoFechasUsadasEnReporte(2022, 1, 1, 2022, 1, DateTime.DaysInMonth(2022, 1), "enero");
            RangoFechasUsadasEnReporte febrero2022 = new RangoFechasUsadasEnReporte(2022, 2, 1, 2022, 2, DateTime.DaysInMonth(2022, 2), "febrero");
            RangoFechasUsadasEnReporte marzo2022 = new RangoFechasUsadasEnReporte(2022, 3, 1, 2022, 3, DateTime.DaysInMonth(2022, 3), "marzo");
            RangoFechasUsadasEnReporte abril2022 = new RangoFechasUsadasEnReporte(2022, 4, 1, 2022, 4, DateTime.DaysInMonth(2022, 4), "abril");
            RangoFechasUsadasEnReporte mayo2022 = new RangoFechasUsadasEnReporte(2022, 5, 1, 2022, 5, DateTime.DaysInMonth(2022, 5), "mayo");
            RangoFechasUsadasEnReporte junio2022 = new RangoFechasUsadasEnReporte(2022, 6, 1, 2022, 6, DateTime.DaysInMonth(2022, 6), "junio");
            RangoFechasUsadasEnReporte julio2022 = new RangoFechasUsadasEnReporte(2022, 7, 1, 2022, 7, DateTime.DaysInMonth(2022, 7), "julio");
            RangoFechasUsadasEnReporte agosto2022 = new RangoFechasUsadasEnReporte(2022, 8, 1, 2022, 8, DateTime.DaysInMonth(2022, 8), "agosto");
            RangoFechasUsadasEnReporte septiembre2022 = new RangoFechasUsadasEnReporte(2022, 9, 1, 2022, 9, DateTime.DaysInMonth(2022, 9), "septiembre");
            RangoFechasUsadasEnReporte octubre2022 = new RangoFechasUsadasEnReporte(2022, 10, 1, 2022, 10, DateTime.DaysInMonth(2022, 10), "octubre");
            RangoFechasUsadasEnReporte noviembre2022 = new RangoFechasUsadasEnReporte(2022, 11, 1, 2022, 11, DateTime.DaysInMonth(2022, 11), "noviembre");
            RangoFechasUsadasEnReporte diciembre2022 = new RangoFechasUsadasEnReporte(2022, 12, 1, 2022, 12, DateTime.DaysInMonth(2022, 12), "diciembre");

            Hashtable tablaHash = new Hashtable();
            tablaHash.Add("enero2018", enero2018);
            tablaHash.Add("febrero2018", febrero2018);
            tablaHash.Add("marzo2018", marzo2018);
            tablaHash.Add("abril2018", abril2018);
            tablaHash.Add("mayo2018", mayo2018);
            tablaHash.Add("junio2018", junio2018);
            tablaHash.Add("julio2018", julio2018);
            tablaHash.Add("agosto2018", agosto2018);
            tablaHash.Add("septiembre2018", septiembre2018);
            tablaHash.Add("octubre2018", octubre2018);
            tablaHash.Add("noviembre2018", noviembre2018);
            tablaHash.Add("diciembre2018", diciembre2018);

            tablaHash.Add("enero2019", enero2019);
            tablaHash.Add("febrero2019", febrero2019);
            tablaHash.Add("marzo2019", marzo2019);
            tablaHash.Add("abril2019", abril2019);
            tablaHash.Add("mayo2019", mayo2019);
            tablaHash.Add("junio2019", junio2019);
            tablaHash.Add("julio2019", julio2019);
            tablaHash.Add("agosto2019", agosto2019);
            tablaHash.Add("septiembre2019", septiembre2019);
            tablaHash.Add("octubre2019", octubre2019);
            tablaHash.Add("noviembre2019", noviembre2019);
            tablaHash.Add("diciembre2019", diciembre2019);

            tablaHash.Add("enero2020", enero2020);
            tablaHash.Add("febrero2020", febrero2020);
            tablaHash.Add("marzo2020", marzo2020);
            tablaHash.Add("abril2020", abril2020);
            tablaHash.Add("mayo2020", mayo2020);
            tablaHash.Add("junio2020", junio2020);
            tablaHash.Add("julio2020", julio2020);
            tablaHash.Add("agosto2020", agosto2020);
            tablaHash.Add("septiembre2020", septiembre2020);
            tablaHash.Add("octubre2020", octubre2020);
            tablaHash.Add("noviembre2020", noviembre2020);
            tablaHash.Add("diciembre2020", diciembre2020);

            tablaHash.Add("enero2021", enero2021);
            tablaHash.Add("febrero2021", febrero2021);
            tablaHash.Add("marzo2021", marzo2021);
            tablaHash.Add("abril2021", abril2021);
            tablaHash.Add("mayo2021", mayo2021);
            tablaHash.Add("junio2021", junio2021);
            tablaHash.Add("julio2021", julio2021);
            tablaHash.Add("agosto2021", agosto2021);
            tablaHash.Add("septiembre2021", septiembre2021);
            tablaHash.Add("octubre2021", octubre2021);
            tablaHash.Add("noviembre2021", noviembre2021);
            tablaHash.Add("diciembre2021", diciembre2021);

            tablaHash.Add("enero2022", enero2022);
            tablaHash.Add("febrero2022", febrero2022);
            tablaHash.Add("marzo2022", marzo2022);
            tablaHash.Add("abril2022", abril2022);
            tablaHash.Add("mayo2022", mayo2022);
            tablaHash.Add("junio2022", junio2022);
            tablaHash.Add("julio2022", julio2022);
            tablaHash.Add("agosto2022", agosto2022);
            tablaHash.Add("septiembre2022", septiembre2022);
            tablaHash.Add("octubre2022", octubre2022);
            tablaHash.Add("noviembre2022", noviembre2022);
            tablaHash.Add("diciembre2022", diciembre2022);

            return (tablaHash);
        }

        private Hashtable ObtenerTextObjectsDeCrystalReport(CRReporteEgresosIngresosMensual crReporte)
        {
            Hashtable tablaHash = new Hashtable();
            TextObject textObject;

            
            textObject = crReporte.ReportDefinition.ReportObjects["Text3"] as TextObject;
            tablaHash.Add("disponibleEnBancos", textObject);

            textObject = crReporte.ReportDefinition.ReportObjects["Text4"] as TextObject;
            tablaHash.Add("totalIngresos", textObject);

            textObject = crReporte.ReportDefinition.ReportObjects["Text5"] as TextObject;
            tablaHash.Add("gastos", textObject);

            textObject = crReporte.ReportDefinition.ReportObjects["Text6"] as TextObject;
            tablaHash.Add("chequesCobradosDePeriodosAnteriores", textObject);

            textObject = crReporte.ReportDefinition.ReportObjects["Text7"] as TextObject;
            tablaHash.Add("chequesNoCobradosEnElPeriodo", textObject);

            textObject = crReporte.ReportDefinition.ReportObjects["Text8"] as TextObject;
            tablaHash.Add("saldoRealDeRetirosEstadoDeCuenta", textObject);

            textObject = crReporte.ReportDefinition.ReportObjects["Text9"] as TextObject;
            tablaHash.Add("saldoDisponibleEnCuentaBancaria", textObject);

            textObject = crReporte.ReportDefinition.ReportObjects["Text10"] as TextObject;
            tablaHash.Add("saldoRealEnCuenta", textObject);

            return (tablaHash);
        }

        private List<string> MezclarAnioConMesElegido(ComboBox anioComboBox, ComboBox mesComboBox)
        {
            string anioElegido = anioComboBox.SelectedItem.ToString();
            string mesElegido = (mesComboBox.SelectedItem.ToString()).ToLower();

            StringBuilder stb = new StringBuilder();
            stb.Append(mesElegido);
            stb.Append(anioElegido);

            List<string> mesConAnioElegido = new List<string>();
            mesConAnioElegido.Add(stb.ToString());

            return (mesConAnioElegido);
        }

        private void MostrarEnTextBoxDisponibleEnBancos(DataTable periodo, TextObject textBox)
        {
            DataRow filaUnica = periodo.AsEnumerable().Single();
            textBox.Text = filaUnica.Field<decimal>("DisponibleEnBancos").ToString();
        }

        private void MostrarEnTextBoxDisponibleEnBancosReal(DataTable periodo, TextObject textBox)
        {
            DataRow filaUnica = periodo.AsEnumerable().Single();
            textBox.Text = filaUnica.Field<decimal>("DisponibleEnBancosReal").ToString();
        }

        private void MostrarEnTextBoxSumaDeTodosLosProductos(DataTable tabla, TextObject textBox)
        {
            DataRow filaUnica = tabla.Rows[0];
            decimal sumaTodosLosProductos;
            bool sePudoExtraerLaSuma;

            //la tabla contiene una unica fila con un valor diferente de 0 ó null
            sePudoExtraerLaSuma = Decimal.TryParse(filaUnica["suma del pago de todos los productos"].ToString(), out sumaTodosLosProductos);

            if (sePudoExtraerLaSuma)
                textBox.Text = sumaTodosLosProductos.ToString();
            else
                textBox.Text = "0.0000";
        }

        private void MostrarEnTextBoxSumaDeTodosLosCheques(DataTable tabla, TextObject textBox)
        {
            DataRow filaUnica = tabla.Rows[0];
            decimal sumaTodosLosCheques;
            bool sePudoExtraerLaSuma;

            //la tabla contiene una unica fila con un valor diferente de 0 ó null
            sePudoExtraerLaSuma = Decimal.TryParse(filaUnica["suma de importes"].ToString(), out sumaTodosLosCheques);

            if (sePudoExtraerLaSuma)
                textBox.Text = sumaTodosLosCheques.ToString();
            else
                textBox.Text = "0.0000";
        }


        private void MostrarEnTextBoxSumaDeChequesCobradosDePeriodosAnteriores(DataTable tabla, TextObject textBox,
            decimal inicialTotalDeChequesCobradosDePeriodosAnteriores, DateTime fechaDePeriodoInicialDeChequesCobradosDePeriodosAnteriores,
            RangoFechasUsadasEnReporte rango)
        {
            DataRow filaUnica = tabla.Rows[0];
            decimal sumaDeCheques = 0.0m;
            bool sePudoExtraerLaSuma = false;

            //la tabla contiene una unica fila con un valor diferente de 0 ó null
            sePudoExtraerLaSuma = Decimal.TryParse(filaUnica["suma de importes"].ToString(), out sumaDeCheques);

            if(sePudoExtraerLaSuma == false)
            { sumaDeCheques = 0.0000m; }

            if (rango.FechaInicio >= fechaDePeriodoInicialDeChequesCobradosDePeriodosAnteriores)
            {
                textBox.Text = (sumaDeCheques + inicialTotalDeChequesCobradosDePeriodosAnteriores).ToString();
            }

            else
            {
                textBox.Text = sumaDeCheques.ToString();
            }
        }

        private void MostrarEnTextBoxSumaDeTodosLosChequeNoCobrados(DataTable tabla, TextObject textBox)
        {
            DataRow filaUnica = tabla.Rows[0];
            decimal sumaTodosLosCheques;
            bool sePudoExtraerLaSuma;

            //la tabla contiene una unica fila con un valor diferente de 0 ó null
            sePudoExtraerLaSuma = Decimal.TryParse(filaUnica["suma de importes"].ToString(), out sumaTodosLosCheques);

            if (sePudoExtraerLaSuma)
                textBox.Text = sumaTodosLosCheques.ToString();
            else
                textBox.Text = "0.0000";
        }

        private void MostrarEnTextBoxSaldoRealDeRetirosEstadoDeCuenta(TextObject suma1TxtBox, TextObject suma2TxtBox, TextObject restaTxtBox, TextObject destinoTextBox)
        {

            decimal suma = Decimal.Parse(suma1TxtBox.Text) + Decimal.Parse(suma2TxtBox.Text) - Decimal.Parse(restaTxtBox.Text);
            destinoTextBox.Text = suma.ToString();
        }

        private void MostrarEnTextBoxSaldoDisponibleEnCuentaBancaria(TextObject totalIngresos, TextObject saldoRealRetirosEstadoDeCuenta, TextObject disponibleEnBancosDePeriodo, TextObject destino)
        {
            decimal suma = Decimal.Parse(totalIngresos.Text) + Decimal.Parse(saldoRealRetirosEstadoDeCuenta.Text);
            destino.Text = (Decimal.Parse(disponibleEnBancosDePeriodo.Text) - suma).ToString();
        }


        private void PonerTituloAReporte(TextObject textObject, string texto)
        {
            textObject.Text = texto;
        }


        private decimal ExtraerTotalInicialDeChequesCobradosDePeriodosAnteriores(DataTable inicialTotalDeChequesCobradosDePeriodosAnteriores)
        {
            var res = inicialTotalDeChequesCobradosDePeriodosAnteriores.AsEnumerable();
            List<DataRow> listaElementos = res.ToList<DataRow>();  //Esa lista solo contiene un elemento o esta vacia

            DataRow filaUnica = listaElementos.SingleOrDefault<DataRow>();
            if (filaUnica != null)
                return (filaUnica.Field<decimal>("Total"));
            else
                return (0.0m);
        }


        private DateTime ExtraerFechaDePeriodoInicialDeChequesCobradosDePeriodosAnteriores(DataTable inicialTotalDeChequesCobradosDePeriodosAnteriores)
        {
            var res = inicialTotalDeChequesCobradosDePeriodosAnteriores.AsEnumerable();
            List<DataRow> listaElementos = res.ToList<DataRow>();  //Esa lista solo contiene un elemento o esta vacia

            DataRow filaUnica = listaElementos.SingleOrDefault<DataRow>();
            if (filaUnica != null)
                return (filaUnica.Field<DateTime>("FechaDePeriodoInicial"));
            else
                return (DateTime.MinValue);
        }

        private Hashtable ObtenerFormulaFieldDefinitionDeCR(CRReporteEgresosIngresosMensualParaExportar crReporte)
        {
            Hashtable tablaHash = new Hashtable();
            FormulaFieldDefinition formulaFieldDefinition;

            formulaFieldDefinition = crReporte.DataDefinition.FormulaFields["UnboundCurrency6"] as FormulaFieldDefinition;
            tablaHash.Add("disponibleEnBancos", formulaFieldDefinition);

            formulaFieldDefinition = crReporte.DataDefinition.FormulaFields["UnboundCurrency7"] as FormulaFieldDefinition;
            tablaHash.Add("totalIngresos", formulaFieldDefinition);

            formulaFieldDefinition = crReporte.DataDefinition.FormulaFields["UnboundCurrency1"] as FormulaFieldDefinition;
            tablaHash.Add("gastos", formulaFieldDefinition);

            formulaFieldDefinition = crReporte.DataDefinition.FormulaFields["UnboundCurrency2"] as FormulaFieldDefinition;
            tablaHash.Add("chequesCobradosDePeriodosAnteriores", formulaFieldDefinition);

            formulaFieldDefinition = crReporte.DataDefinition.FormulaFields["UnboundCurrency3"] as FormulaFieldDefinition;
            tablaHash.Add("chequesNoCobradosEnElPeriodo", formulaFieldDefinition);

            formulaFieldDefinition = crReporte.DataDefinition.FormulaFields["UnboundCurrency4"] as FormulaFieldDefinition;
            tablaHash.Add("saldoRealDeRetirosEstadoDeCuenta", formulaFieldDefinition);


            formulaFieldDefinition = crReporte.DataDefinition.FormulaFields["UnboundCurrency8"] as FormulaFieldDefinition;
            tablaHash.Add("saldoDisponibleEnCuentaBancaria", formulaFieldDefinition);

            formulaFieldDefinition = crReporte.DataDefinition.FormulaFields["UnboundCurrency9"] as FormulaFieldDefinition;
            tablaHash.Add("saldoRealEnCuenta", formulaFieldDefinition);


            return (tablaHash);
        }

        private void MostrarEnFormulaFieldDefinitionDisponibleEnBancos(DataTable periodo, FormulaFieldDefinition formulaFieldDefinition)
        {
            DataRow filaUnica = periodo.AsEnumerable().Single();
            formulaFieldDefinition.Text = filaUnica.Field<decimal>("DisponibleEnBancos").ToString();
        }


        private void MostrarEnFormulaFieldDefinitionSumaDeTodosLosProductos(DataTable tabla, FormulaFieldDefinition formulaFieldDefinition)
        {
            DataRow filaUnica = tabla.Rows[0];
            decimal sumaTodosLosProductos;
            bool sePudoExtraerLaSuma;

            //la tabla contiene una unica fila con un valor diferente de 0 ó null
            sePudoExtraerLaSuma = Decimal.TryParse(filaUnica["suma del pago de todos los productos"].ToString(), out sumaTodosLosProductos);

            if (sePudoExtraerLaSuma)
                formulaFieldDefinition.Text = sumaTodosLosProductos.ToString();
            else
                formulaFieldDefinition.Text = "0.0000";
        }

        private void MostrarEnFormulaFieldDefinitionSumaDeTodosLosCheques(DataTable tabla, FormulaFieldDefinition formulaFieldDefinition)
        {
            DataRow filaUnica = tabla.Rows[0];
            decimal sumaTodosLosCheques;
            bool sePudoExtraerLaSuma;

            //la tabla contiene una unica fila con un valor diferente de 0 ó null
            sePudoExtraerLaSuma = Decimal.TryParse(filaUnica["suma de importes"].ToString(), out sumaTodosLosCheques);

            if (sePudoExtraerLaSuma)
                formulaFieldDefinition.Text = sumaTodosLosCheques.ToString();
            else
                formulaFieldDefinition.Text = "0.0000";
        }



        private void MostrarEnFormulaFieldDefinitionSumaDeChequesCobradosDePeriodosAnteriores(DataTable tabla, FormulaFieldDefinition formulaFieldDefinition,
            decimal inicialTotalDeChequesCobradosDePeriodosAnteriores, DateTime fechaDePeriodoInicialDeChequesCobradosDePeriodosAnteriores,
            RangoFechasUsadasEnReporte rango)
        {
            DataRow filaUnica = tabla.Rows[0];
            decimal sumaDeCheques = 0.0m;
            bool sePudoExtraerLaSuma = false;

            //la tabla contiene una unica fila con un valor diferente de 0 ó null
            sePudoExtraerLaSuma = Decimal.TryParse(filaUnica["suma de importes"].ToString(), out sumaDeCheques);

            if (sePudoExtraerLaSuma == false)
            { sumaDeCheques = 0.0000m; }

            if (rango.FechaInicio >= fechaDePeriodoInicialDeChequesCobradosDePeriodosAnteriores)
            {
                formulaFieldDefinition.Text = (sumaDeCheques + inicialTotalDeChequesCobradosDePeriodosAnteriores).ToString();
            }

            else
            {
                formulaFieldDefinition.Text = sumaDeCheques.ToString();
            }
        }

        private void MostrarEnFormulaFieldDefinitionSumaDeTodosLosChequeNoCobrados(DataTable tabla, FormulaFieldDefinition formulaFieldDefinition)
        {
            DataRow filaUnica = tabla.Rows[0];
            decimal sumaTodosLosCheques;
            bool sePudoExtraerLaSuma;

            //la tabla contiene una unica fila con un valor diferente de 0 ó null
            sePudoExtraerLaSuma = Decimal.TryParse(filaUnica["suma de importes"].ToString(), out sumaTodosLosCheques);

            if (sePudoExtraerLaSuma)
                formulaFieldDefinition.Text = sumaTodosLosCheques.ToString();
            else
                formulaFieldDefinition.Text = "0.0000";
        }


        private void MostrarEnFormulaFieldDefinitionSaldoRealDeRetirosEstadoDeCuenta(FormulaFieldDefinition suma1FormulaFieldDefinition,
            FormulaFieldDefinition suma2FormulaFieldDefinition, FormulaFieldDefinition restaFormulaFieldDefinition, 
            FormulaFieldDefinition destinoFormulaFieldDefinition)
        {
            decimal suma = Decimal.Parse(suma1FormulaFieldDefinition.Text) + Decimal.Parse(suma2FormulaFieldDefinition.Text) - Decimal.Parse(restaFormulaFieldDefinition.Text);
            destinoFormulaFieldDefinition.Text = suma.ToString();
        }

        private void MostrarEnFormulaFieldDefinitionSaldoDisponibleEnCuentaBancaria(FormulaFieldDefinition totalIngresos, FormulaFieldDefinition saldoRealRetirosEstadoDeCuenta, FormulaFieldDefinition disponibleEnBancosDePeriodo, FormulaFieldDefinition destino)
        {
            //TextObject totalIngresos, TextObject saldoRealRetirosEstadoDeCuenta, TextObject disponibleEnBancosDePeriodo, TextObject destino
            decimal suma = Decimal.Parse(totalIngresos.Text) + Decimal.Parse(saldoRealRetirosEstadoDeCuenta.Text);
            destino.Text = (Decimal.Parse(disponibleEnBancosDePeriodo.Text) - suma).ToString();
        }


        private void MostrarEnFormulaFieldDefinitionDisponibleEnBancosReal(DataTable periodo, FormulaFieldDefinition formulaFielDefinition)
        {
            DataRow filaUnica = periodo.AsEnumerable().Single();
            formulaFielDefinition.Text = filaUnica.Field<decimal>("DisponibleEnBancosReal").ToString();
        }


        private void ConfigurarOpcionesDeRPTParaExportacion(CRReporteEgresosIngresosMensualParaExportar reporte, string nomArchivo)
        {
            // Declare variables and get the export options.
            ExportOptions exportOpts = new ExportOptions();
            ExcelFormatOptions excelFormatOpts = new ExcelFormatOptions();
            DiskFileDestinationOptions diskOpts = new DiskFileDestinationOptions();
            exportOpts = reporte.ExportOptions;
            // Set the excel format options.
            excelFormatOpts.ExcelUseConstantColumnWidth = false;
            excelFormatOpts.ShowGridLines = true;

            //exportOpts.ExportFormatType = ExportFormatType.ExcelRecord;
            exportOpts.ExportFormatType = ExportFormatType.Excel;
            exportOpts.FormatOptions = excelFormatOpts;
            // Set the disk file options and export.
            exportOpts.ExportDestinationType = ExportDestinationType.DiskFile;
            //diskOpts.DiskFileName = "miotroreporttttte.xls";
            diskOpts.DiskFileName = nomArchivo;
            exportOpts.DestinationOptions = diskOpts;
        }


        //-------------------------Events
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (EstaSeleccionadoComboBoxAnioYComBoBoxMes())
                {
                    Hashtable parametrosFechaInicioFechaFin = GenerarParametrosParaReporte();
                    CRReporteEgresosIngresosMensual reporteEgresosIngresosMensual = new CRReporteEgresosIngresosMensual();
                    reporteEgresosIngresosMensual.SetDatabaseLogon("sa", "modomixto", "CRUZ2-THINK", "DBCajaCuentas2");
                    Hashtable tablaConTextObjectsDeCrystalReport = ObtenerTextObjectsDeCrystalReport(reporteEgresosIngresosMensual);
                    TextObject textObjectTitulo = reporteEgresosIngresosMensual.ReportDefinition.ReportObjects["Text13"] as TextObject;
                    PonerTituloAReporte(textObjectTitulo, "Reporte ingresos - egresos " + comboBox2.SelectedItem.ToString() + " " + comboBox1.SelectedItem.ToString());

                    DataTable totalInicialDeChequesCobradosDePeriodosAnterioresTable = InicialTotalDeChequesCobradosDePeriodosAnteriores_BuscarActivoController();
                    decimal inicialTotalDeChequesCobradosDePeriodosAnteriores = ExtraerTotalInicialDeChequesCobradosDePeriodosAnteriores(totalInicialDeChequesCobradosDePeriodosAnterioresTable);
                    DateTime fechaDePeriodoInicialDeChequesCobradosDePeriodosAnteriores = ExtraerFechaDePeriodoInicialDeChequesCobradosDePeriodosAnteriores(totalInicialDeChequesCobradosDePeriodosAnterioresTable);

                    List<string> mesConAnioElegido = MezclarAnioConMesElegido(comboBox1, comboBox2);

                    Action<string> FuncionParacadaItem = item =>
                    {
                        RangoFechasUsadasEnReporte rango = (RangoFechasUsadasEnReporte)parametrosFechaInicioFechaFin[item];
                       

                        DataTable periodoBancos = Bancos_BuscarPeriodoActivoController(rango.FechaInicio.Year, rango.NombreMes);                        
                        TextObject textObject = (TextObject)tablaConTextObjectsDeCrystalReport["disponibleEnBancos"];
                        MostrarEnTextBoxDisponibleEnBancos(periodoBancos, textObject);

                        TextObject textObjectX = (TextObject)tablaConTextObjectsDeCrystalReport["saldoRealEnCuenta"];
                        MostrarEnTextBoxDisponibleEnBancosReal(periodoBancos, textObjectX);

                        DataTable sumaDeProductosDelMes = MovsEnCaja_SumarPagoDeTodosProductosController(rango.FechaInicio, rango.FechaFin);                     
                        TextObject textObject2 = (TextObject)tablaConTextObjectsDeCrystalReport["totalIngresos"];
                        MostrarEnTextBoxSumaDeTodosLosProductos(sumaDeProductosDelMes, textObject2);

                        reporteEgresosIngresosMensual.SetParameterValue("@fechaInicio", rango.FechaInicio, reporteEgresosIngresosMensual.Subreports[0].Name.ToString());
                        reporteEgresosIngresosMensual.SetParameterValue("@fechaFin", rango.FechaFin, reporteEgresosIngresosMensual.Subreports[0].Name.ToString());
                                               
                        DataTable sumaDeImporteDeChequesDePeriodo = Cheque_SumarImporteDeChequesActivosController(rango.FechaInicio, rango.FechaFin);
                        TextObject textObject3 = (TextObject)tablaConTextObjectsDeCrystalReport["gastos"];
                        MostrarEnTextBoxSumaDeTodosLosCheques(sumaDeImporteDeChequesDePeriodo, textObject3); 


                        DateTime fechaCentinelaInicio = new DateTime(2000, 1, 1, 0, 1, 0);
                        DataTable sumaDeChequesCobradosDePeriodosAnteriores = Cheque_SumarImporteDeChequesCobradosController(fechaCentinelaInicio, rango.UltimoDiaDeMesAnterior);
                        TextObject textObject4 = (TextObject)tablaConTextObjectsDeCrystalReport["chequesCobradosDePeriodosAnteriores"];
                        MostrarEnTextBoxSumaDeChequesCobradosDePeriodosAnteriores(sumaDeChequesCobradosDePeriodosAnteriores, textObject4,
                            inicialTotalDeChequesCobradosDePeriodosAnteriores, fechaDePeriodoInicialDeChequesCobradosDePeriodosAnteriores, rango);

                        
                        DataTable sumaDeChequesNoCobradosDePeriodo = Cheque_SumarImporteDeChequesNoCobradosController(rango.FechaInicio, rango.FechaFin);
                        TextObject textObject5 = (TextObject)tablaConTextObjectsDeCrystalReport["chequesNoCobradosEnElPeriodo"];
                        MostrarEnTextBoxSumaDeTodosLosChequeNoCobrados(sumaDeChequesNoCobradosDePeriodo, textObject5);

                        TextObject textObject6 = (TextObject)tablaConTextObjectsDeCrystalReport["saldoRealDeRetirosEstadoDeCuenta"];
                        MostrarEnTextBoxSaldoRealDeRetirosEstadoDeCuenta(textObject3, textObject4, textObject5, textObject6);

                        reporteEgresosIngresosMensual.SetParameterValue("@fechaInicio", rango.FechaInicio, reporteEgresosIngresosMensual.Subreports[1].Name.ToString());
                        reporteEgresosIngresosMensual.SetParameterValue("@fechaFin", rango.FechaFin, reporteEgresosIngresosMensual.Subreports[1].Name.ToString());


                        TextObject textObject7 = (TextObject)tablaConTextObjectsDeCrystalReport["saldoDisponibleEnCuentaBancaria"];
                        MostrarEnTextBoxSaldoDisponibleEnCuentaBancaria(textObject2, textObject6, textObject, textObject7);

                    };


                    mesConAnioElegido.ForEach(FuncionParacadaItem);
                    crystalReportViewer1.ReportSource = reporteEgresosIngresosMensual;
                }

                else
                {
                    MessageBox.Show("Seleccione un año y un mes", "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (EstaSeleccionadoComboBoxAnioYComBoBoxMes())
                    {
                        Hashtable parametrosFechaInicioFechaFin = GenerarParametrosParaReporte();
                        CRReporteEgresosIngresosMensualParaExportar reporte = new CRReporteEgresosIngresosMensualParaExportar();
                        reporte.SetDatabaseLogon("sa", "modomixto", "CRUZ2-THINK", "DBCajaCuentas2");
                        Hashtable tablaConFormulaFieldDefinitionDeCrystalReport = ObtenerFormulaFieldDefinitionDeCR(reporte);
                        TextObject textObjectTitulo = reporte.ReportDefinition.ReportObjects["Text1"] as TextObject;
                        PonerTituloAReporte(textObjectTitulo, "Reporte ingresos - egresos " + comboBox2.SelectedItem.ToString() + " " + comboBox1.SelectedItem.ToString());

                        DataTable totalInicialDeChequesCobradosDePeriodosAnterioresTable = InicialTotalDeChequesCobradosDePeriodosAnteriores_BuscarActivoController();
                        decimal inicialTotalDeChequesCobradosDePeriodosAnteriores = ExtraerTotalInicialDeChequesCobradosDePeriodosAnteriores(totalInicialDeChequesCobradosDePeriodosAnterioresTable);
                        DateTime fechaDePeriodoInicialDeChequesCobradosDePeriodosAnteriores = ExtraerFechaDePeriodoInicialDeChequesCobradosDePeriodosAnteriores(totalInicialDeChequesCobradosDePeriodosAnterioresTable);

                        List<string> mesConAnioElegido = MezclarAnioConMesElegido(comboBox1, comboBox2);

                        Action<string> FuncionParaCadaItem = item =>
                        {
                            RangoFechasUsadasEnReporte rango = (RangoFechasUsadasEnReporte)parametrosFechaInicioFechaFin[item];

                            DataTable periodoBancos = Bancos_BuscarPeriodoActivoController(rango.FechaInicio.Year, rango.NombreMes);
                            FormulaFieldDefinition formulaFieldDefinition = (FormulaFieldDefinition)tablaConFormulaFieldDefinitionDeCrystalReport["disponibleEnBancos"];
                            MostrarEnFormulaFieldDefinitionDisponibleEnBancos(periodoBancos, formulaFieldDefinition);

                            DataTable sumaDeProductosDelMes = MovsEnCaja_SumarPagoDeTodosProductosController(rango.FechaInicio, rango.FechaFin);
                            FormulaFieldDefinition formulaFieldDefinition2 = (FormulaFieldDefinition)tablaConFormulaFieldDefinitionDeCrystalReport["totalIngresos"];
                            MostrarEnFormulaFieldDefinitionSumaDeTodosLosProductos(sumaDeProductosDelMes, formulaFieldDefinition2);

                            reporte.SetParameterValue("@fechaInicio", rango.FechaInicio, reporte.Subreports[0].Name.ToString());
                            reporte.SetParameterValue("@fechaFin", rango.FechaFin, reporte.Subreports[0].Name.ToString());

                            DataTable sumaDeImporteDeChequesDePeriodo = Cheque_SumarImporteDeChequesActivosController(rango.FechaInicio, rango.FechaFin);
                            FormulaFieldDefinition formulaFieldDefinition3 = (FormulaFieldDefinition)tablaConFormulaFieldDefinitionDeCrystalReport["gastos"];
                            MostrarEnFormulaFieldDefinitionSumaDeTodosLosCheques(sumaDeImporteDeChequesDePeriodo, formulaFieldDefinition3);

                            DateTime fechaCentinelaInicio = new DateTime(2000, 1, 1, 0, 1, 0);
                            DataTable sumaDeChequesCobradosDePeriodosAnteriores = Cheque_SumarImporteDeChequesCobradosController(fechaCentinelaInicio, rango.UltimoDiaDeMesAnterior);
                            FormulaFieldDefinition formulaFieldDefinition4 = (FormulaFieldDefinition)tablaConFormulaFieldDefinitionDeCrystalReport["chequesCobradosDePeriodosAnteriores"];
                            MostrarEnFormulaFieldDefinitionSumaDeChequesCobradosDePeriodosAnteriores(sumaDeChequesCobradosDePeriodosAnteriores, formulaFieldDefinition4,
                                inicialTotalDeChequesCobradosDePeriodosAnteriores, fechaDePeriodoInicialDeChequesCobradosDePeriodosAnteriores, rango);


                            DataTable sumaDeChequesNoCobradosDePeriodo = Cheque_SumarImporteDeChequesNoCobradosController(rango.FechaInicio, rango.FechaFin);
                            FormulaFieldDefinition formulaFieldDefinition5 = (FormulaFieldDefinition)tablaConFormulaFieldDefinitionDeCrystalReport["chequesNoCobradosEnElPeriodo"];
                            MostrarEnFormulaFieldDefinitionSumaDeTodosLosChequeNoCobrados(sumaDeChequesNoCobradosDePeriodo, formulaFieldDefinition5);

                            FormulaFieldDefinition formulaFieldDefinition6 = (FormulaFieldDefinition)tablaConFormulaFieldDefinitionDeCrystalReport["saldoRealDeRetirosEstadoDeCuenta"];
                            MostrarEnFormulaFieldDefinitionSaldoRealDeRetirosEstadoDeCuenta(formulaFieldDefinition3, formulaFieldDefinition4, formulaFieldDefinition5, formulaFieldDefinition6);

                            reporte.SetParameterValue("@fechaInicio", rango.FechaInicio, reporte.Subreports[1].Name.ToString());
                            reporte.SetParameterValue("@fechaFin", rango.FechaFin, reporte.Subreports[1].Name.ToString());

                            FormulaFieldDefinition formulaFieldDefinition7 = (FormulaFieldDefinition)tablaConFormulaFieldDefinitionDeCrystalReport["saldoDisponibleEnCuentaBancaria"];
                            MostrarEnFormulaFieldDefinitionSaldoDisponibleEnCuentaBancaria(formulaFieldDefinition2, formulaFieldDefinition6, formulaFieldDefinition, formulaFieldDefinition7);


                            FormulaFieldDefinition formulaFieldDefinitionX = (FormulaFieldDefinition)tablaConFormulaFieldDefinitionDeCrystalReport["saldoRealEnCuenta"];
                            MostrarEnFormulaFieldDefinitionDisponibleEnBancosReal(periodoBancos, formulaFieldDefinitionX);
                        };

                        mesConAnioElegido.ForEach(FuncionParaCadaItem);
                        ConfigurarOpcionesDeRPTParaExportacion(reporte, saveFileDialog1.FileName);
                        reporte.Export();

                        MessageBox.Show("Exportacion lista", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    else
                    {
                        MessageBox.Show("Seleccione un año y un mes", "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }
    }
}
