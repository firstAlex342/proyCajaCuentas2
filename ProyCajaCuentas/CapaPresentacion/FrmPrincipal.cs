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
    public partial class FrmPrincipal : MetroForm
    {
        //-----------------constructor
        public FrmPrincipal()
        {
            InitializeComponent();
            metroTile1.UseSelectable = false;
            metroTile2.UseSelectable = false;
            metroTile9.UseSelectable = false;
            metroTile13.UseSelectable = false;

            DeshabilitarDeMenuOpciones();
        }

        //-------------Methods controller
        public bool EsActivoModuloController(int idModuloBuscado)
        {
            var lista = ClsLogin.ModulosALosQueTieneAccesoUsuario.AsEnumerable();
            DataRow filaBuscada = lista.FirstOrDefault(s => 
            (s.Field<int>(1) == idModuloBuscado)  && (s.Field<bool>(2) == true) );

            if (filaBuscada != null)
            {
                return true;
            }

            else
                return false;
        }



        //---------------Methods
        private void DeshabilitarDeMenuOpciones()
        {
            bool respuesta = false;

            //Deshabilitar / Habilitar la opcion Usuarios del menu
            respuesta =EsActivoModuloController(1) ? true : false;
            if(respuesta)
            { /*metroTile9 permanece habilitado*/ }
            else
            { metroTile9.Enabled = false; }


            //Habilitar / deshabilitar la opcion agregar socio del menu
            respuesta = EsActivoModuloController(2) ? true : false;
            if(respuesta)
            {   /*Permanece habilitada la opcion agregar socio*/  }
            else
            {   metroTile3.Enabled = false; }

            //Habilitar / deshabilitar la opcion buscar socio del menu
            respuesta = EsActivoModuloController(3) ? true : false;
            if (respuesta)
            {   /*Permanece habilitada la opcion buscar socio*/  }
            else
            { metroTile5.Enabled = false; }


            //Habilitar / deshabilitar la opcion Actualizar socio del menu
            respuesta = EsActivoModuloController(4) ? true : false;
            if(respuesta)
            { /*Permanece habilitada la opcion actualizar socio */  }
            else
            { metroTile4.Enabled = false; }

            //Habilitar / deshabilitar la opcion crear Producto del menu
            respuesta = EsActivoModuloController(5) ? true : false;
            if (respuesta)
            { /*Permanece habilitada la opcion Crear Producto del menu*/}
            else { metroTile6.Enabled = false; }

            //Habilitar / deshabilitar la opcion Añadir Tarifa a Producto, del menu
            //respuesta = EsActivoModuloController(6) ? true : false;
            //if (respuesta)
            //{ /*Permanece habilitada la opcion Añadir Tarifa a Producto, del menu*/}
            //else { metroTile8.Enabled = false; }

            ////Habilitar / deshabilitar la opcion  Modificar tarifas de producto
            //respuesta = EsActivoModuloController(7) ? true : false;
            //if (respuesta)
            //{ /*Permanece habilitada la opcion Modificar tarifas a Producto, del menu*/}
            //else { metroTile7.Enabled = false; }


            //Habilitar / deshabilitar la opcion Abrir caja
            respuesta = EsActivoModuloController(8) ? true : false;
            if (respuesta)
            { /*Permanece habilitada la opcion Abrir caja, del menu*/}
            else { metroTile14.Enabled = false; }


        }



        //--------Eventos de menu

        private void metroPanel1_MouseLeave(object sender, EventArgs e)
        {
            //metroPanel2.Visible = false;
            //metroPanel4.Visible = false;
        }

        private void metroTile1_MouseEnter(object sender, EventArgs e)
        {
            //Oculto lo que no quiero mostrar
            metroPanel4.Visible = false;
            metroPanel3.Visible = false;
            metroPanel5.Visible = false;

            //Hago visible lo que quiero mostrar
            metroPanel2.Visible = true;
        }

        private void metroTile2_MouseEnter(object sender, EventArgs e)
        {
            //Oculto lo que no quiero mostrar
            metroPanel2.Visible = false;
            metroPanel3.Visible = false;
            metroPanel5.Visible = false;

            //Hago visible lo que quiero mostrar
            metroPanel4.Visible = true;
        }

        private void metroTile9_MouseEnter(object sender, EventArgs e)
        {
            //Oculto lo que no quiero mostrar
            metroPanel2.Visible = false;
            metroPanel4.Visible = false;
            metroPanel5.Visible = false;

            //Hago visible lo que quiero mostrar
            metroPanel3.Visible = true;
        }


        private void metroTile13_MouseEnter(object sender, EventArgs e)
        {
            //Oculto lo que no quiero mostrar
            metroPanel2.Visible = false;
            metroPanel4.Visible = false;
            metroPanel3.Visible = false;

            //Hago visible lo que quiero mostrar
            metroPanel5.Visible = true;
        }




        //-----------Eventos de abrir un Formulario

        private void metroTile3_Click(object sender, EventArgs e)
        {
            FrmSocioAgregar c = new FrmSocioAgregar();
            c.ShowDialog(this);
            c.Dispose();
        }

        private void metroTile10_Click(object sender, EventArgs e)
        {
            FrmAgregarUsuario c = new FrmAgregarUsuario();
            c.ShowDialog(this);
            c.Dispose();
        }

        private void FrmPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void metroTile12_Click(object sender, EventArgs e)
        {
            FrmAsignarPrivilegios c = new FrmAsignarPrivilegios();
            c.ShowDialog(this);
            c.Dispose();
        }

        private void metroTile5_Click(object sender, EventArgs e)
        {
            FrmSocioBuscar c = new FrmSocioBuscar();
            c.ShowDialog(this);
            c.Dispose();
        }

        private void metroTile4_Click(object sender, EventArgs e)
        {
            FrmSocioModificar c = new FrmSocioModificar();
            c.ShowDialog(this);
            c.Dispose();
        }

        private void metroTile11_Click(object sender, EventArgs e)
        {
            FrmUsuarioModificar c = new FrmUsuarioModificar();
            c.ShowDialog(this);
            c.Dispose();
        }

        private void metroTile6_Click(object sender, EventArgs e)
        {
            FrmProducto c = new FrmProducto();
            c.ShowDialog(this);
            c.Dispose();

        }

        private void metroTile7_Click(object sender, EventArgs e)
        {
            FrmProductoEditarTarifas c = new FrmProductoEditarTarifas();
            c.ShowDialog(this);
            c.Dispose();
        }

        private void metroTile8_Click(object sender, EventArgs e)
        {
            FrmProductoCrearTarifas c = new FrmProductoCrearTarifas();
            c.ShowDialog(this);
            c.Dispose();
        }

        private  async void metroTile14_Click(object sender, EventArgs e)
        {
            //Las siguientes 2 lineas aceleran la carga de crystal report
            Func<CRReporteVacio> funcion = () => { return new CRReporteVacio(); };
            await Task.Run(funcion);

            FrmRealizarCobro c = new FrmRealizarCobro();         
            c.ShowDialog(this);
            c.Dispose();
        }



























        //----------------Methods
    }
}
