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
    public partial class FrmRealizarCobro : Form
    {

        //-----------Properties
        private int IdCajaDelDia { set; get; }
        private DataTable TarifasDeTodosProductos { set; get; }


        //--------------Constructor
        public FrmRealizarCobro()
        {
            InitializeComponent();

            try
            {
                InicializarIdCajaDelDiaController();
                MostrarProductos( Producto_Select_Id_Nombre_DeTodosController());
                this.TarifasDeTodosProductos = ProductoPosee_innerjoin_Tarifas_DeTodosController();                

            }

            catch(Exception ex)
            {
                tableLayoutPanel1.Enabled = false;
                MessageBox.Show(ex.Message + " " + ex.Source);
            }
        }

        //--------------Methods
        private DataTable OperaCaja_BuscarCajasDelDiaDelUsuarioController(int idUsuarioOperador)
        {
            ClsOperaCaja clsOperaCaja = new ClsOperaCaja();
            clsOperaCaja.IdUsuario = ClsLogin.Id;

            return (clsOperaCaja.OperaCaja_BuscarCajasDelDiaDelUsuario());
        }


        private int Caja_CreateController(int idUsuarioOperador)
        {
            ClsCaja clsCaja = new ClsCaja();
            clsCaja.IdUsuarioAlta = idUsuarioOperador;

            DataTable res = clsCaja.Caja_create();  
            var coleccion = from s in res.AsEnumerable()
                            select s;

            DataRow unicaFila = coleccion.First();
            int idCajaNueva = unicaFila.Field<int>(0);
            return (idCajaNueva);
        }


        private string OperaCaja_CreateController(int idUsuarioOperador, int idCaja)
        {
            ClsOperaCaja clsOperaCaja = new ClsOperaCaja();
            clsOperaCaja.IdUsuario = idUsuarioOperador;
            clsOperaCaja.IdCaja = idCaja;
            clsOperaCaja.IdUsuarioAlta = idUsuarioOperador;

            return (clsOperaCaja.OperaCaja_create());
        }

        private DataTable Producto_Select_Id_Nombre_DeTodosController()
        {
            ClsProducto clsProducto = new ClsProducto();
            return (clsProducto.Producto_Select_Id_Nombre_DeTodos());
        }



        private void InicializarIdCajaDelDiaController()
        {
            DataTable res = OperaCaja_BuscarCajasDelDiaDelUsuarioController(ClsLogin.Id);

            if (res.Rows.Count == 1)
            {
                //En res esta la caja del dia 
                var coleccion = from s in res.AsEnumerable()
                                select s;

                DataRow unicaFila = coleccion.First();
                this.IdCajaDelDia = unicaFila.Field<int>(1);   //En la columna 1 de res..
            }

            else if (res.Rows.Count > 1)
            {
                string mensajeError;
                mensajeError = "Un usuario no puede tener 2 cajas con la misma fecha de creación en la tabla OperaCaja, es decir en un dia solo puede tener una caja";
                throw new ArgumentException(mensajeError);
            }

            else
            {
                //El dia de hoy aún no tiene caja aún por lo tanto se le crea
                int idCajaNueva = Caja_CreateController(ClsLogin.Id);
                OperaCaja_CreateController(ClsLogin.Id, idCajaNueva);
                this.IdCajaDelDia = idCajaNueva;
            }
        }

        
        private DataTable ProductoPosee_innerjoin_Tarifas_DeTodosController()
        {
            ClsProductoPosee clsProductoPosee = new ClsProductoPosee();
            return (clsProductoPosee.ProductoPosee_innerjoin_Tarifas_DeTodos());
        }


        //------------Utils
        private void MostrarProductos(DataTable misProductos)
        {
            List<ClsProductoViewModel> lista = new List<ClsProductoViewModel>();

            var coleccion = from s in misProductos.AsEnumerable()
                            select new ClsProductoViewModel { Id= s.Field<int>("Id"),
                                Nombre =  s.Field<string>("Nombre"), CampoAMostrar = "Nombre" }; 

            lista = coleccion.ToList<ClsProductoViewModel>();

            foreach(var item in lista)
                listBox1.Items.Add(item);
           
        }

        //--------------------Events
        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if( listBox1.SelectedIndex > -1)
            {
                listBox2.SelectedItem = null;
                listBox2.Items.Clear();
                textBox3.Text = "";
                textBox2.Text = "";

                ClsProductoViewModel clsProductoViewModel = (ClsProductoViewModel)listBox1.SelectedItem;
                List<decimal> listaTarifas = new List<decimal>();

                var res = from s in TarifasDeTodosProductos.AsEnumerable()
                          where s.Field<int>("IdProducto") == clsProductoViewModel.Id
                          select s.Field<decimal>("Cantidad");

                listaTarifas = res.ToList<decimal>();

                foreach (var item in listaTarifas)
                    listBox2.Items.Add(item);

            }

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox2.SelectedIndex > -1)
            {
                decimal tarifaElegida = (decimal)listBox2.SelectedItem;
                textBox3.Text = tarifaElegida.ToString();
                textBox2.Text = "";
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            bool listBox1Seleccionado = listBox1.SelectedIndex > -1 ? true : false; //Averiguar si esta seleccionado
            bool listBox2Seleccionado = listBox2.SelectedIndex > -1 ? true : false; //Averiguar si esta seleccionado
            bool textBox3ConContenido = textBox3.Text.Length > 0 ? true : false; // Averiguar si tiene contenido

            if(listBox1Seleccionado && listBox2Seleccionado && textBox3ConContenido)
            {
                if (textBox2.Text.Length > 0)
                {
                    decimal descuento = 0.0m;
                    bool esNumero = Decimal.TryParse(textBox2.Text, out descuento);

                    if(esNumero)
                    {
                        decimal tarifaElegida = (decimal)listBox2.SelectedItem;

                        if (descuento >= 0m && descuento <= tarifaElegida)
                        {
                            decimal resta = tarifaElegida - descuento;
                            textBox3.Text = resta.ToString();
                        }
                        else
                        {
                            MessageBox.Show("El descuento debe ser menor ó igual a la tarifa seleccionada", "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }

                    else
                    {
                        MessageBox.Show("Entrada no numerica", "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                }

                else
                {
                    textBox3.Text = ((decimal)listBox2.SelectedItem).ToString();
                }
            }

        }
    }

    public class ClsProductoViewModel
    {
        //-----------Properties
        public int Id { set; get; }
        public string Nombre { set; get; }
        public string Descripcion { set; get; }
        public int IdUsuarioAlta { set; get; }
        public DateTime FechaAlta { set; get; }
        public int IdUsuarioModifico { set; get; }
        public DateTime FechaModificacion { set; get; }
        public bool Activo { set; get; }

        public string CampoAMostrar { set; get; }

        //---------------Constructor
        public ClsProductoViewModel()
        {
            this.Id = 0;
            this.Nombre = String.Empty;
            this.Descripcion = String.Empty;
            this.IdUsuarioAlta = 0;
            this.FechaAlta = new DateTime();
            this.IdUsuarioModifico = 0;
            this.FechaModificacion = new DateTime();
            this.Activo = true;

            this.CampoAMostrar = String.Empty;
        }

        //----------------Methods
        public override string ToString()
        {
            if (this.CampoAMostrar.Equals("Nombre"))
                return (this.Nombre);

            else
            {
                string texto;
                texto = " Id = " + this.Id.ToString();
                texto += " Nombre = " + this.Nombre;
                texto += " Descripcion = " + this.Descripcion;
                texto += " IdUsuarioAlta = " + this.IdUsuarioAlta;
                texto += " FechaAlta = " + this.FechaAlta.ToString();
                texto += " IdUsuarioModifico = " + this.IdUsuarioModifico.ToString();
                texto += " FechaModificacion = " + this.FechaModificacion.ToString();
                texto += " Activo = " + this.Activo.ToString();
                return (texto);
            }
        }
    }
}
