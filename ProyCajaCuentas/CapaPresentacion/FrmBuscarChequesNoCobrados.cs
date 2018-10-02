using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class FrmBuscarChequesNoCobrados : Form
    {
        public FrmBuscarChequesNoCobrados()
        {
            InitializeComponent();
        }

        private void FrmBuscarChequesCobrados_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //https://www.codeproject.com/Articles/10173/Loading-Crystal-Report-reports-which-use-Stored-Pr
            CRReporteChequesNoCobrados crReporte = new CRReporteChequesNoCobrados();
            crReporte.SetDatabaseLogon("sa", "modomixto", "CRUZ2-THINK", "DBCajaCuentas2");
            crReporte.SetParameterValue("@fechaInicio", DateTime.Now);
            crReporte.SetParameterValue("@fechaFin", DateTime.Now);

            crystalReportViewer1.ReportSource = crReporte;
        }
    }
}
