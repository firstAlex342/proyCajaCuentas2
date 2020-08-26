using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaPresentacion
{
    public interface IConectarProveedorEnCheque
    {
        void MostrarNombreProveedorElegido(int idProveedorSeleccionado, string nombreProveedorSeleccionado);
    }
}
