using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Collections;
using CapaLogicaNegocios;
using System.Data.SqlClient;

namespace CapaPresentacion
{
    public partial class FrmConfigurarInfoConexion : Form
    {
        //------------------Constructor 
        public FrmConfigurarInfoConexion()
        {
            InitializeComponent();

            try
            {
                textBox5.Text = ObtenerCadenaConexionAppController();
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }//constructor parameterless


        public FrmConfigurarInfoConexion(bool modo)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.StartPosition = FormStartPosition.CenterScreen;

            

            try
            {
                textBox5.Text = ObtenerCadenaConexionAppController();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }

        //----------------------------------Methods



        //-------------------------------Controllers
        private string ObtenerCadenaConexionAppController()
        {
            ClsEnlaceToAppConfig clsEnlaceToAppConfig = new ClsEnlaceToAppConfig();
            return (clsEnlaceToAppConfig.ObtenerCadenaConexionAppConfig());
        }

        private void ModificarContenidoAppConfigController(string cadena)
        {
            ClsEnlaceToAppConfig clsEnlaceToAppConfig = new ClsEnlaceToAppConfig();
            clsEnlaceToAppConfig.ModificarContenidoAppConfig(cadena);
        }

        private void QuitarProteccion()
        {
            ClsEnlaceToAppConfig cls = new ClsEnlaceToAppConfig();
            cls.QuitarEncriptacion();
        }


        //-------------------------------Events
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                DialogResult res = MessageBox.Show("¿Esta usted seguro que desea continuar?", "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if(res == DialogResult.Yes)
                {
                    ModificarContenidoAppConfigController(textBox5.Text);
                    MessageBox.Show(this, "Reinice la aplicacion para aplicar cambios", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }               
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }


    }
}
