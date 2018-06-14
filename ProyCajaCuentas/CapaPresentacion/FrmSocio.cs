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
        }


        private void CargarComBoBoxOpcionesFiltro()
        {
            comboBox1.Items.Add("Direccion");
            comboBox1.Items.Add("Propietario");
            comboBox1.Items.Add("Patente");
            comboBox1.Items.Add("RFC propietario");
            comboBox1.Items.Add("Propietario");
            comboBox1.Items.Add("Comodatario");
            comboBox1.Items.Add("RFC comodatario");

        }

        //-----------Events

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                DialogResult res = MessageBox.Show("¿Estas usted seguro que desea continuar?", "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(res == DialogResult.Yes)
                {
                    int idUsuarioOperador = 2;

                    string respuesta = Socio_createController(textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text,
                      textBox8.Text, textBox9.Text, textBox15.Text, textBox10.Text, textBox16.Text, textBox11.Text, textBox17.Text,
                      textBox12.Text, idUsuarioOperador);

                    LimpiarTextBoxes();
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
        }
    }
}
