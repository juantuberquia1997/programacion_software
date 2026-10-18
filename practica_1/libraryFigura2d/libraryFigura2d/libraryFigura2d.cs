using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libraryFigura2d
{
    public abstract class clsBaseFigura2d
    {
        // atributos
        protected float area;
        protected float perimetro;
        protected string error;

        // propiedades
        public float Area { get => area; }
        public float Perimetro { get => perimetro; }
        public string Error { get => error; }

        // metodos publicos
        public abstract bool hallarArea();
        public abstract bool hallarPerimetro();
    }

}
