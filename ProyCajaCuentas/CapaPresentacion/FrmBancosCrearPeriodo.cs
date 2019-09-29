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
    public partial class FrmBancosCrearPeriodo : Form
    {
        //--------------Constructor
        public FrmBancosCrearPeriodo()
        {
            InitializeComponent();
            CargarComboBoxAnios();
            CargarComBoBoxMeses();
        }

        //---------------Controllers Methods
        private string Bancos_createController(int anio, string mes, decimal disponibleEnBancos, decimal disponibleEnBancosReal, int idUsuarioOperador)
        {
            ClsBancos clsBancos = new ClsBancos();
            clsBancos.PeriodoAnio = anio;
            clsBancos.PeriodoMes = mes;
            clsBancos.DisponibleEnBancos = disponibleEnBancos;
            clsBancos.DisponibleEnBancosReal = disponibleEnBancosReal;
            clsBancos.IdUsuarioAlta = idUsuarioOperador;

            return (clsBancos.Bancos_create());
        }

        //---------------Utils
        private void CargarComboBoxAnios()
        {
            List<int> anios = new List<int>();
            anios.Add(2018);
            anios.Add(2019);
            anios.Add(2020);
            anios.Add(2021);
            anios.Add(2022);
            anios.Add(2023);
            anios.Add(2024);
            anios.Add(2025);


            Action<int> delegateAction1 = item => {
                comboBox1.Items.Add(item);
            };

            anios.ForEach(delegateAction1);
        }

        private void CargarComBoBoxMeses()
        {
            List<string> meses = new List<string>();
            meses.Add("Enero");
            meses.Add("Febrero");
            meses.Add("Marzo");
            meses.Add("Abril");
            meses.Add("Mayo");
            meses.Add("Junio");
            meses.Add("Julio");
            meses.Add("Agosto");
            meses.Add("Septiembre");
            meses.Add("Octubre");
            meses.Add("Noviembre");
            meses.Add("Diciembre");

            Action<string> delegado2 = parametro => {
                comboBox2.Items.Add(parametro);
            };

            meses.ForEach(delegado2);
        }

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


        private bool SeSeleccionoAlgoDeComBoBox(ComboBox comboOrigen)
        {
            bool existeAlgoSeleccionado = comboOrigen.SelectedIndex >= 0 ? true : false;

            return (existeAlgoSeleccionado);
        }


        private void LimpiarGroupBox()
        {
            comboBox1.Items.Clear();
            comboBox2.Text = null;

            comboBox2.Items.Clear();
            comboBox2.Text = null;

            textBox1.Text = "";
            textBox2.Text = "";
        }
        //----------------------------Events

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult res = MessageBox.Show("¿Esta usted seguro que desea continuar?", "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    bool seSeleccionoAlgoDeComboBoxAnios = SeSeleccionoAlgoDeComBoBox(comboBox1);
                    bool seSeleccionoAlgoDeComBoBoxMeses = SeSeleccionoAlgoDeComBoBox(comboBox2);
                    bool seCapturoAlgoEnTextBox = TieneAlgoMasQueEspaciosEnBlanco(textBox1.Text);
                    bool seCapturoAlgoEnOtroTextBox = TieneAlgoMasQueEspaciosEnBlanco(textBox2.Text);

                    if (seSeleccionoAlgoDeComboBoxAnios && seSeleccionoAlgoDeComBoBoxMeses && seCapturoAlgoEnTextBox && seCapturoAlgoEnOtroTextBox)
                    {
                        decimal cantidadDecimal;
                        decimal otraCantidadDecimal;
                        bool cantidadEstaEnFormatoValido = Decimal.TryParse(textBox1.Text, out cantidadDecimal);
                        bool otraCantidadEstaEnFormatoValido = Decimal.TryParse(textBox2.Text, out otraCantidadDecimal);

                        if (cantidadEstaEnFormatoValido && otraCantidadEstaEnFormatoValido)
                        {
                            int anio = Int32.Parse(comboBox1.SelectedItem.ToString());
                            string mes = comboBox2.SelectedItem.ToString();
                            string mensaje = Bancos_createController(anio, mes, cantidadDecimal, otraCantidadDecimal, ClsLogin.Id);
                            if (mensaje.Contains("ok"))
                            {
                                StringBuilder strB = new StringBuilder();
                                strB.Append("Periodo ");
                                strB.Append(comboBox1.SelectedItem.ToString());
                                strB.Append(" ");
                                strB.Append(comboBox2.SelectedItem.ToString());
                                strB.Append(" agregado correctamente");
                                MessageBox.Show(strB.ToString(), "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                LimpiarGroupBox();
                                CargarComboBoxAnios();
                                CargarComBoBoxMeses();
                            }

                            else
                            {
                                MessageBox.Show(mensaje, "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }
                    }

                    else
                    {
                        MessageBox.Show("Seleccione un año, un mes y escriba una cantidad para el periodo", "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }


            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LimpiarGroupBox();
            CargarComboBoxAnios();
            CargarComBoBoxMeses();
        }

        private void FrmBancosCrearPeriodo_Load(object sender, EventArgs e)
        {

        }
    }
}
