using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class Form1 : Form
    {
         //-------------Constructor
        public Form1()
        {
            InitializeComponent();
        }


        //--------------Methods
        private void AbrirFormulario(object formHijo)
        {
            if (this.panelContenedor.Controls.Count > 0)
                this.panelContenedor.Controls.RemoveAt(0);

            Form fh = formHijo as Form;
            fh.TopLevel = false;
            fh.Dock = DockStyle.Fill;

            this.panelContenedor.Controls.Add(fh);
            this.panelContenedor.Tag = fh;
            fh.Show();
        }
        private void nuevaCuentaPorPagarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmProductosAPagar());
        }

        private void aportacionesACuentaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmAportacionesAProductoEnCuenta());
        }

        private void editarSocioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmSocio());
        }

        private void editarProductoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmProducto());
        }

        private void nuevoPagoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmPagoProducto());
        }
    }
}
