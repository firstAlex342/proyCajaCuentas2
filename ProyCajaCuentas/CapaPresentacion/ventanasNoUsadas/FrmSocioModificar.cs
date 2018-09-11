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
    public partial class FrmSocioModificar : MetroForm
    {
        public FrmSocioModificar()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
        }

        //------------------Methods
        private DataTable Socio_BuscarXLicenciaController(string licenciaBuscada)
        {
            ClsSocio clsSocio = new ClsSocio();
            clsSocio.NumeroLicencia = licenciaBuscada;
            return (clsSocio.Socio_BuscarXLicencia());
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

        //----------------Utils
        private void MostrarEnMetroTxtBoxesSocio(DataTable tabla)
        {
            DataRow filaUnica = (tabla.AsEnumerable()).FirstOrDefault();

            if(filaUnica != null)
            {
                metroTextBox15.Text = filaUnica["NumeroLicencia"].ToString();
                metroTextBox2.Text = filaUnica["NombreComercial"].ToString();
                metroTextBox3.Text = filaUnica["DireccionSupmza"].ToString();
                metroTextBox4.Text = filaUnica["DireccionLote"].ToString();
                metroTextBox5.Text = filaUnica["DireccionManzana"].ToString();
                metroTextBox6.Text = filaUnica["DireccionCalle"].ToString();
                metroTextBox7.Text = filaUnica["DireccionComplemento"].ToString();
                metroTextBox8.Text = filaUnica["PropietarioPatente"].ToString();
                metroTextBox9.Text = filaUnica["RFCPropietario"].ToString();
                metroTextBox10.Text = filaUnica["Comodatario"].ToString();
                metroTextBox11.Text = filaUnica["RFCComodatario"].ToString();
                metroTextBox12.Text = filaUnica["Telefono"].ToString();
                metroTextBox13.Text = filaUnica["Celular"].ToString();
                metroTextBox14.Text = filaUnica["CorreoElectronico"].ToString();
                metroTextBox16.Text = filaUnica["Id"].ToString();

            }
        }

        private void LimpiarMetroTextBoxesDetalles()
        {
            metroTextBox15.Clear();
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
            metroTextBox16.Clear();

        }


        //---------------------Events
        private void FrmSocioModificar_Load(object sender, EventArgs e)
        {

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarMetroTextBoxesDetalles();

                DataTable respuesta = Socio_BuscarXLicenciaController(metroTextBox1.Text);
                MostrarEnMetroTxtBoxesSocio(respuesta);
            }

            catch(Exception ex)
            {
                MetroMessageBox.Show(this, ex.Message + " " + ex.Source);
            }
        }

        private void metroTextBox15_Click(object sender, EventArgs e)
        {
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult res = MetroMessageBox.Show(this, "¿Esta usted seguro que desea continuar?", "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    int idSocioAActualizar = Int32.Parse(metroTextBox16.Text);
                    string respuesta = Socio_updateController(idSocioAActualizar, metroTextBox15.Text, metroTextBox2.Text, metroTextBox3.Text,
                        metroTextBox5.Text, metroTextBox4.Text, metroTextBox6.Text, metroTextBox7.Text,
                        metroTextBox8.Text, metroTextBox9.Text, metroTextBox10.Text, metroTextBox11.Text,
                        metroTextBox12.Text, metroTextBox13.Text, metroTextBox14.Text, ClsLogin.Id);

                    if (respuesta.Contains("ok"))
                    {
                        MetroMessageBox.Show(this, "Registro guardado exitosamente", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    else
                    {
                        MetroMessageBox.Show(this, "Error en FrmSocioModificar", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    metroTextBox1.Clear();
                    LimpiarMetroTextBoxesDetalles();
                }
             }

            catch(Exception ex)
            {
                MetroMessageBox.Show(this, ex.Message + " " + ex.Source);
            }
        }
    }
}
