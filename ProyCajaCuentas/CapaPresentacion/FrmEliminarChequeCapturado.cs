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
    public partial class FrmEliminarChequeCapturado : Form
    {
        //------------------constructor
        public FrmEliminarChequeCapturado()
        {
            InitializeComponent();
        }

        //--------------Methods
        public string Cheque_DescripcionDeCheque_ConceptoEnCheque_UpdateActivo(string numChequeACancelar)
        {
            ClsCheque clsCheque = new ClsCheque();
            clsCheque.NumCheque = numChequeACancelar;
            clsCheque.Activo = false;

            return ( clsCheque.Cheque_DescripcionDeCheque_ConceptoEnCheque_UpdateActivo() );
        }

        //--------------Utils


        //-------------------Events
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Esta usted seguro que desea continuar?", "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (respuesta == DialogResult.Yes)
                {
                   string mensaje= Cheque_DescripcionDeCheque_ConceptoEnCheque_UpdateActivo(textBox1.Text);
                   if(mensaje.Contains("ok"))
                    {
                        StringBuilder texto = new StringBuilder();
                        texto.Append("El cheque número ");
                        texto.Append(textBox1.Text);
                        texto.Append(" ha sido eliminado exitosamente");

                        MessageBox.Show(texto.ToString(), "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textBox1.Text = "";
                    }

                   else
                    {
                        MessageBox.Show(mensaje, "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                ClsMyException clsMyException = new ClsMyException();
                string res = clsMyException.FormarTextoDeSqlException(ex);

                MessageBox.Show(res, "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }
    }
}
