using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libraryFigura2d
{
    public class clsTriangulo : clsPoligono
    {
        // atributos
        private float fltLado2;
        private float fltAngulo;
        
        // propiedades
        public float Lado2 { set => fltLado2 = value; }
        public float Angulo { set => fltAngulo = value; }

        // metodos 
        private bool Validar() {

            if (fltLado1 <= 0 || fltLado2 <= 0 || fltAngulo <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
          
        }

        // constructor 1
        public clsTriangulo()
        {
            area = 0;
            perimetro = 0;
            error = "";
            fltLado1= 0;
            fltLado2 = 0;
            fltAngulo = 0;
        }

        // constructor 2
        public clsTriangulo(float Lado1, float Lado2, float angulo)
        {
            area = 0;
            perimetro = 0;
            error = "";
            fltLado1 = Lado1;
            fltLado2 = Lado2;
            fltAngulo = angulo;

        }

        //polimorfismo para hallar el area de un triangulo
        public override bool hallarArea() {

            try
            {
                if (!Validar()) {
                    return false;
                }

                // math usa radianes y el usuario ingresa grados, se debe convertir a radianes
                area = (float)(Math.Sin(fltAngulo * Math.PI / 180)) * fltLado1 * fltLado2 / 2;

                return true;
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }

        //polimorfismo para hallar el perimetro del triangulo
        public override bool hallarPerimetro()
        {
            try
            {
                if (!Validar())
                {
                    return false;
                }

                float fltLado3 = (float)Math.Sqrt(
                    Math.Pow(fltLado1, 2) + Math.Pow(fltLado2, 2) -
                    (2f * fltLado1 * fltLado2 * Math.Cos(fltAngulo * Math.PI / 180)));
                perimetro = fltLado1 + fltLado2 + fltLado3;

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
