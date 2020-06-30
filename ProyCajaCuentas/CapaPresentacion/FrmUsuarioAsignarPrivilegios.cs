using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaLogicaNegocios;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class FrmUsuarioAsignarPrivilegios : Form
    {
        public FrmUsuarioAsignarPrivilegios()
        {
            InitializeComponent();

            try
            {
                DataTable tabla = RecuperarIdNombreUsuarioPassActivoDeTodosController();
                MostrarNombreUsuarioPassActivoDeTodos(tabla);
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                ClsMyException clsMyException = new ClsMyException();
                string res = clsMyException.FormarTextoDeSqlException(ex);

                MessageBox.Show(res, "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }

        //------------Controllers
        public DataTable RecuperarIdNombreUsuarioPassActivoDeTodosController()
        {
            ClsUsuario clsUsuario = new ClsUsuario();
            DataTable tabla = clsUsuario.Usuario_Select_Id_Nombre_Usuario_Pass_Activo_DeTodos();

            return (tabla);
        }

        public DataTable AccesoAModulo_Modulo_InnerJoinController(int idUsuario)
        {
            ClsAccesoAModulo clsAccesoAModulo = new ClsAccesoAModulo();
            clsAccesoAModulo.IdUsuario = idUsuario;
            return (clsAccesoAModulo.AccesoAModulo_Modulo_InnerJoin());
        }


        private string AccesoAModulo_update(int idUsuario, int idModulo, bool nuevoEstado)
        {
            ClsAccesoAModulo clsAccesoAModulo = new ClsAccesoAModulo();
            clsAccesoAModulo.IdUsuario = idUsuario;
            clsAccesoAModulo.IdModulo = idModulo;
            clsAccesoAModulo.Activo = nuevoEstado;

            return (clsAccesoAModulo.AccesoAModulo_update());
        }


        private string AccesoAModulo_Update_CollectionController(int idUsuarioAActualizar, DataTable listaIdModulosYEstados)
        {
            ClsAccesoAModulo clsAccesoAModulo = new ClsAccesoAModulo();
            clsAccesoAModulo.IdUsuario = idUsuarioAActualizar;
            return (clsAccesoAModulo.AccesoAModulo_Update_Collection(listaIdModulosYEstados));
        }

        //---------------Utils
        public void MostrarNombreUsuarioPassActivoDeTodos(DataTable tabla)
        {
            dataGridView1.DataSource = tabla;
            dataGridView1.Columns[0].Visible = false;
        }

        private void MostrarEnGridModulosALosQueTieneAcceso(DataTable tabla)
        {
            dataGridView2.DataSource = tabla;   //0 1 y 3
            dataGridView2.Columns[0].Visible = false;
            dataGridView2.Columns[1].Visible = false;
            dataGridView2.Columns[3].Visible = false;

            //0 1 3 y 4 solo lectura
            dataGridView2.Columns[0].ReadOnly = true;
            dataGridView2.Columns[1].ReadOnly = true;
            dataGridView2.Columns[3].ReadOnly = true;
            dataGridView2.Columns[4].ReadOnly = true;
        }



        //----------------------Events
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {

                    string idUsuarioEnTexto = (dataGridView1.Rows[e.RowIndex].Cells[0].EditedFormattedValue).ToString();
                    DataTable miTabla = AccesoAModulo_Modulo_InnerJoinController(Int32.Parse(idUsuarioEnTexto));

                    MostrarEnGridModulosALosQueTieneAcceso(miTabla);
                }

                catch (System.Data.SqlClient.SqlException ex)
                {
                    ClsMyException clsMyException = new ClsMyException();
                    string res = clsMyException.FormarTextoDeSqlException(ex);
                    dataGridView2.DataSource = null;
                    MessageBox.Show(res, "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult res = MessageBox.Show( "¿Esta usted seguro que desea continuar?", "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    DataTable listaIdModulosYEstados = new DataTable();
                    listaIdModulosYEstados.Columns.Add("IdModulo", typeof(int));
                    listaIdModulosYEstados.Columns.Add("Estado", typeof(bool));


                    IEnumerable<DataGridViewRow> filasGrid = dataGridView2.Rows.Cast<DataGridViewRow>();
                    DataGridViewRow fila = filasGrid.First<DataGridViewRow>();  //First lanza excepcion si esta vacia la lista
                    int idUsuario = Int32.Parse(fila.Cells[0].EditedFormattedValue.ToString());

                    DataGridViewRowCollection collection = dataGridView2.Rows;
                    foreach (DataGridViewRow row in collection)
                    {                       
                        int idModulo = 0;
                        bool estado = false;

                        idModulo = Int32.Parse(row.Cells[1].EditedFormattedValue.ToString());
                        estado = bool.Parse(row.Cells[2].EditedFormattedValue.ToString());

                        listaIdModulosYEstados.Rows.Add(idModulo, estado);
                    }

                    string respuesta = AccesoAModulo_Update_CollectionController(idUsuario, listaIdModulosYEstados);
                    if(respuesta.Contains("ok"))
                    {
                        MessageBox.Show(this, "Registro guardado exitosamente", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    else
                    {
                        MessageBox.Show(this, respuesta, "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DataTable tabla = RecuperarIdNombreUsuarioPassActivoDeTodosController();
                        MostrarNombreUsuarioPassActivoDeTodos(tabla);
                        dataGridView2.DataSource = null;
                    }
                }
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                ClsMyException clsMyException = new ClsMyException();
                string res = clsMyException.FormarTextoDeSqlException(ex);

                MessageBox.Show(res, "Reglas de operación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.Source + " " + ex.StackTrace);
            }
        }
    }
}
