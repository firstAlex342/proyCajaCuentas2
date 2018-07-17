using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace CapaLogicaNegocios
{
    public static class ClsLogin
    {
        public static int Id { set; get; }
        public static DataTable Usuario { set; get; }
        public static DataTable ModulosALosQueTieneAccesoUsuario { set; get; }
    }
}
