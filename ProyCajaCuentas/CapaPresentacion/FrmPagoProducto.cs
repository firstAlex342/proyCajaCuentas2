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
    public partial class FrmPagoProducto : Form
    {

        private bool EsposibleCambiarContenidoPanel2 { set; get; }

        //----------------Constructor
        public FrmPagoProducto()
        {

                InitializeComponent();
                EsposibleCambiarContenidoPanel2 = true;

                this.MinimumSize = this.Size;
            splitContainer1.Panel1MinSize = 60;

                CargarComBoBoxOpcionesFiltro();
                CargarComBoBoxOpcionesDeDescuento();

            try
            {
                DataTable resumenProductos = Producto_Select_Id_Nombre_DeTodosController();
                MostrarProducto_Id_Nombre_En_ComBoBox(resumenProductos);
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source);
            }
        }

        //------------------Methods
        private DataTable Socio_BuscarXRFCPropietarioController(string rfcPropietarioBuscado)
        {
            ClsSocio clsSocio = new ClsSocio();
            clsSocio.RFCPropietario = rfcPropietarioBuscado;

            return (clsSocio.Socio_BuscarXRFCPropietario());
        }

        private DataTable Socio_BuscarXComodatarioController(string comodatarioBuscado)
        {
            ClsSocio clsSocio = new ClsSocio();
            clsSocio.Comodatario = comodatarioBuscado;
            return (clsSocio.Socio_BuscarXComodatario());
        }


        private DataTable Socio_BuscarXRFCComodatarioController(string rfcComodatarioBuscado)
        {
            ClsSocio clsSocio = new ClsSocio();
            clsSocio.RFCComodatario = rfcComodatarioBuscado;

            return (clsSocio.Socio_BuscarXRFCComodatario());
        }

        private DataTable Socio_BuscarXPropietarioPatenteController(string propietarioPatenteBuscado)
        {
            ClsSocio clsSocio = new ClsSocio();
            clsSocio.PropietarioPatente = propietarioPatenteBuscado;

            return (clsSocio.Socio_BuscarXPropietarioPatente());
        }

        private DataTable Socio_BuscarXIdController(int idSocioBuscado)
        {
            ClsSocio clsSocio = new ClsSocio();
            clsSocio.Id = idSocioBuscado;

            return (clsSocio.Socio_BuscarXId());
        }

        private DataTable Producto_Select_Id_Nombre_DeTodosController()
        {
            ClsProducto clsProducto = new ClsProducto();
            return (clsProducto.Producto_Select_Id_Nombre_DeTodos());
        }

        private DataTable ProductoPosee_innerjoin_TarifasController(int idProductoBuscado)
        {
            //Busca las diferentes tarifas de un producto
            ClsProductoPosee clsProductoPosee = new ClsProductoPosee();
            clsProductoPosee.IdProducto = idProductoBuscado;
            return (clsProductoPosee.ProductoPosee_innerjoin_Tarifas());
        }

        private string PagoProducto_CreateController( int idSocio, int idProducto, decimal cantidadPagada, 
            string tipoDescuento, decimal cantidadDescuento, int idUsuarioOperador)
        {
            ClsPagoProducto clsPagoProducto = new ClsPagoProducto();
            clsPagoProducto.IdSocio = idSocio;
            clsPagoProducto.IdProducto = idProducto;
            clsPagoProducto.CantidadPagada = cantidadPagada;
            clsPagoProducto.TipoDescuento = tipoDescuento;
            clsPagoProducto.CantidadDescuento = cantidadDescuento;
            clsPagoProducto.IdUsuarioAlta = idUsuarioOperador;

            return (clsPagoProducto.PagoProducto_create());
        }

        //------------------Utils
        private void CargarComBoBoxOpcionesFiltro()
        {
            comboBox1.Items.Add("Direccion");
            comboBox1.Items.Add("Propietario patente");
            comboBox1.Items.Add("RFC propietario");
            comboBox1.Items.Add("Comodatario");
            comboBox1.Items.Add("RFC comodatario");
        }

        private void MostrarResultadoDeBusquedaXFiltro(DataTable tabla)
        {           
            dataGridView1.DataSource = tabla;
        }



        private void MostrarEnTxtBoxsSocio(DataTable tabla)
        {
            DataRow fila = tabla.Rows[0];
            textBox2.Text = fila["Id"].ToString();
            textBox3.Text = fila["NumeroLicencia"].ToString();
            textBox4.Text = fila["NombreComercial"].ToString();
            textBox5.Text = fila["PropietarioPatente"].ToString();
            textBox6.Text = fila["RFCPropietario"].ToString();
            textBox7.Text = fila["Comodatario"].ToString();
            textBox8.Text = fila["RFCComodatario"].ToString();
        }

        private void MostrarProducto_Id_Nombre_En_ComBoBox(DataTable resumenProductos)
        {
            var res = from item in resumenProductos.AsEnumerable()
                      select new ProductoCompact{ Id=item.Field<int>("Id"), Nombre = item.Field<string>("Nombre") };

            foreach(object item in res)         
                comboBox2.Items.Add(item);
            
        }


        private void MostrarTarifasDeProducto(DataTable tabla)
        {
            foreach(DataRow row in  tabla.Rows)           
                comboBox3.Items.Add(row["Cantidad"].ToString());
                   
        }

        private void CargarComBoBoxOpcionesDeDescuento()
        {
            comboBox4.Items.Add("%");
            comboBox4.Items.Add("Importe");
        }

        private void DeshabilitarComBoBoxesYTextBoxDeCalculo()
        {
            comboBox2.Enabled = false;
            comboBox3.Enabled = false;
            comboBox4.Enabled = false;
            textBox10.Enabled = false;
            //textBox9 siempre permanece deshabilitado
        }

        private void LimpiarContenidoControlesPanel2()
        {
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            comboBox2.Items.Clear(); comboBox2.ResetText();
            comboBox3.Items.Clear(); comboBox3.ResetText();
            comboBox4.Items.Clear(); comboBox4.ResetText();
            textBox10.Clear();
            textBox9.Clear();
        }

        private void HabilitarComBoBoxesYTextBoxDeCalculo()
        {
            comboBox2.Enabled = true;
            comboBox3.Enabled = true;
            comboBox4.Enabled = true;
            textBox10.Enabled = true;
            //textbox9 no se habilita
        }

        //-----------------------Events
        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.DataSource = null;

                if (comboBox1.SelectedIndex == 1)
                {
                    DataTable respuesta = Socio_BuscarXPropietarioPatenteController(textBox1.Text);
                    MostrarResultadoDeBusquedaXFiltro(respuesta);
                }

                if (comboBox1.SelectedIndex == 2)
                {
                    DataTable respuesta = Socio_BuscarXRFCPropietarioController(textBox1.Text);
                    MostrarResultadoDeBusquedaXFiltro(respuesta);
                }

                if (comboBox1.SelectedIndex == 3)
                {
                    DataTable respuesta = Socio_BuscarXComodatarioController(textBox1.Text);
                    MostrarResultadoDeBusquedaXFiltro(respuesta);
                }

                if (comboBox1.SelectedIndex == 4)
                {
                    DataTable respuesta = Socio_BuscarXRFCComodatarioController(textBox1.Text);
                    MostrarResultadoDeBusquedaXFiltro(respuesta);
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.Source);
            }
        }




        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    if(EsposibleCambiarContenidoPanel2)
                    {
                        string idSocioEnTexto = (dataGridView1.Rows[e.RowIndex].Cells[0].EditedFormattedValue).ToString();
                        DataTable miTabla = Socio_BuscarXIdController(Int32.Parse(idSocioEnTexto));
                        MostrarEnTxtBoxsSocio(miTabla);
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + ex.Source);
                }
            }
        }

        private void dataGridView2_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox2.SelectedIndex >= 0)
                {
                    comboBox3.Items.Clear();
                    comboBox3.ResetText();
                    ProductoCompact elementoSeleccionado = (ProductoCompact)comboBox2.SelectedItem;
                    DataTable respuesta = ProductoPosee_innerjoin_TarifasController(elementoSeleccionado.Id);
                    MostrarTarifasDeProducto(respuesta);
                }

            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                bool estaElegidoSocio = textBox2.Text.Length > 0 ? true : false;
                bool estaElegidoProducto = comboBox2.SelectedIndex >=0 ? true : false;
                bool estaElegidoTarifa = comboBox3.SelectedIndex >= 0 ? true : false;
                bool estaElegidoTipoDescuento = comboBox4.SelectedIndex >= 0 ? true : false;
                decimal numero = 0.0M;
                bool esUnNumeroCantidadADescontar = Decimal.TryParse(textBox10.Text, out numero);
                
                if( (estaElegidoSocio == true) && (estaElegidoProducto == true)
                    && (estaElegidoTarifa== true) && (estaElegidoTipoDescuento == false) )
                {
                    textBox9.Text = comboBox3.SelectedItem.ToString();
                    DeshabilitarComBoBoxesYTextBoxDeCalculo();
                    EsposibleCambiarContenidoPanel2 = false;
                }

                else
                {
                    if ((estaElegidoSocio == true) && (estaElegidoProducto == true) && (estaElegidoTarifa == true) && (estaElegidoTipoDescuento == true)
                        && (esUnNumeroCantidadADescontar == true))
                    {
                        if (comboBox4.SelectedItem.ToString().Contains("Importe"))
                        {
                            decimal total = decimal.Parse(comboBox3.SelectedItem.ToString()) - decimal.Parse(textBox10.Text);
                            textBox9.Text = total.ToString();

                            DeshabilitarComBoBoxesYTextBoxDeCalculo();
                            EsposibleCambiarContenidoPanel2 = false;
                        }

                        if(comboBox4.SelectedItem.ToString().Contains("%"))
                        {
                            decimal tarifaElegida = decimal.Parse(comboBox3.SelectedItem.ToString());
                            decimal aux = (decimal.Parse(textBox10.Text) / 100.0M) * tarifaElegida;
                            decimal totalAPagar = tarifaElegida - aux;
                            textBox9.Text = totalAPagar.ToString();

                            DeshabilitarComBoBoxesYTextBoxDeCalculo();
                            EsposibleCambiarContenidoPanel2 = false;
                        }
                    }
                }
            }

            else
            {
                EsposibleCambiarContenidoPanel2 = true;
                LimpiarContenidoControlesPanel2();
                HabilitarComBoBoxesYTextBoxDeCalculo();
                CargarComBoBoxOpcionesDeDescuento();
                try
                {
                    DataTable resumenProductos = Producto_Select_Id_Nombre_DeTodosController();
                    MostrarProducto_Id_Nombre_En_ComBoBox(resumenProductos);
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " " + ex.Source);
                }
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if(EsposibleCambiarContenidoPanel2 == false)
                {

                    bool estaElegidoSocio = textBox2.Text.Length > 0 ? true : false;
                    bool estaElegidoProducto = comboBox2.SelectedIndex >= 0 ? true : false;
                    bool estaElegidoTarifa = comboBox3.SelectedIndex >= 0 ? true : false;
                    bool estaElegidoTipoDescuento = comboBox4.SelectedIndex >= 0 ? true : false;
                    decimal numero = 0.0M;
                    bool esUnNumeroCantidadADescontar = Decimal.TryParse(textBox10.Text, out numero);

                    if ((estaElegidoSocio == true) && (estaElegidoProducto == true)
                        && (estaElegidoTarifa == true) && (estaElegidoTipoDescuento == false))
                    {

                        int idSocio = Int32.Parse(textBox2.Text);
                        int idProducto = ((ProductoCompact)comboBox2.SelectedItem).Id;
                        decimal tarifa = Decimal.Parse(comboBox3.SelectedItem.ToString());
                        string tipoDescuento = String.Empty;
                        decimal descuento = 0.0M;
                        decimal cantidadAPagar = Decimal.Parse(textBox9.Text);

                        string respuesta = PagoProducto_CreateController(idSocio, idProducto, cantidadAPagar, tipoDescuento, descuento, ClsUsuario.Id);
                        MessageBox.Show(respuesta, "Salida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        checkBox1.Checked = false;  // Se dispara el evento checkBox1_CheckedChanged
                    }

                    else if(  (estaElegidoSocio == true) && (estaElegidoProducto == true) && (estaElegidoTarifa == true) && (estaElegidoTipoDescuento == true)
                        && (esUnNumeroCantidadADescontar == true)   )
                    {
                        int idSocio = Int32.Parse(textBox2.Text);
                        int idProducto = ((ProductoCompact)comboBox2.SelectedItem).Id;
                        decimal tarifa = Decimal.Parse(comboBox3.SelectedItem.ToString());
                        string tipoDescuento = comboBox4.SelectedItem.ToString();
                        decimal descuento = Decimal.Parse(textBox10.Text);
                        decimal cantidadAPagar = Decimal.Parse(textBox9.Text);

                        string respuesta = PagoProducto_CreateController(idSocio, idProducto, cantidadAPagar, tipoDescuento, descuento, ClsUsuario.Id);
                        MessageBox.Show(respuesta, "Salida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        checkBox1.Checked = false;  // Se dispara el evento checkBox1_CheckedChanged
                    }

                }
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
            //Se dispara de forma implicita el evento checkBox1_CheckedChanged, despues de la linea anterior
        }
    }


    class ProductoCompact
    {
        public int Id { set; get; }
        public string Nombre { set; get; }

        //---------constructor
        public ProductoCompact()
        {
            this.Id = 0;
            this.Nombre = string.Empty;
        }

        public override string ToString()
        {
            return "Id=" + this.Id.ToString() + " Nombre= " + this.Nombre;
        }
    }
}
