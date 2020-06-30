using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace CapaLogicaNegocios
{
    public class ClsTarifasYEstados
    {
        private DataTable misTarifasYEstados;

        //--------------Constructor
        public ClsTarifasYEstados()
        {
            this.MisTarifasYEstados = new DataTable();
            MisTarifasYEstados.Columns.Add("IdTarifa", typeof(int));
            MisTarifasYEstados.Columns.Add("Estado", typeof(bool));
        }

        public void AddItem(int idTarifa, bool estado)
        {
            this.MisTarifasYEstados.Rows.Add(idTarifa, estado);
        }

        //-----------------properties
        public DataTable MisTarifasYEstados
        {
            set { misTarifasYEstados = value; }
            get { return misTarifasYEstados;  }
        }
    }
}
