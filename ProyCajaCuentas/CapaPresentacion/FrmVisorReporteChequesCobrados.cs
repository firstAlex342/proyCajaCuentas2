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
    public partial class FrmVisorReporteChequesCobrados : Form
    {
        //----------------------Constructor
        public FrmVisorReporteChequesCobrados()
        {
            InitializeComponent();
        }

        //--------------------Methods controller
        private async Task<DataTable> Cheque_RecuperarDetallesDeChequesCobradosControllerAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            ClsCheque clsCheque = new ClsCheque();
            clsCheque.FechaAlta = fechaInicio;
            clsCheque.FechaModificacion = fechaFin;

            DataTable res = clsCheque.Cheque_RecuperarDetallesDeChequesCobrados();
            await Task.Delay(200);
            return (res);
        }

        private string ObtenerCadenaConexionAppController()
        {
            ClsEnlaceToAppConfig clsEnlaceToAppConfig = new ClsEnlaceToAppConfig();
            return (clsEnlaceToAppConfig.ObtenerCadenaConexionAppConfig());
        }


        //------------------------Utils
        private string MuestraFechaDeBusquedaSinLaHora(DateTime fecha)
        {
            return (fecha.ToShortDateString());
        }

        private async Task<int> CargarCRViewerAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            SqlConnectionStringBuilder sqlStrBuilder = new SqlConnectionStringBuilder(ObtenerCadenaConexionAppController());

            CRReporteChequesCobrados crReporte = new CRReporteChequesCobrados();
            //crReporte.SetDatabaseLogon("sa", "modomixto", "CRUZ2-THINK", "DBCajaCuentas2");
            crReporte.SetDatabaseLogon(sqlStrBuilder.UserID, sqlStrBuilder.Password, sqlStrBuilder.DataSource, sqlStrBuilder.InitialCatalog);
            crReporte.SetParameterValue("@fechaInicio", fechaInicio);
            crReporte.SetParameterValue("@fechaFin", fechaFin);

            TextObject periodoDeBusquedaTextObject = crReporte.ReportDefinition.ReportObjects["Text12"] as TextObject;
            periodoDeBusquedaTextObject.Text = "periodo " + MuestraFechaDeBusquedaSinLaHora(fechaInicio) + " a " + MuestraFechaDeBusquedaSinLaHora(fechaFin);
            crystalReportViewer1.ReportSource = crReporte;

            await Task.Delay(200);
            return (1);
        }

        private void DeshabilitarButtonsYDateTimePicker()
        {
            dateTimePicker2.Enabled = false;
            dateTimePicker3.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
        }

        private void HabilitarButtonsYDateTimePicker()
        {
            dateTimePicker2.Enabled = true;
            dateTimePicker3.Enabled = true;
            button1.Enabled = true;
            button2.Enabled = true;
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

        private async Task<int> GenerarReporteExcelAsync(DateTime fechaInicio, DateTime fechaFin, string nomArchivo)
        {
            SqlConnectionStringBuilder sqlStrBuilder = new SqlConnectionStringBuilder(ObtenerCadenaConexionAppController());

            //http://aspalliance.com/478_Exporting_to_Excel_in_Crystal_Reports_NET__Perfect_Excel_Exports.3
            //https://www.c-sharpcorner.com/UploadFile/mahesh/savefiledialog-in-C-Sharp/
            CRReporteChequesCobradosParaExportar reporte = new CRReporteChequesCobradosParaExportar();
            //reporte.SetDatabaseLogon("sa", "modomixto", "CRUZ2-THINK", "DBCajaCuentas2");
            reporte.SetDatabaseLogon(sqlStrBuilder.UserID, sqlStrBuilder.Password, sqlStrBuilder.DataSource, sqlStrBuilder.InitialCatalog);
            reporte.SetParameterValue("@fechaInicio", fechaInicio);
            reporte.SetParameterValue("@fechaFin", fechaFin);

            //Ponerle las fechas de busqueda al reporte
            TextObject periodoDeBusquedaTextObject = reporte.ReportDefinition.ReportObjects["Text19"] as TextObject;
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


        //----------------------Events
        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DeshabilitarButtonsYDateTimePicker();
                IniciarProgressBar();

                DateTime fechaInicio;
                DateTime fechaFin;

                fechaInicio = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month,
                        dateTimePicker2.Value.Day, 0, 1, 0);

                fechaFin = new DateTime(dateTimePicker3.Value.Year, dateTimePicker3.Value.Month,
                        dateTimePicker3.Value.Day, 23, 59, 58);

                DataTable tablaNatural = await Cheque_RecuperarDetallesDeChequesCobradosControllerAsync(fechaInicio, fechaFin);

                if (tablaNatural.Rows.Count == 0)
                {
                    crystalReportViewer1.ReportSource = null;
                    DetenerProgressBar();
                    HabilitarButtonsYDateTimePicker();
                    MessageBox.Show("No se encontraron cheques con fecha de cobro en el rango de fechas solicitado", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                else
                {
                    await CargarCRViewerAsync(fechaInicio, fechaFin);
                    DetenerProgressBar();
                    HabilitarButtonsYDateTimePicker();
                }
            }

            catch (Exception ex)
            {
                DetenerProgressBar();
                HabilitarButtonsYDateTimePicker();
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }


        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fechaInicio;
                DateTime fechaFin;


                fechaInicio = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month,
                    dateTimePicker2.Value.Day, 0, 1, 0);

                fechaFin = new DateTime(dateTimePicker3.Value.Year, dateTimePicker3.Value.Month,
                    dateTimePicker3.Value.Day, 23, 59, 58);

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
