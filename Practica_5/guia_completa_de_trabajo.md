# INSTITUCIÓN UNIVERSITARIA ITM

**GUÍA DE TRABAJO - TECNOLOGÍA EN DESARROLLO DE SOFTWARE**

* **Código:** FDE 048

* **Versión:** 03

* **Fecha:** 2009-06-09

* **Asignatura:** Programación de Software

* **Docente / Contacto:** `delloaristizabal@itm.edu.co`

* **Contenido documentado:** Páginas 9 a 19 (Secciones 1.5 a 1.9)

## ÍNDICE DE CONTENIDO

1. [1.5. Proceso de Consulta](#15-proceso-de-consulta-página-9-a-10)

2. [1.6. Pre-Inscripción: En HTML y JavaScript](#16-pre-inscripción-en-html-y-javascript-página-10-a-11)

3. [1.7. Grabado de la Inscripción (Maestro-Detalle)](#17-grabado-de-la-inscripción-maestro-detalle-páginas-13-a-17)

   * 1.7.3. Capa Operativa / Datos C# (`clsOpeInscrip`)

   * 1.7.4. Controlador API `inscripc`

   * 1.7.5. Controlador API `detInscripc`

   * 1.7.6. Funciones JavaScript de Formulario y Grabación

4. [1.8. Eliminación del Registro de una Inscripción (Detalle)](#18-eliminación-del-registro-de-una-inscripción-páginas-17-a-18)

5. [1.9. Modificar Datos del Encabezado de la Inscripción](#19-modificar-datos-del-encabezado-de-la-inscripción-páginas-18-a-19)

## 1.5. Proceso de Consulta (Página 9 a 10)

### 1.5.5. En el archivo JavaScript del formulario para el proceso de consulta:

#### i. Inicialización de DataTable

Al inicio del archivo, antes del método `jQuery`, en la línea siguiente a la declaración de la variable `dir`, declarar la variable `oTabla` para establecer la tabla HTML `tablaDatos` de tipo DataTable:

```
var oTabla = $("#tablaDatos").DataTable();

```

#### ii. Función asincrónica `llenarTabla()`

Para realizar el llenado de la tabla `tablaDatos`, con los artistas registrados en una inscripción. Esta tabla mostrará el detalle de la inscripción:

```
async function llenarTabla() {
    let codInsc = parseInt($("#txtCodigo").val()) || 0; // Si no hay cantidad, toma 0
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

```

#### iii. Función sincrónica `retirarDatosCmboArtBand()`

Para retirar del combo de Artistas por banda (`cboArtBand`), los ya inscritos en una inscripción:

```
function retirarDatosCmboArtBand() {
    let kRgtros = $("#tablaDatos tr").length;
    if (kRgtros > 0) {
        for (let i = 1; i <= kRgtros; i++) {
            // Cualquiera de las 2 instrucciones siguientes es válida:
            // let codAB = $("#tablaDatos").find('tr').eq(i).find('td').eq(2).html();
            let codAB = $("#tablaDatos").find("tr").eq(i).find("td").eq(2).html();
            $("#cboArtBand option[value='" + codAB + "']").remove();
        }
    }
}

```

#### iv. Función asincrónica `Consultar()`

Para mostrar los datos del encabezado de una inscripción:

```
async function Consultar() {
    try {
        limpiar();
        $("#txtCodigo").prop("disabled", false);
        let cod = parseInt($("#txtCodigo").val()) || 0; // Si no hay cantidad, toma 0
        if (cod <= 0) {
            $("#txtCodigo").val("");
            $("#txtCodigo").trigger("focus");
            alerta("Ingresar el código de la inscripción", "Continuar");
            return;
        }

        $("#txtCodigo").prop("disabled", true);
        // Invocar el servicio
        var url = dir + "inscripc/buscarInscrip?nroInsc=" + cod;
        const datosOut = await fetch(url, {
            method: "GET",
            mode: "cors",
            headers: {
                "content-type": "application/json"
            }
        });

        const Rpta = await datosOut.json();
        if (Rpta == "") {
            alerta("No se encontró inscripción con nro: " + cod, "Información");
            $("#txtCodigo").val("0");
            return;
        }

        $("#tablaDatos tr").empty();
        await llenarTabla(); // Acá realizará la invocación para el llenado de la tabla con los artistas registrados
        $("#cboEvento").val(Rpta[0].idEvento);
        $("#lblFechaInsc").text(Rpta[0].fec);
        $("#txtRepres").val(Rpta[0].repre);
        $("#txtTelRep").val(Rpta[0].telef);
        $("#txtValorTot").val(Rpta[0].vrTot);
        $("#cboBanda").val(Rpta[0].idBand);
        idCtlBand = Rpta[0].idBand; // Para control de cambio en combo: cboArtBand
    } catch (error) {
        alerta(error, "Error: ");
    }
}

```

#### v. Asignación de Listener

Agregar al listener del botón `btnBusc` la invocación del método `Consultar()`:

```
Consultar();

```

## 1.6. Pre-Inscripción: En HTML y JavaScript (Página 10 a 11)

#### i. Función sincrónica `controlCmboIncritos()`

Para realizar el control de la cantidad de artistas pendientes por inscribir; si no existen más, preparar para grabar registro:

```
function controlCmboIncritos() {
    let kArtBand = $("#cboArtBand option").length;
    if (kArtBand <= 0) {
        alerta("Sin Artistas para inscribir", "Alerta");
        cambiarLblBtnAgregar(3);
    }
}

```

#### ii. Función sincrónica `mostrarInscritos()`

Para realizar el llenado de la tabla HTML `tablaDetalleMae` de la sección `seccionDetalleMae` a partir del vector `detMae` con los registros de los artistas inscritos de forma temporal para una inscripción. Sería el Detalle de la inscripción:

```
function mostrarInscritos() {
    let detalles = $("#tablaDetalleMae"); // Tabla de artistas agregados temporalmente (Detalle)
    detalles.empty(); // Limpiar la tabla antes de agregar los artistas actuales
    // Iterar sobre el array de detMae (está en agregarArtista) y agregar cada uno a la tabla
    detMae.forEach((rgtro, index) => {
        detalles.append(`
            <tr>
                <td>${rgtro.idArtBand}</td> <!-- ID del artista de ArtBand -->
                <td>${rgtro.nombre}</td>    <!-- nombre del artista -->
                <td>${rgtro.edad}</td>      <!-- edad del artista -->
                <td><button class="btn btn-danger btn-sm" onclick="eliminarRgtroPreInscrip(${index})">Eliminar</button></td>
                <!-- Botón para eliminar el registro de un artista inscrito -->
            </tr>
        `);
    });

    $("#seccionDetalleMae").slideDown();
    if (detMae.length === 0) { // Mostrar u ocultar mensaje de "sin artistas inscritos"
        $("#mensajeSinDetalle").show();
    } else {
        $("#mensajeSinDetalle").hide();
    }
}

```

#### v. Función sincrónica `vrTotalInscrip()`

Para recalcular el valor a pagar por la inscripción:

```
function vrTotalInscrip() {
    let vrIndInsc = parseInt($("#lblVrIndInsc").text()) || 0;
    let vrTot = document.getElementById("tablaDetalleMae").rows.length * vrIndInsc; // Cuenta solo las filas dentro del cuerpo
    $("#txtValorTot").val(vrTot);
}

```

#### vi. Función sincrónica `recuperarArtCmbo()`

Para ingresar (recuperar) los datos de un artista de una banda al combo `cboArtBand` después de haber sido retirado de la pre-inscripción:

```
function recuperarArtCmbo(codArtAB, name) {
    cmbAB = document.getElementById('cboArtBand');
    var opt = document.createElement('option');
    opt.value = codArtAB;
    opt.innerHTML = name;
    cmbAB.appendChild(opt);
    consultarInstrum();
}

```

#### vii. Función sincrónica `eliminarRgtroPreInscrip()`

Para eliminar el registro de una inscripción en curso de la lista `detMae`, volver a recuperar el artista al combo de ArtBanda, actualizar la tabla y el valor a pagar:

```
function eliminarRgtroPreInscrip(index) {
    var codArtAB = detMae[index].idArtBand;
    var name = detMae[index].nombre;
    let txt = "¿Seguro desea eliminar la inscripción de: " + name + "?";
    
    // Confirmación
    let rpta = window.confirm(txt);
    if (!rpta) {
        alerta('Cancelada acción de eliminación', 'Atención');
        return;
    } else {
        detMae.splice(index, 1); // Elimina el artista del array 'detMae' en la posición 'index'
        recuperarArtCmbo(codArtAB, name);
        cambiarLblBtnAgregar(1);
        alerta("Registro de: " + name + " ha sido borrado", "Información");
        mostrarInscritos(); // Vuelve a mostrar la lista actualizada de artistas en la tabla
        vrTotalInscrip();
    }
}

```

#### viii. Función sincrónica `quitarArtistaCmbPreInscCombo()`

Para retirar del combo `cboArtBand` los artistas de una banda que ya se encuentran registrados en la prematrícula desde la tabla HTML `tablaDetalleMae`:

```
function quitarArtistaCmbPreInscCombo() {
    let kRgtros = $("#tablaDetalleMae tr").length;
    if (kRgtros > 0) {
        for (i = 0; i < kRgtros; i++) {
            let codAB = $("#tablaDetalleMae").find('tr').eq(i).find('td').eq(0).html();
            $("#cboArtBand option[value='" + codAB + "']").remove();
        }
    }
    controlCmboIncritos();
}

```

#### ix. Función sincrónica `agregarDetalle_PreInsc()`

Para adicionar el registro de un artista al array `detMae` que contiene los artistas ya preinscritos y actualizar el combo `cboArtBand`, el valor total de la inscripción, la tabla con los inscritos, la etiqueta del botón agregar y establecer si desea o no continuar agregando artistas a la preinscripción:

```
function agregarDetalle_PreInsc() { // Artista
    try {
        // Captura los valores del artista a inscribir
        let idCodAB = $("#cboArtBand").val();
        let nombreArt = $("#cboArtBand option:selected").text(); // Opción seleccionada en el combo
        let edadA = parseInt($("#txtEdad").val()) || 0;

        // Validar datos del artista a inscribir
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

        // Agregar el artista inscrito al array
        detMae.push({
            idArtBand: idCodAB,
            nombre: nombreArt,
            edad: edadA
        });

        mostrarInscritos(); // Mostrar artistas registrados y total actualizado
        vrTotalInscrip();
        quitarArtistaCmbPreInscCombo();
        alerta(nombreArt + ", agregado con éxito", "Procedimiento con éxito");
        idCtlBand = $("#cboBanda").val();

        // Confirmar si desea agregar más
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
        // Manejo de excepción
    }
}

```

## 1.7. Grabado de la Inscripción (Maestro-Detalle) (Páginas 13 a 17)

### 1.7.3. En la clase operativa: `clsOpeInscrip` (Página 13)

* **i. Usings necesarios:**

  ```
  using static apiFestRock.Clases.Auxiliar.clsInscripCompleta;
  
  ```

* **ii. Crear el método `registrarDetalles`:**
  Retorna un valor booleano y recibe como argumentos el número de la inscripción y la lista de tipo de la clase auxiliar `detalleInsc` con nombre `inscritos` con los datos de los artistas que serán registrados en la inscripción en la tabla `TblDetInscs` del context del Entity Framework:

  ```
  public bool registrarDetalles(int idNroInsc, List<detalleInsc> inscritos)
  {
      try
      {
          foreach (var insc in inscritos)
          {
              // Recorre cada detalle de la inscripción
              TblDetInsc tblDetInsc = new TblDetInsc();
              // Propiedad de la clase que apunta a la tabla del modelo Entity Framework
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

* **iii. Crear el método `grabarDetalleInd`:**
  Realizará el procedimiento de grabar un registro del detalle de cada inscripción:

  ```
  public int grabarDetalleInd(int idNroInsc, int idArtBanda, int edad)
  {
      try
      {
          // Propiedad de la clase que apunta a la tabla del modelo Entity Framework
          TblDetInsc tblDetInsc = new TblDetInsc();
          // Codigo es Identity, Autoincrementable
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

* **ii (Bis). Crear el método `registrarEncabezado`:**
  Retorna el número de inscripción del encabezado al grabar en la tabla maestra (`encabezado`) `TbIInscrips` del context del Entity Framework:

  ```
  public int registrarEncabezado(inscripCompleta datosEncab)
  {
      int nuevoID = (ultimo <= 0) ? 1 : ultimo + 1;
      var ultimo = ctx.TbIInscrips.Max(x => (int?)x.Codigo) ?? 0;
  
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

* **iii (Bis). Crear el método `eliminarEncabezado`:**
  Elimina el encabezado de una inscripción fallida al momento de realizar el registro de una inscripción:

  ```
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

* **i. Agregar el using:**

  ```
  using static apiFestRock.Clases.Auxiliar.clsInscripCompleta;
  
  ```

* **ii. Crear el servicio con el método asincrónico `HttpPost` con alias `agregarInscrip`:**
  Recibe como parámetro de tipo clase auxiliar: `inscripCompleta` con nombre: `incripcWeb` y retorna un entero con el número de grabada la inscripción:

  ```
  [HttpPost("agregarInscrip")]
  public int Post([FromBody] inscripCompleta incripcWeb)
  {
      try
      {
          // 1. Guardar el encabezado de la inscripción
          clsOpeInscrip opeI = new clsOpeInscrip(ctx);
          int idNroInsc = opeI.registrarEncabezado(incripcWeb);
  
          // 2. Guardar el(los) artista(s) inscrito(s) como detalle de la inscripción
          clsOpeDetInscrip opeDI = new clsOpeDetInscrip(ctx);
          if (opeDI.registrarDetalles(idNroInsc, incripcWeb.Inscritos))
          {
              return idNroInsc; // Nro. id de registro, procedimiento con éxito
          }
          else
          {
              opeI.eliminarEncabezado(idNroInsc);
              return 0;
          }
      }
      catch
      {
          return -1; // Error por procedimiento
      }
  }
  
  ```

* **ii (Prueba). Ejecutar el proyecto y probar el servicio con Swagger:**
  Estructura JSON:

  ```
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

* **iii. Probar con Swagger el servicio `buscarInscrip`:**
  Para mirar el encabezado de lo grabado, luego el método: `consultaDatosDetInscrip`, para mirar el detalle de la inscripción.

### 1.7.5. En el controlador: `detInscripc` (Página 14)

* **i. Crear el método de tipo Post `adicionaDetalleInd` con el alias `agregarDetalleInd`:**
  Realizará el procedimiento de adicionar un registro al detalle de una inscripción existente previamente:

  ```
  // Agregar un detalle a inscripción grabada anteriormente
  [HttpPost("agregarDetalleInd")]
  public int adicionaDetalleInd(int nroInscrip, int idArtBand, int edad)
  {
      // Agregar una asignatura a una matrícula
      clsOpeDetInscrip oX = new clsOpeDetInscrip(ctx);
      return oX.grabarDetalleInd(nroInscrip, idArtBand, edad);
  }
  
  ```

### 1.7.6. En el archivo JavaScript del formulario para el proceso de Agregar una inscripción (Páginas 15 a 17)

#### i. Función sincrónica `validar()`

Para validar los datos generales de la inscripción al momento de grabar:

```
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

```

#### ii. Función sincrónica `controlCmboArtistas()`

Para verificar la cantidad de artista por inscribir y la consulta del instrumento que toca o acción que realiza en la banda:

```
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

```

#### iii. Función asincrónica `grabarInscripc()`

Para adicionar una inscripción (Maestro/Detalle) con los artistas a registrar relacionados en el array `detInscritos` y acciones después de dicha grabación:

```
async function grabarInscripc() {
    try {
        // Capturar información y validación base
        let nroIn = $("#txtCodigo").val();
        let idEve = $("#cboEvento").val();
        let idBan = $("#cboBanda").val();
        let repre = $("#txtRepres").val();
        let telRe = $("#txtTelRep").val();
        let vrTot = parseFloat($("#txtValorTot").val()) || 0;

        // Validar información
        if (!validar(repre, telRe, vrTot)) return;

        // Si es nuevo registro de inscripción
        if (nroIn == 0) {
            // Datos encabezado y detalles en la lista: artistas inscritos
            let fechInsc = new Date().toISOString().split('T')[0];
            // Crea un objeto con la fecha y hora actuales y las convierte en un texto estándar de tipo ISO 8601
            // (por ejemplo: 2026-06-07T17:10:00.000Z). La T separa la fecha de la hora

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
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(datosParaInscripc) // Convierte el pedido a JSON y lo envía al servidor
            });

            const rpta = await envio.text(); // Obtiene la respuesta del servidor
            let nroInsc = rpta.toString();

            if (nroInsc <= 0) {
                alerta("Se produjo error al grabar inscripción, revisar", "Error en grabar");
                return;
            }

            $("#txtCodigo").val(nroInsc);
            $("#seccionDetalleMae").hide();
            let inscritos = $("#tablaDetalleMae"); // Tabla de artistas agregados
            inscritos.empty();
            cambiarLblBtnAgregar(1);
            await Consultar();
            alerta("Se generó la inscripción #" + nroInsc, "Proceso con éxito"); // Muestra un mensaje de éxito
            return;
        } else if (parseInt(nroIn, 10) > 0) { // Cuando se adiciona en inscripción ya en BD
            let idCodAB = $("#cboArtBand").val();
            let edadArt = $("#txtEdad").val();

            // Validar datos del artista a inscribir
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

            // Confirmación
            let continua = window.confirm(txt);
            if (!continua) {
                alerta('Cancelada acción de agregar', 'Información');
                return;
            }

            let url = dir + "detInscripc/agregarDetalleInd?nroInscrip=" + nroIn + "&idArtBand=" + idCodAB + "&edad=" + edadArt;
            const datosEnvio = await fetch(url, {
                method: "POST",
                mode: "cors",
                headers: {
                    "content-type": "application/json"
                }
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
        alerta("Error al grabar inscripción: " + error, "Alerta"); // Manejo de errores
    }
}

```

#### vi. Asignación del listener al botón

Agregar al listener del botón `btnAgre` la invocación del método `grabarInscripc()`; en la parte verdadera del if que se encuentra codificado:

```
grabarInscripc();

```

## 1.8. Eliminación del Registro de una Inscripción (Páginas 17 a 18)

*(Artista de la banda) usando el listener de la `tablaDatos`, ya grabado en la BD.*

### 1.8.1. En la clase operativa: `clsOpeDetInscrip`

* **i. Crear el método `Eliminar`:**
  Con un parámetro de tipo entero que indica el número de la inscripción, `idDetInsc` y retorna una cadena del registro eliminado o no encontrado del artista registrado en la inscripción de la tabla `TblDetInscs` del context del Entity Framework:

  ```
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

* **i. Crear el servicio con el método asincrónico `HttpDelete` con alias: `borrarDetalle`:**
  Con retorno de tipo: cadena, que recibe un parámetro de tipo entero con el código del detalle de la inscripción: `idDet`:

  ```
  [HttpDelete("borrarDetalle")]
  public string borradoDetalle(int idDet)
  {
      // Borrar una inscripción por código
      clsOpeDetInscrip oX = new clsOpeDetInscrip(ctx);
      return oX.Eliminar(idDet);
  }
  
  ```

### 1.8.3. En el archivo JavaScript del formulario para el proceso de Eliminar/Borrar un artista existente en BD:

* **i. Crear la función asincrónica `eliminarDetInsc()`:**
  Para eliminar un registro de inscripción de la tabla utilizando el listener de la tabla: `tablaDatos`:

  ```
  async function eliminarDetInsc(idDetInsc) {
      try {
          // Realiza una solicitud DELETE al servidor para eliminar el detalle de la inscripción con el ID proporcionado
          let url = dir + "detInscripc/borrarDetalle?idDet=" + idDetInsc;
          let respuesta = await fetch(url, {
              method: "DELETE",
              headers: { "Content-Type": "application/json" }
          });
  
          // Convierte la respuesta a texto
          let rpta = await respuesta.text();
  
          if (rpta.substring(0, 5) === "Error") { // Si la respuesta empieza con "Error", muestra un mensaje de error
              alerta(rpta, "Error en procedimiento");
          } else {
              alerta(rpta, "Atención"); // Muestra un mensaje de éxito con la respuesta del servidor
              // Limpia DataTable
              $("#tablaDatos tr").empty();
              // Recarga la tabla para reflejar los cambios
              await llenarTabla(); // Llama a la función que actualiza la tabla con los nuevos datos
              await llenarComboArtista();
              await retirarDatosCmboArtBand();
              controlCmboArtistas();
          }
      } catch (err) {
          alerta(err, "No se pudo eliminar el registro.");
      }
  }
  
  ```

* **ii. Código del listener en la tabla:**
  Dentro del método: `jQuery`, después de la invocación de los métodos de llenado de los combos, el siguiente pedazo de código con el listener de la tabla: `tablaDatos` y que realiza la acción del botón: `btnEliminar` que se creó al momento de llenar la tabla:

  ```
  $(document).on("click", "#tablaDatos .btnEliminar", function (evento) {
      // Esto permite al desarrollador tomar el control del comportamiento del evento y realizar acciones personalizadas
      evento.preventDefault();
      evento.stopPropagation(); // Para que no seleccione la fila cuando se elimina
  
      let fila = $(this).closest("tr");
      let idDetalleInsc = fila.find("td:eq(0)").text(); // Recupera el valor acorde el índice de la columna
      let nombre = fila.find("td:eq(3)").text();
      let codAB = fila.find("td:eq(2)").text();
  
      let txt = "¿Seguro desea eliminar la inscripción de: " + nombre + "?";
      if (confirm(txt)) {
          eliminarDetInsc(parseInt(idDetalleInsc));
      }
  });
  
  ```

## 1.9. Modificar Datos del Encabezado de la Inscripción en la BD (Páginas 18 a 19)

### 1.9.1. En la clase operativa: `clsOpeDetInscrip` / `clsOpeInscrip`

* **i. Crear el método `Modificar`:**
  Para modificar un registro acorde al valor del código contenido en la propiedad y retorna una cadena del registro modificado o registro no actualizado en la tabla `TbIInscrips` del context del Entity Framework:

  ```
  public string Modificar()
  {
      try
      {
          // Verifica que el código de inscripción no sea 0 o nulo
          if (tblInsc.Codigo <= 0)
          {
              return "Inscripción: " + tblInsc.Codigo + ", no es válido.";
          }
  
          // Busca la inscripción en la base de datos que tenga el mismo código de inscripción que llega en la propiedad: tblInsc
          var p = ctx.TbIInscrips.FirstOrDefault(x => x.Codigo == tblInsc.Codigo);
  
          // Si no se encuentra ninguna inscripción con ese código (p == null) devuelve ese mensaje.
          if (p == null)
          {
              return "Inscripción " + tblInsc.Codigo + ", no existe.";
          }
  
          // Actualiza cada campo del encabezado de la inscripción encontrada con los nuevos valores que vienen en el objeto tblInsc.
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

* **i. Crear el servicio con el método asincrónico `HttpPut` con alias: `modificaEncabinscrip`:**
  Con retorno de tipo: cadena, que recibe un parámetro de tipo: `TbIInsc` con el código del registro de la inscripción: `objIn` y que invoca el método: `Modificar` de la clase: `clsOpeInscrip`:

  ```
  // Modificar el encabezado
  [HttpPut("modificaEncabinscrip")]
  public string Put([FromBody] TbIInsc objIn)
  {
      clsOpeInscrip oX = new clsOpeInscrip(ctx);
      oX.tblInsc = objIn;
      return oX.Modificar();
  }
  
  ```

### 1.9.3. En el archivo JavaScript del formulario para el proceso de modificar el encabezado de una inscripción existente en BD:

* **i. Crear la function asincrónica `Modificar()`:**
  Para modificar los datos del encabezado de un registro de inscripción:

  ```
  async function Modificar() {
      let nroInsc = $("#txtCodigo").val(); // Obtener el código de la inscripción
      // Validar que haya una inscripción (consultado previamente)
      if (isNaN(nroInsc) || nroInsc.trim() === "" || parseInt(nroInsc, 10) <= 0) {
          alerta("Solo se modifica un registro existente.", "Atención");
          $("#txtCodigo").trigger("focus");
          return;
      }
  
      // Captura de los campos
      let idEven = $("#cboEvento").val();
      let repres = $("#txtRepres").val();
      let telRep = $("#txtTelRep").val();
  
      // !NaN (Not a Number)
      if (repres.trim() === "" || telRep.trim() === "") {
          alerta("Faltan datos válidos para modificar", "Advertencia");
          $("#txtRepres").trigger("focus");
          return;
      }
  
      // Confirmación
      let rpta = window.confirm('¿Seguro de modificar la inscripción: ' + nroInsc + '?');
      if (rpta == false) {
          alerta('Cancelada acción de modificación', 'Información');
          return;
      } else {
          // Crear objeto en formato clave: valor
          let datosParaModif = {
              codigo: parseInt(nroInsc, 10),
              idEvento: parseInt(idEven, 10),
              represent: repres,
              telRepresent: telRep
          };
  
          try { // Invocar API con fetch
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
  
  ```

* **ii. Agregar al listener del botón: `btnModi` la invocación del método `Modificar()`:**

  ```
  Modificar();
  
  ```