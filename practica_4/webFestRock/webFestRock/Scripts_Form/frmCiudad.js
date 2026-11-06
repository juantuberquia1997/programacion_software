var dir = "http://localhost:5114/api/";

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
            alerta("Error, procedimiento no válido.", "Error, Buscar primero una ciudad");
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

    //Llenado de los combos
    llenarComboDpto();

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
    let url = dir + "ciudad/consultaCiudad?comando=2&dato=0";
    let rpta = await llenarTablaGral(url, "#tablaDatos");
    if (rpta != "Termino") {
        alerta("Error en llenado de tabla", "Error en procedimiento");
    }
}

//Buscar desde la tabla: consulta por el nombre de la ciudad de la fila seleccionada
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
    $("#cboDpto")[0].options[0].selected = true;
    llenarComboCiudad();
    $("#txtNombre").trigger("focus");
}

//Consultar o buscar una ciudad por su nombre
async function Consultar() {
    try {
        let nombre = $("#txtNombre").val();
        if (nombre.trim() == "") {
            alerta("Error, el nombre no es válido", "Error en procedimiento");
            $("#txtNombre").trigger("focus");
            return;
        }
        let url = dir + "ciudad/consultaCiudad?comando=1&dato=" + encodeURIComponent(nombre);
        const datosOut = await fetch(url, {
            method: "GET",
            mode: "cors",
            headers: {
                "content-type": "application/json",
            }
        });
        const datosIn = await datosOut.json();
        if (datosIn == "") {
            alerta("No se encontró ciudad con nombre: " + nombre, "Información del proceso");
            return;
        }
        $("#txtCodigo").val(datosIn[0].codigo);
        $("#txtNombre").val(datosIn[0].nombre);
        // Seleccionar el departamento y llenar el combo de ciudades de ese dpto.
        $("#cboDpto").val(datosIn[0].idDep);
        await llenarComboCiudad(datosIn[0].codigo);
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

//Validar la información de entrada: nombre y departamento de la ciudad
function validar(nombre, idDpto) {
    var msj = "Falta ";
    if (nombre.trim() == "") {
        msj = msj + "El nombre no válido, \n";
    }
    if (idDpto == null || idDpto <= 0) {
        msj = msj + "el departamento no válido. \n";
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
    let idDe = $("#cboDpto").val();

    //Validar información
    if (!validar(Nomb, idDe)) {
        $("#txtNombre").trigger("focus");
        return;
    }

    //Confirmar Procedimiento
    let rpta = "";
    if (accion === "POST") {
        rpta = window.confirm('¿Estás seguro de agregar la ciudad: ' + Nomb + '?');
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
        rpta = window.confirm('¿Estás seguro de modificar la ciudad: ' + Nomb + '?');
        if (rpta == false) {
            alerta('Cancelada acción de Modificar', 'Información');
            return;
        }
    }
    else {
        alerta("Procedimiento no válido", "Error");
        return;
    }

    //Crear JSon
    let datosOut = {
        codigo: parseInt(Codi, 10) || 0,
        nombre: Nomb,
        idDpto: parseInt(idDe, 10)
    }

    //Invocar el servicio con fetch
    try {
        const response = await fetch(dir + "ciudad", {
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
            await llenarTabla();
        }
        else
            alerta(rpta, "Error en procedimiento");
    } catch (error) {
        alerta("Error: " + error, "Error en procedimiento");
    }
}
