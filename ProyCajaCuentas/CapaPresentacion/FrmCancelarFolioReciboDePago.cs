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
        private string CancelarFolioReciboListaProductosController(string folioBuscado)
        {
            ClsReciboListaProductos clsReciboListaProductos = new ClsReciboListaProductos();
            clsReciboListaProductos.Folio = folioBuscado;

            return (clsReciboListaProductos.ReciboListaProductos_UpdateActivoACero());

        }

        private bool EstaCanceladoFolioReciboListaProductosController(string folioBuscado)
        {
            ClsReciboListaProductos clsReciboListaProductos = new ClsReciboListaProductos();
            clsReciboListaProductos.Folio = folioBuscado;
            DataTable resulBusqueda = clsReciboListaProductos.ReciboListaProductos_BuscarFolio();

            DataRow filaUnica = (resulBusqueda.AsEnumerable()).Single<DataRow>();

            bool ActivoONo = filaUnica.Field<bool>("Activo") == false ? true : false;

            return (ActivoONo);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Esta usted seguro que desea continuar?", "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (respuesta == DialogResult.Yes)
                {
                    if(EstaCanceladoFolioReciboListaProductosController(textBox1.Text))
                    {
                        MessageBox.Show("Ya esta cancelado el folio " + textBox1.Text , "Reglas de operación", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    }

                    else
                    {
                        //Proceder a cancelarlo
                        string res = CancelarFolioReciboListaProductosController(textBox1.Text);
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
                }
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }
    }
}
