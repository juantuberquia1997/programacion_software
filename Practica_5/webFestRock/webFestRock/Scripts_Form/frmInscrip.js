//Form: frmInscrip - Maestro de Inscripciones
//Código según la guía de práctica 5: secciones 1.4 y 1.5
//(incluye las funciones de pre-inscripción de la 1.6 que son
//invocadas por los listener especificados en la 1.4, pág. 12)

//Variable de la API (guía 1.4.2.i) - puerto de ejecución de la API
var dir = "http://localhost:5114/api/";

//Arreglo de detalle en memoria: artistas preinscritos (guía 1.4.2.ii)
var detMae = [];

//Variables de control (guía 1.4.2.iii)
var idCtlBand = 0;
var sw = 0;

//Inicializar el DataTable de la tabla de consulta (guía 1.5.5.i)
var oTabla = $("#tablaDatos").DataTable();

//Fecha de hoy con formato dd-mm-yyyy (guía 1.4.2.vi)
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

//Consultar el valor individual de inscripción del evento seleccionado (guía 1.4.2.viii)
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
        alerta(error, "Error: ");
    }
}

//Consultar el instrumento que toca el artista seleccionado (guía 1.4.2.viii)
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

//Cambiar la etiqueta del botón Agregar según la acción (guía 1.4.2.ix)
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

//Funciones asincrónicas para el llenado de combos (guía 1.4.2.x)
async function llenarComboEventos() {
    let url = dir + "evento/listadoEventos";
    let rpta = await llenarComboGral(url, "#cboEvento");
    if (rpta == "Termino") {
        await vrInscEvento();
    }
}

async function llenarComboBandas() {
    $("#cboArtBand").empty(); // (la guía escribe "cboBandArt": es el combo de artistas)
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
    //Control de cambio de banda durante edición/pre-inscripción (guía 1.4, pág. 12)
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
                await llenarComboArtista(); // Restaura el combo de la banda en sesión
                return;
            }
        }
        if (ktblDM > 0) {
            quitarArtistaCmbPreInscCombo();
        } else if (ktbIDa > 2) {
            retirarDatosCmboArtBand();
        }
    }
}

//Limpiar los datos de entrada (guía 1.4.2.xvi)
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

//Cancelar/Limpiar el formulario completo (guía 1.4.2.xvii)
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
    //Referencia vigente del DataTable (llenarTablaGral lo reinicializa)
    oTabla = $("#tablaDatos").DataTable();
    oTabla.clear().draw();
}

//Llenar la tabla de consulta con el detalle de la inscripción (guía 1.5.5.ii)
async function llenarTabla() {
    let codInsc = parseInt($("#txtCodigo").val()) || 0;
    if (codInsc <= 0) {
        alerta("Número de inscripción no válido", "Error");
        $("#txtCodigo").trigger("focus");
        return;
    }

    let url = dir + "detInscripc/consultaDatosDetInscrip?nroInsc=" + codInsc;
    let rpta = await llenarTablaGral(url, "#tablaDatos");
    if (rpta != "Termino") { // llenarTablaGral (Tablas.js) retorna "Termino"
        alerta("Búsqueda: " + codInsc + ", sin artistas inscritos o no encontrada la inscripción", "Alerta/Información");
    }
}

//Retirar del combo de artistas por banda los ya registrados en la tabla
//de consulta (guía 1.5.5.iii)
function retirarDatosCmboArtBand() {
    let kRgtros = $("#tablaDatos tr").length;
    if (kRgtros > 0) {
        for (let i = 1; i <= kRgtros; i++) {
            let codAB = $("#tablaDatos").find("tr").eq(i).find("td").eq(2).html();
            $("#cboArtBand option[value='" + codAB + "']").remove();
        }
    }
}

//Consultar los datos de la cabecera de una inscripción (guía 1.5.5.iv)
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
        await vrInscEvento(); // Actualiza el valor unitario del evento de la inscripción
        $("#lblFechaInsc").text(Rpta[0].fec);
        $("#txtRepres").val(Rpta[0].repre);
        $("#txtTelRep").val(Rpta[0].telef);
        $("#txtValorTot").val(Rpta[0].vrTot);
        $("#cboBanda").val(Rpta[0].idBand);
        idCtlBand = Rpta[0].idBand;
        //Alinea el combo de artistas con la banda de la inscripción y retira
        //los artistas ya inscritos (igual que la guía 1.7.6 en grabarInscripc)
        await llenarComboArtista();
    } catch (error) {
        alerta(error, "Error: ");
    }
}

// ================= PRE-INSCRIPCIÓN (guía 1.6) =================
// Funciones invocadas por los listener de la sección 1.4 (pág. 12)

//Control del combo de artistas inscritos (guía 1.6.i)
function controlCmboIncritos() {
    let kArtBand = $("#cboArtBand option").length;
    if (kArtBand <= 0) {
        alerta("Sin Artistas para inscribir", "Alerta");
        cambiarLblBtnAgregar(3);
    }
}

//Mostrar los inscritos de la pre-inscripción en la tabla HTML (guía 1.6.ii)
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

//Recalcular el valor total de la inscripción (guía 1.6.v)
function vrTotalInscrip() {
    let vrIndInsc = parseInt($("#lblVrIndInsc").text()) || 0;
    let vrTot = document.getElementById("tablaDetalleMae").rows.length * vrIndInsc;
    $("#txtValorTot").val(vrTot);
}

//Recuperar un artista al combo tras eliminarlo de la pre-inscripción (guía 1.6.vi)
function recuperarArtCmbo(codArtAB, name) {
    let cmbAB = document.getElementById('cboArtBand');
    var opt = document.createElement('option');
    opt.value = codArtAB;
    opt.innerHTML = name;
    cmbAB.appendChild(opt);
    consultarInstrum();
}

//Eliminar un registro del arreglo en memoria detMae (guía 1.6.vii)
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

//Quitar del combo los artistas ya existentes en tablaDetalleMae (guía 1.6.viii)
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

//Adicionar un artista al arreglo local detMae (guía 1.6.ix)
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

//Alerta general de los formularios (usa sweetalert2.all.min.js)
function alerta(msj, titulo = "Información") {
    Swal.fire({
        position: 'center',
        type: 'info',
        title: titulo,
        text: msj
    });
}

jQuery(function () {

    //Carga el menú
    $("#dvMenu").load("../Paginas/Menu.html");

    //Registrar los botones para responder al evento click

    //Listener del botón Agregar (guía 1.4, pág. 12, xi)
    $("#btnAgre").on("click", function () {
        const txtBtnAgre = document.getElementById('btnAgre');
        let txt = txtBtnAgre.innerHTML;
        let nroInsc = $("#txtCodigo").val();
        if (txt == "Grabar Inscripción" || parseInt(nroInsc, 10) > 0) {
            alerta("En construcción", "Alerta"); // Se implementa en la guía 1.7 (grabarInscripc)
        } else { // Agregar artistas a la pre-inscripción como detalle
            agregarDetalle_PreInsc();
        }
    });

    //Listener del botón Modificar (se implementa en la guía 1.9)
    $("#btnModi").on("click", function () {
        let nroInsc = $("#txtCodigo").val();
        if (isNaN(nroInsc) || nroInsc.trim() == "" || parseInt(nroInsc, 10) <= 0) {
            alerta("Solo se modifica un registro existente, buscar primero", "Atención");
            return;
        }
        alerta("En construcción", "Alerta"); // Se implementa en la guía 1.9 (Modificar)
    });

    //Listener del botón Consultar (guía 1.5.5.v)
    $("#btnBusc").on("click", function () {
        Consultar();
    });

    //Listener del botón Cancelar (guía 1.4.2.xviii)
    $("#btnCanc").on("click", function () {
        Cancelar();
    });

    //Listener del botón Impresión
    $("#btnImpr").on("click", function () {
        alerta("Impresión", "En futura construcción");
    });

    //Eventos onchange de los combos (guía 1.4.2.xii a xiv)

    //En combo evento: consultar el valor individual de inscripción
    $("#cboEvento").on("change", function () {
        vrInscEvento();
    });

    //En combo banda: llenar el combo de artistas de la banda
    $("#cboBanda").on("change", function () {
        llenarComboArtista();
    });

    //En combo de artistas: consultar el instrumento que toca
    $("#cboArtBand").on("change", function () {
        consultarInstrum();
    });

    //Fecha actual del sistema (guía 1.4.2.vii)
    $("#lblFechaAct").text(fechaHoy());

    //Llenado de los combos (guía 1.4.2.xi)
    llenarComboEventos();
    llenarComboBandas();

    //Focus inicial en el combo de bandas (guía 1.4.2.v)
    $("#cboBanda").trigger("focus");

});  //Del: jQuery
