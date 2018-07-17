using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using MetroFramework;
using CapaLogicaNegocios;

namespace CapaPresentacion
{
    public partial class FrmSocioBuscar : MetroForm
    {

        //-----------------------Constructor
        public FrmSocioBuscar()
        {
            InitializeComponent();
            //this.MinimumSize = this.Size;    si yo habilito solo minimum size la venta al
            //this.MaximumSize = this.Size;     cerrarse con el boton X, la ventana activa pasa a ser una que no es la aplicacion
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

        //---------------Utils
        private void MostrarEnGridResultadoDeBusqueda(DataTable tabla)
        {
            metroGrid1.DataSource = tabla;

            metroGrid1.Columns[0].Visible = false;  //No mostrar el Id del Socio
            metroGrid1.Columns[15].Visible = false; //No mostrar IdUsuarioAlta
            metroGrid1.Columns[16].Visible = false;  //No mostrar fechaAlta
            metroGrid1.Columns[17].Visible = false;  //No mostrar idUsuarioModifico
            metroGrid1.Columns[18].Visible = false;  //No mostrar fehaModificacion

            metroGrid1.ClearSelection();
            metroGrid1.CurrentCell = null;

            metroGrid1.Columns[1].HeaderText = "Numero licencia";
            metroGrid1.Columns[2].HeaderText = "Nombre comercial";
            metroGrid1.Columns[3].HeaderText = "Supermanzana";
            metroGrid1.Columns[4].HeaderText = "Manzana";
            metroGrid1.Columns[5].HeaderText = "Lote";
            metroGrid1.Columns[6].HeaderText = "Calle";
            metroGrid1.Columns[7].HeaderText = "Complemento";
            metroGrid1.Columns[8].HeaderText = "Propietario patente";
            metroGrid1.Columns[9].HeaderText = "RFC propietario";
            metroGrid1.Columns[10].HeaderText = "Comodatario";
            metroGrid1.Columns[11].HeaderText = "RFC comodatario";
            metroGrid1.Columns[12].HeaderText = "Telefono";
            metroGrid1.Columns[13].HeaderText = "Celular";
            metroGrid1.Columns[14].HeaderText = "Correo electrónico";
        }



        //-------------------------Events
        private void FrmSocioBuscar_Load(object sender, EventArgs e)
        {

        }


        private void metroButton1_Click(object sender, EventArgs e)
        {
            if(metroRadioButton1.Checked)
            {
                //buscar por direccion
                DataTable respuesta = Socio_BuscarXDireccionController(metroTextBox1.Text);
                MostrarEnGridResultadoDeBusqueda(respuesta);
            }

            else if(metroRadioButton2.Checked)
            {
                //Buscar por propietario patente
                DataTable respuesta = Socio_BuscarXPropietarioPatenteController(metroTextBox1.Text);
                MostrarEnGridResultadoDeBusqueda(respuesta);
            }

            else if(metroRadioButton3.Checked)
            {
                //Buscar por RFC Propietario
                DataTable respuesta = Socio_BuscarXRFCPropietarioController(metroTextBox1.Text);
                MostrarEnGridResultadoDeBusqueda(respuesta);
            }

            else if(metroRadioButton4.Checked)
            {
                //buscar por comodatario
                DataTable respuesta = Socio_BuscarXComodatarioController(metroTextBox1.Text);
                MostrarEnGridResultadoDeBusqueda(respuesta);
            }

            else if(metroRadioButton5.Checked)
            {
                //buscar por rfc comodatario
                DataTable respuesta = Socio_BuscarXRFCComodatarioController(metroTextBox1.Text);
                MostrarEnGridResultadoDeBusqueda(respuesta);
            }

            else if(metroRadioButton6.Checked)
            {
                //buscar por licencia
                DataTable respuesta = Socio_BuscarXLicenciaController(metroTextBox1.Text);
                MostrarEnGridResultadoDeBusqueda(respuesta);
            }
        }
    }
}
