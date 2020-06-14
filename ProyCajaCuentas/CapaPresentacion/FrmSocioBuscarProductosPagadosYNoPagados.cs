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
using CrystalDecisions.Shared;
using System.Data.SqlClient;
using CapaLogicaNegocios;

namespace CapaPresentacion
{
    public partial class FrmSocioBuscarProductosPagadosYNoPagados : Form
    {

        //---------------------Constructor
        public FrmSocioBuscarProductosPagadosYNoPagados()
        {
            InitializeComponent();

            //Habilitar / deshabilitar el botón exportar a excel
            bool respuesta = EsActivoModuloController(46) ? true : false;
            if (respuesta)
            {   /*Permanece habilitado el botón exportar a excel*/  }
            else
            { button2.Enabled = false; }
        }

        //--------------------Methods
        private void DeshabilitarButtonsYDateTimePicker()
        {
            dateTimePicker1.Enabled = false;
            dateTimePicker2.Enabled = false;
            dateTimePicker3.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
        }

        private void HabilitarButtonsYDateTimePicker()
        {
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;

            if (radioButton1.Checked == true)
            {
                dateTimePicker1.Enabled = true;
                dateTimePicker2.Enabled = false;
                dateTimePicker3.Enabled = false;
            }

            else if (radioButton2.Checked == true)
            {
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = true;
                dateTimePicker3.Enabled = true;
            }

            button1.Enabled = true;

            //Habilitar / deshabilitar el botón exportar a excel
            bool respuesta = EsActivoModuloController(46) ? true : false;
            if (respuesta)
            { button2.Enabled = true;  }
            else
            { button2.Enabled = false; }
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

        private string MuestraFechaDeBusquedaSinLaHora(DateTime fecha)
        {
            return (fecha.ToShortDateString());
        }



        //-----------------Controllers
        private async Task<int> GenerarReporteExcelAsync(DateTime fechaInicio, DateTime fechaFin, string nomArchivo)
        {
            SqlConnectionStringBuilder sqlStrBuilder = new SqlConnectionStringBuilder(ObtenerCadenaConexionAppController());

            //http://aspalliance.com/478_Exporting_to_Excel_in_Crystal_Reports_NET__Perfect_Excel_Exports.3
            //https://www.c-sharpcorner.com/UploadFile/mahesh/savefiledialog-in-C-Sharp/
            CRReporteSociosProductosPagadosYNoPagadosParaExportar reporte = new CRReporteSociosProductosPagadosYNoPagadosParaExportar();
            //reporte.SetDatabaseLogon("sa", "modomixto", "CRUZ2-THINK", "DBCajaCuentas2");
            reporte.SetDatabaseLogon(sqlStrBuilder.UserID, sqlStrBuilder.Password, sqlStrBuilder.DataSource, sqlStrBuilder.InitialCatalog);
            reporte.SetParameterValue("@fechaInicio", fechaInicio);
            reporte.SetParameterValue("@fechaFin", fechaFin);

            //El subreporte cero es el ultimo subreporte y el subreporte 1 es el primer subreporte
            reporte.SetParameterValue("@fechaInicio", fechaInicio, reporte.Subreports[0].Name.ToString());
            reporte.SetParameterValue("@fechaFin", fechaFin, reporte.Subreports[0].Name.ToString());

            reporte.SetParameterValue("@fechaInicio", fechaInicio, reporte.Subreports[1].Name.ToString());
            reporte.SetParameterValue("@fechaFin", fechaFin, reporte.Subreports[1].Name.ToString());

            //Ponerle las fechas de busqueda al reporte
            TextObject periodoDeBusquedaTextObject = reporte.ReportDefinition.ReportObjects["Text6"] as TextObject;
            periodoDeBusquedaTextObject.Text = MuestraFechaDeBusquedaSinLaHora(fechaInicio) + " a " + MuestraFechaDeBusquedaSinLaHora(fechaFin);

            periodoDeBusquedaTextObject = reporte.Subreports[0].ReportDefinition.ReportObjects["Text6"] as TextObject;
            periodoDeBusquedaTextObject.Text = MuestraFechaDeBusquedaSinLaHora(fechaInicio) + " a " + MuestraFechaDeBusquedaSinLaHora(fechaFin);

            periodoDeBusquedaTextObject = reporte.Subreports[1].ReportDefinition.ReportObjects["Text6"] as TextObject;
            periodoDeBusquedaTextObject.Text = MuestraFechaDeBusquedaSinLaHora(fechaInicio) + " a " + MuestraFechaDeBusquedaSinLaHora(fechaFin);

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
            reporte.Export();

            await Task.Delay(200);
            return (1);
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


        private string ObtenerCadenaConexionAppController()
        {
            ClsEnlaceToAppConfig clsEnlaceToAppConfig = new ClsEnlaceToAppConfig();
            return (clsEnlaceToAppConfig.ObtenerCadenaConexionAppConfig());
        }

        //-------------------Events
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DeshabilitarButtonsYDateTimePicker();
                IniciarProgressBar();

                DateTime fechaInicio;
                DateTime fechaFin;

                if (radioButton1.Checked == true)
                {
                    fechaInicio = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month,
                        dateTimePicker1.Value.Day, 0, 1, 0);

                    fechaFin = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month,
                        dateTimePicker1.Value.Day, 23, 59, 58);
                }

                else
                {
                    fechaInicio = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month,
                        dateTimePicker2.Value.Day, 0, 1, 0);

                    fechaFin = new DateTime(dateTimePicker3.Value.Year, dateTimePicker3.Value.Month,
                        dateTimePicker3.Value.Day, 23, 59, 58);
                }


                SqlConnectionStringBuilder sqlStrBuilder = new SqlConnectionStringBuilder(ObtenerCadenaConexionAppController());

                CRReporteSociosProductosPagadosYNoPagados crReporte = new CRReporteSociosProductosPagadosYNoPagados();
                //crReporte.SetDatabaseLogon("sa", "modomixto", "CRUZ2-THINK", "DBCajaCuentas2");
                crReporte.SetDatabaseLogon(sqlStrBuilder.UserID, sqlStrBuilder.Password, sqlStrBuilder.DataSource, sqlStrBuilder.InitialCatalog);
                crReporte.SetParameterValue("@fechaInicio", fechaInicio);
                crReporte.SetParameterValue("@fechaFin", fechaFin);

                TextObject periodoDeBusquedaTextObject = crReporte.ReportDefinition.ReportObjects["Text6"] as TextObject;
                periodoDeBusquedaTextObject.Text = "periodo " + MuestraFechaDeBusquedaSinLaHora(fechaInicio) + " a " + MuestraFechaDeBusquedaSinLaHora(fechaFin);
                
                crReporte.SetParameterValue("@fechaInicio", fechaInicio, crReporte.Subreports[0].Name.ToString());
                crReporte.SetParameterValue("@fechaFin", fechaFin, crReporte.Subreports[0].Name.ToString());

                crReporte.SetParameterValue("@fechaInicio", fechaInicio, crReporte.Subreports[1].Name.ToString());
                crReporte.SetParameterValue("@fechaFin", fechaFin, crReporte.Subreports[1].Name.ToString());
 
                
                crystalReportViewer1.ReportSource = crReporte;
                DetenerProgressBar();
                HabilitarButtonsYDateTimePicker();               
            }

            catch (Exception ex)
            {
                DetenerProgressBar();
                HabilitarButtonsYDateTimePicker();
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == false)
            {
                //Entonces radioButton2.Checked es true, por lo tanto...
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = true;
                dateTimePicker3.Enabled = true;
            }

            else if (radioButton1.Checked == true)
            {
                //Entonces radioButton1.Checked es true, por lo tanto...
                dateTimePicker1.Enabled = true;
                dateTimePicker2.Enabled = false;
                dateTimePicker3.Enabled = false;
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fechaInicio;
                DateTime fechaFin;

                if (radioButton1.Checked == true)
                {
                    fechaInicio = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month,
                        dateTimePicker1.Value.Day, 0, 1, 0);

                    fechaFin = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month,
                        dateTimePicker1.Value.Day, 23, 59, 58);
                }

                else
                {
                    fechaInicio = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month,
    dateTimePicker2.Value.Day, 0, 1, 0);

                    fechaFin = new DateTime(dateTimePicker3.Value.Year, dateTimePicker3.Value.Month,
                        dateTimePicker3.Value.Day, 23, 59, 58);
                }


                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    DeshabilitarButtonsYDateTimePicker();
                    IniciarProgressBar();

                    string nomArchivo = saveFileDialog1.FileName;
                    await GenerarReporteExcelAsync(fechaInicio, fechaFin, nomArchivo);

                    DetenerProgressBar();
                    HabilitarButtonsYDateTimePicker();
                    MessageBox.Show("Exportacion lista", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            catch (Exception ex)
            {
                DetenerProgressBar();
                HabilitarButtonsYDateTimePicker();
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }
    }
}
