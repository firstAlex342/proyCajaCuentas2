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
    public partial class FrmBuscarFoliosActivosYCanceladosDeSocioImprimirExportar : Form
    {
        //------------------Constructor
        public FrmBuscarFoliosActivosYCanceladosDeSocioImprimirExportar()
        {
            InitializeComponent();
        }

        //-------------------Controllers
        private async Task<DataTable> Socio_BuscarFoliosActivos_Y_CanceladosDeSocioEnReciboListaProductosDetalladoAsync(DateTime fechaInicio, DateTime fechaFin, string numLicenciaBuscada)
        {
            ClsSocio clsSocio = new ClsSocio();
            clsSocio.FechaAlta = fechaInicio;
            clsSocio.FechaModificacion = fechaFin;
            clsSocio.NumeroLicencia = numLicenciaBuscada;

            DataTable respuesta = clsSocio.Socio_BuscarFoliosActivos_Y_CanceladosDeSocioEnReciboListaProductosDetallado();
            await Task.Delay(10);
            return (respuesta);
        }

        private string ObtenerCadenaConexionAppController()
        {
            ClsEnlaceToAppConfig clsEnlaceToAppConfig = new ClsEnlaceToAppConfig();
            return (clsEnlaceToAppConfig.ObtenerCadenaConexionAppConfig());
        }

        //-------------------Utils
        private string MuestraFechaDeBusquedaSinLaHora(DateTime fecha)
        {
            return (fecha.ToShortDateString());
        }

        private void MuestraNumeroLicenciaEnReporte(TextObject textObjectDestino, string numLicencia)
        {
            textObjectDestino.Text = numLicencia;
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

        private void DeshabilitarButtonsYDateTimePicker()
        {
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            dateTimePicker1.Enabled = false;
            dateTimePicker2.Enabled = false;
            dateTimePicker3.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
            textBox1.Enabled = false;
        }

        private void HabilitarButtonsYDateTimePicker()
        {
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;

            if(radioButton1.Checked == true)
            {
                dateTimePicker1.Enabled = true;
                dateTimePicker2.Enabled = false;
                dateTimePicker3.Enabled = false;
            }

            else if(radioButton2.Checked == true)
            {
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = true;
                dateTimePicker3.Enabled = true;
            }

            button1.Enabled = true;
            button2.Enabled = true;
            textBox1.Enabled = true;
        }


        //------------------Events
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

        private async void button1_Click(object sender, EventArgs e)
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

                DataTable res = await Socio_BuscarFoliosActivos_Y_CanceladosDeSocioEnReciboListaProductosDetalladoAsync(fechaInicio, fechaFin, textBox1.Text);
                if(res.Rows.Count == 0)
                {
                    crystalReportViewer1.ReportSource = null;
                    DetenerProgressBar();
                    HabilitarButtonsYDateTimePicker();
                    MessageBox.Show("Se encontraron cero capturas en el rango de fechas solicitado", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                else
                {
                    SqlConnectionStringBuilder sqlStrBuilder = new SqlConnectionStringBuilder(ObtenerCadenaConexionAppController());

                    CRReporteProductosEnReciboListaProductosDSocioActivosCancelados crReporte = new CRReporteProductosEnReciboListaProductosDSocioActivosCancelados();
                    //crReporte.SetDatabaseLogon("sa", "modomixto", "CRUZ2-THINK", "DBCajaCuentas2");
                    crReporte.SetDatabaseLogon(sqlStrBuilder.UserID, sqlStrBuilder.Password, sqlStrBuilder.DataSource, sqlStrBuilder.InitialCatalog);
                    crReporte.SetParameterValue("@fechaInicio", fechaInicio);
                    crReporte.SetParameterValue("@fechaFin", fechaFin);
                    crReporte.SetParameterValue("@numeroLicenciaBuscada", textBox1.Text);

                    TextObject periodoDeBusquedaTextObject = crReporte.ReportDefinition.ReportObjects["Text28"] as TextObject;
                    periodoDeBusquedaTextObject.Text = "periodo " + MuestraFechaDeBusquedaSinLaHora(fechaInicio) + " a " + MuestraFechaDeBusquedaSinLaHora(fechaFin);
                    TextObject numLicenciaTextObject = crReporte.ReportDefinition.ReportObjects["Text29"] as TextObject;
                    MuestraNumeroLicenciaEnReporte(numLicenciaTextObject, textBox1.Text);
                    crystalReportViewer1.ReportSource = crReporte;

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

                    SqlConnectionStringBuilder sqlStrBuilder = new SqlConnectionStringBuilder(ObtenerCadenaConexionAppController());
                    //http://aspalliance.com/478_Exporting_to_Excel_in_Crystal_Reports_NET__Perfect_Excel_Exports.3
                    //https://www.c-sharpcorner.com/UploadFile/mahesh/savefiledialog-in-C-Sharp/
                    CRReporteProductosEnReciboListaProductosDSocioActivosCanceladosParaExportar reporte = new CRReporteProductosEnReciboListaProductosDSocioActivosCanceladosParaExportar();
                    //reporte.SetDatabaseLogon("sa", "modomixto", "CRUZ2-THINK", "DBCajaCuentas2");
                    reporte.SetDatabaseLogon(sqlStrBuilder.UserID, sqlStrBuilder.Password, sqlStrBuilder.DataSource, sqlStrBuilder.InitialCatalog);
                    reporte.SetParameterValue("@fechaInicio", fechaInicio);
                    reporte.SetParameterValue("@fechaFin", fechaFin);
                    reporte.SetParameterValue("@numeroLicenciaBuscada", textBox1.Text);

                    //Ponerle las fechas de busqueda al reporte
                    TextObject periodoDeBusquedaTextObject = reporte.ReportDefinition.ReportObjects["Text29"] as TextObject;
                    periodoDeBusquedaTextObject.Text = MuestraFechaDeBusquedaSinLaHora(fechaInicio) + " a " + MuestraFechaDeBusquedaSinLaHora(fechaFin);

                    //Poner el numero de licencia al reporte
                    TextObject numLicTextObject = reporte.ReportDefinition.ReportObjects["Text30"] as TextObject;
                    numLicTextObject.Text = "Número de licencia " + textBox1.Text;

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

                    await Task.Delay(10);
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
