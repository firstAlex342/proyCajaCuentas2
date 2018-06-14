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
    public partial class FrmProductosAPagar : Form
    {
        //------------constructor
        public FrmProductosAPagar()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            CrearColumnasParaDataGridViewCapturoCantidades();
        }

        //-------------Methods
        private DataTable Producto_Select_Id_Nombre_DeTodosController()
        {
            ClsProducto clsProducto = new ClsProducto();
            return (clsProducto.Producto_Select_Id_Nombre_DeTodos());
        }

        private string ProductosAPagar_createController(int idCuentaPorPagar, int idProducto, decimal limite, int idUsuarioOperador)
        {
            ClsProductosAPagar clsProductoAPagar = new ClsProductosAPagar();
            clsProductoAPagar.IdCuenta = idCuentaPorPagar;
            clsProductoAPagar.IdProducto = idProducto;
            clsProductoAPagar.Limite = limite;
            clsProductoAPagar.IdUsuarioAlta = idUsuarioOperador;

            return(clsProductoAPagar.ProductosAPagar_create());
        }


        private DataTable ProductosAPagar_BuscarDetallesProductoXIdCuentaController(int idCuentaBuscada)
        {
            ClsProductosAPagar clsProductosAPagar = new ClsProductosAPagar();
            clsProductosAPagar.IdCuenta = idCuentaBuscada;

            return (clsProductosAPagar.ProductosAPagar_BuscarDetallesProductoXIdCuenta());
        }


        private bool ExisteCuentaPorPagarController(int idCuentaBuscada)
        {
            ClsCuentaPorPagar clsCuentaPorPagar = new ClsCuentaPorPagar();
            clsCuentaPorPagar.Id = idCuentaBuscada;

            DataTable miTabla = clsCuentaPorPagar.CuentaPorPagar_BuscarXId();
            bool respuesta = miTabla.Rows.Count > 0 ? true : false;
            return (respuesta);
        }


        //------------------Utils
        private void MostrarId_Nombre_ProductosEnComboBox(DataTable tabla)
        {
            //var res = from s in tabla.AsEnumerable()
            //          select s.Field<int>(0).ToString() + "-" + s.Field<string>(1);

            //foreach (var item in res)
            //    comboBox1.Items.Add(item.ToString());
            var res = from s in tabla.AsEnumerable()
                      select new { Uno = s.Field<int>(0), Dos = s.Field<string>(1) };

            foreach (var item in res)
                comboBox1.Items.Add(item.Uno + "-" + item.Dos);
        }


        private void CrearColumnasParaDataGridViewCapturoCantidades()
        {
            //Es el dataGridView donde capturo cantidades
            dataGridView2.Columns.Add("Id_Producto", "Id_Producto");
            dataGridView2.Columns.Add("Nombre_Producto", "Nombre_Producto");
            dataGridView2.Columns.Add("Limite", "Limite");

            dataGridView2.Columns[0].ReadOnly = true;
            dataGridView2.Columns[1].ReadOnly = true;
            dataGridView2.Columns[2].ReadOnly = false;
        }
        

        private void AddFilaADataGridView_ProductosAPagar(string idServicioElegidoYSuNombre)
        {
            string[] cadenaDividida = idServicioElegidoYSuNombre.Split('-');
            int n = dataGridView2.Rows.Add();
            dataGridView2.Rows[n].Cells[0].Value = cadenaDividida[0];
            dataGridView2.Rows[n].Cells[1].Value = cadenaDividida[1];
        }

        private void BorrarTxtBoxesCaptura()
        {
            textBox1.Clear();
            textBox2.Clear();
        }

        private void MostrarProductosEnCuentaEnDataGrid(DataTable miTabla)
        {
            dataGridView1.DataSource = miTabla;
        }


        //-----------------Eventos
        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {
        }

        private void FrmCuentaPorPagar_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable respuesta = Producto_Select_Id_Nombre_DeTodosController();
                MostrarId_Nombre_ProductosEnComboBox(respuesta);
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                AddFilaADataGridView_ProductosAPagar(comboBox1.SelectedItem.ToString());
            }

            else
            {
                //No selecciono algo
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult res = MessageBox.Show("¿Estas usted seguro que desea continuar?", "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(res == DialogResult.Yes)
                {

                    DataGridViewRowCollection coleccion = dataGridView2.Rows;
                    foreach (DataGridViewRow row in coleccion)
                    {
                        int idCuentaPorPagar = Int32.Parse(textBox1.Text);
                        int idProducto = Int32.Parse(row.Cells[0].EditedFormattedValue.ToString());
                        decimal limite = Decimal.Parse(row.Cells[2].EditedFormattedValue.ToString());
                        int usuarioOperador = 2;

                        ProductosAPagar_createController(idCuentaPorPagar, idProducto, limite, usuarioOperador);
                    }


                    BorrarTxtBoxesCaptura();
                    dataGridView2.DataSource = null;
                    dataGridView2.Rows.Clear();
                    dataGridView1.DataSource = null;

                    textBox1.Enabled = true;
                    button1.Enabled = true;
                }
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if ( ExisteCuentaPorPagarController(Int32.Parse(textBox1.Text) )   )
                {
                    textBox2.Clear();
                    dataGridView1.DataSource = null;
                    dataGridView2.DataSource = null;
                    dataGridView2.Rows.Clear();

                    textBox1.Enabled = false;
                    button1.Enabled = false;

                    int idCuentaBuscada = Int32.Parse(textBox1.Text);
                    DataTable respuesta = ProductosAPagar_BuscarDetallesProductoXIdCuentaController(idCuentaBuscada);
                    MostrarProductosEnCuentaEnDataGrid(respuesta);
                }

                else
                {
                    MessageBox.Show("No se encontro la cuenta con Id " + textBox1.Text);
                    BorrarTxtBoxesCaptura();
                    dataGridView1.DataSource = null;
                    dataGridView2.DataSource = null;
                    dataGridView2.Rows.Clear();
                    textBox1.Enabled = true;
                    button1.Enabled = true;
                }
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox2_MouseClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show("Hiciste click en el textBox");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BorrarTxtBoxesCaptura();
            dataGridView1.DataSource = null;
            dataGridView2.DataSource = null;
            dataGridView2.Rows.Clear();
            textBox1.Enabled = true;
            button1.Enabled = true;
        }
    }
}
