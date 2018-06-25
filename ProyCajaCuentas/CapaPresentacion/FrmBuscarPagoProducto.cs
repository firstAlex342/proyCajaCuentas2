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
    public partial class FrmBuscarPagoProducto : Form
    {
        public FrmBuscarPagoProducto()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;

            CargarComboBoxOpcionesFiltro();
        }

        //------------Utils
       private void CargarComboBoxOpcionesFiltro()
        {
            comboBox1.Items.Add("Mes de alta");
        }

        private void MostrarResultadoDeBusqueda(DataTable tabla)
        {
            dataGridView1.DataSource = tabla;
        }

        //----------------Methods
        private DataTable PagoProducto_BuscarXMesAltaController(int mesBuscado)
        {
            DateTime d = new DateTime(2018, mesBuscado, 15);
            ClsPagoProducto clsPagoProducto = new ClsPagoProducto();

            clsPagoProducto.FechaAlta = d;

            return (clsPagoProducto.PagoProducto_BuscarXMesAlta()  );
        }


        //--------------Events

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if(comboBox1.SelectedIndex == 0)
                {
                    DataTable respuesta = PagoProducto_BuscarXMesAltaController(Int32.Parse(textBox1.Text));
                    MostrarResultadoDeBusqueda(respuesta);
                }
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source);
            }

        }
    }
}
