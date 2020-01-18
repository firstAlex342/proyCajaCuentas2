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
            dateTimePicker1.Enabled = false;
            dateTimePicker2.Enabled = false;
            dateTimePicker3.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
        }

        private void HabilitarButtonsYDateTimePicker()
        {
            dateTimePicker1.Enabled = true;
            dateTimePicker2.Enabled = true;
            dateTimePicker3.Enabled = true;
            button1.Enabled = true;
            button2.Enabled = true;
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
                    CRReporteProductosEnReciboListaProductosDSocioActivosCancelados crReporte = new CRReporteProductosEnReciboListaProductosDSocioActivosCancelados();
                    crReporte.SetDatabaseLogon("sa", "modomixto", "CRUZ2-THINK", "DBCajaCuentas2");
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
    }
}
