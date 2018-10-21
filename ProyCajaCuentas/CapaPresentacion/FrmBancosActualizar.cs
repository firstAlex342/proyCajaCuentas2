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
    public partial class FrmBancosActualizar : Form
    {
        //--------------constructor
        public FrmBancosActualizar()
        {
            InitializeComponent();

            try
            {
                DataTable res = Bancos_SelectActivosController();
                MostrarEnGridPeriodos(res);
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }

        //------------methods controller
        private string Bancos_Update_DisponibleEnBancosController(int idBuscado, string mesPeriodo, 
            int anioPeriodo, decimal disponibleEnBancos, int idUsuarioOperador)
        {
            ClsBancos clsBancos = new ClsBancos();
            clsBancos.Id = idBuscado;
            clsBancos.PeriodoMes = mesPeriodo;
            clsBancos.PeriodoAnio = anioPeriodo;
            clsBancos.DisponibleEnBancos = disponibleEnBancos;
            clsBancos.IdUsuarioModifico = idUsuarioOperador;

            return (clsBancos.Bancos_Update_DisponibleEnBancos());
        }

        public DataTable Bancos_SelectActivosController()
        {
            ClsBancos clsBancos = new ClsBancos();
            return (clsBancos.Bancos_SelectActivos());
        }

        //-----------Utils
        private void MostrarEnGridPeriodos(DataTable origen)
        {
            dataGridView1.DataSource = origen;

            dataGridView1.Columns[1].HeaderText = "Mes";
            dataGridView1.Columns[2].HeaderText = "Año";
            dataGridView1.Columns[3].HeaderText = "Disponible en bancos";

            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[6].Visible = false;
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[8].Visible = false;
            dataGridView1.Columns[9].Visible = false;
        }


        private void LimpiarGroupBoxDetalles()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
        }
        //-----------------Events

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if( e.RowIndex >= 0)
            {
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells["Id"].EditedFormattedValue.ToString();
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["PeriodoAnio"].EditedFormattedValue.ToString();
                textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells["PeriodoMes"].EditedFormattedValue.ToString();
                textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells["DisponibleEnBancos"].EditedFormattedValue.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LimpiarGroupBoxDetalles();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult res = MessageBox.Show("¿Esta usted seguro que desea continuar?", "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    int id = Int32.Parse(textBox1.Text);
                    int anio = Int32.Parse(textBox2.Text);
                    string mes = textBox3.Text;
                    decimal disponibleEnBancos = Decimal.Parse(textBox4.Text);

                    string mensaje = Bancos_Update_DisponibleEnBancosController(id, mes, anio,
                        disponibleEnBancos, ClsLogin.Id);

                    if (mensaje.Contains("ok"))
                    {
                        string cadena = "Periodo " + textBox2.Text + " " + textBox3.Text + " actualizado correctamente";
                        MessageBox.Show(cadena, "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpiarGroupBoxDetalles();
                        DataTable tabla = Bancos_SelectActivosController();
                        MostrarEnGridPeriodos(tabla);
                    }

                    else
                    {
                        MessageBox.Show(mensaje, "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
