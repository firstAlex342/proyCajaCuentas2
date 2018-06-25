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
    public partial class FrmSocio : Form
    {

        //-----------Constructor
        public FrmSocio()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            CargarComBoBoxOpcionesFiltro();
        }



        //------------Methods
        private string Socio_createController(string numeroLicencia, string nombreComercial, string direccionSupmza, 
            string direccionManzana, string direccionLote, string direccionCalle, string direccionComplemento,
            string propietarioPatente, string rFCPropietario, string comodatario, string rFCComodatario,
            string telefono, string celular, string correoElectronico, int idUsuarioOperador
            )
        {
            ClsSocio clsSocio = new ClsSocio();
            clsSocio.NumeroLicencia = numeroLicencia;
            clsSocio.NombreComercial = nombreComercial;
            clsSocio.DireccionSupmza = direccionSupmza;
            clsSocio.DireccionManzana = direccionManzana;
            clsSocio.DireccionLote = direccionLote;
            clsSocio.DireccionCalle = direccionCalle;
            clsSocio.DireccionComplemento = direccionComplemento;
            clsSocio.PropietarioPatente = propietarioPatente;
            clsSocio.RFCPropietario = rFCPropietario;
            clsSocio.Comodatario = comodatario;
            clsSocio.RFCComodatario = rFCComodatario;
            clsSocio.Telefono = telefono;
            clsSocio.Celular = celular;
            clsSocio.CorreoElectronico = correoElectronico;
            clsSocio.IdUsuarioAlta = idUsuarioOperador;

            return(clsSocio.Socio_create());
        }

        private DataTable Socio_BuscarXRFCPropietarioController(string rfcPropietarioBuscado)
        {
            ClsSocio clsSocio = new ClsSocio();
            clsSocio.RFCPropietario = rfcPropietarioBuscado;

            return (clsSocio.Socio_BuscarXRFCPropietario());
        }

        private DataTable Socio_BuscarXIdController(int idSocioBuscado)
        {
            ClsSocio clsSocio = new ClsSocio();
            clsSocio.Id = idSocioBuscado;

            return (clsSocio.Socio_BuscarXId());
        }

        private string Socio_updateController(int idSocioBuscado, string numeroLicencia, string nombreComercial, string direccionSupmza,
            string direccionManzana, string direccionLote, string direccionCalle, string direccionComplemento,
            string propietarioPatente, string rFCPropietario, string comodatario, string rFCComodatario,
            string telefono, string celular, string correoElectronico, int idUsuarioOperador
            )
        {
            ClsSocio clsSocio = new ClsSocio();
            clsSocio.Id = idSocioBuscado;
            clsSocio.NumeroLicencia = numeroLicencia;
            clsSocio.NombreComercial = nombreComercial;
            clsSocio.DireccionSupmza = direccionSupmza;
            clsSocio.DireccionManzana = direccionManzana;
            clsSocio.DireccionLote = direccionLote;
            clsSocio.DireccionCalle = direccionCalle;
            clsSocio.DireccionComplemento = direccionComplemento;
            clsSocio.PropietarioPatente = propietarioPatente;
            clsSocio.RFCPropietario = rFCPropietario;
            clsSocio.Comodatario = comodatario;
            clsSocio.RFCComodatario = rFCComodatario;
            clsSocio.Telefono = telefono;
            clsSocio.Celular = celular;
            clsSocio.CorreoElectronico = correoElectronico;
            clsSocio.IdUsuarioModifico = idUsuarioOperador;

            return (clsSocio.Socio_update());
        }

        private DataTable Socio_BuscarXComodatarioController(string comodatarioBuscado)
        {
            ClsSocio clsSocio = new ClsSocio();
            clsSocio.Comodatario = comodatarioBuscado;
            return (clsSocio.Socio_BuscarXComodatario()  );
        }


        private DataTable Socio_BuscarXRFCComodatarioController(string rfcComodatarioBuscado)
        {
            ClsSocio clsSocio = new ClsSocio();
            clsSocio.RFCComodatario = rfcComodatarioBuscado;

            return (clsSocio.Socio_BuscarXRFCComodatario());
        }

        private DataTable Socio_BuscarXPropietarioPatenteController( string propietarioPatenteBuscado)
        {
            ClsSocio clsSocio = new ClsSocio();
            clsSocio.PropietarioPatente = propietarioPatenteBuscado;

            return (clsSocio.Socio_BuscarXPropietarioPatente());
        }

        //------------Utils
        private void LimpiarTextBoxes()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();

            textBox15.Clear();
            textBox10.Clear();
            textBox16.Clear();
            textBox11.Clear();
            textBox17.Clear();
            textBox12.Clear();
            textBox20.Clear();
        }


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
            textBox1.Text = fila["Id"].ToString();
            textBox2.Text = fila["NumeroLicencia"].ToString();
            textBox3.Text = fila["NombreComercial"].ToString();
            textBox4.Text = fila["DireccionSupmza"].ToString();
            textBox6.Text = fila["DireccionLote"].ToString();
            textBox5.Text = fila["DireccionManzana"].ToString();
            textBox7.Text = fila["DireccionCalle"].ToString();
            textBox8.Text = fila["DireccionComplemento"].ToString();
            textBox9.Text = fila["PropietarioPatente"].ToString();
            textBox15.Text = fila["RFCPropietario"].ToString();
            textBox10.Text = fila["Comodatario"].ToString();
            textBox16.Text = fila["RFCComodatario"].ToString();
            textBox11.Text = fila["Telefono"].ToString();
            textBox17.Text = fila["Celular"].ToString();
            textBox12.Text = fila["CorreoElectronico"].ToString();
        }




        //-----------Events

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                DialogResult res = MessageBox.Show("¿Esta usted seguro que desea continuar?", "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(res == DialogResult.Yes)
                {
                    //Si esta vacio se va a insertar un Socio nuevo
                    if(textBox1.Text == String.Empty)
                    {
                        int idUsuarioOperador = 2;

                        string respuesta = Socio_createController(textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text,
                          textBox8.Text, textBox9.Text, textBox15.Text, textBox10.Text, textBox16.Text, textBox11.Text, textBox17.Text,
                          textBox12.Text, idUsuarioOperador);

                        LimpiarTextBoxes();
                        comboBox1.SelectedIndex = -1;
                        dataGridView1.DataSource = null;
                    }

                    else
                    {  //Es una actualización
                        int idUsuarioOperador = 2;
                        int idSocioBuscado = Int32.Parse(textBox1.Text);

                        string respuesta = Socio_updateController(idSocioBuscado, textBox2.Text, textBox3.Text, textBox4.Text,
                            textBox5.Text, textBox6.Text, textBox7.Text, textBox8.Text,
                            textBox9.Text, textBox15.Text, textBox10.Text, textBox16.Text,
                            textBox11.Text, textBox17.Text, textBox12.Text, idUsuarioOperador);

                        LimpiarTextBoxes();
                        comboBox1.SelectedIndex = -1;
                        dataGridView1.DataSource = null;

                    }

                }
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + ex.Source);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LimpiarTextBoxes();
            comboBox1.SelectedIndex = -1;
            dataGridView1.DataSource = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //Limpiar los textBoxes que estan abajo del datagridview1 
                textBox1.Clear();   textBox2.Clear();  textBox3.Clear();   textBox4.Clear();
                textBox5.Clear(); textBox6.Clear();  textBox7.Clear();  textBox8.Clear();
                textBox9.Clear(); textBox15.Clear();  textBox10.Clear(); textBox16.Clear();
                textBox11.Clear(); textBox17.Clear();  textBox12.Clear();

                dataGridView1.DataSource = null;

                if(comboBox1.SelectedIndex == 1)
                {
                    DataTable respuesta = Socio_BuscarXPropietarioPatenteController(textBox20.Text);
                    MostrarResultadoDeBusquedaXFiltro(respuesta);
                }

                if (comboBox1.SelectedIndex == 2 )
                {
                    DataTable respuesta = Socio_BuscarXRFCPropietarioController(textBox20.Text);
                    MostrarResultadoDeBusquedaXFiltro(respuesta);
                }

                if(comboBox1.SelectedIndex == 3)
                {
                    DataTable respuesta = Socio_BuscarXComodatarioController(textBox20.Text);
                    MostrarResultadoDeBusquedaXFiltro(respuesta);
                }

                if(comboBox1.SelectedIndex == 4)
                {
                    DataTable respuesta = Socio_BuscarXRFCComodatarioController(textBox20.Text);
                    MostrarResultadoDeBusquedaXFiltro(respuesta);
                }

            }

            catch(Exception ex)
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

                    string idSocioEnTexto = (dataGridView1.Rows[e.RowIndex].Cells[0].EditedFormattedValue).ToString();
                    DataTable miTabla = Socio_BuscarXIdController(Int32.Parse(idSocioEnTexto));
                    MostrarEnTxtBoxsSocio(miTabla);                   
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }              
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
