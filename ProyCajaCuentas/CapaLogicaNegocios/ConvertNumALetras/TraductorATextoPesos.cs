using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogicaNegocios.ConvertNumALetras
{
    public class TraductorATextoPesos
    {


        //-----------constructor
        public TraductorATextoPesos()
        {

        }


        //----------------utils
        //se le envia una cadena de 3 digitos por ejemplo 001, 045, 099, 876, 000, lo regresa en 
        //letra en el contexto de que se esta manejando dinero.
        //el rango es 000 a 999

        public string ATexto(string cadDe3Digitos)
        {
            if (RangoEs_0_A_99(cadDe3Digitos))
                return (ConvierteUnidadesyDecenasATexto(cadDe3Digitos));

            else
                return (ConvierteCentenasATexto(cadDe3Digitos));           
        }


        private string ConvierteUnidadesyDecenasATexto(string cadDe3Digitos)
        {
            //convierte de 00 a 99

            if(cadDe3Digitos[1] == '0')
            {  //convierte 00 al 09
                string texto = ConvierteUnidadesATexto(cadDe3Digitos);
                return (texto);
            }

            else if(cadDe3Digitos[1] == '1')
            {  //convierte 10 al 19
                string texto = Convierte_10al_19ATexto(cadDe3Digitos);
                return (texto);
            }

            else if( EsDecenaCerrada(cadDe3Digitos) )
            {
                //convierte 20,30,40,50,60,70,80,90
                string texto = Convierte20_30_40EtcATexto(cadDe3Digitos);
                return (texto);
            }

            else
            {
                string texto = Convierte21_29_31_39_EtcATexto(cadDe3Digitos);
                return (texto);
            }

        } 


        private string Convierte21_29_31_39_EtcATexto(string cadDe3Digitos)
        {
            Hashtable digitos1 = new Hashtable();
            digitos1.Add("2", "veinti");
            digitos1.Add("3", "treinta");
            digitos1.Add("4", "cuarenta");
            digitos1.Add("5", "cincuenta");
            digitos1.Add("6", "sesenta");
            digitos1.Add("7", "setenta");
            digitos1.Add("8", "ochenta");
            digitos1.Add("9", "noventa");

            Hashtable digitos2 = new Hashtable();
            digitos2.Add("1", "un");
            digitos2.Add("2", "dos");
            digitos2.Add("3", "tres");
            digitos2.Add("4", "cuatro");
            digitos2.Add("5", "cinco");
            digitos2.Add("6", "seis");
            digitos2.Add("7", "siete");
            digitos2.Add("8", "ocho");
            digitos2.Add("9", "nueve");


            string key1 = cadDe3Digitos[1].ToString();
            string texto = digitos1[key1].ToString();

            if(cadDe3Digitos[1].ToString() == "2")
            {
                //hacer nada
            }

            else
            {
                texto += " y ";
            }

            string key2 = cadDe3Digitos[2].ToString();
            texto += digitos2[key2].ToString();

            return (texto);
        }


        private bool EsDecenaCerrada(string cadDe3Digitos)
        {
            //ver si es 20 ó 30 ó 40 ó 50 ó 60 ó 70 ó 80 ó 90
            int numero = Int32.Parse(cadDe3Digitos[1].ToString());
            int numero2 = Int32.Parse(cadDe3Digitos[2].ToString());

            if(  (numero >=2)   && (numero<= 9)  && (numero2==0)  )
            {
                return true;
            }

            else
            {
                return false;
            }
        }


        private string Convierte20_30_40EtcATexto(string cadDe3Digitos)
        {
            Hashtable digitos = new Hashtable();
            digitos.Add("20", "veinte");
            digitos.Add("30", "treinta");
            digitos.Add("40", "cuarenta");
            digitos.Add("50", "cincuenta");
            digitos.Add("60", "sesenta");
            digitos.Add("70", "setenta");
            digitos.Add("80", "ochenta");
            digitos.Add("90", "noventa");

            string clave = cadDe3Digitos[1].ToString() + cadDe3Digitos[2].ToString();

            return (digitos[clave].ToString());
        }


        private string Convierte_10al_19ATexto(string cadDe3Digitos)
        {
            Hashtable digitos = new Hashtable();
            digitos.Add("10", "diez");
            digitos.Add("11", "once");
            digitos.Add("12", "doce");
            digitos.Add("13", "trece");
            digitos.Add("14", "catorce");
            digitos.Add("15", "quince");
            digitos.Add("16", "dieciseis");
            digitos.Add("17", "diecisiete");
            digitos.Add("18", "dieciocho");
            digitos.Add("19", "diecinueve");

            string clave = cadDe3Digitos[1].ToString() + cadDe3Digitos[2].ToString();
            return (digitos[clave].ToString());
        }


        private string ConvierteUnidadesATexto(string cadDe3Digitos)
        {
            Hashtable digitos = new Hashtable();
            digitos.Add("0", "cero");
            digitos.Add("1", "un");
            digitos.Add("2", "dos");
            digitos.Add("3", "tres");
            digitos.Add("4", "cuatro");
            digitos.Add("5", "cinco");
            digitos.Add("6", "seis");
            digitos.Add("7", "siete");
            digitos.Add("8", "ocho");
            digitos.Add("9", "nueve");

            string clave = cadDe3Digitos[2].ToString();
            return (digitos[clave].ToString());
        }

        private string ConvierteCentenasATexto(string cadDe3Digitos)
        {
            Hashtable digitos = new Hashtable();
            digitos.Add("1", "cien");
            digitos.Add("2", "doscientos");
            digitos.Add("3", "trescientos");
            digitos.Add("4", "cuatrocientos");
            digitos.Add("5", "quinientos");
            digitos.Add("6", "seiscientos");
            digitos.Add("7", "setecientos");
            digitos.Add("8", "ochocientos");
            digitos.Add("9", "novecientos");

            string texto;
            if(   (cadDe3Digitos[1] == '0')  &&  (cadDe3Digitos[2] == '0')  )
            {
                //Convierte 100,200,300,400...900
                string key = cadDe3Digitos[0].ToString();
                texto = digitos[key].ToString();
                return (texto);
            }

            else
            {
                string key = cadDe3Digitos[0].ToString();
                texto = digitos[key].ToString();

                if (texto == "cien")
                { texto += "to"; }
                texto += " ";
                texto += ConvierteUnidadesyDecenasATexto(cadDe3Digitos);

                return (texto);
            }
        }

        private bool RangoEs_0_A_99(string cadDe3Digitos)
        {
            int n = Int32.Parse(cadDe3Digitos);
            if (n >= 0 && n <= 99)
                return true;
            else
                return false;
        }

    }
}
