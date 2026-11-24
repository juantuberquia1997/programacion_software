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
            alerta("Error, procedimiento no válido.", "Debe buscar o seleccionar primero un teléfono de empleado");
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

    $("#cboEmpleado").on("change", async function () {
        await llenarTabla();
    });

    inicializarFormulario();
}); //Del: jQuery

async function inicializarFormulario() {
    await llenarComboEmpleados();
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

function validar(telefono, idEmpleado) {
    var msj = "Falta ";
    if (!telefono || telefono.trim() == "") {
        msj = msj + "El número de teléfono no es válido,\n";
    }
    if (!idEmpleado || isNaN(parseInt(idEmpleado, 10)) || parseInt(idEmpleado, 10) <= 0) {
        msj = msj + "Seleccionar un empleado válido,\n";
    }
    if (msj != "Falta ") {
        alerta(msj, "Error en procedimiento");
        return false;
    }
    return true;
}

function Cancelar() {
    $("#txtCodigo").val("0");
    $("#cboEmpleado")[0].options[0].selected = true;
    $("#cboTipoTel")[0].options[0].selected = true;
    $("#txtNumeroTel").val("");
    $("#cboEmpleado").trigger("focus");
}

async function llenarComboEmpleados() {
    let url = dir + "empleado/listadoEmpleados";
    let rpta = await llenarComboGral(url, "#cboEmpleado", "idEmpleado", "nombreApellidoEmp");

    if (rpta != "Termino") {
        alerta("Error llenando combo de empleados!", "Error en procedimiento");
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

function registrarEventoIrTelefonoEmp() {
    $("#tblDatos tbody").off("click", ".btn-ir").on("click", ".btn-ir", async function (evento) {
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

        if (datosFila.idEmpleado !== undefined && datosFila.idEmpleado !== null && datosFila.idEmpleado !== "") {
            $("#cboEmpleado").val(String(datosFila.idEmpleado));
        }

        if (datosFila.idTipoTelefono !== undefined && datosFila.idTipoTelefono !== null && datosFila.idTipoTelefono !== "") {
            $("#cboTipoTel").val(String(datosFila.idTipoTelefono));
        }

        const idEmpFila = $("#cboEmpleado").val();
        if (idEmpFila && !isNaN(parseInt(idEmpFila, 10)) && parseInt(idEmpFila, 10) > 0) {
            await Consultar();
        }
    });
}

async function Consultar() {
    try {
        let idEmp = $("#cboEmpleado").val();
        let telActual = $("#txtNumeroTel").val();

        if (!idEmp || isNaN(parseInt(idEmp, 10)) || parseInt(idEmp, 10) <= 0) {
            alerta("Debe seleccionar un empleado válido para buscar", "Error en procedimiento");
            $("#cboEmpleado").trigger("focus");
            return;
        }

        let url = dir + "empleado/listadoTelXEmpleado?codEmpleado=" + idEmp.trim();

        const resp = await fetch(url, {
            method: "GET",
            mode: "cors",
            headers: {
                "content-type": "application/json"
            }
        });

        const datosin = await resp.json();

        if (!Array.isArray(datosin) || datosin.length === 0) {
            alerta("No se encontraron teléfonos para el empleado seleccionado", "Informacion del proceso");
            await llenarTabla();
            return;
        }

        const primerItem = datosin[0];
        if (typeof primerItem === "string" || typeof primerItem === "number") {
            const coincidencia = telActual && telActual.trim() !== ""
                ? (datosin.find(t => String(t).includes(telActual.trim())) ?? primerItem)
                : primerItem;

            $("#cboEmpleado").val(String(idEmp));
            $("#txtNumeroTel").val(String(coincidencia));
        } else {
            const reg = telActual && telActual.trim() !== ""
                ? (datosin.find(x => String(x.numero ?? x.telefono ?? x.numeroTelefono ?? "").includes(telActual.trim())) || primerItem)
                : primerItem;

            $("#txtCodigo").val(reg.idTel ?? reg.idTelefonoEmpleado ?? reg.codigo ?? "");
            $("#cboEmpleado").val(String(reg.empleado ?? reg.idEmpleado ?? reg.empleadoIdEmpleado ?? idEmp));
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
    let idEmp = $("#cboEmpleado").val();
    let idTipo = $("#cboTipoTel").val();
    let telefono = $("#txtNumeroTel").val();

    let comp = ""
    if (accion === "POST") {
        codi = "0";
        $("#txtCodigo").val("0");
    }

    if (!validar(telefono, idEmp)) {
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

    const datosOut = {
        idTelefonoEmpleado: parseInt(codi, 10) || 0,
        idTipoTelefono: idTipo && !isNaN(parseInt(idTipo, 10)) ? parseInt(idTipo, 10) : 0,
        empleadoIdEmpleado: parseInt(idEmp, 10) || 0,
        numero: telefono.trim()
    };

    const urlServicio = accion === "POST"
        ? dir + "empleado/agregarTelefono"
        : dir + "empleado/modificarTelefono";

    try {
        const response = await fetch(urlServicio, {
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
    let idEmp = $("#cboEmpleado").val();
    let filasFormateadas = [];

    if (idEmp && !isNaN(parseInt(idEmp, 10)) && parseInt(idEmp, 10) > 0) {
        const url = dir + "empleado/listadoTelXEmpleado?codEmpleado=" + encodeURIComponent(idEmp.trim());

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

            const nombreEmpActual = $("#cboEmpleado option:selected").text().trim() || idEmp;
            const idTipoActual = $("#cboTipoTel").val() ?? "";
            const nombreTipoActual = $("#cboTipoTel option:selected").text().trim() || "";

            filasFormateadas = Array.isArray(datos)
                ? datos.map((item, idx) => {
                    if (typeof item === "string" || typeof item === "number") {
                        return {
                            codigo: idx + 1,
                            idEmpleado: idEmp,
                            empleado: nombreEmpActual,
                            idTipoTelefono: idTipoActual,
                            tipoTelefono: nombreTipoActual,
                            telefono: String(item)
                        };
                    }

                    const idEmpleadoItem = item.empleado ?? item.idEmpleado ?? item.empleadoIdEmpleado ?? idEmp;
                    const opEmp = $(`#cboEmpleado option[value="${idEmpleadoItem}"]`);

                    return {
                        codigo: item.idTel ?? item.idTelefonoEmpleado ?? item.codigo ?? (idx + 1),
                        idEmpleado: idEmpleadoItem,
                        empleado: opEmp.length > 0 ? opEmp.text().trim() : (item.nombreEmpleado ?? idEmpleadoItem),
                        idTipoTelefono: item.idTipoTel ?? item.idTipoTelefono ?? "",
                        tipoTelefono: item.tipoTel ?? item.tipoTelefono ?? "",
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
            { data: "empleado", title: "Empleado" },
            { data: "tipoTelefono", title: "Tipo Teléfono" },
            { data: "telefono", title: "Teléfono" }
        ]
    });

    registrarEventoIrTelefonoEmp();
}