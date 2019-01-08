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
        public DataTable Socio_BuscarTodosActivosController()
        {
            ClsSocio clsSocio = new ClsSocio();
            return (clsSocio.Socio_BuscarTodosActivos());           
        }

        //-------------Events
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable sociosTabla = Socio_BuscarTodosActivosController();
                if(sociosTabla.Rows.Count == 0)
                {
                    crystalReportViewer1.ReportSource = null;
                    MessageBox.Show("Se encontraron cero socios", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                else
                {
                    CRReporteSocios crReporte = new CRReporteSocios();
                    crReporte.SetDatabaseLogon("sa", "modomixto", "CRUZ2-THINK", "DBCajaCuentas2");
                    crReporte.SetParameterValue("@parametroNoNecesario", true);
                    crystalReportViewer1.ReportSource = crReporte;
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
                    string nomArchivo = saveFileDialog1.FileName;

                    //http://aspalliance.com/478_Exporting_to_Excel_in_Crystal_Reports_NET__Perfect_Excel_Exports.3
                    //https://www.c-sharpcorner.com/UploadFile/mahesh/savefiledialog-in-C-Sharp/
                    CRReporteSociosParaExportar reporte = new CRReporteSociosParaExportar();
                    reporte.SetDatabaseLogon("sa", "modomixto", "CRUZ2-THINK", "DBCajaCuentas2");
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

                    MessageBox.Show("Exportacion lista", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }
    }
}
