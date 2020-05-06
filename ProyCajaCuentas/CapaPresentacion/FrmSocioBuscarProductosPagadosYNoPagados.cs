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
    public partial class FrmSocioBuscarProductosPagadosYNoPagados : Form
    {

        //---------------------Constructor
        public FrmSocioBuscarProductosPagadosYNoPagados()
        {
            InitializeComponent();
        }

        //--------------------Methods
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


                CRReporteSociosProductosPagadosYNoPagados crReporte = new CRReporteSociosProductosPagadosYNoPagados();
                crReporte.SetDatabaseLogon("sa", "modomixto", "CRUZ2-THINK", "DBCajaCuentas2");
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
    }
}
