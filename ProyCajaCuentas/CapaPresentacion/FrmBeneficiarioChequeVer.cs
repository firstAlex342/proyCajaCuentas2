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
    public partial class FrmBeneficiarioChequeVer : Form
    {
        //----------------Constructor
        public FrmBeneficiarioChequeVer()
        {
            InitializeComponent();

            try
            {
                DataTable beneficiarios = BeneficiarioCheque_Select_ActivosController();
                MostrarEnGridBeneficiarios(beneficiarios);
                MostrarEnLabelNumeroDeElementos(beneficiarios);
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }

        //-------------Controllers
        private DataTable BeneficiarioCheque_Select_ActivosController()
        {
            ClsBeneficiarioCheque clsBeneficiarioCheque = new ClsBeneficiarioCheque();
            return (clsBeneficiarioCheque.BeneficiarioCheque_Select_Activos());
        }

        //----------------Methods
        private void MostrarEnGridBeneficiarios(DataTable beneficiarios)
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = beneficiarios;

            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[2].Visible = false;
        }

        private void MostrarEnLabelNumeroDeElementos(DataTable beneficiarios)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("Numero de elementos encontrados: ");
            strBuilder.Append(beneficiarios.Rows.Count.ToString());
            label2.Text = strBuilder.ToString();
        }



    }
}
