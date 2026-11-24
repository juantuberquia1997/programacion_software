var dir = "http://localhost:5124/api/";

jQuery(function () {
    //Carga el menú
    $("#dvMenu").load("../Paginas/Menu.html");

    //Botones de opciones
    $("#btnAgre").on("click", function () {
        agregarIngreso();
    });
    $("#btnBusc").on("click", function () {
        consultarIngreso();
    });
    $("#btnCanc").on("click", function () {
        limpiarFormulario();
    });
    $("#btnImpr").on("click", function () {
        let nroIngreso = $("#txtCodigo").val();
        if (nroIngreso == undefined || nroIngreso.trim() == "" || parseInt(nroIngreso, 10) <= 0) {
            alerta("Número de ingreso no válido", "Error");
            $("#txtCodigo").trigger("focus");
            return;
        }
        cargarDatosImp(nroIngreso);
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
    let idDireccion = $("#cboDireccionCliente").val();
    let emple = $("#cboEmpleado").val();

    if (!clien || clien === "null") {
        alerta("Seleccione un cliente", "Error en procedimiento");
        return;
    }
    if (!idDireccion || idDireccion === "null") {
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
        IdDireccion: parseInt(idDireccion, 10),
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
            consultarIngreso();
        } else {
            alerta(rpta, "Error en procedimiento");
        }
    } catch (error) {
        alerta("Error: " + error, "Error en procedimiento");
    }
}

// Consultar un ingreso existente y recargar el formulario (encabezado + detalle)
async function consultarIngreso() {
    let codigoIngreso = $("#txtCodigo").val();
    if (codigoIngreso == undefined || codigoIngreso.trim() == "" || parseInt(codigoIngreso, 10) <= 0) {
        alerta("Ingrese un número de ingreso válido", "Error");
        $("#txtCodigo").trigger("focus");
        return;
    }

    try {
        // 1. Encabezado del ingreso
        let urlEncabezado = dir + "ingreso/buscarIngreso?codIngreso=" + parseInt(codigoIngreso, 10);
        const respuestaEncabezado = await fetch(urlEncabezado, { method: "GET", mode: "cors" });
        const datosEncabezado = await respuestaEncabezado.json();

        if (datosEncabezado == "") {
            alerta("No se encontró el ingreso #" + codigoIngreso, "Información");
            return;
        }

        // 2. Detalle del ingreso
        let urlDetalle = dir + "ingreso/consultarDetalleIngreso?codIngreso=" + parseInt(codigoIngreso, 10);
        const respuestaDetalle = await fetch(urlDetalle, { method: "GET", mode: "cors" });
        const lineasDetalle = await respuestaDetalle.json();

        // Rellenar encabezado
        $("#txtCodigo").val(datosEncabezado[0].idIngreso);
        $("#cboCliente").val(datosEncabezado[0].idCliente);
        await llenarComboDirecciones(datosEncabezado[0].idDir);   // carga direcciones del cliente y selecciona
        $("#cboEmpleado").val(datosEncabezado[0].idEmp);
        $("#txtFechaIngreso").val((datosEncabezado[0].fecha ?? "").toString().split('T')[0]);
        $("#chkDomicilio").prop("checked", datosEncabezado[0].domicilio === true);
        $("#chkActivo").prop("checked", datosEncabezado[0].activo === true);

        // Rellenar la tabla de detalle
        $("#tbodyDetalleIngreso").empty();
        lineasDetalle.forEach((linea, indice) => {
            let fila = `
                <tr>
                    <td>${linea.idDet ?? (indice + 1)}</td>
                    <td>${linea.servicio} - ${linea.prenda}</td>
                    <td>${linea.estado}</td>
                    <td></td>
                    <td>${linea.cantidad}</td>
                    <td></td>
                    <td>${parseFloat(linea.valor || 0).toFixed(2)}</td>
                    <td>${linea.observacion ?? ""}</td>
                    <td>${datosEncabezado[0].domicilio === true ? "true" : "false"}</td>
                    <td></td>
                </tr>`;
            $("#tbodyDetalleIngreso").append(fila);
        });

        // Total = suma de los valores del detalle
        let valorTotal = 0;
        lineasDetalle.forEach(linea => { valorTotal += parseFloat(linea.valor) || 0; });
        $("#txtValorTotal").val(valorTotal.toFixed(2));
    } catch (error) {
        console.error("Error consultando ingreso:", error);
        alerta("Error al consultar el ingreso", "Error en procedimiento");
    }
}

// ====== Impresión (jsPDF) ======
var detMaeImp = [];   // Arreglo con los datos del detalle para el PDF
var encMaeImp = null; // Datos del encabezado para el PDF

function fechaHoy_Hora() {
    let f = new Date();
    let y = f.getFullYear();
    let m = f.getMonth() + 1;
    let d = f.getDate();
    let h = f.getHours();
    let mi = f.getMinutes();
    if (d < 10) { d = '0' + d; }
    if (m < 10) { m = '0' + m; }
    if (h < 10) { h = '0' + h; }
    if (mi < 10) { mi = '0' + mi; }
    return "" + d + "/" + m + "/" + y + " " + h + ":" + mi;
}

async function cargarDatosImp(cod) {
    try {
        // 1. Encabezado del ingreso
        let urlEnc = dir + "ingreso/buscarIngreso?codIngreso=" + cod;
        const respEnc = await fetch(urlEnc);
        const datosEnc = await respEnc.json();
        encMaeImp = Array.isArray(datosEnc) ? datosEnc[0] : datosEnc;

        // 2. Detalle del ingreso
        let urlDet = dir + "ingreso/consultarDetalleIngreso?codIngreso=" + cod;
        const respDet = await fetch(urlDet);
        const datosDet = await respDet.json();
        detMaeImp = Array.isArray(datosDet) ? datosDet : [];

        if (!encMaeImp) {
            alerta("Ingreso # " + cod + " no encontrado");
            return;
        }
        if (detMaeImp.length === 0) {
            alerta("Ingreso # " + cod + " sin detalle");
            return;
        }
        imprimir();
    } catch (error) {
        alerta("Error al cargar los datos:", " - " + error);
    }
}

function imprimir() {
    var f = new Date();
    let fecha = fechaHoy_Hora();
    let nombreDoc = 'Ingreso_' + f.getDate() + (f.getMonth() + 1) + f.getFullYear() + "_" + f.getHours() + "_" + f.getMinutes() + '.pdf';

    var doc = new jsPDF('p', 'pt', 'letter');
    var docWidth = doc.internal.pageSize.width;

    // Encabezado
    doc.setFontStyle('bold');
    doc.setFontSize(16);
    doc.text("Lavandería Los Trajes", 145, 45);
    doc.setFontSize(12);
    doc.text("Medellín", 280, 65);
    doc.text("Registro de Ingreso", 230, 80);
    doc.setFontSize(10);
    doc.text("Fecha Impresión: " + fecha, 400, 80);
    doc.setFontSize(12);

    // Datos del encabezado
    let cliente = encMaeImp.cliente ?? "";
    let empleado = encMaeImp.empleado ?? "";
    let nroIng = encMaeImp.idIngreso ?? $("#txtCodigo").val();
    let fecIng = (encMaeImp.fecha ?? "").toString().split('T')[0];
    let domicilio = encMaeImp.domicilio === true ? "Sí" : "No";

    // Total = suma de los valores del detalle
    let total = 0;
    detMaeImp.forEach(d => { total += parseFloat(d.valor) || 0; });

    doc.setFontSize(11);
    doc.setFont("helvetica", "normal");
    let y = 110;

    doc.text("Cliente: " + cliente, 40, y);
    doc.text("Nro. Ingreso: " + nroIng, 240, y);
    doc.text("Fecha Ingreso: " + fecIng, 340, y);
    doc.text("Domicilio: " + domicilio, 470, y);

    y += 20;
    doc.text("Empleado: " + empleado, 40, y);
    doc.text("Valor Total: " + total.toFixed(2), 240, y);

    y += 20;
    doc.line(40, y, docWidth - 40, y);
    y += 20;

    // Título detalle
    doc.setFontSize(14);
    doc.setFont("helvetica", "bold");
    doc.text("Detalle del Ingreso", 250, y);
    y += 20;

    // Encabezados de la tabla
    doc.setFontSize(10);
    const encabezados = ["Servicio", "Prenda", "Estado", "Cantidad", "Valor"];
    let xPos = [50, 180, 300, 420, 500];
    for (let i = 0; i < encabezados.length; i++) {
        doc.text(encabezados[i], xPos[i], y);
    }

    y += 10;
    doc.line(40, y, docWidth - 40, y);
    y += 15;

    // Filas del detalle
    doc.setFont("helvetica", "normal");
    detMaeImp.forEach(dMat => {
        if (y > 750) {
            doc.addPage();
            y = 40;
        }
        doc.text((dMat.servicio ?? "").toString(), xPos[0], y);
        doc.text((dMat.prenda ?? "").toString(), xPos[1], y);
        doc.text((dMat.estado ?? "").toString(), xPos[2], y);
        doc.text((dMat.cantidad ?? 0).toString(), xPos[3], y);
        doc.text(parseFloat(dMat.valor || 0).toFixed(2), xPos[4], y);
        y += 15;
    });

    doc.save(nombreDoc);
}
