# INSTITUCIÓN UNIVERSITARIA ITM
**GUÍA DE TRABAJO - TECNOLOGÍA EN DESARROLLO DE SOFTWARE**

* **Código:** FDE 048
* **Versión:** 03
* **Fecha:** 2009-06-09
* **Guía No.:** 4-2
* **Asignatura:** Programación de Software
* **Área:** Básicas de la Tecnología | **Nivel:** 4 | **Pensum:** 1
* **Código Asignatura:** 580304006 | **Correquisito(s):** 580506004 | **Prerrequisito(s):** NA
* **Créditos:** 4 | **TPS:** 64 | **TIS:** 128 | **TPT / TIT:** N/A
* **Docente / Contacto:** `delloaristizabal@itm.edu.co`
* **Contenido total:** Páginas 1 a 19 (Actividades 1.1 a 1.9 completas)

---

## 1. IDENTIFICACIÓN Y MAQUETACIÓN VISUAL

### Formulario: `frmInscrip` (Página 1)

**Diseño de controles de entrada y disposición en filas:**

* **Título:** Maestro de Inscripciones
* **Texto interno:** Maestro de Inscripción

#### En 1er. Row:
* `text: txtCodigo (2)` — `class="label_1", disabled`
* `select: cboEvento (4)`
* `label: lblVrIndInsc (2)` — `"label_2", disabled` (Texto: Vr. Inscrip. C/U:)
* `label: lblFechaInsc (2)` — `"label_2", disabled` (Texto: Fecha Inscrip:)
* `label: lblFechaAct (2)` — `"label_2", disabled` (Texto: Fecha actual:)

#### En 2do. Row:
* `select: cboBanda (4)` (Texto: Banda)
* `text: txtRepres (4)` (Texto: ¿Quién Inscribe? / Ingrese nombre del Representante...)
* `text: txtTelRep (2)` (Texto: Teléfono / Teléfono representante)
* `text: txtValorTot (2)` — `"label_1", disabled` (Texto: Valor Total Inscr.)

#### En 3er. Row:
* `table: tblDatosDetalle (12)`
* `select: cboArtBand (6)` (Texto: Banda/Artista)
* `text: txtInstrum (4)` — `"label_1", disabled` (Texto: Instrumento)
* `text: txtEdad (2)` (Texto: Edad / Ingrese edad...)

#### Menú de Acciones:
* Botones: `btnAgre (2)`, `btnModi (2)`, `btnBusc (2)`, `btnCanc (4)` (Cancelar/Limpiar) y `btnImpr (2)` (Impresión).
* Tabla de consulta: `tabla: tablaDatos`, con 8 `<th>...</th>`.

#### Recursos a Reclamar:
* Imágenes, Estilos CSS, Sección Tabla: `seccionDetalleMae`.

---

## ACTIVIDAD 1. ACCIONES DE EVENTO - API

### 1.1. Acciones de: Evento (Llenar combo y valor individual de inscripción para el evento)
*(Página 1 y 2)*

#### a. Crear clase operativa: `clsOpeEvento`
* **i.** En la carpeta: `Clases`, crear (agregar) la clase: `clsOpeEvento`.
* **ii.** Usar la carpeta: `Models` del proyecto: `using apiFestRock.Models;`.
* **iii.** Crear el atributo de tipo context de la base de datos: `ctx`:
  ```csharp
  private readonly BdEventosContext ctx;
  ```
* **iv.** Crear el constructor con un parámetro de tipo context de la base de datos llamado, `cntxt`:
  ```csharp
  public clsOpeEvento(BdEventosContext cntxt) {
      ctx = cntxt;
  }
  ```
* **v.** Crear el método: `listaEventos()` que retorna una lista con los id y los nombres de los eventos que se encuentran activos en la tabla de la base de datos:
  ```csharp
  public List<TblEvento> listaEventos() {
      return ctx.TblEventos // Tabla del modelo Entity
          .OrderBy(x => x.Codigo)
          .Where(x => x.Activo == true)
          .ToList();
  }
  ```
* **vi.** Crear el método: `consultaVrInscrip` para consultar el valor de un evento acorde a su código:
  ```csharp
  public IQueryable consultaVrInscrip(int idEvent) {
      return from ev in ctx.TblEventos
             where ev.Codigo == idEvent
             select new {
                 vrIndInsc = ev.VrInscEvent
             };
  }
  ```

#### b. Crear controlador: `eventoController`
*(Página 2)*
* **i.** Sobre la carpeta `Controllers`, agregar un Controller de tipo: *Controller de MVC: en blanco*, con nombre: `eventoController` (cambiar el texto Home por evento, no borrar la palabra Controller).
* **ii.** Usar las carpetas: `Models` y `Clases` del proyecto:
  ```csharp
  using apiFestRock.Clases;
  using apiFestRock.Models;
  ```
* **iii.** Colocar la ruta del controlador y el `ApiController`, antes de la clase:
  ```csharp
  [Route("api/[controller]")]
  [ApiController]
  ```
* **iv.** Crear el atributo privado con nombre: `ctx`, que apunta a la base de datos EF Context:
  ```csharp
  private readonly BdEventosContext ctx;
  ```
* **v.** Crear el constructor con un parámetro de tipo context llamado `context`:
  ```csharp
  public eventoController(BdEventosContext context) {
      ctx = context;
  }
  ```
* **vi.** Crear el servicio asincrónico `HttpGet` con alias `listadoEventos` con nombre `listarEventos` y retorno tipo: `List` al invocar el método: `listaEventos` de la clase:
  ```csharp
  [HttpGet("listadoEventos")]
  public List<TblEvento> listarEventos() {
      clsOpeEvento oX = new clsOpeEvento(ctx);
      return oX.listaEventos();
  }
  ```
* **vii.** Crear el servicio asincrónico `HttpGet` con alias `"valorEvento"` con nombre `valorInsc` y retorno tipo: `List` al invocar el método: `consultaVrInscrip` de la clase:
  ```csharp
  [HttpGet("valorEvento")]
  public IQueryable valorInsc(int idEv) {
      clsOpeEvento oX = new clsOpeEvento(ctx);
      return oX.consultaVrInscrip(idEv);
  }
  ```
* **c. Ejecutar el proyecto y probar el servicio con el software: Swagger** que se abre al ejecutar la API (mirar link).

---

### 1.2. Acciones de: Banda (Llenar combo)
*(Página 2)*

#### a. Crear clase operativa: `clsOpeBanda`
* **i.** En la carpeta: `Clases`, crear (agregar) la clase: `clsOpeBanda`.
* **ii.** Usar la carpeta: `Models` del proyecto: `using apiFestRock.Models;`.
* **iii.** Crear el atributo de tipo context de la base de datos: `ctx`:
  ```csharp
  private readonly BdEventosContext ctx;
  ```
* **iv.** Crear el constructor con un parámetro de tipo context de la base de datos llamado, `cntxt`:
  ```csharp
  public clsOpeBanda(BdEventosContext cntxt) {
      ctx = cntxt;
  }
  ```
* **v.** Crear el método: `listaBandas()` que retorna una lista con los datos de las bandas que se encuentran registradas en la base de datos:
  ```csharp
  public List<TblBandum> listaBandas() {
      return [.. ctx.TblBanda // Tabla del modelo Entity
          .OrderBy(x => x.Nombre)
          .Where(x => x.Activo == true)]; // Otra forma de hacerlo
  }
  ```

#### b. Crear controlador: `bandaController`
* **i.** Sobre la carpeta `Controllers`, agregar un Controller de tipo: *Controller de MVC: en blanco*, con nombre: `bandaController` (cambiar el texto Home por banda, no borrar la palabra Controller).
* **ii.** Usar las carpetas `Models` y `Clases`:
  ```csharp
  using apiFestRock.Clases;
  using apiFestRock.Models;
  ```
* **iii.** Colocar la ruta y atributos:
  ```csharp
  [Route("api/[controller]")]
  [ApiController]
  ```
* **iv.** Atributo privado:
  ```csharp
  private readonly BdEventosContext ctx;
  ```
* **v.** Constructor:
  ```csharp
  public banda(BdEventosContext context) {
      ctx = context;
  }
  ```
* **vi.** Servicio asincrónico `HttpGet` con alias `listadoBandas`:
  ```csharp
  [HttpGet("listadoBandas")]
  public List<TblBandum> listarBandas() {
      clsOpeBanda oX = new clsOpeBanda(ctx);
      return oX.listaBandas();
  }
  ```
* **c.** Ejecutar el proyecto y probar el servicio con el software: **Swagger**.

---

### 1.3. Acciones de: ArtBand (Llenar combo de Artistas por banda)
*(Página 3)*

#### a. Crear clase operativa: `clsOpeArtBand`
* **i.** En la carpeta `Clases`, crear: `clsOpeArtBand`.
* **ii.** Usar `using apiFestRock.Models;`.
* **iii.** Crear atributo privado: `private readonly BdEventosContext ctx;`.
* **iv.** Crear constructor:
  ```csharp
  public clsOpeArtBand(BdEventosContext cntxt) {
      ctx = cntxt;
  }
  ```
* **v.** Método `listarArtistasXBand`:
  ```csharp
  public IQueryable listarArtistasXBand(int idB) {
      return from tBA in ctx.Set<TblArtBand>()
             join tA in ctx.Set<TblArtistum>() on tBA.IdArtista equals tA.Codigo
             where tBA.IdBanda == idB && tBA.Activo == true
             select new {
                 codigo = tBA.Codigo,
                 nombre = tA.Nombre
             };
  }
  ```
* **vi.** Método `consultaInstrumArtBand`:
  ```csharp
  public IQueryable consultaInstrumArtBand(int idInscrip) {
      return from tBA in ctx.Set<TblArtBand>()
             join tI in ctx.Set<TblInstrum>() on tBA.IdInstrum equals tI.Codigo
             where tBA.Codigo == idInscrip
             select new {
                 nombre = tI.Nombre
             };
  }
  ```

#### b. Crear controlador: `artBandController`
* **i.** Controller MVC en blanco: `artBandController`.
* **ii.** `using apiFestRock.Clases; using apiFestRock.Models;`.
* **iii.** `[Route("api/[controller]")]` y `[ApiController]`.
* **iv.** `private readonly BdEventosContext ctx;`.
* **v.** Constructor `public artBand(BdEventosContext context) { ctx = context; }`.
* **vi.** Servicio `HttpGet` con alias `datosArtBanda`:
  ```csharp
  [HttpGet("datosArtBanda")]
  public IQueryable infoArtBand(int dato, int cmd) {
      clsOpeArtBand oX = new clsOpeArtBand(ctx);
      switch (cmd) {
          case 1: // Llenar combo de los artistas por banda
              return oX.listarArtistasXBand(dato);
          case 2: // Consultar el nombre del instrumento de un artista
              return oX.consultaInstrumArtBand(dato);
          default:
              return Enumerable.Empty<string>().AsQueryable();
      }
  }
  ```
* **c. Casos de prueba en Swagger:**
  * `Dato: 1, cmd: 1` $\rightarrow$ Muestra los datos de los 5 artistas de la banda: Overdrive
  * `Dato: 2, cmd: 1` $\rightarrow$ Muestra Batería, es el instrumento que toca Manuela Herrera en: Anankaia
  * `Dato: 2, cmd: 6` $\rightarrow$ Muestra Vocalista, es el instrumento que toca Manuela Herrera en: Overdrive

---

### 1.4. Sección de Inscritos y Llenar Combos en HTML y JavaScript
*(Páginas 3, 4, 5 y 6)*

#### 1.4.1. En el archivo HTML: `frmInscrip.html`
*Página 4:*
* **i.** Crear archivo javascript: `Scripts_Form/frmInscrip.js`.
* **ii.** En la sección `<head>`:
  ```html
  <!-- Estilos para los Select (combos) -->
  <link href="../Estilos/select2.min.css" rel="stylesheet" />
  <link href="../Estilos/select2-bootstrap4.min.css" rel="stylesheet" />
  <!-- Estilos para las tablas (datatables) -->
  <link href="../Estilos/dataTables.min.css" rel="stylesheet" />
  <link href="../Estilos/dataTables.bootstrap4.min.css" rel="stylesheet" />

  <style>
      .label_1 {
          width: 100%;
          height: 30px;
          border-radius: 5px;
          background-color: #f2eded;
          border-style: solid;
          border-width: 1px;
      }
      .label_2 {
          width: 100%;
          height: 30px;
          border-radius: 20px;
          background-color: #f2eded;
          border-style: solid;
          border-width: 1px;
          text-align: center;
      }
  </style>
  ```
* **iii.** Antes del fin del cuerpo `</body>`:
  ```html
  <!-- Select2 -->
  <script src="../Scripts/select2.min.js"></script>
  <script src="../Scripts_Form/Comunes/Combos.js"></script>
  <!-- Tabla -->
  <script src="../Scripts/jquery.dataTables.min.js"></script>
  <script src="../Scripts/dataTables.bootstrap4.min.js"></script>
  <!-- Librerías comunes de Javascript: Tablas -->
  <script src="../Scripts_Form/Comunes/Tablas.js"></script>
  <!-- Para el script de control del form -->
  <script src="../Scripts_Form/frmCtlInscrip.js"></script>
  ```
* **iv.** Estructura de la tabla de inscritos temporal (`seccionDetalleMae`):
  ```html
  <!-- Tabla para mostrar artistas ingresados -->
  <!-- INICIO Sección artistas ingresados (oculta al inicio) -->
  <div id="seccionDetalleMae" style="display: none;">
      <div class="row mt-3">
          <div class="col-md-12">
              <h5>Artistas Agregados</h5>
              <table class="table table-bordered table-striped">
                  <thead>
                      <tr>
                          <th>Cód. Artista (Art_Banda)</th>
                          <th>Nombre Artista</th>
                          <th>Edad</th>
                      </tr>
                  </thead>
                  <tbody id="tablaDetalleMae">
                      <!-- Aquí se agregan dinámicamente los artistas inscritos desde JS -->
                  </tbody>
              </table>
              <div id="mensajeSinDetalle" class="text-center text-muted mt-2" style="display: none;">
                  No hay artistas agregados aún.
              </div>
          </div>
      </div>
  </div>
  <!-- Fin Sección ingresados -->
  ```

#### 1.4.2. En el archivo JavaScript: `frmInscrip.js`
*(Páginas 4, 5, 6 y 7)*
* **i.** Variable de API:
  ```javascript
  var dir = "http://localhost:XXXXX/api/"; // Reemplazar X por el puerto de ejecución
  ```
* **ii.** Arreglo de detalle en memoria:
  ```javascript
  var detMae = [];
  ```
* **iii.** Variables de control:
  ```javascript
  var idCtlBand = 0;
  var sw = 0;
  ```
* **iv.** Borrar la función asíncrona: `ejecutarComando(acción)` (no se requiere).
* **v.** Al final de jQuery: `$("#cboBanda").trigger("focus");`.
* **vi.** Función local `fechaHoy()`:
  ```javascript
  function fechaHoy() {
      let n = new Date();
      let y = n.getFullYear();
      let m = n.getMonth() + 1; // 12
      let d = n.getDate(); // 31
      if (d < 10) { d = '0' + d; }
      if (m < 10) { m = '0' + m; }
      var newfec = `${d}-${m}-${y}`;
      return newfec;
  }
  ```
* **vii.** En jQuery (tras método load de menú):
  ```javascript
  $("#lblFechaAct").text(fechaHoy());
  ```
* **viii. Funciones asincrónicas de consulta:**
  ```javascript
  async function vrInscEvento() {
      try {
          let idEve = parseInt($("#cboEvento").val());
          if (parseInt(idEve, 10) <= 0) {
              alerta("No selección de evento", "Error");
              $("#cboEvento").trigger("focus");
              return;
          }
          let url = dir + "evento/valorEvento?idEv=" + idEve;
          const datosOut = await fetch(url, {
              method: "GET",
              mode: "cors",
              headers: { "content-type": "application/json" }
          });
          const datosIn = await datosOut.json();
          if (datosIn == "") {
              alerta("No se encontró valor para el evento ", "Error de ejecución");
              return;
          }
          $("#lblVrIndInsc").text(datosIn[0].vrIndInsc);
      } catch (error) {
          aleta(error, "Error: ");
      }
  }

  async function consultarInstrum() {
      try {
          let idBA = $("#cboArtBand").val();
          if (idBA.trim() == "" || parseInt(idBA, 10) <= 0) {
              alerta("No selección de artista", "Atención");
              $("#cboArtBand").trigger("focus");
              return;
          }
          let url = dir + "artBand/datosArtBanda?dato=" + idBA + "&cmd=2";
          const datosOut = await fetch(url, {
              method: "GET",
              mode: "cors",
              headers: { "content-type": "application/json" }
          });
          const datosIn = await datosOut.json();
          if (datosIn == "") {
              alerta("No se encontró instrumento para el artista.", "Advertencia");
              return;
          }
          $("#txtInstrum").val(datosIn[0].nombre);
      } catch (error) {
          alerta(error, "Error: ");
      }
  }
  ```
* **ix. Función sincrónica de cambio de etiqueta del botón agregar:**
  *(Página 6)*
  ```javascript
  function cambiarLblBtnAgregar(accion) {
      const btn = document.getElementById('btnAgre');
      if (accion == 1) {
          btn.innerHTML = 'Agregar';
      } else if (accion == 2) {
          btn.innerHTML = 'Agregar otro artista';
      } else if (accion == 3) {
          btn.innerHTML = 'Grabar Inscripción';
      }
  }
  ```
* **x. Funciones asincrónicas para llenado de combos:**
  ```javascript
  async function llenarComboEventos() {
      let url = dir + "evento/listadoEventos";
      let rpta = await llenarComboGral(url, "#cboEvento");
      if (rpta == "Termino") {
          await vrInscEvento();
      }
  }

  async function llenarComboBandas() {
      $("#cboBandArt").empty();
      let url = dir + "banda/listadoBandas";
      let rpta = await llenarComboGral(url, "#cboBanda");
      if (rpta == "Termino") {
          await llenarComboArtista();
      }
  }

  async function llenarComboArtista() {
      let idB = $("#cboBanda").val();
      let url = dir + "artBand/datosArtBanda?dato=" + idB + "&cmd=1";
      let rpta = await llenarComboGral(url, "#cboArtBand");
      if (rpta == "Termino") {
          await consultarInstrum();
      }
      idCtlBand = $("#cboBanda").val();
      sw = 0;
  }
  ```
* **xi a xiv. Eventos `onchange` en combos:**
  ```javascript
  // xi. En jQuery tras listener de botones:
  llenarComboEventos();
  llenarComboBandas();

  // xii. En combo evento:
  // onchange="vrInscEvento();"

  // xiii. En combo banda:
  // onchange="llenarComboArtista();"

  // xiv. En combo de artistas:
  // onchange="consultarInstrum();"
  ```
* **xv. Pruebas de funcionamiento:** Verificar carga de combos y comportamientos.
* **xvi. Función sincrónica `limpiar()`:**
  ```javascript
  function limpiar() {
      $("#lblFechaInsc").text("");
      $("#lblFechaAct").text(fechaHoy());
      $("#txtRepres").val("");
      $("#txtTelRep").val("");
      $("#txtValorTot").val("");
      $("#txtInstrum").val("");
      $("#txtEdad").val("");
      alerta("Ya no hay más artistas para preinscribir", "Advertencia");
      cambiarLblBtnAgregar(3);
      controlCmboIncritos();
  }
  ```
* **x (Página 12). Control de cambio de banda durante edición/pre-inscripción:**
  ```javascript
  if (sw == 0) {
      sw = 1;
      idCtlBand = $("#cboBanda").val();
  } else {
      let ktblDM = document.getElementById("tablaDetalleMae").rows.length;
      let ktbIDa = $("#tablaDatos tr").length;
      if (idB != idCtlBand) {
          if (ktblDM > 0 || ktbIDa > 2) {
              alerta("No permitido cambiar Banda mientras en edición o Consulta", "Alerta");
              $("#cboBanda").val(idCtlBand);
              llenarComboArtista();
          }
      }
      if (ktblDM > 0) {
          quitarArtistaCmbPreInscCombo();
      } else if (ktbIDa > 2) {
          retirarDatosCmboArtBand();
      }
  }
  ```
* **xi (Página 12). Listener del botón `btnAgre`:**
  ```javascript
  const txtBtnAgre = document.getElementById('btnAgre');
  let txt = txtBtnAgre.innerHTML;
  let nroInsc = $("#txtCodigo").val();
  if (txt == "Grabar Inscripción" || parseInt(nroInsc, 10) > 0) {
      alerta("En construcción", "Alerta");
  } else { // Agregar asignaturas/inscritas
      agregarDetalle_PreInsc(); // Agregando artista a la Pre-inscripción como detalle
  }
  ```
* **xvii (Página 7). Función sincrónica `Cancelar()`:**
  ```javascript
  function Cancelar() {
      limpiar();
      $("#txtCodigo").val("0");
      llenarComboBandas();
      $("#cboBanda").trigger("focus");
      sw = 0;
      cambiarLblBtnAgregar(1);
      $("#cboBanda").trigger("focus");
      $("#txtCodigo").prop("disabled", true);
      $("#seccionDetalleMae").hide();
      detMae = []; // Limpia el array de registros realizados
      // Limpia los registros del DataTable del formulario
      const tbody = document.querySelector('#tablaDatos tbody');
      tbody.innerHTML = '';
      $('#tablaDetalleMae tr:not(:has(th))').remove();
      oTabla.clear().draw();
  }
  ```
* **xviii. Asignar al botón `Cancelar` la función `Cancelar()`:**
  ```javascript
  Cancelar();
  ```

---

## 1.5. CONSULTAR UNA INSCRIPCIÓN

### 1.5.1. Clase operativa: `clsOpeInscrip` (Página 7)
* **i.** En la carpeta: `Clases`, crear `clsOpeInscrip`.
* **ii.** `using apiFestRock.Models;`.
* **iii.** `private readonly BdEventosContext ctx;`.
* **iv.** Constructor:
  ```csharp
  public clsOpeInscrip(BdEventosContext cntxt) {
      ctx = cntxt;
  }
  ```
* **iv (Bis). Propiedad `tblInsc`:**
  ```csharp
  public TbIInsc tblInsc { get; set; } // Misma del Model
  ```
* **v. Método `consultarInscrip()`:**
  ```csharp
  public IQueryable consultarInscrip(int idCod) // Encabezado
  {
      return from x in ctx.TbIInscrips
             join e in ctx.TblEventos on x.IdEvento equals e.Codigo
             where x.Codigo == idCod
             select new {
                 nro = x.Codigo,
                 idEvento = x.IdEvento,
                 vrInd = e.VrInscEvent,
                 fec = x.Fecha,
                 repre = x.Represent,
                 telef = x.TelRepresent,
                 idBand = x.IdBanda,
                 vrTot = x.ValorInscrip
             };
  }
  ```

### 1.5.2. Controlador API: `inscripcController` (Páginas 7 y 8)
* MVC en blanco: `inscripcController`.
* `using apiFestRock.Clases; using apiFestRock.Models;`.
* `[Route("api/[controller]")]` y `[ApiController]`.
* Atributo: `private readonly BdEventosContext ctx;`.
* Constructor: `public inscripcController(BdEventosContext context) { ctx = context; }`.
* **vi.** Servicio `HttpGet` con alias `buscarInscrip`:
  ```csharp
  [HttpGet("buscarInscrip")]
  public IQueryable Get(int nroInsc) {
      clsOpeInscrip oX = new clsOpeInscrip(ctx);
      return oX.consultarInscrip(nroInsc);
  }
  ```

### 1.5.3. Clase operativa: `clsOpeDetInscrip` (Página 8)
* **i.** Crear `clsOpeDetInscrip`.
* **ii.** `using apiFestRock.Models;`.
* **iii.** `private readonly BdEventosContext ctx;`.
* **iv.** Constructor: `public clsOpeDetInscrip(BdEventosContext cntxt) { ctx = cntxt; }`.
* **v.** Propiedad: `public TblDetInsc tbIDetInsc { get; set; }`.
* **vi. Método `buscarDetXNroInsc`:**
  ```csharp
  public IQueryable buscarDetXNroInsc(int nroInsc) {
      return from det in ctx.TblDetInscs
             join baAr in ctx.TblArtBands on det.IdArtBand equals baAr.Codigo
             join art in ctx.TblArtista on baAr.IdArtista equals art.Codigo
             join ins in ctx.TblInstrums on baAr.IdInstrum equals ins.Codigo
             join ban in ctx.TblBanda on baAr.IdBanda equals ban.Codigo
             where det.IdNroInsc == nroInsc
             orderby det.Codigo
             select new {
                 nroDetInsc = det.Codigo,
                 banda = ban.Nombre,
                 idArtBand = baAr.Codigo,
                 artista = art.Nombre,
                 edad = det.Edad,
                 instrumento = ins.Nombre,
                 activo = baAr.Activo,
                 Eliminar = "<button class='btn btn-info btn-sm btnEliminar'><i class='fas fa-trash-alt'></i> Quitar </button>"
             };
  }
  ```

### 1.5.4. Controlador API: `detInscripcController` (Página 8)
* MVC en blanco: `detInscripcController`.
* `using apiFestRock.Clases; using apiFestRock.Models;`.
* `[Route("api/[controller]")]` y `[ApiController]`.
* Atributo: `private readonly BdEventosContext ctx;`.
* Constructor: `public detInscripcController(BdEventosContext context) { ctx = context; }`.
* **vi.** Servicio `HttpGet` con alias `consultaDatosDetInscrip`:
  ```csharp
  [HttpGet("consultaDatosDetInscrip")]
  public IQueryable Get(int nroInsc) {
      clsOpeDetInscrip oX = new clsOpeDetInscrip(ctx);
      return oX.buscarDetXNroInsc(nroInsc);
  }
  ```
* **vii.** Probar en Swagger.

---

### 1.5.5. En el archivo JavaScript para el Proceso de Consulta
*(Página 9 y 10)*

```javascript
// i. Inicializar DataTable
var oTabla = $("#tablaDatos").DataTable();

// ii. Llenar tabla detalle
async function llenarTabla() {
    let codInsc = parseInt($("#txtCodigo").val()) || 0;
    if (codInsc <= 0) {
        alerta("Número de inscripción no válido", "Error");
        $("#txtNroDoc").trigger("focus");
        return;
    }

    let url = dir + "detInscripc/consultaDatosDetInscrip?nroInsc=" + codInsc;
    let rpta = await llenarTablaGral(url, "#tablaDatos");
    if (rpta != "Terminó") {
        alerta("Búsqueda: " + codInsc + ", sin artistas inscritos o no encontrada la inscripción", "Alerta/Información");
    }
}

// iii. Retirar datos del combo de artistas por banda ya registrados
function retirarDatosCmboArtBand() {
    let kRgtros = $("#tablaDatos tr").length;
    if (kRgtros > 0) {
        for (let i = 1; i <= kRgtros; i++) {
            let codAB = $("#tablaDatos").find("tr").eq(i).find("td").eq(2).html();
            $("#cboArtBand option[value='" + codAB + "']").remove();
        }
    }
}

// iv. Consultar datos de cabecera
async function Consultar() {
    try {
        limpiar();
        $("#txtCodigo").prop("disabled", false);
        let cod = parseInt($("#txtCodigo").val()) || 0;
        if (cod <= 0) {
            $("#txtCodigo").val("");
            $("#txtCodigo").trigger("focus");
            alerta("Ingresar el código de la inscripción", "Continuar");
            return;
        }

        $("#txtCodigo").prop("disabled", true);
        var url = dir + "inscripc/buscarInscrip?nroInsc=" + cod;
        const datosOut = await fetch(url, {
            method: "GET",
            mode: "cors",
            headers: { "content-type": "application/json" }
        });

        const Rpta = await datosOut.json();
        if (Rpta == "") {
            alerta("No se encontró inscripción con nro: " + cod, "Información");
            $("#txtCodigo").val("0");
            return;
        }

        $("#tablaDatos tr").empty();
        await llenarTabla();
        $("#cboEvento").val(Rpta[0].idEvento);
        $("#lblFechaInsc").text(Rpta[0].fec);
        $("#txtRepres").val(Rpta[0].repre);
        $("#txtTelRep").val(Rpta[0].telef);
        $("#txtValorTot").val(Rpta[0].vrTot);
        $("#cboBanda").val(Rpta[0].idBand);
        idCtlBand = Rpta[0].idBand;
    } catch (error) {
        alerta(error, "Error: ");
    }
}

// v. Invocar en el botón Consultar
// btnBusc invoca Consultar()
```

---

## 1.6. PRE-INSCRIPCIÓN: EN HTML Y JAVASCRIPT
*(Páginas 10 y 11)*

```javascript
// i. Control de combo artistas
function controlCmboIncritos() {
    let kArtBand = $("#cboArtBand option").length;
    if (kArtBand <= 0) {
        alerta("Sin Artistas para inscribir", "Alerta");
        cambiarLblBtnAgregar(3);
    }
}

// ii. Mostrar inscritos en la tabla HTML (seccionDetalleMae)
function mostrarInscritos() {
    let detalles = $("#tablaDetalleMae");
    detalles.empty();
    detMae.forEach((rgtro, index) => {
        detalles.append(`
            <tr>
                <td>${rgtro.idArtBand}</td>
                <td>${rgtro.nombre}</td>
                <td>${rgtro.edad}</td>
                <td><button class="btn btn-danger btn-sm" onclick="eliminarRgtroPreInscrip(${index})">Eliminar</button></td>
            </tr>
        `);
    });

    $("#seccionDetalleMae").slideDown();
    if (detMae.length === 0) {
        $("#mensajeSinDetalle").show();
    } else {
        $("#mensajeSinDetalle").hide();
    }
}

// v. Recalcular valor total de la inscripción
function vrTotalInscrip() {
    let vrIndInsc = parseInt($("#lblVrIndInsc").text()) || 0;
    let vrTot = document.getElementById("tablaDetalleMae").rows.length * vrIndInsc;
    $("#txtValorTot").val(vrTot);
}

// vi. Recuperar datos del artista al combo tras eliminar de pre-inscripción
function recuperarArtCmbo(codArtAB, name) {
    let cmbAB = document.getElementById('cboArtBand');
    var opt = document.createElement('option');
    opt.value = codArtAB;
    opt.innerHTML = name;
    cmbAB.appendChild(opt);
    consultarInstrum();
}

// vii. Eliminar registro del arreglo en memoria detMae
function eliminarRgtroPreInscrip(index) {
    var codArtAB = detMae[index].idArtBand;
    var name = detMae[index].nombre;
    let txt = "¿Seguro desea eliminar la inscripción de: " + name + "?";
    
    let rpta = window.confirm(txt);
    if (!rpta) {
        alerta('Cancelada acción de eliminación', 'Atención');
        return;
    } else {
        detMae.splice(index, 1);
        recuperarArtCmbo(codArtAB, name);
        cambiarLblBtnAgregar(1);
        alerta("Registro de: " + name + " ha sido borrado", "Información");
        mostrarInscritos();
        vrTotalInscrip();
    }
}

// viii. Quitar del combo artistas ya existentes en tablaDetalleMae
function quitarArtistaCmbPreInscCombo() {
    let kRgtros = $("#tablaDetalleMae tr").length;
    if (kRgtros > 0) {
        for (let i = 0; i < kRgtros; i++) {
            let codAB = $("#tablaDetalleMae").find('tr').eq(i).find('td').eq(0).html();
            $("#cboArtBand option[value='" + codAB + "']").remove();
        }
    }
    controlCmboIncritos();
}

// ix. Adicionar artista al arreglo local detMae
function agregarDetalle_PreInsc() {
    try {
        let idCodAB = $("#cboArtBand").val();
        let nombreArt = $("#cboArtBand option:selected").text();
        let edadA = parseInt($("#txtEdad").val()) || 0;

        if (!idCodAB) {
            alerta("Debe seleccionar un artista.", "Alerta");
            $("#cboArtBand").trigger("focus");
            return;
        }

        if (isNaN(edadA) || edadA <= 0) {
            alerta("Edad no válida, debe ser mayor a 0.", "Alerta");
            $("#txtEdad").trigger("focus");
            return;
        }

        detMae.push({
            idArtBand: idCodAB,
            nombre: nombreArt,
            edad: edadA
        });

        mostrarInscritos();
        vrTotalInscrip();
        quitarArtistaCmbPreInscCombo();
        alerta(nombreArt + ", agregado con éxito", "Procedimiento con éxito");
        idCtlBand = $("#cboBanda").val();

        let cantCmbo = document.getElementById('cboArtBand').length;
        if (cantCmbo >= 1) {
            let agregarMas = confirm("¿Desea agregar otro artista a la preinscripción? si no, presionar: Cancelar");
            if (agregarMas) {
                $("#txtEdad").val("");
                $("#cboArtBand").trigger("focus");
                cambiarLblBtnAgregar(2);
            } else {
                cambiarLblBtnAgregar(3);
            }
        }
    } catch {
        // Manejo de error
    }
}
```

---

## 1.7. GRABAR INSCRIPCIÓN EN LA BASE DE DATOS (MAESTRO-DETALLE)

### 1.7.1. Clases Auxiliares (DTOs)
*(Página 12)*
* **i.** En la carpeta: `Clases`, crear subcarpeta: `Auxiliar`. Agregar la clase `clsInscripCompleta` que contendrá 2 clases: `detalleInsc` e `inscripCompleta`.
* **ii. Clase `detalleInsc`:**
  ```csharp
  public class detalleInsc
  {
      public int IdArtBand { get; set; }
      public int Edad { get; set; }
  }
  ```
* **iii. Clase `inscripCompleta`:**
  ```csharp
  public class inscripCompleta
  {
      public int IdEvento { get; set; }
      public required string Fecha { get; set; }
      public int IdBanda { get; set; }
      public required string Represent { get; set; }
      public required string TelRepresent { get; set; }
      public decimal ValorInscrip { get; set; }
      public required List<detalleInsc> Inscritos { get; set; }
  }
  ```

### 1.7.2. En la clase operativa: `clsOpeDetInscrip` (Página 12)
* **i.** Agregar using:
  ```csharp
  using static apiFestRock.Clases.Auxiliar.clsInscripCompleta;
  ```

### 1.7.3. En la clase operativa: `clsOpeInscrip` (Página 13)
* **i.** Agregar using:
  ```csharp
  using static apiFestRock.Clases.Auxiliar.clsInscripCompleta;
  ```
* **ii. Método `registrarDetalles`:**
  ```csharp
  public bool registrarDetalles(int idNroInsc, List<detalleInsc> inscritos)
  {
      try
      {
          foreach (var insc in inscritos)
          {
              TblDetInsc tblDetInsc = new TblDetInsc();
              tblDetInsc.IdNroInsc = idNroInsc;
              tblDetInsc.IdArtBand = insc.IdArtBand;
              tblDetInsc.Edad = insc.Edad;
              ctx.TblDetInscs.Add(tblDetInsc);
          }
          ctx.SaveChanges();
          return true;
      }
      catch
      {
          return false;
      }
  }
  ```
* **iii. Método `grabarDetalleInd`:**
  ```csharp
  public int grabarDetalleInd(int idNroInsc, int idArtBanda, int edad)
  {
      try
      {
          TblDetInsc tblDetInsc = new TblDetInsc();
          tblDetInsc.IdNroInsc = idNroInsc;
          tblDetInsc.IdArtBand = idArtBanda;
          tblDetInsc.Edad = edad;

          ctx.TblDetInscs.Add(tblDetInsc);
          ctx.SaveChanges();
          return 1;
      }
      catch
      {
          return 0;
      }
  }
  ```
* **ii (Bis). Método `registrarEncabezado`:**
  ```csharp
  public int registrarEncabezado(inscripCompleta datosEncab)
  {
      var ultimo = ctx.TbIInscrips.Max(x => (int?)x.Codigo) ?? 0;
      int nuevoID = (ultimo <= 0) ? 1 : ultimo + 1;

      TbIInsc tblInsc = new TbIInsc();
      tblInsc.Codigo = nuevoID;
      tblInsc.IdEvento = datosEncab.IdEvento;
      tblInsc.Fecha = DateTime.Now.ToShortDateString();
      tblInsc.IdBanda = datosEncab.IdBanda;
      tblInsc.Represent = datosEncab.Represent;
      tblInsc.TelRepresent = datosEncab.TelRepresent;
      tblInsc.ValorInscrip = datosEncab.ValorInscrip;

      ctx.TbIInscrips.Add(tblInsc);
      ctx.SaveChanges();
      return nuevoID;
  }
  ```
* **iii (Bis). Método `eliminarEncabezado` (Rollback manual):**
  ```csharp
  public void eliminarEncabezado(int idCod)
  {
      try
      {
          var rgtro = ctx.TbIInscrips.FirstOrDefault(d => d.Codigo == idCod);
          if (rgtro != null)
          {
              ctx.TbIInscrips.Remove(rgtro);
              ctx.SaveChanges();
          }
      }
      catch
      {
          return;
      }
  }
  ```

### 1.7.4. En el controlador: `inscripc` (Página 14)
* **i.** `using static apiFestRock.Clases.Auxiliar.clsInscripCompleta;`.
* **ii. Endpoint `agregarInscrip`:**
  ```csharp
  [HttpPost("agregarInscrip")]
  public int Post([FromBody] inscripCompleta incripcWeb)
  {
      try
      {
          // 1. Guardar el encabezado
          clsOpeInscrip opeI = new clsOpeInscrip(ctx);
          int idNroInsc = opeI.registrarEncabezado(incripcWeb);

          // 2. Guardar el detalle de artistas
          clsOpeDetInscrip opeDI = new clsOpeDetInscrip(ctx);
          if (opeDI.registrarDetalles(idNroInsc, incripcWeb.Inscritos))
          {
              return idNroInsc; // Procedimiento con éxito
          }
          else
          {
              opeI.eliminarEncabezado(idNroInsc); // Deshacer encabezado
              return 0;
          }
      }
      catch
      {
          return -1; // Error general
      }
  }
  ```
* **Estructura JSON de prueba para Swagger:**
  ```json
  {
    "idEvento": 1,
    "fecha": "13/11/2026",
    "idBanda": 2,
    "represent": "Camilo Atehortúa",
    "telRepresent": "3101002030",
    "valorInscrip": 400000,
    "inscritos": [
      {
        "idArtBand": 1,
        "edad": 21
      },
      {
        "idArtBand": 3,
        "edad": 22
      }
    ]
  }
  ```

### 1.7.5. En el controlador: `detInscripc` (Página 14)
* **i. Endpoint `agregarDetalleInd`:**
  ```csharp
  [HttpPost("agregarDetalleInd")]
  public int adicionaDetalleInd(int nroInscrip, int idArtBand, int edad)
  {
      clsOpeDetInscrip oX = new clsOpeDetInscrip(ctx);
      return oX.grabarDetalleInd(nroInscrip, idArtBand, edad);
  }
  ```

### 1.7.6. En el archivo JavaScript del formulario para Agregar Inscripción
*(Páginas 15 a 17)*

```javascript
// i. Validar datos generales
function validar(nombre, tel, vrTi) {
    var msj = "";
    if (nombre == undefined || nombre.trim() == "") {
        msj += "Falta el nombre del representante. \n";
    }
    if (tel == undefined || tel.trim() == "") {
        msj += "Falta el teléfono del representante. \n";
    }
    if (isNaN(vrTi) || vrTi <= 0) {
        msj += "Valor total inscripción debe ser mayor a 0. \n";
    }
    if (msj != "") {
        alerta(msj, "Error en procedimiento");
        $("#txtRepres").trigger("focus");
        return false;
    }
    return true;
}

// ii. Control de combo artistas
function controlCmboArtistas() {
    $("#txtInstrum").val("");
    $("#txtEdad").val("");
    let kArtBand = $("#cboArtBand option").length;
    if (kArtBand <= 0) {
        alerta("Sin Artistas para inscribir", "Alerta");
    } else {
        consultarInstrum();
    }
}

// iii. Función asincrónica grabarInscripc()
async function grabarInscripc() {
    try {
        let nroIn = $("#txtCodigo").val();
        let idEve = $("#cboEvento").val();
        let idBan = $("#cboBanda").val();
        let repre = $("#txtRepres").val();
        let telRe = $("#txtTelRep").val();
        let vrTot = parseFloat($("#txtValorTot").val()) || 0;

        if (!validar(repre, telRe, vrTot)) return;

        // CASO 1: Nuevo registro en BD (nroIn == 0)
        if (nroIn == 0) {
            let fechInsc = new Date().toISOString().split('T')[0];

            if (detMae.length === 0) {
                alerta("Debe inscribir al menos un artista.", "Advertencia");
                $("#cboArtBand option").trigger("focus");
                return;
            }

            let datosParaInscripc = {
                idEvento: idEve,
                fecha: fechInsc,
                idBanda: idBan,
                represent: repre,
                telRepresent: telRe,
                valorInscrip: vrTot,
                inscritos: detMae
            };

            let url = dir + "inscripc/agregarInscrip";
            const envio = await fetch(url, {
                method: "POST",
                mode: "cors",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(datosParaInscripc)
            });

            const rpta = await envio.text();
            let nroInsc = rpta.toString();

            if (nroInsc <= 0) {
                alerta("Se produjo error al grabar inscripción, revisar", "Error en grabar");
                return;
            }

            $("#txtCodigo").val(nroInsc);
            $("#seccionDetalleMae").hide();
            let inscritos = $("#tablaDetalleMae");
            inscritos.empty();
            cambiarLblBtnAgregar(1);
            await Consultar();
            alerta("Se generó la inscripción #" + nroInsc, "Proceso con éxito");
            return;
        } 
        // CASO 2: Adición a inscripción existente en BD (nroIn > 0)
        else if (parseInt(nroIn, 10) > 0) {
            let idCodAB = $("#cboArtBand").val();
            let edadArt = $("#txtEdad").val();

            if (!idCodAB) {
                alerta("Debe seleccionar un artista.", "Alerta");
                $("#cboArtBand").trigger("focus");
                return;
            }

            if (isNaN(edadArt) || edadArt <= 0) {
                alerta("Edad no válida, debe ser mayor a 0.", "Alerta");
                $("#txtEdad").trigger("focus");
                return;
            }

            let nombArt = $("#cboArtBand option:selected").text();
            let txt = "¿Seguro desea agregar el artista: " + nombArt + " a la inscripción?";

            let continua = window.confirm(txt);
            if (!continua) {
                alerta('Cancelada acción de agregar', 'Información');
                return;
            }

            let url = dir + "detInscripc/agregarDetalleInd?nroInscrip=" + nroIn + "&idArtBand=" + idCodAB + "&edad=" + edadArt;
            const datosEnvio = await fetch(url, {
                method: "POST",
                mode: "cors",
                headers: { "content-type": "application/json" }
            });

            const rpta = await datosEnvio.json();
            if (rpta == 1) {
                await llenarTabla();
                await llenarComboArtista();
                await retirarDatosCmboArtBand();
                alerta("Se grabó el artista: " + nombArt + " en la inscripción: " + nroIn, "Información");
                controlCmboArtistas();
            } else {
                alerta("Error en agregar artista, reintente por favor", "Alerta");
            }
        }
    } catch (error) {
        alerta("Error al grabar inscripción: " + error, "Alerta");
    }
}

// vi. Listener en botón btnAgre
// Invocación a grabarInscripc();
```

---

## 1.8. ELIMINACIÓN DEL REGISTRO DE UNA INSCRIPCIÓN (DETALLE)
*(Páginas 17 y 18)*

### 1.8.1. En la clase operativa: `clsOpeDetInscrip`
```csharp
public string Eliminar(int idDetInsc)
{
    try
    {
        var det = ctx.TblDetInscs.FirstOrDefault(d => d.Codigo == idDetInsc);
        if (det == null)
            return "Inscripción no encontrada.";

        ctx.TblDetInscs.Remove(det);
        ctx.SaveChanges();
        return "Registro de Inscripción eliminado correctamente.";
    }
    catch (Exception ex)
    {
        return "Error al eliminar registro: " + ex.Message;
    }
}
```

### 1.8.2. En el controlador: `detInscripc`
```csharp
[HttpDelete("borrarDetalle")]
public string borradoDetalle(int idDet)
{
    clsOpeDetInscrip oX = new clsOpeDetInscrip(ctx);
    return oX.Eliminar(idDet);
}
```

### 1.8.3. En el archivo JavaScript del formulario
```javascript
// i. Función asincrónica para borrar detalle en BD
async function eliminarDetInsc(idDetInsc) {
    try {
        let url = dir + "detInscripc/borrarDetalle?idDet=" + idDetInsc;
        let respuesta = await fetch(url, {
            method: "DELETE",
            headers: { "Content-Type": "application/json" }
        });

        let rpta = await respuesta.text();

        if (rpta.substring(0, 5) === "Error") {
            alerta(rpta, "Error en procedimiento");
        } else {
            alerta(rpta, "Atención");
            $("#tablaDatos tr").empty();
            await llenarTabla();
            await llenarComboArtista();
            await retirarDatosCmboArtBand();
            controlCmboArtistas();
        }
    } catch (err) {
        alerta(err, "No se pudo eliminar el registro.");
    }
}

// ii. Listener delegado para la tabla de datos
$(document).on("click", "#tablaDatos .btnEliminar", function (evento) {
    evento.preventDefault();
    evento.stopPropagation();

    let fila = $(this).closest("tr");
    let idDetalleInsc = fila.find("td:eq(0)").text();
    let nombre = fila.find("td:eq(3)").text();
    let codAB = fila.find("td:eq(2)").text();

    let txt = "¿Seguro desea eliminar la inscripción de: " + nombre + "?";
    if (confirm(txt)) {
        eliminarDetInsc(parseInt(idDetalleInsc));
    }
});
```

---

## 1.9. MODIFICAR DATOS DEL ENCABEZADO DE LA INSCRIPCIÓN EN LA BD
*(Páginas 18 y 19)*

### 1.9.1. En la clase operativa: `clsOpeInscrip`
```csharp
public string Modificar()
{
    try
    {
        if (tblInsc.Codigo <= 0)
        {
            return "Inscripción: " + tblInsc.Codigo + ", no es válido.";
        }

        var p = ctx.TbIInscrips.FirstOrDefault(x => x.Codigo == tblInsc.Codigo);

        if (p == null)
        {
            return "Inscripción " + tblInsc.Codigo + ", no existe.";
        }

        p.IdEvento = tblInsc.IdEvento;
        p.Represent = tblInsc.Represent;
        p.TelRepresent = tblInsc.TelRepresent;

        ctx.SaveChanges();
        return "Inscripción: " + tblInsc.Codigo + ", actualizada correctamente.";
    }
    catch
    {
        return "Error al actualizar inscripción.";
    }
}
```

### 1.9.2. En el controlador: `detInscripc` / `inscripc`
```csharp
[HttpPut("modificaEncabinscrip")]
public string Put([FromBody] TbIInsc objIn)
{
    clsOpeInscrip oX = new clsOpeInscrip(ctx);
    oX.tblInsc = objIn;
    return oX.Modificar();
}
```

### 1.9.3. En el archivo JavaScript del formulario
```javascript
// i. Función asincrónica Modificar()
async function Modificar() {
    let nroInsc = $("#txtCodigo").val();
    if (isNaN(nroInsc) || nroInsc.trim() === "" || parseInt(nroInsc, 10) <= 0) {
        alerta("Solo se modifica un registro existente.", "Atención");
        $("#txtCodigo").trigger("focus");
        return;
    }

    let idEven = $("#cboEvento").val();
    let repres = $("#txtRepres").val();
    let telRep = $("#txtTelRep").val();

    if (repres.trim() === "" || telRep.trim() === "") {
        alerta("Faltan datos válidos para modificar", "Advertencia");
        $("#txtRepres").trigger("focus");
        return;
    }

    let rpta = window.confirm('¿Seguro de modificar la inscripción: ' + nroInsc + '?');
    if (rpta == false) {
        alerta('Cancelada acción de modificación', 'Información');
        return;
    } else {
        let datosParaModif = {
            codigo: parseInt(nroInsc, 10),
            idEvento: parseInt(idEven, 10),
            represent: repres,
            telRepresent: telRep
        };

        try {
            let url = dir + "inscripc/modificaEncabinscrip";
            const rpta = await fetch(url, {
                method: "PUT",
                mode: "cors",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(datosParaModif),
            });

            const txtRpta = await rpta.text();
            alerta(txtRpta, "Atención");
        } catch (error) {
            alerta("Error: " + error, "Error de procedimiento");
        }
    }
}

// ii. Listener para el botón Modificar
// btnModi invoca el método Modificar();
```