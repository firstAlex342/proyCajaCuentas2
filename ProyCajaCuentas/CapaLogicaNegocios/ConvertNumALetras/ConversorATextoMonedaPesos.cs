using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogicaNegocios.ConvertNumALetras
{
    public class ConversorATextoMonedaPesos
    {
        //----------Constructor
        public ConversorATextoMonedaPesos()
        {
        }

        //Convierte un texto en el rango 0 a 999999999 a expresion monetaria
        //Ejemplo 567.0    quinientos sesenta y siete pesos 0/100 M.N
        //Es obligatorio poner el punto si la cantidad tieno ó no decimales
        public string Analizar(string cadenaNumerica)
        {       
            if(ContieneElPunto(cadenaNumerica) )
            {
                string[] s = cadenaNumerica.Split('.');

                if(  EstaEnRangoYesNumero(s[0])   )
                {

                    string res1 = AnalizarParteEntera(s[0]);
                    res1 += MostrarTerminacionPeso_Pesos_DePesos(s[0]);
                    res1 += s[1] + "/100 M.N.";

                    return (res1);
                }

            }

            return ("Error....");
        }

        //-------------Methods
        private string AnalizarParteEntera(string parteEntera)
        {
            //parteEntera debe estar en un rango de 0 a 999,999,999

            List<ReglaConstruccionCadenaATexto> tabla = ConstruirTablaFilasConstruccionCadenaATexto();
            ReglaConstruccionCadenaATexto regla = BuscarFilaEn(tabla, parteEntera);
            string textoConCeros = GenerarCadenaConCeros(parteEntera, regla);

            TraductorATextoPesos traductorAtextoPesos = new TraductorATextoPesos();

            StringBuilder resultado = new StringBuilder();
            for (int i = 0; i < textoConCeros.Length; i += 3)
            {
                string cadDe3Digitos = textoConCeros.Substring(i, 3);
                string fragmentoNumeroEnTexto = traductorAtextoPesos.ATexto(cadDe3Digitos);


                if (fragmentoNumeroEnTexto.Equals("cero") )
                {   //no se añade  
                }

                else
                {
                    resultado.Append(fragmentoNumeroEnTexto);

                    //Las siguientes 2 lineas ubican si el contador del for i esta en una posicion de millo ó mil, etc
                    string numTemporal = textoConCeros.Substring(i, textoConCeros.Length - i);
                    ReglaConstruccionCadenaATexto reglaTemp = BuscarFilaEn(tabla, numTemporal);

                    if (reglaTemp.CantidadEspaciosEnCadenaVacia == 9)
                    {
                        if (Int32.Parse(cadDe3Digitos) == 1)
                            resultado.Append(" millon ");
                        else
                            resultado.Append(" millones ");
                    }

                    else if (reglaTemp.CantidadEspaciosEnCadenaVacia == 6)
                        resultado.Append(" mil ");
                }
            }


            if (resultado.ToString().Length == 0)
                return "cero";
            else
                return (resultado.ToString());
        }

        private List<ReglaConstruccionCadenaATexto> ConstruirTablaFilasConstruccionCadenaATexto()
        {
            ReglaConstruccionCadenaATexto fila1 = new ReglaConstruccionCadenaATexto(1, 3, 3, "");
            ReglaConstruccionCadenaATexto fila2 = new ReglaConstruccionCadenaATexto(4, 6, 6, "mil");
            ReglaConstruccionCadenaATexto fila3 = new ReglaConstruccionCadenaATexto(7, 9, 9, "millon");

            List<ReglaConstruccionCadenaATexto> tablaInfoConstruccionCadenaATexto = new List<ReglaConstruccionCadenaATexto>();
            tablaInfoConstruccionCadenaATexto.Add(fila1);
            tablaInfoConstruccionCadenaATexto.Add(fila2);
            tablaInfoConstruccionCadenaATexto.Add(fila3);

            return (tablaInfoConstruccionCadenaATexto);
        }

        private ReglaConstruccionCadenaATexto BuscarFilaEn(List<ReglaConstruccionCadenaATexto> tabla, string parteEntera)
        {
            int longBuscada = parteEntera.Length;

            Func<ReglaConstruccionCadenaATexto, bool> apuntador = (s) =>
            {
                return (longBuscada >= s.LimiteInferiorCadena && longBuscada <= s.LimiteSuperiorCadena);
            };

            ReglaConstruccionCadenaATexto fila = tabla.Single(apuntador);
            return (fila);
        }

        private string GenerarCadenaConCeros(string parteEntera, ReglaConstruccionCadenaATexto regla)
        {
            int nCeros = regla.CantidadEspaciosEnCadenaVacia - parteEntera.Length;

            StringBuilder s = new StringBuilder();
            s.Append('0',nCeros);
            s.Append(parteEntera);

            return (s.ToString());
        }


        private bool EstaEnRangoYesNumero(string cadenaNumerica)
        {
            decimal d = Decimal.Parse(cadenaNumerica);

            if ((d >= 0) && (d <= 999999999))
                return true;

            else
            {
                throw new ArgumentException("La cadena no se encuentra en el rango de operación. ");
            }
        }

        private bool ContieneElPunto(string cadenaNumerica)
        {
            if (cadenaNumerica.Contains("."))
                return true;
            else
            {
                throw new ArgumentException("La cadena no contiene el caracter punto (.) ");
            }
        }


        private string MostrarTerminacionPeso_Pesos_DePesos(string parteEntera_)
        {
            int parteEntera = Int32.Parse(parteEntera_);

            if (parteEntera == 0)
                return " pesos ";

            else if (parteEntera == 1)
                return " peso ";

            else if ((parteEntera >= 2) && (parteEntera <= 999999))
                return " pesos ";

            else if ((parteEntera >= 1000000) && (parteEntera <= 999999999))
            {
                if ((parteEntera % 1000000) == 0)                
                    return " de pesos ";
               
                else               
                    return " pesos ";               
            }

            else
               return "Terminación no considerada peso ó pesos ó de pesos"; 
        }


    }
}
