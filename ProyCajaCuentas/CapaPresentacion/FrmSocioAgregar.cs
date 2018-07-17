using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using MetroFramework;
using CapaLogicaNegocios;

namespace CapaPresentacion
{
    public partial class FrmSocioAgregar : MetroForm
    {
        public FrmSocioAgregar()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
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
        private void LimpiarMetroTextBoxes()
        {
            metroTextBox1.Clear();
            metroTextBox2.Clear();
            metroTextBox3.Clear();
            metroTextBox4.Clear();
            metroTextBox5.Clear();
            metroTextBox6.Clear();
            metroTextBox7.Clear();
            metroTextBox8.Clear();
            metroTextBox9.Clear();
            metroTextBox10.Clear();
            metroTextBox11.Clear();
            metroTextBox12.Clear();
            metroTextBox13.Clear();
            metroTextBox14.Clear();

        }



        //--------------------Eventos
        private void metroButton1_Click(object sender, EventArgs e)
        {
            try
            {

                DialogResult res = MetroMessageBox.Show(this, "¿Esta usted seguro que desea continuar?", "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(res == DialogResult.Yes)
                {
                    string respuesta = Socio_createController(metroTextBox1.Text, metroTextBox2.Text, metroTextBox3.Text, metroTextBox5.Text,
metroTextBox4.Text, metroTextBox6.Text, metroTextBox7.Text, metroTextBox8.Text, metroTextBox9.Text,
metroTextBox10.Text, metroTextBox11.Text, metroTextBox12.Text, metroTextBox13.Text, metroTextBox14.Text,
ClsLogin.Id);


                    if (respuesta.Contains("ok"))
                    {
                        MetroMessageBox.Show(this, "Registro guardado exitosamente", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    else
                    {
                        MetroMessageBox.Show(this, "Error en FrmSocioAgregar", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    LimpiarMetroTextBoxes();
                }


            }

            catch(Exception ex)
            {
                MetroMessageBox.Show(this, ex.Message + " " + ex.Source);
            }

        }

        private void metroTextBox14_Click(object sender, EventArgs e)
        {

        }
    }
}
