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
    public partial class FrmRaiz2 : Form
    {

        public  FrmRaiz2()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;

            CargarPanelesDePestanias();
            CargarComBoBox();
            comboBox1.SelectedIndex = 0;
            DeshabilitarDeMenuOpciones();
            MostrarUsuarioDeSesionEnMetroLabel();
        }


        //-------------Methods controller
        public bool EsActivoModuloController(int idModuloBuscado)
        {
            var lista = ClsLogin.ModulosALosQueTieneAccesoUsuario.AsEnumerable();
            DataRow filaBuscada = lista.FirstOrDefault(s =>
            (s.Field<int>(1) == idModuloBuscado) && (s.Field<bool>(2) == true));

            if (filaBuscada != null)
            {
                return true;
            }

            else
                return false;
        }



        //---------------------Methods
        private void MostrarUsuarioDeSesionEnMetroLabel()
        {
            DataRow filaUnica = (ClsLogin.Usuario).AsEnumerable().Single();
            label1.Text = "Usuario: " + filaUnica.Field<string>("Usuario");
        }

        private void AbrirFormulario(Form formHijo)
        {
            SetFormACajaDeDialogModal(formHijo);
            formHijo.ShowDialog(this);
            formHijo.Dispose();
        }

        private void SetFormACajaDeDialogModal(Form formulario)
        {           
            formulario.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            formulario.ShowInTaskbar = false;
            formulario.MinimumSize = formulario.Size;
            formulario.MaximumSize = formulario.Size;
            formulario.StartPosition = FormStartPosition.CenterScreen;
        }

        private void CargarPanelesDePestanias()
        {
            CargarPanelConForm(panel3, new FrmSocioAdd());
            CargarPanelConForm(panel4, new FrmSocioActualizar());
            CargarPanelConForm(panel5, new FrmSocioBusca());
            CargarPanelConForm(panel6, new FrmSocioImprimirExportar());
            CargarPanelConForm(panel7, new FrmRealizarCobro());
            CargarPanelConForm(panel8, new FrmCancelarFolioReciboDePago()); 
            CargarPanelConForm(panel9, new FrmBuscarFoliosDeSocio());  
            CargarPanelConForm(panel10, new FrmBuscarFoliosDeTodos()); 
            CargarPanelConForm(panel11, new FrmBuscarFoliosCanceladosDeTodos());
            CargarPanelConForm(panel12, new FrmSumarFoliosDePagoPorUsuario());
            CargarPanelConForm(panel13, new FrmSumarProductosPorProducto());
            CargarPanelConForm(panel14, new FrmBuscarFoliosActivosDeTodosImprimirExportar()); 
            CargarPanelConForm(panel15, new FrmBuscarFoliosActivosYCanceladosDeTodosImprimirExportar());
            CargarPanelConForm(panel16, new FrmBuscarFoliosActivosYCanceladosDeSocioImprimirExportar());
            CargarPanelConForm(panel17,new FrmSocioBuscarProductosPagadosYNoPagados());
            CargarPanelConForm(panel18, new FrmAgregarCheque());
            CargarPanelConForm(panel19, new FrmActualizarInfoCheque());
            CargarPanelConForm(panel20, new FrmEliminarChequeCapturado());
            CargarPanelConForm(panel21, new FrmVisorReporteChequesNoCobrados()); 
            CargarPanelConForm(panel22, new FrmVisorReporteChequesCobrados());
            CargarPanelConForm(panel23, new FrmVisorReporteChequesDePeriodo());
            CargarPanelConForm(panel24, new FrmProveedorAgregar());
            CargarPanelConForm(panel25, new FrmProveedorActualizar());
            CargarPanelConForm(panel26, new FrmProveedorEliminar());
            CargarPanelConForm(panel27, new FrmProveedoresVer());
            CargarPanelConForm(panel28, new FrmProveedorAgregarElemento()); 
            CargarPanelConForm(panel29, new FrmProveedorEditarElemento());
            CargarPanelConForm(panel30, new FrmBeneficiarioChequeCrear());
            CargarPanelConForm(panel31, new FrmBeneficiarioChequeEliminar());
            CargarPanelConForm(panel32, new FrmBeneficiarioChequeVer());  
            CargarPanelConForm(panel33, new FrmBancosCrearPeriodo());
            CargarPanelConForm(panel34,new FrmBancosActualizar());
            CargarPanelConForm(panel35, new FrmVisorReporteIngresosEgresos());
            CargarPanelConForm(panel36, new FrmVisorReporteEgresosIngresosMensual());
            CargarPanelConForm(panel37, new FrmVisorReporteEgresosIngresosMensualAgruparXElementoDeProveedor());
            CargarPanelConForm(panel38, new FrmChequesCobradosDePeriodosAnterioresTotalInicial());
        }

        private void CargarPanelConForm(Panel destinoPanel, Form origenFrm)
        {
            origenFrm.FormBorderStyle = FormBorderStyle.None;
            origenFrm.BackColor = Color.White;

            origenFrm.TopLevel = false;
            origenFrm.Dock = DockStyle.Fill;
            destinoPanel.Controls.Add(origenFrm);
            destinoPanel.Tag = origenFrm;
            origenFrm.Show();
        }

        private void CargarComBoBox()
        {
            comboBox1.Items.Add("Mas opciones");
            comboBox1.Items.Add("Crear producto");
            comboBox1.Items.Add("Crear tarifas de producto");
            comboBox1.Items.Add("Editar tarifas de producto");
            comboBox1.Items.Add("Agregar usuario");
            comboBox1.Items.Add("Modificar usuario");
            comboBox1.Items.Add("Privilegios de usuario");
            comboBox1.Items.Add("Editar datos de conexión");
        }

        private void DeshabilitarDeMenuOpciones()
        {
            bool respuesta = false;

            //Deshabilitar / Habilitar la opcion Agregar usuario, modificar usuario, privilegios de usuario del combobox
            respuesta = EsActivoModuloController(1) ? true : false;
            if (respuesta)
            { /* permanecen habilitadas los item del combobox  agregar usuario, modificar usuario, 
                privilegios de usuario*/
            }
            else
            {
                comboBox1.Items.Remove("Agregar usuario");
                comboBox1.Items.Remove("Modificar usuario");
                comboBox1.Items.Remove("Privilegios de usuario");
            }


            //Habilitar / deshabilitar la opcion Productos y tarifas del menu
            respuesta = EsActivoModuloController(5) ? true : false;
            if (respuesta)
            { /*Permanece habilitada la opcion Crear Producto del menu*/}
            else { comboBox1.Items.Remove("Crear producto"); }

            //Habilitar / deshabilitar la opcion crear tarifas de Producto, del menu
            respuesta = EsActivoModuloController(6) ? true : false;
            if (respuesta)
            { /*Permanece habilitada la opcion crear tarifas de Producto, del menu*/}
            else { comboBox1.Items.Remove("Crear tarifas de producto"); }

            //Habilitar / deshabilitar la opcion  Modificar tarifas de producto
            respuesta = EsActivoModuloController(7) ? true : false;
            if (respuesta)
            { /*Permanece habilitada la opcion Modificar tarifas a Producto, del menu*/}
            else { comboBox1.Items.Remove("Editar tarifas de producto"); }


            //Habilitar / deshabilitar la opcion agregar socio del menu
            respuesta = EsActivoModuloController(2) ? true : false;
            if (respuesta)
            {   /*Permanece habilitada la opcion agregar socio*/  }
            else
            { panel3.Enabled = false; }


            //Habilitar / deshabilitar la opcion buscar socio del menu
            respuesta = EsActivoModuloController(3) ? true : false;
            if (respuesta)
            {   /*Permanece habilitada la opcion buscar socio*/  }
            else
            { panel5.Enabled = false; }


            //Habilitar / deshabilitar la opcion Actualizar socio del menu
            respuesta = EsActivoModuloController(4) ? true : false;
            if (respuesta)
            { /*Permanece habilitada la opcion actualizar socio */  }
            else
            { panel4.Enabled = false; }



            //Habilitar / deshabilitar la opcion Abrir caja
            respuesta = EsActivoModuloController(8) ? true : false;
            if (respuesta)
            { /*Permanece habilitada la opcion Abrir caja, del menu*/}
            else { panel7.Enabled = false; }


            respuesta = EsActivoModuloController(9) ? true : false;
            if (respuesta)
            { /*Permanece habilitada la opcion Buscar folios de recibo de pago de socio*/}
            else { panel9.Enabled = false; }


            respuesta = EsActivoModuloController(10) ? true : false;
            if (respuesta)
            { /*Permanece habilitada la opcion Buscar folios de recibo de pago de todos los socios*/}
            else { panel10.Enabled = false; }


            respuesta = EsActivoModuloController(11) ? true : false;
            if (respuesta)
            { /*Permanece habilitada la opcion Cancelar folio de recibo de pago */}
            else { panel8.Enabled = false; }


            respuesta = EsActivoModuloController(12) ? true : false;
            if (respuesta)
            { /*Permanece habilitada la opcion Buscar folios de pago cancelados */}
            else { panel11.Enabled = false; }


            respuesta = EsActivoModuloController(13) ? true : false;
            if (respuesta)
            { /*Permanece habilitada la opcion Gastos (lo de capturar info de cheques) */}
            else { panel18.Enabled = false; }


            respuesta = EsActivoModuloController(14) ? true : false;
            if (respuesta)
            {  /*Permanece habilitada la opcion modificar gastos */}
            else { panel19.Enabled = false; }

            respuesta = EsActivoModuloController(15) ? true : false;
            if (respuesta)
            { /*Permanece habilitada la opcion eliminar cheque capturado*/   }
            else { panel20.Enabled = false; }


            respuesta = EsActivoModuloController(16) ? true : false;
            if (respuesta)
            { /*Permanece habilitada la opcion cheques no cobrados en el periodo*/ }
            else { panel21.Enabled = false; }

            respuesta = EsActivoModuloController(17) ? true : false;
            if (respuesta)
            {  /*Permance habilitada la opcion cheques cobrados en el periodo*/ }
            else { panel22.Enabled = false; }

            respuesta = EsActivoModuloController(18) ? true : false;
            if (respuesta)
            {   /*Permanece habilitada la opcion cheques de periodo*/  }
            else { panel23.Enabled = false; }

            respuesta = EsActivoModuloController(19) ? true : false;
            if (respuesta)
            {   /*Permanece habilitada la opcion insertar proveedor*/  }
            else { panel24.Enabled = false; }

            respuesta = EsActivoModuloController(20) ? true : false;
            if (respuesta)
            {   /*Permanece habilitada la opcion actualizar proveedor*/  }
            else { panel25.Enabled = false; }

            respuesta = EsActivoModuloController(21) ? true : false;
            if (respuesta)
            { /*Permanece habilitada la opcion eliminar proveedor*/ }
            else { panel26.Enabled = false; }

            respuesta = EsActivoModuloController(22) ? true : false;
            if (respuesta)
            {  /*Permanece habilitada la opcion ver proveedores*/  }
            else { panel27.Enabled = false; }

            respuesta = EsActivoModuloController(23) ? true : false;
            if (respuesta)
            {   /*Permanece habilitada la opcion sumar folios de usuario*/ }
            else { panel12.Enabled = false; }


            respuesta = EsActivoModuloController(24) ? true : false;
            if (respuesta)
            {   /*Permanece habilitada la opcion ver informe egresos- ingresos*/ }
            else { panel35.Enabled = false; }

            respuesta = EsActivoModuloController(25) ? true : false;
            if (respuesta)
            {  /*Permanece habilitada la opción sumar productos*/}
            else { panel13.Enabled = false; }

            respuesta = EsActivoModuloController(26) ? true : false;
            if (respuesta)
            {    /*Permanece habilitada la opción Bancos - crear periodo*/ }
            else { panel33.Enabled = false; }


            respuesta = EsActivoModuloController(28) ? true : false;
            if (respuesta)
            { /*Permanece habilitada la opcion Bancos - modificar saldo*/}
            else { panel34.Enabled = false; }

            respuesta = EsActivoModuloController(29) ? true : false;
            if (respuesta)
            { /*Permanece habilitada la opcion: Consultar folios de recibo de pago , imprimir exportar activos*/}
            else { panel14.Enabled = false; }


            respuesta = EsActivoModuloController(30) ? true : false;
            if (respuesta)
            { /* Permanece habilitada la opción: Socio, imprimir exportar*/}
            else
            { panel6.Enabled = false; }

            respuesta = EsActivoModuloController(31) ? true : false;
            if (respuesta)
            { /*Permanece habilitada la opción: Consular folios de recibo de pago, imprimir-exportar activos y cancelados*/}
            else
            { panel15.Enabled = false; }


            respuesta = EsActivoModuloController(32) ? true : false;
            if (respuesta)
            { /*Permanece habilitada la opción: Proveedor, agregar producto o servicio*/}
            else
            { panel28.Enabled = false; }


            respuesta = EsActivoModuloController(33) ? true : false;
            if (respuesta)
            { /*Permanece habilitada la opción: Proveedor, editar producto o servicio*/}
            else
            { panel29.Enabled = false; }


            respuesta = EsActivoModuloController(34) ? true : false;
            if (respuesta)
            { /*Permanece habilitada la opción: Beneficiario de cheque, nuevo*/}
            else
            { panel30.Enabled = false; }

            respuesta = EsActivoModuloController(35) ? true : false;
            if (respuesta)
            { /*Permanece habilitada la opción: Beneficiario de cheque, Ver todos*/}
            else
            { panel32.Enabled = false; }

            respuesta = EsActivoModuloController(36) ? true : false;
            if (respuesta)
            { /* Permanece habilitada la opción: Beneficiario de cheque, Eliminar */}
            else
            { panel31.Enabled = false; }

            respuesta = EsActivoModuloController(38) ? true : false;
            if (respuesta)
            { /*Permanece habilitada la opción: Informe egresos / ingresos, Informe mensual*/}
            else
            { panel36.Enabled = false; }

            respuesta = EsActivoModuloController(39) ? true : false;
            if (respuesta)
            { /*Permanece habilitada la opción: Informe egresos / ingresos, Total inicial de 
                cheques cobrados de periodos anteriores*/
            }
            else
            { panel38.Enabled = false; }

            //el modulo 40 es el boton Informe egresos-ingresos, exportar a excel
            //el modulo 41 es el boton Informe egresos-ingresos mensual, exportar a excel

            respuesta = EsActivoModuloController(42) ? true : false;
            if (respuesta)
            { /*Permanece habilitada la opción: Informe x elementos de proveedor */
            }
            else
            { panel37.Enabled = false; }


            respuesta = EsActivoModuloController(44) ? true : false;
            if (respuesta)
            {/*Permance habilitada la opción: Activos y cancelados de socio*/ }
            else
            { panel16.Enabled = false; }


            respuesta = EsActivoModuloController(45) ? true : false;
            if (respuesta)
            {/*Permance habilitada la opción: Consultar folios de recibo de pago - productos pagados y no pagados*/ }
            else
            { panel17.Enabled = false; }


            respuesta = EsActivoModuloController(47) ? true : false;
            if (respuesta)
            { /*Permancede habilitada en el combobox el item "Editar datos de conexión*/}
            else
            { comboBox1.Items.Remove("Editar datos de conexión"); }
        }



        //--------------------------Events
        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush _textBrush;

            // Get the item from the collection.
            TabPage _tabPage = tabControl1.TabPages[e.Index];

            // Get the real bounds for the tab rectangle.
            Rectangle _tabBounds = tabControl1.GetTabRect(e.Index);

            if (e.State == DrawItemState.Selected)
            {

                // Draw a different background color, and don't paint a focus rectangle.
                _textBrush = new SolidBrush(Color.White);
                g.FillRectangle(Brushes.DodgerBlue, e.Bounds);
            }
            else
            {
                _textBrush = new System.Drawing.SolidBrush(e.ForeColor);
                e.DrawBackground();
            }

            // Use our own font.
            Font _tabFont = new Font("Microsoft Sans Serif", (float)14.0, FontStyle.Regular, GraphicsUnit.Pixel);

            // Draw string. Center the text.
            StringFormat _stringFlags = new StringFormat();
            _stringFlags.Alignment = StringAlignment.Center;
            _stringFlags.LineAlignment = StringAlignment.Center;
            g.DrawString(_tabPage.Text, _tabFont, _textBrush, _tabBounds, new StringFormat(_stringFlags));
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > 0)
            {
                if (comboBox1.SelectedItem.ToString() == "Crear producto")
                {
                    AbrirFormulario(new FrmProductoCrear());
                }

                else if (comboBox1.SelectedItem.ToString() == "Crear tarifas de producto")
                {
                    AbrirFormulario(new FrmCrearTarifaAProducto());
                }

                else if (comboBox1.SelectedItem.ToString() == "Editar tarifas de producto")
                {
                    AbrirFormulario(new FrmEditarTarifasDeProducto());
                }

                else if (comboBox1.SelectedItem.ToString() == "Agregar usuario")
                {
                    AbrirFormulario(new FrmUsuarioAgregar());
                }

                else if (comboBox1.SelectedItem.ToString() == "Modificar usuario")
                {
                    AbrirFormulario(new FrmUsuarioActualizar());
                }

                else if (comboBox1.SelectedItem.ToString() == "Privilegios de usuario")
                {
                    AbrirFormulario(new FrmUsuarioAsignarPrivilegios());
                }

                else if (comboBox1.SelectedItem.ToString() == "Editar datos de conexión")
                {
                    AbrirFormulario(new FrmConfigurarInfoConexion());
                }

                comboBox1.SelectedIndex = 0;
            }
        }
    }
}
