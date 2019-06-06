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
    public partial class FrmChequesCobradosDePeriodosAnterioresTotalInicial : Form
    {
        //----------------------Constructor
        public FrmChequesCobradosDePeriodosAnterioresTotalInicial()
        {
            InitializeComponent();
            DataTable res = InicialTotalDeChequesCobradosDePeriodosAnteriores_BuscarActivoController();
            MostrarInicialTotalDeChequesCobradosDePeriodosAnteriores(textBox1, res);
        }

        //---------------------Controllers
        private DataTable InicialTotalDeChequesCobradosDePeriodosAnteriores_BuscarActivoController()
        {
            ClsInicialTotalDeChequesCobradosDePeriodosAnteriores clsInicialTotalDeChequesCobradosDePeriodosAnteriores;
            clsInicialTotalDeChequesCobradosDePeriodosAnteriores = new ClsInicialTotalDeChequesCobradosDePeriodosAnteriores();

            return (clsInicialTotalDeChequesCobradosDePeriodosAnteriores.InicialTotalDeChequesCobradosDePeriodosAnteriores_BuscarActivo());
        }

        private string InicialTotalDeChequesCobradosDePeriodosAnteriores_UpdateController(decimal total, int idUsuarioOperador)
        {
            ClsInicialTotalDeChequesCobradosDePeriodosAnteriores clsInicialTotalDeChequesCobradosDePeriodosAnteriores;
            clsInicialTotalDeChequesCobradosDePeriodosAnteriores = new ClsInicialTotalDeChequesCobradosDePeriodosAnteriores();

            clsInicialTotalDeChequesCobradosDePeriodosAnteriores.Total = total;
            clsInicialTotalDeChequesCobradosDePeriodosAnteriores.IdUsuarioModifico = idUsuarioOperador;
            return (clsInicialTotalDeChequesCobradosDePeriodosAnteriores.InicialTotalDeChequesCobradosDePeriodosAnteriores_Update());
        }


        //----------------------Methods
        private void MostrarInicialTotalDeChequesCobradosDePeriodosAnteriores(TextBox destinoTextBox, DataTable inicialTotalDeChequesCobradosDePeriodosAnteriores)
        {
            var res = inicialTotalDeChequesCobradosDePeriodosAnteriores.AsEnumerable();
            List<DataRow> listaElementos = res.ToList<DataRow>();  //Esa lista solo contiene un elemento o esta vacia

            DataRow filaUnica = listaElementos.SingleOrDefault<DataRow>();
            if (filaUnica != null)
                destinoTextBox.Text = (filaUnica.Field<decimal>("Total")).ToString();
            else
                destinoTextBox.Text = "0.0000";
        }

        //----------------------Events
        private void button1_Click(object sender, EventArgs e)
        {
           try
           {
                DialogResult res = MessageBox.Show("¿Esta usted seguro que desea continuar?", "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    decimal cantidadDecimal;
                    bool cantidadEstaEnFormatoValido = Decimal.TryParse(textBox1.Text, out cantidadDecimal);
                    if(cantidadEstaEnFormatoValido)
                    {
                        string mensaje = InicialTotalDeChequesCobradosDePeriodosAnteriores_UpdateController(cantidadDecimal, ClsLogin.Id);
                        if(mensaje.Contains("ok"))
                        {
                            MessageBox.Show("Registro guardado exitosamente", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        else
                        {
                            MessageBox.Show(mensaje, "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }

                    else
                    {
                        MessageBox.Show("Introduzca un valor númerico adecuado", "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
