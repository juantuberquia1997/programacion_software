var dir = "http://localhost:5124/api/";

jQuery(function () {
    //Carga el menú
    $("#dvMenu").load("../Paginas/Menu.html");

    //Botones de opciones
    $("#btnAgre").on("click", function () {
        agregarIngreso();
    });
    $("#btnModi").on("click", function () {
        alerta("Modificar", "En futura construcción");
    });
    $("#btnBusc").on("click", function () {
        alerta("Buscar", "En futura construcción");
    });
    $("#btnCanc").on("click", function () {
        limpiarFormulario();
    });
    $("#btnImpr").on("click", function () {
        alerta("Impresión", "En futura construcción");
    });

    //Botón para agregar una línea al detalle
    $("#btnAgregarLinea").on("click", function () {
        agregarLineaDetalle();
    });

    //Botón para quitar una línea del detalle
    $("#tbodyDetalleIngreso").on("click", ".btn-quitar", function () {
        $(this).closest("tr").remove();
        renumerarLineas();
        actualizarTotal();
    });

    //Al cambiar de cliente se cargan sus direcciones
    $("#cboCliente").on("change", function () {
        llenarComboDirecciones();
    });

    //Al cambiar la prenda/servicio se actualiza el valor unitario y el descuento
    $("#cboTipoPrendaServicio").on("change", function () {
        calcularValorLinea();
    });

    //Al cambiar la cantidad se recalcula descuento y valor
    $("#txtCantidad").on("change input", function () {
        calcularValorLinea();
    });

    inicializarIngreso();
});

async function inicializarIngreso() {
    $("#txtFechaIngreso").val(new Date().toISOString().split('T')[0]);
    await llenarComboClientes();
    await llenarComboEmpleados();
    await llenarComboTipoPrendaServicio();
    await llenarComboEstadoPrenda();
    await llenarComboDirecciones();
}

function alerta(msj, titulo) {
    Swal.fire({
        position: 'center',
        type: 'info',
        title: titulo,
        text: msj
    });
}

function limpiarFormulario() {
    $("#txtCodigo").val("0");
    $("#cboCliente").val("");
    $("#cboEmpleado").val("");
    $("#cboDireccionCliente").empty();
    $("#cboTipoPrendaServicio").val("");
    $("#cboEstadoPrenda").val("");
    $("#txtCantidad").val("1");
    $("#txtDescuento").val("");
    $("#txtValorLinea").val("");
    $("#txtObservacion").val("");
    $("#chkDomicilio").prop("checked", false);
    $("#chkActivo").prop("checked", true);
    $("#txtValorTotal").val("0.00");
    $("#tbodyDetalleIngreso").empty();
    $("#txtFechaIngreso").val(new Date().toISOString().split('T')[0]);
}

async function llenarComboClientes() {
    let url = dir + "cliente/listadoClientes";
    let rpta = await llenarComboGral(url, "#cboCliente", "idCliente", "nombreApellidoCliente");
    if (rpta != "Termino") {
        alerta("Error llenando combo de cliente!", "Error en procedimiento");
    }
}

async function llenarComboEmpleados() {
    let url = dir + "empleado/listadoEmpleados";
    let rpta = await llenarComboGral(url, "#cboEmpleado", "idEmpleado", "nombreApellidoEmp");
    if (rpta != "Termino") {
        alerta("Error llenando combo de empleado!", "Error en procedimiento");
    }
}

async function llenarComboEstadoPrenda() {
    let url = dir + "estadoPrenda/listadoEstadoPrendas";
    let rpta = await llenarComboGral(url, "#cboEstadoPrenda", "idEstadoPrenda", "estadoPrenda");
    if (rpta != "Termino") {
        alerta("Error llenando combo de estado de prenda!", "Error en procedimiento");
    }
}

async function llenarComboTipoPrendaServicio() {
    let url = dir + "tipoPrendaServicio/comboTipoPrendaServicios";
    let rpta = await llenarComboGral(url, "#cboTipoPrendaServicio", "codigo", "nombre");
    if (rpta != "Termino") {
        alerta("Error llenando combo de prenda/servicio!", "Error en procedimiento");
    }
}

async function llenarComboDirecciones(idDir) {
    let clien = $("#cboCliente").val();
    if (!clien || clien === "null") {
        $("#cboDireccionCliente").empty();
        return;
    }
    let url = dir + "cliente/listadoDirXCliente?codCliente=" + clien;
    let rpta = await llenarComboGral(url, "#cboDireccionCliente", "idDir", "direccion");
    if (rpta != "Termino") {
        alerta("Error llenando combo de dirección!", "Error en procedimiento");
        return;
    } else if (idDir != undefined) {
        $("#cboDireccionCliente").val(idDir);
    }
}

// Consulta el valor unitario y el descuento de la prenda/servicio seleccionada
async function calcularValorLinea() {
    let idTPS = $("#cboTipoPrendaServicio").val();
    if (!idTPS || idTPS === "null") {
        $("#txtValorLinea").val("");
        $("#txtTotalLinea").val("");
        $("#txtDescuento").val("");
        return;
    }

    try {
        let url = dir + "tipoPrendaServicio/comboTipoPrendaServicios";
        const respuesta = await fetch(url, { method: "GET", mode: "cors" });
        const datos = await respuesta.json();
        const item = (Array.isArray(datos) ? datos : []).find(x => String(x.codigo) === String(idTPS));
        if (!item) {
            $("#txtValorLinea").val("");
            $("#txtTotalLinea").val("");
            return;
        }

        let cant = parseInt($("#txtCantidad").val(), 10) || 1;
        let valorUnitario = parseFloat(item.valorUnitario) || 0;
        let subtotal = valorUnitario * cant;

        // Descuento por tipo de prenda y cantidad
        let urlDesc = dir + "descuento/consultaDescuentoXPrenda?idPrenda=" + item.idPrenda;
        const rDesc = await fetch(urlDesc, { method: "GET", mode: "cors" });
        const descs = await rDesc.json();

        let porcentaje = 0;
        if (Array.isArray(descs)) {
            for (const d of descs) {
                if (cant >= d.cantMin && cant <= d.cantMax) {
                    porcentaje = parseFloat(d.porcen) || 0;
                    break;
                }
            }
        }

        let total = subtotal;
        if (porcentaje > 0) {
            total = subtotal * (1 - (porcentaje / 100));
            $("#txtDescuento").val(porcentaje + "%");
        } else {
            $("#txtDescuento").val("0%");
        }

        $("#txtValorLinea").val(subtotal.toFixed(2));
        $("#txtTotalLinea").val(total.toFixed(2));
    } catch (error) {
        console.error("Error calculando valor:", error);
    }
}

function agregarLineaDetalle() {
    let idTPS = $("#cboTipoPrendaServicio").val();
    let idEstado = $("#cboEstadoPrenda").val();
    let cant = $("#txtCantidad").val();
    let subtotal = $("#txtValorLinea").val();
    let total = $("#txtTotalLinea").val();
    let obs = $("#txtObservacion").val();

    if (!idTPS || idTPS === "null") {
        alerta("Seleccione una prenda/servicio", "Error en procedimiento");
        return;
    }
    if (!idEstado || idEstado === "null") {
        alerta("Seleccione un estado de prenda", "Error en procedimiento");
        return;
    }
    if (!cant || parseInt(cant, 10) < 1) {
        alerta("Cantidad no válida", "Error en procedimiento");
        return;
    }

    const nomTPS = $("#cboTipoPrendaServicio option:selected").text();
    const nomEstado = $("#cboEstadoPrenda option:selected").text();

    // ID consecutivo local de la línea (aún no guardada en BD)
    const numLinea = $("#tbodyDetalleIngreso tr").length + 1;
    const domicilio = $("#chkDomicilio").prop("checked");

    let fila = `
        <tr>
            <td>${numLinea}</td>
            <td>${nomTPS}</td>
            <td>${nomEstado}</td>
            <td>${$("#txtDescuento").val()}</td>
            <td>${parseInt(cant, 10)}</td>
            <td>${parseFloat(subtotal || 0).toFixed(2)}</td>
            <td>${parseFloat(total || 0).toFixed(2)}</td>
            <td>${obs}</td>
            <td>${domicilio ? "true" : "false"}</td>
            <td><button type="button" class="btn btn-danger btn-sm btn-quitar">Quitar</button></td>
        </tr>`;
    $("#tbodyDetalleIngreso").append(fila);

    // Datos ocultos en la fila para armar el JSON al grabar
    $("#tbodyDetalleIngreso tr:last").data("idTPS", idTPS);
    $("#tbodyDetalleIngreso tr:last").data("idEstado", idEstado);
    $("#tbodyDetalleIngreso tr:last").data("cantidad", parseInt(cant, 10));

    actualizarTotal();

    // Limpiar la línea capturada
    $("#txtCantidad").val("1");
    $("#txtObservacion").val("");
    $("#txtValorLinea").val("");
    $("#txtTotalLinea").val("");
    $("#txtDescuento").val("");
}

function actualizarTotal() {
    let total = 0;
    $("#tbodyDetalleIngreso tr").each(function () {
        const valor = parseFloat($(this).find("td:eq(6)").text()) || 0;
        total += valor;
    });
    $("#txtValorTotal").val(total.toFixed(2));
}

// Renumera la columna ID Detalle después de quitar líneas
function renumerarLineas() {
    $("#tbodyDetalleIngreso tr").each(function (index) {
        $(this).find("td:eq(0)").text(index + 1);
    });
}

async function agregarIngreso() {
    let clien = $("#cboCliente").val();
    let dir = $("#cboDireccionCliente").val();
    let emple = $("#cboEmpleado").val();

    if (!clien || clien === "null") {
        alerta("Seleccione un cliente", "Error en procedimiento");
        return;
    }
    if (!dir || dir === "null") {
        alerta("Seleccione una dirección", "Error en procedimiento");
        return;
    }
    if (!emple || emple === "null") {
        alerta("Seleccione un empleado", "Error en procedimiento");
        return;
    }

    const detalles = [];
    $("#tbodyDetalleIngreso tr").each(function () {
        detalles.push({
            IdTipoPrendaServicio: parseInt($(this).data("idTPS"), 10),
            IdEstadoPrenda: parseInt($(this).data("idEstado"), 10),
            Cantidad: parseInt($(this).data("cantidad"), 10),
            Observacion: $(this).find("td:eq(7)").text().trim()
        });
    });

    if (detalles.length === 0) {
        alerta("Debe agregar al menos una prenda/servicio al detalle", "Error en procedimiento");
        return;
    }

    const datosOut = {
        IdCliente: parseInt(clien, 10),
        IdDireccion: parseInt(dir, 10),
        EmpleadoIdEmpleado: parseInt(emple, 10),
        Fecha: $("#txtFechaIngreso").val(),
        Domicilio: $("#chkDomicilio").prop("checked"),
        Activo: $("#chkActivo").prop("checked"),
        Detalles: detalles
    };

    const confirma = window.confirm("¿Está seguro de grabar el ingreso?");
    if (!confirma) {
        alerta("Cancelada acción de grabar", "Información");
        return;
    }

    try {
        const response = await fetch(dir + "ingreso/agregarIngreso", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(datosOut)
        });
        const rpta = await response.json();
        if (rpta == -1) {
            alerta("Error general al grabar el ingreso", "Error en proceso");
        } else if (rpta == 0) {
            alerta("Error al grabar el detalle (se deshizo el encabezado)", "Error en grabar");
        } else if (rpta > 0) {
            alerta("Ingreso " + rpta + " grabado con éxito", "Proceso con éxito");
            $("#txtCodigo").val(rpta);
            limpiarFormulario();
        } else {
            alerta(rpta, "Error en procedimiento");
        }
    } catch (error) {
        alerta("Error: " + error, "Error en procedimiento");
    }
}
