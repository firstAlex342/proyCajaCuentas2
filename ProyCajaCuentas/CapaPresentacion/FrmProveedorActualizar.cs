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
    public partial class FrmProveedorActualizar : Form
    {
        //------------------------constructor
        public FrmProveedorActualizar()
        {
            InitializeComponent();
        }

        //----------------------Methods controllers
        private DataTable Proveedor_BuscarPrimerProveedorConNombreActivoController(string nombreProveedor)
        {
            ClsProveedor clsProveedor = new ClsProveedor();
            clsProveedor.Nombre = nombreProveedor;

            return (clsProveedor.Proveedor_BuscarPrimerProveedorConNombreActivo());
        }

        private string Proveedor_updateController(int id, string nombre, string superManzana, string lote, string manzana, string calle, 
            string complemento, string telefono, string celular, string correoElectronico, int idUsuarioOperador )
        {
            ClsProveedor clsProveedor = new ClsProveedor();
            clsProveedor.Id = id;
            clsProveedor.Nombre = nombre;
            clsProveedor.DireccionSupmza = superManzana;
            clsProveedor.DireccionLote = lote;
            clsProveedor.DireccionManzana = manzana;
            clsProveedor.DireccionCalle = calle;
            clsProveedor.DireccionComplemento = complemento;
            clsProveedor.Telefono = telefono;
            clsProveedor.Celular = celular;
            clsProveedor.CorreoElectronico = correoElectronico;
            clsProveedor.IdUsuarioModifico = idUsuarioOperador;

            return (clsProveedor.Proveedor_update());
        }
        //---------------------Utils
        private void MostrarInfoProveedorEnTextBoxes(DataTable tabla)
        {
            var x = tabla.AsEnumerable().Single();
            textBox1.Text = (x.Field<int>("Id")).ToString();
            textBox8.Text = x.Field<string>("Nombre");
            textBox2.Text = x.Field<string>("DireccionSupmza");
            textBox9.Text = x.Field<string>("DireccionLote");
            textBox3.Text = x.Field<string>("DireccionManzana");
            textBox10.Text = x.Field<string>("DireccionCalle");
            textBox4.Text = x.Field<string>("DireccionComplemento");
            textBox13.Text = x.Field<string>("Telefono");
            textBox7.Text = x.Field<string>("Celular");
            textBox14.Text = x.Field<string>("CorreoElectronico");
        }

        private void LimpiarTextBoxes()
        {
            textBox1.Text = "";
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

        private void HabilitarTextBoxes2A14()
        {
            textBox2.Enabled = true;
            textBox9.Enabled = true;
            textBox3.Enabled = true;
            textBox10.Enabled = true;
            textBox4.Enabled = true;
            textBox13.Enabled = true;
            textBox7.Enabled = true;
            textBox14.Enabled = true;
        }

        private void DeshabilitartextBoxes2A14()
        {
            textBox2.Enabled = false;
            textBox9.Enabled = false;
            textBox3.Enabled = false;
            textBox10.Enabled = false;
            textBox4.Enabled = false;
            textBox13.Enabled = false;
            textBox7.Enabled = false;
            textBox14.Enabled = false;
        }

        //----------------------Events
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable res = Proveedor_BuscarPrimerProveedorConNombreActivoController(textBox8.Text);
                if(res.Rows.Count > 0)
                {
                    LimpiarTextBoxes();
                    HabilitarTextBoxes2A14();
                    textBox8.ReadOnly = true;
                    button1.Enabled = false;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    MessageBox.Show("Proveedor encontrado", "Resultado de búsqueda", MessageBoxButtons.OK, MessageBoxIcon.Information);                    
                    MostrarInfoProveedorEnTextBoxes(res);
                }
                else
                {
                    MessageBox.Show("Proveedor " + textBox8.Text + " no encontrado", "Resultado de búsqueda", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Esta usted seguro que desea continuar?", "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (respuesta == DialogResult.Yes)
                {
                    string mensaje = Proveedor_updateController(Int32.Parse(textBox1.Text), textBox8.Text, textBox2.Text, textBox9.Text,
                        textBox3.Text, textBox10.Text, textBox4.Text, textBox13.Text, textBox7.Text,
                        textBox14.Text, ClsLogin.Id);

                    if (mensaje.Contains("ok"))
                    {
                        MessageBox.Show("Proveedor actualizado", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpiarTextBoxes();
                        DeshabilitartextBoxes2A14();
                        button1.Enabled = true;
                        button2.Enabled = false;
                        button3.Enabled = false;
                        textBox8.ReadOnly = false;
                    }

                    else
                    {
                        MessageBox.Show(mensaje, "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            DialogResult respuesta = MessageBox.Show("¿Esta usted seguro que desea salir sin guardar los cambios?", "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (respuesta == DialogResult.Yes)
            {
                LimpiarTextBoxes();
                DeshabilitartextBoxes2A14();
                textBox8.ReadOnly = false;
                button1.Enabled = true;
                button2.Enabled = false;
                button3.Enabled = false;
            }
                
        }
    }
}
