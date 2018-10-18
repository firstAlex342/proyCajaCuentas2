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
    public partial class FrmReporteIngresosEgresos : Form
    {

        //-----------------------constructor
        public FrmReporteIngresosEgresos()
        {
            InitializeComponent();
        }

        //------------------Methods
        private DataTable MovsEnCaja_SumarPagoDeAfiliacionesController(DateTime fechaInicio, DateTime fechaFin)
        {
            ClsMovsEnCaja clsMovsEnCaja = new ClsMovsEnCaja();
            clsMovsEnCaja.FechaAlta = fechaInicio;
            clsMovsEnCaja.FechaModificacion = fechaFin;

            return (clsMovsEnCaja.MovsEnCaja_SumarPagoDeAfiliaciones());
        }

        private DataTable MovsEnCaja_SumarPagoDeTodosProductosController(DateTime fechaInicio, DateTime fechaFin)
        {
            ClsMovsEnCaja clsMovsEnCaja = new ClsMovsEnCaja();
            clsMovsEnCaja.FechaAlta = fechaInicio;
            clsMovsEnCaja.FechaModificacion = fechaFin;

            return (clsMovsEnCaja.MovsEnCaja_SumarPagoDeTodosProductos());
        }

        private DataTable Cheque_RecuperarDetallesDeChequesCapturadosActivosController(DateTime fechaInicio, DateTime fechaFin)
        {
            ClsCheque clsCheque = new ClsCheque();
            clsCheque.FechaAlta = fechaInicio;
            clsCheque.FechaModificacion = fechaFin;

            return (clsCheque.Cheque_RecuperarDetallesDeChequesCapturadosActivos());
        }

        private DataTable Cheque_RecuperarDetallesDeChequesNoCobradosController(DateTime fechaInicio, DateTime fechaFin)
        {
            ClsCheque clsCheque = new ClsCheque();
            clsCheque.FechaAlta = fechaInicio;
            clsCheque.FechaModificacion = fechaFin;

            return (clsCheque.Cheque_RecuperarDetallesDeChequesNoCobrados());
        }

       private DataTable Cheque_RecuperarDetallesDeChequesCobradosController(DateTime fechaInicio, DateTime fechaFin)
       {
            ClsCheque clsCheque = new ClsCheque();
            clsCheque.FechaAlta = fechaInicio;
            clsCheque.FechaModificacion = fechaFin;

            return (clsCheque.Cheque_RecuperarDetallesDeChequesCobrados());
       }

        //--------------------Utils
        private void MostrarEnTextBoxSumaDeAfiliaciones(DataTable tabla)
        {
            DataRow filaUnica = tabla.Rows[0];
            decimal sumaDeAfiliaciones;
            bool sePudoExtraerLaSuma;

            //la tabla contiene una unica fila con un valor diferente de 0 o null
            sePudoExtraerLaSuma = Decimal.TryParse(filaUnica["suma de afiliaciones"].ToString(), out sumaDeAfiliaciones);

            if (sePudoExtraerLaSuma)
                textBox1.Text = sumaDeAfiliaciones.ToString();
            else
                textBox1.Text = "0";
        }


        private void MostrarEnTextBoxSumaDeTodosLosProductos(DataTable tabla)
        {
            DataRow filaUnica = tabla.Rows[0];
            decimal sumaTodosLosProductos;
            bool sePudoExtraerLaSuma;

            //la tabla contiene una unica fila con un valor diferente de 0 ó null
            sePudoExtraerLaSuma = Decimal.TryParse(filaUnica["suma del pago de todos los productos"].ToString(), out sumaTodosLosProductos);

            if (sePudoExtraerLaSuma)
                textBox2.Text = sumaTodosLosProductos.ToString();
            else
                textBox2.Text = "0";
        }

        private void MostrarEnTextBoxSumaDeTodosLosCheques(DataTable tabla)
        {
            Func<DataRow, decimal> funcion = item =>
            {
                return item.Field<decimal>("Importe");
            };

            decimal sumaDeConceptos = tabla.AsEnumerable().Sum(funcion);

            textBox3.Text = sumaDeConceptos.ToString();
        }

        private void MostrarEnTextBoxSumaDeTodosLosChequeNoCobrados(DataTable tabla)
        {
            Func<DataRow, decimal> funcion = item =>
            {
                return (item.Field<decimal>("Importe"));
            };

            textBox5.Text = tabla.AsEnumerable().Sum(funcion).ToString();
        }


        private void MostrarEnTextBoxSumaDeChequesCobradosDePeriodosAnteriores(DataTable tabla)
        {
            Func<DataRow, decimal> funcion = item => {
                return (item.Field<decimal>("Importe"));
            };

            textBox4.Text = tabla.AsEnumerable().Sum(funcion).ToString();
        }


        private void MostrarEnTextBoxSaldoRealDeRetirosEstadoDeCuenta()
        {
            //textBox3 + textBox4 - textBox5
            decimal suma = Decimal.Parse(textBox3.Text) + Decimal.Parse(textBox4.Text) - Decimal.Parse(textBox5.Text);
            textBox6.Text = suma.ToString();
        }

        private void MostrarEnTextBoxSaldoDisponibleEnCuentaBancaria()
        {
            decimal suma = Decimal.Parse(textBox2.Text) + Decimal.Parse(textBox6.Text);
            textBox7.Text = (Decimal.Parse(textBox1.Text) - suma).ToString();
        }

        //----------------------Events
        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                DateTime fechaInicio;
                DateTime fechaFin;

                fechaInicio = new DateTime(2018, 10, 1, 0, 1, 0);
                fechaFin = new DateTime(2018, 10, 30, 23, 59, 58);
                DataTable res = MovsEnCaja_SumarPagoDeAfiliacionesController(fechaInicio, fechaFin);
                MostrarEnTextBoxSumaDeAfiliaciones(res);

                DataTable res2 = MovsEnCaja_SumarPagoDeTodosProductosController(fechaInicio, fechaFin);
                MostrarEnTextBoxSumaDeTodosLosProductos(res2);

                DataTable res3 = Cheque_RecuperarDetallesDeChequesCapturadosActivosController(fechaInicio, fechaFin);
                MostrarEnTextBoxSumaDeTodosLosCheques(res3);

                DateTime fechaCentinelaInicio = new DateTime(2000, 1, 1, 0, 1, 0);
                DateTime fechaCentinelaFin = new DateTime(2018, 9, 30, 23, 59, 58);
                DataTable res4 = Cheque_RecuperarDetallesDeChequesCobradosController(fechaCentinelaInicio, fechaCentinelaFin);
                MostrarEnTextBoxSumaDeChequesCobradosDePeriodosAnteriores(res4);


                DataTable res5 = Cheque_RecuperarDetallesDeChequesNoCobradosController(fechaInicio, fechaFin);
                MostrarEnTextBoxSumaDeTodosLosChequeNoCobrados(res5);
                MostrarEnTextBoxSaldoRealDeRetirosEstadoDeCuenta();

                MostrarEnTextBoxSaldoDisponibleEnCuentaBancaria();

            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }
    }
}
