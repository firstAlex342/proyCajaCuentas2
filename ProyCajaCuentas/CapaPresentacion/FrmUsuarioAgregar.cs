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
    public partial class FrmUsuarioAgregar : Form
    {
        //----------------Constructor
        public FrmUsuarioAgregar()
        {
            InitializeComponent();
        }

        //----------------Methods controller
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
            foreach (DataRow fila in listaDeTodosLosModulos.Rows)
            {
                ClsAccesoAModulo clsAccesoAModulo = new ClsAccesoAModulo();
                clsAccesoAModulo.IdUsuario = idUsuario;
                clsAccesoAModulo.IdModulo = Int32.Parse(fila[0].ToString());

                clsAccesoAModulo.AccesoAModulo_create();
            }
        }

        //----------------Utils
        private void LimpiarTextBoxes()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
        }


        //-------------------Events
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int idGenerado = Usuario_createController(textBox1.Text.Trim(), textBox2.Text.Trim(), textBox3.Text.Trim(), ClsLogin.Id);
                DataTable listaDeTodosLosModulos = ObtenerListaDeTodosLosModulosController();
                DarAccesoAModulosAUsuarioController(idGenerado, listaDeTodosLosModulos);

                MessageBox.Show("Nuevo usuario agregado exitosamente", "Resultado de operacion", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarTextBoxes();
            }

            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message + " " + ex.Source);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LimpiarTextBoxes();
        }
    }
}
