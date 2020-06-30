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
    public partial class FrmControlMenu : Form
    {
        public FrmControlMenu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                FrmInicioSesion fr1 = new FrmInicioSesion();
                fr1.Show();
            }

            if (comboBox1.SelectedIndex == 1)
            {
                FrmIniSes2 fr1 = new FrmIniSes2();
                fr1.Show();
            }
        }
    }
}
