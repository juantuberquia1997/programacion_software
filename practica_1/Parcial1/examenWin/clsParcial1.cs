using System;
using System.IO;
using System.Text;

namespace libParcial1
{
    public abstract class clsPersona
    {
        private int codigo;
        private int nroDoc;
        private string nombre;
        private string error;

        public int Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

        public int NroDoc
        {
            get { return nroDoc; }
            set { nroDoc = value; }
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public string Error
        {
            get { return error; }
            set { error = value; }
        }

        protected clsPersona()
        {
            codigo = 0;
            nroDoc = 0;
            nombre = string.Empty;
            error = string.Empty;
        }

        protected clsPersona(int codigo, int nroDoc, string nombre)
        {
            this.codigo = codigo;
            this.nroDoc = nroDoc;
            this.nombre = nombre;
            error = string.Empty;
        }

        public abstract bool Grabar();
    }

    public abstract class clsEstudiante : clsPersona
    {
        private string tipoEstud;
        private string sede;
        private float n1;
        private float n2;

        public string TipoEstud
        {
            get { return tipoEstud; }
            set { tipoEstud = value; }
        }

        public string Sede
        {
            get { return sede; }
            set { sede = value; }
        }

        public float N1
        {
            get { return n1; }
            set { n1 = value; }
        }

        public float N2
        {
            get { return n2; }
            set { n2 = value; }
        }

        public clsEstudiante()
        {
            tipoEstud = string.Empty;
            sede = string.Empty;
            n1 = 0;
            n2 = 0;
        }

        public clsEstudiante(int codigo, int nroDoc, string nombre, string tipoEstud, string sede, float n1, float n2)
            : base(codigo, nroDoc, nombre)
        {
            this.tipoEstud = tipoEstud;
            this.sede = sede;
            this.n1 = n1;
            this.n2 = n2;
        }
    }

    public class clsPregrado : clsEstudiante
    {
        private string programa;
        private float n3;
        private float n4;

        public string Programa
        {
            get { return programa; }
            set { programa = value; }
        }

        public float N3
        {
            get { return n3; }
            set { n3 = value; }
        }

        public float N4
        {
            get { return n4; }
            set { n4 = value; }
        }

        public clsPregrado()
        {
            programa = string.Empty;
            n3 = 0;
            n4 = 0;
            TipoEstud = "PREGRADO";
        }

        public clsPregrado(int codigo, int nroDoc, string nombre, string sede, string programa, float n2)
            : base(codigo, nroDoc, nombre, "PREGRADO", sede, 0, n2)
        {
            this.programa = programa;
            n3 = 0;
            n4 = 0;
        }

        public void CalcularNotas()
        {
            N1 = Math.Abs(N2 - 6 * (float)Math.Sqrt(N2));
            float auxiliar1 = N2 + (float)Math.Sqrt(N1) + (N2 / 9);
            float auxiliar2 = (N2 - (N2 % 10)) / 10;
            N3 = auxiliar1 + auxiliar2;
            N4 = N3 + (5 * N2) + (N1 / 4);
        }

        public bool Validar()
        {
            if (Codigo <= 0)
            {
                Error = "El código debe ser mayor que cero";
                return false;
            }
            if (NroDoc <= 0)
            {
                Error = "El número de documento debe ser mayor que cero";
                return false;
            }
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                Error = "El nombre es obligatorio";
                return false;
            }
            if (string.IsNullOrWhiteSpace(Sede))
            {
                Error = "La sede es obligatoria";
                return false;
            }
            if (string.IsNullOrWhiteSpace(Programa))
            {
                Error = "El programa es obligatorio";
                return false;
            }
            if (N2 <= 0)
            {
                Error = "La nota 2 debe ser mayor que cero";
                return false;
            }
            if (N1 <= 0 || N3 <= 0 || N4 <= 0)
            {
                Error = "Las notas calculadas deben ser mayores que cero";
                return false;
            }
            Error = string.Empty;
            return true;
        }

        public override bool Grabar()
        {
            if (!Validar())
            {
                return false;
            }

            try
            {
                string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "estPreg.txt");

                using (StreamWriter sw = new StreamWriter(ruta, true, Encoding.UTF8))
                {
                    sw.WriteLine(Codigo + ";" + NroDoc + ";" + Nombre + ";" + TipoEstud + ";" + Sede + ";" + Programa + ";" + N1 + ";" + N2 + ";" + N3 + ";" + N4);
                }

                return true;
            }
            catch (Exception ex)
            {
                Error = "Error al grabar el archivo: " + ex.Message;
                return false;
            }
        }
    }

    public class clsExtension : clsEstudiante
    {
        private string cursos;
        private float nd;

        public string Cursos
        {
            get { return cursos; }
            set { cursos = value; }
        }

        public float Nd
        {
            get { return nd; }
            set { nd = value; }
        }

        public clsExtension()
        {
            cursos = string.Empty;
            nd = 0;
            TipoEstud = "EXTENSION";
        }

        public clsExtension(int codigo, int nroDoc, string nombre, string sede, string cursos, float n2, float nd)
            : base(codigo, nroDoc, nombre, "EXTENSION", sede, 0, n2)
        {
            this.cursos = cursos;
            this.nd = nd;
        }

        public override bool Grabar()
        {
            if (N2 <= 0 || Nd <= 0)
            {
                Error = "La nota 2 y el valor de nd deben ser mayores que cero";
                return false;
            }
            return true;
        }
    }
}
