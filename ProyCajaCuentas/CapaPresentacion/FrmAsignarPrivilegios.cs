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
    public partial class FrmAsignarPrivilegios : MetroForm
    {
        public FrmAsignarPrivilegios()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;

            DataTable tabla = RecuperarIdNombreUsuarioPassActivoDeTodosController();
            MostrarNombreUsuarioPassActivoDeTodos(tabla);
        }

        //------------Methods
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

        //---------------utils
        public void MostrarNombreUsuarioPassActivoDeTodos(DataTable tabla)
        {

            metroGrid1.DataSource = tabla;
            metroGrid1.Columns[0].Visible = false;

        }

        private void MostrarEnGridModulosALosQueTieneAcceso(DataTable tabla)
        {
            metroGrid2.DataSource = tabla;   //0 1 y 3
            metroGrid2.Columns[0].Visible = false;
            metroGrid2.Columns[1].Visible = false;
            metroGrid2.Columns[3].Visible = false;
        }


        //---------------Eventos
        private void metroGrid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void FrmAsignarPrivilegios_Load(object sender, EventArgs e)
        {

        }

        private void metroGrid1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                try
                {
                    
                    string idUsuarioEnTexto = (metroGrid1.Rows[e.RowIndex].Cells[0].EditedFormattedValue).ToString();
                    DataTable miTabla = AccesoAModulo_Modulo_InnerJoinController(Int32.Parse(idUsuarioEnTexto));

                    MostrarEnGridModulosALosQueTieneAcceso(miTabla);
                }

                catch (Exception ex)
                {
                    MetroMessageBox.Show(this, ex.Message + " " + ex.Source);
                }
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult res = MetroMessageBox.Show(this, "¿Esta usted seguro que desea continuar?", "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(res == DialogResult.Yes)
                {
                    DataGridViewRowCollection collection = metroGrid2.Rows;
                    foreach (DataGridViewRow row in collection)
                    {
                        int idUsuario = 0;
                        int idModulo = 0;
                        bool estado = false;

                        idUsuario = Int32.Parse(row.Cells[0].Value.ToString());
                        idModulo = Int32.Parse(row.Cells[1].Value.ToString());
                        estado = bool.Parse(row.Cells[2].Value.ToString());


                        string respuesta = AccesoAModulo_update(idUsuario, idModulo, estado);
                    }

                    MetroMessageBox.Show(this, "Registro guardado exitosamente", "Resultado de operación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

                else
                {
                    //No se hace algo
                }

            }

            catch(Exception ex)
            {
                MetroMessageBox.Show(this, ex.Message + " " + ex.Source);
            }
        }
    }
}
