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
    public partial class FrmProveedorAgregarElemento : Form
    {
        //----------------constructor
        public FrmProveedorAgregarElemento()
        {
            InitializeComponent();

            try
            {
                DataTable res = Proveedor_SelectTodosActivosController();
                MostrarProveedoresEnDataGrid(res);
                CrearColumnasEnGridParaSeleccionarProveedor();
                AniadirNombresProveedorAGrid(res);
            }


            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source);
            }
        }//parameterless constructor

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

        public string Proveedor_ProveeElemento_ElementoProveido_createController(string nombreElemento, int idProveedor,
            int idUsuarioOperador)
        {
            ClsProveedorProveeElementoElementoProveido clsProveedorProveeElementoElementoProveido = new ClsProveedorProveeElementoElementoProveido();
            clsProveedorProveeElementoElementoProveido.IdProveedor = idProveedor;
            clsProveedorProveeElementoElementoProveido.NombreElemento = nombreElemento;
            clsProveedorProveeElementoElementoProveido.IdUsuarioAlta = idUsuarioOperador;

            return (clsProveedorProveeElementoElementoProveido.Proveedor_ProveeElemento_ElementoProveido_create() );
        }


        //--------------------Methods
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

        private void MostrarElementosProveidosDeProveedorEnDataGrid(DataTable elementosProveidosTable)
        {
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = elementosProveidosTable;

            dataGridView2.Columns[0].Visible = false;
            dataGridView2.Columns[1].Visible = false;
            dataGridView2.Columns[2].Visible = false;
            dataGridView2.Columns[3].Visible = false;
        }

        private void CrearColumnasEnGridParaSeleccionarProveedor()
        {
            DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
            chk.Name = "Seleccionar";
            chk.HeaderText = "Seleccionar";

            dataGridView3.Columns.Add(chk);
            dataGridView3.Columns.Add("Id", "Id");
            dataGridView3.Columns.Add("Proveedor", "Proveedor");

            dataGridView3.Columns[1].Visible = false;
            dataGridView3.Columns[2].ReadOnly = true;
        }

        private void AniadirNombresProveedorAGrid(DataTable proveedores)
        {
            var res = proveedores.AsEnumerable();
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

            DataTable res = Proveedor_SelectTodosActivosController();
            MostrarProveedoresEnDataGrid(res);
            AniadirNombresProveedorAGrid(res);
        }

        private bool TieneAlgoMasQueEspaciosEnBlanco(string texto)
        {
            string cad = texto.Trim();

            bool res = cad.Length > 0 ? true : false;
            return (res);
        }

        //--------------------Events
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    //Buscar los productos de ese id
                    int idProveedor= Int32.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].EditedFormattedValue.ToString());

                    DataTable res = Proveedor_BuscarElementosProveidosActivosController(idProveedor);
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
                    bool seCapturoNombreElemento = TieneAlgoMasQueEspaciosEnBlanco(textBox1.Text);

                    if(seCapturoNombreElemento)
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
                            MessageBox.Show("Necesita seleccionar algún proveedor", "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }

                        else
                        {
                            int idProveedor = Int32.Parse(filaSeleccionada.Cells[1].EditedFormattedValue.ToString());
                            string mensaje = Proveedor_ProveeElemento_ElementoProveido_createController(textBox1.Text.Trim(), idProveedor, ClsLogin.Id);

                            if (mensaje.Contains("ok"))
                            {
                                MessageBox.Show("Nuevo elemento de proveedor agregado", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                ReinicializarTodosGroupBox();
                            }

                            else
                            {
                                MessageBox.Show("No fue posible agregar elemento a proveedor. " + mensaje, "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }
                    }

                    else
                    {
                        MessageBox.Show("Necesita capturar algún nombre para el elemento", "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }
    }
}
