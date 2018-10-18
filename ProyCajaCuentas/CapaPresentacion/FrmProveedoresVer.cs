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
    public partial class FrmProveedoresVer : Form
    {
        //----------------constructor
        public FrmProveedoresVer()
        {
            InitializeComponent();

            try
            {
                DataTable datosDeProveedorTable = Proveedor_SelectTodosActivosController();
                LLenarDataGridConDatos(datosDeProveedorTable);
                label2.Text = label2.Text + " " + datosDeProveedorTable.Rows.Count.ToString();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }

        //-------------------Method controller
        private DataTable Proveedor_SelectTodosActivosController()
        {
            ClsProveedor clsProveedor = new ClsProveedor();
            return (clsProveedor.Proveedor_SelectTodosActivos());
        }


        //-------------------Utils
        private void LLenarDataGridConDatos(DataTable tabla)
        {
            dataGridView1.DataSource = tabla;
            dataGridView1.Columns[0].Visible = false;

            dataGridView1.Columns[2].HeaderText = "Supermanzana";
            dataGridView1.Columns[3].HeaderText = "Manzana";
            dataGridView1.Columns[4].HeaderText = "Lote";
            dataGridView1.Columns[5].HeaderText = "Calle";
            dataGridView1.Columns[6].HeaderText = "Complemento";
            dataGridView1.Columns[7].HeaderText = "Teléfono";
            dataGridView1.Columns[8].HeaderText = "Celular";
            dataGridView1.Columns[9].HeaderText = "Correo eléctronico";
        }
    }
}
