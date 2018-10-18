using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaLogicaNegocios;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class FrmSumarProductosPorProducto : Form
    {
        //-------------------constructor
        public FrmSumarProductosPorProducto()
        {
            InitializeComponent();
        }

        //---------------------methods 
        public DataTable MovsEnCaja_SumarPagoDeProductosXProductoController(DateTime fechaInicio, DateTime fechaFin)
        {
            ClsMovsEnCaja clsMovsEnCaja = new ClsMovsEnCaja();
            clsMovsEnCaja.FechaAlta = fechaInicio;
            clsMovsEnCaja.FechaModificacion = fechaFin;
            return (clsMovsEnCaja.MovsEnCaja_SumarPagoDeProductosXProducto());
        }

        //----------------------utils
        public void MostrarEnGridSumaDeProductos(DataTable tabla)
        {
            dataGridView1.DataSource = tabla;
        }


        //-----------------------Events

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
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

                dataGridView1.DataSource = null;
                DataTable sumaDeProductosTabla = MovsEnCaja_SumarPagoDeProductosXProductoController(fechaInicio, fechaFin);

                MostrarEnGridSumaDeProductos(sumaDeProductosTabla);
            }

            catch (Exception ex)
            {
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
