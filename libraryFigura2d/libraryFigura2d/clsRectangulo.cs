using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace libraryFigura2d
{
    public class clsRectangulo : clsCuadrilatero
    {

        //atributo
        private float fltLado2;

        // propiedades
        public float Lado2
        {
            set
            {
                fltLado2 = value;
            }
        }


        //constructor
        public clsRectangulo()
        {
            fltLado1 = 0; 
            fltLado2 = 0;
            area = 0;
            perimetro = 0;
            error = "";
        }

        //constructor 2
        public clsRectangulo(float Lado1, float Lado2)
        {
            fltLado1 = Lado1;
            fltLado2 = Lado2;
            area = 0;
            perimetro = 0;
            error = "";
        }

        private bool Validar()
        {
            if (fltLado1 <= 0 || fltLado2 <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public override bool hallarArea()
        {
            try
            {
                if (!Validar())
                {
                    return false;
                }

                area = fltLado1 * fltLado2;

                return true;
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }


        public override bool hallarPerimetro()
        {
            try
            {
                if (!Validar())
                {
                    return false;
                }

                perimetro = 2 * fltLado1 + 2 * fltLado2;

                return true;
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }



    }

}
