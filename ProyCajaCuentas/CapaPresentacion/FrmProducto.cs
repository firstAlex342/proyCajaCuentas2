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

            CrearDataGridView_ParaCapturaTarifas();

            DataTable respuesta = Producto_Select_Id_Nombre_DeTodosController();
            MostrarProducto_Id_Nombre_DeTodos(respuesta);
        }

        //----------------Methods
        private DataTable Producto_Select_Id_Nombre_DeTodosController()
        {
            ClsProducto clsProducto = new ClsProducto();
            return (clsProducto.Producto_Select_Id_Nombre_DeTodos());
        }

        private int InsertarProductoController(string nombre, string descripcion, int idUsuarioOperador)
        {

            ClsProducto clsProducto = new ClsProducto();
            clsProducto.Nombre = nombre;
            clsProducto.Descripcion = descripcion;
            clsProducto.IdUsuarioAlta = idUsuarioOperador;

            DataTable respuesta = clsProducto.Producto_create();
            DataRow fila = respuesta.Rows[0];
            int idQueSeCreo = Int32.Parse(fila[0].ToString());
            return (idQueSeCreo);
        }

        private int Tarifa_createController(decimal cantidad, int idUsuarioOperador)
        {
            ClsTarifa clsTarifa = new ClsTarifa();
            clsTarifa.Cantidad = cantidad;
            clsTarifa.IdUsuarioAlta = idUsuarioOperador;

            DataTable respuesta = clsTarifa.Tarifa_create();
            DataRow row = respuesta.Rows[0];
            int idQueSeCreo = Int32.Parse(row[0].ToString());
            return (idQueSeCreo);
        }

        private string ProductoPosee_createController(int idProducto, int idTarifa, int idUsuarioOperador)
        {
            ClsProductoPosee clsProductoPosee = new ClsProductoPosee();
            clsProductoPosee.IdProducto = idProducto;
            clsProductoPosee.IdTarifa = idTarifa;
            clsProductoPosee.IdUsuarioAlta = idUsuarioOperador;

            return (clsProductoPosee.ProductoPosee_create());
        }

        private DataTable Producto_BuscarXIdController(int idProductoBuscado)
        {
            ClsProducto clsProducto = new ClsProducto();
            clsProducto.Id = idProductoBuscado;
            return (clsProducto.Producto_BuscarXId());
        }


        private DataTable ProductoPosee_innerjoin_TarifasController(int idProductoBuscado)
        {
            //Busca las diferentes tarifas de un producto
            ClsProductoPosee clsProductoPosee = new ClsProductoPosee();
            clsProductoPosee.IdProducto = idProductoBuscado;
            return (clsProductoPosee.ProductoPosee_innerjoin_Tarifas());
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

        private string Tarifa_updateController(int idTarifaBuscada, decimal newCantidad, int idUsuarioOperador)
        {
            ClsTarifa clsTarifa = new ClsTarifa();
            clsTarifa.Id = idTarifaBuscada;
            clsTarifa.Cantidad = newCantidad;
            clsTarifa.IdUsuarioModifico = idUsuarioOperador;

            return (clsTarifa.Tarifa_update());
        }


        //------------------Utils
        private void CrearDataGridView_ParaCapturaTarifas()
        {
            //Es el dataGridView donde capturo cantidades
            dataGridView2.Columns.Add("Id_Tarifa", "Id_Tarifa");
            dataGridView2.Columns.Add("Cantidad", "Cantidad");

            dataGridView2.Columns[0].Visible = false;
            dataGridView2.Columns[0].ReadOnly = true;          
            dataGridView2.Columns[1].ReadOnly = false;
        }


        private void MostrarProducto_Id_Nombre_DeTodos(DataTable tabla)
        {
            dataGridView1.DataSource = tabla;
            dataGridView1.Columns[0].Visible = false;
        }

        private void MostrarId_Nombre_Descrip_DeProducto(DataTable tabla)
        {
            DataRow fila = tabla.Rows[0];
            textBox1.Text = fila["Id"].ToString();
            textBox2.Text = fila["Nombre"].ToString();
            textBox3.Text = fila["Descripcion"].ToString();
        }

        //private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        //{

        //}

        private void MostrarTarifasDeProducto(DataTable tabla)
        {
            foreach(DataRow row in tabla.Rows)
            {
                int n = dataGridView2.Rows.Add();
                dataGridView2.Rows[n].Cells[0].Value = row["Id"].ToString();  //Se muestra en el grid el id de la tarifa
                dataGridView2.Rows[n].Cells[1].Value = row["Cantidad"].ToString(); //Se muestra en el grid el monto de la tarifa
            }
        }

        private void LimpiarTodosTextBoxs()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
        }


        //-------------------Events
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    LimpiarTodosTextBoxs();
                    dataGridView2.DataSource = null;
                    dataGridView2.Rows.Clear();

                    string idEnTexto = (dataGridView1.Rows[e.RowIndex].Cells[0].EditedFormattedValue).ToString();
                    DataTable respuesta = Producto_BuscarXIdController(Int32.Parse(idEnTexto));
                    MostrarId_Nombre_Descrip_DeProducto(respuesta);

                    //Recuperar las tarifas que tenga del producto
                    DataTable respuesta2 = ProductoPosee_innerjoin_TarifasController(Int32.Parse(idEnTexto));
                    MostrarTarifasDeProducto(respuesta2);
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " " + ex.Source );
                }
            }
        }



        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                DialogResult res = MessageBox.Show("¿Estas usted seguro que desea continuar?", "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    if (textBox1.Text == String.Empty)
                    {
                        //Se inserta un nuevo producto
                       int idProductoCreado = InsertarProductoController(textBox2.Text, textBox3.Text, ClsLogin.Id);

                        //Se insertan las tarifas si existen                        
                        foreach(DataGridViewRow row in dataGridView2.Rows)
                        {
                            if(row.IsNewRow == false)
                            {
                                //Se inserta una Tarifa
                                decimal cantidad = Decimal.Parse(row.Cells[1].EditedFormattedValue.ToString());
                                int idTarifaCreada = Tarifa_createController(cantidad, ClsLogin.Id);

                                //Se inserta en la tabla ProductoPosee la relacion producto-tarifas
                                ProductoPosee_createController(idProductoCreado, idTarifaCreada, ClsLogin.Id);
                            }
                        }

                        
                        LimpiarTodosTextBoxs();
                        DataTable respuesta = Producto_Select_Id_Nombre_DeTodosController();
                        MostrarProducto_Id_Nombre_DeTodos(respuesta);
                        dataGridView2.DataSource = null;
                        dataGridView2.Rows.Clear();
                    }
                    else
                    {
                        //Se actualiza en la tabla Producto                        
                        int idProductoAActualizar = Int32.Parse(textBox1.Text);
                        string mensaje = Producto_updateController(idProductoAActualizar, textBox2.Text, textBox3.Text, ClsLogin.Id );

                        //Se recorre el grid y se actualizan las cantidades de las tarifas
                        foreach(DataGridViewRow row in dataGridView2.Rows)
                        {
                            if(row.IsNewRow == false)
                            {
                                if( String.IsNullOrEmpty(row.Cells[0].EditedFormattedValue.ToString())   )
                                {
                                    //Es una fila nueva que el usuario capturo en el datagrid, insetarla en la bd

                                    //Se inserta una Tarifa
                                    decimal cantidad = Decimal.Parse(row.Cells[1].EditedFormattedValue.ToString());
                                    int idTarifaCreada = Tarifa_createController(cantidad, ClsLogin.Id);
                                    //Se inserta en la tabla ProductoPosee la relacion producto-tarifas
                                    ProductoPosee_createController(idProductoAActualizar, idTarifaCreada, ClsLogin.Id);
                                }

                                else
                                {
                                    //Es una fila que ya existe en la BD, se actualiza
                                    int idTarifa = Int32.Parse(row.Cells[0].EditedFormattedValue.ToString());
                                    decimal newCantidad = Decimal.Parse(row.Cells[1].EditedFormattedValue.ToString());

                                    Tarifa_updateController(idTarifa, newCantidad, ClsLogin.Id);
                                }
                            }
                        }

                        LimpiarTodosTextBoxs();
                        DataTable respuesta = Producto_Select_Id_Nombre_DeTodosController();
                        MostrarProducto_Id_Nombre_DeTodos(respuesta);
                        dataGridView2.DataSource = null;
                        dataGridView2.Rows.Clear();
                    }

                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                LimpiarTodosTextBoxs();
                DataTable respuesta = Producto_Select_Id_Nombre_DeTodosController();
                MostrarProducto_Id_Nombre_DeTodos(respuesta);
                dataGridView2.DataSource = null;
                dataGridView2.Rows.Clear();
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source);
            }
        }
    }
}
