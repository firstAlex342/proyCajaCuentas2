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
    public partial class FrmSocioAdd : Form
    {
        //-------------------constructor
        public FrmSocioAdd()
        {
            InitializeComponent();
        }


        //----------------Methods
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

            return (clsSocio.Socio_create());
        }


        //--------------Utils
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
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();
            textBox14.Clear();
        }


        //----------------------Eventos
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                DialogResult res = MessageBox.Show(this, "¿Esta usted seguro que desea continuar?", "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    string respuesta = Socio_createController(textBox1.Text.Trim(), textBox8.Text,textBox2.Text,
                        textBox3.Text, textBox9.Text, textBox10.Text, textBox4.Text, 
                        textBox11.Text, textBox5.Text, textBox12.Text, textBox6.Text, textBox13.Text,
                        textBox7.Text, textBox14.Text, ClsLogin.Id);


                    if (respuesta.Contains("ok"))
                    {
                        MessageBox.Show(this, "Registro guardado exitosamente", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpiarTextBoxes();
                    }

                    else
                    {
                        MessageBox.Show(this, respuesta, "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LimpiarTextBoxes();
        }
    }
}
