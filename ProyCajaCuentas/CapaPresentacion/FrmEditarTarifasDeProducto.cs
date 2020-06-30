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
    public partial class FrmEditarTarifasDeProducto : Form
    {
        public FrmEditarTarifasDeProducto()
        {
            InitializeComponent();

            try
            {
                DataTable res = Producto_Select_Id_Nombre_DeTodosController();
                MostrarProductosEnGrid(res);
                CrearColumnasEnGridParaSeleccionarTarifa();
                
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

        public void ProductoPosee_Tarifa_updateActivoACeroController(DataGridView grid)
        {
            IEnumerable<DataGridViewRow> coleccionFilas = grid.Rows.Cast<DataGridViewRow>();
            Action<DataGridViewRow> funcionCambiaActivoACero = item =>
            {
                if(item.Cells[0].EditedFormattedValue.ToString() == "True")
                {
                    ClsProductoPosee clsProductoPosee = new ClsProductoPosee();
                    clsProductoPosee.IdProducto = Int32.Parse(item.Cells[1].EditedFormattedValue.ToString());
                    clsProductoPosee.IdTarifa = Int32.Parse(item.Cells[2].EditedFormattedValue.ToString());
                    clsProductoPosee.ProductoPosee_Tarifa_updateActivoACero();
                }
            };

            coleccionFilas.ToList().ForEach(funcionCambiaActivoACero);
        }

        public string  ProductoPosee_Tarifa_UpdateActivoACero_CollectionController(DataGridView grid, int idUsuarioOperador)
        {
            ClsTarifasYEstados clsTarifasYEstados = new ClsTarifasYEstados();
            IEnumerable<DataGridViewRow> coleccionFilas = grid.Rows.Cast<DataGridViewRow>();
            Action<DataGridViewRow> funcionLLenarColleccion = item =>
            {
                if (item.Cells[0].EditedFormattedValue.ToString() == "True")
                    clsTarifasYEstados.AddItem(Int32.Parse(item.Cells[2].EditedFormattedValue.ToString()), false);

                else
                    clsTarifasYEstados.AddItem(Int32.Parse(item.Cells[2].EditedFormattedValue.ToString()), true);           
            };

            coleccionFilas.ToList().ForEach(funcionLLenarColleccion);


            DataGridViewRow fila = coleccionFilas.First();
            ClsProductoPosee clsProductoPosee = new ClsProductoPosee();   
            clsProductoPosee.IdProducto = Int32.Parse(fila.Cells[1].EditedFormattedValue.ToString());
            clsProductoPosee.IdUsuarioModifico = idUsuarioOperador;
            return (clsProductoPosee.ProductoPosee_Tarifa_UpdateActivoACero_Collection(clsTarifasYEstados) );
        }

        //-----------------------------Utils
        private void MostrarProductosEnGrid(DataTable productosTable)
        {
            dataGridView1.DataSource = productosTable;
            dataGridView1.Columns[0].Visible = false;

            dataGridView1.CurrentCell = null;
            dataGridView1.ClearSelection();
            
        }

        private void MostrarTarifasDeProductoEnGrid(DataTable tarifasDeProductoTable)
        {
            var res = tarifasDeProductoTable.AsEnumerable();
            List<DataRow> listDataRows = res.ToList<DataRow>();

            Action<DataRow> AniadirADataGrid = fila => {
                int n = dataGridView2.Rows.Add();
                dataGridView2.Rows[n].Cells[0].Value = false;
                dataGridView2.Rows[n].Cells[1].Value = fila.Field<int>("IdProducto");  
                dataGridView2.Rows[n].Cells[2].Value = fila.Field<int>("IdTarifa");
                dataGridView2.Rows[n].Cells[3].Value = fila.Field<int>("Id");
                dataGridView2.Rows[n].Cells[4].Value = fila.Field<decimal>("Cantidad");
            };

            listDataRows.ForEach(AniadirADataGrid);
        }

        private void CrearColumnasEnGridParaSeleccionarTarifa()
        {
            DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
            chk.Name = "Seleccionar";
            chk.HeaderText = "Seleccionar";

            dataGridView2.Columns.Add(chk);
            dataGridView2.Columns.Add("IdProducto", "IdProducto");
            dataGridView2.Columns.Add("IdTarifa", "IdTarifa");
            dataGridView2.Columns.Add("Id", "Id");
            dataGridView2.Columns.Add("Cantidad", "Cantidad");


            dataGridView2.Columns[1].Visible = false;
            dataGridView2.Columns[2].Visible = false;
            dataGridView2.Columns[3].Visible = false;
            dataGridView2.Columns[4].ReadOnly = true;
        }

        private bool ExisteTarifaSeleccionadaParaEliminar(DataGridView grid)
        {
            IEnumerable<DataGridViewRow> filas = grid.Rows.Cast<DataGridViewRow>();
            Func<DataGridViewRow, bool> buscaCasillaSeleccionada = item =>
            {
                if (item.Cells[0].EditedFormattedValue.ToString() == "True")
                    return true;
                else
                    return false;
            };

            var x = from item in filas
                    where buscaCasillaSeleccionada(item)
                    select item;

            return (x.Count() > 0);
        }
        


        //----------------------Events
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    //Buscar los productos de ese id
                    int idProducto = Int32.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].EditedFormattedValue.ToString());
                    DataTable res = ProductoPosee_innerjoin_TarifasController(idProducto);
                    dataGridView2.Rows.Clear();
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



        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult res = MessageBox.Show("¿Esta usted seguro que desea continuar?", "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {

                    if( ExisteTarifaSeleccionadaParaEliminar(dataGridView2) )
                    {
                        string mensaje = ProductoPosee_Tarifa_UpdateActivoACero_CollectionController(dataGridView2, ClsLogin.Id);
                        if(mensaje.Contains("ok"))
                        {
                            MessageBox.Show("Lista de productos y sus tarifas actualizada", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            DataTable tabla = Producto_Select_Id_Nombre_DeTodosController();
                            MostrarProductosEnGrid(tabla);
                            dataGridView2.Rows.Clear();
                        }

                        else
                        {
                            MessageBox.Show(mensaje, "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Error);                          
                            DataTable tabla = Producto_Select_Id_Nombre_DeTodosController();
                            MostrarProductosEnGrid(tabla);
                            dataGridView2.Rows.Clear();
                        }
                    }

                    else
                    {
                        MessageBox.Show("Seleccione alguna tarifa a eliminar", "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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


        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable tabla = Producto_Select_Id_Nombre_DeTodosController();
                MostrarProductosEnGrid(tabla);
                dataGridView2.Rows.Clear();
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
