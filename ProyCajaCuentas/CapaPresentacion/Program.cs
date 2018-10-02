using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaLogicaNegocios;

namespace CapaPresentacion
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new FrmInicioSesion());   //con esta linea corr el programa en modo obscuro, este es el que estoy probando todo el tiempo
            //Application.Run(new FrmVisorReporteChequesNoCobrados());

            //Application.Run(new FrmPagoProducto3());  // con esta liena inicia viendose menu con color rojo
        }
    }
}
