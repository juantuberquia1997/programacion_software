var dir = "http://localhost:5124/api/";
var oTabla = null;

jQuery(function () {
    //Carga el menú
    $("#dvMenu").load("../Paginas/Menu.html");

    //Registrar los botones para responder al evento click
    $("#btnAgre").on("click", function () {
        //alerta("Agregar", "Agregando");
        $("#txtCodigo").val("0");
        ejecutarComando("POST");
    });

    $("#btnModi").on("click", function () {
        //alerta("Modificar", "Modificando");
        let cod = $("#txtCodigo").val();
        if (isNaN(cod) || parseInt(cod, 10) <= 0 || cod.trim() == "") {
            alerta("Error, procedimiento no válido.", "Debe buscar o seleccionar primero un teléfono");
            return;
        }
        ejecutarComando("PUT");
    });

    $("#btnBusc").on("click", function () {
        //alerta("Buscar", "Buscando");
        Consultar();
    });

    $("#btnCanc").on("click", function () {
        //alerta("Cancelar", "Cancelando");
        Cancelar();
    });

    inicializarFormulario();
}); //Del: jQuery

async function inicializarFormulario() {
    await llenarComboClientes();
    await llenarComboTipoTelefono();
    await llenarTabla();
}

function alerta(msj, titulo) {  // Instalar en scripts_form/Comunes: sweetalert2.all.min.js y
    Swal.fire({                   //                                   sweetalert2.min.css
        position: 'center',
        type: 'info',
        title: titulo,
        text: msj
    });
}

function validar(telefono, idCliente) {
    var msj = "Falta ";
    if (!telefono || telefono.trim() == "") {
        msj = msj + "El número de teléfono no es válido,\n";
    }
    if (!idCliente || isNaN(parseInt(idCliente, 10)) || parseInt(idCliente, 10) <= 0) {
        msj = msj + "Seleccionar un cliente válido,\n";
    }
    if (msj != "Falta ") {
        alerta(msj, "Error en procedimiento");
        return false;
    }
    return true;
}

function Cancelar() {
    $("#txtCodigo").val("0");
    $("#cboCliente")[0].options[0].selected = true;
    $("#cboTipoTel")[0].options[0].selected = true;
    $("#txtNumeroTel").val("");
    $("#cboCliente").trigger("focus");
}

async function llenarComboClientes() {
    let url = dir + "cliente/listadoClientes";
    let rpta = await llenarComboGral(url, "#cboCliente", "idCliente", "nombreApellidoCliente");

    if (rpta != "Termino") {
        alerta("Error llenando combo de cliente!", "Error en procedimiento");
        return false;
    }
    return true;
}

async function llenarComboTipoTelefono() {
    let url = dir + "tipoTelefono/listadoTipoTelefonos";
    let rpta = await llenarComboGral(url, "#cboTipoTel", "idTipoTelefono", "tipoTelefono");

    if (rpta != "Termino") {
        alerta("Error llenando combo de tipo de teléfono!", "Error en procedimiento");
        return false;
    }
    return true;
}

function registrarEventoIrTelefono() {
    $("#tblDatos tbody").off("click", ".btn-ir").on("click", ".btn-ir", function (evento) {
        evento.preventDefault();
        evento.stopPropagation();

        const fila = $(this).closest("tr");
        $("#tblDatos tbody tr.selected").removeClass("selected");
        fila.addClass("selected");

        const datosFila = oTabla ? oTabla.row(fila).data() : $("#tblDatos").DataTable().row(fila).data();
        if (!datosFila) {
            alerta("No se pudieron leer los datos de la fila", "Error en procedimiento");
            return;
        }

        $("#txtCodigo").val(datosFila.codigo ?? "");
        $("#txtNumeroTel").val(datosFila.telefono ?? "");

        if (datosFila.idCliente !== undefined && datosFila.idCliente !== null && datosFila.idCliente !== "") {
            $("#cboCliente").val(String(datosFila.idCliente));
        }

        if (datosFila.idTipoTelefono !== undefined && datosFila.idTipoTelefono !== null && datosFila.idTipoTelefono !== "") {
            $("#cboTipoTel").val(String(datosFila.idTipoTelefono));
        }

        let idCliFila = $("#cboCliente").val();
        if (idCliFila && !isNaN(parseInt(idCliFila, 10)) && parseInt(idCliFila, 10) > 0) {
           Consultar();
        }
    });
}

async function Consultar() {
    try {
        let idCli = $("#cboCliente").val();
        let telActual = $("#txtNumeroTel").val();

        if (!idCli || isNaN(parseInt(idCli, 10)) || parseInt(idCli, 10) <= 0) {
            alerta("Debe seleccionar un cliente válido para buscar", "Error en procedimiento");
            $("#cboCliente").trigger("focus");
            return;
        }

        let url = dir + "cliente/listadoTelXCliente?codCliente=" + encodeURIComponent(idCli.trim());

        const resp = await fetch(url, {
            method: "GET",
            mode: "cors",
            headers: {
                "content-type": "application/json"
            }
        });

        const datosin = await resp.json();

        if (!Array.isArray(datosin) || datosin.length === 0) {
            alerta("No se encontraron teléfonos para el cliente seleccionado", "Informacion del proceso");
            await llenarTabla();
            return;
        }

        const primerItem = datosin[0];
        if (typeof primerItem === "string" || typeof primerItem === "number") {
            const coincidencia = telActual && telActual.trim() !== ""
                ? (datosin.find(t => String(t).includes(telActual.trim())) ?? primerItem)
                : primerItem;

            $("#cboCliente").val(String(idCli));
            $("#txtNumeroTel").val(String(coincidencia));
        } else {
            const reg = telActual && telActual.trim() !== ""
                ? (datosin.find(x => String(x.numero ?? x.telefono ?? x.numeroTelefono ?? "").includes(telActual.trim())) || primerItem)
                : primerItem;

            $("#txtCodigo").val(reg.idTel ?? reg.idTelefonoCliente ?? reg.codigo ?? "");
            $("#cboCliente").val(String(reg.idCliente ?? reg.clienteIdCliente ?? idCli));
            $("#cboTipoTel").val(String(reg.idTipoTel ?? reg.idTipoTelefono ?? ""));
            $("#txtNumeroTel").val(reg.numero ?? reg.telefono ?? reg.numeroTelefono ?? "");
        }

        await llenarTabla();
    } catch (error) {
        alerta("Error: " + error, "Error en consultar");
    }
}

async function ejecutarComando(accion) {
    let codi = $("#txtCodigo").val();
    let idCli = $("#cboCliente").val();
    let idTipo = $("#cboTipoTel").val();
    let telefono = $("#txtNumeroTel").val();

    let comp = "cliente/modificarTelefono"

    if (accion === "POST") {
        codi = "0";
        $("#txtCodigo").val("0");
        comp = "cliente/agregarTelefono"
    }

    if (!validar(telefono, idCli)) {
        return;
    }

    if (accion === "PUT" && (!codi || isNaN(codi) || parseInt(codi, 10) <= 0 || codi.trim() === "")) {
        alerta("Código no válido para modificar registro", "Alerta");
        return;
    }

    const textoAccion = accion === "POST" ? "agregar" : "modificar";
    const confirma = window.confirm(`¿Estás seguro de ${textoAccion} el teléfono ${telefono.trim()}?`);
    if (!confirma) {
        alerta(`Cancelada acción de ${textoAccion}`, "Información");
        return;
    }

    // Revisa en Swagger los nombres exactos del Request body del endpoint de teléfono de cliente
    const datosOut = {
        idTelefono: parseInt(codi, 10),
        idCliente: parseInt(idCli, 10),
        idTipoTelefono: idTipo && !isNaN(parseInt(idTipo, 10)) ? parseInt(idTipo, 10) : 0,
        numero: telefono.trim()
    };

    try {
        const response = await fetch(dir + comp, {
            method: accion,
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(datosOut)
        });

        const textoRpta = await response.text();
        let rpta = null;
        if (textoRpta && textoRpta.trim() !== "") {
            rpta = JSON.parse(textoRpta);
        } else if (response.ok) {
            rpta = codi;
        }

        if (rpta == -1) {
            alerta("Problema al grabar el registro (Revisar si existe)", "Error en proceso");
        } else if (rpta == 0) {
            alerta("Error al momento del grabar, reintente por favor", "Error en grabar");
        } else if (rpta > 0) {
            alerta("Registro grabado con éxito", "Proceso con éxito");
            $("#txtCodigo").val(rpta);
            $("#txtNumeroTel").val(telefono.trim());
            await llenarTabla();
        } else {
            const msj = typeof rpta === "object" && rpta !== null ? JSON.stringify(rpta) : String(rpta ?? "");
            alerta(msj, "Error en procedimiento");
        }
    } catch (error) {
        alerta("Error: " + error, "Error en procedimiento");
    }
}

async function llenarTabla() {
    let idCli = $("#cboCliente").val();
    let filasFormateadas = [];

    if (idCli && !isNaN(parseInt(idCli, 10)) && parseInt(idCli, 10) > 0) {
        const url = dir + "cliente/listadoTelXCliente?codCliente=" + idCli.trim();

        try {
            const respuesta = await fetch(url, {
                method: "GET",
                mode: "cors",
                headers: {
                    "content-type": "application/json"
                }
            });

            const crudo = respuesta.ok ? await respuesta.json() : null;
            const datos = Array.isArray(crudo)
                ? crudo
                : (crudo?.data ?? crudo?.datos ?? crudo?.resultado ?? []);

            const nombreCliActual = $("#cboCliente option:selected").text().trim() || idCli;
            const idTipoActual = $("#cboTipoTel").val() ?? "";
            const nombreTipoActual = $("#cboTipoTel option:selected").text().trim() || "";

            filasFormateadas = Array.isArray(datos)
                ? datos.map((item, idx) => {
                    if (typeof item === "string" || typeof item === "number") {
                        return {
                            codigo: idx + 1,
                            idCliente: idCli,
                            cliente: nombreCliActual,
                            idTipoTelefono: idTipoActual,
                            tipoTelefono: nombreTipoActual,
                            telefono: String(item)
                        };
                    }

                    const idClienteItem = item.idCliente ?? item.clienteIdCliente ?? idCli;
                    const idTipoItem = item.idTipoTelefono ?? item.idTipoTel ?? "";
                    const opCli = $(`#cboCliente option[value="${idClienteItem}"]`);
                    const opTipo = $(`#cboTipoTel option[value="${idTipoItem}"]`);

                    return {
                        codigo: item.idTel ?? item.idTelefonoCliente ?? item.codigo ?? (idx + 1),
                        idCliente: idClienteItem,
                        cliente: opCli.length > 0 ? opCli.text().trim() : (item.nombreCliente ?? idClienteItem),
                        idTipoTelefono: idTipoItem,
                        tipoTelefono: item.tipoTel ?? (opTipo.length > 0 ? opTipo.text().trim() : (item.tipoTelefono ?? idTipoItem)),
                        telefono: item.numero ?? item.telefono ?? item.numeroTelefono ?? ""
                    };
                })
                : [];
        } catch (error) {
            console.error("Error en llenarTabla:", error);
            alerta("Error en llenado de tabla", "Error en procedimiento");
        }
    }

    if ($.fn.DataTable.isDataTable("#tblDatos")) {
        $("#tblDatos").DataTable().clear().destroy();
    }

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
            { data: "cliente", title: "Cliente" },
            { data: "tipoTelefono", title: "Tipo Teléfono" },
            { data: "telefono", title: "Teléfono" }
        ]
    });

    registrarEventoIrTelefono();
}