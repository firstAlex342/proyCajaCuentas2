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
            PersonalizarMiDateTimePicker();

            try
            {
                DataTable res = InicialTotalDeChequesCobradosDePeriodosAnteriores_BuscarActivoController();
                MostrarInicialTotalDeChequesCobradosDePeriodosAnteriores(textBox1, dateTimePicker1, res);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }

        //---------------------Controllers
        private DataTable InicialTotalDeChequesCobradosDePeriodosAnteriores_BuscarActivoController()
        {
            ClsInicialTotalDeChequesCobradosDePeriodosAnteriores clsInicialTotalDeChequesCobradosDePeriodosAnteriores;
            clsInicialTotalDeChequesCobradosDePeriodosAnteriores = new ClsInicialTotalDeChequesCobradosDePeriodosAnteriores();

            return (clsInicialTotalDeChequesCobradosDePeriodosAnteriores.InicialTotalDeChequesCobradosDePeriodosAnteriores_BuscarActivo());
        }

        private string InicialTotalDeChequesCobradosDePeriodosAnteriores_UpdateController(decimal total, DateTime fechaDePeriodoInicial, int idUsuarioOperador)
        {
            ClsInicialTotalDeChequesCobradosDePeriodosAnteriores clsInicialTotalDeChequesCobradosDePeriodosAnteriores;
            clsInicialTotalDeChequesCobradosDePeriodosAnteriores = new ClsInicialTotalDeChequesCobradosDePeriodosAnteriores();

            clsInicialTotalDeChequesCobradosDePeriodosAnteriores.Total = total;
            clsInicialTotalDeChequesCobradosDePeriodosAnteriores.FechaDePeriodoInicial = fechaDePeriodoInicial;
            clsInicialTotalDeChequesCobradosDePeriodosAnteriores.IdUsuarioModifico = idUsuarioOperador;
            return (clsInicialTotalDeChequesCobradosDePeriodosAnteriores.InicialTotalDeChequesCobradosDePeriodosAnteriores_Update());
        }


        //----------------------Methods
        private void MostrarInicialTotalDeChequesCobradosDePeriodosAnteriores(TextBox destinoTextBox, DateTimePicker fechaDePeriodoInicialDateTimePicker, DataTable inicialTotalDeChequesCobradosDePeriodosAnteriores)
        {
            var res = inicialTotalDeChequesCobradosDePeriodosAnteriores.AsEnumerable();
            List<DataRow> listaElementos = res.ToList<DataRow>();  //Esa lista solo contiene un elemento o esta vacia

            DataRow filaUnica = listaElementos.SingleOrDefault<DataRow>();
            if (filaUnica != null)
            {
                destinoTextBox.Text = (filaUnica.Field<decimal>("Total")).ToString();
                fechaDePeriodoInicialDateTimePicker.Value = filaUnica.Field<DateTime>("FechaDePeriodoInicial");
            }

            else
            {
                destinoTextBox.Text = "0.0000";
                fechaDePeriodoInicialDateTimePicker.Value = DateTimePicker.MinimumDateTime;
            }
        }

        private void PersonalizarMiDateTimePicker()
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/yyyy";
            dateTimePicker1.ShowUpDown = true;
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
                        string mensaje = InicialTotalDeChequesCobradosDePeriodosAnteriores_UpdateController(cantidadDecimal, dateTimePicker1.Value, ClsLogin.Id);
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
