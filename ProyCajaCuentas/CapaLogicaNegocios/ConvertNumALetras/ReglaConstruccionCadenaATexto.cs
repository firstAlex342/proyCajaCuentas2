using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogicaNegocios.ConvertNumALetras
{
    public class ReglaConstruccionCadenaATexto
    {
        private int limiteInferiorCadena;
        private int limiteSuperiorCadena;
        private int cantidadEspaciosEnCadenaVacia;
        private string terminacion;

        //-----------Constructor
        public ReglaConstruccionCadenaATexto(int limiteInferiorCadena, int limiteSuperiorCadena,
            int cantidadEspaciosEnCadenaVacia, string terminacion)
        {
            this.LimiteInferiorCadena = limiteInferiorCadena;
            this.LimiteSuperiorCadena = limiteSuperiorCadena;
            this.CantidadEspaciosEnCadenaVacia = cantidadEspaciosEnCadenaVacia;
            this.Terminacion = terminacion;
        }

        //------------Methods
        public override string ToString()
        {
            string texto;
            texto = "LimiteInferiorCadena "  + this.LimiteInferiorCadena.ToString();
            texto += " LimiteSuperiorCadena " + this.LimiteSuperiorCadena.ToString();
            texto += " CantidadEspaciosEnCadenaVacia " + this.CantidadEspaciosEnCadenaVacia.ToString();
            texto += " Terminacion " + this.Terminacion;

            return (texto);
        }

        //---------Properties
        public int LimiteInferiorCadena
        {
            set { limiteInferiorCadena = value; }
            get { return limiteInferiorCadena; }
        }

        public int LimiteSuperiorCadena
        {
            set { limiteSuperiorCadena = value; }
            get { return limiteSuperiorCadena; }
        }

        public int CantidadEspaciosEnCadenaVacia
        {
            set { cantidadEspaciosEnCadenaVacia = value; }
            get { return cantidadEspaciosEnCadenaVacia;  }
        }

        public string Terminacion
        {
            set { terminacion = value; }
            get { return terminacion; }
        }




    }
}
