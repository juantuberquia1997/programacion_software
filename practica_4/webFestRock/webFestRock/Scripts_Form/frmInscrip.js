var dir = "http://localhost:5114/api/";

//Detalle de la inscripción (tblDetInsc): artistas inscritos con su edad
var detalleArtistas = [];
//Cache de los artistas de la banda seleccionada (para el combo cboArtBand)
var artistasBanda = [];

//Fecha de hoy con formato dd/mm/yyyy
function fechaHoy() {
    let hoy = new Date();
    let dd = String(hoy.getDate()).padStart(2, "0");
    let mm = String(hoy.getMonth() + 1).padStart(2, "0");
    let yyyy = hoy.getFullYear();
    return dd + "/" + mm + "/" + yyyy;
}

// Funciones para el llenado de los combos del formulario
async function llenarComboEvento() {
    let url = dir + "evento/listadoEventos";
    let rpta = await llenarComboGral(url, "#cboEvento");
    if (rpta != "Termino") {
        alerta("error en llenado combo evento", "Error en procedimiento");
        return;
    }
    // Valor unitario de inscripción del evento seleccionado
    await consultarVrInscrip();
}

//Consultar el valor por artista del evento seleccionado (lblVrIndInsc)
async function consultarVrInscrip() {
    try {
        let idEvento = $("#cboEvento").val();
        if (idEvento == null || idEvento <= 0) {
            return;
        }
        let url = dir + "evento/consultaVrInscrip?idEvent=" + idEvento;
        const datosOut = await fetch(url, {
            method: "GET",
            mode: "cors",
            headers: {
                "content-type": "application/json",
            }
        });
        const datosIn = await datosOut.json();
        if (datosIn == "") {
            alerta("No se encontró el evento", "Información del proceso");
            return;
        }
        $("#lblVrIndInsc").text(datosIn[0].vrInsc);
        actualizarValorTot();
    } catch (error) {
        alerta("Error: " + error, "Error al consultar el valor");
    }
}

async function llenarComboBanda() {
    let url = dir + "banda/listadoBandas";
    let rpta = await llenarComboGral(url, "#cboBanda");
    if (rpta != "Termino") {
        alerta("error en llenado combo banda", "Error en procedimiento");
        return;
    }
    // Llenado anidado: artistas de la banda seleccionada
    await llenarComboArtXBand();
}

//Combo anidado: artistas de la banda seleccionada con su instrumento (cboArtBand)
async function llenarComboArtXBand() {
    try {
        let codBanda = $("#cboBanda").val();
        if (codBanda == null || codBanda <= 0) {
            return;
        }
        let url = dir + "banda/listadoArtXBand?codBanda=" + codBanda;
        const datosOut = await fetch(url, {
            method: "GET",
            mode: "cors",
            headers: {
                "content-type": "application/json",
            }
        });
        const datosIn = await datosOut.json();
        artistasBanda = datosIn;  // Cache para mostrar el instrumento al seleccionar
        //El servicio retorna idArtBand/artista: se llena el combo manualmente
        $("#cboArtBand").empty();
        for (i = 0; i < datosIn.length; i++) {
            $("#cboArtBand").append('<option value=' + datosIn[i].idArtBand + '>' + datosIn[i].artista + '</option>');
        }
        $("#txtInstrum").val("");
        $("#txtEdad").val("");
    } catch (error) {
        alerta("Error: " + error, "Error al cargar los artistas de la banda");
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
            alerta("Error, procedimiento no válido.", "Error, Buscar primero una inscripción");
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

    //Adicionar un artista al detalle de la inscripción
    $("#btnAgreArt").on("click", function () {
        agregarArtista();
    });

    //Al cambiar el evento: actualizar el valor unitario de inscripción
    $("#cboEvento").on("change", function () {
        consultarVrInscrip();
    });

    //Al cambiar la banda: llenar el combo de artistas de esa banda
    $("#cboBanda").on("change", function () {
        llenarComboArtXBand();
    });

    //Al seleccionar un artista: mostrar su instrumento
    $("#cboArtBand").on("change", function () {
        let idAB = $(this).val();
        let art = artistasBanda.find(x => x.idArtBand == idAB);
        $("#txtInstrum").val(art ? art.instrumento : "");
    });

    //Fecha actual del sistema
    $("#lblFechaAct").text(fechaHoy());

    //Llenado de los combos
    llenarComboEvento();
    llenarComboBanda();

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
    let url = dir + "inscrip/consultaInscrip?comando=2&dato=0";
    let rpta = await llenarTablaGral(url, "#tablaDatos");
    if (rpta != "Termino") {
        alerta("Error en llenado de tabla", "Error en procedimiento");
    }
}

//Buscar desde la tabla: consulta por el nro. de inscripción de la fila seleccionada
function editarFila(datosFila) {
    let nroInsc = datosFila.find("td:eq(1)").text();  // Columna 1 (td:eq(1)): nroInsc
    if (nroInsc > 0) {
        ConsultarXCodigo(nroInsc);
        return;
    }
    alerta("Sin registros", "Procedimiento incompleto");
}

function Cancelar() {
    $("#txtCodigo").val("0");
    $("#cboEvento")[0].options[0].selected = true;
    consultarVrInscrip();
    $("#cboBanda")[0].options[0].selected = true;
    llenarComboArtXBand();
    $("#txtRepres").val("");
    $("#txtTelRep").val("");
    $("#txtValorTot").val("");
    $("#lblFechaInsc").text(fechaHoy());
    //Limpiar el detalle de artistas
    limpiarDetalle();
}

//Limpiar el detalle de artistas de la inscripción
function limpiarDetalle() {
    detalleArtistas = [];
    $("#tablaDetalleMae").empty();
    $("#seccionDetalleMae").hide();
    $("#mensajeSinDetalle").hide();
    $("#txtEdad").val("");
    $("#txtInstrum").val("");
}

//Adicionar el artista seleccionado (combo + edad) al detalle de la inscripción
function agregarArtista() {
    let idAB = $("#cboArtBand").val();
    let edad = $("#txtEdad").val();
    if (idAB == null || idAB <= 0) {
        alerta("Seleccione un artista", "Error en procedimiento");
        return;
    }
    if (isNaN(edad) || parseInt(edad, 10) <= 0 || edad.trim() == "") {
        alerta("Error, la edad no es válida", "Error en procedimiento");
        $("#txtEdad").trigger("focus");
        return;
    }
    //Validar que el artista no esté ya en el detalle
    if (detalleArtistas.find(x => x.idArtBand == idAB)) {
        alerta("El artista ya está en el detalle de la inscripción", "Error en procedimiento");
        return;
    }
    let art = artistasBanda.find(x => x.idArtBand == idAB);
    detalleArtistas.push({
        idArtBand: parseInt(idAB, 10),
        edad: parseInt(edad, 10)
    });
    pintarDetalle(art.idArtBand, art.artista, art.instrumento, edad);
    actualizarValorTot();
}

//Pintar una fila del detalle de artistas inscritos
function pintarDetalle(idArtBand, nomArt, nomIns, edad) {
    $("#seccionDetalleMae").show();
    $("#mensajeSinDetalle").hide();
    let fila = '<tr>' +
        '<td>' + idArtBand + '</td>' +
        '<td>' + nomArt + '</td>' +
        '<td>' + nomIns + '</td>' +
        '<td>' + edad + '</td>' +
        '<td><button type="button" class="btn btn-danger btn-sm btnQuitar">Quitar</button></td>' +
        '</tr>';
    $("#tablaDetalleMae").append(fila);
    //Quitar un artista del detalle: la posición de la fila coincide con la del arreglo
    $("#tablaDetalleMae tr").last().find(".btnQuitar").on("click", function () {
        let idx = $(this).closest("tr").index();
        detalleArtistas.splice(idx, 1);
        $(this).closest("tr").remove();
        if (detalleArtistas.length == 0) {
            $("#seccionDetalleMae").hide();
            $("#mensajeSinDetalle").show();
        }
        actualizarValorTot();
    });
}

//Actualizar el valor total: valor unitario del evento x nro. de artistas del detalle
function actualizarValorTot() {
    let vrUnit = parseFloat($("#lblVrIndInsc").text());
    if (isNaN(vrUnit)) {
        vrUnit = 0;
    }
    $("#txtValorTot").val((vrUnit * detalleArtistas.length).toLocaleString("es-CO"));
}

//Consultar o buscar una inscripción por el evento y la banda seleccionados
async function Consultar() {
    try {
        let idEvento = $("#cboEvento").val();
        let idBanda = $("#cboBanda").val();
        if (idEvento == null || idEvento <= 0 || idBanda == null || idBanda <= 0) {
            alerta("Seleccione el evento y la banda", "Error en procedimiento");
            return;
        }
        let url = dir + "inscrip/consultaInscrip?comando=4&dato=" + idEvento + "&dato2=" + idBanda;
        const datosOut = await fetch(url, {
            method: "GET",
            mode: "cors",
            headers: {
                "content-type": "application/json",
            }
        });
        const datosIn = await datosOut.json();
        if (datosIn == "") {
            alerta("La banda no está inscrita en el evento seleccionado", "Información del proceso");
            return;
        }
        await ConsultarXCodigo(datosIn[0].codigo);
    } catch (error) {
        alerta("Error: " + error, "Error al consultar");
    }
}

//Consultar una inscripción por su código (nro. de inscripción)
async function ConsultarXCodigo(codigo) {
    try {
        let url = dir + "inscrip/consultaInscrip?comando=1&dato=" + codigo;
        const datosOut = await fetch(url, {
            method: "GET",
            mode: "cors",
            headers: {
                "content-type": "application/json",
            }
        });
        const datosIn = await datosOut.json();
        if (datosIn == "") {
            alerta("No se encontró la inscripción con código: " + codigo, "Información del proceso");
            return;
        }
        //Seleccionar el evento y la banda de la inscripción
        $("#cboEvento").val(datosIn[0].idEvento);
        await consultarVrInscrip();
        $("#cboBanda").val(datosIn[0].idBanda);
        await llenarComboArtXBand();
        //Llenar los datos del encabezado
        $("#txtCodigo").val(datosIn[0].codigo);
        $("#lblFechaInsc").text(datosIn[0].fecha);
        $("#txtRepres").val(datosIn[0].represent);
        $("#txtTelRep").val(datosIn[0].tel);
        $("#txtValorTot").val(datosIn[0].valor.toLocaleString("es-CO"));
        //Cargar el detalle de artistas inscritos
        limpiarDetalle();
        let urlDet = dir + "inscrip/consultaInscrip?comando=3&dato=" + datosIn[0].codigo;
        const detOut = await fetch(urlDet, {
            method: "GET",
            mode: "cors",
            headers: {
                "content-type": "application/json",
            }
        });
        const detIn = await detOut.json();
        for (i = 0; i < detIn.length; i++) {
            detalleArtistas.push({
                idArtBand: detIn[i].idArtBand,
                edad: detIn[i].edad
            });
            pintarDetalle(detIn[i].idArtBand, detIn[i].artista, detIn[i].instrumento, detIn[i].edad);
        }
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

//Validar la información de entrada: representante, teléfono y mínimo 1 artista
function validar(repres, tel) {
    var msj = "Falta ";
    if (repres.trim() == "") {
        msj = msj + "El representante no válido, \n";
    }
    if (tel.trim() == "") {
        msj = msj + "el teléfono no válido. \n";
    }
    if (detalleArtistas.length == 0) {
        msj = msj + "ingresar mínimo un artista (solitario) en el detalle. \n";
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
    let idEv = $("#cboEvento").val();
    let idBa = $("#cboBanda").val();
    let Repr = $("#txtRepres").val();
    let TelR = $("#txtTelRep").val();

    //Validar información
    if (idEv == null || idEv <= 0 || idBa == null || idBa <= 0) {
        alerta("Seleccione el evento y la banda", "Error en procedimiento");
        return;
    }
    if (!validar(Repr, TelR)) {
        $("#txtRepres").trigger("focus");
        return;
    }

    //Confirmar Procedimiento
    let rpta = "";
    if (accion === "POST") {
        rpta = window.confirm('¿Estás seguro de inscribir la banda en el evento con ' + detalleArtistas.length + ' artista(s)?');
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
        rpta = window.confirm('¿Estás seguro de modificar la inscripción: ' + Codi + '? (el valor no se recalcula)');
        if (rpta == false) {
            alerta('Cancelada acción de Modificar', 'Información');
            return;
        }
    }
    else {
        alerta("Procedimiento no válido", "Error");
        return;
    }

    //Crear JSon: encabezado (tblInscrip) + detalle de artistas (tblDetInsc)
    //El valor total lo calcula la API (valor del evento x nro. de artistas)
    let datosOut = {
        tblInsc: {
            codigo: parseInt(Codi, 10) || 0,
            idEvento: parseInt(idEv, 10),
            idBanda: parseInt(idBa, 10),
            represent: Repr,
            telRepresent: TelR
        },
        detalle: detalleArtistas
    }

    //Invocar el servicio con fetch
    try {
        const response = await fetch(dir + "inscrip", {
            method: accion,
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(datosOut),
        });

        const rpta = await response.json();
        if (rpta == -1)
            alerta("La banda ya está inscrita en el evento", "Error en proceso");
        else if (rpta == -2)
            alerta("El detalle está vacío: mínimo un artista (solitario)", "Error en proceso");
        else if (rpta == 0)
            alerta("Error al momento del grabar, reintente por favor", "Error en grabar");
        else if (rpta > 0) {
            alerta("Registro grabado con éxito", "Proceso con éxito");
            $("#txtCodigo").val(rpta);  // Nro. de la inscripción grabada
            await ConsultarXCodigo(rpta);  // Recarga la inscripción con el detalle
            await llenarTabla();
        }
        else
            alerta(rpta, "Error en procedimiento");
    } catch (error) {
        alerta("Error: " + error, "Error en procedimiento");
    }
}
