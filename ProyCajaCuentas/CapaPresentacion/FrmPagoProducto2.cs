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
    public partial class FrmPagoProducto2 : Form
    {
        public FrmPagoProducto2()
        {
            InitializeComponent();
        }

        private void btn_Click_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Maximized;
            //btnMaximizar.Visible = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
