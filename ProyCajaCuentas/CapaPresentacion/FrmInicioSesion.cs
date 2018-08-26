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
    public partial class FrmInicioSesion : MetroForm
    {

        //--------------Constructor
        public FrmInicioSesion()
        {
            InitializeComponent();
            //this.MinimumSize = this.Size;
            //this.MaximumSize = this.Size;
        }


        //--------------Methods
        public bool ExisteUsuarioYEsActivoController(string usuario, string password)
        {
            ClsUsuario clsUsuario = new ClsUsuario();
            clsUsuario.Usuario = usuario;
            clsUsuario.Password = password;

            DataTable respuesta = clsUsuario.Usuario_BuscarXUsuarioYPassword();

            if(respuesta.Rows.Count == 1)
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

        public DataTable BuscarModulosALosQueTieneAccesoController( int idUsuarioBuscado)
        {
            ClsAccesoAModulo clsAccesoAModulo = new ClsAccesoAModulo();
            clsAccesoAModulo.IdUsuario = idUsuarioBuscado;
            return (clsAccesoAModulo.AccesoAModulo_Modulo_InnerJoin());
        }

        //----------------Eventos
        private  void metroButton1_Click(object sender, EventArgs e)
        {
            
            try
            {
                bool existeUsuarioYEsActivo = ExisteUsuarioYEsActivoController(metroTextBox1.Text, metroTextBox2.Text);
                if(existeUsuarioYEsActivo)
                {
                    int idUsuario = BuscarIdUsuarioController(metroTextBox1.Text, metroTextBox2.Text);
                    DataTable tablaUsuario = BuscarUsuarioXIdController(idUsuario);  //recupero solo de la tabla Usuario
                    DataTable modulosALosQueTieneAccesoUsuario = BuscarModulosALosQueTieneAccesoController(idUsuario); //recupero de la tabla AccesoAModulo y de tabla Modulo
                    

                    ClsLogin.Id = idUsuario;
                    ClsLogin.Usuario = tablaUsuario;
                    ClsLogin.ModulosALosQueTieneAccesoUsuario = modulosALosQueTieneAccesoUsuario;

                    this.Hide();
                    FrmPrincipal frmPrincipal = new FrmPrincipal();
                    frmPrincipal.ShowDialog();
                    frmPrincipal.Dispose();
                    Application.Exit();

                    //FrmPrincipal frmPrincipal = new FrmPrincipal();
                    //frmPrincipal.Show(this);
                    //this.Hide();

                }

                else
                {
                    string cadena = "No se encontro el usuario =" + metroTextBox1.Text + " ó existe pero esta inactivo";
                    MetroMessageBox.Show(this, cadena, "Resultado de inicio de sesión", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            catch (Exception ex)
            {
                MetroMessageBox.Show(this, ex.Message + " " + ex.Source);
            }
        }

        private  void FrmInicioSesion_Load(object sender, EventArgs e)
        {

           
        }
    }
}
