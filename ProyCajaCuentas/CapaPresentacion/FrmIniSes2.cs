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
    public partial class FrmIniSes2 : Form
    {
        public FrmIniSes2()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        //----------------------Controllers
        public bool ExisteUsuarioYEsActivoController(string usuario, string password)
        {
            ClsUsuario clsUsuario = new ClsUsuario();
            clsUsuario.Usuario = usuario;
            clsUsuario.Password = password;

            DataTable respuesta = clsUsuario.Usuario_BuscarXUsuarioYPassword();

            if (respuesta.Rows.Count == 1)
            {
                //Existe una fila, vere si esta activo ó no
                var res = respuesta.AsEnumerable();
                DataRow fila = res.First();

                string valorEnCadena = fila["Activo"].ToString();
                bool valorEnBool = Boolean.Parse(valorEnCadena);
                return (valorEnBool);  //regresa true or false
            }

            else
            { //No existe el usuario
                return (false);
            }
        }

        public int BuscarIdUsuarioController(string usuario, string password)
        {
            ClsUsuario clsUsuario = new ClsUsuario();
            clsUsuario.Usuario = usuario;
            clsUsuario.Password = password;

            DataTable respuesta = clsUsuario.Usuario_BuscarXUsuarioYPassword();
            DataRow filaUnica = respuesta.AsEnumerable().First<DataRow>();

            int idBuscado = Int32.Parse(filaUnica["Id"].ToString());
            return (idBuscado);
        }

        public DataTable BuscarUsuarioXIdController(int idBuscado)
        {
            ClsUsuario clsUsuario = new ClsUsuario();
            clsUsuario.Id = idBuscado;

            return (clsUsuario.Usuario_BuscarXId());
        }

        public DataTable BuscarModulosALosQueTieneAccesoController(int idUsuarioBuscado)
        {
            ClsAccesoAModulo clsAccesoAModulo = new ClsAccesoAModulo();
            clsAccesoAModulo.IdUsuario = idUsuarioBuscado;
            return (clsAccesoAModulo.AccesoAModulo_Modulo_InnerJoin());
        }



        //------------------------------Events
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                bool existeUsuarioYEsActivo = ExisteUsuarioYEsActivoController(textBox1.Text, textBox2.Text);
                if (existeUsuarioYEsActivo)
                {
                    int idUsuario = BuscarIdUsuarioController(textBox1.Text, textBox2.Text);
                    DataTable tablaUsuario = BuscarUsuarioXIdController(idUsuario);  //recupero solo de la tabla Usuario
                    DataTable modulosALosQueTieneAccesoUsuario = BuscarModulosALosQueTieneAccesoController(idUsuario); //recupero de la tabla AccesoAModulo y de tabla Modulo


                    ClsLogin.Id = idUsuario;
                    ClsLogin.Usuario = tablaUsuario;
                    ClsLogin.ModulosALosQueTieneAccesoUsuario = modulosALosQueTieneAccesoUsuario;

                    this.Hide();
                    //FrmPrincipal frmPrincipal = new FrmPrincipal();
                    //frmPrincipal.ShowDialog();
                    //frmPrincipal.Dispose();
                    //Application.Exit();

                    //FrmPrincipal3 frmPrincipal = new FrmPrincipal3();
                    //frmPrincipal.ShowDialog();
                    //frmPrincipal.Dispose();
                    //Application.Exit();

                    FrmRaiz2 frmPrincipal = new FrmRaiz2();
                    frmPrincipal.ShowDialog();
                    frmPrincipal.Dispose();
                    Application.Exit();

                }

                else
                {
                    string cadena = "No se encontro el usuario =" + textBox1.Text + " ó existe pero esta inactivo";
                    MessageBox.Show(this, cadena, "Resultado de inicio de sesión", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                StringBuilder errorMessages = new StringBuilder();
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    errorMessages.Append("Index #" + i + "\n" +
                        "Message: " + ex.Errors[i].ToString() + "\n" +
                        "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                        "Source: " + ex.Errors[i].Source + "\n" +
                        "Procedure: " + ex.Errors[i].Procedure + "\n");
                }
                MessageBox.Show(errorMessages.ToString(), "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("¿Esta usted seguro que desea configurar la conexión?", "Conexión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(res == DialogResult.Yes)
            {
                try
                {
                    FrmConfigurarInfoConexion frmConfigurarInfoConexion = new FrmConfigurarInfoConexion(false);
                    frmConfigurarInfoConexion.ShowDialog();
                    frmConfigurarInfoConexion.Dispose();
                }

                catch (System.Data.SqlClient.SqlException ex)
                {
                    StringBuilder errorMessages = new StringBuilder();
                    for (int i = 0; i < ex.Errors.Count; i++)
                    {
                        errorMessages.Append("Index #" + i + "\n" +
                            "Message: " + ex.Errors[i].ToString() + "\n" +
                            "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                            "Source: " + ex.Errors[i].Source + "\n" +
                            "Procedure: " + ex.Errors[i].Procedure + "\n");
                    }
                    MessageBox.Show(errorMessages.ToString(), "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message + " " + ex.Source + " " + ex.StackTrace);
                }
            }
        }
    }
}
