using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaLogicaNegocios;
using System.Windows.Forms;
using System.Collections;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace CapaPresentacion
{
    public partial class FrmVisorReporteIngresosEgresos : Form
    {

        //-----------------------constructor
        public FrmVisorReporteIngresosEgresos()
        {
            InitializeComponent();
            CargarComboBoxAnios();
            SelectFirtsElementInComboBox();

            try
            {
                if (EsActivoModuloController(40)) { /*El button exportar a excel esta habilitado*/ }
                else { DeshabilitarButtonExportarAExcel(); /* Se deshabilita botón exportar a excel*/}
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }

        //------------------Methods

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

        private DataTable Cheque_SumarImporteDeChequesNoCobradosController(DateTime fechaInicio, DateTime fechaFin)
        {
            ClsCheque clsCheque = new ClsCheque();
            clsCheque.FechaAlta = fechaInicio;
            clsCheque.FechaModificacion = fechaFin;

            return (clsCheque.Cheque_SumarImporteDeChequesNoCobrados());
        }


        private DataTable Bancos_BuscarPeriodoActivoController(int anio, string mes) 
        {
            ClsBancos clsBancos = new ClsBancos();
            clsBancos.PeriodoAnio = anio;
            clsBancos.PeriodoMes = mes;

            return (clsBancos.Bancos_BuscarPeriodoActivo());
        }


        private DataTable InicialTotalDeChequesCobradosDePeriodosAnteriores_BuscarActivoController()
        {
            ClsInicialTotalDeChequesCobradosDePeriodosAnteriores clsInicialTotalDeChequesCobradosDePeriodosAnteriores;
            clsInicialTotalDeChequesCobradosDePeriodosAnteriores = new ClsInicialTotalDeChequesCobradosDePeriodosAnteriores();

            return (clsInicialTotalDeChequesCobradosDePeriodosAnteriores.InicialTotalDeChequesCobradosDePeriodosAnteriores_BuscarActivo());
        }


        public bool EsActivoModuloController(int idModuloBuscado)
        {
            var lista = ClsLogin.ModulosALosQueTieneAccesoUsuario.AsEnumerable();
            DataRow filaBuscada = lista.FirstOrDefault(s =>
            (s.Field<int>(1) == idModuloBuscado) && (s.Field<bool>(2) == true));

            if (filaBuscada != null)
            {
                return true;
            }

            else
                return false;
        }

        //--------------------Utils
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
            {
                sumaDeCheques = 0.0000m;
            }
           
            if(rango.FechaInicio >= fechaDePeriodoInicialDeChequesCobradosDePeriodosAnteriores)
            {
                textBox.Text = (sumaDeCheques + inicialTotalDeChequesCobradosDePeriodosAnteriores).ToString();
            }

            else
            {
                textBox.Text = sumaDeCheques.ToString();
            }
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

            RangoFechasUsadasEnReporte enero2023 = new RangoFechasUsadasEnReporte(2023, 1, 1, 2023, 1, DateTime.DaysInMonth(2023, 1), "enero");
            RangoFechasUsadasEnReporte febrero2023 = new RangoFechasUsadasEnReporte(2023, 2, 1, 2023, 2, DateTime.DaysInMonth(2023, 2), "febrero");
            RangoFechasUsadasEnReporte marzo2023 = new RangoFechasUsadasEnReporte(2023, 3, 1, 2023, 3, DateTime.DaysInMonth(2023, 3), "marzo");
            RangoFechasUsadasEnReporte abril2023 = new RangoFechasUsadasEnReporte(2023, 4, 1, 2023, 4, DateTime.DaysInMonth(2023, 4), "abril");
            RangoFechasUsadasEnReporte mayo2023 = new RangoFechasUsadasEnReporte(2023, 5, 1, 2023, 5, DateTime.DaysInMonth(2023, 5), "mayo");
            RangoFechasUsadasEnReporte junio2023 = new RangoFechasUsadasEnReporte(2023, 6, 1, 2023, 6, DateTime.DaysInMonth(2023, 6), "junio");
            RangoFechasUsadasEnReporte julio2023 = new RangoFechasUsadasEnReporte(2023, 7, 1, 2023, 7, DateTime.DaysInMonth(2023, 7), "julio");
            RangoFechasUsadasEnReporte agosto2023 = new RangoFechasUsadasEnReporte(2023, 8, 1, 2023, 8, DateTime.DaysInMonth(2023, 8), "agosto");
            RangoFechasUsadasEnReporte septiembre2023 = new RangoFechasUsadasEnReporte(2023, 9, 1, 2023, 9, DateTime.DaysInMonth(2023, 9), "septiembre");
            RangoFechasUsadasEnReporte octubre2023 = new RangoFechasUsadasEnReporte(2023, 10, 1, 2023, 10, DateTime.DaysInMonth(2023, 10), "octubre");
            RangoFechasUsadasEnReporte noviembre2023 = new RangoFechasUsadasEnReporte(2023, 11, 1, 2023, 11, DateTime.DaysInMonth(2023, 11), "noviembre");
            RangoFechasUsadasEnReporte diciembre2023 = new RangoFechasUsadasEnReporte(2023, 12, 1, 2023, 12, DateTime.DaysInMonth(2023, 12), "diciembre");

            RangoFechasUsadasEnReporte enero2024 = new RangoFechasUsadasEnReporte(2024, 1, 1, 2024, 1, DateTime.DaysInMonth(2024, 1), "enero");
            RangoFechasUsadasEnReporte febrero2024 = new RangoFechasUsadasEnReporte(2024, 2, 1, 2024, 2, DateTime.DaysInMonth(2024, 2), "febrero");
            RangoFechasUsadasEnReporte marzo2024 = new RangoFechasUsadasEnReporte(2024, 3, 1, 2024, 3, DateTime.DaysInMonth(2024, 3), "marzo");
            RangoFechasUsadasEnReporte abril2024 = new RangoFechasUsadasEnReporte(2024, 4, 1, 2024, 4, DateTime.DaysInMonth(2024, 4), "abril");
            RangoFechasUsadasEnReporte mayo2024 = new RangoFechasUsadasEnReporte(2024, 5, 1, 2024, 5, DateTime.DaysInMonth(2024, 5), "mayo");
            RangoFechasUsadasEnReporte junio2024 = new RangoFechasUsadasEnReporte(2024, 6, 1, 2024, 6, DateTime.DaysInMonth(2024, 6), "junio");
            RangoFechasUsadasEnReporte julio2024 = new RangoFechasUsadasEnReporte(2024, 7, 1, 2024, 7, DateTime.DaysInMonth(2024, 7), "julio");
            RangoFechasUsadasEnReporte agosto2024 = new RangoFechasUsadasEnReporte(2024, 8, 1, 2024, 8, DateTime.DaysInMonth(2024, 8), "agosto");
            RangoFechasUsadasEnReporte septiembre2024 = new RangoFechasUsadasEnReporte(2024, 9, 1, 2024, 9, DateTime.DaysInMonth(2024, 9), "septiembre");
            RangoFechasUsadasEnReporte octubre2024 = new RangoFechasUsadasEnReporte(2024, 10, 1, 2024, 10, DateTime.DaysInMonth(2024, 10), "octubre");
            RangoFechasUsadasEnReporte noviembre2024 = new RangoFechasUsadasEnReporte(2024, 11, 1, 2024, 11, DateTime.DaysInMonth(2024, 11), "noviembre");
            RangoFechasUsadasEnReporte diciembre2024 = new RangoFechasUsadasEnReporte(2024, 12, 1, 2024, 12, DateTime.DaysInMonth(2024, 12), "diciembre");

            RangoFechasUsadasEnReporte enero2025 = new RangoFechasUsadasEnReporte(2025, 1, 1, 2025, 1, DateTime.DaysInMonth(2025, 1), "enero");
            RangoFechasUsadasEnReporte febrero2025 = new RangoFechasUsadasEnReporte(2025, 2, 1, 2025, 2, DateTime.DaysInMonth(2025, 2), "febrero");
            RangoFechasUsadasEnReporte marzo2025 = new RangoFechasUsadasEnReporte(2025, 3, 1, 2025, 3, DateTime.DaysInMonth(2025, 3), "marzo");
            RangoFechasUsadasEnReporte abril2025 = new RangoFechasUsadasEnReporte(2025, 4, 1, 2025, 4, DateTime.DaysInMonth(2025, 4), "abril");
            RangoFechasUsadasEnReporte mayo2025 = new RangoFechasUsadasEnReporte(2025, 5, 1, 2025, 5, DateTime.DaysInMonth(2025, 5), "mayo");
            RangoFechasUsadasEnReporte junio2025 = new RangoFechasUsadasEnReporte(2025, 6, 1, 2025, 6, DateTime.DaysInMonth(2025, 6), "junio");
            RangoFechasUsadasEnReporte julio2025 = new RangoFechasUsadasEnReporte(2025, 7, 1, 2025, 7, DateTime.DaysInMonth(2025, 7), "julio");
            RangoFechasUsadasEnReporte agosto2025 = new RangoFechasUsadasEnReporte(2025, 8, 1, 2025, 8, DateTime.DaysInMonth(2025, 8), "agosto");
            RangoFechasUsadasEnReporte septiembre2025 = new RangoFechasUsadasEnReporte(2025, 9, 1, 2025, 9, DateTime.DaysInMonth(2025, 9), "septiembre");
            RangoFechasUsadasEnReporte octubre2025 = new RangoFechasUsadasEnReporte(2025, 10, 1, 2025, 10, DateTime.DaysInMonth(2025, 10), "octubre");
            RangoFechasUsadasEnReporte noviembre2025 = new RangoFechasUsadasEnReporte(2025, 11, 1, 2025, 11, DateTime.DaysInMonth(2025, 11), "noviembre");
            RangoFechasUsadasEnReporte diciembre2025 = new RangoFechasUsadasEnReporte(2025, 12, 1, 2025, 12, DateTime.DaysInMonth(2025, 12), "diciembre");


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

            tablaHash.Add("enero2023", enero2023);
            tablaHash.Add("febrero2023", febrero2023);
            tablaHash.Add("marzo2023", marzo2023);
            tablaHash.Add("abril2023", abril2023);
            tablaHash.Add("mayo2023", mayo2023);
            tablaHash.Add("junio2023", junio2023);
            tablaHash.Add("julio2023", julio2023);
            tablaHash.Add("agosto2023", agosto2023);
            tablaHash.Add("septiembre2023", septiembre2023);
            tablaHash.Add("octubre2023", octubre2023);
            tablaHash.Add("noviembre2023", noviembre2023);
            tablaHash.Add("diciembre2023", diciembre2023);

            tablaHash.Add("enero2024", enero2024);
            tablaHash.Add("febrero2024", febrero2024);
            tablaHash.Add("marzo2024", marzo2024);
            tablaHash.Add("abril2024", abril2024);
            tablaHash.Add("mayo2024", mayo2024);
            tablaHash.Add("junio2024", junio2024);
            tablaHash.Add("julio2024", julio2024);
            tablaHash.Add("agosto2024", agosto2024);
            tablaHash.Add("septiembre2024", septiembre2024);
            tablaHash.Add("octubre2024", octubre2024);
            tablaHash.Add("noviembre2024", noviembre2024);
            tablaHash.Add("diciembre2024", diciembre2024);

            tablaHash.Add("enero2025", enero2025);
            tablaHash.Add("febrero2025", febrero2025);
            tablaHash.Add("marzo2025", marzo2025);
            tablaHash.Add("abril2025", abril2025);
            tablaHash.Add("mayo2025", mayo2025);
            tablaHash.Add("junio2025", junio2025);
            tablaHash.Add("julio2025", julio2025);
            tablaHash.Add("agosto2025", agosto2025);
            tablaHash.Add("septiembre2025", septiembre2025);
            tablaHash.Add("octubre2025", octubre2025);
            tablaHash.Add("noviembre2025", noviembre2025);
            tablaHash.Add("diciembre2025", diciembre2025);

            return (tablaHash);
        }

        private Hashtable ObtenerTextObjectsDeCrystalReport(CRReporteEgresosIngresos crReporteEgresosIngresos)
        {
            Hashtable tablaHash = new Hashtable();
            TextObject textObject;

            //Enero
            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text49"] as TextObject;
            tablaHash.Add("disponibleEnBancos_enero", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text61"] as TextObject;
            tablaHash.Add("totalIngresos_enero", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text73"] as TextObject;
            tablaHash.Add("gastos_enero", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text85"] as TextObject;
            tablaHash.Add("chequesCobradosDePeriodosAnteriores_enero", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text104"] as TextObject;
            tablaHash.Add("chequesNoCobradosEnElPeriodo_enero", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text118"] as TextObject;
            tablaHash.Add("saldoRealDeRetirosEstadoDeCuenta_enero", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text152"] as TextObject;
            tablaHash.Add("saldoDisponibleEnCuentaBancaria_enero", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text24"] as TextObject;
            tablaHash.Add("saldoRealEnCuenta_enero", textObject);

            //Febrero
            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text50"] as TextObject;
            tablaHash.Add("disponibleEnBancos_febrero", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text62"] as TextObject;
            tablaHash.Add("totalIngresos_febrero", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text74"] as TextObject;
            tablaHash.Add("gastos_febrero", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text86"] as TextObject;
            tablaHash.Add("chequesCobradosDePeriodosAnteriores_febrero", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text105"] as TextObject;
            tablaHash.Add("chequesNoCobradosEnElPeriodo_febrero", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text119"] as TextObject;
            tablaHash.Add("saldoRealDeRetirosEstadoDeCuenta_febrero", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text153"] as TextObject;
            tablaHash.Add("saldoDisponibleEnCuentaBancaria_febrero", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text25"] as TextObject;
            tablaHash.Add("saldoRealEnCuenta_febrero", textObject);

            //marzo
            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text51"] as TextObject;
            tablaHash.Add("disponibleEnBancos_marzo", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text63"] as TextObject;
            tablaHash.Add("totalIngresos_marzo", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text75"] as TextObject;
            tablaHash.Add("gastos_marzo", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text87"] as TextObject;
            tablaHash.Add("chequesCobradosDePeriodosAnteriores_marzo", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text106"] as TextObject;
            tablaHash.Add("chequesNoCobradosEnElPeriodo_marzo", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text120"] as TextObject;
            tablaHash.Add("saldoRealDeRetirosEstadoDeCuenta_marzo", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text154"] as TextObject;
            tablaHash.Add("saldoDisponibleEnCuentaBancaria_marzo", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text26"] as TextObject;
            tablaHash.Add("saldoRealEnCuenta_marzo", textObject);

            //Abril
            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text52"] as TextObject;
            tablaHash.Add("disponibleEnBancos_abril", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text64"] as TextObject;
            tablaHash.Add("totalIngresos_abril", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text76"] as TextObject;
            tablaHash.Add("gastos_abril", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text88"] as TextObject;
            tablaHash.Add("chequesCobradosDePeriodosAnteriores_abril", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text107"] as TextObject;
            tablaHash.Add("chequesNoCobradosEnElPeriodo_abril", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text121"] as TextObject;
            tablaHash.Add("saldoRealDeRetirosEstadoDeCuenta_abril", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text155"] as TextObject;
            tablaHash.Add("saldoDisponibleEnCuentaBancaria_abril", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text27"] as TextObject;
            tablaHash.Add("saldoRealEnCuenta_abril", textObject);

            //Mayo
            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text53"] as TextObject;
            tablaHash.Add("disponibleEnBancos_mayo", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text65"] as TextObject;
            tablaHash.Add("totalIngresos_mayo", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text77"] as TextObject;
            tablaHash.Add("gastos_mayo", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text89"] as TextObject;
            tablaHash.Add("chequesCobradosDePeriodosAnteriores_mayo", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text108"] as TextObject;
            tablaHash.Add("chequesNoCobradosEnElPeriodo_mayo", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text122"] as TextObject;
            tablaHash.Add("saldoRealDeRetirosEstadoDeCuenta_mayo", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text156"] as TextObject;
            tablaHash.Add("saldoDisponibleEnCuentaBancaria_mayo", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text28"] as TextObject;
            tablaHash.Add("saldoRealEnCuenta_mayo", textObject);

            //Junio
            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text54"] as TextObject;
            tablaHash.Add("disponibleEnBancos_junio", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text66"] as TextObject;
            tablaHash.Add("totalIngresos_junio", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text78"] as TextObject;
            tablaHash.Add("gastos_junio", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text90"] as TextObject;
            tablaHash.Add("chequesCobradosDePeriodosAnteriores_junio", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text109"] as TextObject;
            tablaHash.Add("chequesNoCobradosEnElPeriodo_junio", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text123"] as TextObject;
            tablaHash.Add("saldoRealDeRetirosEstadoDeCuenta_junio", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text157"] as TextObject;
            tablaHash.Add("saldoDisponibleEnCuentaBancaria_junio", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text29"] as TextObject;
            tablaHash.Add("saldoRealEnCuenta_junio", textObject);

            //julio
            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text55"] as TextObject;
            tablaHash.Add("disponibleEnBancos_julio", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text67"] as TextObject;
            tablaHash.Add("totalIngresos_julio", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text79"] as TextObject;
            tablaHash.Add("gastos_julio", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text91"] as TextObject;
            tablaHash.Add("chequesCobradosDePeriodosAnteriores_julio", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text110"] as TextObject;
            tablaHash.Add("chequesNoCobradosEnElPeriodo_julio", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text124"] as TextObject;
            tablaHash.Add("saldoRealDeRetirosEstadoDeCuenta_julio", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text158"] as TextObject;
            tablaHash.Add("saldoDisponibleEnCuentaBancaria_julio", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text30"] as TextObject;
            tablaHash.Add("saldoRealEnCuenta_julio", textObject);

            //agosto
            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text56"] as TextObject;
            tablaHash.Add("disponibleEnBancos_agosto", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text68"] as TextObject;
            tablaHash.Add("totalIngresos_agosto", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text80"] as TextObject;
            tablaHash.Add("gastos_agosto", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text92"] as TextObject;
            tablaHash.Add("chequesCobradosDePeriodosAnteriores_agosto", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text111"] as TextObject;
            tablaHash.Add("chequesNoCobradosEnElPeriodo_agosto", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text125"] as TextObject;
            tablaHash.Add("saldoRealDeRetirosEstadoDeCuenta_agosto", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text159"] as TextObject;
            tablaHash.Add("saldoDisponibleEnCuentaBancaria_agosto", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text31"] as TextObject;
            tablaHash.Add("saldoRealEnCuenta_agosto", textObject);

            //septiembre
            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text57"] as TextObject;
            tablaHash.Add("disponibleEnBancos_septiembre", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text69"] as TextObject;
            tablaHash.Add("totalIngresos_septiembre", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text81"] as TextObject;
            tablaHash.Add("gastos_septiembre", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text93"] as TextObject;
            tablaHash.Add("chequesCobradosDePeriodosAnteriores_septiembre", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text112"] as TextObject;
            tablaHash.Add("chequesNoCobradosEnElPeriodo_septiembre", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text126"] as TextObject;
            tablaHash.Add("saldoRealDeRetirosEstadoDeCuenta_septiembre", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text160"] as TextObject;
            tablaHash.Add("saldoDisponibleEnCuentaBancaria_septiembre", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text32"] as TextObject;
            tablaHash.Add("saldoRealEnCuenta_septiembre", textObject);

            //Octubre
            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text58"] as TextObject;
            tablaHash.Add("disponibleEnBancos_octubre", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text70"] as TextObject;
            tablaHash.Add("totalIngresos_octubre", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text82"] as TextObject;
            tablaHash.Add("gastos_octubre", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text94"] as TextObject;
            tablaHash.Add("chequesCobradosDePeriodosAnteriores_octubre", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text113"] as TextObject;
            tablaHash.Add("chequesNoCobradosEnElPeriodo_octubre", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text127"] as TextObject;
            tablaHash.Add("saldoRealDeRetirosEstadoDeCuenta_octubre", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text161"] as TextObject;
            tablaHash.Add("saldoDisponibleEnCuentaBancaria_octubre", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text33"] as TextObject;
            tablaHash.Add("saldoRealEnCuenta_octubre", textObject);

            //Noviembre
            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text48"] as TextObject;
            tablaHash.Add("disponibleEnBancos_noviembre", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text60"] as TextObject;
            tablaHash.Add("totalIngresos_noviembre", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text72"] as TextObject;
            tablaHash.Add("gastos_noviembre", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text84"] as TextObject;
            tablaHash.Add("chequesCobradosDePeriodosAnteriores_noviembre", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text103"] as TextObject;
            tablaHash.Add("chequesNoCobradosEnElPeriodo_noviembre", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text117"] as TextObject;
            tablaHash.Add("saldoRealDeRetirosEstadoDeCuenta_noviembre", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text151"] as TextObject;
            tablaHash.Add("saldoDisponibleEnCuentaBancaria_noviembre", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text34"] as TextObject;
            tablaHash.Add("saldoRealEnCuenta_noviembre", textObject);

            //Diciembre
            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text59"] as TextObject;
            tablaHash.Add("disponibleEnBancos_diciembre", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text71"] as TextObject;
            tablaHash.Add("totalIngresos_diciembre", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text83"] as TextObject;
            tablaHash.Add("gastos_diciembre", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text95"] as TextObject;
            tablaHash.Add("chequesCobradosDePeriodosAnteriores_diciembre", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text114"] as TextObject;
            tablaHash.Add("chequesNoCobradosEnElPeriodo_diciembre", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text128"] as TextObject;
            tablaHash.Add("saldoRealDeRetirosEstadoDeCuenta_diciembre", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text162"] as TextObject;
            tablaHash.Add("saldoDisponibleEnCuentaBancaria_diciembre", textObject);

            textObject = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text35"] as TextObject;
            tablaHash.Add("saldoRealEnCuenta_diciembre", textObject);

            return (tablaHash);
        }

        private Hashtable ObtenerFormulaFieldDefinitionDeCR_ParaExportar(CRReporteEgresosIngresosParaExportar cRReporteEgresosIngresosParaExportar)
        {
            Hashtable tablaHash = new Hashtable();
            FormulaFieldDefinition formulaFieldDefinition;

            //Enero
            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency1"] as FormulaFieldDefinition;
            tablaHash.Add("disponibleEnBancos_enero", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency2"] as FormulaFieldDefinition;
            tablaHash.Add("totalIngresos_enero", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency3"] as FormulaFieldDefinition;
            tablaHash.Add("gastos_enero", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency4"] as FormulaFieldDefinition;
            tablaHash.Add("chequesCobradosDePeriodosAnteriores_enero", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency5"] as FormulaFieldDefinition;
            tablaHash.Add("chequesNoCobradosEnElPeriodo_enero", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency6"] as FormulaFieldDefinition;
            tablaHash.Add("saldoRealDeRetirosEstadoDeCuenta_enero", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency7"] as FormulaFieldDefinition;
            tablaHash.Add("saldoDisponibleEnCuentaBancaria_enero", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency8"] as FormulaFieldDefinition;
            tablaHash.Add("saldoRealEnCuenta_enero", formulaFieldDefinition);

            //Febrero
            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency9"] as FormulaFieldDefinition;
            tablaHash.Add("disponibleEnBancos_febrero", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency10"] as FormulaFieldDefinition;
            tablaHash.Add("totalIngresos_febrero", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency11"] as FormulaFieldDefinition;
            tablaHash.Add("gastos_febrero", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency12"] as FormulaFieldDefinition;
            tablaHash.Add("chequesCobradosDePeriodosAnteriores_febrero", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency13"] as FormulaFieldDefinition;
            tablaHash.Add("chequesNoCobradosEnElPeriodo_febrero", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency14"] as FormulaFieldDefinition;
            tablaHash.Add("saldoRealDeRetirosEstadoDeCuenta_febrero", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency15"] as FormulaFieldDefinition;
            tablaHash.Add("saldoDisponibleEnCuentaBancaria_febrero", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency16"] as FormulaFieldDefinition;
            tablaHash.Add("saldoRealEnCuenta_febrero", formulaFieldDefinition);


            //marzo
            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency17"] as FormulaFieldDefinition;
            tablaHash.Add("disponibleEnBancos_marzo", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency18"] as FormulaFieldDefinition;
            tablaHash.Add("totalIngresos_marzo", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency19"] as FormulaFieldDefinition;
            tablaHash.Add("gastos_marzo", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency20"] as FormulaFieldDefinition;
            tablaHash.Add("chequesCobradosDePeriodosAnteriores_marzo", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency21"] as FormulaFieldDefinition;
            tablaHash.Add("chequesNoCobradosEnElPeriodo_marzo", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency22"] as FormulaFieldDefinition;
            tablaHash.Add("saldoRealDeRetirosEstadoDeCuenta_marzo", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency23"] as FormulaFieldDefinition;
            tablaHash.Add("saldoDisponibleEnCuentaBancaria_marzo", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency24"] as FormulaFieldDefinition;
            tablaHash.Add("saldoRealEnCuenta_marzo", formulaFieldDefinition);


            //Abril
            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency25"] as FormulaFieldDefinition;
            tablaHash.Add("disponibleEnBancos_abril", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency26"] as FormulaFieldDefinition;
            tablaHash.Add("totalIngresos_abril", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency27"] as FormulaFieldDefinition;
            tablaHash.Add("gastos_abril", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency28"] as FormulaFieldDefinition;
            tablaHash.Add("chequesCobradosDePeriodosAnteriores_abril", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency29"] as FormulaFieldDefinition;
            tablaHash.Add("chequesNoCobradosEnElPeriodo_abril", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency30"] as FormulaFieldDefinition;
            tablaHash.Add("saldoRealDeRetirosEstadoDeCuenta_abril", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency31"] as FormulaFieldDefinition;
            tablaHash.Add("saldoDisponibleEnCuentaBancaria_abril", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency32"] as FormulaFieldDefinition;
            tablaHash.Add("saldoRealEnCuenta_abril", formulaFieldDefinition);

            //Mayo
            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency33"] as FormulaFieldDefinition;
            tablaHash.Add("disponibleEnBancos_mayo", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency34"] as FormulaFieldDefinition;
            tablaHash.Add("totalIngresos_mayo", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency35"] as FormulaFieldDefinition;
            tablaHash.Add("gastos_mayo", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency36"] as FormulaFieldDefinition;
            tablaHash.Add("chequesCobradosDePeriodosAnteriores_mayo", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency37"] as FormulaFieldDefinition;
            tablaHash.Add("chequesNoCobradosEnElPeriodo_mayo", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency38"] as FormulaFieldDefinition;
            tablaHash.Add("saldoRealDeRetirosEstadoDeCuenta_mayo", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency39"] as FormulaFieldDefinition;
            tablaHash.Add("saldoDisponibleEnCuentaBancaria_mayo", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency40"] as FormulaFieldDefinition;
            tablaHash.Add("saldoRealEnCuenta_mayo", formulaFieldDefinition);


            //Junio
            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency41"] as FormulaFieldDefinition;
            tablaHash.Add("disponibleEnBancos_junio", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency42"] as FormulaFieldDefinition;
            tablaHash.Add("totalIngresos_junio", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency43"] as FormulaFieldDefinition;
            tablaHash.Add("gastos_junio", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency44"] as FormulaFieldDefinition;
            tablaHash.Add("chequesCobradosDePeriodosAnteriores_junio", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency45"] as FormulaFieldDefinition;
            tablaHash.Add("chequesNoCobradosEnElPeriodo_junio", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency46"] as FormulaFieldDefinition;
            tablaHash.Add("saldoRealDeRetirosEstadoDeCuenta_junio", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency47"] as FormulaFieldDefinition;
            tablaHash.Add("saldoDisponibleEnCuentaBancaria_junio", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency48"] as FormulaFieldDefinition;
            tablaHash.Add("saldoRealEnCuenta_junio", formulaFieldDefinition);


            //julio
            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency49"] as FormulaFieldDefinition;
            tablaHash.Add("disponibleEnBancos_julio", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency50"] as FormulaFieldDefinition;
            tablaHash.Add("totalIngresos_julio", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency51"] as FormulaFieldDefinition;
            tablaHash.Add("gastos_julio", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency52"] as FormulaFieldDefinition;
            tablaHash.Add("chequesCobradosDePeriodosAnteriores_julio", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency53"] as FormulaFieldDefinition;
            tablaHash.Add("chequesNoCobradosEnElPeriodo_julio", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency54"] as FormulaFieldDefinition;
            tablaHash.Add("saldoRealDeRetirosEstadoDeCuenta_julio", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency55"] as FormulaFieldDefinition;
            tablaHash.Add("saldoDisponibleEnCuentaBancaria_julio", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency56"] as FormulaFieldDefinition;
            tablaHash.Add("saldoRealEnCuenta_julio", formulaFieldDefinition);


            //agosto
            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency57"] as FormulaFieldDefinition;
            tablaHash.Add("disponibleEnBancos_agosto", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency58"] as FormulaFieldDefinition;
            tablaHash.Add("totalIngresos_agosto", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency59"] as FormulaFieldDefinition;
            tablaHash.Add("gastos_agosto", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency60"] as FormulaFieldDefinition;
            tablaHash.Add("chequesCobradosDePeriodosAnteriores_agosto", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency61"] as FormulaFieldDefinition;
            tablaHash.Add("chequesNoCobradosEnElPeriodo_agosto", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency62"] as FormulaFieldDefinition;
            tablaHash.Add("saldoRealDeRetirosEstadoDeCuenta_agosto", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency63"] as FormulaFieldDefinition;
            tablaHash.Add("saldoDisponibleEnCuentaBancaria_agosto", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency64"] as FormulaFieldDefinition;
            tablaHash.Add("saldoRealEnCuenta_agosto", formulaFieldDefinition);


            //septiembre
            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency65"] as FormulaFieldDefinition;
            tablaHash.Add("disponibleEnBancos_septiembre", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency66"] as FormulaFieldDefinition;
            tablaHash.Add("totalIngresos_septiembre", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency67"] as FormulaFieldDefinition;
            tablaHash.Add("gastos_septiembre", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency68"] as FormulaFieldDefinition;
            tablaHash.Add("chequesCobradosDePeriodosAnteriores_septiembre", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency69"] as FormulaFieldDefinition;
            tablaHash.Add("chequesNoCobradosEnElPeriodo_septiembre", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency70"] as FormulaFieldDefinition;
            tablaHash.Add("saldoRealDeRetirosEstadoDeCuenta_septiembre", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency71"] as FormulaFieldDefinition;
            tablaHash.Add("saldoDisponibleEnCuentaBancaria_septiembre", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency72"] as FormulaFieldDefinition;
            tablaHash.Add("saldoRealEnCuenta_septiembre", formulaFieldDefinition);


            //Octubre
            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency73"] as FormulaFieldDefinition;
            tablaHash.Add("disponibleEnBancos_octubre", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency74"] as FormulaFieldDefinition;
            tablaHash.Add("totalIngresos_octubre", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency75"] as FormulaFieldDefinition;
            tablaHash.Add("gastos_octubre", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency76"] as FormulaFieldDefinition;
            tablaHash.Add("chequesCobradosDePeriodosAnteriores_octubre", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency77"] as FormulaFieldDefinition;
            tablaHash.Add("chequesNoCobradosEnElPeriodo_octubre", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency78"] as FormulaFieldDefinition;
            tablaHash.Add("saldoRealDeRetirosEstadoDeCuenta_octubre", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency79"] as FormulaFieldDefinition;
            tablaHash.Add("saldoDisponibleEnCuentaBancaria_octubre", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency80"] as FormulaFieldDefinition;
            tablaHash.Add("saldoRealEnCuenta_octubre", formulaFieldDefinition);


            //Noviembre
            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency81"] as FormulaFieldDefinition;
            tablaHash.Add("disponibleEnBancos_noviembre", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency82"] as FormulaFieldDefinition;
            tablaHash.Add("totalIngresos_noviembre", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency83"] as FormulaFieldDefinition;
            tablaHash.Add("gastos_noviembre", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency84"] as FormulaFieldDefinition;
            tablaHash.Add("chequesCobradosDePeriodosAnteriores_noviembre", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency85"] as FormulaFieldDefinition;
            tablaHash.Add("chequesNoCobradosEnElPeriodo_noviembre", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency86"] as FormulaFieldDefinition;
            tablaHash.Add("saldoRealDeRetirosEstadoDeCuenta_noviembre", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency87"] as FormulaFieldDefinition;
            tablaHash.Add("saldoDisponibleEnCuentaBancaria_noviembre", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency88"] as FormulaFieldDefinition;
            tablaHash.Add("saldoRealEnCuenta_noviembre", formulaFieldDefinition);


            //Diciembre
            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency89"] as FormulaFieldDefinition;
            tablaHash.Add("disponibleEnBancos_diciembre", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency90"] as FormulaFieldDefinition;
            tablaHash.Add("totalIngresos_diciembre", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency91"] as FormulaFieldDefinition;
            tablaHash.Add("gastos_diciembre", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency92"] as FormulaFieldDefinition;
            tablaHash.Add("chequesCobradosDePeriodosAnteriores_diciembre", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency93"] as FormulaFieldDefinition;
            tablaHash.Add("chequesNoCobradosEnElPeriodo_diciembre", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency94"] as FormulaFieldDefinition;
            tablaHash.Add("saldoRealDeRetirosEstadoDeCuenta_diciembre", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency95"] as FormulaFieldDefinition;
            tablaHash.Add("saldoDisponibleEnCuentaBancaria_diciembre", formulaFieldDefinition);

            formulaFieldDefinition = cRReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency96"] as FormulaFieldDefinition;
            tablaHash.Add("saldoRealEnCuenta_diciembre", formulaFieldDefinition);

            return (tablaHash);
        }

        private void MostrarEnFormulaFieldDefinitionDisponibleEnBancos(DataTable periodo, FormulaFieldDefinition formulaFieldDefinition)
        {
            DataRow filaUnica = periodo.AsEnumerable().Single();
            formulaFieldDefinition.Text = filaUnica.Field<decimal>("DisponibleEnBancos").ToString();
        }

        private void MostrarEnFormulaFieldDefinitionDisponibleEnBancosReal(DataTable periodo, FormulaFieldDefinition formulaFieldDefinition)
        {
            DataRow filaUnica = periodo.AsEnumerable().Single();
            formulaFieldDefinition.Text = filaUnica.Field<decimal>("DisponibleEnBancosReal").ToString();
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
            {
                sumaDeCheques = 0.0000m;
            }

            if (rango.FechaInicio >= fechaDePeriodoInicialDeChequesCobradosDePeriodosAnteriores)
            {
                formulaFieldDefinition.Text = (sumaDeCheques + inicialTotalDeChequesCobradosDePeriodosAnteriores).ToString();
            }

            else
            {
                formulaFieldDefinition.Text = sumaDeCheques.ToString();
            }
        }

        private void MostrarEnFormulaFieldDefinitionSumaDeTodosLosChequesNoCobrados(DataTable tabla, FormulaFieldDefinition formulaFieldDefinition)
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

        private void MostrarEnFormulaFieldDefinitionSaldoRealDeRetirosEstadoDeCuenta(FormulaFieldDefinition gastos, 
            FormulaFieldDefinition chequesCobradosDePeriodosAnteriores, FormulaFieldDefinition chequesNoCobradosEnElPeriodo,
            FormulaFieldDefinition destino)
        {
            decimal suma = Decimal.Parse(gastos.Text) + Decimal.Parse(chequesCobradosDePeriodosAnteriores.Text) - Decimal.Parse(chequesNoCobradosEnElPeriodo.Text);
            destino.Text = suma.ToString();
        }

        private void MostrarEnFormulaFieldDefinitionSaldoDisponibleEnCuentaBancaria(FormulaFieldDefinition totalIngresos, FormulaFieldDefinition saldoRealRetirosEstadoDeCuenta, FormulaFieldDefinition disponibleEnBancosDePeriodo, FormulaFieldDefinition destino)
        {
            decimal suma = Decimal.Parse(totalIngresos.Text) + Decimal.Parse(saldoRealRetirosEstadoDeCuenta.Text);
            destino.Text = (Decimal.Parse(disponibleEnBancosDePeriodo.Text) - suma).ToString();
        }


        private void CargarComboBoxAnios()
        {
            comboBox1.Items.Add("2018");
            comboBox1.Items.Add("2019");
            comboBox1.Items.Add("2020");
            comboBox1.Items.Add("2021");
            comboBox1.Items.Add("2022");
            comboBox1.Items.Add("2023");
            comboBox1.Items.Add("2024");
            comboBox1.Items.Add("2025");
        }

        private void SelectFirtsElementInComboBox()
        {
            comboBox1.SelectedIndex = 0;
        }

        private List<string> MezclarMesesConAnioElegido(string anioDeComboBox)
        {
            List<string> mezclaDeAniosConMeses = new List<string>();

            List<string> meses = new List<string>();
            meses.Add("enero");
            meses.Add("febrero");
            meses.Add("marzo");
            meses.Add("abril");
            meses.Add("mayo");
            meses.Add("junio");
            meses.Add("julio");
            meses.Add("agosto");
            meses.Add("septiembre");
            meses.Add("octubre");
            meses.Add("noviembre");
            meses.Add("diciembre");

            var x = from item in meses
                    select item + anioDeComboBox.ToString();

            return (x.ToList<string>());           
        }

        private string QuitarAnioYUnirConTexto(string itemMesAnio, string texto)
        {
            int longitud = itemMesAnio.Length;
            string cadenaMes = itemMesAnio.Substring(0, itemMesAnio.Length - 4);  //le quito los 4 ultimos digitos del año, 2018, 2019, etc

            return (texto + cadenaMes);          
        }

        private void PonerTituloAReporte(TextObject textObject, string texto)
        {
            textObject.Text = texto; 
        }

        private bool EstaSeleccionadoComboBox(ComboBox comboBox)
        {
            return (comboBox.SelectedIndex > -1);
        }

        private decimal ExtraerTotalInicialDeChequesCobradosDePeriodosAnteriores(DataTable inicialTotalDeChequesCobradosDePeriodosAnteriores)
        {
            var res = inicialTotalDeChequesCobradosDePeriodosAnteriores.AsEnumerable();
            List<DataRow> listaElementos = res.ToList<DataRow>();  //Esa lista solo contiene un elemento o esta vacia

            DataRow filaUnica = listaElementos.SingleOrDefault<DataRow>();
            if (filaUnica != null)
                return( filaUnica.Field<decimal>("Total") );
            else
                return( 0.0m );
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

        private void ConfigurarOpcionesDeRPTParaExportacion(CRReporteEgresosIngresosParaExportar reporte, string nomArchivo)
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


        private void DeshabilitarButtonExportarAExcel()
        {
            button2.Enabled = false;
        }


        private void InhabilitarComboBoxYButtons()
        {
            comboBox1.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
        }

        private void HabilitarComboBoxYButtons()
        {
            comboBox1.Enabled = true;
            button1.Enabled = true;
            button2.Enabled = EsActivoModuloController(40) ? true : false;
        }

        private void IniciarProgressBar()
        {
            progressBar1.Visible = true;
            progressBar1.Value = 0;
            progressBar1.MarqueeAnimationSpeed = 30;
            progressBar1.Style = ProgressBarStyle.Marquee;
        }

        private void DetenerProgressBar()
        {
            progressBar1.Value = 0;
            progressBar1.MarqueeAnimationSpeed = 100;
            progressBar1.Style = ProgressBarStyle.Blocks;
            progressBar1.Visible = false;
        }

        //----------------------Events
        private async void button1_Click(object sender, EventArgs e)
        {

            try
            {
                if( EstaSeleccionadoComboBox(comboBox1) )
                {
                    InhabilitarComboBoxYButtons();
                    IniciarProgressBar();

                    Hashtable parametrosFechaInicioFechaFin = GenerarParametrosParaReporte();
                    CRReporteEgresosIngresos crReporteEgresosIngresos = new CRReporteEgresosIngresos();
                    Hashtable tablaConTextObjectsDeCrystalReport = ObtenerTextObjectsDeCrystalReport(crReporteEgresosIngresos);
                    TextObject textObjectTitulo = crReporteEgresosIngresos.ReportDefinition.ReportObjects["Text1"] as TextObject;
                    PonerTituloAReporte(textObjectTitulo, "Reporte ingresos - egresos " + comboBox1.SelectedItem.ToString());

                    DataTable totalInicialDeChequesCobradosDePeriodosAnterioresTable = InicialTotalDeChequesCobradosDePeriodosAnteriores_BuscarActivoController();
                    decimal inicialTotalDeChequesCobradosDePeriodosAnteriores = ExtraerTotalInicialDeChequesCobradosDePeriodosAnteriores(totalInicialDeChequesCobradosDePeriodosAnterioresTable);
                    DateTime fechaDePeriodoInicialDeChequesCobradosDePeriodosAnteriores = ExtraerFechaDePeriodoInicialDeChequesCobradosDePeriodosAnteriores(totalInicialDeChequesCobradosDePeriodosAnterioresTable);

                    List<string> mesesConAnioElegido = MezclarMesesConAnioElegido(comboBox1.SelectedItem.ToString());
                    Action<string> FuncionParaCadaItem = item =>
                    {
                        RangoFechasUsadasEnReporte rango = (RangoFechasUsadasEnReporte)parametrosFechaInicioFechaFin[item];

                        DataTable periodoBancos = Bancos_BuscarPeriodoActivoController(rango.FechaInicio.Year, rango.NombreMes);
                        string llave = QuitarAnioYUnirConTexto(item, "disponibleEnBancos_");
                        TextObject textObject = (TextObject)tablaConTextObjectsDeCrystalReport[llave];
                        MostrarEnTextBoxDisponibleEnBancos(periodoBancos, textObject);

                        string llaveX = QuitarAnioYUnirConTexto(item, "saldoRealEnCuenta_");
                        TextObject textObjectX = (TextObject)tablaConTextObjectsDeCrystalReport[llaveX];
                        MostrarEnTextBoxDisponibleEnBancosReal(periodoBancos, textObjectX);

                        DataTable sumaDeProductosDelMes = MovsEnCaja_SumarPagoDeTodosProductosController(rango.FechaInicio, rango.FechaFin);
                        string llave2 = QuitarAnioYUnirConTexto(item, "totalIngresos_");
                        TextObject textObject2 = (TextObject)tablaConTextObjectsDeCrystalReport[llave2];
                        MostrarEnTextBoxSumaDeTodosLosProductos(sumaDeProductosDelMes, textObject2);

                        DataTable sumaDeImporteDeChequesDePeriodo = Cheque_SumarImporteDeChequesActivosController(rango.FechaInicio, rango.FechaFin);
                        string llave3 = QuitarAnioYUnirConTexto(item, "gastos_");
                        TextObject textObject3 = (TextObject)tablaConTextObjectsDeCrystalReport[llave3];
                        MostrarEnTextBoxSumaDeTodosLosCheques(sumaDeImporteDeChequesDePeriodo, textObject3);

                        DateTime fechaCentinelaInicio = new DateTime(2000, 1, 1, 0, 1, 0);
                        DataTable sumaDeChequesCobradosDePeriodosAnteriores = Cheque_SumarImporteDeChequesCobradosController(fechaCentinelaInicio, rango.UltimoDiaDeMesAnterior);
                        string llave4 = QuitarAnioYUnirConTexto(item, "chequesCobradosDePeriodosAnteriores_");
                        TextObject textObject4 = (TextObject)tablaConTextObjectsDeCrystalReport[llave4];
                        MostrarEnTextBoxSumaDeChequesCobradosDePeriodosAnteriores(sumaDeChequesCobradosDePeriodosAnteriores, textObject4, 
                            inicialTotalDeChequesCobradosDePeriodosAnteriores, fechaDePeriodoInicialDeChequesCobradosDePeriodosAnteriores,rango);

                       
                        DataTable sumaDeChequesNoCobradosDePeriodo = Cheque_SumarImporteDeChequesNoCobradosController(rango.FechaInicio, rango.FechaFin);
                        string llave5 = QuitarAnioYUnirConTexto(item, "chequesNoCobradosEnElPeriodo_");
                        TextObject textObject5 = (TextObject)tablaConTextObjectsDeCrystalReport[llave5];
                        MostrarEnTextBoxSumaDeTodosLosChequeNoCobrados(sumaDeChequesNoCobradosDePeriodo, textObject5);

                        string llave6 = QuitarAnioYUnirConTexto(item, "saldoRealDeRetirosEstadoDeCuenta_");
                        TextObject textObject6 = (TextObject)tablaConTextObjectsDeCrystalReport[llave6];
                        MostrarEnTextBoxSaldoRealDeRetirosEstadoDeCuenta(textObject3, textObject4, textObject5, textObject6);


                        string llave7 = QuitarAnioYUnirConTexto(item, "saldoDisponibleEnCuentaBancaria_");
                        TextObject textObject7 = (TextObject)tablaConTextObjectsDeCrystalReport[llave7];
                        MostrarEnTextBoxSaldoDisponibleEnCuentaBancaria(textObject2, textObject6, textObject, textObject7);
                    };

                    mesesConAnioElegido.ForEach(FuncionParaCadaItem);
                    crystalReportViewer1.ReportSource = crReporteEgresosIngresos;

                    await Task.Delay(10);
                    DetenerProgressBar();
                    HabilitarComboBoxYButtons();
                }

                else
                {
                    MessageBox.Show("Seleccione un año", "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                ClsMyException clsMyException = new ClsMyException();
                string res = clsMyException.FormarTextoDeSqlException(ex);

                MessageBox.Show(res, "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            catch (Exception ex)
            {
                DetenerProgressBar();
                HabilitarComboBoxYButtons();
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }


        private void FrmVisorReporteIngresosEgresos_Load(object sender, EventArgs e)
        {

        }

        private async void button2_Click(object sender, EventArgs e)
        {

            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (EstaSeleccionadoComboBox(comboBox1))
                    {
                        InhabilitarComboBoxYButtons();
                        IniciarProgressBar();

                        Hashtable parametrosFechaInicioFechaFin = GenerarParametrosParaReporte();
                        CRReporteEgresosIngresosParaExportar crReporteEgresosIngresosParaExportar = new CRReporteEgresosIngresosParaExportar();
                        Hashtable tablaConFormulaFieldDefinitionDeCrystalReport = ObtenerFormulaFieldDefinitionDeCR_ParaExportar(crReporteEgresosIngresosParaExportar);
                        TextObject textObjectTitulo = crReporteEgresosIngresosParaExportar.ReportDefinition.ReportObjects["Text132"] as TextObject;
                        PonerTituloAReporte(textObjectTitulo, "Reporte ingresos - egresos " + comboBox1.SelectedItem.ToString());

                        //FormulaFieldDefinition fx = crReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency1"] as FormulaFieldDefinition;
                        //fx.Text = "98.0060";

                        //Asi se accede a FieldObject
                        //crReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundCurrency1"].Text = "560";
                        //crReporteEgresosIngresosParaExportar.DataDefinition.FormulaFields["UnboundNumber1"].Text = "569";


                        DataTable totalInicialDeChequesCobradosDePeriodosAnterioresTable = InicialTotalDeChequesCobradosDePeriodosAnteriores_BuscarActivoController();
                        decimal inicialTotalDeChequesCobradosDePeriodosAnteriores = ExtraerTotalInicialDeChequesCobradosDePeriodosAnteriores(totalInicialDeChequesCobradosDePeriodosAnterioresTable);
                        DateTime fechaDePeriodoInicialDeChequesCobradosDePeriodosAnteriores = ExtraerFechaDePeriodoInicialDeChequesCobradosDePeriodosAnteriores(totalInicialDeChequesCobradosDePeriodosAnterioresTable);

                        List<string> mesesConAnioElegido = MezclarMesesConAnioElegido(comboBox1.SelectedItem.ToString());
                        Action<string> FuncionParaCadaItem = item =>
                        {
                            RangoFechasUsadasEnReporte rango = (RangoFechasUsadasEnReporte)parametrosFechaInicioFechaFin[item];

                            DataTable periodoBancos = Bancos_BuscarPeriodoActivoController(rango.FechaInicio.Year, rango.NombreMes);
                            string llave = QuitarAnioYUnirConTexto(item, "disponibleEnBancos_");
                            FormulaFieldDefinition formulaFieldDefinition = (FormulaFieldDefinition)tablaConFormulaFieldDefinitionDeCrystalReport[llave];
                            MostrarEnFormulaFieldDefinitionDisponibleEnBancos(periodoBancos, formulaFieldDefinition);

                            
                            string llaveX = QuitarAnioYUnirConTexto(item, "saldoRealEnCuenta_");
                            FormulaFieldDefinition formulaFieldDefinitionX = (FormulaFieldDefinition)tablaConFormulaFieldDefinitionDeCrystalReport[llaveX];
                            MostrarEnFormulaFieldDefinitionDisponibleEnBancosReal(periodoBancos, formulaFieldDefinitionX);

                            DataTable sumaDeProductosDelMes = MovsEnCaja_SumarPagoDeTodosProductosController(rango.FechaInicio, rango.FechaFin);
                            string llave2 = QuitarAnioYUnirConTexto(item, "totalIngresos_");
                            FormulaFieldDefinition formulaFieldDefinition2 = (FormulaFieldDefinition)tablaConFormulaFieldDefinitionDeCrystalReport[llave2];
                            MostrarEnFormulaFieldDefinitionSumaDeTodosLosProductos(sumaDeProductosDelMes, formulaFieldDefinition2);

                            DataTable sumaDeImporteDeChequesDePeriodo = Cheque_SumarImporteDeChequesActivosController(rango.FechaInicio, rango.FechaFin);
                            string llave3 = QuitarAnioYUnirConTexto(item, "gastos_");
                            FormulaFieldDefinition formulaFieldDefinition3 = (FormulaFieldDefinition)tablaConFormulaFieldDefinitionDeCrystalReport[llave3];
                            MostrarEnFormulaFieldDefinitionSumaDeTodosLosCheques(sumaDeImporteDeChequesDePeriodo, formulaFieldDefinition3);

                            DateTime fechaCentinelaInicio = new DateTime(2000, 1, 1, 0, 1, 0);
                            DataTable sumaDeChequesCobradosDePeriodosAnteriores = Cheque_SumarImporteDeChequesCobradosController(fechaCentinelaInicio, rango.UltimoDiaDeMesAnterior);
                            string llave4 = QuitarAnioYUnirConTexto(item, "chequesCobradosDePeriodosAnteriores_");
                            FormulaFieldDefinition formulaFieldDefinition4 = (FormulaFieldDefinition)tablaConFormulaFieldDefinitionDeCrystalReport[llave4];
                            MostrarEnFormulaFieldDefinitionSumaDeChequesCobradosDePeriodosAnteriores(sumaDeChequesCobradosDePeriodosAnteriores, formulaFieldDefinition4,
                                inicialTotalDeChequesCobradosDePeriodosAnteriores, fechaDePeriodoInicialDeChequesCobradosDePeriodosAnteriores, rango);


                            DataTable sumaDeChequesNoCobradosDePeriodo = Cheque_SumarImporteDeChequesNoCobradosController(rango.FechaInicio, rango.FechaFin);
                            string llave5 = QuitarAnioYUnirConTexto(item, "chequesNoCobradosEnElPeriodo_");
                            FormulaFieldDefinition formulaFieldDefinition5 = (FormulaFieldDefinition)tablaConFormulaFieldDefinitionDeCrystalReport[llave5];
                            MostrarEnFormulaFieldDefinitionSumaDeTodosLosChequesNoCobrados(sumaDeChequesNoCobradosDePeriodo, formulaFieldDefinition5);


                            string llave6 = QuitarAnioYUnirConTexto(item, "saldoRealDeRetirosEstadoDeCuenta_");
                            FormulaFieldDefinition formulaFieldDefinition6 = (FormulaFieldDefinition)tablaConFormulaFieldDefinitionDeCrystalReport[llave6];
                            MostrarEnFormulaFieldDefinitionSaldoRealDeRetirosEstadoDeCuenta(formulaFieldDefinition3, formulaFieldDefinition4,
                                formulaFieldDefinition5, formulaFieldDefinition6);


                            string llave7 = QuitarAnioYUnirConTexto(item, "saldoDisponibleEnCuentaBancaria_");
                            FormulaFieldDefinition formulaFieldDefinition7 = (FormulaFieldDefinition)tablaConFormulaFieldDefinitionDeCrystalReport[llave7];
                            MostrarEnFormulaFieldDefinitionSaldoDisponibleEnCuentaBancaria(formulaFieldDefinition2, formulaFieldDefinition6,
                                formulaFieldDefinition, formulaFieldDefinition7);
                            
                        };

                        mesesConAnioElegido.ForEach(FuncionParaCadaItem);
                        ConfigurarOpcionesDeRPTParaExportacion(crReporteEgresosIngresosParaExportar, saveFileDialog1.FileName);
                        crReporteEgresosIngresosParaExportar.Export();

                        await Task.Delay(10);
                        DetenerProgressBar();
                        HabilitarComboBoxYButtons();
                        MessageBox.Show("Exportacion lista", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    else
                    {
                        MessageBox.Show("Seleccione un año", "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                ClsMyException clsMyException = new ClsMyException();
                string res = clsMyException.FormarTextoDeSqlException(ex);

                MessageBox.Show(res, "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            catch (Exception ex)
            {
                DetenerProgressBar();
                HabilitarComboBoxYButtons();
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }
    }




    public class RangoFechasUsadasEnReporte
    {
        public DateTime FechaInicio { set; get; }
        public DateTime FechaFin { set; get; }
        public System.String NombreMes { set; get; }
        public DateTime UltimoDiaDeMesAnterior { set; get; }

        //--------------constructor
        public RangoFechasUsadasEnReporte(int anioInicio, int mesInicio, int diaInicio, 
            int anioFin, int mesFin, int diaFin, string nombreMes )
        {
            this.FechaInicio = new DateTime(anioInicio, mesInicio, diaInicio, 0, 1, 0 );
            this.FechaFin = new DateTime(anioFin, mesFin, diaFin, 23, 59, 58);
            this.NombreMes = nombreMes;
            this.UltimoDiaDeMesAnterior = ObtenerDiaUltimoDeMesAnterior(anioInicio, mesInicio);

        }

        private DateTime ObtenerDiaUltimoDeMesAnterior(int anioInicio, int mesInicio)
        {
            if( mesInicio == 1)
            {
                DateTime ultimoDiaDeMesAnterior = new DateTime(anioInicio - 1, 12, DateTime.DaysInMonth(anioInicio - 1, 12), 23,59, 58);
                return (ultimoDiaDeMesAnterior);
            }

            else
            {
                DateTime ultimoDiaDeMesAnterior = new DateTime(anioInicio, mesInicio - 1, DateTime.DaysInMonth(anioInicio, mesInicio - 1), 23, 59, 58);
                return (ultimoDiaDeMesAnterior);
            }
        }
    }

    


}
