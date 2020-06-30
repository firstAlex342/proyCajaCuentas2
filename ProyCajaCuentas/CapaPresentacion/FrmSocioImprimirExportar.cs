using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaLogicaNegocios;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data.SqlClient;

namespace CapaPresentacion
{
    public partial class FrmSocioImprimirExportar : Form
    {
        //----------------constructor
        public FrmSocioImprimirExportar()
        {
            InitializeComponent();
        }

        //---------------controllers
        public async Task<DataTable> Socio_BuscarTodosActivosControllerAsync()
        {
            ClsSocio clsSocio = new ClsSocio();
            DataTable respuesta = clsSocio.Socio_BuscarTodosActivos();

            await Task.Delay(10);
            return (respuesta);
        }

        private string ObtenerCadenaConexionAppController()
        {
            ClsEnlaceToAppConfig clsEnlaceToAppConfig = new ClsEnlaceToAppConfig();
            return (clsEnlaceToAppConfig.ObtenerCadenaConexionAppConfig());
        }


        //-----------------methods
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

        private void DeshabilitarButtons()
        {
            button1.Enabled = false;
            button2.Enabled = false;
        }

        private void HabilitarButtons()
        {
            button1.Enabled = true;
            button2.Enabled = true;
        }

        //-------------Events
        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DeshabilitarButtons();
                IniciarProgressBar();

                DataTable sociosTabla = await Socio_BuscarTodosActivosControllerAsync();
                if(sociosTabla.Rows.Count == 0)
                {
                    crystalReportViewer1.ReportSource = null;
                    DetenerProgressBar();
                    HabilitarButtons();
                    MessageBox.Show("Se encontraron cero socios", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                else
                {
                    SqlConnectionStringBuilder sqlStrBuilder = new SqlConnectionStringBuilder(ObtenerCadenaConexionAppController());

                    CRReporteSocios crReporte = new CRReporteSocios();
                    //crReporte.SetDatabaseLogon("sa", "modomixto", "CRUZ2-THINK", "DBCajaCuentas2");
                    crReporte.SetDatabaseLogon(sqlStrBuilder.UserID, sqlStrBuilder.Password, sqlStrBuilder.DataSource, sqlStrBuilder.InitialCatalog);
                    crReporte.SetParameterValue("@parametroNoNecesario", true);
                    crystalReportViewer1.ReportSource = crReporte;

                    DetenerProgressBar();
                    HabilitarButtons();
                }
            }

            catch(Exception ex)
            {
                DetenerProgressBar();
                HabilitarButtons();
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    DeshabilitarButtons();
                    IniciarProgressBar();

                    string nomArchivo = saveFileDialog1.FileName;
                    SqlConnectionStringBuilder sqlStrBuilder = new SqlConnectionStringBuilder(ObtenerCadenaConexionAppController());

                    //http://aspalliance.com/478_Exporting_to_Excel_in_Crystal_Reports_NET__Perfect_Excel_Exports.3
                    //https://www.c-sharpcorner.com/UploadFile/mahesh/savefiledialog-in-C-Sharp/
                    CRReporteSociosParaExportar reporte = new CRReporteSociosParaExportar();
                    //reporte.SetDatabaseLogon("sa", "modomixto", "CRUZ2-THINK", "DBCajaCuentas2");
                    reporte.SetDatabaseLogon(sqlStrBuilder.UserID, sqlStrBuilder.Password, sqlStrBuilder.DataSource, sqlStrBuilder.InitialCatalog);
                    reporte.SetParameterValue("@parametroNoNecesario", true);


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
                    HabilitarButtons();
                    MessageBox.Show("Exportacion lista", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }

            catch(Exception ex)
            {
                DetenerProgressBar();
                HabilitarButtons();
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }


    }
}
