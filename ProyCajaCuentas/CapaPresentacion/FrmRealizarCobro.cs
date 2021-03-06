﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaLogicaNegocios;
using System.Threading;

namespace CapaPresentacion
{
    public partial class FrmRealizarCobro : Form
    {

        //-----------Properties
        private DataTable InfoCajaDelDia { set; get; }
        private DataTable TarifasDeTodosProductos { set; get; }


        //--------------Constructor
        public FrmRealizarCobro()
        {
            InitializeComponent();
            CrearColumnasParaDataGridViewProductosAPagar();

            try
            {
                InicializarInfoCajaDelDiaController();
                MostrarProductos( Producto_Select_Id_Nombre_DeTodosController());
                this.TarifasDeTodosProductos = ProductoPosee_innerjoin_Tarifas_DeTodosController();       
            }

            catch(Exception ex)
            {
                tableLayoutPanel1.Enabled = false;
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }

        //--------------Methods
        private DataTable OperaCaja_BuscarCajasDelDiaDelUsuarioController(int idUsuarioOperador)
        {
            ClsOperaCaja clsOperaCaja = new ClsOperaCaja();
            clsOperaCaja.IdUsuario = ClsLogin.Id;

            return (clsOperaCaja.OperaCaja_BuscarCajasDelDiaDelUsuario());
        }

      

        private int Caja_CreateController(int idUsuarioOperador)
        {
            ClsCaja clsCaja = new ClsCaja();
            clsCaja.IdUsuarioAlta = idUsuarioOperador;

            DataTable res = clsCaja.Caja_create();  
            var coleccion = from s in res.AsEnumerable()
                            select s;

            DataRow unicaFila = coleccion.First();
            int idCajaNueva = unicaFila.Field<int>(0);
            return (idCajaNueva);
        }


        private string OperaCaja_CreateController(int idUsuarioOperador, int idCaja)
        {
            ClsOperaCaja clsOperaCaja = new ClsOperaCaja();
            clsOperaCaja.IdUsuario = idUsuarioOperador;
            clsOperaCaja.IdCaja = idCaja;
            clsOperaCaja.IdUsuarioAlta = idUsuarioOperador;

            return (clsOperaCaja.OperaCaja_create());
        }

        private DataTable Producto_Select_Id_Nombre_DeTodosController()
        {
            ClsProducto clsProducto = new ClsProducto();
            return (clsProducto.Producto_Select_Id_Nombre_DeTodos());
        }



        private void InicializarInfoCajaDelDiaController()
        {
            DataTable res = OperaCaja_BuscarCajasDelDiaDelUsuarioController(ClsLogin.Id);

            if (res.Rows.Count == 1)
            {
                //En res esta la caja del dia 
                this.InfoCajaDelDia = res;
            }

            else if (res.Rows.Count > 1)
            {
                string mensajeError;
                mensajeError = "Un usuario no puede tener 2 cajas con la misma fecha de creación en la tabla OperaCaja, es decir en un dia solo puede tener una caja";
                throw new ArgumentException(mensajeError);
            }

            else
            {
                //El dia de hoy aún no tiene caja aún por lo tanto se le crea
                int idCajaNueva = Caja_CreateController(ClsLogin.Id);
                OperaCaja_CreateController(ClsLogin.Id, idCajaNueva);

                this.InfoCajaDelDia = OperaCaja_BuscarCajasDelDiaDelUsuarioController(ClsLogin.Id);
            }
        }

        
        private DataTable ProductoPosee_innerjoin_Tarifas_DeTodosController()
        {
            ClsProductoPosee clsProductoPosee = new ClsProductoPosee();
            return (clsProductoPosee.ProductoPosee_innerjoin_Tarifas_DeTodos());
        }


        private string GuardarPagoBasicoDelSocioController(int idCaja,int idSocio, 
            decimal TotalPagado, int idUsuarioOperador, DataGridViewRowCollection filas, 
            string folioReciboListaPoductos, string folioReciboLicencia )
        {

            ClsPagoBasico clsPagoBasico = new ClsPagoBasico();
            clsPagoBasico.IdCaja = idCaja;
            clsPagoBasico.IdSocio = idSocio;
            clsPagoBasico.TotalPagado = TotalPagado;
            clsPagoBasico.FolioReciboListaPoductos = folioReciboListaPoductos;
            clsPagoBasico.FolioReciboLicencia = folioReciboLicencia;
            clsPagoBasico.IdUsuarioOperador = idUsuarioOperador;
            
           
            foreach(DataGridViewRow fila in filas)
            {
                ClsProductoViewModel clsProductoViewModel = (ClsProductoViewModel)fila.Cells[1].Value;
                ClsTarifaViewModel clsTarifaViewModel = (ClsTarifaViewModel)fila.Cells[2].Value;
                decimal descuento = Decimal.Parse(fila.Cells[3].EditedFormattedValue.ToString());
                decimal cantidadAPagar = Decimal.Parse(fila.Cells[4].EditedFormattedValue.ToString());

                clsPagoBasico.AddProductoAPagar(clsProductoViewModel.Id, clsTarifaViewModel.Id, clsTarifaViewModel.Cantidad , 
                    "campo no usado", descuento, cantidadAPagar);               
            }

            string respuesta = clsPagoBasico.MovsEnCaja_PagoProducto_DetallesProductosEnPago_create();
            return (respuesta);
        }

        private DataTable Socio_BuscarXLicenciaController(string licenciaBuscada)
        {
            ClsSocio clsSocio = new ClsSocio();
            clsSocio.NumeroLicencia = licenciaBuscada;
            return (clsSocio.Socio_BuscarXLicencia());
        }

        private bool Socio_BuscarAfiliacionActivaController( string numeroLicencia)
        {
            ClsSocio clsSocio = new ClsSocio();
            clsSocio.NumeroLicencia = numeroLicencia;

            DataTable tabla = clsSocio.Socio_BuscarAfiliacionActiva();
            bool res = (tabla.Rows.Count == 1) ? true : false;
            return (res);
        }


        private bool ExisteFolioReciboListaProductosController(string folioBuscado)
        {
            ClsReciboListaProductos clsReciboListaProductos = new ClsReciboListaProductos();
            clsReciboListaProductos.Folio = folioBuscado;
            DataTable resulBusqueda = clsReciboListaProductos.ReciboListaProductos_BuscarFolio();

            bool res = resulBusqueda.Rows.Count >= 1 ? true : false;
            return (res);
        }

        private bool ExisteFolioReciboLicenciaController(string folioBuscado)
        {
            ClsReciboLicencia clsReciboLicencia = new ClsReciboLicencia();
            clsReciboLicencia.Folio = folioBuscado;
            DataTable resulBusqueda = clsReciboLicencia.ReciboLicencia_BuscarFolio();

            bool res = resulBusqueda.Rows.Count >= 1 ? true : false;
            return (res);
        }

        public DataTable Socio_BuscarXIdController(int idSocioABuscar)
        {
            ClsSocio clsSocio = new ClsSocio();
            clsSocio.Id = idSocioABuscar;

            return (clsSocio.Socio_BuscarXId());
        }


        //------------Utils
        private void MostrarEnTxtBoxesInfoSocio(DataTable infoSocio)
        {
            var coleccion = from s in infoSocio.AsEnumerable()
                            select s;

            //En este punto ya se que solo tiene una fila la DataTable
            DataRow unicaFila = coleccion.First();
            textBox19.Text = (unicaFila.Field<int>("Id")).ToString();
            textBox4.Text = unicaFila.Field<string>("NombreComercial");
            textBox5.Text = unicaFila.Field<string>("DireccionSupmza");
            textBox6.Text = unicaFila.Field<string>("DireccionLote");
            textBox7.Text = unicaFila.Field<string>("DireccionManzana");
            textBox8.Text = unicaFila.Field<string>("DireccionCalle");
            textBox9.Text = unicaFila.Field<string>("DireccionComplemento");
            textBox10.Text = unicaFila.Field<string>("PropietarioPatente");
            textBox11.Text = unicaFila.Field<string>("RFCPropietario");
            textBox12.Text = unicaFila.Field<string>("Comodatario");
            textBox13.Text = unicaFila.Field<string>("RFCComodatario");
            textBox14.Text = unicaFila.Field<string>("Telefono");
            textBox15.Text = unicaFila.Field<string>("Celular");
            textBox16.Text = unicaFila.Field<string>("CorreoElectronico");
        }

        private void MostrarProductos(DataTable misProductos)
        {
            List<ClsProductoViewModel> lista = new List<ClsProductoViewModel>();

            var coleccion = from s in misProductos.AsEnumerable()
                            select new ClsProductoViewModel { Id= s.Field<int>("Id"),
                                Nombre =  s.Field<string>("Nombre"), CampoAMostrar = "Nombre" }; 

            lista = coleccion.ToList<ClsProductoViewModel>();

            foreach(var item in lista)
                listBox1.Items.Add(item);
           
        }

        private void CrearColumnasParaDataGridViewProductosAPagar()
        {
            DataGridViewButtonColumn columnaBotonesEliminar = new DataGridViewButtonColumn();
            columnaBotonesEliminar.Name = "columnaBotonesEliminar";
            columnaBotonesEliminar.HeaderText = "";
            columnaBotonesEliminar.Text = "X";
            columnaBotonesEliminar.UseColumnTextForButtonValue = true;

            dataGridView1.Columns.Add(columnaBotonesEliminar);
            dataGridView1.Columns.Add("Producto", "Producto");
            dataGridView1.Columns.Add("Tarifa", "Tarifa");
            dataGridView1.Columns.Add("Descuento", "Descuento");
            dataGridView1.Columns.Add("CantidadAPagar", "Cantidad a pagar");

            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void AddFilaADataGridView_ProductosAPagar(ClsProductoViewModel clsProductoViewModel, ClsTarifaViewModel clsTarifaViewModel, decimal descuento,  decimal cantidadAPagar)
        {
            int n = dataGridView1.Rows.Add();
            dataGridView1.Rows[n].Cells[1].Value = clsProductoViewModel;
            dataGridView1.Rows[n].Cells[2].Value = clsTarifaViewModel;
            dataGridView1.Rows[n].Cells[3].Value = descuento;
            dataGridView1.Rows[n].Cells[4].Value = cantidadAPagar;
        }

        private decimal CalcularSumaTotalEnDataGridView()
        {
              decimal suma = 0.0m;
              foreach(DataGridViewRow row in dataGridView1.Rows)
              {
                DataGridViewCell celda = row.Cells[4];
                suma += Decimal.Parse(celda.EditedFormattedValue.ToString());
              }

            return (suma);
        }


        private bool ExisteProductoEnDataGridView(ClsProductoViewModel clsProductoViewModelAInsertar)
        {
            bool bandera = false;

            foreach (DataGridViewRow fila in dataGridView1.Rows)
            {
                ClsProductoViewModel clsProductoViewModelInsertado = (ClsProductoViewModel)fila.Cells[1].Value;
                if(clsProductoViewModelInsertado.Id == clsProductoViewModelAInsertar.Id)
                {
                    bandera = true;
                    break;
                }              
            }

            return (bandera);
        }

        private void LimpiarTextBoxesInfoSocio()
        {
            textBox4.Text = "";
            textBox5.Text = ""; 
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";
            textBox12.Text = "";
            textBox13.Text = "";
            textBox14.Text = "";
            textBox15.Text = "";
            textBox16.Text = "";
            textBox19.Text = "";
        }

        private void EnviarImpresion(DataRow filaUnica, DataGridViewRowCollection filas, string totalAPagar,DataTable infoUsuarioTable,
            string folioReciboListaProductos, string folioReciboLicencia)
        {
            FrmVisorReporte frmVisorReporte = new FrmVisorReporte(filaUnica, filas, totalAPagar, infoUsuarioTable, folioReciboListaProductos, folioReciboLicencia);
            frmVisorReporte.ShowDialog(this);
            frmVisorReporte.Dispose();
        }


        private bool EsAfiliacion(ClsProductoViewModel clsProductoViewModel)
        {
            bool res = clsProductoViewModel.Nombre == "Afiliación" ? true: false;
            bool res2 = clsProductoViewModel.Nombre == "afiliación" ? true : false;
            bool res3 = clsProductoViewModel.Nombre == "Afiliacion" ? true : false; 
            bool res4 = clsProductoViewModel.Nombre == "afiliacion" ? true : false;
            bool res5 = clsProductoViewModel.Nombre == "AFILIACIÓN" ? true : false;
            bool res6 = clsProductoViewModel.Nombre == "AFILIACION" ? true : false;

            if (res || res2 || res3 || res4 || res5 || res6)
                return true;
            else
                return false;           
        }

        private bool ExisteAfiliacionEnDataGrid()
        {
            bool bandera = false;

            foreach (DataGridViewRow fila in dataGridView1.Rows)
            {
                ClsProductoViewModel clsProductoViewModelInsertado = (ClsProductoViewModel)fila.Cells[1].Value;
                if (EsAfiliacion(clsProductoViewModelInsertado))
                {
                    bandera = true;
                    break;
                }
            }

            return (bandera);
        }

        private bool EstaEnFormatoNumerico(string texto)
        {
            int numero;
            bool respuesta;

            respuesta = Int32.TryParse(texto, out numero);

            return (respuesta);
        }


        public void LimpiarYReinicializarListBox1ListBox2TarifasTodosProductosTxtBox2TxtBox3()
        {
            listBox1.SelectedIndex = -1;
            listBox1.Items.Clear();

            listBox2.SelectedIndex = -1;
            listBox2.Items.Clear();

            textBox2.Text = String.Empty;
            textBox3.Text = String.Empty;

            try
            {
                MostrarProductos(Producto_Select_Id_Nombre_DeTodosController());
                this.TarifasDeTodosProductos = ProductoPosee_innerjoin_Tarifas_DeTodosController();
            }

            catch (Exception ex)
            {
                tableLayoutPanel1.Enabled = false;
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }




        //--------------------Events
        private void button1_Click(object sender, EventArgs e)
        {
            //En este punto los calculos del descuento ya se supone estan correctos, por eso no se vuelven a calcular
            //al momento de agregar al datagridView
            try
            {
                bool listBox1Seleccionado = listBox1.SelectedIndex > -1 ? true : false; //Averiguar si esta seleccionado
                bool listBox2Seleccionado = listBox2.SelectedIndex > -1 ? true : false; //Averiguar si esta seleccionado
                bool textBox3ConContenido = textBox3.Text.Length > 0 ? true : false; // El textBox3 ya se que solo numeros contiene, por ello solo compruebo si tiene algo
                bool textBox2Vacio = textBox2.Text.Length == 0 ? true : false; //Averiguar si esta vacío 

                if (listBox1Seleccionado && listBox2Seleccionado && textBox3ConContenido && textBox2Vacio)
                {
                    ClsProductoViewModel clsProductoViewModel = (ClsProductoViewModel)listBox1.SelectedItem;
                    ClsTarifaViewModel clsTarifaViewModel = (ClsTarifaViewModel)listBox2.SelectedItem;
                    decimal cantidadAPagar = Decimal.Parse(textBox3.Text);

                    if (ExisteProductoEnDataGridView (clsProductoViewModel) == false)
                    {
                        AddFilaADataGridView_ProductosAPagar(clsProductoViewModel, clsTarifaViewModel, 0.0m, cantidadAPagar);
                        label7.Text = CalcularSumaTotalEnDataGridView().ToString();

                        if(EsAfiliacion(clsProductoViewModel))
                        { label22.Enabled = true;
                          textBox18.Enabled = true; 
                        }
                    }

                    else
                    {
                        MessageBox.Show("Ya capturaste este producto", "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                else if (listBox1Seleccionado && listBox2Seleccionado && textBox3ConContenido && (textBox2Vacio == false))
                {
                    ClsProductoViewModel clsProductoViewModel = (ClsProductoViewModel)listBox1.SelectedItem;
                    ClsTarifaViewModel clsTarifaViewModel = (ClsTarifaViewModel)listBox2.SelectedItem;
                    decimal descuento = Decimal.Parse(textBox2.Text);
                    decimal cantidadAPagar = Decimal.Parse(textBox3.Text);

                    if(ExisteProductoEnDataGridView(clsProductoViewModel) == false)
                    {
                        AddFilaADataGridView_ProductosAPagar(clsProductoViewModel, clsTarifaViewModel, descuento, cantidadAPagar);
                        label7.Text = CalcularSumaTotalEnDataGridView().ToString();
                        if (EsAfiliacion(clsProductoViewModel))
                        {
                            label22.Enabled = true;
                            textBox18.Enabled = true;
                        }
                    }

                    else
                    {
                        MessageBox.Show("Ya capturaste este producto", "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                else
                {
                    string texto = "Recuerda que se requiere seleccionar un producto, una tarifa, e ingresar opcionalmente un descuento";
                    MessageBox.Show(texto, "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                listBox2.SelectedItem = null;
                listBox2.Items.Clear();
                textBox3.Text = "";
                textBox2.Text = "";

                ClsProductoViewModel clsProductoViewModel = (ClsProductoViewModel)listBox1.SelectedItem;
                List<ClsTarifaViewModel> listaTarifas = new List<ClsTarifaViewModel>();

                var res = from s in TarifasDeTodosProductos.AsEnumerable()
                          where s.Field<int>("IdProducto") == clsProductoViewModel.Id
                          select new ClsTarifaViewModel { Id = s.Field<int>("IdTarifa"), Cantidad = s.Field<decimal>("Cantidad"), CampoAMostrar = "Cantidad" };

                listaTarifas = res.ToList<ClsTarifaViewModel>();
                Action<ClsTarifaViewModel> Funcion = s =>
                {
                    listBox2.Items.Add(s);
                };

                listaTarifas.ForEach(Funcion);
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox2.SelectedIndex > -1)
            {
                ClsTarifaViewModel clsTarifaElegidaViewModel = (ClsTarifaViewModel)listBox2.SelectedItem;
                decimal tarifaElegida = (decimal)clsTarifaElegidaViewModel.Cantidad;
                textBox3.Text = tarifaElegida.ToString();
                textBox2.Text = "";
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            bool listBox1Seleccionado = listBox1.SelectedIndex > -1 ? true : false; //Averiguar si esta seleccionado
            bool listBox2Seleccionado = listBox2.SelectedIndex > -1 ? true : false; //Averiguar si esta seleccionado
            bool textBox3ConContenido = textBox3.Text.Length > 0 ? true : false; // Averiguar si tiene contenido

            if(listBox1Seleccionado && listBox2Seleccionado && textBox3ConContenido)
            {
                if (textBox2.Text.Length > 0)
                {
                    decimal descuento = 0.0m;
                    bool esNumero = Decimal.TryParse(textBox2.Text, out descuento);

                    if(esNumero)
                    {
                        ClsTarifaViewModel clsTarifaViewModelSeleccionado = (ClsTarifaViewModel)listBox2.SelectedItem;
                        decimal tarifaElegida = (decimal)clsTarifaViewModelSeleccionado.Cantidad;

                        if (descuento >= 0m && descuento <= tarifaElegida)
                        {
                            decimal resta = tarifaElegida - descuento;
                            textBox3.Text = resta.ToString();
                        }
                        else
                        {
                            textBox2.Text = "";  //Despues de esta linea se vuelve a disparar el evento textBox2_TextChanged
                            MessageBox.Show("El descuento debe ser menor ó igual a la tarifa seleccionada", "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }

                    else
                    {
                        textBox2.Text = ""; //Despues de esta linea se vuelve a disparar el evento textBox2_TextChanged
                        MessageBox.Show("Entrada no numerica", "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }

                else
                {
                    ClsTarifaViewModel clsTarifaViewModelSeleccionado = (ClsTarifaViewModel)listBox2.SelectedItem;
                    textBox3.Text = (clsTarifaViewModelSeleccionado.Cantidad).ToString();
                }
            }

        }





        private void textBox1_Leave(object sender, EventArgs e)
        {
            try
            {
                LimpiarTextBoxesInfoSocio();

                if (textBox1.Text.Length > 0)
                {
                    DataTable res = Socio_BuscarXLicenciaController(textBox1.Text);
                    if (res.Rows.Count == 1)
                        MostrarEnTxtBoxesInfoSocio(res);

                    else
                    {
                        string mensaje = "Se encontro " + res.Rows.Count.ToString() + " coincidencias para la licencia " + textBox1.Text;
                        MessageBox.Show(mensaje, "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source);
            }
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                //Deshabilitar el label y el textBox de captura de Folio licencia, si la fila contíene el producto Afiliacion
                DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];
                DataGridViewCell celda = fila.Cells[1];
                ClsProductoViewModel clsProductoViewModel = (ClsProductoViewModel)celda.Value;
                if( EsAfiliacion(clsProductoViewModel) )
                {
                    label22.Enabled = false;
                    textBox18.Enabled = false;
                    textBox18.Text = "";
                }


                //Borrar la fila
                dataGridView1.Rows.RemoveAt(e.RowIndex);
                label7.Text = CalcularSumaTotalEnDataGridView().ToString();
            }
        }





        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if(dataGridView1.Rows.Count >= 1)
                {
                    DialogResult res = MessageBox.Show("¿Esta usted seguro que desea continuar?", "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if(res ==DialogResult.Yes)
                    {
                            bool existeIdSocioEnTextBox = EstaEnFormatoNumerico(textBox19.Text);
                            if (existeIdSocioEnTextBox)
                            {
                                DataTable infoDeSocio = Socio_BuscarXIdController(Int32.Parse(textBox19.Text));
                                if(infoDeSocio.Rows.Count == 1)
                                {
                                        var x = (from s in infoDeSocio.AsEnumerable()
                                                 select s).ToList();

                                        DataRow filaUnica = x.First(); //Recupero la información del Socio

                                    if (EstaEnFormatoNumerico(textBox17.Text.Trim()))
                                    {
                                        bool esOkReciboLicencia = (textBox18.Enabled == true) ? EstaEnFormatoNumerico(textBox18.Text.Trim()) : true;
                                        if (esOkReciboLicencia)
                                        {
                                            var m = (from item in InfoCajaDelDia.AsEnumerable()
                                                     select item).ToList();
                                            var m1 = m.First(); //Recupero la fila

                                            string respuesta = GuardarPagoBasicoDelSocioController(m1.Field<int>("IdCaja"), Int32.Parse(textBox19.Text), Decimal.Parse(label7.Text), ClsLogin.Id, dataGridView1.Rows, textBox17.Text.Trim(), textBox18.Text.Trim());
                                            if (respuesta.Contains("ok"))
                                            {
                                                MessageBox.Show("Registros guardados exitosamente", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                                //InfoCajaDelDia lo utiliza EnviarImpresion, porque de ahi sala la fecha que ira en la impresion                                          
                                                EnviarImpresion(filaUnica, dataGridView1.Rows, label7.Text, InfoCajaDelDia, textBox17.Text.Trim(), textBox18.Text.Trim());

                                                dataGridView1.Rows.Clear();
                                                label7.Text = "0.0";
                                                textBox1.Text = "";
                                                listBox1.SelectedIndex = -1;
                                                listBox2.SelectedIndex = -1; listBox2.Items.Clear();
                                                textBox2.Text = "";
                                                textBox3.Text = "";
                                                LimpiarTextBoxesInfoSocio();

                                                textBox17.Text = "";  //Son los textBox y el label donde se capturan folios
                                                textBox18.Text = "";
                                                label22.Enabled = false;
                                                textBox18.Enabled = false;
                                            }

                                            else
                                            {
                                                MessageBox.Show(respuesta, "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                            }
                                        }

                                        else
                                        {
                                            MessageBox.Show("Capture el folio licencia, por ejemplo: 1343 ó 0345", "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Capture el folio del recibo de pago, por ejemplo: 02345 ó 04567", "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    }
                                }

                                else
                                {
                                    MessageBox.Show("No se encuentra disponible el socio", "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                }

                            }

                            else
                            {
                                MessageBox.Show("Capture un número de licencia", "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                    }
                }

                else
                {
                    MessageBox.Show("Necesitas capturar algún producto", "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            DialogResult res = MessageBox.Show("¿Esta usted seguro que desea cancelar la edición actual?", "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if(res == DialogResult.Yes)
            {
                dataGridView1.Rows.Clear();
                label7.Text = "0.0";
                textBox1.Text = "";
                listBox1.SelectedIndex = -1;
                listBox2.SelectedIndex = -1; listBox2.Items.Clear();
                textBox2.Text = "";
                textBox3.Text = "";
                LimpiarTextBoxesInfoSocio();

                textBox17.Text = "";  //Son los textBox y el label donde se capturan folios
                textBox18.Text = "";
                label22.Enabled = false;
                textBox18.Enabled = false;
            }
        }
    }

    public class ClsProductoViewModel
    {
        //-----------Properties
        public int Id { set; get; }
        public string Nombre { set; get; }
        public string Descripcion { set; get; }
        public int IdUsuarioAlta { set; get; }
        public DateTime FechaAlta { set; get; }
        public int IdUsuarioModifico { set; get; }
        public DateTime FechaModificacion { set; get; }
        public bool Activo { set; get; }

        public string CampoAMostrar { set; get; }

        //---------------Constructor
        public ClsProductoViewModel()
        {
            this.Id = 0;
            this.Nombre = String.Empty;
            this.Descripcion = String.Empty;
            this.IdUsuarioAlta = 0;
            this.FechaAlta = new DateTime();
            this.IdUsuarioModifico = 0;
            this.FechaModificacion = new DateTime();
            this.Activo = true;

            this.CampoAMostrar = String.Empty;
        }

        //----------------Methods
        public override string ToString()
        {
            if (this.CampoAMostrar.Equals("Nombre"))
                return (this.Nombre);

            else
            {
                string texto;
                texto = " Id = " + this.Id.ToString();
                texto += " Nombre = " + this.Nombre;
                texto += " Descripcion = " + this.Descripcion;
                texto += " IdUsuarioAlta = " + this.IdUsuarioAlta;
                texto += " FechaAlta = " + this.FechaAlta.ToString();
                texto += " IdUsuarioModifico = " + this.IdUsuarioModifico.ToString();
                texto += " FechaModificacion = " + this.FechaModificacion.ToString();
                texto += " Activo = " + this.Activo.ToString();
                return (texto);
            }
        }
    }


    public class ClsTarifaViewModel
    {
        public int Id { set; get; }
        public decimal Cantidad { set; get; }
        public int IdUsuarioAlta { set; get; }
        public DateTime FechaAlta { set; get; }
        public int IdUsuarioModifico { set; get; }
        public DateTime FechaModificacion { set; get; }
        public bool Activo { set; get; }

        public string CampoAMostrar { set; get; }

        //--------------Constructor
        public ClsTarifaViewModel()
        {
            this.Id = 0;
            this.Cantidad = 0.0M;
            this.IdUsuarioAlta = 0;
            this.FechaAlta = new DateTime();
            this.IdUsuarioModifico = 0;
            this.FechaModificacion = new DateTime();
            this.Activo = true;

            this.CampoAMostrar = String.Empty;
        }


        public override string ToString()
        {
            if( this.CampoAMostrar.Equals("Cantidad") )
            {
                return (this.Cantidad.ToString());
            }

            else
            {
                StringBuilder str = new StringBuilder();
                str.Append("Id = " + this.Id.ToString() + " ");
                str.Append("Cantidad = " + this.Cantidad.ToString()+ " ");
                str.Append("IdUsuarioAlta = " + this.IdUsuarioAlta.ToString() + " ");
                str.Append("FechaAlta = " + this.FechaAlta.ToString() + " ");
                str.Append("IdUsuarioModifico = " + this.IdUsuarioModifico.ToString() + " ");
                str.Append("FechaModificacion = " + this.FechaModificacion.ToString() + " ");
                str.Append("Activo = " + this.Activo.ToString() + " ");

                return (str.ToString());
            }
        }
    }
}
