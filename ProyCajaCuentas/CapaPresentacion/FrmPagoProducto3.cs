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
    public partial class FrmPagoProducto3 : Form
    {
        public FrmPagoProducto3()
        {
            InitializeComponent();
            menuStrip1.Renderer = new ToolStripProfessionalRenderer(new TestColorTable());
        }




        private void unoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void productoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void socioToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void FrmPagoProducto3_Load(object sender, EventArgs e)
        {
            
           
        }
    
    }




    public class TestColorTable : ProfessionalColorTable
    {
        public override Color MenuItemSelected
        {
            get { return Color.Red; }
        }

        
        //public override Color MenuBorder  //added for changing the menu border
        //{
        //    get { return Color.Green; }
        //}
        //public int prueba()
        //{
        //    this.
        //}
    }




}
