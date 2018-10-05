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
    public partial class FrmProductoCrear : Form
    {
        //-----------------Constructor
        public FrmProductoCrear()
        {
            InitializeComponent();
        }

        //-----------------Methods
        private string Producto_Producto_createController(string nombreProducto, string descripcion, int idUsuarioOperador)
        {
            ClsProducto clsProducto = new ClsProducto();
            clsProducto.Nombre = nombreProducto;
            clsProducto.Descripcion = descripcion;
            clsProducto.IdUsuarioAlta = idUsuarioOperador;

            return (clsProducto.Producto_create());
        }

        //--------------------Utils
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


        //-----------------Events
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Esta usted seguro que desea continuar?", "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (respuesta == DialogResult.Yes)
                {
                    bool capturoNombreDelProducto = TieneAlgoMasQueEspaciosEnBlanco(textBox1.Text);
                    if (capturoNombreDelProducto)
                    {
                        string mensaje = Producto_Producto_createController(textBox1.Text, "", ClsLogin.Id);
                        if (mensaje.Contains("ok"))
                        {
                            StringBuilder texto = new StringBuilder();
                            texto.Append("El producto ");
                            texto.Append(textBox1.Text);
                            texto.Append(" ha sido creado exitosamente");

                            MessageBox.Show(texto.ToString(), "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            textBox1.Text = "";
                        }

                        else
                        {
                            MessageBox.Show(mensaje, "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }

                    else
                    {
                        MostrarMensajeSiNoSeCapturo("Nombre del producto", capturoNombreDelProducto);
                    }
                }
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }
    }
}
