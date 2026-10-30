var dir = "http://localhost:5114/api/";

// Funciones para el llenado de los combos del formulario
async function llenarComboGenero( ) {
    let url = dir + "genero/listaGeneros";  // Alias del servicio en generoController: [HttpGet("listaGeneros")]
    let rpta = await llenarComboGral( url, "#cboGenero" );  // Invoca el servicio y el método con el alias
    if ( rpta != "Termino" ) {
        alerta("error en llenado combo género", "Error en procedimiento");
        return;
    }
}

async function llenarComboTipDoc( ) {
    let url = dir + "tipoDoc/listadoTipDocs";
    let rpta = await llenarComboGral(url, "#cboTipDoc");  // Invoca el servicio y el método con el alias
    if ( rpta != "Termino" ) {
        alerta("error en llenado combo tipo documento", "Error en procedimiento");
        return;
    }
}

async function llenarComboDpto( ) {
    let url = dir + "departamento/listadoDptos";
    let rpta = await llenarComboGral(url, "#cboDpto" );  // Invoca el servicio y el método con el alias

    if ( rpta != "Termino" ) {
        alerta("error en llenado combo departamento", "Error en procedimiento");
        return;
    }
    // Llenado anidado del combo de ciudades
    llenarComboCiudad( );
}

async function llenarComboCiudad( idCiu ) {
    let idDpto = $("#cboDpto").val();
    let url = dir + "ciudad/listadoCiudsXDpto?codDpto=" + idDpto;
    let rpta = await llenarComboGral( url, "#cboCiudad" );
    if ( rpta != "Termino" ) {
        alerta("Error en llenado combo ciudad", "Error en procedimiento" );
        return;
    }
    else if ( idCiu != undefined ) {
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
            alerta("Error, procedimiento no válido.", "Error, Buscar primero un artista");
            return;
        }
        ejecutarComando("PUT");
    });
    $("#btnBusc").on("click", function () {
        alerta("Buscar");
        ConsultarXDocument();
    });
    $("#btnCanc").on("click", function () {
        alerta("Cancelar");
        Cancelar();
    });
    $("#btnImpr").on("click", function () {
        alerta("Impresión", "En futura construcción");
        //Imprimir();
    });

    //Llenado de los combos
    llenarComboGenero();
    llenarComboTipDoc();
    llenarComboDpto();
    llenarTabla();

    //Listener para la tabla (datatable): evento click sobre un registro (tr) de la tabla
    //Se delega desde #tablaDatos porque el tbody lo crea DataTables después (asíncrono)


    if ($("#tablaDatos tbody tr")) {
        $("#tablaDatos").on("click", "tbody tr", function (evento) {
            evento.preventDefault();  // Permite al desarrollador tomar el control del comportamiento del evento

            if ($(this).hasClass("selected")) {
                $(this).removeClass("selected");
            } else {
                $("#tablaDatos tbody tr.selected").removeClass("selected");
                $(this).addClass("selected");
                editarFila($(this).closest("tr"));
            }
        });
    }

});  //Del: jQuery

async function llenarTabla() {
    let url = dir + "artista/consultarArtista?comando=2&dato=0";
    let rpta = await llenarTablaGral(url, "#tablaDatos");
    if (rpta != "Termino") {
        alerta("Error en llenado de tabla", "Error en procedimiento");
    }
}

//Buscar desde la tabla: consulta y llenado de los objetos del formulario
//con los datos del artista, acorde a la fila seleccionada
function editarFila(datosFila) {
    let nroDoc = datosFila.find("td:eq(4)").text();  // Columna 4 (td:eq(4)): nro_Doc
    if (nroDoc > 0) {
        $("#txtNroDoc").val(nroDoc);
        ConsultarXDocument();
        alerta("Ok", "Procedimiento con éxito");
        return;
    }
    alerta("Sin registros", "Procedimiento incompleto");
}

function Cancelar() {

    $("#txtCodigo").val("0");
    $("#txtNombre").val("");
    $("#cboGenero")[0].options[0].selected = true;
    $("#cboTipDoc")[0].options[0].selected = true;
    $("#txtNroDoc").val("");
    $("#cboDpto")[0].options[0].selected = true;
    llenarComboCiudad();
    $("#chkActivo").prop("checked", true);
    $("#txtNombre").trigger("focus");

}

async function ConsultarXDocument() {
    try {
        let nroDoc = $("#txtNroDoc").val();
        Cancelar();
        $("#txtNroDoc").val(nroDoc);
        if (isNaN(nroDoc) || parseFloat(nroDoc, 10) <= 0 || nroDoc.trim() == "") {
            alerta("Error, el nro. del documento no es válido", "Error en procedimiento");
            $("#txtNroDoc").trigger("focus");
            return;
        }
        let url = dir + "artista/consultarArtista?comando=1&dato=" + nroDoc;
        const datosOut = await fetch(url, {
            method: "GET",
            mode: "cors",
            headers: {
                "content-type": "application/json",
            }
        });
        const datosIn = await datosOut.json();
        if (datosIn == "") {
            alerta("No se encontró artista con nro. doc.: " + nroDoc, "Información del proceso");
            return;
        }
        $("#txtCodigo").val(datosIn[0].codigo);
        $("#txtNombre").val(datosIn[0].nombre);
        $("#cboGenero").val(datosIn[0].idG);
        $("#cboTipDoc").val(datosIn[0].idTD);
        $("#cboDpto").val(datosIn[0].idDep);
        // Llenar Combo Ciudad y seleccionar ciudad
        await llenarComboCiudad(datosIn[0].idC);
        $("#chkActivo").prop("checked", datosIn[0].act);
    } catch (error) {
        alerta("Error: " + error, "Error al consultar", error.status);
    }
}


function alerta(msj, titulo = "Información") {  // Instalar en scripts_form/Comunes: sweetalert2.all.min.js y
    Swal.fire({                   //                                   sweetalert2.min.css
        position: 'center',
        type: 'info',
        title: titulo,
        text: msj
    });
}

//Validar la información de entrada: nombre y nro. de documento del artista
function validar(name, nroDoc) {
    var msj = "Falta ";
    if (name.trim() == "") {
        msj = msj + "El nombre no válido, \n";
    }
    if (isNaN(nroDoc) || parseFloat(nroDoc, 10) <= 0 || nroDoc.trim() == "") {
        msj = msj + "el nro. de documento no válido. \n";
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
    let idGe = $("#cboGenero").val();
    let idTD = $("#cboTipDoc").val();
    let nroD = $("#txtNroDoc").val();
    let idCi = $("#cboCiudad").val();
    let acti = $("#chkActivo").prop("checked");

    alerta("Codi: " + Codi + ", Nomb: " + Nomb + ", idGe: " + idGe + ", idTD: " + idTD + ", nroD: " + nroD + ", idCi: " + idCi + ", activo: " + acti, "Datos obtenidos");

    //Validar información
    if (!validar(Nomb, nroD)) {
        $("#txtNombre").trigger("focus");
        return;
    }

    //Confirmar Procedimiento
    let rpta = "";
    if (accion === "POST") {
        /*if (isNaN(Codi) || parseInt(Codi, 10) != 0 || Codi.trim() == "") {
            alerta("Código no válido para registro nuevo", "Alerta");
            return;
        }*/
        rpta = window.confirm('Estas seguro de agregar datos de: ' + Nomb + ', con nro. Doc.: ' + nroD + '?');
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
        rpta = window.confirm('Estas seguro de modificar datos de: ' + Nomb + ', con nro. Doc.: ' + nroD + '?');
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
        codigo: Codi,
        nombre: Nomb,
        idtipoDoc: idTD,
        nroDoc: nroD,
        idGenero: idGe,
        idCiudad: idCi,
        activo: acti
    }

    //Invocar el servicio con fetch
    try {   // fetch se utiliza para realizar solicitudes HTTP a recursos de red
        const response = await fetch(dir + "artista", {
            method: accion,
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(datosOut),
        });  // stringify() convierte un objeto o valor de JavaScript en una cadena de texto JSON

        const rpta = await response.json();
        if (rpta == -1)
            alerta("Problema al grabar el registro (Revisar si existe)", "Error en proceso");
        else if (rpta == 0)
            alerta("Error al momento del grabar, reintente por favor", "Error en grabar");
        else if (rpta > 0) {
            alerta("Registro grabado con éxito", "Proceso con éxito");
            $("#txtCodigo").val(rpta);  // Clave primaria del registro grabado
            await ConsultarXDocument();
            await llenarTabla();
        }
        else
            alerta(rpta, "Error en procedimiento");
    } catch (error) {
        alerta("Error: " + error, "Error en procedimiento");
    }
}