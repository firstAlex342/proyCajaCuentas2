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
    public partial class FrmBuscarFoliosDeSocio : Form
    {
        //-------------------Constructor
        public FrmBuscarFoliosDeSocio()
        {
            InitializeComponent();
        }

        //------------------Methods controller
        private DataTable Socio_BuscarFoliosActivosDeReciboListaProductosController(string licenciaBuscada, DateTime fechaInicio, DateTime fechaFin)
        {
            ClsSocio clsSocio = new ClsSocio();
            clsSocio.NumeroLicencia = licenciaBuscada;
            clsSocio.FechaAlta = fechaInicio;
            clsSocio.FechaModificacion = fechaFin;

            return (clsSocio.Socio_BuscarFoliosActivosDeReciboListaProductos());
        }


        private DataTable MovsEnCaja_BuscarDetallesDeMovimientoController(int idMovimientoBuscado)
        {
            ClsMovsEnCaja clsMovsEnCaja = new ClsMovsEnCaja();
            clsMovsEnCaja.IdMovimiento = idMovimientoBuscado;

            return (clsMovsEnCaja.MovsEnCaja_BuscarDetallesDeMovimiento());
        }



        private DataTable MovsEnCaja_BuscarReciboLicenciaXIdMovimientoController(int idMovimientoBuscado)
        {
            ClsMovsEnCaja clsMovsEnCaja = new ClsMovsEnCaja();
            clsMovsEnCaja.IdMovimiento = idMovimientoBuscado;

            return ( clsMovsEnCaja.MovsEnCaja_BuscarReciboLicenciaXIdMovimiento());
        }


        //-------------------Utils
        private void MostrarFoliosDeSocioEnGrid(DataTable tabla)
        {
            
            this.dataGridView1.DataSource = tabla;
            this.dataGridView1.Columns[0].Visible = false;
            this.dataGridView1.Columns[1].Visible = false;
            this.dataGridView1.Columns[2].Visible = false;
            
            this.dataGridView1.Columns[5].Visible = false;

            dataGridView1.Columns[4].HeaderText = "Fecha alta";
            //dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }


        private void MostrarProductosDeMovEnGrid(DataTable detallesMovimientoTabla)
        {

            this.dataGridView2.DataSource = detallesMovimientoTabla;
            this.dataGridView2.Columns[0].Visible = false;
            this.dataGridView2.Columns[1].Visible = false;
            this.dataGridView2.Columns[2].Visible = false;
            this.dataGridView2.Columns[3].Visible = false;
            this.dataGridView2.Columns[4].Visible = false;
            this.dataGridView2.Columns[5].Visible = false;
            this.dataGridView2.Columns[6].Visible = false;
            this.dataGridView2.Columns[7].Visible = false;  //PagoProducto.IdProducto

            this.dataGridView2.Columns[8].HeaderText = "Cantidad pagada";
            this.dataGridView2.Columns[9].HeaderText = "Tarifa";
            this.dataGridView2.Columns[10].HeaderText = "Descuento";

            this.dataGridView2.Columns[11].Visible = false;

            this.dataGridView2.Columns[12].HeaderText = "Nombre de producto";

            this.dataGridView2.Columns[13].Visible = false;
            this.dataGridView2.Columns[14].Visible = false;
            this.dataGridView2.Columns[15].Visible = false;
            this.dataGridView2.Columns[16].Visible = false;
            this.dataGridView2.Columns[17].Visible = false;
            this.dataGridView2.Columns[18].Visible = false;
            this.dataGridView2.Columns[19].Visible = false;
            this.dataGridView2.Columns[20].Visible = false;
            this.dataGridView2.Columns[21].Visible = false;
            this.dataGridView2.Columns[22].Visible = false;
            this.dataGridView2.Columns[23].Visible = false;
            this.dataGridView2.Columns[24].Visible = false;
            this.dataGridView2.Columns[25].Visible = false;
            this.dataGridView2.Columns[26].Visible = false;
            this.dataGridView2.Columns[27].Visible = false;
            this.dataGridView2.Columns[28].Visible = false;
            this.dataGridView2.Columns[29].Visible = false;
        }

        private void MostrarInfoDeSocioEnTabControl(DataTable detallesMovimientoTabla)
        {
            DataRow fila = (detallesMovimientoTabla.AsEnumerable()).First<DataRow>();
            //Socio.Id es columna 12
            textBox18.Text = fila.Field<string>("Socio.NumeroLicencia");
            textBox5.Text = fila.Field<string>("Socio.NombreComercial");
            textBox12.Text = fila.Field<string>("Socio.DireccionSupmza");
            textBox6.Text = fila.Field<string>("Socio.DireccionLote");
            textBox13.Text = fila.Field<string>("Socio.DireccionManzana");
            textBox7.Text = fila.Field<string>("Socio.DireccionCalle");
            textBox14.Text = fila.Field<string>("Socio.DireccionComplemento");
            textBox8.Text = fila.Field<string>("Socio.PropietarioPatente");
            textBox15.Text = fila.Field<string>("Socio.RFCPropietario");
            textBox9.Text = fila.Field<string>("Socio.Comodatario");
            textBox16.Text = fila.Field<string>("Socio.RFCComodatario");
            textBox10.Text = fila.Field<string>("Socio.Telefono");
            textBox17.Text = fila.Field<string>("Socio.Celular");
            textBox11.Text = fila.Field<string>("Socio.CorreoElectronico");
        }

        private void MostrarTotalDeMovimientoEnTextBox(DataTable detallesMovimientoTabla)
        {
            DataRow fila = (detallesMovimientoTabla.AsEnumerable()).First<DataRow>();
            textBox3.Text = fila.Field<Decimal>("MovsEnCaja.Cantidad").ToString();
        }

        private void MostrarUsuarioCapturoMovimiento(DataTable detallesMovimientoTabla)
        {
            DataRow fila = (detallesMovimientoTabla.AsEnumerable()).First<DataRow>();
            textBox2.Text = fila.Field<string>("Usuario.Nombre");
        }

        private void LimpiarTodoContenidoGroupBoxDetallesDeFolio()
        {
            textBox2.Text = "";
            dataGridView2.DataSource = null;
            textBox3.Text = "";

            //limpiar pestaña 2
            textBox18.Text = "";
            textBox12.Text = "";
            textBox13.Text = "";
            textBox14.Text = "";
            textBox15.Text = "";
            textBox16.Text = "";
            textBox17.Text = "";

            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";

            //Limpia pestaña 3
            textBox4.Text = "";
        }

        private void MostrarEnTextBoxReciboLicencia(DataTable infoReciboLicenciaTabla)
        {
            if(infoReciboLicenciaTabla.Rows.Count == 1)
            {
                DataRow fila = (infoReciboLicenciaTabla.AsEnumerable()).First<DataRow>();
                textBox4.Text = fila.Field<string>("Folio");
            }
        }

        //------------------Events
        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fechaInicio;
                DateTime fechaFin;

                if(radioButton1.Checked == true)
                {
                    fechaInicio = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month,
                        dateTimePicker1.Value.Day, 0, 1, 0);

                    fechaFin = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month,
                        dateTimePicker1.Value.Day, 23,59, 58);
                }

                else 
                {
                    fechaInicio = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, 
                        dateTimePicker2.Value.Day, 0, 1, 0);

                    fechaFin = new DateTime(dateTimePicker3.Value.Year, dateTimePicker3.Value.Month,
                        dateTimePicker3.Value.Day, 23, 59, 58);
                }


                dataGridView1.DataSource = null;
                LimpiarTodoContenidoGroupBoxDetallesDeFolio();
                

                DataTable listaFolios = Socio_BuscarFoliosActivosDeReciboListaProductosController(textBox1.Text, fechaInicio, fechaFin);

                MessageBox.Show("Se encontraron " + listaFolios.Rows.Count.ToString() + " folios", "Resultados de búsqueda",MessageBoxButtons.OK, MessageBoxIcon.Information);
                MostrarFoliosDeSocioEnGrid(listaFolios);

                
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }



        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    LimpiarTodoContenidoGroupBoxDetallesDeFolio();

                    DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];
                    DataGridViewCell celda = fila.Cells[0];
                    int idMovimientoBuscado = Int32.Parse(celda.EditedFormattedValue.ToString());
                    DataTable detallesDeMovTabla = MovsEnCaja_BuscarDetallesDeMovimientoController(idMovimientoBuscado);
                    DataTable detallesDeReciboLicenciaTabla = MovsEnCaja_BuscarReciboLicenciaXIdMovimientoController(idMovimientoBuscado);

                    MostrarProductosDeMovEnGrid(detallesDeMovTabla);
                    MostrarTotalDeMovimientoEnTextBox(detallesDeMovTabla);
                    MostrarInfoDeSocioEnTabControl(detallesDeMovTabla);
                    MostrarUsuarioCapturoMovimiento(detallesDeMovTabla);
                    MostrarEnTextBoxReciboLicencia(detallesDeReciboLicenciaTabla);
                }
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
            
        }



        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if( radioButton1.Checked == false)
            {
                //Entonces radioButton2.Checked es true, por lo tanto...
                dateTimePicker1.Enabled = false;             
                dateTimePicker2.Enabled = true;
                dateTimePicker3.Enabled = true;
            }

            else if(radioButton1.Checked == true)
            {
                //Entonces radioButton1.Checked es true, por lo tanto...
                dateTimePicker1.Enabled = true;
                dateTimePicker2.Enabled = false;
                dateTimePicker3.Enabled = false;
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }


    }
}
