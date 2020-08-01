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
    public partial class FrmProveedorEliminar : Form
    {
        //------------------constructor
        public FrmProveedorEliminar()
        {
            InitializeComponent();
            CrearColumnasParaDataGridViewNombreDeProveedoresActivos();

            try
            {
                DataTable datosDeProveedorTable = Proveedor_SelectTodosActivosController();
                LLenarDataGridConDatos(datosDeProveedorTable);
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

        //---------------------methods controller
        private DataTable Proveedor_SelectTodosActivosController()
        {
            ClsProveedor clsProveedor = new ClsProveedor();
            return (clsProveedor.Proveedor_SelectTodosActivos());
        }


        public string Proveedor_updateActivoACeroController(int idProveedor, int idUsuarioOperador)
        {
            ClsProveedor clsProveedor = new ClsProveedor();
            clsProveedor.Id = idProveedor;
            clsProveedor.IdUsuarioModifico = idUsuarioOperador;

            return (clsProveedor.Proveedor_updateActivoACero() );
        }

        //-----------------------------utils
        private void CrearColumnasParaDataGridViewNombreDeProveedoresActivos()
        {
            DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
            chk.Name = "Seleccionar";
            chk.HeaderText = "Seleccionar";

            dataGridView1.Columns.Add(chk);
            dataGridView1.Columns.Add("Id", "Id");
            dataGridView1.Columns.Add("Nombre", "Nombre");
            dataGridView1.Columns.Add("DireccionSupmza", "Supermanzana");
            dataGridView1.Columns.Add("DireccionManzana", "Manzana");
            dataGridView1.Columns.Add("DireccionLote", "Lote");
            dataGridView1.Columns.Add("DireccionCalle", "Calle");
            dataGridView1.Columns.Add("DireccionComplemento", "Complemento");
            dataGridView1.Columns.Add("Telefono", "Teléfono");
            dataGridView1.Columns.Add("Celular", "Celular");
            dataGridView1.Columns.Add("CorreoElectronico", "Correo electrónico");

            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[3].ReadOnly = true;
            dataGridView1.Columns[4].ReadOnly = true;
            dataGridView1.Columns[5].ReadOnly = true;
            dataGridView1.Columns[6].ReadOnly = true;
            dataGridView1.Columns[7].ReadOnly = true;
            dataGridView1.Columns[8].ReadOnly = true;
            dataGridView1.Columns[9].ReadOnly = true;
            dataGridView1.Columns[10].ReadOnly = true;
        }

        private void LLenarDataGridConDatos(DataTable datosDeProveedorTable)
        {
            var res = datosDeProveedorTable.AsEnumerable();
            List<DataRow> listDataRows = res.ToList<DataRow>();

            Action<DataRow> AniadirADataGrid = fila => {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[1].Value = fila.Field<int>("Id").ToString();
                dataGridView1.Rows[n].Cells[2].Value = fila.Field<string>("Nombre");  //Extraer el nombre de la datatable y asignarlo al grid
                dataGridView1.Rows[n].Cells[3].Value = fila.Field<string>("DireccionSupmza");
                dataGridView1.Rows[n].Cells[4].Value = fila.Field<string>("DireccionManzana");
                dataGridView1.Rows[n].Cells[5].Value = fila.Field<string>("DireccionLote");
                dataGridView1.Rows[n].Cells[6].Value = fila.Field<string>("DireccionCalle");
                dataGridView1.Rows[n].Cells[7].Value = fila.Field<string>("DireccionComplemento");
                dataGridView1.Rows[n].Cells[8].Value = fila.Field<string>("Telefono");
                dataGridView1.Rows[n].Cells[9].Value = fila.Field<string>("Celular");
                dataGridView1.Rows[n].Cells[10].Value = fila.Field<string>("CorreoElectronico");
            };

            listDataRows.ForEach(AniadirADataGrid);
        }

        private DataGridViewRow BuscarCheckBoxActivado(DataGridView grid)
        {
            IEnumerable<DataGridViewRow> filas = grid.Rows.Cast<DataGridViewRow>();
            Func<DataGridViewRow, bool> buscaCasillaSeleccionada = item =>
            {
                if (item.Cells[0].EditedFormattedValue.ToString() == "True")
                    return true;
                else
                    return false;
            };

            return( filas.SingleOrDefault(buscaCasillaSeleccionada)  );           
        }


        public void ReinicializarDataGrid()
        {
            dataGridView1.Rows.Clear();
            DataTable datosDeProveedorTable = Proveedor_SelectTodosActivosController();
            LLenarDataGridConDatos(datosDeProveedorTable);
        }


        //--------------------------Events
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
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

                IEnumerable<DataGridViewRow> coleccionFilas = dataGridView1.Rows.Cast<DataGridViewRow>();
                coleccionFilas.ToList().ForEach(DeSeleccionarTodosCheckBoxExceptoEnDondeOcurrioElEvento);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Esta usted seguro que desea continuar?", "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (respuesta == DialogResult.Yes)
                {
                    DataGridViewRow filaConCheckBoxSeleccionado = BuscarCheckBoxActivado(dataGridView1);
                    if (filaConCheckBoxSeleccionado != null   )
                    {
                        int idProveedor = Int32.Parse(filaConCheckBoxSeleccionado.Cells[1].EditedFormattedValue.ToString());
                        string mensaje = Proveedor_updateActivoACeroController(idProveedor, ClsLogin.Id);

                        if(mensaje.Contains("ok") )
                        {
                            MessageBox.Show("Proveedor eliminado", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dataGridView1.Rows.Clear();
                            DataTable datosDeProveedorTable = Proveedor_SelectTodosActivosController();
                            LLenarDataGridConDatos(datosDeProveedorTable);
                        }

                        else
                        {
                            MessageBox.Show(mensaje, "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }

                    else
                    {
                        MessageBox.Show("Necesita elegir un proveedor", "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }


            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }
    }
}
