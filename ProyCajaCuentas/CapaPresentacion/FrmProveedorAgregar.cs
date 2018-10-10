using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaLogicaNegocios;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class FrmProveedorAgregar : Form
    {
        //-------------------constructor
        public FrmProveedorAgregar()
        {
            InitializeComponent();
        }

        //---------------------Methods controllers
        private string Proveedor_create_Controller(string nombre, string superManzana, string lote, string manzana,
            string calle, string complemento, string telefono, string celular, string correoElectronico)
        {
            ClsProveedor clsProveedor = new ClsProveedor();
            clsProveedor.Nombre = nombre;
            clsProveedor.DireccionSupmza = superManzana;
            clsProveedor.DireccionLote = lote;
            clsProveedor.DireccionManzana = manzana;
            clsProveedor.DireccionCalle = calle;
            clsProveedor.DireccionComplemento = complemento;
            clsProveedor.Telefono = telefono;
            clsProveedor.Celular = celular;
            clsProveedor.CorreoElectronico = correoElectronico;

            clsProveedor.IdUsuarioAlta = ClsLogin.Id;
            return ( clsProveedor.Proveedor_create() );
        }


        //--------------------------Utils
        private bool TieneAlgoMasQueEspaciosEnBlanco(string texto)
        {
            string cad = texto.Trim();

            bool res = cad.Length > 0 ? true : false;
            return (res);
        }

        private void MostrarMensajeSiNoSeCapturo(string nombreDelCampo, bool tieneCapturaElCampo)
        {
            if (tieneCapturaElCampo == false)
                MessageBox.Show(nombreDelCampo + " no capturado, se requiere", "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void LimpiarContenidoGroupBoxDetallesProveedor()
        {
            textBox8.Text = "";
            textBox2.Text = "";
            textBox9.Text = "";
            textBox3.Text = "";
            textBox10.Text = "";
            textBox4.Text = "";
            textBox13.Text = "";
            textBox7.Text = "";
            textBox14.Text = "";
        }
        //----------------------Events
        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult res = MessageBox.Show(this, "¿Esta usted seguro que desea continuar?", "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    bool capturoNombreProveedor = TieneAlgoMasQueEspaciosEnBlanco(textBox8.Text);
                    if(capturoNombreProveedor)
                    {
                        string mensaje = Proveedor_create_Controller(textBox8.Text, textBox2.Text, textBox9.Text,
                            textBox3.Text, textBox10.Text, textBox4.Text, textBox13.Text, textBox7.Text, textBox14.Text);
                        
                        if( mensaje.Contains("ok") )
                        {
                            MessageBox.Show("Registros guardados exitosamente", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LimpiarContenidoGroupBoxDetallesProveedor();
                        }

                        else
                        {
                            MessageBox.Show(mensaje, "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }

                    else
                    {
                        MostrarMensajeSiNoSeCapturo("Nombre de proveedor", capturoNombreProveedor);
                    }
                }

            }

            catch(Exception ex)
            {
                MessageBox.Show(this, ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LimpiarContenidoGroupBoxDetallesProveedor();
        }
    }
}
