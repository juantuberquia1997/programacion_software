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
        alerta("Agregar", "agregando");
        Agregar();
    });
    $("#btnModi").on("click", function () {
        alerta("Modificar", "modificando");
        Modificar();
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

async function ejecutarComando(accion) {
    //Capturar los datos de entrada
    let _vr1 = $("#txt_vr1").val();
    let _vr2 = $("#cbo_vr2").val();
    let _vr3 = $("#txt_vr3").val();
    let _vr4 = $("#chk_vr4").prop("checked");

    alerta("vr1: " + _vr1 + "..." + ", vr2: " + _vr2 + "..." + ", vr3: " + _vr3 + "..." + ", vr4: " + _vr4);

    if (_vr1 == undefined || _vr1.trim() == "" || parseInt(_vr1, 10) < 0) {
        alerta("xxx no válido", "Error");
        $("#txtCarnet").trigger("focus");
        return;
    }
    if (parseInt(_vr2, 10) <= 0) {
        alerta("xxx no válido", "Error");
        $("#cboPrograma").trigger("focus");
        return;
    }

    //Crear Json 
    let datosOut = {
        campo1: _vr1,
        campo2: _vr2,
        campo3: _vr3,
        campo4: _vr4
    }
    //Invocar el servicio con fetch
    try {
        const response = await fetch(dir + "nombre servicio", {
            method: accion,
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(datosOut),
        });   // stringify() convierte un objeto o valor de JavaScript en una cadena de texto JSON
        const rpta = await response.json();
        if (rpta == -1)
            alerta("Problema al grabar el registro (Revisar si existe)", "Error en proceso");
        else if (rpta == 0)
            alerta("Error al momento del grabar, reintente por favor", "Error en grabar");
        else if (rpta > 0) {
            alerta("Registro grabado con éxito", "Proceso con éxito");
            $("#txt_vr1").val(rpta);
            //llenarTabla();
        }
        else
            alerta(rpta, "Error en procedimiento");
    } catch (error) {
        alerta("Error: " + error, "Error en procedimiento");
    }
}