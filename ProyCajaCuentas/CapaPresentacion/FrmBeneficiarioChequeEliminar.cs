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
    public partial class FrmBeneficiarioChequeEliminar : Form
    {
        //-------------------constructor
        public FrmBeneficiarioChequeEliminar()
        {
            InitializeComponent();
            
            try
            {
                CrearColumnasEnGridParaSeleccionarElemento();
                DataTable beneficiarios = BeneficiarioCheque_Select_ActivosController();
                MostrarBeneficiariosEnGrid(beneficiarios);
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

        private string BeneficiarioCheque_UpdateActivoACeroController(int idBeneficiarioCheque, int idUsuarioOperador)
        {
            ClsBeneficiarioCheque clsBeneficiarioCheque = new ClsBeneficiarioCheque();
            clsBeneficiarioCheque.Id = idBeneficiarioCheque;
            clsBeneficiarioCheque.IdUsuarioModifico = idUsuarioOperador;

            return (clsBeneficiarioCheque.BeneficiarioCheque_UpdateActivoACero());
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

        private object BuscarFilaSeleccionada(DataGridView dataGridViewX)
        {
            Func<DataGridViewRow, bool> delegado = item =>
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


            IEnumerable<DataGridViewRow> coleccionFilas = dataGridViewX.Rows.Cast<DataGridViewRow>();
            var x = coleccionFilas.ToList<DataGridViewRow>();

            return (x.SingleOrDefault(delegado));
        }

        private void ReinicializarDataGrid()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            DataTable beneficiarios = BeneficiarioCheque_Select_ActivosController();
            MostrarBeneficiariosEnGrid(beneficiarios);
        }

        //----------------------Events
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

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult res = MessageBox.Show("¿Esta usted seguro que desea continuar?", "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    object filaSeleccionada = BuscarFilaSeleccionada(dataGridView1);

                    if (filaSeleccionada != null)
                    {
                        DataGridViewRow fila = (DataGridViewRow)filaSeleccionada;
                        int idBeneficiarioCheque = Int32.Parse(fila.Cells[1].EditedFormattedValue.ToString());

                        string mensaje = BeneficiarioCheque_UpdateActivoACeroController(idBeneficiarioCheque, ClsLogin.Id);
                        if (mensaje.Contains("ok"))
                        {
                            MessageBox.Show("Elemento eliminado", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ReinicializarDataGrid();
                        }

                        else
                        {
                            MessageBox.Show(mensaje, "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }

                    else
                    {
                        MessageBox.Show("Es necesario seleccionar un beneficiario", "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }//if
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }
    }
}
