var dir = "http://localhost:5124/api/";
var oTabla;

jQuery(function () {
    $("#dvMenu").load("../Paginas/Menu.html");

    $("#btnAgre").on("click", function () {
        $("#txtCodigo").val("0");
        ejecutarComando("POST");
    });

    $("#btnModi").on("click", function () {
        let cod = $("#txtCodigo").val();
        if (isNaN(cod) || parseInt(cod, 10) <= 0 || cod.trim() === "") {
            alerta("Error, procedimiento no válido.", "Error, buscar primero un cliente");
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

    inicializarCliente();
});

function alerta(msj, titulo) {
    Swal.fire({
        position: "center",
        icon: "info",
        title: titulo,
        text: msj
    });
}

function validar(name, nroDoc, empleado, tipoDoc) {
    var msj = "Falta ";
    if (!name || name.trim() === "") {
        msj = msj + "El nombre no válido,\n";
    }
    if (!nroDoc || nroDoc.trim() === "") {
        msj = msj + "el nro. de documento no es válido,\n";
    }
    if (!empleado || isNaN(parseInt(empleado, 10)) || parseInt(empleado, 10) <= 0) {
        msj = msj + "seleccionar un empleado válido,\n";
    }
    if (!tipoDoc || isNaN(parseInt(tipoDoc, 10)) || parseInt(tipoDoc, 10) <= 0) {
        msj = msj + "seleccionar un tipo de documento válido,\n";
    }
    if (msj !== "Falta ") {
        alerta(msj, "Error en procedimiento");
        return false;
    }
    return true;
}

async function inicializarCliente() {
    await llenarComboEmpleado();
    await llenarComboTipoDoc();
    await llenarTabla();
}

function seleccionarComboPorTexto(selectorCombo, texto) {
    if (!texto) return;
    const objetivo = texto.toString().trim().toLowerCase();
    $(`${selectorCombo} option`).each(function () {
        if ($(this).text().trim().toLowerCase() === objetivo) {
            $(selectorCombo).val($(this).val()).trigger("change");
            return false;
        }
    });
}

function registrarEventoIrCliente() {
    $("#tblDatos tbody").off("click", ".btn-ir").on("click", ".btn-ir", function (evento) {
        evento.preventDefault();
        evento.stopPropagation();

        const fila = $(this).closest("tr");
        $("#tblDatos tbody tr.selected").removeClass("selected");
        fila.addClass("selected");

        const datosFila = $("#tblDatos").DataTable().row(fila).data();
        if (!datosFila) return;

        $("#txtCodigo").val(datosFila.codigo ?? "");
        $("#txtNombre").val(datosFila.nombre ?? "");
        $("#txtNroDoc").val(datosFila.documento ?? "");
        seleccionarComboPorTexto("#cboTipoDoc", datosFila.tipoDocumento);

        if (datosFila.documento && datosFila.documento.toString().trim() !== "") {
            Consultar();
        }
    });
}

async function llenarComboTipoDoc() {
    let url = dir + "tipoDocumento/listadoTipoDocumentos";
    let rpta = await llenarComboGral(url, "#cboTipoDoc", "idTipoDocumento", "nombreDocumento");

    if (rpta !== "Termino") {
        alerta("Error llenando combo de tipo de documento!", "Error en procedimiento");
    }
}

async function llenarComboEmpleado() {
    let url = dir + "empleado/listadoEmpleados";
    let rpta = await llenarComboGral(url, "#cboEmpleado", "idEmpleado", "nombreApellidoEmp");

    if (rpta !== "Termino") {
        alerta("Error llenando combo de empleados!", "Error en procedimiento");
    }
}

async function ejecutarComando(accion) {
    let Codi = $("#txtCodigo").val();
    let Empl = $("#cboEmpleado").val();
    let Nomb = $("#txtNombre").val();
    let idTD = $("#cboTipoDoc").val();
    let nroD = $("#txtNroDoc").val();

    if (!validar(Nomb, nroD, Empl, idTD)) {
        $("#txtNombre").trigger("focus");
        return;
    }

    let rpta = false;
    if (accion === "POST") {
        rpta = window.confirm("¿Estás seguro de agregar datos de: " + Nomb + ", con Nro. Doc.: " + nroD + "?");
        if (!rpta) {
            alerta("Cancelada acción de agregar", "Información");
            return;
        }
    } else if (accion === "PUT") {
        if (isNaN(Codi) || parseFloat(Codi, 10) <= 0 || Codi.trim() === "") {
            alerta("Código no válido para modificar registro", "Alerta");
            return;
        }
        rpta = window.confirm("¿Estás seguro de modificar datos de: " + Nomb + ", con Nro. Doc.: " + nroD + "?");
        if (!rpta) {
            alerta("Cancelada acción de modificar", "Información");
            return;
        }
    } else {
        alerta("Procedimiento no válido", "Alerta");
        return;
    }

    let datosOut = {
        tblCliente: {
            idCliente: accion === "POST" ? 0 : parseInt(Codi, 10),
            empleadoIdEmpleado: parseInt(Empl, 10),
            idTipoDocumento: parseInt(idTD, 10),
            nombreApellidoCliente: Nomb.trim(),
            numeroDocumento: nroD.trim()
        },
        direcciones: [],
        telefonos: []
    };

    try {
        const response = await fetch(dir + "cliente", {
            method: accion,
            mode: "cors",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(datosOut)
        });

        const resultado = await response.json();

        if (resultado === -1) {
            alerta("Problema al grabar el registro (Revisar si existe)", "Error en proceso");
        } else if (resultado === 0) {
            alerta("Error al momento de grabar, reintente por favor", "Error en grabar");
        } else if (resultado > 0) {
            // Conserva nroD para que Consultar() busque por documento y no por el ID devuelto
            $("#txtCodigo").val(resultado);
            $("#txtNroDoc").val(nroD.trim());
            await Consultar();
            await llenarTabla();
            alerta("Registro grabado con éxito", "Proceso con éxito");
        } else {
            alerta(resultado, "Error en procedimiento");
        }
    } catch (error) {
        alerta("Error: " + error, "Error en procedimiento");
    }
}

function Cancelar() {
    $("#txtCodigo").val("0");
    if ($("#cboEmpleado")[0] && $("#cboEmpleado")[0].options.length > 0) {
        $("#cboEmpleado")[0].options[0].selected = true;
    }
    $("#txtNombre").val("");
    if ($("#cboTipoDoc")[0] && $("#cboTipoDoc")[0].options.length > 0) {
        $("#cboTipoDoc")[0].options[0].selected = true;
    }
    $("#txtNroDoc").val("");
    $("#txtNombre").trigger("focus");
}

async function Consultar() {
    try {
        let nroDoc = $("#txtNroDoc").val();

        if (!nroDoc || nroDoc.trim() === "") {
            alerta("Error, el nro. del documento no es válido", "Error en procedimiento");
            $("#txtNroDoc").trigger("focus");
            return;
        }

        let url = dir + "cliente/consultaCliente?comando=1&dato=" + encodeURIComponent(nroDoc.trim());

        const response = await fetch(url, {
            method: "GET",
            mode: "cors",
            headers: {
                "Content-Type": "application/json"
            }
        });

        const datosin = await response.json();

        if (!Array.isArray(datosin) || datosin.length === 0) {
            alerta("No se encontró cliente con nro. doc.: " + nroDoc.trim(), "Información del proceso");
            return;
        }

        $("#txtCodigo").val(datosin[0].codigo);
        $("#cboEmpleado").val(datosin[0].empleado);
        $("#txtNombre").val(datosin[0].nombre);
        $("#cboTipoDoc").val(datosin[0].idTD);
        $("#txtNroDoc").val(datosin[0].documento);
    } catch (error) {
        alerta("Error: " + error, "Error en consultar");
    }
}

async function llenarTabla() {
    const url = dir + "cliente/consultaCliente?comando=2&dato=0";

    try {
        const respuesta = await fetch(url, {
            method: "GET",
            mode: "cors",
            headers: {
                "Content-Type": "application/json"
            }
        });

        const datos = await respuesta.json();

        const filasFormateadas = Array.isArray(datos)
            ? datos.map(item => ({
                codigo: item.codigo,
                nombre: item.nombre,
                tipoDocumento: item.tipoDoc ?? "",
                documento: item.documento
            }))
            : [];

        if ($.fn.DataTable.isDataTable("#tblDatos")) {
            $("#tblDatos").DataTable().clear().rows.add(filasFormateadas).draw();
        } else {
            oTabla = $("#tblDatos").DataTable({
                data: filasFormateadas,
                columns: [
                    {
                        data: null,
                        title: "Editar",
                        orderable: false,
                        searchable: false,
                        defaultContent: '<button type="button" class="btn btn-info btn-sm btn-ir">ir</button>'
                    },
                    { data: "codigo", title: "Código" },
                    { data: "nombre", title: "Nombre" },
                    { data: "tipoDocumento", title: "Tipo Documento" },
                    { data: "documento", title: "Nro. Documento" }
                ]
            });
        }

        registrarEventoIrCliente();
    } catch (error) {
        console.error("Error en llenarTabla:", error);
        alerta("Error en llenado de tabla", "Error en procedimiento");
    }
}