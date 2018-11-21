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
using CapaLogicaNegocios;

namespace CapaPresentacion
{
    public partial class FrmBuscarFoliosDeTodosImprimirExportar : Form
    {
        //-------------------constructor
        public FrmBuscarFoliosDeTodosImprimirExportar()
        {
            InitializeComponent();
        }

        //----------------Methods controller
        private DataTable Socio_BuscarFoliosActivosDeTodosEnReciboListaProductosController(DateTime fechaInicio, DateTime fechaFin)
        {
            ClsSocio clsSocio = new ClsSocio();
            clsSocio.FechaAlta = fechaInicio;
            clsSocio.FechaModificacion = fechaFin;

            return (clsSocio.Socio_BuscarFoliosActivosDeTodosEnReciboListaProductos());
        }

        //------------------------Utils
        private string MuestraFechaDeBusquedaSinLaHora(DateTime fecha)
        {
            return (fecha.ToShortDateString());
        }


        //-----------------Events
        private void button1_Click(object sender, EventArgs e)
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

                DataTable res = Socio_BuscarFoliosActivosDeTodosEnReciboListaProductosController(fechaInicio, fechaFin);
                if(res.Rows.Count == 0)
                {
                    crystalReportViewer1.ReportSource = null;
                    MessageBox.Show("Se encontraron cero capturas en el rango de fechas solicitado", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                else
                {
                    CRReporteProductosEnReciboListaProductos crReporte = new CRReporteProductosEnReciboListaProductos();
                    crReporte.SetDatabaseLogon("sa", "modomixto", "CRUZ2-THINK", "DBCajaCuentas2");
                    crReporte.SetParameterValue("@fechaInicio", fechaInicio);
                    crReporte.SetParameterValue("@fechaFin", fechaFin);

                    TextObject periodoDeBusquedaTextObject = crReporte.ReportDefinition.ReportObjects["Text25"] as TextObject;
                    periodoDeBusquedaTextObject.Text = "periodo " + MuestraFechaDeBusquedaSinLaHora(fechaInicio) + " a " + MuestraFechaDeBusquedaSinLaHora(fechaFin);
                    crystalReportViewer1.ReportSource = crReporte;
                }

            }

            catch (Exception ex)
            {
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
    }
}
