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
    public partial class FrmPrincipal2 : MetroForm
    {
        public FrmPrincipal2()
        {
            InitializeComponent();
            metroTile1.UseSelectable = false;
            metroTile2.UseSelectable = false;
            metroTile9.UseSelectable = false;
        }

        private void metroTile1_Click(object sender, EventArgs e)
        {

        }

        private void metroTile2_Click(object sender, EventArgs e)
        {

        }

        private void metroTile9_Click(object sender, EventArgs e)
        {

        }

        private void metroTile1_MouseEnter(object sender, EventArgs e)
        {
            //Oculto lo que no quiero mostrar
            metroPanel4.Visible = false;
            metroPanel3.Visible = false;

            //Hago visible lo que quiero mostrar
            metroPanel2.Visible = true;
        }

        private void metroTile2_MouseEnter(object sender, EventArgs e)
        {
            //Oculto lo que no quiero mostrar
            metroPanel2.Visible = false;
            metroPanel3.Visible = false;

            //Hago visible lo que quiero mostrar
            metroPanel4.Visible = true;

        }

        private void metroTile9_MouseEnter(object sender, EventArgs e)
        {
            //Oculto lo que no quiero mostrar
            metroPanel2.Visible = false;
            metroPanel4.Visible = false;

            //Hago visible lo que quiero mostrar
            metroPanel3.Visible = true;
        }

        private void FrmPrincipal2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void metroTile3_Click(object sender, EventArgs e)
        {
            FrmSocioAgregar c = new FrmSocioAgregar();
            c.ShowDialog(this);
            c.Dispose();
        }

        private void metroTile4_Click(object sender, EventArgs e)
        {
            FrmSocioModificar c = new FrmSocioModificar();
            c.ShowDialog(this);
            c.Dispose();
        }

        private void metroTile5_Click(object sender, EventArgs e)
        {
            FrmSocioBuscar c = new FrmSocioBuscar();
            c.ShowDialog(this);
            c.Dispose();
        }

        private void metroTile6_Click(object sender, EventArgs e)
        {
            //FrmProductoCrear c = new FrmProductoCrear();
            //c.ShowDialog(this);
            //c.Dispose();
        }

        private void metroTile7_Click(object sender, EventArgs e)
        {
            //FrmProductoEditarTarifas c = new FrmProductoEditarTarifas();
            //c.ShowDialog(this);
            //c.Dispose();
        }

        private void metroTile8_Click(object sender, EventArgs e)
        {
            //FrmProductoCrearTarifas c = new FrmProductoCrearTarifas();
            //c.ShowDialog(this);
            //c.Dispose();
        }

        private void metroTile10_Click(object sender, EventArgs e)
        {
            FrmAgregarUsuario c = new FrmAgregarUsuario();
            c.ShowDialog(this);
            c.Dispose();
        }

        private void metroTile11_Click(object sender, EventArgs e)
        {
            FrmUsuarioModificar c = new FrmUsuarioModificar();
            c.ShowDialog(this);
            c.Dispose();
        }

        private void metroTile12_Click(object sender, EventArgs e)
        {
            FrmAsignarPrivilegios c = new FrmAsignarPrivilegios();
            c.ShowDialog(this);
            c.Dispose();
        }
    }
}
