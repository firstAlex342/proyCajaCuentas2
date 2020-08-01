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
    public partial class FrmProveedorEditarElemento : Form
    {
        //---------------constructor
        public FrmProveedorEditarElemento()
        {
            InitializeComponent();

            try
            {
                DataTable datosDeProveedores = Proveedor_SelectTodosActivosController();
                MostrarProveedoresEnDataGrid(datosDeProveedores);
                CrearColumnasEnGridParaSeleccionarElemento();
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

        //--------------controllers
        public DataTable Proveedor_SelectTodosActivosController()
        {
            ClsProveedor clsProveedor = new ClsProveedor();
            return (clsProveedor.Proveedor_SelectTodosActivos());
        }

        public DataTable Proveedor_BuscarElementosProveidosActivosController(int idProveedor)
        {
            ClsProveedor clsProveedor = new ClsProveedor();
            clsProveedor.Id = idProveedor;

            return (clsProveedor.Proveedor_BuscarElementosProveidosActivos());
        }

        public string ProveedorProveeElemento_Update_ElementoProveido_ActivoACeroController(int idProveedor, int idElementoProveido, int idUsuarioOperador)
        {
            ClsProveedorProveeElemento clsProveedorProveeElemento = new ClsProveedorProveeElemento();
            clsProveedorProveeElemento.IdProveedor = idProveedor;
            clsProveedorProveeElemento.IdElementoProveido = idElementoProveido;
            clsProveedorProveeElemento.IdUsuarioModifico = idUsuarioOperador;

            return ( clsProveedorProveeElemento.ProveedorProveeElemento_Update_ElementoProveido_ActivoACero());
        }

        //-------------Methods
        private void MostrarProveedoresEnDataGrid(DataTable datosDeProveedorTable)
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = datosDeProveedorTable;

            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[3].Visible = false;
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[6].Visible = false;
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[8].Visible = false;
            dataGridView1.Columns[9].Visible = false;
        }

        private void CrearColumnasEnGridParaSeleccionarElemento()
        {
            DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
            chk.Name = "Seleccionar";
            chk.HeaderText = "Seleccionar";

            dataGridView2.Columns.Add(chk);
            dataGridView2.Columns.Add("Id", "Id");
            dataGridView2.Columns.Add("Nombre", "Nombre");

            dataGridView2.Columns[1].Visible = false;
            dataGridView2.Columns[2].ReadOnly = true;
        }

        private void MostrarElementosProveidosDeProveedorEnDataGrid(DataTable elementosProveidosTable)
        {
            var res = elementosProveidosTable.AsEnumerable();
            List<DataRow> listaDataRows = res.ToList<DataRow>();

            Action<DataRow> AniadirADataGrid = fila => {
                int n = dataGridView2.Rows.Add();
                dataGridView2.Rows[n].Cells[1].Value = fila.Field<int>("ElementoProveido.Id");
                dataGridView2.Rows[n].Cells[2].Value = fila.Field<string>("Nombre de elemento");
            };

            listaDataRows.ForEach(AniadirADataGrid);
        }

        private object BuscarFilaSeleccionada(DataGridView dataGridViewX)
        {
            Func<DataGridViewRow, bool> delegado = fila =>
            {
                if(fila.Cells[0].Value != null)
                {
                    DataGridViewCheckBoxCell miCheckBox = (DataGridViewCheckBoxCell)fila.Cells[0];
                    if (miCheckBox.Value.ToString() == "True")
                        return (true);
                    else
                        return (false);
                }
                return false;
            };


            IEnumerable<DataGridViewRow> coleccionFilas = dataGridViewX.Rows.Cast<DataGridViewRow>();
            var x = coleccionFilas.ToList<DataGridViewRow>();

            return (x.SingleOrDefault(delegado));
        }

        public void ReinicializarDataGrids()
        {
            DataTable datosDeProveedores = Proveedor_SelectTodosActivosController();
            MostrarProveedoresEnDataGrid(datosDeProveedores);

            dataGridView2.DataSource = null;
            dataGridView2.Rows.Clear();
        }

        //-----------------------------Events
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    //Buscar los productos de ese id
                    int idProveedor = Int32.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].EditedFormattedValue.ToString());
                    DataTable res = Proveedor_BuscarElementosProveidosActivosController(idProveedor);
                    dataGridView2.Rows.Clear();
                    MostrarElementosProveidosDeProveedorEnDataGrid(res);
                }
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                StringBuilder errorMessages = new StringBuilder();
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    errorMessages.Append("Index #" + i + "\n" +
                        "Message: " + ex.Errors[i].ToString() + "\n" +
                        "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                        "Source: " + ex.Errors[i].Source + "\n" +
                        "Procedure: " + ex.Errors[i].Procedure + "\n");
                }
                MessageBox.Show(errorMessages.ToString(), "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
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

                IEnumerable<DataGridViewRow> coleccionFilas = dataGridView2.Rows.Cast<DataGridViewRow>();
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
                    DataGridViewRow filaActual = dataGridView1.CurrentRow;
                    object filaSeleccionada = BuscarFilaSeleccionada(dataGridView2);

                    if (filaActual.Index >= 0)
                    {
                        if (filaSeleccionada != null)
                        {
                            DataGridViewRow fila = (DataGridViewRow)filaSeleccionada;
                            int idElemento = Int32.Parse(fila.Cells[1].EditedFormattedValue.ToString());
                            int idProveedor = Int32.Parse(filaActual.Cells[0].EditedFormattedValue.ToString());

                            string mensaje = ProveedorProveeElemento_Update_ElementoProveido_ActivoACeroController(idProveedor, idElemento, ClsLogin.Id);
                            if (mensaje.Contains("ok"))
                            {
                                MessageBox.Show("Elemento eliminado", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                ReinicializarDataGrids();
                            }

                            else
                            {
                                MessageBox.Show(mensaje, "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }

                        else
                        {
                            MessageBox.Show("Es necesario seleccionar un elemento proveído", "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }

                    else
                    {
                        MessageBox.Show("Es necesario elegir un proveedor", "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }//if
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
