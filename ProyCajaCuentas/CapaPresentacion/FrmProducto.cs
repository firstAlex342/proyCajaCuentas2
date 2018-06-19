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
    public partial class FrmProducto : Form
    {

        //----------------Constructor
        public FrmProducto()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;

            DataTable respuesta = Producto_Select_Id_Nombre_DeTodosController();
            MostrarProducto_Id_Nombre_DeTodos(respuesta);
        }

        //----------------Methods
        private DataTable Producto_Select_Id_Nombre_DeTodosController()
        {
            ClsProducto clsProducto = new ClsProducto();
            return (clsProducto.Producto_Select_Id_Nombre_DeTodos());
        }

        private string InsertarProductoController(string nombre, string descripcion, int idUsuarioOperador)
        {

            ClsProducto clsProducto = new ClsProducto();
            clsProducto.Nombre = nombre;
            clsProducto.Descripcion = descripcion;
            clsProducto.IdUsuarioAlta = idUsuarioOperador;

            return (clsProducto.Producto_create());
        }

        private DataTable Producto_BuscarXIdController(int idProductoBuscado)
        {
            ClsProducto clsProducto = new ClsProducto();
            clsProducto.Id = idProductoBuscado;
            return (clsProducto.Producto_BuscarXId());
        }

        private string Producto_updateController(int idProductoBuscado, string newNombre, string newDescripcion, int idusuarioOperador)
        {
            ClsProducto clsProducto = new ClsProducto();
            clsProducto.Id = idProductoBuscado;
            clsProducto.Nombre = newNombre;
            clsProducto.Descripcion = newDescripcion;
            clsProducto.IdUsuarioModifico = idusuarioOperador;

            return (clsProducto.Producto_update());
        }


        //------------------Utils
        private void MostrarProducto_Id_Nombre_DeTodos(DataTable tabla)
        {
            dataGridView1.DataSource = tabla;
        }

        private void MostrarId_Nombre_Descrip_DeProducto(DataTable tabla)
        {
            DataRow fila = tabla.Rows[0];
            textBox1.Text = fila["Id"].ToString();
            textBox2.Text = fila["Nombre"].ToString();
            textBox3.Text = fila["Descripcion"].ToString();
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void LimpiarTodosTextBoxs()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
        }


        //-------------------Events
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult res = MessageBox.Show("¿Estas usted seguro que desea continuar?", "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(res == DialogResult.Yes)
                {
                    if(textBox1.Text == String.Empty)
                    {
                        //Se inserta un nuevo producto
                        int idUsuarioOperador = 99;

                        InsertarProductoController(textBox2.Text, textBox3.Text, idUsuarioOperador);
                        LimpiarTodosTextBoxs();
                        DataTable respuesta = Producto_Select_Id_Nombre_DeTodosController();
                        MostrarProducto_Id_Nombre_DeTodos(respuesta);
                    }
                    else
                    {
                        //Se actualiza un producto
                        int idUsuarioOperador = 99;

                        int idProductoAActualizar = Int32.Parse(textBox1.Text);
                        string mensaje = Producto_updateController(idProductoAActualizar, textBox2.Text, textBox3.Text, idUsuarioOperador);

                        LimpiarTodosTextBoxs();
                        DataTable respuesta = Producto_Select_Id_Nombre_DeTodosController();
                        MostrarProducto_Id_Nombre_DeTodos(respuesta);
                    }

                }
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {

                    string idEnTexto = (dataGridView1.Rows[e.RowIndex].Cells[0].EditedFormattedValue).ToString();
                    DataTable respuesta = Producto_BuscarXIdController(Int32.Parse(idEnTexto));
                    MostrarId_Nombre_Descrip_DeProducto(respuesta);
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " " + ex.Source);
                }               
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarTodosTextBoxs();
                DataTable respuesta = Producto_Select_Id_Nombre_DeTodosController();
                MostrarProducto_Id_Nombre_DeTodos(respuesta);
            }



            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source);
            }
        }
    }
}
