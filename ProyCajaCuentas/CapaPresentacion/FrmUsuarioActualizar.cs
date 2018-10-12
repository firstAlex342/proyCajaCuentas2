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
    public partial class FrmUsuarioActualizar : Form
    {
        //------------------Constructor
        public FrmUsuarioActualizar()
        {
            InitializeComponent();
        }

        //----------------------Controllers
        private DataTable Usuario_BuscarXUsuarioController(string usuarioBuscado)
        {
            ClsUsuario clsUsuario = new ClsUsuario();
            clsUsuario.Usuario = usuarioBuscado;
            return (clsUsuario.Usuario_BuscarXUsuario());
        }

        private string Usuario_ActualizarController(int idUsuarioBuscado, string nuevoNombre, string nuevoUsuario,
    string nuevoPassword, int idUsuarioOperador)
        {
            ClsUsuario clsUsuario = new ClsUsuario();
            clsUsuario.Id = idUsuarioBuscado;
            clsUsuario.Nombre = nuevoNombre;
            clsUsuario.Usuario = nuevoUsuario;
            clsUsuario.Password = nuevoPassword;
            clsUsuario.IdUsuarioModifico = idUsuarioOperador;

            return (clsUsuario.Usuario_update());
        }

        //---------------------Utils
        private void LimpiarMetrotextBoxesDeDetalles()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
        }

        private void MostrarEnTextBoxes(DataTable tabla)
        {
            DataRow filaUnica = (tabla.AsEnumerable()).FirstOrDefault();

            if (filaUnica != null)
            {


                textBox1.Text = filaUnica["Usuario"].ToString();
                textBox2.Text = filaUnica["Password"].ToString();
                textBox3.Text = filaUnica["Nombre"].ToString();
                textBox4.Text = filaUnica["Id"].ToString();
            }

        }

        private void HabilitarTextBoxesYButtonGuardarCancelarDeshabButtonBuscar()
        {
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            button3.Enabled = false;
            button1.Enabled = true;
            button2.Enabled = true;
        }

        private void LimpiarTextBoxesQueNoSonUsuario()
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
        }

        private void DeshaTextBoxesExcepUsuarioDesHabButtonGuardarCancelarHabButtonBuscar()
        {
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            button1.Enabled = false;
            button3.Enabled = true;
            button2.Enabled = false;
        }

        //---------------Events
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarTextBoxesQueNoSonUsuario();

                //Regresa una ó ninguna fila
                DataTable res = Usuario_BuscarXUsuarioController(textBox1.Text);
                if(res.Rows.Count == 1)
                {
                    MostrarEnTextBoxes(res);
                    HabilitarTextBoxesYButtonGuardarCancelarDeshabButtonBuscar();
                    textBox1.Enabled = false;
                }

                else
                {
                    MessageBox.Show("No se encontro el usuario " + textBox1.Text , "Reglas de operación",MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult res = MessageBox.Show("¿Esta usted seguro que desea continuar?", "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    int idUsuarioBuscado = Int32.Parse(textBox4.Text);

                    string respuesta = Usuario_ActualizarController(idUsuarioBuscado, textBox3.Text, textBox1.Text,
                        textBox2.Text, ClsLogin.Id);

                    if (respuesta.Contains("ok"))
                    {
                        MessageBox.Show("Registro guardado exitosamente", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpiarMetrotextBoxesDeDetalles();
                        DeshaTextBoxesExcepUsuarioDesHabButtonGuardarCancelarHabButtonBuscar();
                        textBox1.Enabled = true;
                    }

                    else
                    {
                        MessageBox.Show(this, "Error en FrmUsuarioActualizar", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }                   
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LimpiarMetrotextBoxesDeDetalles();
            DeshaTextBoxesExcepUsuarioDesHabButtonGuardarCancelarHabButtonBuscar();
            textBox1.Enabled = true;
        }
    }
}
