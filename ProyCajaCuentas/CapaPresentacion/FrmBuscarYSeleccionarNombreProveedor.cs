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
    public partial class FrmBuscarYSeleccionarNombreProveedor : Form
    {
        private string nombreProveeedorSeleccionado;
        private int idProveedorSeleccionado;

        //------------Properties
        public string NombreProveeedorSeleccionado
        {
            set { nombreProveeedorSeleccionado = value; }
            get { return nombreProveeedorSeleccionado; }
        }

        public int IdProveedorSeleccionado
        {
            set { idProveedorSeleccionado = value; }
            get { return idProveedorSeleccionado; }
        }


        //------------------constructor
        public FrmBuscarYSeleccionarNombreProveedor()
        {
            InitializeComponent();

            this.NombreProveeedorSeleccionado = "";
            this.IdProveedorSeleccionado = 0;
            CrearColumnasParaDataGridViewNombreDeProveedoresActivos();

            try
            {
                DataTable datosDeProveedorTable = Proveedor_SelectTodosActivosController();
                LLenarDataGridConDatos(datosDeProveedorTable);
                label2.Text = label2.Text + " " + datosDeProveedorTable.Rows.Count.ToString();
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }


        //-----------------controllers
        private DataTable Proveedor_SelectTodosActivosController()
        {
            ClsProveedor clsProveedor = new ClsProveedor();
            return (clsProveedor.Proveedor_SelectTodosActivos());
        }

        private DataTable Proveedor_BuscarXNombreYQueEstenActivosController(string nombreBuscado)
        {
            ClsProveedor clsProveedor = new ClsProveedor();
            clsProveedor.Nombre = nombreBuscado;
            return (clsProveedor.Proveedor_BuscarXNombreYQueEstenActivos());
        }



        //--------------------------Utils
        private void CrearColumnasParaDataGridViewNombreDeProveedoresActivos()
        {
            DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
            chk.Name = "Seleccionar";
            chk.HeaderText = "Seleccionar";


            dataGridView1.Columns.Add(chk);
            dataGridView1.Columns.Add("Id", "Id");
            dataGridView1.Columns.Add("Proveedor", "Nombre de proveedor");
            dataGridView1.Columns.Add("DireccionSupmza", "Supermanzana");
            dataGridView1.Columns.Add("DireccionManzana", "Manzana");
            dataGridView1.Columns.Add("DireccionLote", "Lote");
            dataGridView1.Columns.Add("DireccionCalle", "Calle");
            dataGridView1.Columns.Add("DireccionComplemento", "Complemento");
            dataGridView1.Columns.Add("Telefono", "Teléfono");
            dataGridView1.Columns.Add("Celular", "Celular");
            dataGridView1.Columns.Add("CorreoElectronico", "Correo electrónico");


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

            dataGridView1.Columns[1].Visible = false;
        }

        private void LLenarDataGridConDatos(DataTable datosDeProveedorTable)
        {
            var res = datosDeProveedorTable.AsEnumerable();
            List<DataRow> listDataRows = res.ToList<DataRow>();

            Action<DataRow> AniadirADataGrid = fila => {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[1].Value = fila.Field<int>("Id");
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if(radioButton1.Checked == true)
                {
                    textBox1.Text = "";
                    dataGridView1.Rows.Clear();
                    label2.Text = "";
                    DataTable datosDeProveedorTable = Proveedor_SelectTodosActivosController();
                    LLenarDataGridConDatos(datosDeProveedorTable);
                    label2.Text = "Elementos encontrados ";
                    label2.Text += datosDeProveedorTable.Rows.Count > 0 ? datosDeProveedorTable.Rows.Count.ToString() : "0";

                }

                else
                {
                    dataGridView1.Rows.Clear();
                    label2.Text = "";
                    DataTable datosDeProveedorTable = Proveedor_BuscarXNombreYQueEstenActivosController(textBox1.Text);
                    LLenarDataGridConDatos(datosDeProveedorTable);
                    label2.Text = "Elementos encontrados ";
                    label2.Text += datosDeProveedorTable.Rows.Count > 0 ? datosDeProveedorTable.Rows.Count.ToString() : "0";

                }
            }


            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                IEnumerable<DataGridViewRow> coleccionFilas = dataGridView1.Rows.Cast<DataGridViewRow>();
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
                if(filaSeleccionada == null)
                {
                    this.NombreProveeedorSeleccionado = String.Empty;
                }

                else
                {
                    this.NombreProveeedorSeleccionado = filaSeleccionada.Cells[2].EditedFormattedValue.ToString();
                    this.IdProveedorSeleccionado = Int32.Parse(filaSeleccionada.Cells[1].EditedFormattedValue.ToString());
                }

                this.Visible = false;
            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }
    }
}
