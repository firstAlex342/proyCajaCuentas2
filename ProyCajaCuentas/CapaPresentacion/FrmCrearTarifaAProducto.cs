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
    public partial class FrmCrearTarifaAProducto : Form
    {
        //------------------Constructor
        public FrmCrearTarifaAProducto()
        {
            InitializeComponent();

            try
            {
                DataTable res = Producto_Select_Id_Nombre_DeTodosController();
                MostrarProductosEnGrid(res);
                CrearColumnasEnGridParaSeleccionarProducto();
                AniadirNombresProductoAGrid(res);
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                ClsMyException clsMyException = new ClsMyException();
                string res = clsMyException.FormarTextoDeSqlException(ex);

                MessageBox.Show(res, "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }


        //--------------------Methods controllers
        private DataTable Producto_Select_Id_Nombre_DeTodosController()
        {
            ClsProducto clsProducto = new ClsProducto();
            return (clsProducto.Producto_Select_Id_Nombre_DeTodos());
        }

        public DataTable ProductoPosee_innerjoin_TarifasController(int idProductoBuscado)
        {
            ClsProductoPosee clsProductoPosee = new ClsProductoPosee();
            clsProductoPosee.IdProducto = idProductoBuscado;
            return (clsProductoPosee.ProductoPosee_innerjoin_Tarifas());      
        }


        public DataTable Tarifa_createController(decimal tarifa, int idUsuarioOperador)
        {
            ClsTarifa clsTarifa = new ClsTarifa();
            clsTarifa.Cantidad = tarifa;
            clsTarifa.IdUsuarioAlta = idUsuarioOperador;

            return (clsTarifa.Tarifa_create());
        }

        public string ProductoPosee_createController(int idProducto, int idTarifa, int idUsuarioOperador)
        {
            ClsProductoPosee clsProductoPosee = new ClsProductoPosee();
            clsProductoPosee.IdProducto = idProducto;
            clsProductoPosee.IdTarifa = idTarifa;
            clsProductoPosee.IdUsuarioAlta = idUsuarioOperador;

            return (clsProductoPosee.ProductoPosee_create());
        }

        public string ProductoPosee_Tarifa_Create_Controller(int idProducto, decimal cantidad, int idUsuarioOperador)
        {
            ClsProductoPosee clsProductoPosee = new ClsProductoPosee();
            clsProductoPosee.IdProducto = idProducto;
            clsProductoPosee.IdUsuarioAlta = idUsuarioOperador;

           return( clsProductoPosee.ProductoPosee_Tarifa_Create(cantidad) );
        }

        //-----------------------------Utils
        private void MostrarProductosEnGrid(DataTable productosTable)
        {
            dataGridView1.DataSource = productosTable;
            dataGridView1.Columns[0].Visible = false;
        }

        private void MostrarTarifasDeProductoEnGrid(DataTable tarifasDeProductoTable)
        {
            dataGridView2.DataSource = tarifasDeProductoTable;
            dataGridView2.Columns[0].Visible = false;
            dataGridView2.Columns[1].Visible = false;
            dataGridView2.Columns[2].Visible = false;
        }

        private void CrearColumnasEnGridParaSeleccionarProducto()
        {
            DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
            chk.Name = "Seleccionar";
            chk.HeaderText = "Seleccionar";

            dataGridView3.Columns.Add(chk);
            dataGridView3.Columns.Add("Id", "Id");
            dataGridView3.Columns.Add("Producto", "Producto");

            dataGridView3.Columns[1].Visible = false;
            dataGridView3.Columns[2].ReadOnly = true;
        }

        private void AniadirNombresProductoAGrid(DataTable productos)
        {
            var res = productos.AsEnumerable();
            List<DataRow> listDataRows = res.ToList<DataRow>();

            Action<DataRow> AniadirADataGrid = fila => {
                int n = dataGridView3.Rows.Add();
                dataGridView3.Rows[n].Cells[1].Value = fila.Field<int>("Id");  //Extraer el nombre de la datatable y asignarlo al grid
                dataGridView3.Rows[n].Cells[2].Value = fila.Field<string>("Nombre");
            };

            listDataRows.ForEach(AniadirADataGrid);
        }

        private void ReinicializarTodosGroupBox()
        {
            dataGridView1.DataSource = null;
            dataGridView2.DataSource = null;
            dataGridView3.Rows.Clear();
            textBox1.Text = "";

            DataTable res = Producto_Select_Id_Nombre_DeTodosController();
            MostrarProductosEnGrid(res);
            AniadirNombresProductoAGrid(res);
        }

        //-------------------------------Events
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    //Buscar los productos de ese id
                    int idProducto = Int32.Parse( dataGridView1.Rows[e.RowIndex].Cells[0].EditedFormattedValue.ToString() );
                    DataTable res = ProductoPosee_innerjoin_TarifasController(idProducto);
                    MostrarTarifasDeProductoEnGrid(res);
                }
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                ClsMyException clsMyException = new ClsMyException();
                string res = clsMyException.FormarTextoDeSqlException(ex);

                MessageBox.Show(res, "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }



        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn &&
                e.RowIndex >= 0)
            {
                Action<DataGridViewRow> DeSeleccionarTodosCheckBoxExceptoEnDondeOcurrioElEvento = fila =>
                {
                    if (fila.Index != e.RowIndex)
                    {
                        fila.Cells[0].Value = false;
                    }
                };

                IEnumerable<DataGridViewRow> coleccionFilas = dataGridView3.Rows.Cast<DataGridViewRow>();
                coleccionFilas.ToList().ForEach(DeSeleccionarTodosCheckBoxExceptoEnDondeOcurrioElEvento);


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult res = MessageBox.Show("¿Esta usted seguro que desea continuar?", "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    IEnumerable<DataGridViewRow> coleccionFilas = dataGridView3.Rows.Cast<DataGridViewRow>();
                    var x = coleccionFilas.ToList<DataGridViewRow>();

                    Func<DataGridViewRow, bool> BuscarFilaSeleccionada = item => {
                        if (item.Cells[0].Value != null)
                        {
                            DataGridViewCheckBoxCell celda = (DataGridViewCheckBoxCell)item.Cells[0];
                            if (celda.Value.ToString() == "True")
                                return true;
                            else
                                return false;
                        }
                        return false;
                    };


                    DataGridViewRow filaSeleccionada = x.SingleOrDefault(BuscarFilaSeleccionada);
                    if (filaSeleccionada == null)
                    {
                        MessageBox.Show("Necesita seleccionar algún producto", "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                    else
                    {
                        int idProducto = Int32.Parse(filaSeleccionada.Cells[1].EditedFormattedValue.ToString());
                        string mensaje = ProductoPosee_Tarifa_Create_Controller(idProducto, Decimal.Parse(textBox1.Text), ClsLogin.Id);

                        if (mensaje.Contains("ok"))
                        {
                            MessageBox.Show("Nueva tarifa agregada", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ReinicializarTodosGroupBox();
                        }

                        else
                        {
                            MessageBox.Show("No fue posible agregar tarifa deseada", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                }
             
            }


            catch (System.Data.SqlClient.SqlException ex)
            {
                ClsMyException clsMyException = new ClsMyException();
                string res = clsMyException.FormarTextoDeSqlException(ex);

                MessageBox.Show(res, "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }
    }
}
