using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace libraryFigura2d
{
    public class clsRombo : clsCuadrilatero
    {

        //atributo
        private float fltDiagMayor;
        private float fltDiagMenor;


        // propiedades
        public float diag_mayor
        {
            set
            {
                fltDiagMayor = value;
            }
        }

        public float diag_menor
        {
            set
            {
                fltDiagMenor = value;
            }
        }

        private bool Validar()
        {
            if (fltDiagMayor <= 0 || fltDiagMenor <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public clsRombo()
        {
            fltDiagMayor = 0;
            fltDiagMenor = 0;
            fltLado1 = 0;
            area = 0;
            perimetro = 0;
            error = "";
        }

        public clsRombo(float lado, float diag_mayor, float diag_menor)
        {
            fltDiagMayor = diag_mayor;
            fltDiagMenor = diag_menor;
            fltLado1 = lado;
            area = 0;
            perimetro = 0;
            error = "";
        }


        //pendiente calculo
        public override bool hallarArea()
        {
            try
            {
                if (!Validar())
                {
                    return false;
                }

                area = (fltDiagMayor * fltDiagMenor) / 2;

                return true;
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }

        //pendiente calculo
        public override bool hallarPerimetro()
        {
            try
            {
                if (!Validar())
                {
                    return false;
                }

                fltLado1 = (float)Math.Sqrt(Math.Pow(fltDiagMayor / 2, 2) + Math.Pow(fltDiagMenor / 2, 2));
                perimetro = 4 * fltLado1;

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
  