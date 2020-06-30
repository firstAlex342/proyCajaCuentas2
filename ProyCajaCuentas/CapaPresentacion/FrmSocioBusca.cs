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
    public partial class FrmSocioBusca : Form
    {
        //-----------------constructor
        public FrmSocioBusca()
        {
            InitializeComponent();
        }

        //------------------Methods
        private DataTable Socio_BuscarXLicenciaController(string licenciaBuscada)
        {
            ClsSocio clsSocio = new ClsSocio();
            clsSocio.NumeroLicencia = licenciaBuscada;
            return (clsSocio.Socio_BuscarXLicencia());
        }

        private DataTable Socio_BuscarXRFCPropietarioController(string rfcPropietarioBuscado)
        {
            ClsSocio clsSocio = new ClsSocio();
            clsSocio.RFCPropietario = rfcPropietarioBuscado;

            return (clsSocio.Socio_BuscarXRFCPropietario());
        }

        private DataTable Socio_BuscarXComodatarioController(string comodatarioBuscado)
        {
            ClsSocio clsSocio = new ClsSocio();
            clsSocio.Comodatario = comodatarioBuscado;
            return (clsSocio.Socio_BuscarXComodatario());
        }


        private DataTable Socio_BuscarXRFCComodatarioController(string rfcComodatarioBuscado)
        {
            ClsSocio clsSocio = new ClsSocio();
            clsSocio.RFCComodatario = rfcComodatarioBuscado;

            return (clsSocio.Socio_BuscarXRFCComodatario());
        }

        private DataTable Socio_BuscarXPropietarioPatenteController(string propietarioPatenteBuscado)
        {
            ClsSocio clsSocio = new ClsSocio();
            clsSocio.PropietarioPatente = propietarioPatenteBuscado;

            return (clsSocio.Socio_BuscarXPropietarioPatente());
        }

        private DataTable Socio_BuscarXDireccionController(string textoBuscado)
        {
            ClsSocio clsSocio = new ClsSocio();
            clsSocio.DireccionCalle = textoBuscado;
            clsSocio.DireccionComplemento = textoBuscado;
            clsSocio.DireccionLote = textoBuscado;
            clsSocio.DireccionManzana = textoBuscado;
            clsSocio.DireccionSupmza = textoBuscado;

            return (clsSocio.Socio_BuscarXDireccion());
        }

        private DataTable Socio_BuscarTodosActivosController()
        {
            ClsSocio clsSocio = new ClsSocio();
            return (clsSocio.Socio_BuscarTodosActivos());
        }

        //---------------Utils
        private void MostrarEnGridResultadoDeBusqueda(DataTable tabla)
        {
            dataGridView1.DataSource = tabla;

            dataGridView1.Columns[0].Visible = false;  //No mostrar el Id del Socio
            dataGridView1.Columns[15].Visible = false; //No mostrar IdUsuarioAlta
            dataGridView1.Columns[16].Visible = false;  //No mostrar fechaAlta
            dataGridView1.Columns[17].Visible = false;  //No mostrar idUsuarioModifico
            dataGridView1.Columns[18].Visible = false;  //No mostrar fehaModificacion
            dataGridView1.Columns[19].Visible = false; //No mostrar Activo

            dataGridView1.ClearSelection();
            dataGridView1.CurrentCell = null;

            dataGridView1.Columns[1].HeaderText = "Numero licencia";
            dataGridView1.Columns[2].HeaderText = "Nombre comercial";
            dataGridView1.Columns[3].HeaderText = "Supermanzana";
            dataGridView1.Columns[4].HeaderText = "Manzana";
            dataGridView1.Columns[5].HeaderText = "Lote";
            dataGridView1.Columns[6].HeaderText = "Calle";
            dataGridView1.Columns[7].HeaderText = "Complemento";
            dataGridView1.Columns[8].HeaderText = "Propietario patente";
            dataGridView1.Columns[9].HeaderText = "RFC propietario";
            dataGridView1.Columns[10].HeaderText = "Comodatario";
            dataGridView1.Columns[11].HeaderText = "RFC comodatario";
            dataGridView1.Columns[12].HeaderText = "Telefono";
            dataGridView1.Columns[13].HeaderText = "Celular";
            dataGridView1.Columns[14].HeaderText = "Correo electrónico";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButton1.Checked)
                {
                    //buscar por direccion
                    DataTable respuesta = Socio_BuscarXDireccionController(textBox1.Text);
                    MostrarEnGridResultadoDeBusqueda(respuesta);
                    label2.Text = "Resultado de búsqueda: " + respuesta.Rows.Count.ToString();

                }

                else if (radioButton2.Checked)
                {
                    //Buscar por propietario patente
                    DataTable respuesta = Socio_BuscarXPropietarioPatenteController(textBox1.Text);
                    MostrarEnGridResultadoDeBusqueda(respuesta);
                    label2.Text = "Resultado de búsqueda: " + respuesta.Rows.Count.ToString();

                }

                else if (radioButton3.Checked)
                {
                    //Buscar por RFC Propietario
                    DataTable respuesta = Socio_BuscarXRFCPropietarioController(textBox1.Text);
                    MostrarEnGridResultadoDeBusqueda(respuesta);
                    label2.Text = "Resultado de búsqueda: " + respuesta.Rows.Count.ToString();

                }

                else if (radioButton4.Checked)
                {
                    //buscar por comodatario
                    DataTable respuesta = Socio_BuscarXComodatarioController(textBox1.Text);
                    MostrarEnGridResultadoDeBusqueda(respuesta);
                    label2.Text = "Resultado de búsqueda: " + respuesta.Rows.Count.ToString();

                }

                else if (radioButton5.Checked)
                {
                    //buscar por rfc comodatario
                    DataTable respuesta = Socio_BuscarXRFCComodatarioController(textBox1.Text);
                    MostrarEnGridResultadoDeBusqueda(respuesta);
                    label2.Text = "Resultado de búsqueda: " + respuesta.Rows.Count.ToString();

                }

                else if (radioButton6.Checked)
                {
                    //buscar por licencia
                    DataTable respuesta = Socio_BuscarXLicenciaController(textBox1.Text);
                    MostrarEnGridResultadoDeBusqueda(respuesta);
                    label2.Text = "Resultado de búsqueda: " + respuesta.Rows.Count.ToString();
                }

                else if (radioButton7.Checked)
                {
                    //buscar todos
                    DataTable respuesta = Socio_BuscarTodosActivosController();
                    dataGridView1.DataSource = respuesta;
                    label2.Text = "Resultado de búsqueda: " + respuesta.Rows.Count.ToString();
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
