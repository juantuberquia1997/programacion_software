# GUÍA DE TRABAJO — Desarrollo de Software

| Campo | Valor |
|---|---|
| Código | FDE 048 |
| Versión | 03 (elaborado por: Delio Augusto Aristizábal Martínez, versión 7, julio de 2026) |
| Fecha | 2009-06-09 |
| Institución | ITM — Institución Universitaria |
| Contacto | delioaristizabal@itm.edu.co |

---

## 1. IDENTIFICACIÓN

| Campo | Valor | Campo | Valor |
|---|---|---|---|
| Asignatura | Programación de Software | Guía No. | 1 |
| Área | Básicas de la Tecnología | Nivel | 4 |
| Código | 580304006 | Pensum | 1 |
| Correquisito(s) | 580506004 | Prerrequisito(s) | NA |
| Créditos | 4 | TPS 64 / TIS 128 | TPT / TIT |

**Trabajo independiente:** Teórico ✔ · Práctico ✔
**Trabajo presencial:** Teórico — · Práctico ✔

---

## 2. IDENTIFICACIÓN (Competencias)

| Competencias | Contenido temático | Indicador de logro |
|---|---|---|
| Conceptos y componentes de la Programación Orientada a Objetos (POO) | Uso de IDE para la implementación de aplicaciones para entornos de escritorio y Web con lenguajes propios en Programación Orientada a Objetos (POO). Crear Instancias de clase y mensajes entre objetos para interacción de funcionabilidades de clase y acciones a realizar en la aplicación. | ✓ Construir una aplicación donde se demuestre la propiedad de herencia o derivación de clases en la programación orientada a objetos, a partir de la representación UML.<br>✓ Establece las características de la clase para emplear la herencia o derivación simple, el polimorfismo y uso de la sobrecarga.<br>✓ Construir una clase haciendo uso de las características de la POO (Encapsulación, Abstracción, polimorfismo y Herencia). |

---

## 3. RECURSOS REQUERIDOS

- Sala de sistemas con 40 equipos con **Visual Studio 2026**
- 1 proyector de video y marcadores varios
- Material bibliográfico

---

## 4. PROCEDIMIENTO

### 4.1 Consultar

**Tema:** Clases Abstractas — Herencia – Polimorfismo y Sobrecarga

1. ¿Cómo se representa el modelado de una clase en UML?
2. ¿Qué es un Constructor y cuál es la funcionabilidad dentro de la clase? ¿Cómo se hace en C#?
3. ¿Qué es un modificador de acceso en una clase?
4. ¿Qué es un atributo de clase? ¿Cómo se implementa en C#?
5. ¿Qué es un descriptor de clase? ¿Cómo se implementa en C#?
6. ¿Qué hay que tener en cuenta para construir una propiedad de clase? ¿Cómo se hace en C#?
7. ¿Qué es un método de clase? ¿Qué hay que tener en cuenta para su implementación? ¿Cómo se hace en C#?
8. ¿Qué es un archivo tipo .dll? ¿Cuál es su funcionalidad básica?
9. ¿Cuál es el proceso para *referenciar* una librería de tipo .dll en una aplicación realizada en Visual Studio?
10. ¿Qué es y cómo se realiza una instancia de clase en C#?
11. ¿Cómo se emplean las propiedades y métodos del objeto instanciado (*Mensaje*)?
12. ¿Qué diferencia existe entre los tipos de datos: *double* y *float*?
13. ¿Qué es Herencia o derivación de clases?
14. ¿Qué es *polimorfismo* y el cómo se construye en una clase derivada?
15. ¿Cuáles son las diferencias entre *Polimorfismo* y *Sobrecarga* en una clase?
16. ¿Qué es una clase abstracta en POO? ¿El porqué de una clase abstracta en POO?
17. ¿Cuáles son los componentes básicos de una clase abstracta?
18. ¿Cómo se representa el modelado de una clase base y derivada en UML?
19. ¿Requisitos para construir un atributo, propiedad o método de una clase base o derivada? ¿Cómo se hace en C#?
20. ¿Qué determina en una clase base los términos: *Protected* y *Abstract*?
21. ¿En qué consiste el término: *Override* en una clase derivada?
22. ¿Qué consideraciones se deben de tener en cuenta con los constructores, cuando hay derivación de clases?
23. ¿Cómo utilizar el *StreamWriter* para leer y escribir en un archivo plano conservando los registros anteriores?
24. ¿Para qué sirve la fórmula de Herón?
25. ¿Cómo convertir radianes en grados y viceversa? El método Math.Sin() y Math.Cos()¹ devuelven un valor numérico entre -1 y 1, que representa el seno o coseno del ángulo dado en radianes. **OJO**: con esta observación (ver pie de página).

> ¹ Las funciones trigonométricas de la librería Math, en la mayoría de los lenguajes de programación, trabaja con ángulos en radianes, no en grados. Para obtener el Math.sin() o Math.cos() de un ángulo en grados, primero convertir ese ángulo a radianes y luego pasar el valor convertido a la función requerida.

---

### 4.2 Problema para solucionar

**Figura Geométrica 2D**

Crear un Proyecto WEB en C# para hallar el área y el perímetro de un rectángulo (conociendo los 2 lados), o de un rombo (conociendo la distancia menor, la distancia mayor y el valor del lado), o de un triángulo (conociendo el valor de dos de sus lados —b y c— y el ángulo A, en radianes, que forman entre ellos). Utilizar el concepto de herencia/derivación, polimorfismo y sobrecarga, creando una clase abstracta (`clsFigura2D`) con los datos y comportamientos comunes, y creando clases derivadas/heredadas para cada figura que determine el área y el perímetro acorde a los datos capturados de la figura geométrica deseada (seleccionada) y mostrándolos en la GUI.

> **Nota:** El Área y Perímetro del triángulo debe ser en grados.

#### Fórmulas

| | Triángulo | Rectángulo | Rombo |
|---|---|---|---|
| **Datos conocidos** | Usar fórmula de Herón. Lados *b*, *c* conocidos; ángulo *A* conocido | Lado *a*, lado *b* | Diagonal Mayor *D*, Diagonal Menor *d*, Lado *L* |
| **Área** | `Area = (b*c/2) * sen(A)` | `A = a * b` | `A = D * d / 2` |
| **Perímetro** | Lado desconocido *a* se calcula con el teorema del coseno:<br>`a = √(b² + c² - 2bc·cos(A))`<br>`P = a + b + c` | `P = 2a + 2b` | `P = 4 * L`<br>`L = √[(D/2)² + (d/2)²]` (Teorema de Pitágoras) |

**Suponer (casos de prueba):**
- c = 7, b = 5, A = 60 → área = 15.155 cm², perímetro = 18.24
- c = 15, b = 17, A = 120 → área = 110.42 cm², perímetro = 59.73

---

#### Actividad 1
Crear un algoritmo para solucionar el problema planteado, donde se determinen las entradas, el proceso a emplear y las salidas requeridas. Realizar la respectiva prueba de escritorio. **(Traerlo realizado)**

#### Actividad 2
Establecer el diseño tipo UML para las clases a construir con sus correspondientes componentes de entrada, salida y comportamientos, acorde al algoritmo planteado.

> **Nota:** Una clase abstracta se denota con el nombre de la clase y de los métodos abstractos con letra *itálica*. Esto indica que la clase definida no puede ser instanciada pues posee métodos abstractos (aún no han sido definidos, es decir, sin implementación). La única forma de utilizarla es definiendo subclases, que implementan los métodos abstractos definidos.
> ¿Cuál es la clase abstracta en el diseño siguiente?

**Diagrama UML de sugerencia (jerarquía de clases):**

```
                    clsBaseFigura2D
              # fltArea    : float
              # fltPerimetro : float
              # strError   : string
              + Area       : float      ↑
              + Perimetro  : float      ↑
              + Error      : string     ↑
              + hallarArea()    : bool
              + hallarPerimetro() : bool
                        △
                        │
                   clsPoligono
              # fltLado1 : float
              + Lado1     : float       ↓
                  △               △
                  │               │
           clsTriangulo      clsCuadrilatero
        - fltLado2 : float   
        - fltAngulo: float          △               △
        + Lado2 : float ↓           │               │
        + Angulo: float ↓    clsRectangulo      clsRombo
        - Validar() : bool   - fltLado2 : float  - fltDiagMy : float
                              + Lado2 : float ↓   - fltDiagMn : float
                              - Validar() : bool  + diag_Mayor : float ↓
                                                   + diag_Menor : float ↓
                                                   - Validar() : bool
```

#### Actividad 3 — Crear la biblioteca de clases (`libFigura2D.dll`) con la Clase abstracta (`abstract`): `clsFigura2D`

a. Crear una carpeta con nombre: **Practica1_Figuras2D**.

b. En Microsoft Visual Studio 2026, utilizar la opción: **Crear un Proyecto**, buscar la plantilla: **Biblioteca de clases (.NET Framework)**, con Visual C#, con nombre: **libFigura2D**, en la carpeta anterior. Clic en botón: **Crear**.

c. Cambiar el nombre de la clase `class1` por: `clsFigura2D`. (nombre del archivo físico en el proyecto).

d. En la clase interna creada por defecto, modificar para crear la clase **abstracta** (`abstract`): `clsBaseFigura2D`, acorde a la representación UML planteada en la actividad 2.

1. Crear los atributos **protegidos** (`protected`): `fltArea` (para el valor del área de la figura), `fltPerimetro` (para el valor del perímetro de la figura) y `strError` (para el mensaje de error ocurrido en la clase). Tener en cuenta el tipo de dato y el modificador de acceso.
2. Crear las propiedades de **solo salida**: `Area`, `Perimetro` y `Error`. Tener en cuenta el tipo de dato, el modificador de acceso y los descriptores de acceso para cada propiedad.
3. Crear los Métodos públicos **abstractos** (`abstract`²): `hallarArea()` y `hallarPerimetro()` que retornen un valor booleano; *verdadero* si fue ejecutado correctamente o *falso* en caso contrario. Tener en cuenta que los métodos *no* tienen implementación alguna por ser abstractos para su posterior implementación (Override).

> ² El modificador `abstract` se puede utilizar con clases, métodos, propiedades, indizadores y eventos. Use el modificador `abstract` en una declaración de clase para indicar que la clase sólo se puede utilizar como clase base de otras clases. Los miembros que están marcados como abstractos o que se incluyen en una clase abstracta, deben ser implementados por clases derivadas de la clase abstracta.

#### Actividad 4 — Agregar a la biblioteca las Clases **abstractas**: `clsPoligono` y `clsCuadrilatero`, que se derivan de la clase base abstracta: `clsBaseFigura2D`

a. Agregar la clase abstracta `clsPoligono` que se derive a partir de la clase abstracta `clsBaseFigura2D`, acorde a la representación UML planteada en la actividad 2.

1. Crear el atributo protegido (`protected`): `fltLado1` (para el valor del lado (primero)). Tener en cuenta el tipo de dato y el modificador de acceso.
2. Crear la propiedad de **solo entrada**: `Lado1`. Tener en cuenta el tipo de dato, el modificador de acceso y el descriptor de acceso para la propiedad.

b. Agregar la clase abstracta `clsCuadrilatero` que se derive a partir de la clase abstracta `clsPoligono`, acorde a la representación UML planteada en la actividad 2. Esta clase es con el fin de soportar los diferentes tipos de cuadriláteros (cuadrado y otros, en este caso, rectángulo y rombo).

#### Actividad 5 — Agregar a la biblioteca las Clases funcionales: `clsTriangulo`, `clsRectangulo` y `clsRombo`, que se derivan a partir de la clase abstracta que las precede

**a. `clsTriangulo`** (se deriva de `clsPoligono`), acorde a la representación UML planteada en la actividad 2.

1. Crear los atributos: `fltLado2` y `fltAngulo` (para el valor del 2do. lado y el ángulo conocido de la figura). Tener en cuenta el tipo de dato y el modificador de acceso.
2. Crear **dos constructores (Sobrecarga)**:
   - El primero que inicialice los atributos anteriores y los atributos heredados: `fltLado1`, `fltArea`, `fltPerimetro` y `strError` (en cero los numéricos y en blanco los de texto).
   - El segundo que tenga tres parámetros de tipo `float` con nombre: `Lado1`, `Lado2` y `Angulo` que inicialicen con sus valores de entrada los atributos: `fltLado1`, `fltLado2` y `fltAngulo` respectivamente; y el resto: en cero los numéricos y en blanco los de texto.
3. Crear las propiedades de solo entrada: `Lado2` y `Angulo`. Tener en cuenta el tipo de dato, el modificador de acceso y los descriptores de acceso para la propiedad.
4. Crear el método privado: `Validar()` que retorne un valor booleano; verdadero, si fue ejecutado correctamente o falso, en caso contrario. Este método determina si los valores recibidos son válidos (`fltLado1`, `fltLado2` y `fltAngulo`).
5. Crear los métodos públicos tipo sobrescrito (`override`³): `hallarArea()` y `hallarPerimetro()` que retornen un valor booleano; verdadero, si fue ejecutado correctamente o falso, en caso contrario.
   - **Nota1**: Tener en cuenta las fórmulas a emplear en el numeral 4.2.
   - **Nota2**: *El valor del seno del ángulo ingresado en la GUI es en grados y el resultado del seno se halla en radianes.*

> ³ Un método `override` proporciona una nueva implementación de un miembro que se hereda de una clase base.

**b. `clsRectangulo`** (se deriva de `clsCuadrilatero`), acorde a la representación UML planteada en la actividad 2.

1. Crear el atributo: `fltLado2` (para el valor del lado 2). Tener en cuenta el tipo de dato y el modificador de acceso.
2. Crear **2 constructores**: el primero que inicialice el atributo anterior y los atributos heredados: `fltLado1`, `fltArea`, `fltPerimetro` y `strError` (en cero los numéricos y en blanco los de texto); el segundo constructor que tenga dos parámetros de tipo `float` con nombre `Lado1` y `Lado2` que inicialicen con sus valores de entrada los atributos `fltLado1` y `fltLado2` respectivamente y el resto: en cero los numéricos y en blanco los de texto.
3. Crear una propiedad de solo entrada: `Lado2`. Tener en cuenta el tipo de dato, el modificador de acceso y el descriptor de acceso para la propiedad.
4. Crear el Método privado: `Validar()` que retorne un valor booleano; verdadero, si fue ejecutado correctamente o falso, en caso contrario. Este método determina si los valores recibidos, son consistentes y válidos (`fltLado1` y `fltLado2`).
5. Crear los Métodos públicos tipo sobrescrito (`override`⁴) (Polimorfismo): `hallarArea()` y `hallarPerimetro()` que retornen un valor booleano; verdadero, si fue ejecutado correctamente o falso, en caso contrario.
   - **Nota**: Tener en cuenta las fórmulas a emplear en el numeral 4.2.

> ⁴ Un método `override` proporciona una nueva implementación de un miembro que se hereda de una clase base.

**c. `clsRombo`** (se deriva de `clsCuadrilatero`), acorde a la representación UML planteada en la actividad 2.

1. Crear los atributos: `fltDiagMy` (para la diagonal mayor de la figura) y `fltDiagMn` (para la diagonal menor de la figura). Tener en cuenta el tipo de dato y el modificador de acceso.
2. Crear **dos constructores (Sobrecarga)**: el primero que inicialice los atributos propios y los atributos heredados: `fltLado1`, `fltArea`, `fltPerimetro` y `strError` (en cero los numéricos y en blanco los de texto); el segundo que tenga tres parámetros de tipo `float` con nombres: `lado`, `diag_Mayor` y `diag_Menor` que inicialicen los atributos `fltLado1`, `fltDiagMy` y `fltRadMn` con el resto: en cero los numéricos y en blanco los de texto.
3. Crear las propiedades de solo entrada: `diag_Mayor` y `diag_Menor`. Tener en cuenta el tipo de dato, el modificador de acceso y los descriptores de acceso para la propiedad.
4. Crear el Método privado: `Validar()` que retorne un valor booleano; verdadero, si fue ejecutado correctamente o falso, en caso contrario. Este método determina si los valores recibidos, son consistentes y válidos.
5. Crear los Métodos públicos tipo sobrescrito (`override`) (Polimorfismo): `hallarArea()` y `hallarPerimetro()` que retornen un valor booleano; verdadero, si fue ejecutado correctamente o falso, en caso contrario.
   - **Nota**: Tener en cuenta las fórmulas a emplear en el numeral 4.2.

#### Actividad 6 — Aplicación para entorno WEB para realizar la captura y muestra de datos, utilizando las librerías de clases implementadas anteriormente

a. En Microsoft Visual Studio 2026, utilizar la opción: **Crear un Proyecto**, buscar la plantilla: **Aplicación web ASP.NET (.NET Framework)** en C#, con nombre: **webFig2D**, en la subcarpeta creada en la actividad 3(a). Clic botón: **Crear**.

Te lleva a la ventana de: *Crear una aplicación web ASP.NET*, en ella seleccionar: **Empty** (Vacío) y chequear la opción: **Formularios Web Forms**, y desmarcar la opción: **Configuración para HTTPS**, luego: **Aceptar**.

b. Crear una carpeta llamada: **Imagenes**, en ella adicionar las imágenes suministradas por el docente (`triangulo.JPG`, `rectangulo.jpg` y `rombo.JPG`).

c. Agregar un nuevo elemento, C#, WEB, de tipo plantilla: **Formulario Web Forms** con nombre: **frmHerencia**.

d. En `frmHerencia` diseñar los objetos de entrada y salida propuestos en la siguiente imagen, utilizando una tabla de 13*1, con ancho del 80%, centrada.

**Form:** `frmHerencia` — (Título): *Práctica # 2 – Herencia y Polimorfismo*

| Línea | Elemento | Detalle |
|---|---|---|
| 1ra | (Text) | Cálculos Figuras Geométricas 2D |
| 3ra | `RadioButtonList` **rblFiguras** | Opciones: Triángulo (`opcTri`, *Selected=true*), Rectángulo (`opcRec`), Rombo (`opcRom`). `RepeatDirection: Horizontal`, `RepeatLayout: Flow` |
| 4ta | (image) **imgFigura** | `Width=150px`, `height=150px` |
| 5ta | Label **lblMsj** | sin texto, forecolor: Rojo, Bold |
| 7ma | **pnlTriangulo** | tabla 5*2, 80%, centrada, `Visible: true`<br>2da línea — TextBox: `txtLadoA`<br>3ra línea — TextBox: `txtLadoB`<br>4ta línea — TextBox: `txtAngulo` |
| 8va | **pnlRectangulo** | tabla 4*2, 80%, centrada, `Visible: false`<br>2da línea — TextBox: `txtLado1`<br>3ra línea — TextBox: `txtLado2` |
| 9na | **pnlRombo** | tabla 5*2, 80%, centrada, `Visible: false`<br>2da línea — TextBox: `txtDiagMy`<br>3ra línea — TextBox: `txtDiagMn`<br>4ta línea — TextBox: `txtLado` (`ReadOnly: True`) |
| 11va | Button: **btnCalcular** | |
| 13va | **pnlRpta** | tabla 3*2, 80%, centrada, `Visible: false`<br>Label: **lblArea** sin texto<br>Label: **lblPerim** sin texto<br>Button: **btnLimpiar** | |

#### Actividad 7 — Integración GUI y Clase, funcionamiento de la aplicación

a. Referenciar y usar la librería `libFigura2D` creada en la actividad 3.

b. Por cada opción de figura geométrica seleccionada, hacer que aparezca la imagen guía y el panel de captura de datos correspondiente.

c. Codificar los botones: **Limpiar** y **Calcular** de la GUI.
- El botón **Limpiar**, debe limpiar los TextBox de captura y Labels de respuestas y de Mensaje.
- El botón **Calcular**, acorde a la opción de figura geométrica seleccionada en el RadioButtonList, debe capturar los datos, crear la instancia de clase acorde a la figura, enviar los datos ya sea por las propiedades o utilizando el constructor, invocar los métodos del objeto (`hallarArea()` y `hallarPerimetro()`), realizar el tratamiento de error o recuperación de datos obtenidos (Área y Perímetro) para ser mostrados en la GUI.

d. Realizar pruebas y correcciones necesarias hasta el correcto funcionamiento de la aplicación.

---

### Código ya resuelto de ejemplo — `clsTriangulo`

```csharp
public override bool hallarArea()
{
    if (!validar())
        return false;
    try
    {
        fltArea = (float)(Math.Sin(fltAngulo * Math.PI / 180)) * fltLado1 * fltLado2 / 2;
        return true;
    }
    catch
    {
        strError = "Error en proceso Área";
        return false;
    }
}
```

Fórmula empleada (teorema del coseno, para hallar el lado desconocido):

```
a = √(b² + c² - 2bc·cos(A))
```

```csharp
public override bool hallarPerimetro()
{
    if (!validar())
        return false;

    float fltLado3 = 0;
    try
    {
        fltLado3 = (float)Math.Sqrt(Math.Pow(fltLado1, 2) + Math.Pow(fltLado2, 2) -
                    (2f * fltLado1 * fltLado2 * Math.Cos(fltAngulo * Math.PI / 180)));
        fltPerimetro = fltLado1 + fltLado2 + fltLado3;
        return true;
    }
    catch
    {
        strError = "Error en proceso Perímetro";
        return false;
    }
}
```

### Código ya resuelto de ejemplo — `frmHerencia`

```csharp
private void cargarImagen(string nombre)
{
    string dir = "~/Imagenes/" + nombre;
    this.imgFigura.ImageUrl = dir;
}

private float hallarLado(float a, float b)
{
    return (float)Math.Sqrt(Math.Pow(a / 2, 2) + Math.Pow(b / 2, 2));
}
```

---

#### Actividad 8 (Ejercicios)

**A) Crear una aplicación con herencia para el siguiente diseño:**

```
                          Persona
        # strDocumento : string
        # strNombres   : string
        # dtmFechaNac  : DateTime
        # strTelefono  : string
        # strError     : string
        + Documento : string     ↕
        + Nombre    : string     ↕
        + FechaNac  : DateTime   ↕
        + Telefono  : string     ↕
        + Error     : string     ↑
        + Grabar()  : bool
              △                    △
              │                    │
         Estudiante              Empleado
   # strFacultad : string    - strCargo   : string
   + Facultad : string ↕     - intSalario : Int32
                             + Cargo   : string ↕
                             + Salario : Int32  ↕
        △            △
        │            │
    PreGrado      Extensión
 - strPrograma:string  - strCurso : string
 - strCarnet  :string  + Curso : string ↕
 + Programa : string ↕
 + Carnet   : string ↕
```

El método `Grabar()` consiste en grabar la información capturada en el formulario en un archivo plano, ya sea de texto (txt) o tipo XML.

**B) Crear una aplicación con herencia para el siguiente diseño:**

```
   Institución                Persona
   Nombre                     Nro. Doc
   Dirección                  Apellido
   Registrar()                Telefono
   Consultar()                Genero
   Resumen()                  Salario
                                  △
                    ┌─────────────┼─────────────┐
              Administrativo   Docente      Estudiante
              Área             Tipo         Carné
              Cargo            Categoría    Programa
              Extensión        Horas
```

En la clase *Institución*: el método `Registrar()` está pensado para grabar en un archivo plano de tipo txt o xml la información capturada de un administrativo, docente o estudiante; el método `Consultar()` está pensado para realizar una consulta acorde al nro. de documento del administrativo, docente o estudiante; y el método `Resumen()` muestra toda la información existente de todos los administrativos, docentes o estudiantes en el formulario.

**C) Crear una aplicación con herencia para el siguiente diseño (modelo tipo base de datos):**

```
                          persona
                  # id     BIGINT(20)
                  ○ edad   INT(11)
                  ○ nombre VARCHAR(255)
                       △
              ┌────────┴────────┐
          tecnologo           normal
     ○ aniosDeEstudios INT(11)  ○ ocupacion VARCHAR(255)
     # id BIGINT(20)            # id BIGINT(20)
          △
     ┌────┴─────┐
 programador   tester
 ○ aniosDeExperiencia INT(11)   ○ herramientaDeTesteo VARCHAR(255)
 ○ lenguajeFavorito VARCHAR(255) # id BIGINT(20)
 # id BIGINT(20)
```

---

## 5. PARÁMETROS PARA ELABORACIÓN DEL INFORME

N/A.

---

## 6. BIBLIOGRAFÍA

- Deitel, Harvey M.; Deitel, Paul J. *Cómo programar en C#*. 2. Ed., México: Pearson Educación, 2007, 1166 p. 1CD ROM, ISBN: 9702610567, (ITM) 005.133 D325
- Charte Ojeda, Francisco; Serrano Pérez, Jorge. *Programación con Visual Studio .NET*, Madrid: Anaya Multimedia, 2002, 654 p. + CD-ROM, ISBN: 8441513767, (ITM) 005.133 C486
- Kingsley-Hughes, Adrian; Kingsley-Hughes, Kathie. *C# 2005: Aprendizaje y referencia*, Madrid: Anaya Multimedia, 2007, 448 p, ISBN: 9788441522381, (ITM) 005.133 K55
- Bradley, Julia Case; Millspaugh, Anita C. *Programming in C# .NET*, New York: McGraw-Hill, 2003, 556p. + CD-ROM, ISBN: 0071215646, (ITM) 005.133 B811
- Wright, Charles. *Superutilidades para C#*, Madrid: McGraw-Hill, 2003, 639p, ISBN: 9789701047834, (ITM) 005.133 W948
- Ramírez, Felipe. *Introducción a la programación: algoritmos y su implementación en VB.NET, C#, Java y C++*. 2. Ed., México: Alfaomega, 2007, 488 p, ISBN: 9789701512807, (ITM) 005.1 R173
- Charte Ojeda, Francisco. *Visual C# 2005*, Madrid: Anaya Multimedia, 2007, 351 p, ISBN: 8441520836, (ITM) 005.133 C486
- Foxall, James. *Visual C# 2005*, Madrid: Anaya Multimedia, 2007, 559 p, ISBN: 9788441521216, (ITM) 005.133 F792
- Hoffman, Kevin. *Visual C# 2005*, Madrid: Anaya Multimedia, 2007, 732 p, ISBN: 8441520984, (ITM) 005.133 H699
- Ceballos Sierra, Francisco Javier. *El lenguaje de programación C#*, México: Alfaomega, 2002, 302 p. CD-ROM, ISBN: 9701508017, (ITM) 005.133 C387
- Wille, Christoph. *C#*, Madrid: Prentice-Hall, 2001, 187 p, ISBN: 8420531138, (ITM) 005.133 W698
- Ceballos Sierra, Francisco Javier. *Enciclopedia de Microsoft Visual C#*, México: McGraw-Hill, 2006, 936 p. + CD-ROM, ISBN: 9701512170, (ITM) 005.133 C387e
- <http://www.universoformulas.com/matematicas/trigonometria/area-triangulo-razones-trigonometricas/> (Ejemplo Área)
- <http://www.universoformulas.com/matematicas/trigonometria/resolucion-triangulos/>
- <https://www.disfrutalasmatematicas.com/geometria/elipse-perimetro.html>
- <http://www.universoformulas.com/matematicas/geometria/area-elipse/>

---

**Elaborado por:** Delio Augusto Aristizábal Martínez
**Versión:** 7
**Fecha:** Julio de 2026
**Aprobado por:** _(pendiente)_
