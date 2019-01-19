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
    public partial class FrmCancelarFolioReciboDePago : Form
    {

        //-----------Constructor
        public FrmCancelarFolioReciboDePago()
        {
            InitializeComponent();
        }

        //--------------Methods controller
        private string CancelarFolioReciboListaProductosController(string folioBuscado, int idUsuarioOperador)
        {
            ClsReciboListaProductos clsReciboListaProductos = new ClsReciboListaProductos();
            clsReciboListaProductos.Folio = folioBuscado;
            clsReciboListaProductos.IdUsuarioModifico = idUsuarioOperador;

            return (clsReciboListaProductos.ReciboListaProductos_UpdateActivoACero());

        }

        private bool SePuedeCancelarFolioReciboListaProductosController(string folioBuscado)
        {
            ClsReciboListaProductos clsReciboListaProductos = new ClsReciboListaProductos();
            clsReciboListaProductos.Folio = folioBuscado;
            DataTable resulBusqueda = clsReciboListaProductos.ReciboListaProductos_BuscarFolio();

            if(resulBusqueda.Rows.Count  == 1 )
            {
                DataRow filaUnica = (resulBusqueda.AsEnumerable()).Single<DataRow>();
                bool ActivoONo = filaUnica.Field<bool>("Activo") == true ? true : false;
                return (ActivoONo);
            }

            else
            { return (false);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Esta usted seguro que desea continuar?", "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (respuesta == DialogResult.Yes)
                {
                    if(SePuedeCancelarFolioReciboListaProductosController(textBox1.Text))
                    {
                        //Proceder a cancelarlo
                        string res = CancelarFolioReciboListaProductosController(textBox1.Text, ClsLogin.Id);
                        if (res.Contains("ok"))
                        {
                            StringBuilder mensaje = new StringBuilder();
                            mensaje.Append("Folio ");
                            mensaje.Append(textBox1.Text);
                            mensaje.Append(" cancelado");
                            MessageBox.Show(mensaje.ToString(), "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            textBox1.Text = "";
                        }

                        else
                        {
                            MessageBox.Show(res, "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }

                    else
                    {
                        MessageBox.Show("El folio " + textBox1.Text + " no se encuentra disponible" , "Reglas de operación", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
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
