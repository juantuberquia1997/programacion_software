var dir = "http://localhost:5114/api/";

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
            alerta("Error, procedimiento no válido.", "Error, Buscar primero un departamento");
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
    let url = dir + "departamento/consultaDpto?comando=2&dato=0";
    let rpta = await llenarTablaGral(url, "#tablaDatos");
    if (rpta != "Termino") {
        alerta("Error en llenado de tabla", "Error en procedimiento");
    }
}

//Buscar desde la tabla: consulta por el nombre del departamento de la fila seleccionada
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
    $("#txtNombre").trigger("focus");
}

//Consultar o buscar un departamento por su nombre
async function Consultar() {
    try {
        let nombre = $("#txtNombre").val();
        if (nombre.trim() == "") {
            alerta("Error, el nombre no es válido", "Error en procedimiento");
            $("#txtNombre").trigger("focus");
            return;
        }
        let url = dir + "departamento/consultaDpto?comando=1&dato=" + encodeURIComponent(nombre);
        const datosOut = await fetch(url, {
            method: "GET",
            mode: "cors",
            headers: {
                "content-type": "application/json",
            }
        });
        const datosIn = await datosOut.json();
        if (datosIn == "") {
            alerta("No se encontró departamento con nombre: " + nombre, "Información del proceso");
            return;
        }
        $("#txtCodigo").val(datosIn[0].codigo);
        $("#txtNombre").val(datosIn[0].nombre);
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

//Validar la información de entrada: nombre del departamento
function validar(nombre) {
    var msj = "Falta ";
    if (nombre.trim() == "") {
        msj = msj + "El nombre no válido, \n";
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

    //Validar información
    if (!validar(Nomb)) {
        $("#txtNombre").trigger("focus");
        return;
    }

    //Confirmar Procedimiento
    let rpta = "";
    if (accion === "POST") {
        rpta = window.confirm('¿Estás seguro de agregar el departamento: ' + Nomb + '?');
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
        rpta = window.confirm('¿Estás seguro de modificar el departamento: ' + Nomb + '?');
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
        nombre: Nomb
    }

    //Invocar el servicio con fetch
    try {
        const response = await fetch(dir + "departamento", {
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
