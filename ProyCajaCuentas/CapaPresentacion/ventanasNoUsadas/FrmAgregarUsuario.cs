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
    public partial class FrmAgregarUsuario : MetroForm
    {
        public FrmAgregarUsuario()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
        }

        //----------------Methods 
        public int Usuario_createController(string nombre, string usuario, string password, int idUsuarioOperador)
        {
            ClsUsuario clsUsuario = new ClsUsuario();
            clsUsuario.Nombre = nombre;
            clsUsuario.Usuario = usuario;
            clsUsuario.Password = password;
            clsUsuario.IdUsuarioAlta = idUsuarioOperador;

            DataTable respuesta = clsUsuario.Usuario_create();

            DataRow filaUnica = respuesta.Rows[0];

            int idGenerado = Int32.Parse(filaUnica[0].ToString());
            return (idGenerado);
        }


        private DataTable ObtenerListaDeTodosLosModulosController()
        {
            ClsModulo clsModulo = new ClsModulo();
            return (clsModulo.Modulo_Select_Id_Nombre_DeTodos());
        }

        private void DarAccesoAModulosAUsuarioController(int idUsuario, DataTable listaDeTodosLosModulos)
        {
            foreach(DataRow fila in listaDeTodosLosModulos.Rows)
            {
                ClsAccesoAModulo clsAccesoAModulo = new ClsAccesoAModulo();
                clsAccesoAModulo.IdUsuario = idUsuario;
                clsAccesoAModulo.IdModulo = Int32.Parse(fila[0].ToString());

                clsAccesoAModulo.AccesoAModulo_create();
            }
        }


        //---------Utils

        private void LimpiarTextBoxes()
        {
            metroTextBox1.Clear();
            metroTextBox2.Clear();
            metroTextBox3.Clear();
        }



        private void metroButton1_Click(object sender, EventArgs e)
        {
            try
            {
                int idGenerado = Usuario_createController(metroTextBox1.Text, metroTextBox2.Text, metroTextBox3.Text, ClsLogin.Id);
                DataTable listaDeTodosLosModulos = ObtenerListaDeTodosLosModulosController();
                DarAccesoAModulosAUsuarioController(idGenerado, listaDeTodosLosModulos);

                MetroMessageBox.Show(this, "Nuevo usuario agregado exitosamente", "Resultado de operacion", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarTextBoxes();
            }

            catch(Exception ex)
            {
                MetroMessageBox.Show(this, ex.Message + " " + ex.Source);
            }
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            LimpiarTextBoxes();
        }
    }
}
