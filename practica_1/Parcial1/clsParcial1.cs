using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsParcial1
{
    public abstract class clsPersona
    {
        protected int codigo;
        protected int nroDoc;
        protected string Nombre;
        protected string Error;

        // metodos publicos
        public abstract bool Grabar();
    }


    public abstract class clsEstudiante : clsPersona
    {

        //atributo
        protected string tipoEstud;
        protected string sede;
        protected float n1;
        protected float n2;

        public float N2 {  set => n2 = value; }
        public float N1 { set => n1 = value; }
    }


    public class clsPregrado : clsEstudiante
    {
        // atributos
        private string programa;
        private float n3;
        private float n4;

        public float N3 { set => n3 = value; }
        public float N4 { set => n4 = value; }


        public override bool Grabar()
        {

            if (n3 <= 0 || n4 <= 0 || n1 <= 0 || n2<= 0)
            {
                return false;
            }
            else {
                return true;
            }
        }

        
        public clsPregrado(float nota1, float nota2, float nota3, float nota4)
        {
            n1 = nota1;
            N2 = nota2;
            n3 = nota3;
            n4 = nota4;
            programa = "";
            sede = "";
            tipoEstud = "";
            codigo = 0;
            nroDoc = 0;
            Nombre = string.Empty;
            Error = string.Empty;

        }

        public clsPregrado()
        {
            n1 = 0;
            N2 = 0;
            n3 = 0;
            n4 = 0;
            programa = "";
            sede = "";
            tipoEstud = "";
            codigo = 0;
            nroDoc = 0;
            Nombre = "";
            Error = string.Empty;

        }
        
        

    }


    public class clsExtension : clsEstudiante
    {

        public string curso;
        public float nd;

        public override bool Grabar()
        {

            if (n2 <= 0 )
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }

    
}
