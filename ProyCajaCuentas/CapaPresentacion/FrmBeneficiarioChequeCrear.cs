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
    public partial class FrmBeneficiarioChequeCrear : Form
    {
        //---------------constructor
        public FrmBeneficiarioChequeCrear()
        {
            InitializeComponent();
        }

        //------------controllers
        private string BeneficiarioCheque_createController(string nombre, int idUsuarioOperador)
        {
            ClsBeneficiarioCheque clsBeneficiarioCheque = new ClsBeneficiarioCheque();
            clsBeneficiarioCheque.Nombre = nombre;
            clsBeneficiarioCheque.IdUsuarioAlta = idUsuarioOperador;

            return (clsBeneficiarioCheque.BeneficiarioCheque_create());
        }

        //---------------Methods
        private bool TieneAlgoMasQueEspaciosEnBlanco(string texto)
        {
            string cad = texto.Trim();

            bool res = cad.Length > 0 ? true : false;
            return (res);
        }

        //-------------------Events
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult res = MessageBox.Show("¿Esta usted seguro que desea continuar?", "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    bool seCapturoBeneficiario = TieneAlgoMasQueEspaciosEnBlanco(textBox1.Text);

                    if(seCapturoBeneficiario)
                    {
                        string mensaje = BeneficiarioCheque_createController(textBox1.Text.Trim(), ClsLogin.Id);
                        if (mensaje.Contains("ok"))
                        {
                            MessageBox.Show("Registros guardados exitosamente", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            textBox1.Text = String.Empty;
                        }

                        else
                        {
                            MessageBox.Show(mensaje, "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }

                    else
                    {
                        MessageBox.Show("Se requiere la captura de un beneficiario", "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                  
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
