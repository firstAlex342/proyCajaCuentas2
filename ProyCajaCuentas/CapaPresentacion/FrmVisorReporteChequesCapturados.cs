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

namespace CapaPresentacion
{
    public partial class FrmVisorReporteChequesCapturados : Form
    {
        //----------------Constructor
        public FrmVisorReporteChequesCapturados()
        {
            InitializeComponent();
        }

        //------------------------Utils
        private string MuestraFechaDeBusquedaSinLaHora(DateTime fecha)
        {
            return (fecha.ToShortDateString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fechaInicio;
                DateTime fechaFin;


                fechaInicio = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month,
                        dateTimePicker2.Value.Day, 0, 1, 0);

                fechaFin = new DateTime(dateTimePicker3.Value.Year, dateTimePicker3.Value.Month,
                        dateTimePicker3.Value.Day, 23, 59, 58);


                CRReporteChequesCapturados crReporte = new CRReporteChequesCapturados();
                crReporte.SetDatabaseLogon("sa", "modomixto", "CRUZ2-THINK", "DBCajaCuentas2");
                crReporte.SetParameterValue("@fechaInicio", fechaInicio);
                crReporte.SetParameterValue("@fechaFin", fechaFin);

                TextObject periodoDeBusquedaTextObject = crReporte.ReportDefinition.ReportObjects["Text13"] as TextObject;
                periodoDeBusquedaTextObject.Text = "periodo " + MuestraFechaDeBusquedaSinLaHora(fechaInicio) + " a " + MuestraFechaDeBusquedaSinLaHora(fechaFin);
                crystalReportViewer1.ReportSource = crReporte;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }
    }
}
