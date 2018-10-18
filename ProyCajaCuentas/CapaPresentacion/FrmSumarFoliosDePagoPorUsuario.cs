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
    public partial class FrmSumarFoliosDePagoPorUsuario : Form
    {

        //--------------------Constructor
        public FrmSumarFoliosDePagoPorUsuario()
        {
            InitializeComponent();
        }

        //---------------------Methods
        private DataTable Usuario_SumarContenidosDeFolioController (DateTime fechaInicio, DateTime fechaFin)
        {
            ClsUsuario clsUsuario = new ClsUsuario();
            clsUsuario.FechaAlta = fechaInicio;
            clsUsuario.FechaModificacion = fechaFin;

            return (clsUsuario.Usuario_SumarContenidosDeFolio());
        }

        private void MostrarContenidoEnGrid(DataTable tabla)
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = tabla;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[2].Visible = false;
        }

        //---------------------Utils


        //--------------------Events
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fechaInicio;
                DateTime fechaFin;

                fechaInicio = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month,
    dateTimePicker1.Value.Day, 0, 1, 0);

                fechaFin = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month,
                    dateTimePicker1.Value.Day, 23, 59, 58);

                DataTable tabla = Usuario_SumarContenidosDeFolioController(fechaInicio, fechaFin);
                MostrarContenidoEnGrid(tabla);
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }
    }
}
