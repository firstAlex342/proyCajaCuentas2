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
    public partial class FrmUsuarioModificar : MetroForm
    {
        public FrmUsuarioModificar()
        {
            InitializeComponent();

            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
        }

        //----------------Methods
        private DataTable Usuario_BuscarXUsuarioController(string usuarioBuscado)
        {
            ClsUsuario clsUsuario = new ClsUsuario();
            clsUsuario.Usuario = usuarioBuscado;
            return (clsUsuario.Usuario_BuscarXUsuario());
        }

        private string Usuario_Actualizar(int idUsuarioBuscado, string nuevoNombre, string nuevoUsuario,
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

        //---------------Utils
        private void MostrarEnTextBoxes(DataTable tabla)
        {
            DataRow filaUnica = (tabla.AsEnumerable()).FirstOrDefault();

            if (filaUnica != null)
            {

                
                metroTextBox1.Text = filaUnica["Usuario"].ToString();
                metroTextBox2.Text = filaUnica["Password"].ToString();
                metroTextBox3.Text = filaUnica["Nombre"].ToString();
                metroTextBox5.Text = filaUnica["Id"].ToString();
            }

        }

        private void LimpiarMetrotextBoxesDeDetalles()
        {
            metroTextBox1.Clear();
            metroTextBox2.Clear();
            metroTextBox3.Clear();
            metroTextBox5.Clear();
        }



        //------------Events

        private void metroButton1_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarMetrotextBoxesDeDetalles();

                //Regresa una ó ninguna fila

                DataTable respuesta = Usuario_BuscarXUsuarioController(metroTextBox4.Text);
                MostrarEnTextBoxes(respuesta);
            }

            catch(Exception ex)
            {
                MetroMessageBox.Show(this, ex.Message + " " + ex.Source);
            }
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            try
            {

                DialogResult res = MetroMessageBox.Show(this, "¿Esta usted seguro que desea continuar?", "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    int idUsuarioBuscado = Int32.Parse(metroTextBox5.Text);

                    string respuesta = Usuario_Actualizar(idUsuarioBuscado, metroTextBox3.Text, metroTextBox1.Text,
                        metroTextBox2.Text, ClsLogin.Id);

                    if (respuesta.Contains("ok"))
                    {
                        MetroMessageBox.Show(this, "Registro guardado exitosamente", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    else
                    {
                        MetroMessageBox.Show(this, "Error en FrmUsuarioModificar", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    metroTextBox4.Clear();
                    LimpiarMetrotextBoxesDeDetalles();
                }
            }

            catch(Exception ex)
            {
                MetroMessageBox.Show(this, ex.Message + " " + ex.Source);
            }
        }
    }
}
