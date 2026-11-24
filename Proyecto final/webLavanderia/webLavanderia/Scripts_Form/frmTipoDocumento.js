var dir = "http://localhost:5124/api/";
var oTabla = $("#tblDatos").DataTable();

jQuery(function () {
    //Carga el menú
    $("#dvMenu").load("../Paginas/Menu.html");

    //Registrar los botones para responder al evento click
    $("#btnAgre").on("click", function () {
        //alerta("Agregar", "Agregando");
        $("#txtCodigo").val("0");
        ejecutarComando("POST")
    });
    $("#btnModi").on("click", function () {
        //alerta("Modificar", "Modificando");
        let cod = $("#txtCodigo").val();
        if (isNaN(cod) || parseInt(cod, 10) <= 0 || cod.trim() == "") {
            alerta("Error, procedimiento no válido.", "Error, buscar primero un artista");
            return;
        }
        ejecutarComando("PUT")
    });
    $("#btnBusc").on("click", function () {
        //alerta("Buscar", "Buscando");
        Consultar();
    });
    $("#btnCanc").on("click", function () {
        //alerta("Cancelar", "Cancelando");
        Cancelar();
    });
    inicializarTipoDocumento()

});  //Del: jQuery


function alerta(msj, titulo) {  // Instalar en scripts_form/Comunes: sweetalert2.all.min.js y
    Swal.fire({                   //                                   sweetalert2.min.css
        position: 'center',
        type: 'info',
        title: titulo,
        text: msj
    });
}

function validar(name) {
    var msj = "Falta ";
    if (name.trim() == "") {
        msj = msj + "El nombre no válido,\n"
    }
    if (msj != "Falta ") {
        alerta(msj, "Error en procedimiento");
        return false;
    }

    return true;
}

function Cancelar() {
    $("#txtCodigo").val("0");
    $("#txtTipoDoc").val("");
    $("#txtTipoDoc").trigger("focus");
}

async function Consultar() {

    try {

        let texto = $("#txtTipoDoc").val();
        Cancelar();
        $("#txtTipoDoc").val(texto);

        if (!texto || texto.trim() == "") {
            alerta("Error, el tipo doc. no es válido", "Error en procedimiento");
            $("#txtTipoDoc").trigger("focus");
            return;
        }
        let url = dir + "tipoDocumento/consultaTipoDocumento?comando=1&dato=" + texto.trim();

        const datosOut = await fetch(url, {
            method: "GET",
            mode: "cors",
            headers: {
                "content-type": "application/json"
            }
        });


        const datosin = await datosOut.json();

        if (!Array.isArray(datosin) || datosin.length === 0) {
            $("#txtTipoDoc").val(texto);
            alerta("No se encontró tipo de documento: " + texto, "Informacion del proceso");
            return;
        }

        $("#txtCodigo").val(datosin[0].codigo);
        $("#txtTipoDoc").val(datosin[0].nombre);


    } catch (error) {
        alerta("Error: " + error, "Error en consultar");
    }
}

async function ejecutarComando(accion) {
    //Capturar los datos de entrada
    let Codi = $("#txtCodigo").val();
    let Nomb = $("#txtTipoDoc").val();

    if (!validar(Nomb)) {
        $("#txtTipoDoc").trigger("focus");
        return;
    }
    //Confirmar procedimiento
    let rpta = "";
    if (accion === "POST") {
        rpta = window.confirm('Estas seguro de agregar datos de: ' + Nomb + ' ?');
        if (rpta == false) {
            alerta('Cancelada acción de gregar', "Información");
            return;
        }
    }
    else if (accion === "PUT") {
        if (isNaN(Codi) || parseFloat(Codi, 10) <= 0 || Codi.trim() == "") {
            alerta("Código no válido para modificar registro", "Alerta");
            return;
        }
        rpta = window.confirm('Estas seguro de modificar datos de: ' + Nomb + ' ?');
        if (rpta == false) {
            alerta('Cancelada acción de modificar', "Información");
            return;
        }
    }
    else {
        alerta("Procedimiento no válido", "Alerta");
        return;
    }

    //Crear Json 
    let datosOut = {
        IdTipoDocumento: accion === "POST" ? 0 : (parseInt(Codi, 10) || 0),
        NombreDocumento: Nomb ? Nomb.trim() : "",
    }
    //Invocar el servicio con fetch
    try {
        const response = await fetch(dir + "tipoDocumento", {
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
            $("#txtCodigo").val(rpta);
            await Consultar();
            await llenarTabla();
        }
        else
            alerta(rpta, "Error en procedimiento");
    } catch (error) {
        alerta("Error: " + error, "Error en procedimiento");
    }

}

async function inicializarTipoDocumento() {
    await llenarTabla();
}

function registrarEventoIrTipoDocumento() {
    $("#tblDatos tbody").off("click", ".btn-ir").on("click", ".btn-ir", function (evento) {
        evento.preventDefault();
        evento.stopPropagation();

        const fila = $(this).closest("tr");
        $("#tblDatos tbody tr.selected").removeClass("selected");
        fila.addClass("selected");

        const datosFila = $("#tblDatos").DataTable().row(fila).data();
        if (!datosFila) {
            alerta("No se pudieron leer los datos de la fila", "Error en procedimiento");
            return;
        }

        $("#txtCodigo").val(datosFila.codigo ?? "");
        $("#txtTipoDoc").val(datosFila.nombre ?? "");
    });
}

async function llenarTabla() {
    const url = dir + "tipoDocumento/listadoTipoDocumentos";

    try {
        const respuesta = await fetch(url, {
            method: "GET",
            mode: "cors",
            headers: {
                "content-type": "application/json"
            }
        });

        const datos = respuesta.ok ? await respuesta.json() : [];

        const filasFormateadas = Array.isArray(datos)
            ? datos.map(item => ({
                codigo: item.idTipoDocumento ?? item.codigo ?? "",
                nombre: item.nombreDocumento ?? item.nombre ?? ""
            }))
            : [];

        $("#tblDatos").DataTable({
            data: filasFormateadas,
            destroy: true,
            columns: [
                {
                    data: null,
                    title: "Editar",
                    orderable: false,
                    searchable: false,
                    defaultContent: '<button type="button" class="btn btn-info btn-sm btn-ir">ir</button>'
                },
                { data: "codigo", title: "Código" },
                { data: "nombre", title: "Tipo Documento" }
            ]
        });

        registrarEventoIrTipoDocumento();
    } catch (error) {
        console.error("Error en llenarTabla:", error);
        alerta("Error en llenado de tabla", "Error en procedimiento");
    }
}