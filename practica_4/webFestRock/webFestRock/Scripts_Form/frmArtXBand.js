var dir = "http://localhost:5114/api/";

// Formulario de solo consultas (no graba datos)
// 1. Combo Evento   -> bandas participantes del evento
// 2. Combo Banda    -> artistas por banda (con su instrumento)
// 3. Combo Instrumento -> artistas por instrumento en el evento

async function llenarComboEvento() {
    let url = dir + "evento/listadoEventos";
    let rpta = await llenarComboGral(url, "#cboEvento");
    if (rpta != "Termino") {
        alerta("error en llenado combo evento", "Error en procedimiento");
        return;
    }
}

async function llenarComboBanda() {
    let url = dir + "banda/listadoBandas";
    let rpta = await llenarComboGral(url, "#cboBanda");
    if (rpta != "Termino") {
        alerta("error en llenado combo banda", "Error en procedimiento");
        return;
    }
    // Consulta inicial: artistas de la primera banda
    await consultarArtistasXBanda();
}

async function llenarComboInstrum() {
    let url = dir + "instrum/listadoInstrums";
    let rpta = await llenarComboGral(url, "#cboInstrum");
    if (rpta != "Termino") {
        alerta("error en llenado combo instrumento", "Error en procedimiento");
        return;
    }
}

jQuery(function () {

    //Carga el menú
    $("#dvMenu").load("../Paginas/Menu.html");

    //Registrar los botones para responder al evento click
    $("#btnAgre").on("click", function () {
        alerta("Este formulario es de solo consulta", "Información");
    });
    $("#btnModi").on("click", function () {
        alerta("Este formulario es de solo consulta", "Información");
    });
    $("#btnBusc").on("click", function () {
        //Ejecuta la consulta acorde al último combo seleccionado
        if ($("#cboInstrum").val() != null && $("#cboInstrum").val() > 0) {
            consultarArtistasXInstrum();
        }
        else if ($("#cboEvento").val() != null && $("#cboEvento").val() > 0) {
            consultarBandasXEvento();
        }
    });
    $("#btnCanc").on("click", function () {
        //Restablecer los combos y la consulta inicial
        $("#cboEvento")[0].options[0].selected = true;
        $("#cboInstrum")[0].options[0].selected = true;
        consultarArtistasXBanda();
    });
    $("#btnImpr").on("click", function () {
        alerta("Impresión", "En futura construcción");
    });

    //Al cambiar el evento: consultar las bandas participantes
    $("#cboEvento").on("change", function () {
        if ($(this).val() != null && $(this).val() > 0) {
            consultarBandasXEvento();
        }
    });

    //Al cambiar la banda: consultar los artistas de la banda
    $("#cboBanda").on("change", function () {
        if ($(this).val() != null && $(this).val() > 0) {
            consultarArtistasXBanda();
        }
    });

    //Al cambiar el instrumento: consultar los artistas por instrumento en el evento
    $("#cboInstrum").on("change", function () {
        if ($(this).val() != null && $(this).val() > 0) {
            consultarArtistasXInstrum();
        }
    });

    //Llenado de los combos
    llenarComboEvento();
    llenarComboBanda();
    llenarComboInstrum();

});  //Del: jQuery


//Llenar la tabla con el resultado de una consulta (valida si viene vacía)
async function llenarTablaConsulta(url) {
    try {
        const datosOut = await fetch(url, {
            method: "GET",
            mode: "cors",
            headers: {
                "content-type": "application/json",
            }
        });
        const datosIn = await datosOut.json();
        if (datosIn == "") {
            //Limpiar la tabla si hay una consulta previa (sin datos)
            if ($.fn.dataTable.isDataTable("#tablaDatos")) {
                $("#tablaDatos").DataTable().clear().destroy();
            }
            alerta("Sin datos para la consulta", "Información del proceso");
            return;
        }
        let rpta = await llenarTablaGral(url, "#tablaDatos");
        if (rpta != "Termino") {
            alerta("Error en llenado de tabla", "Error en procedimiento");
        }
    } catch (error) {
        alerta("Error: " + error, "Error en procedimiento");
    }
}

//Consulta: bandas participantes de un evento
async function consultarBandasXEvento() {
    let idEvento = $("#cboEvento").val();
    let url = dir + "consulta/bandasXEvento?idEvento=" + idEvento;
    await llenarTablaConsulta(url);
}

//Consulta: artistas por banda (con su instrumento)
async function consultarArtistasXBanda() {
    let codBanda = $("#cboBanda").val();
    if (codBanda == null || codBanda <= 0) {
        return;
    }
    let url = dir + "banda/listadoArtXBand?codBanda=" + codBanda;
    await llenarTablaConsulta(url);
}

//Consulta: artistas por instrumento en un evento
async function consultarArtistasXInstrum() {
    let idEvento = $("#cboEvento").val();
    let idInstrum = $("#cboInstrum").val();
    if (idEvento == null || idEvento <= 0) {
        alerta("Seleccione primero un evento", "Error en procedimiento");
        return;
    }
    let url = dir + "consulta/artistasXInstrum?idEvento=" + idEvento + "&idInstrum=" + idInstrum;
    await llenarTablaConsulta(url);
}

function alerta(msj, titulo = "Información") {  // Usa sweetalert2.all.min.js (Scripts_Form/Comunes)
    Swal.fire({
        position: 'center',
        type: 'info',
        title: titulo,
        text: msj
    });
}
