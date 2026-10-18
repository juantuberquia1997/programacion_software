using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace libraryFigura2d
{
    public abstract class clsPoligono : clsBaseFigura2d
    {

        //atributo
         protected float fltLado1;

        // propiedades
        public float Lado1
        {
            set {
                fltLado1 = value;
            }
        }

    }

}
