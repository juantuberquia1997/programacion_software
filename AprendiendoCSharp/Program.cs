// ============================================================================
//  Aprendiendo C# - Boilerplate educativo
//  Estructura: un archivo Program.cs con ejemplos comentados por secciones.
//  Ejecutar:  dotnet run, polimorfismo metodo especifico para algo-override, sobrecarga, clases abtractas
//  - el constructor y todo lo privado no se heradan
//  - # modificador de acceso protected es un atributo, que solo es visible y usar ella misma y sus hijas top-down
// - jerarquia de clases
// ============================================================================

using System;
l
namespace AprendiendoCSharp
{
    // ========================================================================
    //  SECCIÓN 1: Clases y objetos (POO básica)
    // ========================================================================
    public class Persona
    {
        // Propiedades auto-implementadas (con get/set)
        public string Nombre { get; set; }
        public int Edad { get; set; }

        // Constructor
        public Persona(string nombre, int edad)
        {
            Nombre = nombre;
            Edad = edad;
        }

        // Método de instancia
        public void Saludar()
        {
            Console.WriteLine($"Hola, soy {Nombre} y tengo {Edad} años.");
        }
    }

    // ========================================================================
    //  SECCIÓN 2: Herencia y polimorfismo
    // ========================================================================
    public class Estudiante : Persona
    {
        public string Carrera { get; set; }

        // Constructor que llama al constructor base
        public Estudiante(string nombre, int edad, string carrera)
            : base(nombre, edad)
        {
            Carrera = carrera;
        }'

        // Override de un método virtual (podrías sobrescribir Saludar)
        public void Presentarse()
        {
            Console.WriteLine($"Soy {Nombre}, estudio {Carrera}.");
        }
    }

    // ========================================================================
    //  SECCIÓN 3: Enumeraciones
    // ========================================================================
    public enum EstadoAnimo
    {
        Feliz,
        Triste,
        Motivado,
        Cansado
    }

    // ========================================================================
    //  Punto de entrada de la aplicación
    // ========================================================================
    class Program
    {
        static void Main(string[] args)
        {
            // ----------------------------------------------------------------
            //  SECCIÓN 4: Variables y tipos de datos
            // ----------------------------------------------------------------
            int entero = 42;
            double decimalDoble = 3.1416;
            bool esVerdad = true;
            char letra = 'A';
            string texto = "Hola C#";

            // var: el compilador infiere el tipo
            var inferido = "soy un string";

            // Constantes
            const double PI = 3.14159;

            Console.WriteLine("=== Tipos de datos ===");
            Console.WriteLine($"int: {entero}, double: {decimalDoble}, bool: {esVerdad}");
            Console.WriteLine($"char: {letra}, string: {texto}, var: {inferido}, const: {PI}");

            // ----------------------------------------------------------------
            //  SECCIÓN 5: Control de flujo
            // ----------------------------------------------------------------
            Console.WriteLine("\n=== Control de flujo ===");

            if (entero > 10)
                Console.WriteLine("entero es mayor que 10");
            else
                Console.WriteLine("entero es menor o igual a 10");

            // switch con enum
            EstadoAnimo animo = EstadoAnimo.Motivado;
            switch (animo)
            {
                case EstadoAnimo.Feliz:
                    Console.WriteLine("¡Qué bueno que estás feliz!");
                    break;
                case EstadoAnimo.Motivado:
                    Console.WriteLine("¡A programar!");
                    break;
                default:
                    Console.WriteLine("Ánimo igual.");
                    break;
            }

            // Bucles
            Console.WriteLine("\n=== Bucles ===");
            for (int i = 1; i <= 3; i++)
                Console.WriteLine($"for: {i}");

            int contador = 0;
            while (contador < 3)
            {
                Console.WriteLine($"while: {contador}");
                contador++;
            }

            // ----------------------------------------------------------------
            //  SECCIÓN 6: Colecciones
            // ----------------------------------------------------------------
            Console.WriteLine("\n=== Colecciones ===");

            // Arreglo
            int[] numeros = { 1, 2, 3, 4, 5 };

            // Lista (List<T>)
            List<string> nombres = new List<string> { "Ana", "Luis", "María" };
            nombres.Add("Pedro");

            foreach (var n in nombres)
                Console.WriteLine($"- {n}");

            // ----------------------------------------------------------------
            //  SECCIÓN 7: Métodos
            // ----------------------------------------------------------------
            Console.WriteLine("\n=== Métodos ===");
            int suma = Sumar(3, 4);
            Console.WriteLine($"Sumar(3, 4) = {suma}");

            // Método con parámetros opcionales
            SaludarPersona("Carlos");
            SaludarPersona("Carlos", "Programación");

            // ----------------------------------------------------------------
            //  SECCIÓN 8: Clases y objetos en acción
            // ----------------------------------------------------------------
            Console.WriteLine("\n=== POO ===");
            Persona p = new Persona("María", 25);
            p.Saludar();

            Estudiante e = new Estudiante("Juan", 20, "Ingeniería");
            e.Saludar();      // heredado
            e.Presentarse();  // propio

            // ----------------------------------------------------------------
            //  SECCIÓN 9: Manejo de excepciones
            // ----------------------------------------------------------------
            Console.WriteLine("\n=== Excepciones ===");
            try
            {
                int divisor = 0;
                int resultado = 10 / divisor;
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine($"Error capturado: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Bloque finally siempre se ejecuta.");
            }

            Console.WriteLine("\n¡Listo! Revisá el código comentado para aprender cada concepto.");
        }

        // Método estático con retorno
        static int Sumar(int a, int b)
        {
            return a + b;
        }

        // Método con parámetro opcional (curso = "Desconocida")
        static void SaludarPersona(string nombre, string curso = "Desconocida")
        {
            Console.WriteLine($"Hola {nombre}, estás en {curso}.");
        }
    }
}
