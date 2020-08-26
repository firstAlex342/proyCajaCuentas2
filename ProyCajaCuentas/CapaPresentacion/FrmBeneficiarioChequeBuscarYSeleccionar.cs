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
    public partial class FrmBeneficiarioChequeBuscarYSeleccionar : Form
    {
        public int IdBeneficiarioChequeSeleccionado { set; get; }
        public string NombreBeneficiarioChequeSeleccionado { set; get; }
        public List<IConectarBeneficiarioElegido> MisSubscriptores { set; get; }


        //---------------Constructor
        public FrmBeneficiarioChequeBuscarYSeleccionar()
        {
            InitializeComponent();

            this.IdBeneficiarioChequeSeleccionado = 0;
            this.NombreBeneficiarioChequeSeleccionado = String.Empty;
            this.MisSubscriptores = new List<IConectarBeneficiarioElegido>();

            try
            {
                CrearColumnasEnGridParaSeleccionarElemento();
                DataTable beneficiariosCheque = BeneficiarioCheque_Select_ActivosController();
                MostrarBeneficiariosEnGrid(beneficiariosCheque);
                label2.Text = " Elementos encontrados: " + beneficiariosCheque.Rows.Count.ToString();
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }//parameterless constructor


        //---------------controllers
        private DataTable BeneficiarioCheque_Select_ActivosController()
        {
            ClsBeneficiarioCheque clsBeneficiarioCheque = new ClsBeneficiarioCheque();
            return (clsBeneficiarioCheque.BeneficiarioCheque_Select_Activos());
        }

        //----------------Methods
        private void CrearColumnasEnGridParaSeleccionarElemento()
        {
            DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
            chk.Name = "Seleccionar";
            chk.HeaderText = "Seleccionar";

            dataGridView1.Columns.Add(chk);
            dataGridView1.Columns.Add("Id", "Id");
            dataGridView1.Columns.Add("Nombre", "Nombre");

            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].ReadOnly = true;
        }

        private void MostrarBeneficiariosEnGrid(DataTable beneficiarios)
        {
            var res = beneficiarios.AsEnumerable();
            List<DataRow> listaDataRows = res.ToList<DataRow>();

            Action<DataRow> AniadirADataGrid = fila => {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[1].Value = fila.Field<int>("Id");
                dataGridView1.Rows[n].Cells[2].Value = fila.Field<string>("Nombre");
            };

            listaDataRows.ForEach(AniadirADataGrid);
        }

        public void AddSubscriptor(IConectarBeneficiarioElegido subscriptor)
        {
            this.MisSubscriptores.Add(subscriptor);
        }

        private void MostrarNombreBeneficiarioSeleccionado()
        {
            foreach (IConectarBeneficiarioElegido subscriptor in this.MisSubscriptores)
                subscriptor.MostrarBeneficiarioElegido(this.IdBeneficiarioChequeSeleccionado, this.NombreBeneficiarioChequeSeleccionado);
        }


        //---------------Events
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                IEnumerable<DataGridViewRow> coleccionFilas = dataGridView1.Rows.Cast<DataGridViewRow>();
                var x = coleccionFilas.ToList<DataGridViewRow>();

                Func<DataGridViewRow, bool> BuscarFilaSeleccionada = item =>
                {
                    if (item.Cells[0].Value != null)
                    {
                        DataGridViewCheckBoxCell celda = (DataGridViewCheckBoxCell)item.Cells[0];
                        if (celda.Value.ToString() == "True")
                            return true;
                        else
                            return false;
                    }
                    return false;
                };


                DataGridViewRow filaSeleccionada = x.SingleOrDefault(BuscarFilaSeleccionada);
                if (filaSeleccionada == null)
                {
                    this.NombreBeneficiarioChequeSeleccionado = String.Empty;
                    this.IdBeneficiarioChequeSeleccionado = 0;
                }

                else
                {
                    this.NombreBeneficiarioChequeSeleccionado = filaSeleccionada.Cells[2].EditedFormattedValue.ToString();
                    this.IdBeneficiarioChequeSeleccionado = Int32.Parse(filaSeleccionada.Cells[1].EditedFormattedValue.ToString());
                }

                MostrarNombreBeneficiarioSeleccionado();
                this.Close();
            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn &&
                e.RowIndex >= 0)
            {
                Action<DataGridViewRow> DeSeleccionarTodosCheckBoxExceptoEnDondeOcurrioElEvento = fila =>
                {
                    if (fila.Index != e.RowIndex)
                    {
                        fila.Cells[0].Value = false;
                    }
                };

                IEnumerable<DataGridViewRow> coleccionFilas = dataGridView1.Rows.Cast<DataGridViewRow>();
                coleccionFilas.ToList().ForEach(DeSeleccionarTodosCheckBoxExceptoEnDondeOcurrioElEvento);
            }
        }
    }
}
