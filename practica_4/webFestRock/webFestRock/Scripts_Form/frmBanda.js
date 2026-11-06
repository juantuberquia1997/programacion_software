var dir = "http://localhost:5114/api/";

//Detalle de artistas por banda (tblArtBand): código 0 = artista por adicionar
var detalleArtistas = [];

// Funciones para el llenado de los combos del formulario
async function llenarComboDpto() {
    let url = dir + "departamento/listadoDptos";
    let rpta = await llenarComboGral(url, "#cboDpto");
    if (rpta != "Termino") {
        alerta("error en llenado combo departamento", "Error en procedimiento");
        return;
    }
    // Llenado anidado del combo de ciudades
    llenarComboCiudad();
}

//Combo anidado: ciudades del departamento seleccionado (o una ciudad específica)
async function llenarComboCiudad(idCiu) {
    let idDpto = $("#cboDpto").val();
    let url = dir + "ciudad/listadoCiudsXDpto?codDpto=" + idDpto;
    let rpta = await llenarComboGral(url, "#cboCiudad");
    if (rpta != "Termino") {
        alerta("Error en llenado combo ciudad", "Error en procedimiento");
        return;
    }
    else if (idCiu != undefined) {
        $("#cboCiudad").val(idCiu);
    }
}

//Combo: todos los artistas registrados (para adicionar al detalle de la banda)
async function llenarComboArtistas() {
    // El servicio retorna código y nombre del artista (llenarComboGral usa esos campos)
    let url = dir + "artista/consultarArtista?comando=2&dato=0";
    let rpta = await llenarComboGral(url, "#cboArtBand");
    if (rpta != "Termino") {
        alerta("error en llenado combo artistas", "Error en procedimiento");
        return;
    }
}

//Combo: instrumentos activos
async function llenarComboInstrum() {
    let url = dir + "instrum/listadoInstrums";
    let rpta = await llenarComboGral(url, "#cboInstrum");
    if (rpta != "Termino") {
        alerta("error en llenado combo instrumentos", "Error en procedimiento");
        return;
    }
}

jQuery(function () {

    //Carga el menú
    $("#dvMenu").load("../Paginas/Menu.html");

    //Registrar los botones para responder al evento click
    $("#btnAgre").on("click", function () {
        $("#txtCodigo").val("0");
        ejecutarComando("POST");
    });
    $("#btnModi").on("click", function () {
        let cod = $("#txtCodigo").val();
        if (isNaN(cod) || parseInt(cod, 10) <= 0 || cod.trim() == "") {
            alerta("Error, procedimiento no válido.", "Error, Buscar primero una banda");
            return;
        }
        ejecutarComando("PUT");
    });
    $("#btnBusc").on("click", function () {
        Consultar();
    });
    $("#btnCanc").on("click", function () {
        Cancelar();
    });
    $("#btnImpr").on("click", function () {
        alerta("Impresión", "En futura construcción");
    });

    //Adicionar un artista al detalle de la banda
    $("#btnAgreArt").on("click", function () {
        agregarArtista();
    });

    //Llenado de los combos
    llenarComboDpto();
    llenarComboArtistas();
    llenarComboInstrum();

    //Llenado de la tabla
    llenarTabla();

    //Listener para la tabla (datatable): evento click sobre un registro (tr) de la tabla
    $("#tablaDatos").on("click", "tbody tr", function (evento) {
        evento.preventDefault();
        if ($(this).hasClass("selected")) {
            $(this).removeClass("selected");
        } else {
            $("#tablaDatos tbody tr.selected").removeClass("selected");
            $(this).addClass("selected");
            editarFila($(this).closest("tr"));
        }
    });

});  //Del: jQuery


async function llenarTabla() {
    let url = dir + "banda/consultaBanda?comando=2&dato=0";
    let rpta = await llenarTablaGral(url, "#tablaDatos");
    if (rpta != "Termino") {
        alerta("Error en llenado de tabla", "Error en procedimiento");
    }
}

//Buscar desde la tabla: consulta por el nombre de la banda de la fila seleccionada
function editarFila(datosFila) {
    let nombre = datosFila.find("td:eq(2)").text();  // Columna 2 (td:eq(2)): nombre
    if (nombre.trim() != "") {
        $("#txtNombre").val(nombre);
        Consultar();
        return;
    }
    alerta("Sin registros", "Procedimiento incompleto");
}

function Cancelar() {
    $("#txtCodigo").val("0");
    $("#txtNombre").val("");
    $("#txtAnio").val("");
    $("#cboDpto")[0].options[0].selected = true;
    llenarComboCiudad();
    $("#txtRepres").val("");
    $("#txtTelRep").val("");
    $("#chkActivo").prop("checked", true);
    //Limpiar el detalle de artistas
    limpiarDetalle();
    $("#txtNombre").trigger("focus");
}

//Limpiar el detalle de artistas de la banda
function limpiarDetalle() {
    detalleArtistas = [];
    $("#tablaDetalleMae").empty();
    $("#seccionDetalle").hide();
    $("#cboArtBand")[0].options[0].selected = true;
    $("#cboInstrum")[0].options[0].selected = true;
    $("#chkActArt").prop("checked", true);
}

//Adicionar el artista seleccionado (combo + instrumento + activo) al detalle
function agregarArtista() {
    let idArt = $("#cboArtBand").val();
    let nomArt = $("#cboArtBand option:selected").text();
    let idIns = $("#cboInstrum").val();
    let nomIns = $("#cboInstrum option:selected").text();
    let act = $("#chkActArt").prop("checked");
    if (idArt == null || idArt <= 0 || idIns == null || idIns <= 0) {
        alerta("Seleccione el artista y el instrumento", "Error en procedimiento");
        return;
    }
    //Validar que el artista no esté ya en el detalle
    if (detalleArtistas.find(x => x.idArtista == idArt)) {
        alerta("El artista ya está en el detalle de la banda", "Error en procedimiento");
        return;
    }
    detalleArtistas.push({
        codigo: 0,  // 0: artista nuevo en el detalle (lo asigna la base de datos)
        idArtista: parseInt(idArt, 10),
        idInstrum: parseInt(idIns, 10),
        activo: act
    });
    pintarDetalle(nomArt, nomIns, act);
}

//Pintar una fila del detalle de artistas agregados
function pintarDetalle(nomArt, nomIns, act) {
    $("#seccionDetalle").show();
    let fila = '<tr>' +
        '<td>' + nomArt + '</td>' +
        '<td>' + nomIns + '</td>' +
        '<td>' + (act ? 'Sí' : 'No') + '</td>' +
        '<td><button type="button" class="btn btn-danger btn-sm btnQuitar">Quitar</button></td>' +
        '</tr>';
    $("#tablaDetalleMae").append(fila);
    //Quitar un artista del detalle: la posición de la fila coincide con la del arreglo
    $("#tablaDetalleMae tr").last().find(".btnQuitar").on("click", function () {
        let idx = $(this).closest("tr").index();
        detalleArtistas.splice(idx, 1);
        $(this).closest("tr").remove();
        if (detalleArtistas.length == 0) {
            $("#seccionDetalle").hide();
        }
    });
}

//Cargar el detalle de artistas de una banda (código de tblArtBand incluido para el PUT)
async function cargarDetalle(codBanda) {
    limpiarDetalle();
    try {
        let url = dir + "banda/listadoArtXBand?codBanda=" + codBanda;
        const datosOut = await fetch(url, {
            method: "GET",
            mode: "cors",
            headers: {
                "content-type": "application/json",
            }
        });
        const datosIn = await datosOut.json();
        for (i = 0; i < datosIn.length; i++) {
            detalleArtistas.push({
                codigo: datosIn[i].idArtBand,
                idArtista: datosIn[i].idArtista,
                idInstrum: datosIn[i].idInstrum,
                activo: datosIn[i].activo
            });
            pintarDetalle(datosIn[i].artista, datosIn[i].instrumento, datosIn[i].activo);
        }
    } catch (error) {
        alerta("Error: " + error, "Error al cargar el detalle");
    }
}

//Consultar o buscar una banda por su nombre
async function Consultar() {
    try {
        let nombre = $("#txtNombre").val();
        if (nombre.trim() == "") {
            alerta("Error, el nombre no es válido", "Error en procedimiento");
            $("#txtNombre").trigger("focus");
            return;
        }
        let url = dir + "banda/consultaBanda?comando=1&dato=" + encodeURIComponent(nombre);
        const datosOut = await fetch(url, {
            method: "GET",
            mode: "cors",
            headers: {
                "content-type": "application/json",
            }
        });
        const datosIn = await datosOut.json();
        if (datosIn == "") {
            alerta("No se encontró banda con nombre: " + nombre, "Información del proceso");
            return;
        }
        $("#txtCodigo").val(datosIn[0].codigo);
        $("#txtNombre").val(datosIn[0].nombre);
        $("#txtAnio").val(datosIn[0].anioFund);
        // Seleccionar el departamento, llenar el combo de ciudades y seleccionar la ciudad
        $("#cboDpto").val(datosIn[0].idDep);
        await llenarComboCiudad(datosIn[0].idC);
        $("#txtRepres").val(datosIn[0].represent);
        $("#txtTelRep").val(datosIn[0].tel);
        $("#chkActivo").prop("checked", datosIn[0].act);
        //Cargar el detalle de artistas de la banda
        await cargarDetalle(datosIn[0].codigo);
    } catch (error) {
        alerta("Error: " + error, "Error al consultar");
    }
}

function alerta(msj, titulo = "Información") {  // Usa sweetalert2.all.min.js (Scripts_Form/Comunes)
    Swal.fire({
        position: 'center',
        type: 'info',
        title: titulo,
        text: msj
    });
}

//Validar la información de entrada: nombre, año, ciudad, representante y teléfono
function validar(nombre, anio, idCiu, repres, tel) {
    var msj = "Falta ";
    if (nombre.trim() == "") {
        msj = msj + "El nombre no válido, \n";
    }
    if (isNaN(anio) || parseInt(anio, 10) <= 0 || anio.trim() == "") {
        msj = msj + "el año de fundación no válido. \n";
    }
    if (idCiu == null || idCiu <= 0) {
        msj = msj + "la ciudad no válida. \n";
    }
    if (repres.trim() == "") {
        msj = msj + "el representante no válido. \n";
    }
    if (tel.trim() == "") {
        msj = msj + "el teléfono no válido. \n";
    }
    if (msj != "Falta ") {
        alerta(msj, "Error en procedimiento");
        return false;
    }
    return true;
}

//ejecutarComando: utilizada tanto para agregar (POST) como para modificar (PUT)
async function ejecutarComando(accion) {
    //Capturar los datos de entrada
    let Codi = $("#txtCodigo").val();
    let Nomb = $("#txtNombre").val();
    let Anio = $("#txtAnio").val();
    let idCi = $("#cboCiudad").val();
    let Repr = $("#txtRepres").val();
    let TelR = $("#txtTelRep").val();
    let acti = $("#chkActivo").prop("checked");

    //Validar información
    if (!validar(Nomb, Anio, idCi, Repr, TelR)) {
        $("#txtNombre").trigger("focus");
        return;
    }

    //Confirmar Procedimiento
    let rpta = "";
    if (accion === "POST") {
        rpta = window.confirm('¿Estás seguro de agregar la banda: ' + Nomb + ' con ' + detalleArtistas.length + ' artista(s)?');
        if (rpta == false) {
            alerta('Cancelada acción de Agregar', 'Información');
            return;
        }
    }
    else if (accion === "PUT") {
        if (isNaN(Codi) || parseInt(Codi, 10) <= 0 || Codi.trim() == "") {
            alerta("Código no válido para modificar registro", "Alerta");
            return;
        }
        rpta = window.confirm('¿Estás seguro de modificar la banda: ' + Nomb + '?');
        if (rpta == false) {
            alerta('Cancelada acción de Modificar', 'Información');
            return;
        }
    }
    else {
        alerta("Procedimiento no válido", "Error");
        return;
    }

    //Crear JSon: encabezado (tblBanda) + detalle de artistas (tblArtBand)
    let datosOut = {
        tblBand: {
            codigo: parseInt(Codi, 10) || 0,
            nombre: Nomb,
            anioFund: parseInt(Anio, 10),
            represent: Repr,
            telefono: TelR,
            idCiudad: parseInt(idCi, 10),
            activo: acti
        },
        detalle: detalleArtistas
    }

    //Invocar el servicio con fetch
    try {
        const response = await fetch(dir + "banda", {
            method: accion,
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(datosOut),
        });

        const rpta = await response.json();
        if (rpta == -1)
            alerta("Problema al grabar el registro (Revisar si existe)", "Error en proceso");
        else if (rpta == 0)
            alerta("Error al momento del grabar, reintente por favor", "Error en grabar");
        else if (rpta > 0) {
            alerta("Registro grabado con éxito", "Proceso con éxito");
            $("#txtCodigo").val(rpta);  // Clave primaria del registro grabado
            await Consultar();          // Recarga la banda con el detalle (códigos reales)
            await llenarTabla();
        }
        else
            alerta(rpta, "Error en procedimiento");
    } catch (error) {
        alerta("Error: " + error, "Error en procedimiento");
    }
}
