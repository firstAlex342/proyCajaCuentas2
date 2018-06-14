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
    public partial class FrmAportacionesAProductoEnCuenta : Form
    {
        public FrmAportacionesAProductoEnCuenta()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
        }

        //----------------Methods
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


        private DataTable Socio_Select_Id_NombreComercial_DeTodosController()
        {
            ClsSocio clsSocio = new ClsSocio();
            return ( clsSocio.Socio_Select_Id_NombreComercial_DeTodos() );
        }


        private void MovimientoSocioProducto_createController(int idSocio, int idProducto, int idCuentaPorPagar, int idUsuario, decimal cantidad, DateTime fechaMovimiento)
        {
            ClsMovimientoSocioProducto clsMovimientoSocioProducto = new ClsMovimientoSocioProducto();
            clsMovimientoSocioProducto.IdSocio = idSocio;
            clsMovimientoSocioProducto.IdProducto = idProducto;
            clsMovimientoSocioProducto.IdCuentaPorPagar = idCuentaPorPagar;
            clsMovimientoSocioProducto.IdUsuario = idUsuario;
            clsMovimientoSocioProducto.Cantidad = cantidad;
            clsMovimientoSocioProducto.FechaMovimiento = fechaMovimiento;

            clsMovimientoSocioProducto.MovimientoSocioProducto_create();

        }


        //---------------Utils
        private void MostrarDetallesProductosAPagarEnCuenta(DataTable tabla)
        {
            dataGridView1.DataSource = tabla;
        }

        private void MostrarProductosQSePuedenPagar(DataTable respuesta)
        {
            //Se carga el combobox con valores de los productos que se pueden pagar
            var s = from item in respuesta.AsEnumerable()
                    select new { IdProducto = item.Field<int>("IdProducto") , NombreProducto = item.Field<string>("Nombre") };

            var s2 = from item in s
                     select item.IdProducto.ToString() + "-" + item.NombreProducto;

            foreach(string item in s2)
            {
                comboBox2.Items.Add(item);
            }
        }


        private void MostrarSocios_Id_NombreComercial(DataTable tabla)
        {
            //tabla viene con 2 columnas, el id del socio y el nombre comercial
            var res = from s in tabla.AsEnumerable()
                      select new { IdSocio = s.Field<int>(0), NombreComercial = s.Field<string>(1) };

            string[] lista = (from s in res
                              select s.IdSocio.ToString() + "-" + s.NombreComercial).ToArray<string>();

            comboBox1.Items.AddRange(lista);
        }

        private void CrearColumnasParaDataGridViewAportacionesPorIngresar()
        {
            //Es el dataGridView donde capturo cantidades
            dataGridView2.Columns.Add("Socio", "Socio");
            dataGridView2.Columns.Add("Id_CuentaXPagar", "Id_CuentaXPagar");
            dataGridView2.Columns.Add("Producto", "Producto");
            dataGridView2.Columns.Add("Cantidad", "Cantidad");
            dataGridView2.Columns.Add("Fecha Movimiento", "Fecha Movimiento");



            dataGridView2.Columns[0].ReadOnly = true;
            dataGridView2.Columns[1].ReadOnly = true;
            dataGridView2.Columns[2].ReadOnly = true;
            dataGridView2.Columns[3].ReadOnly = true;
            dataGridView2.Columns[4].ReadOnly = true;
        }

        private bool SeCapturoInfoCorrecta()
        {
            //Echa un vistazo a los comboBox
            if ((comboBox1.SelectedIndex >= 0) && (comboBox2.SelectedIndex >= 0))
            {   /*Se Eligio algo de los 2 comBoBox . Todo va bien hasta ahora*/ }

            else
            { return (false); }


            //echa un vistazo al textBox donde se debe capturar una cantidad
            decimal cantidad;
            bool esCorrectaConversion = Decimal.TryParse(textBox2.Text, out cantidad);
            if (esCorrectaConversion)
                return true;
            else
                return false;
        }


        private void AddFilaADataGridView_ProductosAPagar(string socio, string id_CuentaXPagar, string producto, string cantidad, DateTime fechaMovimiento)
        {
            int n = dataGridView2.Rows.Add();
            dataGridView2.Rows[n].Cells[0].Value = socio;
            dataGridView2.Rows[n].Cells[1].Value = id_CuentaXPagar;
            dataGridView2.Rows[n].Cells[2].Value = producto;
            dataGridView2.Rows[n].Cells[3].Value = cantidad;
            dataGridView2.Rows[n].Cells[4].Value = fechaMovimiento;

        }



        //------------------Eventos
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (ExisteCuentaPorPagarController(Int32.Parse(textBox1.Text)))
                {
                    textBox1.Enabled = false;
                    button1.Enabled = false;

                    int idCuentaBuscada = Int32.Parse(textBox1.Text);
                    DataTable respuesta = ProductosAPagar_BuscarDetallesProductoXIdCuentaController(idCuentaBuscada);
                    MostrarDetallesProductosAPagarEnCuenta(respuesta);
                    MostrarProductosQSePuedenPagar(respuesta);

                    DataTable otraRespuesta = Socio_Select_Id_NombreComercial_DeTodosController();
                    MostrarSocios_Id_NombreComercial(otraRespuesta);

                    textBox3.Text = textBox1.Text;
                    textBox3.Enabled = false;

                    CrearColumnasParaDataGridViewAportacionesPorIngresar();

                }

                else
                {
                    MessageBox.Show("No se encontro la cuenta con Id = " + textBox1.Text);
                }
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (SeCapturoInfoCorrecta())
            {
                AddFilaADataGridView_ProductosAPagar(comboBox1.SelectedItem.ToString(), textBox3.Text, comboBox2.SelectedItem.ToString(), textBox2.Text, dateTimePicker1.Value);
            }

            else
            {
                MessageBox.Show("Es incorrecta la captura");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            try
            {
                foreach(DataGridViewRow row in dataGridView2.Rows)
                {
                    string[] aux = row.Cells[0].EditedFormattedValue.ToString().Split('-');
                    int idSocio = Int32.Parse(aux[0]);

                    string[] aux2  = row.Cells[2].EditedFormattedValue.ToString().Split('-');
                    int idProducto = Int32.Parse(aux2[0]);

                    int idCuentaXPagar = Int32.Parse(  row.Cells[1].EditedFormattedValue.ToString()  );

                    int idUsuario = 2;

                    decimal cantidad = Decimal.Parse(row.Cells[3].EditedFormattedValue.ToString());
                    DateTime fechaMovimiento = DateTime.Parse(row.Cells[4].EditedFormattedValue.ToString()  );

                    MovimientoSocioProducto_createController(idSocio, idProducto, idCuentaXPagar, idUsuario, cantidad, fechaMovimiento);
                }

                textBox1.Clear();
                textBox1.Enabled = true;
                button1.Enabled = true;
                dataGridView1.DataSource = null;
                comboBox1.Items.Clear();     comboBox1.ResetText();
                textBox3.Clear();
                comboBox2.Items.Clear();    comboBox2.ResetText();
                textBox2.Clear();
                dataGridView2.Rows.Clear();
                dataGridView2.Columns.Clear();
                dataGridView2.DataSource = null;
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
