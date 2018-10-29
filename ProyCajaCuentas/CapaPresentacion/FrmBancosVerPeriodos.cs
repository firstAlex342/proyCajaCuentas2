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
    public partial class FrmBancosVerPeriodos : Form
    {
        //----------------constructor
        public FrmBancosVerPeriodos()
        {
            InitializeComponent();
             
            try
            {
                DataTable res = Bancos_SelectActivosController();
                dataGridView1.DataSource = null;
                MostrarEnGrid(res);
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Source + " " + ex.Source + " " + ex.StackTrace);
            }
        }

        //-----------------Methods
        public DataTable Bancos_SelectActivosController()
        {
            ClsBancos clsBancos = new ClsBancos();
            return (clsBancos.Bancos_SelectActivos());
        }

        //--------------Utils
        private void MostrarEnGrid(DataTable origen)
        {
            dataGridView1.DataSource = origen;

            dataGridView1.Columns[1].HeaderText = "Mes";
            dataGridView1.Columns[2].HeaderText = "Año";
            dataGridView1.Columns[3].HeaderText = "Disponible en bancos ";
            dataGridView1.Columns[4].HeaderText = "Disponible en bancos (real)";

            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[6].Visible = false;
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[8].Visible = false;
            dataGridView1.Columns[9].Visible = false;
            dataGridView1.Columns[10].Visible = false;
        }


    }
}
