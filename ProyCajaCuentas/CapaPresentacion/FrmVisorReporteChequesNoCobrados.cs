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
    public partial class FrmVisorReporteChequesNoCobrados : Form
    {
        //-----------------------Constructor
        public FrmVisorReporteChequesNoCobrados()
        {
            InitializeComponent();
        }

        //------------------------Utils
        private string MuestraFechaDeBusquedaSinLaHora(DateTime fecha)
        {
            return (fecha.ToShortDateString());
        }

        //------------------Methods controller
        private DataTable Cheque_RecuperarDetallesDeChequesNoCobradosController(DateTime fechaInicio, DateTime fechaFin)
        {
            ClsCheque clsCheque = new ClsCheque();
            clsCheque.FechaAlta = fechaInicio;
            clsCheque.FechaModificacion = fechaFin;

            return (clsCheque.Cheque_RecuperarDetallesDeChequesNoCobrados());
        }

        //---------------------------Events
        private void FrmBuscarChequesCobrados_Load(object sender, EventArgs e)
        {

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

                DataTable tablaNatural = Cheque_RecuperarDetallesDeChequesNoCobradosController(fechaInicio, fechaFin);
                if(tablaNatural.Rows.Count == 0)
                {
                    crystalReportViewer1.ReportSource = null;
                    MessageBox.Show("Se encontraron 0 cheques sin fecha de cobro, en el rango de fechas solicitado", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                else
                {
                    //https://www.codeproject.com/Articles/10173/Loading-Crystal-Report-reports-which-use-Stored-Pr
                    CRReporteChequesNoCobrados crReporte = new CRReporteChequesNoCobrados();
                    crReporte.SetDatabaseLogon("sa", "modomixto", "CRUZ2-THINK", "DBCajaCuentas2");
                    crReporte.SetParameterValue("@fechaInicio", fechaInicio);
                    crReporte.SetParameterValue("@fechaFin", fechaFin);

                    TextObject periodoDeBusquedaTextObject = crReporte.ReportDefinition.ReportObjects["Text12"] as TextObject;
                    periodoDeBusquedaTextObject.Text = "periodo " + MuestraFechaDeBusquedaSinLaHora(fechaInicio) + " a " + MuestraFechaDeBusquedaSinLaHora(fechaFin);
                    crystalReportViewer1.ReportSource = crReporte;
                }                
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }


    }
}
