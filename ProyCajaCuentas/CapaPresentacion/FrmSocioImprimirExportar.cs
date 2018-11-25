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
    }
}
