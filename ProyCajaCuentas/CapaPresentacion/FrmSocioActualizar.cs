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
    public partial class FrmSocioActualizar : Form
    {
        public FrmSocioActualizar()
        {
            InitializeComponent();
        }


        
        //------------------Methods
        private DataTable Socio_BuscarXLicenciaController(string licenciaBuscada)
        {
            ClsSocio clsSocio = new ClsSocio();
            clsSocio.NumeroLicencia = licenciaBuscada;
            return (clsSocio.Socio_BuscarXLicencia());
        }


        private string Socio_updateController(int id, string numeroLicencia, string nombreComercial, string direccionSupmza,
    string direccionManzana, string direccionLote, string direccionCalle, string direccionComplemento,
    string propietarioPatente, string rFCPropietario, string comodatario, string rFCComodatario,
    string telefono, string celular, string correoElectronico, int idUsuarioOperador
    )
        {
            ClsSocio clsSocio = new ClsSocio();
            clsSocio.Id = id;
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


        //---------------------Utils
        private void LimpiarTextBoxesDetalles()
        {
            textBox2.Text = "";
            textBox3.Text = "";
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
        }

        private void MostrarEnTxtBoxesSocio(DataTable infoSocio)
        {
            DataRow filaUnica = (infoSocio.AsEnumerable()).Single();

            textBox16.Text = filaUnica["Id"].ToString();
            textBox2.Text = filaUnica["NumeroLicencia"].ToString();
            textBox9.Text = filaUnica["NombreComercial"].ToString();
            textBox3.Text = filaUnica["DireccionSupmza"].ToString();
            textBox10.Text = filaUnica["DireccionLote"].ToString();
            textBox4.Text = filaUnica["DireccionManzana"].ToString();
            textBox11.Text = filaUnica["DireccionCalle"].ToString();
            textBox5.Text = filaUnica["DireccionComplemento"].ToString();
            textBox12.Text = filaUnica["PropietarioPatente"].ToString();
            textBox6.Text = filaUnica["RFCPropietario"].ToString();
            textBox13.Text = filaUnica["Comodatario"].ToString();
            textBox7.Text = filaUnica["RFCComodatario"].ToString();
            textBox14.Text = filaUnica["Telefono"].ToString();
            textBox8.Text = filaUnica["Celular"].ToString();
            textBox15.Text = filaUnica["CorreoElectronico"].ToString();
        }


        //-------------------------Events

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarTextBoxesDetalles();

                DataTable respuesta = Socio_BuscarXLicenciaController(textBox1.Text);

                if( respuesta.Rows.Count == 1)
                {
                    MostrarEnTxtBoxesSocio(respuesta);
                }

                else
                {
                    MessageBox.Show("No se encontro la licencia " + textBox1.Text, "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


            catch(Exception ex )
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult res = MessageBox.Show(this, "¿Esta usted seguro que desea continuar?", "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    
                    string respuesta = Socio_updateController(Int32.Parse(textBox16.Text), textBox2.Text, textBox9.Text, textBox3.Text, textBox4.Text,
                        textBox10.Text, textBox11.Text, textBox5.Text, textBox12.Text, 
                        textBox6.Text, textBox13.Text, textBox7.Text, textBox14.Text,
                        textBox8.Text, textBox15.Text, ClsLogin.Id);

                    if (respuesta.Contains("ok"))
                    {
                        MessageBox.Show(this, "Registro guardado exitosamente", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpiarTextBoxesDetalles();
                        textBox1.Text = "";
                    }

                    else
                    {
                        MessageBox.Show(this, respuesta, "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LimpiarTextBoxesDetalles();
            textBox1.Text = "";
        }


    }
}
